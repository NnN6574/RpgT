using System;
using System.Collections.Generic;
using UnityEngine;

namespace Playstrom.Core.GameDebug
{
    public class GUIField : GUIBody
    {
        private enum TypeElementsField
        {
            NameButton,
            NameField,
            StartTextField
        }

        /// <summary>
        /// input field
        /// </summary>
        /// <param name="parameters">
        /// <br></br>
        /// [0] name button
        /// <br></br>
        /// [1] name field
        /// <br></br>
        /// [2] start text field
        /// </param>
        public GUIField(params object[] parameters)
        {
            Init(parameters);
        }
        
        private string text;
        private string nameButton;
        private string nameField;
        private GUIStyle styleField;
        private GUIStyle styleButton;
        private GUIStyle borderStyle;
        private GUIStyle styleLabel;

        protected sealed override void Init(params object[] parameters)
        {
            nameButton = parameters[(int) TypeElementsField.NameButton].ToString();
            nameField = parameters[(int) TypeElementsField.NameField].ToString();

            if (parameters.Length >(int) TypeElementsField.StartTextField && parameters[(int) TypeElementsField.StartTextField].ToString() != string.Empty)
            {
                text = parameters[(int) TypeElementsField.StartTextField].ToString();
            }
        }

        protected override void GUIStyleConstruct()
        {
            styleField = new GUIStyle(GUISkin.textField)
            {
                fixedWidth = Width/2,
                fixedHeight = Height,
                alignment = TextAnchor.MiddleLeft,
                fontSize = (int) SizeFont,

            };
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
            text = GUILayout.TextField(text, styleField);
            if (GUILayout.Button(nameButton, styleButton))
            {
                foreach (var cmdEvent in listCmdActions)
                {
                    cmdEvent.ForceExecute(new object[]{text});
                }

                text = string.Empty;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}