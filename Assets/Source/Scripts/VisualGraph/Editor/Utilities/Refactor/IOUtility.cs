using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Utilities.Refactor
{
    public class IOUtility
    {
        protected static void CreateFolder(string parentFolderPath, string newFolderName)
        {
            Debug.Log(parentFolderPath+"/"+newFolderName);
            if (AssetDatabase.IsValidFolder($"{parentFolderPath}/{newFolderName}"))
            {
                return;
            }
            AssetDatabase.CreateFolder(parentFolderPath, newFolderName);
        }
        
        protected static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }
        
        protected static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        
        protected static T LoadAsset<T>(string path) where T : ScriptableObject
        {
            string fullPath = $"{path}.asset";
            
            Debug.Log(fullPath);

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        
        protected static void SaveAsset(Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        protected static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }
        
        protected static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }
    }
}