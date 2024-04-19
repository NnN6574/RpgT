namespace LevelsConstructor.New.Editor
{
    public struct PathConstants
    {
        private const string Distribution = "/";
        
        public const string FolderAssets = "Assets";
        public const string FolderEditor = "Editor";
        public const string FolderRoot = "CustomGraphs";
        public const string FolderGraphs = "Graphs";
        public const string FolderGroups = "Groups"; 
        public const string FolderData = "Data"; 
        public const string FolderGlobal = "Global";
        public const string FolderPreferences = "Preferences";
        public const string FolderConstructors = "Constructors";


        public const string PathAssets = FolderAssets;
        public const string PathEditor = PathAssets +Distribution+ FolderEditor;
        public const string PathRoot = PathEditor +Distribution+ FolderRoot;
        public const string PathPreferences = PathRoot +Distribution+ FolderPreferences;
        public const string PathConstructors = PathRoot +Distribution+ FolderConstructors;
        //public const string PathGraphs = PathRoot +Distribution+ FolderGraphs;
    }
}