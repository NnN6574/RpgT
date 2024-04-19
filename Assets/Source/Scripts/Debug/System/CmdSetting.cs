using System.Collections.Generic;
using Playstrom.Core.CallEvent;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
    public class CmdSetting : CallEventSetting
    {
        public static string version = "1.0.13";
        public static string PathSetting = "Assets/Debug/Data/";
        public static string PathEnumCommand = "Assets/Debug/Scripts/";
        public static string PathDebugConfigs = $"Assets/Debug/Resources/{PathDebugLoadConfigs}";
        public static string PathDebugLoadConfigs => "DebugConfigs/";

        public const string CONST_DEBUG_SETTINGS = "DebugSetting";
        public const string CONST_NAME_PREFERENCES = "Debug";
    }
}
