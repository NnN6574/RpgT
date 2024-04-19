using System;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Save
{
    [Serializable]
    public class LCChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public Guid NodeID { get; set; }
    }
}