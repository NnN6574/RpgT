using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playstrom.Core.CallEvent
{
    public class CallEventMaintenance : MonoBehaviour
    {
        [SerializeField] private bool isDontDestroy = true;
        protected Dictionary<string, List<CallEvent>> dictionaryActions
            = new Dictionary<string, List<CallEvent>>();

        protected CallEventInformer CallEventInformer;

        public void MainConstruct()
        {
            Construct();
            CallEventInformer.Init();
            Events();
        }

        public void Construct()
        {
            CallEventInformer = GetComponent<CallEventInformer>();

            if (CallEventInformer == null)
            {
                CallEventInformer = gameObject.AddComponent<CallEventInformer>();
            }

            dictionaryActions.Clear();

            if(isDontDestroy)
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
                DontDestroyOnLoad(gameObject);
        }

        public virtual void Events()
        {
            CallEvent.OnInit += Init;
            CallEvent.OnExecute += Execute;
            CallEvent.OnDeInit += DeInit;
        }

        public virtual void DeEvents()
        {
            CallEvent.OnInit -= Init;
            CallEvent.OnExecute -= Execute;
            CallEvent.OnDeInit -= DeInit;
        }

        protected void Init(string codeName, CallEvent callEvent)
        {
            CallEventConfig config = CallEventInformer.GetConfig(codeName);

            callEvent.CallEventDispatcherInit(config);

            if (!dictionaryActions.ContainsKey(codeName))
            {
                dictionaryActions.Add(codeName, new List<CallEvent>() {callEvent});
            }
            else
            {
                if (IsHaveCallEvent(callEvent))
                    return;

                dictionaryActions[codeName].Add(callEvent);
            }
        }

        protected void DeInit(CallEvent callEvent)
        {
            foreach (var elements in dictionaryActions)
            {
                foreach (var elCallEvent in elements.Value)
                {
                    if (elCallEvent != callEvent) continue;
                    
                    dictionaryActions[elements.Key].Remove(elCallEvent);
                    return;
                }
            }
        }

        protected void DeInit(string codeName, CallEvent callEvent)
        {
            foreach (var elCallEvent in dictionaryActions[codeName])
            {
                if (elCallEvent != callEvent) continue;

                dictionaryActions[codeName].Remove(elCallEvent);
                return;
            }
        }

        protected void Execute(string codeName, object[] @object, string messageExecute = "")
        {
            try
            {
                if (dictionaryActions.ContainsKey(codeName))
                {
                    foreach (var callEvent in dictionaryActions[codeName])
                    {
                        if (!callEvent.IsActiveEvent) continue;
                    
                        callEvent.ForceExecute(@object);
                        // if (@object == null)
                        // {
                        //     callEvent.ForceExecute();
                        // }
                        // else
                        // {
                        //     callEvent.ForceExecute(@object);
                        // }
                    }
                
                }
                else
                {
                    Debug.LogWarning(
                        $"<color=red> CALL EVENT: Not found debug name code {codeName} in dictionary debug manager</color>");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"<color=yellow> CALL EVENT: Was updated dictionary commands. May be was delete command. Command execute with error, but not fatal \n Message Error: {e.Message}</color>");
            }
            
        }
        
        private bool IsHaveCallEvent(CallEvent callEvent)
        {
            var isHave = false;
            foreach (var actionDebug in dictionaryActions)
            {
                isHave = actionDebug.Value.Contains(callEvent);
                if(isHave)
                    break;
            }
            return isHave;
        }
    }
}