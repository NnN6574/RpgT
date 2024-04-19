using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Playstrom.Core.CallEvent.Editor
{
    public class EditorUICallEventGenerate : EditorWindow
    {
        private const string CONST_NAME_FILE = "TypeCallEventCommands";
        private const string CONST_CODE_COMMAND = "CodeCommand";
        private const string CONST_NAME_COMMAND = "NameCommand";
        private const string CONST_DESCRIPTION_COMMAND = "DescriptionCommand";

        private GUIStyle styleHeader;
        private GUIStyle styleSecondHeader;
        private GUIStyle styleWarning;

        private static EditorUICallEventGenerate window;

        private string nameCommand;
        private string codeCommand;
        private string description;
        
        private bool isHaveCommand;

        [RuntimeInitializeOnLoadMethod]
        public static void GenerateEnumCommand()
        {
            string pathEnumFolder = CallEventSetting.PathEnumCommand;
            string nameFullFile = CONST_NAME_FILE + ".cs";
            string pathFullEnum = pathEnumFolder + nameFullFile;

            CallEventConfig[] enumEntries =
                CallEventSetting.GetLoadResources(CallEventSetting.PathCallEventLoadConfigs).ToArray();

            if (enumEntries.Length <= 0) return;

            if (!Directory.Exists(pathEnumFolder))
            {
                Directory.CreateDirectory(pathEnumFolder);
            }

            using (StreamWriter streamWriter = new StreamWriter(pathFullEnum))
            {
                streamWriter.WriteLine("public class " + CONST_NAME_FILE);
                streamWriter.WriteLine("{");
                for (int i = 0; i < enumEntries.Length; i++)
                {
                    string filterEnum = enumEntries[i].CodeName.Replace(' ', '_');
                    char symbol = '"';
                    streamWriter.WriteLine($"\t public static string {filterEnum} = {symbol}{filterEnum}{symbol};");
                }

                streamWriter.WriteLine("}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void GUIStyle()
        {
            styleHeader = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 35,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.cyan
                }
            };

            styleSecondHeader = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.cyan
                }
            };
            
            styleWarning = new GUIStyle
            {
                alignment = TextAnchor.MiddleRight,
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = Color.red
                }
            };

        }

        private void GUILabels()
        {
            GUILayout.Space(35);
            GUILayout.Label("EVENT SYSTEM", styleHeader);
            GUILayout.Label("Создание call event команд", styleSecondHeader);
            GUILayout.Space(20);
            nameCommand = EditorGUILayout.TextField("Name command*", EditorPrefs.GetString(CONST_NAME_COMMAND));
            codeCommand = EditorGUILayout.TextField("Code command", EditorPrefs.GetString(CONST_CODE_COMMAND));
            if(isHaveCommand)
                GUILayout.Label("Имя данной команды уже существует!  ", styleWarning);
            GUILayout.Space(15);
            GUILayout.Label("Description");
            description = EditorGUILayout.TextArea(EditorPrefs.GetString(CONST_DESCRIPTION_COMMAND));

            if (GUI.changed)
            {
                EditorPrefs.SetString(CONST_CODE_COMMAND, codeCommand);
                EditorPrefs.SetString(CONST_NAME_COMMAND, nameCommand);
                EditorPrefs.SetString(CONST_DESCRIPTION_COMMAND, description);
                EditorGUILayout.TextField(CONST_CODE_COMMAND, codeCommand);
            }

            GUILayout.Space(5);
        }

        private void GUIButton()
        {
            if (GUILayout.Button("Generate"))
            {
                if (nameCommand == "" && codeCommand == "")
                {
                    GUILayout.EndVertical();
                    return;
                }

                if (codeCommand == "") codeCommand = nameCommand;

                if (CallEventSetting.IsHave(codeCommand, CallEventSetting.PathCallEventLoadConfigs))
                {
                    GUILayout.EndVertical();
                    return;
                }

                SaveCallEventConfig(nameCommand, codeCommand, description);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GenerateEnumCommand();

                EditorPrefs.SetString(CONST_CODE_COMMAND, "");
                EditorPrefs.SetString(CONST_NAME_COMMAND, "");
                EditorPrefs.SetString(CONST_DESCRIPTION_COMMAND, "");
                EditorGUILayout.TextField("Code Command", EditorPrefs.GetString(CONST_CODE_COMMAND));
                EditorGUILayout.TextField("Name Command", EditorPrefs.GetString(CONST_NAME_COMMAND));
                EditorGUILayout.TextArea(EditorPrefs.GetString(CONST_DESCRIPTION_COMMAND));

                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
        }

        private void OnGUI()
        {
            GUIStyle();
            GUILayout.BeginVertical(EditorStyles.toolbar);

            GUILabels();
            GUIButton();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("* - Обязательное заполнение");
            GUILayout.EndVertical();
        }

        private static void LabelReset()
        {
            EditorPrefs.SetString(CONST_CODE_COMMAND, "");
            EditorPrefs.SetString(CONST_NAME_COMMAND, "");
            EditorPrefs.SetString(CONST_DESCRIPTION_COMMAND, "");
        }


        private void SaveCallEventConfig(string nameCommand, string codeCommand, string description)
        {
            CallEventConfig asset = ScriptableObject.CreateInstance<CallEventConfig>();

            Debug.Log(nameCommand);
            Debug.Log(codeCommand);
            asset.Name = nameCommand;
            int countSpace = codeCommand.Split(' ').Length;
            asset.CodeName = countSpace > 0 ? codeCommand.Replace(' ', '_') : codeCommand;
            asset.Description = description;
            asset.IsActive = true;
            asset.SetDirty();

            string path = CallEventSetting.PathDebugConfigs;
            string fullPath = path + nameCommand + ".asset";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            AssetDatabase.CreateAsset(asset, fullPath);
        }

        [MenuItem("Tools/Playstrom/CallEvent/Create Command #%e")]
        public static void ShowWindow()
        {
            if (window == null)
            {
                window = GetWindow<EditorUICallEventGenerate>(title: $"Create Call Event Command v. {CallEventSetting.version}");
                window.minSize = new Vector2(350, 450);
                window.maxSize = new Vector2(350, 450);
                LabelReset();
                window.ShowAuxWindow();
            }
            else
            {
               FocusWindowIfItsOpen<EditorUICallEventGenerate>();
            }
        }
    }
}