using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Playstrom.Core.CallEvent;
using UnityEditor;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
    public class CmdInformer : CallEventInformer
    {
        public override void Init()
        {
            Configs = CallEventSetting.GetLoadResources(CmdSetting.PathDebugLoadConfigs);
            DataReset();
        }

        public CmdConfig GetConfig(string codeName)
        {
            foreach (var config in Configs)
            {
                if (config.CodeName == codeName)
                {
                    return (CmdConfig)config;
                }
            }

            return null;
        }
    }
}
