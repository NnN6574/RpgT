namespace LevelsConstructor.New.Editor.Utilities.Refactor.Graph
{
    public abstract class AutoFolderIOUtility :IOUtility
    {
        public static void CreateDefaultFolders()
        {
            CreateFolder(PathConstants.FolderAssets, PathConstants.FolderEditor);
            CreateFolder(PathConstants.PathEditor, PathConstants.FolderRoot);
            CreateFolder(PathConstants.PathRoot, PathConstants.FolderPreferences);
            CreateFolder(PathConstants.PathRoot, PathConstants.FolderConstructors);
        }

        public static void AutoFolder(string path)
        {
            var folders = path.Split("/");
            var fullPath = folders[0];
            
            for (var index = 1; index < folders.Length; index++)
            {
                var folder = folders[index];
                CreateFolder(fullPath, folder);
                fullPath += $"/{folder}";
            }
        }
    }
}