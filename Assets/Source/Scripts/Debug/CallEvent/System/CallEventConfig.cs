using System.Collections.Generic;
using System;
using Unity.Collections;
#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
#endif
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Playstrom.Core.CallEvent
{
    public class CallEventConfig : ScriptableObject
    {
        public string Name;
        public string CodeName;
        public bool IsActive;
        [Multiline(10)]public string Description;

#if UNITY_EDITOR

        [Serializable]
        public class FullPathClassCallEvent
        {
            public string ClassCallEvent;
            public List<string> FullPath = new List<string>();
        }
        
        
        [Space]
        [Header("Editor")]
        public List<FullPathClassCallEvent> ClassCallEvent = new List<FullPathClassCallEvent>();

        public void DataReset()
        {
            ClassCallEvent.Clear();
        }
        public void SetupListLinkCommands()
        {
            string temp = "";

            FullPathClassCallEvent tempPath = new FullPathClassCallEvent();;
            int count =  new StackTrace().FrameCount;
            for (int i = 0; i < count; i++)
            {
                StackFrame stackFrame = new StackTrace().GetFrame(count-i-1);
                Type calledBy = stackFrame.GetMethod().DeclaringType;
                int numberLine = new StackFrame(count-i-1, true).GetFileLineNumber();
                temp = $"{numberLine}. {calledBy}";

                if (i == 0)
                {
                    tempPath.ClassCallEvent = temp;
                }


                tempPath.FullPath.Add(temp);


            }

            ClassCallEvent.Add(tempPath);
            
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
