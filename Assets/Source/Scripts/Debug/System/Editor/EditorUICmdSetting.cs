using System;
using System.IO;
using System.Linq;
using Playstrom.Core.CallEvent;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Playstrom.Core.GameDebug.Editor
{
    public static class EditorUICmdSetting
    {
        private class EditorGUIPreferencesData
        {
            public bool IsPreload;
            public object Data;
            public string Key;
            public string Label;

            public EditorGUIPreferencesData(string key, object data, string label)
            {
                Key = key;
                Data = data;
                Label = label;
            }
        }

        private static readonly EditorGUIPreferencesData editPathSetting = new("PathSetting", CmdSetting.PathSetting, "Path setting");
        private static readonly EditorGUIPreferencesData editPathEnumCommand = new("PathEnumCommand", CmdSetting.PathEnumCommand, "Path enum command");
     
        [MenuItem("Tools/Playstrom/Debug/Setting")]
        private static void OpenDebugSetting()
        {
            OpenPreferences();
            LoadSetting<CmdSetting>();
        }
        
        public static void Initialization()
        {
            if (!Directory.Exists(CmdSetting.PathSetting))
            {
                Directory.CreateDirectory(CmdSetting.PathSetting);
            }

            string fullPath = CmdSetting.PathSetting + CmdSetting.CONST_DEBUG_SETTINGS + ".asset";
            
            CmdSetting asset = ScriptableObject.CreateInstance<CmdSetting>();
            
            AssetDatabase.CreateAsset(asset, fullPath);
      
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        
        private static void LoadSetting <T>() where T: CmdSetting
        {
            var guids = AssetDatabase.FindAssets("t:"+typeof(T).FullName);
            
            if (guids.Length <= 0)
            {
                Initialization();
                guids = AssetDatabase.FindAssets("t:"+typeof(T).FullName);
            }

            foreach (var item in guids)
            {
                var configPath = AssetDatabase.GUIDToAssetPath(item);
                var config = AssetDatabase.LoadAssetAtPath<T>(configPath);
                
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = config;
            }
        }
        
        private static void GUILabel(EditorGUIPreferencesData editorGUIPreferencesData, out string newData)
        {
            string data = editorGUIPreferencesData.Data.ToString();

            if (!editorGUIPreferencesData.IsPreload) {
                data = EditorPrefs.GetString(editorGUIPreferencesData.Key, data);
                editorGUIPreferencesData.IsPreload = true;
            }
            
            editorGUIPreferencesData.Data = EditorGUILayout.TextField(editorGUIPreferencesData.Label, data);

            if (GUI.changed)
            {
                EditorPrefs.SetString(editorGUIPreferencesData.Key, data);
            }

            newData = data;
        }

        public static void GUICommands()
        {
            GUILayout.BeginVertical();
            CmdConfig[] configs = CallEventSetting.GetLoadResources(CmdSetting.PathDebugConfigs)
                .Select(item => (CmdConfig) item)
                .ToArray();
            GUILayout.BeginScrollView(Vector2.up, false, true);
            for (int i = 0; i < configs.Length+30; i++)
            {
                GUILayout.Label(configs[0].CodeName);
                GUILayout.Space(10);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        [PreferenceItem (CmdSetting.CONST_NAME_PREFERENCES)]
        public static void PreferencesGUI () 
        {
            GUILabel(editPathSetting, out CmdSetting.PathSetting);
            GUILabel(editPathEnumCommand, out CmdSetting.PathEnumCommand);
            //GUICommands();
        }

        private static void OpenPreferences()
        {
            SettingsService.OpenUserPreferences("Preferences/" + GameDebug.CmdSetting.CONST_NAME_PREFERENCES);
        }
    }
}