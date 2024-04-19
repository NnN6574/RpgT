using System.Collections.Generic;
using UnityEngine;

namespace Playstrom.Core.CallEvent
{
    public class CallEventSetting : ScriptableObject
    {
        public static string version = "1.0.5";
        public static string PathSetting = "Assets/CallEvent/Data/";
        public static string PathEnumCommand = "Assets/CallEvent/Scripts/";
        public static string PathDebugConfigs = $"Assets/CallEvent/Resources/{PathCallEventLoadConfigs}";
        public static string PathCallEventLoadConfigs => "CallEventConfigs/";

        public const string CONST_DEBUG_SETTINGS = "CallEventSetting";
        public const string CONST_NAME_PREFERENCES = "CallEvent";
        
        public static List<CallEventConfig> GetLoadResources(string path)
        {
            List<CallEventConfig> temp = new List<CallEventConfig>();
            var resources = Resources.LoadAll(path);
            foreach (var element in resources)
            {
                temp.Add((CallEventConfig) element);
            }
            
            return temp;
        }

        public static bool IsHave(string codeCommand, string path)
        {
            List<CallEventConfig> loadConfigs = GetLoadResources(path);

            foreach (var command in loadConfigs)
            {
                if (string.Equals(command.CodeName, codeCommand))
                {
#if UNITY_EDITOR
                    Debug.LogError($"Name code {codeCommand} is have!!!");
#endif
                    return true;
                }
            }

            return false;
        }
    }
}
