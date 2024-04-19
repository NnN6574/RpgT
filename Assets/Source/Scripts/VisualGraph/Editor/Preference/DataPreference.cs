using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{
    public class DataPreference
    {
        public List<InfoDataGraphPreference> InfoDataGraphPreferences;

        public InfoDataGraphPreference SelectedPreference;

        public DataPreference()
        {
            ClearDataPreference();
        }

        public int Count => InfoDataGraphPreferences.Count;
        public void FirstClearGraph()
        {
            InfoDataGraphPreferences[0] = new InfoDataGraphPreference();
        }

        public void FillAllInfoDataGraphs()
        {
            foreach (var infoDataGraph in InfoDataGraphPreferences)
            {
                var data = infoDataGraph.GetTryData;
                if (data == null)
                {
                    Debug.Log($"Graph data to null");
                    continue;
                }
            }
        }

        public void RemoveGraph(InfoDataGraphPreference infoDataGraphPreference)
        {
            if (Count <= 1)
            {
                FirstClearGraph();
                return;
            }
            InfoDataGraphPreferences.Remove(infoDataGraphPreference);
        }

        public void RemoveLastGraph()
        {
            if (Count <= 1)
            {
                FirstClearGraph();
                return;
            }
            InfoDataGraphPreferences.RemoveAt(InfoDataGraphPreferences.Count-1);
        }
        
        public void ClearDataPreference()
        {
            InfoDataGraphPreferences = new List<InfoDataGraphPreference>() {new ()};
        }

        public void AddEmptyGraph()
        {
            InfoDataGraphPreferences.Add(new InfoDataGraphPreference());
        }

        public void AddGraph(InfoDataGraphPreference infoDataGraphPreference)
        {
            if (InfoDataGraphPreferences.Count == 1 && InfoDataGraphPreferences[0].GetTryData == null)
            {
                InfoDataGraphPreferences[0] = infoDataGraphPreference;
                return;
            }

            for (var index = 0; index < InfoDataGraphPreferences.Count; index++)
            {
                var preference = InfoDataGraphPreferences[index];
                if (!string.Equals(infoDataGraphPreference.NameGraph, preference.NameGraph)) continue;
                InfoDataGraphPreferences[index] = infoDataGraphPreference;
                return;
            }

            InfoDataGraphPreferences.Add(infoDataGraphPreference);
        }
        
        public void AllDeselectGraphs()
        {
            foreach (var infoDataGraphPreference in InfoDataGraphPreferences)
            {
                infoDataGraphPreference.IsSelected = false;
            }
        }

        public void SelectGraph(InfoDataGraphPreference infoDataGraphPreference)
        { 
            AllDeselectGraphs();
           infoDataGraphPreference.IsSelected = true;
           SelectedPreference = infoDataGraphPreference;
        }

        public void SelectToNameGraph(string name)
        {
            SelectGraph(FindGraphToName(name));
        }

        public InfoDataGraphPreference FindGraphToName(string name)
        {
            return InfoDataGraphPreferences.FirstOrDefault(item => item.GetTryData.NameGraph == name);
        }
    }
}