using System;
using LevelsConstructor.New.Data;
using UnityEngine;

namespace LevelsConstructor.New.Node
{
    [Serializable]
    public class LCPlatformChoiceData
    {
        [field: SerializeField] public GameObject Platform { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public LCPlatformSO NextPlatform { get; set; }
    }
}