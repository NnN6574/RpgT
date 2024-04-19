using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Playstrom.Core.CallEvent;
using UnityEngine;
using static UnityEngine.Screen;

namespace Playstrom.Core.GameDebug
{
    public class UICmdCommander : MonoBehaviour
    {
#if CMD_DEBUG

        private const string DEFAULT_NAME_GUISKIN = "GUISkin";
        
        private GUISkin GUISkin;
        private bool isOpenDebug;
        private Vector2 scrollPosition;

        private GUIStyle borderStyle;
        private GUIStyle styleHeader;
        private GUIStyle styleSecondHeader;
        private Color colorDebug = new Color(1f, .3f, 0);

        private List<GUIField> GUIFields = new List<GUIField>();
        private List<GUIButton> GUIButtons = new List<GUIButton>();
        private List<GUIToggle> GUIToggles = new List<GUIToggle>();
      

        private void Start()
        {
            GUISkin = Resources.Load<GUISkin>(DEFAULT_NAME_GUISKIN);
            
            borderStyle = new GUIStyle(GUISkin.box)
            {
                padding = new RectOffset(10, 10, 0, 0)
            };
            
            styleHeader = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 70,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = colorDebug
                }
            };
            
            styleSecondHeader = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 40,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = colorDebug
                }
            };
        }

        private void Update()
        {

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                OpenDebugWindow();
            }

           

#elif UNITY_ANDROID || UNITY_IOS

             if (Input.touchCount < 3) return;
            
            Touch touch = Input.GetTouch(2);

            if (touch.tapCount < 2)
            {
                return;
            }
                
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OpenDebugWindow();
                    break;
            }
 #endif
        }

        private void OnGUI()
        {
            if(!isOpenDebug) return;
            
            GUILayout.BeginArea (new Rect (width
                                           /(2f+(borderStyle.padding.left+borderStyle.padding.right)/100f)
                ,height/2f
                ,width
                 /(2f-(borderStyle.padding.left+borderStyle.padding.right)/100f)
                                          ,height/2f));
            GUILayout.BeginVertical(borderStyle);
            GUILayout.Label($"DEBUG", styleHeader);
            GUILayout.Label($"v.{CmdSetting.version}", styleSecondHeader);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, 
                false, true);
            GUIButton();
            GUIField();
            GUIToggle();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void GUIField()
        {
            foreach (var guiField in GUIFields)
            {
                guiField.Draw();
            }
        }
        
        private void GUIButton()
        {
            foreach (var guiButton in GUIButtons)
            {
                guiButton.Draw();
            }
        }
        
        private void GUIToggle()
        {
            foreach (var guiToggle in GUIToggles)
            {
                guiToggle.Draw();
            }
        }

        private void OpenDebugWindow()
        {
            isOpenDebug = !isOpenDebug;
            
            if(isOpenDebug)
                CreateGUI();
        }

        private void CreateGUI()
        {
            GUIReset();
            CmdCommander cmdCommander = FindObjectOfType<CmdCommander>();

            foreach (var callEvent in cmdCommander.DictionaryActions)
            {
                CmdEvent cmdEvent = (CmdEvent) callEvent.Value[0];
                if(!cmdEvent.IsActiveEvent) continue;
                
                SwitchGUI(cmdEvent, callEvent.Value.Select(item => (CmdEvent)item).ToList());
            }
        }

        private void SwitchGUI(CmdEvent target, List<CmdEvent> commands)
        {
            GUIBody guiBody = target.GUIBody;
            guiBody.Init(commands, GUISkin);
            switch (guiBody)
            {
                case GUIButton guiButton:
                    GUIButtons.Add(guiButton);
                    break;
                case GUIField guiField:
                    GUIFields.Add(guiField);
                    break;
                case GUIToggle guiToggle:
                    GUIToggles.Add(guiToggle);
                    break;
            }
        }

        private void GUIReset()
        {
            GUIButtons.Clear();
            GUIFields.Clear();
            GUIToggles.Clear();
        }

#endif
    }
}
