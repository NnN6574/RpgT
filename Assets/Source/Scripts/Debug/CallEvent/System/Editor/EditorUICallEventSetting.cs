using System.IO;
using UnityEditor;
using UnityEngine;

namespace Playstrom.Core.CallEvent.Editor
{
    public static class EditorUICallEventSetting
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

        private static readonly EditorGUIPreferencesData editPathSetting = new("PathSetting", CallEventSetting.PathSetting, "Path setting");
        private static readonly EditorGUIPreferencesData editPathEnumCommand = new("PathEnumCommand", CallEventSetting.PathEnumCommand, "Path enum command");
     
        [MenuItem("Tools/Playstrom/CallEvent/Setting")]
        private static void OpenDebugSetting()
        {
            OpenPreferences();
            LoadSetting<CallEventSetting>();
        }
        
        public static void Initialization()
        {
            if (!Directory.Exists(CallEventSetting.PathSetting))
            {
                Directory.CreateDirectory(CallEventSetting.PathSetting);
            }

            string fullPath = CallEventSetting.PathSetting + CallEventSetting.CONST_DEBUG_SETTINGS + ".asset";
            
            CallEventSetting asset = ScriptableObject.CreateInstance<CallEventSetting>();
            
            AssetDatabase.CreateAsset(asset, fullPath);
      
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        
        private static void LoadSetting <T>() where T: CallEventSetting
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
            CallEventConfig[] configs = CallEventSetting.GetLoadResources(CallEventSetting.PathDebugConfigs).ToArray();
            GUILayout.BeginScrollView(Vector2.up, false, true);
            for (int i = 0; i < configs.Length+30; i++)
            {
                GUILayout.Label(configs[0].CodeName);
                GUILayout.Space(10);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        [PreferenceItem (CallEventSetting.CONST_NAME_PREFERENCES)]
        public static void PreferencesGUI () 
        {
            GUILabel(editPathSetting, out CallEventSetting.PathSetting);
            GUILabel(editPathEnumCommand, out CallEventSetting.PathEnumCommand);
            //GUICommands();
        }

        private static void OpenPreferences()
        {
            SettingsService.OpenUserPreferences("Preferences/" + CallEventSetting.CONST_NAME_PREFERENCES);
        }
    }
}