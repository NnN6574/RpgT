using System.Collections.Generic;
using DS.Data;
using DS.Enumerations;
using LevelsConstructor.New.Enumerations;
using LevelsConstructor.New.Node;
using UnityEngine;

namespace LevelsConstructor.New.Data
{
    public class LCPlatformSO: ScriptableObject
    {
        [field: SerializeField] public string PlatformName { get; set; }
        [field: SerializeField] public List<LCPlatformChoiceData> Choices { get; set; }
        [field: SerializeField] public LCChoiceType ChoiceType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, List<LCPlatformChoiceData> nodeData, LCChoiceType choiceType, bool isStartingDialogue)
        {
            PlatformName = dialogueName;
            Choices = nodeData;
            ChoiceType = choiceType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}