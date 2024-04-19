using System;
using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Utilities.Refactor
{
    public abstract class GUIElementUtility
    {
        public static void HorizontalGroup(params Action[] drawCallback)
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var action in drawCallback)
            {
                action?.Invoke();
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void VerticalGroup(params Action[] drawCallback)
        {
            EditorGUILayout.BeginHorizontal();
            foreach (var action in drawCallback)
            {
                action?.Invoke();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}