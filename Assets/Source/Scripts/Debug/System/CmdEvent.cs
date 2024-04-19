using System;
#if UNITY_EDITOR
using System.Diagnostics;
#endif
using Playstrom.Core.CallEvent;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Playstrom.Core.GameDebug
{
    public class CmdEvent : CallEvent.CallEvent
    {
        public static event Action<string, CmdEvent> OnInit;
        public static event Action<string, object[], string> OnExecute;
        public static event Action<string, CmdEvent> OnDeInit;
        
        public GUIBody GUIBody
        {
            get => guiBody;
            private set => guiBody = value;
        }

        public CmdEvent()
        {
            
        }
        public CmdEvent(string codeName, Action<object[]> action, GUIBody guiBody = null)
        {
            Init(codeName, action);
            GUIBody = guiBody;
        }
        public CmdEvent(string codeName, Action action, GUIBody guiBody = null)
        {
            GUIBody = guiBody;
            Init(codeName, action);
        }

        public sealed override void Init(string codeName, Action<object[]> action)
        {
            this.codeName = codeName;
            listActions.Add(action);
            OnInit?.Invoke(codeName.ToString(), this);
        }

        public sealed override void Init(string codeName, Action action)
        {
            this.codeName = codeName;
            listActions.Add(_ => { action?.Invoke();});
            OnInit?.Invoke(codeName.ToString(), this);
        }

        public override void Add(string codeName, Action<object[]> action) => Init(codeName, action);
        public override void Add(string codeName, Action action) => Init(codeName, action);

        public override void DeInit()
        {
            OnDeInit?.Invoke(codeName, this);
        }

        public override void CallEventDispatcherInit(CallEventConfig config)
        {
            NameEvent = config.Name;
            DescriptionEvent = config.Description;
            IsActiveEvent = config.IsActive;
#if UNITY_EDITOR
            config.SetupListLinkCommands();
#endif
        }
        
        public new static void Execute(string codeName, string messageExecute = "")
        {
            OnExecute?.Invoke(codeName.ToString(), null, messageExecute);
        }

        public new static void Execute(string codeName, string messageExecute = "", params object[] value)
        {
            OnExecute?.Invoke(codeName.ToString(), value, messageExecute);
        }
        
        //TODO Нужно перенести
        private GUIBody guiBody;
        
    }
}
