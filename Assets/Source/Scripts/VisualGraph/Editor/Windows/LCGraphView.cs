using System;
using System.Collections.Generic;
using System.Linq;
using LevelsConstructor.New.Editor.Elements;
using LevelsConstructor.New.Editor.Error;
using LevelsConstructor.New.Editor.Preference;
using LevelsConstructor.New.Editor.Save;
using LevelsConstructor.New.Editor.Utilities;
using LevelsConstructor.New.Utilities;
using Source.Scripts.VisualGraph.Editor.Elements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace LevelsConstructor.New.Editor.Windows
{
    public class LcGraphView : GraphView
    {
        public InfoDataGraphPreferenceSO GraphData { get; }

        private LcSearchWindow _searchWindow;
        private MiniMap _miniMap;

        private readonly SerializableDictionary<Guid, LCNodeErrorData> _ungroupedNodes;
        private readonly SerializableDictionary<Guid, LCGroupErrorData> _groups;
        private readonly SerializableDictionary<Group, SerializableDictionary<Guid, LCNodeErrorData>> _groupedNodes;
        private GridBackground _gridBackground;

        public LcGraphView(InfoDataGraphPreferenceSO graphData)
        {
            GraphData = graphData;
            
            _ungroupedNodes = new SerializableDictionary<Guid, LCNodeErrorData>();
            _groups = new SerializableDictionary<Guid, LCGroupErrorData>();
            _groupedNodes = new SerializableDictionary<Group, SerializableDictionary<Guid, LCNodeErrorData>>();

            AddManipulators();
            AddGridBackground();
            AddSearchWindow();
            AddMiniMap();

            deleteSelection = OnElementsDeleted;
            elementsAddedToGroup = OnGroupElementsAdded;
            elementsRemovedFromGroup = OnGroupElementsRemoved;
            groupTitleChanged = OnGroupRenamed;
            graphViewChanged = OnGraphViewChanged;

            AddStyles();
            AddMiniMapStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();

            foreach (Port port in ports)
            {
                if (startPort == port) continue;
                if (startPort.node == port.node) continue;
                if (startPort.direction == port.direction) continue;
                if (nodeAdapter.GetAdapter(port.source, startPort.source) == null) continue;

                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            foreach (NodeDescription node in GraphData.Nodes) this.AddManipulator(CreateNodeContextualMenu(node));

            this.AddManipulator(CreateGroupContextualMenu());
        }
        private void AddGridBackground()
        {
            _gridBackground = new GridBackground();
            _gridBackground.StretchToParentSize();
            Insert(0, _gridBackground);
        }
        private void AddSearchWindow()
        {
            if (_searchWindow == null) _searchWindow = ScriptableObject.CreateInstance<LcSearchWindow>();

            _searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        private void AddMiniMap()
        {
            _miniMap = new MiniMap
            {
                anchored = true,
                visible = false
            };

            _miniMap.SetPosition(new Rect(15, 50, 200, 180));

            Add(_miniMap);
        }
        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
            );
        }
        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new(new Color32(51, 51, 51, 255));

            _miniMap.style.backgroundColor = backgroundColor;
            _miniMap.style.borderTopColor = borderColor;
            _miniMap.style.borderRightColor = borderColor;
            _miniMap.style.borderBottomColor = borderColor;
            _miniMap.style.borderLeftColor = borderColor;
        }

        private IManipulator CreateNodeContextualMenu(NodeDescription nodeDescription)
        {
            return !string.IsNullOrWhiteSpace(nodeDescription.ContextMenuPath) 
                ? new ContextualMenuManipulator(MenuBuilder) 
                : null;

            void MenuBuilder(ContextualMenuPopulateEvent menuEvent) =>
                menuEvent.menu.AppendAction(nodeDescription.ContextMenuPath, actionEvent =>
                {
                    Vector2 position = actionEvent.eventInfo.localMousePosition;
                    LCNode node = CreateNode(nodeDescription, position);
                    AddElement(node);
                });
        }
        private IManipulator CreateGroupContextualMenu()
        {
            return new ContextualMenuManipulator(MenuBuilder);
            
            void MenuBuilder(ContextualMenuPopulateEvent menuEvent) =>
                menuEvent.menu.AppendAction("Add Group",
                    actionEvent =>
                    {
                        Vector2 position = actionEvent.eventInfo.localMousePosition;
                        CreateGroup("Group", position);
                    });
        }
        public LCGroup CreateGroup(string title, Vector2 position)
        {
            LCGroup group = new(title, position);
            AddGroup(group);
            AddElement(group);
            
            foreach (ISelectable selectable in selection)
            {
                if (selectable is not LCNode node) continue;
                group.AddElement(node);
            }
            return group;
        }
        public LCNode CreateNode(NodeDescription nodeDescription, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType(nodeDescription.TypeName);
            if (nodeType == null) return null;
            LCNode node = (LCNode) Activator.CreateInstance(nodeType);
            node.Initialize(nodeDescription.Name, this, position);
            if (shouldDraw) node.Draw();
            AddUngroupedNode(node);
            return node;
        }

        protected virtual void OnElementsDeleted(string operationName, AskUser askUser)
        {
            List<LCGroup> groupsToDelete = new();
            List<LCNode> nodesToDelete = new();
            List<Edge> edgesToDelete = new();

            foreach (ISelectable selectable in selection)
            {
                switch (selectable)
                {
                    case LCNode node:
                        nodesToDelete.Add(node);
                        break;
                    case Edge edge:
                        edgesToDelete.Add(edge);
                        break;
                    case LCGroup group:
                        groupsToDelete.Add(group);
                        break;
                }
            }

            foreach (LCGroup groupToDelete in groupsToDelete)
            {
                IEnumerable<LCNode> groupNodes = groupToDelete.containedElements
                    .Select(g => g as LCNode)
                    .Where(n => n != null); 
                
                groupToDelete.RemoveElements(groupNodes);

                RemoveGroup(groupToDelete);
                RemoveElement(groupToDelete);
            }

            DeleteElements(edgesToDelete);

            foreach (LCNode nodeToDelete in nodesToDelete)
            {
                nodeToDelete.Group?.RemoveElement(nodeToDelete);
                RemoveUngroupedNode(nodeToDelete);
                nodeToDelete.DisconnectAllPorts();
                RemoveElement(nodeToDelete);
            }
        }
        protected virtual void OnGroupElementsAdded(Group group, IEnumerable<GraphElement> elements)
        {
            LCGroup dsGroup = (LCGroup) group;
            foreach (GraphElement element in elements)
            {
                if (element is not LCNode node) continue;
                RemoveUngroupedNode(node);
                AddGroupedNode(node, dsGroup);
            }
        }
        protected virtual void OnGroupElementsRemoved(Group group, IEnumerable<GraphElement> elements)
        {
            LCGroup dsGroup = (LCGroup) group;
            foreach (GraphElement element in elements)
            {
                if (element is not LCNode node) continue;
                RemoveGroupedNode(node, dsGroup);
                AddUngroupedNode(node);
            }
        }
        protected virtual void OnGroupRenamed(Group group, string newTitle)
        {
            LCGroup dsGroup = (LCGroup) group;

            dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

            RemoveGroup(dsGroup);

            dsGroup.OldTitle = dsGroup.title;

            AddGroup(dsGroup);
        }
        protected virtual GraphViewChange OnGraphViewChanged(GraphViewChange changes)
        {
            if (changes.edgesToCreate != null)
                foreach (Edge edge in changes.edgesToCreate)
                {
                    LCNode nextNode = (LCNode) edge.input.node;
                    LCChoiceSaveData choiceData = (LCChoiceSaveData) edge.output.userData;
                    choiceData.NodeID = nextNode.ID;
                }

            if (changes.elementsToRemove != null)
                foreach (GraphElement element in changes.elementsToRemove)
                {
                    if (element is not Edge edge) continue;
                    LCChoiceSaveData choiceData = (LCChoiceSaveData) edge.output.userData;
                    choiceData.NodeID = Guid.Empty;
                }

            return changes;
        }

        public void AddUngroupedNode(LCNode node)
        {
            Guid nodeID = node.ID;

            if (!_ungroupedNodes.ContainsKey(nodeID))
            {
                LCNodeErrorData nodeErrorData = new();
                nodeErrorData.Nodes.Add(node);
                _ungroupedNodes.Add(nodeID, nodeErrorData);
                return;
            }

            List<LCNode> ungroupedNodesList = _ungroupedNodes[nodeID].Nodes;
            ungroupedNodesList.Add(node);
            Color errorColor = _ungroupedNodes[nodeID].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2) ungroupedNodesList[0].SetErrorStyle(errorColor);
        }
        public void RemoveUngroupedNode(LCNode node)
        {
            Guid nodeID = node.ID;
            List<LCNode> ungroupedNodesList = _ungroupedNodes[nodeID].Nodes;
            ungroupedNodesList.Remove(node);
            node.ResetStyle();

            if (ungroupedNodesList.Count == 1) ungroupedNodesList[0].ResetStyle();
            else if (ungroupedNodesList.Count == 0) _ungroupedNodes.Remove(nodeID);
        }
        private void AddGroup(LCGroup group)
        {
            Guid groupID = group.ID;

            if (!_groups.ContainsKey(groupID))
            {
                LCGroupErrorData groupErrorData = new();
                groupErrorData.Groups.Add(group);
                _groups.Add(groupID, groupErrorData);
                return;
            }

            LCGroupErrorData errorData = _groups[groupID];
            List<LCGroup> groupsList = errorData.Groups;
            groupsList.Add(group);
            Color errorColor = errorData.ErrorData.Color;
            group.SetErrorStyle(errorColor);
            if (groupsList.Count == 2)
            {
                groupsList[0].SetErrorStyle(errorColor);
            }
        }
        private void RemoveGroup(LCGroup group)
        {
            Guid groupID = group.ID;
            List<LCGroup> groupsList = _groups[groupID].Groups;
            groupsList.Remove(group);
            group.ResetStyle();

            if (groupsList.Count == 1) groupsList[0].ResetStyle();
            else if (groupsList.Count == 0) _groups.Remove(groupID);
        }
        public void AddGroupedNode(LCNode node, LCGroup group)
        {
            Guid nodeID = node.ID;
            node.Group = group;

            if (!_groupedNodes.ContainsKey(group)) _groupedNodes.Add(group, new SerializableDictionary<Guid, LCNodeErrorData>());

            if (!_groupedNodes[group].ContainsKey(nodeID))
            {
                LCNodeErrorData nodeErrorData = new();
                nodeErrorData.Nodes.Add(node);
                _groupedNodes[group].Add(nodeID, nodeErrorData);
                return;
            }

            List<LCNode> groupedNodesList = _groupedNodes[group][nodeID].Nodes;
            groupedNodesList.Add(node);
            Color errorColor = _groupedNodes[group][nodeID].ErrorData.Color;
            node.SetErrorStyle(errorColor);
            if (groupedNodesList.Count == 2)
            {
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }
        public void RemoveGroupedNode(LCNode node, LCGroup group)
        {
            Guid nodeID = node.ID;
            node.Group = null;
            List<LCNode> groupedNodesList = _groupedNodes[group][nodeID].Nodes;
            groupedNodesList.Remove(node);
            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                groupedNodesList[0].ResetStyle();
                return;
            }

            if (groupedNodesList.Count != 0) return;
            _groupedNodes[group].Remove(nodeID);
            
            if (_groupedNodes[group].Count == 0) _groupedNodes.Remove(group);
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            if (isSearchWindow) worldMousePosition = parent.ChangeCoordinatesTo(parent.parent, parent.WorldToLocal(mousePosition));
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        public void ClearGraph()
        {
            foreach (GraphElement graphElement in graphElements) RemoveElement(graphElement);

            _groups.Clear();
            _groupedNodes.Clear();
            _ungroupedNodes.Clear();
        }

        public void ToggleMiniMap() => _miniMap.visible = !_miniMap.visible;
    }
}