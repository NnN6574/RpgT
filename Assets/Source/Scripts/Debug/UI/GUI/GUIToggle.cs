using System.Collections;
using System.Collections.Generic;
using Playstrom.Core.GameDebug;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
    public class GUIToggle : GUIBody
    {
        private enum TypeElementsField
        {
            NameButton,
            NameField,
            StartValue
        }

        /// <summary>
        /// toggle
        /// </summary>
        /// <param name="parameters">
        /// <br></br>
        /// [0] label toggle
        /// <br></br>
        /// [1] name gui field
        /// <br></br>
        /// [2] start value
        /// </param>
        public GUIToggle(params object[] parameters)
        {
            Init(parameters);
        }

        private bool value;

        private string nameToggle;
        private string nameField;
        private GUIStyle styleToggle;
        private GUIStyle styleButton;
        private GUIStyle borderStyle;
        private GUIStyle styleLabel;

        protected sealed override void Init(params object[] parameters)
        {
            nameToggle = parameters[(int) TypeElementsField.NameButton].ToString();
            nameField = parameters[(int) TypeElementsField.NameField].ToString();
            value = (bool)parameters[(int) TypeElementsField.StartValue];
        }

        protected override void GUIStyleConstruct()
        {
            styleLabel = new GUIStyle(GUISkin.label)
            {
                fixedWidth = Width,
                fixedHeight = Height,
                alignment = TextAnchor.LowerLeft,
                fontSize = (int) SizeFont,

            };

            styleButton = new GUIStyle(GUISkin.button)
            {
                fixedWidth = Width/2,
                fixedHeight = Height,
                alignment = TextAnchor.MiddleCenter,
                fontSize = (int) SizeFont,
            };
            
            styleToggle = new GUIStyle(GUISkin.label)
            {
                fixedWidth = Width/2,
                fixedHeight = Height,
                alignment = TextAnchor.MiddleLeft,
                fontSize = (int) SizeFont,
                normal =
                {
                    textColor = value ? Color.green : Color.red
                }
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
            GUILayout.Label(value.ToString(), styleToggle);
            if (GUILayout.Button(nameToggle, styleButton))
            {
                value = !value;
                styleToggle.normal.textColor = value ? Color.green : Color.red;
                foreach (var cmdEvent in listCmdActions)
                {
                    cmdEvent.ForceExecute(new object[]{value});
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
