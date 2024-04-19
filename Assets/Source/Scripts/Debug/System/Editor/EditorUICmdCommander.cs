using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Playstrom.Core.GameDebug.Editor
{
    public class EditorUICmdCommander
    {
        [MenuItem("Tools/Playstrom/Debug/Create Debug Commander")]
        public static void GenerateDebugger()
        {
            string nameObject = "Debug Commander";

            GameObject findDebugSystem = GameObject.Find(nameObject);

            if (findDebugSystem != null)
            {
                Debug.Log($"Was found debug commander!!! Object will destroy and update data!!!");
                Debug.Log($"Settings debug system will set - default");
                Object.DestroyImmediate(findDebugSystem);
            }

            GameObject debugSystem = new GameObject(nameObject);
            
            debugSystem.AddComponent<CmdCommander>();
            debugSystem.AddComponent<UICmdCommander>();
            debugSystem.AddComponent<CmdInformer>();
            
            EditorUICmdSetting.Initialization();
            
            Debug.Log($"Was create debug commander system");

            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            
            Debug.Log($"Save scene");
        }
    }
}
