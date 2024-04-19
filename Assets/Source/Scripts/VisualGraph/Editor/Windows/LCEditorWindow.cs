using System.IO;
using LevelsConstructor.New.Editor.Preference;
using LevelsConstructor.New.Editor.Utilities;
using LevelsConstructor.New.Editor.Utilities.Refactor.Graph;
using LevelsConstructor.New.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LevelsConstructor.New.Editor.Windows
{
    public class LcEditorWindow : EditorWindow
    {
        private LcGraphView _graphView;

        private readonly string _defaultFileName = "New Graph";

        private Toolbar _toolbar;

        private static TextField _fileNameTextField;
        private static Label _labelSpace;
        private static Label _labelFileName;
        private static Label _labelSelectConstructor;
        private static ToolbarMenu _toolbarMenu;
        private static Button _saveButton;
        private static Button _exportButton;
        private static Button _loadButton;
        private static Button _clearButton;
        private static Button _resetButton;
        private static Button _miniMapButton;
        private static Button _refreshButton;

        [MenuItem("Window/Constructor/Custom Graph")]
        public static void Open()
        {
            GetWindow<LcEditorWindow>("Custom Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        private void AddGraphView()
        {
            DataPreference preference = StaticDataPreference.GetTryDataPreference;
            InfoDataGraphPreference graph = preference.InfoDataGraphPreferences[0];
            preference.SelectGraph(graph);
            
            _graphView = new LcGraphView(graph.Data);
            rootVisualElement.Add(_graphView);
            _graphView.StretchToParentSize();
        }

        private void AddToolbar()
        {
            if(_toolbar != null)
                rootVisualElement.Remove(_toolbar);
            
            _toolbar = new Toolbar();

            CreateLabelSpace();
            CreateButtonRefresh();
            CreateLabelSelectConstructorGraph();
            CreateLabelFileName();
            CreateToolbarMenuSelectGraph();
            CreateLabelFileName();
            CreateTextFieldFileName();
            CreateButtonSave();
            CreateButtonLoad();
            CreateButtonClear();
            CreateButtonReset();
            CreateButtonMinimap();
            CreateButtonExport();
            
            _toolbar.Add(_labelSelectConstructor);
            _toolbar.Add(_toolbarMenu);
            _toolbar.Add(_refreshButton);
            _toolbar.Add(_labelSpace);
            _toolbar.Add(_labelFileName);
            _toolbar.Add(_fileNameTextField);
            _toolbar.Add(_saveButton);
            _toolbar.Add(_loadButton);
            _toolbar.Add(_clearButton);
            _toolbar.Add(_resetButton);
            _toolbar.Add(_miniMapButton);
            _toolbar.Add(_exportButton);
           

            LCStyleUtility.AddStyleSheets(_toolbar, "DialogueSystem/DSToolbarStyles.uss");

            rootVisualElement.Add(_toolbar);
        }

        private void CreateToolbarMenuSelectGraph()
        {
            _toolbarMenu = LCElementUtility.CreateToolbarMenu(StaticDataPreference.GetInfoDataGraphValueTuples(),
                selectElement =>
                {
                    StaticDataPreference.SelectToNameGraph(selectElement);
                    AddToolbar();
                });

        }
        private void CreateLabelSpace()
        {
            _labelSpace = LCElementUtility.CreateLabel("\t\t");
        }
        
        
        private void CreateLabelFileName()
        {
            _labelFileName = LCElementUtility.CreateLabel("File Name:");
        }
        private void CreateLabelSelectConstructorGraph()
        {
            _labelSelectConstructor = LCElementUtility.CreateLabel("Select Constructor:");
        }

        private void CreateTextFieldFileName()
        {
            _fileNameTextField = LCElementUtility.CreateTextField(_defaultFileName, "",
                callback =>
                {
                    _fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                });
        }

        private void CreateButtonSave()
        {
            _saveButton = LCElementUtility.CreateButton("Save", () => Save());
        }
        
        private void CreateButtonExport()
        {
            _exportButton = LCElementUtility.CreateButton("Export", () => { Debug.Log("Export");});
        }

        private void CreateButtonLoad()
        {
            _loadButton = LCElementUtility.CreateButton("Load", () => Load());
        }

        private void CreateButtonClear()
        {

            _clearButton = LCElementUtility.CreateButton("Clear", () => Clear());

        }
        
        private void CreateButtonRefresh()
        {

            _refreshButton = LCElementUtility.CreateButton("↻", () => Refresh());

        }

        private void CreateButtonReset()
        {
            _resetButton = LCElementUtility.CreateButton("Reset", () => ResetGraph());
        }

        private void CreateButtonMinimap()
        {
            _miniMapButton = LCElementUtility.CreateButton("Minimap", () => ToggleMiniMap());

        }


        private void AddStyles()
        {
            LCStyleUtility.AddStyleSheets(rootVisualElement, "DialogueSystem/DSVariables.uss");
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.",
                    "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            GraphIOUtility.Initialize(_graphView, _fileNameTextField.value);
            GraphIOUtility.Save();
        }

        private void Load()
        {
            string filePath =
                EditorUtility.OpenFilePanel("Constructors", PathConstants.PathConstructors, ".asset");
            
            Debug.Log(filePath);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();
            
            Debug.Log( Path.GetFileNameWithoutExtension(filePath));
            
            GraphIOUtility.Initialize(_graphView, Path.GetFileNameWithoutExtension(filePath));
            GraphIOUtility.Load();

            // LCIOUtility.Initialize(_graphView, Path.GetFileNameWithoutExtension(filePath));
            // LCIOUtility.Load();
        }

        private void Clear()
        {
            _graphView.ClearGraph();
        }
        
        private void Refresh()
        {
            AddToolbar();
        }

        private void ResetGraph()
        {
            Clear();

            UpdateFileName(_defaultFileName);
        }

        private void ToggleMiniMap()
        {
            _graphView.ToggleMiniMap();

            _miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
        }

        public static void UpdateFileName(string newFileName)
        {
            _fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }
    }
}