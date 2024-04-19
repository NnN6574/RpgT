using System;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Save
{
    [Serializable]
    public class LCGroupSaveData
    {
        [field: SerializeField] public Guid ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}