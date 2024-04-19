using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playstrom.Core.CallEvent
{
    public class CallEventInformer : MonoBehaviour
    {
        public List<CallEventConfig> Configs = new List<CallEventConfig>();

        public virtual void Init()
        {
            Configs = CallEventSetting.GetLoadResources(CallEventSetting.PathCallEventLoadConfigs);
            DataReset();
        }

        public void DataReset()
        {
#if UNITY_EDITOR
            foreach (var config in Configs)
            {
                config.DataReset();
            }
#endif
        }

        public CallEventConfig GetConfig(string codeName)
        {
            foreach (var config in Configs)
            {
                if (config.CodeName == codeName)
                {
                    return config;
                }
            }

            return null;
        }
    }
}