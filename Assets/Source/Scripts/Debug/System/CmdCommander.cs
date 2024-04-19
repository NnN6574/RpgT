using System.Collections.Generic;
using Playstrom.Core.CallEvent;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
    public class CmdCommander : CallEventMaintenance
    {
        public Dictionary<string, List<CallEvent.CallEvent>> DictionaryActions => dictionaryActions;

        public static CmdCommander CreateSelf(bool isEnableUI = false)
        {
            GameObject obj = new GameObject("Debug Commander");
            obj.AddComponent<CmdInformer>();
            if(isEnableUI)
                obj.AddComponent<UICmdCommander>();
            obj.AddComponent<CmdCommander>();
            return obj.GetComponent<CmdCommander>();
        }
        
        public override void Events()
        {
            CmdEvent.OnInit += Init;
            CmdEvent.OnExecute += Execute;
            CmdEvent.OnDeInit += DeInit;
        }

        public override void DeEvents()
        {
            CmdEvent.OnInit -= Init;
            CmdEvent.OnExecute -= Execute;
            CmdEvent.OnDeInit -= DeInit;
        }

        private void DebugMessage(string message)
        {
#if UNITY_EDITOR
            if (!_isShowLogsInConsole) return;

            Debug.Log(
                $"<color=green> CMD_DEBUG: {message} </color>");
#endif
        }

#if UNITY_EDITOR
        public void EditorTestAwake()
        {
            Awake();
        }
#endif

        private void Awake()
        {
#if CMD_DEBUG
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DeEvents();
                if(CallEventInformer != null)
                    CallEventInformer.DataReset();
            }
#endif

            Construct();
            Events();
            ((CmdInformer) CallEventInformer).Init();
#else
            DeEvents();
#endif
        }

        private void OnDestroy()
        {
            DeEvents();
        }


        [SerializeField] private bool _isShowLogsInConsole;
    }
}