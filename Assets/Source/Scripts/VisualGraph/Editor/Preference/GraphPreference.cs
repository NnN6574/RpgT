using System;
using LevelsConstructor.New.Editor.Utilities.Refactor;
using LevelsConstructor.New.Editor.Utilities.Refactor.Graph;
using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{
    public static class GraphPreference
    {
        private static DataPreference _dataPreference;

        private static string _nameGraph;
        private static string _pathRoot { get; set; } = PathConstants.PathPreferences;

        [InitializeOnLoadMethod]
        private static void Initialization()
        {
            _dataPreference = StaticDataPreference.GetTryDataPreference;
            _dataPreference.FillAllInfoDataGraphs();
        }
        
        [PreferenceItem (ConstantsPreference.PreferenceName)]
        [Obsolete("Obsolete")]
        public static void PreferencesGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(GUIConstantsPreference.StartSpace);
            DrawBlockCreateNewGraph();

            DrawBlockUpButtonsGraphPreference();
            
            EditorGUILayout.Space(GUIConstantsPreference.BetweenGraphPreferenceSpace);
            
            DrawBlockInformationListGraphs();
           
            EditorGUILayout.Space(GUIConstantsPreference.SmallPaddingUI);
            
            DrawBlockGraphPreference();
            
            EditorGUILayout.Space(GUIConstantsPreference.BetweenGraphPreferenceSpace);
            
            DrawBlockDownButtonsGraphPreference();

            EditorGUILayout.EndVertical();
        }

    

        private static void DrawBlockUpButtonsGraphPreference()
        {
            EditorGUILayout.BeginHorizontal();
            DrawBlockAddGraphPreference();
            EditorGUILayout.EndHorizontal();
        }
        
        private static void DrawBlockDownButtonsGraphPreference()
        {
            EditorGUILayout.BeginHorizontal();
            DrawBlockClearPreference();
            DrawBlockRemoveLastGraphPreference();
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawBlockCreateNewGraph()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Create new graph", new GUIStyle("Label"){fontSize= 15, fontStyle = FontStyle.Bold});
            EditorGUILayout.Space(GUIConstantsPreference.SmallPaddingUI);
            EditorGUILayout.LabelField("Name graph: ", new GUIStyle("Label"){fontStyle = FontStyle.Bold});
            _nameGraph = EditorGUILayout.TextField(_nameGraph);
            EditorGUILayout.LabelField("Root path ", new GUIStyle("Label"){fontStyle = FontStyle.Bold});
            _pathRoot = EditorGUILayout.TextField(_pathRoot);

            EditorGUILayout.EndVertical();
        }
        
        private static void DrawBlockInformationListGraphs()
        {
            void Label()
            {
                EditorGUILayout.LabelField($"List graphs [{_dataPreference.Count}]:", new GUIStyle("Label"){fontSize= 15,  fontStyle = FontStyle.Bold});
            }

            void ButtonRefresh()
            {
                if (GUILayout.Button("Refresh Preference", new GUIStyle("Button"){ fixedHeight = 30}))
                { 
                    Initialization();
                }
            }
            
            void ButtonClear()
            {
                if (GUILayout.Button("Clear Preference", new GUIStyle("Button"){ fixedHeight = 30}))
                { 
                    _dataPreference.ClearDataPreference();
                }
            }
            

            void GroupButtons()
            {
                GUIElementUtility.HorizontalGroup(ButtonRefresh, ButtonClear);
            }
            GUIElementUtility.HorizontalGroup(Label, GroupButtons);
        }
        

        private static void DrawBlockAddGraphPreference()
        {
            if (GUILayout.Button("Add Graph"))
            {

                if(string.IsNullOrEmpty(_nameGraph)) return;
                
                AutoFolderIOUtility.CreateDefaultFolders();

                InfoDataGraphPreferenceSoFactory factory = new InfoDataGraphPreferenceSoFactory();
                var so = factory.CreateGraph(_nameGraph, _pathRoot);

                InfoDataGraphPreference infoDataGraphPreference = new InfoDataGraphPreference(so);
                
                _dataPreference.AddGraph(infoDataGraphPreference);

                _nameGraph = "";
                _pathRoot = PathConstants.PathPreferences;
            }
        }
        
        private static void DrawBlockClearPreference()
        {
            if (GUILayout.Button("Auto Folders Preference"))
            {
                AutoFolderIOUtility.CreateDefaultFolders();
            }
        }
        
        private static void DrawBlockRemoveLastGraphPreference()
        {
            if (GUILayout.Button("Remove Last Graph"))
            {
                _dataPreference.RemoveLastGraph();
            }
        }
        
        
        private static void DrawBlockLabel(string value)
        {
            EditorGUILayout.LabelField(value);
        }

        [Obsolete("Obsolete")]
        private static void DrawBlockGraphPreference()
        {
            if (_dataPreference.Count <= 0)
            {
                DrawBlockLabel("Not create graphs!!!");
                StaticDataPreference.LoadingDataPreference();
                return;
            }

            for (int i = 0; i < _dataPreference.Count; i++)
            {
                var value = _dataPreference.InfoDataGraphPreferences[i];
                var constMessage = "Empty graph";
                var fullMessage = value.Data != null ? value.Data.NameGraph : constMessage;

                value.IsStateShowGroup =
                    EditorGUILayout.BeginFoldoutHeaderGroup(value.IsStateShowGroup, value.NameGroup);
                
                EditorGUILayout.Space(GUIConstantsPreference.BetweenGraphPreferenceSpace);

                if (value.IsStateShowGroup)
                {
                    EditorGUILayout.BeginHorizontal();
                    value.Data =
                        EditorGUILayout.ObjectField(value.Data, typeof(InfoDataGraphPreferenceSO)) as
                            InfoDataGraphPreferenceSO;

                    if (_dataPreference.Count > 1 && i != 0)
                    {
                        if (GUILayout.Button("X", new GUIStyle("Button") {fixedWidth = 30, fixedHeight = 20}))
                        {
                            _dataPreference.RemoveGraph(value);
                        }
                    }

                    EditorGUILayout.EndHorizontal();

                    if (GUI.changed)
                    {
                        value.Data = value.Data;
                    }

                    value.NameGroup = fullMessage;
                }
                else
                {
                    value.NameGroup = $"Select at graph: {fullMessage}";
                }

                EditorGUILayout.Space(GUIConstantsPreference.BetweenGraphPreferenceSpace);
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if (GUI.changed)
            {
               StaticDataPreference.SaveDataPreference();
            }
            
        }


        private static void DrawBlockTextFieldPreference(string value)
        {
            if (value == String.Empty)
            {
                value = EditorPrefs.GetString("NamePreferenceKey", String.Empty);
            }
            
            value = EditorGUILayout.TextField("Name preference: ", value);

            if (GUI.changed)
            {
                EditorPrefs.SetString("NamePreferenceKey", value);
            }
            
        }
        
    }
}