using LevelsConstructor.New.Editor.Utilities.Refactor;
using LevelsConstructor.New.Editor.Utilities.Refactor.Graph;
using UnityEditor;
using UnityEngine;

namespace LevelsConstructor.New.Editor.Preference
{
    public class InfoDataGraphPreferenceSoFactory
    {
        public InfoDataGraphPreferenceSO CreateGraph(string name, string path)
        {
            AutoFolderIOUtility.AutoFolder(path);
            var so = ScriptableObject.CreateInstance<InfoDataGraphPreferenceSO>();
            AssetDatabase.CreateAsset(so, path+$"/{name}.asset");
            return so;
        }
    }
}