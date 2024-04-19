using UnityEngine;

namespace LevelsConstructor.New.Data
{
    public class LCPlatformGroupSO: ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}