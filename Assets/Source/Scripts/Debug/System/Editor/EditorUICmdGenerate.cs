using System.Collections.Generic;
using System.IO;
using System.Linq;
using Playstrom.Core.CallEvent;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Playstrom.Core.GameDebug.Editor
{
    public class EditorUICmdGenerate : EditorWindow
    {
        private const string CONST_NAME_FILE = "TypeDebugCommands";
        private const string CONST_CODE_COMMAND = "CodeCommand";
        private const string CONST_NAME_COMMAND = "NameCommand";
        private const string CONST_DESCRIPTION_COMMAND = "DescriptionCommand";

        private Color colorDebug = new Color(1f, .3f, 0);

        private GUIStyle styleHeader;
        private GUIStyle styleSecondHeader;
        private GUIStyle styleWarning;

        private static EditorUICmdGenerate window;

        private string nameCommand;
        private string codeCommand;
        private string description;

        private bool isHaveCommand;
        private Vector2 scrollPosition;

        private static List<string> nameCommands = new List<string>();

#if CMD_DEBUG
        [RuntimeInitializeOnLoadMethod]
#endif
        private static void GenerateEnumCommand()
        {
            string pathEnumFolder = CmdSetting.PathEnumCommand;
            string nameFullFile = CONST_NAME_FILE + ".cs";
            string pathFullEnum = pathEnumFolder + nameFullFile;

            CmdConfig[] enumEntries = CallEventSetting.GetLoadResources(CmdSetting.PathDebugLoadConfigs)
                .Select(item => (CmdConfig) item)
                .ToArray();

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
                fontSize = 50,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = colorDebug
                }
            };

            styleSecondHeader = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = colorDebug
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
            GUILayout.Space(25);
            GUILayout.Label("DEBUG", styleHeader);
            GUILayout.Label("Создание cmd debug команд", styleSecondHeader);
            GUILayout.Space(10);
            nameCommand = EditorGUILayout.TextField("Name command*", EditorPrefs.GetString(CONST_NAME_COMMAND));
            codeCommand = EditorGUILayout.TextField("Code command", EditorPrefs.GetString(CONST_CODE_COMMAND));
            if (isHaveCommand)
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

                isHaveCommand = CallEventSetting.IsHave(codeCommand, CmdSetting.PathDebugLoadConfigs);

                if (isHaveCommand)
                {
                    GUILayout.EndVertical();
                    return;
                }

                SaveCmdConfig(nameCommand, codeCommand, description);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GenerateEnumCommand();

                LabelReset();
                EditorGUILayout.TextField("Code Command", EditorPrefs.GetString(CONST_CODE_COMMAND));
                EditorGUILayout.TextField("Name Command", EditorPrefs.GetString(CONST_NAME_COMMAND));
                EditorGUILayout.TextArea(EditorPrefs.GetString(CONST_DESCRIPTION_COMMAND));

                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                LoadCommandsData();
            }
        }

        private static void LabelReset()
        {
            EditorPrefs.SetString(CONST_CODE_COMMAND, "");
            EditorPrefs.SetString(CONST_NAME_COMMAND, "");
            EditorPrefs.SetString(CONST_DESCRIPTION_COMMAND, "");
        }

        private void GUIScrollCommands()
        {
            GUILayout.Space(15);
            GUILayout.Label("Info commands");


            scrollPosition =
                EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(350), GUILayout.Height(120));
            for (int i = 0; i < nameCommands.Count; i++)
            {
                GUILayout.Label($"{nameCommands[i]}");
            }

            EditorGUILayout.EndScrollView();
        }

        private void OnGUI()
        {
            GUIStyle();


            GUILayout.BeginVertical(EditorStyles.toolbar);

            GUILabels();
            GUIButton();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("* - Обязательное заполнение");

            GUIScrollCommands();
            GUILayout.EndVertical();
        }


        private void SaveCmdConfig(string nameCommand, string codeCommand, string description)
        {
            CmdConfig asset = ScriptableObject.CreateInstance<CmdConfig>();

            asset.Name = nameCommand;
            int countSpace = codeCommand.Split(' ').Length;
            asset.CodeName = countSpace > 0 ? codeCommand.Replace(' ', '_') : codeCommand;
            asset.Description = description;
            asset.IsActive = true;
            asset.SetDirty();

            string path = CmdSetting.PathDebugConfigs;
            string fullPath = path + nameCommand + ".asset";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            AssetDatabase.CreateAsset(asset, fullPath);
        }

        private static void LoadCommandsData()
        {
            List<CallEventConfig> commands = new List<CallEventConfig>();
            commands = CallEventSetting.GetLoadResources(CmdSetting.PathDebugLoadConfigs);

            nameCommands.Clear();

            for (int i = 0; i < commands.Count; i++)
            {
                string classes = "";
                for (int j = 0; j < commands[i].ClassCallEvent.Count; j++)
                {
                    classes += $"\t{commands[i].ClassCallEvent[j]}\n";
                }

                nameCommands.Add($"{i + 1}.\tName: {commands[i].Name}\n" +
                                 $"\tCode Name: {commands[i].CodeName}\n" +
                                 $"\tUse Classes:\n" +
                                 $"{classes}");
            }
        }

        [MenuItem("Tools/Playstrom/Debug/Create Command #%d")]
        public static void ShowWindow()
        {
            LoadCommandsData();
            if (window == null)
            {
                window = GetWindow<EditorUICmdGenerate>(title: $"Create Debug Command v. {CmdSetting.version}");
                window.minSize = new Vector2(350, 450);
                window.maxSize = new Vector2(350, 450);
                LabelReset();
                window.Show();
            }
            else
            {
                FocusWindowIfItsOpen<EditorUICmdGenerate>();
            }

        }
    }
}