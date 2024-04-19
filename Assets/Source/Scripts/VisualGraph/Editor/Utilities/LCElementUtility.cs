using System;
using System.Collections.Generic;
using System.Linq;
using LevelsConstructor.New.Editor.Elements;
using Source.Scripts.VisualGraph.Editor.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace LevelsConstructor.New.Editor.Utilities
{
    public static class LCElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Label CreateLabel(string text)
        {
            Label label = new Label
            {
                text = text,
                enableRichText = true,
            };

            return label;
        }


        public static Port CreatePort(this LCNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }
        
        public static ObjectField CreateObjectField<T>(T value, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null) where T: Object
        {
            ObjectField objectField = new ObjectField
            {
                objectType = typeof(T),
                value = value,
            };
 
            objectField.RegisterValueChangedCallback(v =>
            {
               value = objectField.value as T;
            });
            
            return objectField;
        }
        
        public static ToolbarMenu CreateToolbarMenu(List<(string label, bool isState)> values, Action<string> OnValueChanged = null)
        {
            string nameToolbar = "Select custom graph";
            ToolbarMenu toolbarMenu = new ToolbarMenu();
            foreach (var value in values)
            {
                if (value.isState) nameToolbar = value.label;
                toolbarMenu.menu.AppendAction(value.label,
                    dropdownMenuAction =>
                    {
                        OnValueChanged?.Invoke(dropdownMenuAction.name);
                    }
                    , status=> value.isState ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
            }
            
            toolbarMenu.text = nameToolbar;

            return toolbarMenu;
        }
        
       

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }
    }
}