using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Elements
{
    public class LCGroup : Group
    {
        public Guid ID { get; set; }
        public string OldTitle { get; set; }

        private readonly Color _defaultBorderColor;
        private readonly float _defaultBorderWidth;

        public LCGroup(string groupTitle, Vector2 position)
        {
            ID = Guid.NewGuid();

            title = groupTitle;
            OldTitle = groupTitle;

            SetPosition(new Rect(position, Vector2.zero));

            _defaultBorderColor = contentContainer.style.borderBottomColor.value;
            _defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        public void SetErrorStyle(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetStyle()
        {
            contentContainer.style.borderBottomColor = _defaultBorderColor;
            contentContainer.style.borderBottomWidth = _defaultBorderWidth;
        }
    }
}