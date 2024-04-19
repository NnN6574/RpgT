using System;
using System.Collections.Generic;
using System.Linq;
using LevelsConstructor.New.Data;
using LevelsConstructor.New.Editor.Elements;
using LevelsConstructor.New.Editor.Save;
using LevelsConstructor.New.Editor.Windows;
using LevelsConstructor.New.Node;
using LevelsConstructor.New.Utilities;
using Source.Scripts.VisualGraph.Editor.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelsConstructor.New.Editor.Utilities.Refactor.Graph
{
    public class GraphIOUtility : IOUtility
    {
        private static LcGraphView _graphView;

        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<LCNode> _nodes;
        private static List<LCGroup> _groups;

        private static Dictionary<Guid, LCPlatformGroupSO> _createdPlatformGroups;
        private static Dictionary<Guid, LCPlatformSO> _createdPlatforms;

        private static Dictionary<Guid, LCGroup> _loadedGroups;
        private static Dictionary<Guid, LCNode> _loadedNodes;

        public static void Initialize(LcGraphView dsGraphView, string graphName)
        {
            _graphView = dsGraphView;

            _graphFileName = graphName;
            _containerFolderPath = $"{PathConstants.PathConstructors}/{graphName}";
            Debug.Log(_containerFolderPath);

            _nodes = new List<LCNode>();
            _groups = new List<LCGroup>();

            _createdPlatformGroups = new Dictionary<Guid, LCPlatformGroupSO>();
            _createdPlatforms = new Dictionary<Guid, LCPlatformSO>();

            _loadedGroups = new Dictionary<Guid, LCGroup>();
            _loadedNodes = new Dictionary<Guid, LCNode>();
        }
        
        public static void Save()
        {
            CreateDefaultFolders();

            GetElementsFromGraphView();
            
            LCGraphSaveDataSO graphData = CreateAsset<LCGraphSaveDataSO>(_containerFolderPath, $"{_graphFileName}_ConstructorGraph");

            graphData.Initialize(_graphFileName);

            LCPlatformContainerSO platformContainer = CreateAsset<LCPlatformContainerSO>(_containerFolderPath + $"/{PathConstants.FolderGraphs}", _graphFileName);

            platformContainer.Initialize(_graphFileName);

            SaveGroups(graphData, platformContainer);
            SaveNodes(graphData, platformContainer);

            SaveAsset(graphData);
            SaveAsset(platformContainer);
        }

        private static void SaveGroups(LCGraphSaveDataSO graphData, LCPlatformContainerSO platformContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (LCGroup group in _groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToScriptableObject(group, platformContainer);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToGraph(LCGroup group, LCGraphSaveDataSO graphData)
        {
            LCGroupSaveData groupData = new LCGroupSaveData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };

            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToScriptableObject(LCGroup group, LCPlatformContainerSO platformContainer)
        {
            string groupName = group.title;

            CreateFolder($"{_containerFolderPath}/{PathConstants.FolderGroups}", groupName);
            CreateFolder($"{_containerFolderPath}/{PathConstants.FolderGroups}/{groupName}", PathConstants.FolderData);

            LCPlatformGroupSO platformGroup = CreateAsset<LCPlatformGroupSO>($"{_containerFolderPath}/{PathConstants.FolderGroups}/{groupName}", groupName);

            platformGroup.Initialize(groupName);

            _createdPlatformGroups.Add(group.ID, platformGroup);

            platformContainer.PlatformGroups.Add(platformGroup, new List<LCPlatformSO>());

            SaveAsset(platformGroup);
        }

        private static void UpdateOldGroups(List<string> currentGroupNames, LCGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{_containerFolderPath}/{PathConstants.FolderGroups}/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        private static void SaveNodes(LCGraphSaveDataSO graphData, LCPlatformContainerSO platformContainer)
        {
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
            List<string> ungroupedNodeNames = new List<string>();

            foreach (LCNode node in _nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node, platformContainer);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.NodeName);

                    continue;
                }

                ungroupedNodeNames.Add(node.NodeName);
            }

            UpdateDialoguesChoicesConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);
            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        private static void SaveNodeToGraph(LCNode node, LCGraphSaveDataSO graphData)
        {
            List<LCChoiceSaveData> choices = CloneNodeChoices(node.Choices);

            LCNodeSaveData nodeData = new()
            {
                ID = node.ID,
                Name = node.NodeName,
                Choices = choices,
                //Text = node.Text,
                GroupID = node.Group?.ID ?? Guid.Empty,
                ChoiceType = node.ChoiceType,
                Position = node.GetPosition().position
            };

            graphData.Choices.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(LCNode node, LCPlatformContainerSO platformContainer)
        {
            LCPlatformSO platform;

            if (node.Group != null)
            {
                platform = CreateAsset<LCPlatformSO>($"{_containerFolderPath}/{PathConstants.FolderGroups}/{node.Group.title}/{PathConstants.FolderData}", node.NodeName);

                platformContainer.PlatformGroups.AddItem(_createdPlatformGroups[node.Group.ID], platform);
            }
            else
            {
                platform = CreateAsset<LCPlatformSO>($"{_containerFolderPath}/{PathConstants.FolderGlobal}/{PathConstants.FolderData}", node.NodeName);

                platformContainer.UngroupedPlatforms.Add(platform);
            }

            platform.Initialize(
                node.NodeName,
                //node.Text,
                ConvertNodeChoicesToDialogueChoices(node.Choices),
                node.ChoiceType,
                node.IsStartingNode()
            );

            _createdPlatforms.Add(node.ID, platform);

            SaveAsset(platform);
        }

        private static List<LCPlatformChoiceData> ConvertNodeChoicesToDialogueChoices(List<LCChoiceSaveData> nodeChoices)
        {
            List<LCPlatformChoiceData> platformChoices = new List<LCPlatformChoiceData>();

            foreach (LCChoiceSaveData nodeChoice in nodeChoices)
            {
                LCPlatformChoiceData choiceData = new LCPlatformChoiceData()
                {
                    Text = nodeChoice.Text
                };

                platformChoices.Add(choiceData);
            }

            return platformChoices;
        }

        private static void UpdateDialoguesChoicesConnections()
        {
            foreach (LCNode node in _nodes)
            {
                LCPlatformSO dialogue = _createdPlatforms[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
                {
                    LCChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                    
                    if (nodeChoice.NodeID == Guid.Empty)
                    {
                        continue;
                    }

                    dialogue.Choices[choiceIndex].NextPlatform = _createdPlatforms[nodeChoice.NodeID];

                    SaveAsset(dialogue);
                }
            }
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, LCGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{_containerFolderPath}/{PathConstants.FolderGroups}/{oldGroupedNode.Key}/{PathConstants.FolderData}", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, LCGraphSaveDataSO graphData)
        {
            if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/{PathConstants.FolderGlobal}/{PathConstants.FolderData}", nodeToRemove);
                }
            }

            graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }

        public static void Load()
        {
            LCGraphSaveDataSO graphData = LoadAsset<LCGraphSaveDataSO>(_containerFolderPath);

            Debug.Log(_containerFolderPath + ".asset");
            
            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Could not find the file!",
                    "The file at the following path could not be found:\n\n" +
                    $"\"Assets/Editor/DialogueSystem/Graphs/{_graphFileName}\".\n\n" +
                    "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }

            LcEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Choices);
            LoadNodesConnections();
        }

        private static void LoadGroups(List<LCGroupSaveData> groups)
        {
            foreach (LCGroupSaveData groupData in groups)
            {
                LCGroup group = _graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;

                _loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<LCNodeSaveData> nodes)
        {
            foreach (LCNodeSaveData nodeData in nodes)
            {
                List<LCChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);

                // LCNode node = graphView.CreateNode(nodeData.Name, nodeData.ChoiceType, nodeData.Position, false);
                LCNode node = _graphView.CreateNode(null, Vector2.zero);

                node.ID = nodeData.ID;
                node.Choices = choices;
                //node.Text = nodeData.Text;

                node.Draw();

                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);

                if (nodeData.GroupID == Guid.Empty) continue;

                LCGroup group = _loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }

        private static void LoadNodesConnections()
        {
            foreach ((Guid _, LCNode node) in _loadedNodes)
            {
                foreach (VisualElement visualElement in node.outputContainer.Children())
                {
                    var choicePort = visualElement as Port;
                    if (choicePort == null) continue;
                    
                    LCChoiceSaveData choiceData = (LCChoiceSaveData) choicePort.userData;

                    if (choiceData.NodeID == Guid.Empty) continue;

                    LCNode nextNode = _loadedNodes[choiceData.NodeID];

                    Port nextNodeInputPort = (Port) nextNode.inputContainer.Children().First();

                    Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                    _graphView.AddElement(edge);

                    node.RefreshPorts();
                }
            }
        }
        
        private static void CreateDefaultFolders()
        {
            CreateFolder(PathConstants.PathConstructors, _graphFileName);
            CreateFolder(_containerFolderPath, PathConstants.FolderGlobal);
            CreateFolder(_containerFolderPath, PathConstants.FolderGraphs);
            CreateFolder($"{_containerFolderPath}/{PathConstants.FolderGlobal}", PathConstants.FolderData);
        }

        private static void GetElementsFromGraphView()
        {
            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is LCNode node) _nodes.Add(node);
                if (graphElement is LCGroup group) _groups.Add(group);
            });
        }

        private static List<LCChoiceSaveData> CloneNodeChoices(List<LCChoiceSaveData> nodeChoices)
        {
            List<LCChoiceSaveData> choices = new();

            foreach (LCChoiceSaveData choice in nodeChoices)
            {
                LCChoiceSaveData choiceData = new()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };

                choices.Add(choiceData);
            }

            return choices;
        }
    }
}