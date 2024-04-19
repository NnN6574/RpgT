using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Playstrom.Core.CallEvent.Editor
{
    public class EditorUICallEventMaintenance
    {
        [MenuItem("Tools/Playstrom/CallEvent/Create Call Event Maintenance")]
        public static void GenerateDebugger()
        {
            string nameObject = "Call Event Maintenance";

            GameObject findDebugSystem = GameObject.Find(nameObject);

            if (findDebugSystem != null)
            {
                Debug.Log($"Was found call event maintenance!!! Object will destroy and update data!!!");
                Debug.Log($"Settings call event system will set - default");
                Object.DestroyImmediate(findDebugSystem);
            }

            GameObject callEventSystem = new GameObject(nameObject);
            
            callEventSystem.AddComponent<CallEventMaintenance>();
            callEventSystem.AddComponent<CallEventInformer>();
            
            EditorUICallEventSetting.Initialization();
            
            Debug.Log($"Was create debug commander system");

            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            
            Debug.Log($"Save scene");
        }
    }
}
