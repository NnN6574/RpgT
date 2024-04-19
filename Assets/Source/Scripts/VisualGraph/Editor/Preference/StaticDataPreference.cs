using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{
    public static class StaticDataPreference
    {
        private const string InfoDataGraphPreferenceKey = "InfoDataGraphPreferenceKey";
        private static DataPreference DataPreference;

        public static InfoDataGraphPreference SelectedPreference => GetTryDataPreference.SelectedPreference;

        public static DataPreference GetTryDataPreference
        {
            get
            {
                if (DataPreference == null)
                {
                    DataPreference = new DataPreference();
                    LoadingDataPreference();
                }

                return DataPreference;
            }
        }
        
        private static void SelectGraph(InfoDataGraphPreference infoDataGraphPreference)
        {
            GetTryDataPreference.SelectGraph(infoDataGraphPreference);
        }

        public static void SelectToNameGraph(string nameGraph)
        {
           GetTryDataPreference.SelectToNameGraph(nameGraph);
        }
        
        public static void LoadingDataPreference()
        {
            DataPreference temp =
                JsonUtility.FromJson<DataPreference>(
                    EditorPrefs.GetString(InfoDataGraphPreferenceKey, String.Empty));
            if(temp != null)
                DataPreference.InfoDataGraphPreferences = temp.InfoDataGraphPreferences;
        }

        public static void SaveDataPreference()
        {
            foreach (var infoDataGraph in DataPreference.InfoDataGraphPreferences)
            {
                infoDataGraph.SetupPath();
            }
            
            var value = JsonUtility.ToJson(DataPreference);
            
            EditorPrefs.SetString(InfoDataGraphPreferenceKey, value);
        }

        public static List<(string, bool)> GetInfoDataGraphValueTuples()
        {
            List<(string, bool)> temp = new List<(string, bool)>();
            for (int i = 0; i < GetTryDataPreference.InfoDataGraphPreferences.Count; i++)
            {
                var el = GetTryDataPreference.InfoDataGraphPreferences[i];
                
                temp.Add((el.NameGroup, el.IsSelected));
            }

            return temp;
        }
        
        
        
        
    }
}