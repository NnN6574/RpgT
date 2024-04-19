using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
    public class GUIButton : GUIBody
    {
         private enum TypeElementsField
        {
            NameButton,
            NameField,
        }

        /// <summary>
        /// button
        /// </summary>
        /// <param name="parameters">
        /// <br></br>
        /// [0] name button
        /// <br></br>
        /// [1] name gui field
        /// </param>
        public GUIButton(params object[] parameters)
        {
            Init(parameters);
        }
        
        private string nameButton;
        private string nameField;
        private GUIStyle styleButton;
        private GUIStyle borderStyle;
        private GUIStyle styleLabel;

        protected sealed override void Init(params object[] parameters)
        {
            nameButton = parameters[(int) TypeElementsField.NameButton].ToString();
            nameField = parameters[(int) TypeElementsField.NameField].ToString();
        }

        protected override void GUIStyleConstruct()
        {
            styleLabel = new GUIStyle(GUISkin.label)
            {
                fixedWidth = Width,
                fixedHeight = Height,
                alignment = TextAnchor.LowerLeft,
                fontSize = (int)SizeFont,

            };

            styleButton = new GUIStyle(GUISkin.button)
            {
                fixedWidth = Width,
                fixedHeight = Height,
                alignment = TextAnchor.MiddleCenter,
                fontSize = (int)SizeFont,
            };

            borderStyle = new GUIStyle()
            {
                padding = new RectOffset(10, 10, 0, 0)
            };
        }

        public override void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(nameField, styleLabel);
            GUILayout.BeginHorizontal(borderStyle);
            if (GUILayout.Button(nameButton, styleButton))
            {
                foreach (var cmdEvent in listCmdActions)
                {
                    cmdEvent.ForceExecute(null);
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}