using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Playstrom.Core.CallEvent
{
    public class CallEvent
    {
        public static event Action<string, CallEvent> OnInit;
        public static event Action<string, object[], string> OnExecute;
        public static event Action<string, CallEvent> OnDeInit;

        protected string codeName;
        protected List<Action<object[]>> listActions = new List<Action<object[]>>();
        protected string NameEvent { get; set; }
        protected string DescriptionEvent { get; set; }
        public bool IsActiveEvent { get; protected set; }

        public virtual void CallEventDispatcherInit(CallEventConfig config)
        {
            NameEvent = config.Name;
            DescriptionEvent = config.Description;
            IsActiveEvent = config.IsActive;
            
#if UNITY_EDITOR
            config.SetupListLinkCommands();
#endif
        }

        public virtual void Init(string codeName, Action action)
        {
            this.codeName = codeName;
            listActions.Add(_ => { action?.Invoke();});
            OnInit?.Invoke(codeName.ToString(), this);
        }

        public virtual void Init(string codeName, Action<object[]> action)
        {
            this.codeName = codeName;
            listActions.Add(action);
            OnInit?.Invoke(codeName.ToString(), this);
        }
        
        public virtual void Add(string codeName, Action<object[]> action) => Init(codeName, action);
        public virtual void Add(string codeName, Action action) => Init(codeName, action);
        
        public virtual void DeInit()
        {
            OnDeInit?.Invoke(codeName, this);
        }

        public static void Execute(string codeName, string messageExecute = "")
        {
            OnExecute?.Invoke(codeName.ToString(), null, messageExecute);
        }

        public static void Execute(string codeName, string messageExecute = "", params object[] value)
        {
            OnExecute?.Invoke(codeName.ToString(), value, messageExecute);
        }

        // public void ForceExecute()
        // {
        //     action?.Invoke();
        // }

        public void ForceExecute(object[] value)
        {
          //  actionObject?.Invoke(value);
            foreach (var action in listActions)
            {
                action?.Invoke(value);
            }
        }
    }
}
