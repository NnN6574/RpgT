using System;
using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{
    [Serializable]
    public class InfoDataGraphPreference
    {
        public InfoDataGraphPreferenceSO Data;
        public string NameGraph => GetTryData == null ? "Empty data" : Data.NameGraph;
        public string PathData;
        public bool IsStateShowGroup = true;
        public string NameGroup;
        public bool IsSelected;

        public InfoDataGraphPreference()
        {
        }
        
        public InfoDataGraphPreference(InfoDataGraphPreferenceSO data)
        {
            Data = data;
            SetupPath();
        }
        
        public void SetupPath()
        {
            if (Data == null)
            {
                return;
            }
            PathData = AssetDatabase.GetAssetPath(Data);
        }
        public InfoDataGraphPreferenceSO GetTryData
        {
            get
            {
                if (Data != null) return Data;
                if (PathData == string.Empty) return null;
                Data = AssetDatabase.LoadAssetAtPath<InfoDataGraphPreferenceSO>(PathData);
                return Data;
            }
        }
    }
}