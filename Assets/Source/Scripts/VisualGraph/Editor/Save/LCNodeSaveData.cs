using System;
using System.Collections.Generic;
using LevelsConstructor.New.Enumerations;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Save
{
    [Serializable]
    public class LCNodeSaveData
    {
        [field: SerializeField] public Guid ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<LCChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public Guid GroupID { get; set; }
        [field: SerializeField] public LCChoiceType ChoiceType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}