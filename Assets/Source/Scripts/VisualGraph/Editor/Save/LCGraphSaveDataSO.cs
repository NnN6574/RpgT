using System.Collections.Generic;
using LevelsConstructor.New.Utilities;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Save
{
    public class LCGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<LCGroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<LCNodeSaveData> Choices { get; set; }
        [field: SerializeField] public List<string> OldGroupNames { get; set; }
        [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<LCGroupSaveData>();
            Choices = new List<LCNodeSaveData>();
        }
    }
}