using System.Collections.Generic;
using LevelsConstructor.New.Utilities;
using UnityEngine;

namespace LevelsConstructor.New.Data
{
    public class LCPlatformContainerSO: ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<LCPlatformGroupSO, List<LCPlatformSO>> PlatformGroups { get; set; }
        [field: SerializeField] public List<LCPlatformSO> UngroupedPlatforms { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            PlatformGroups = new SerializableDictionary<LCPlatformGroupSO, List<LCPlatformSO>>();
            UngroupedPlatforms = new List<LCPlatformSO>();
        }

        public List<string> GetPlatformGroupNames()
        {
            List<string> platformGroupNames = new List<string>();

            foreach (LCPlatformGroupSO platformGroup in PlatformGroups.Keys)
            {
                platformGroupNames.Add(platformGroup.GroupName);
            }

            return platformGroupNames;
        }

        public List<string> GetGroupedPlatformNames(LCPlatformGroupSO platformGroup, bool startingPlatformsOnly)
        {
            List<LCPlatformSO> groupedPlatforms = PlatformGroups[platformGroup];

            List<string> groupedPlatformNames = new List<string>();

            foreach (LCPlatformSO groupedPlatform in groupedPlatforms)
            {
                if (startingPlatformsOnly && !groupedPlatform.IsStartingDialogue)
                {
                    continue;
                }

                groupedPlatformNames.Add(groupedPlatform.PlatformName);
            }

            return groupedPlatformNames;
        }

        public List<string> GetUngroupedPlatformNames(bool startingPlatformOnly)
        {
            List<string> ungroupedPlatformNames = new List<string>();

            foreach (LCPlatformSO ungroupedPlatform in UngroupedPlatforms)
            {
                if (startingPlatformOnly && !ungroupedPlatform.IsStartingDialogue)
                {
                    continue;
                }

                ungroupedPlatformNames.Add(ungroupedPlatform.PlatformName);
            }

            return ungroupedPlatformNames;
        }
    }
}