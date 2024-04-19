using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{ 
    public class InfoDataGraphPreferenceSO : ScriptableObject
    {
        [SerializeField] private NodeDescription[] _nodes;
        public string NameGraph =>name;
        public NodeDescription[] Nodes => _nodes;
        
        private void OnValidate()
        {
            foreach (NodeDescription node in _nodes) node.Validate();
        }
    }
}