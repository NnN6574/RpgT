using System;
using System.Collections.Generic;
using System.Linq;
using LevelsConstructor.New.Editor.Elements;
using LevelsConstructor.New.Editor.Preference;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Windows
{
    public class LcSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private LcGraphView _graphView;

        public void Initialize(LcGraphView graphView) => _graphView = graphView;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Nodes")));
            NodeDescription[] nodes = _graphView.GraphData.Nodes;
            List<NodeDescription> sortedNodes = nodes.ToList();
            sortedNodes.Sort((a, b) =>
            {
                string[] splitsA = a.SearchMenuPath.Split('/');
                string[] splitsB = b.SearchMenuPath.Split('/');
                for (int i = 0; i < splitsA.Length; i++)
                {
                    if (i > splitsB.Length) return 1;
                    int value = string.Compare(splitsA[i], splitsB[i], StringComparison.Ordinal);
                    if (value == 0) continue;

                    bool isLast = i == splitsA.Length - 1 || i == splitsB.Length - 1;
                    if (splitsA.Length != splitsB.Length && isLast) return splitsA.Length < splitsB.Length ? 1 : -1;
                    return value;
                }

                return 0;
            });

            List<string> groups = new();
            foreach (NodeDescription node in sortedNodes)
            {
                string[] entryTitle = node.SearchMenuPath.Split('/');
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        SearchTreeGroupEntry group = new(new GUIContent(entryTitle[i]), i + 1);
                        entries.Add(group);
                        groups.Add(groupName);
                    }

                    groupName += '/';
                }

                SearchTreeEntry entry = new(new GUIContent(entryTitle[^1]))
                {
                    level = entryTitle.Length,
                    userData = node
                };
                entries.Add(entry);
            }
            
            entries.Add(new SearchTreeEntry(new GUIContent("Group")) {level = 1, userData = new Group()});

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            Vector2 localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (entry.userData)
            {
                case NodeDescription nodeDescription:
                {
                    _graphView.AddElement(_graphView.CreateNode(nodeDescription, localMousePosition));
                    return true;
                }
                case Group:
                {
                    _graphView.CreateGroup("Group", localMousePosition);
                    return true;
                }

                default: return false;
            }
        }
    }
}