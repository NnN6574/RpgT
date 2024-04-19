using System;
using System.Collections.Generic;
using System.Linq;
using LevelsConstructor.New.Editor.Elements;
using LevelsConstructor.New.Editor.Save;
using LevelsConstructor.New.Editor.Utilities;
using LevelsConstructor.New.Editor.Windows;
using LevelsConstructor.New.Enumerations;
using LevelsConstructor.New.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Scripts.VisualGraph.Editor.Elements
{
    public class LCNode : Node
    {
        public Guid ID { get; set; }

        public GameObject GameObject { get; set; }
        public string NodeName { get; set; }
        public List<LCChoiceSaveData> Choices { get; set; }
        //public string Text { get; set; }
        public LCChoiceType ChoiceType { get; set; }
        public LCGroup Group { get; set; }

        protected LcGraphView GraphView;
        private Color _defaultBackgroundColor;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(evt);
        }

        public virtual void Initialize(string nodeName, LcGraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid();

            NodeName = nodeName;
            Choices = new List<LCChoiceSaveData>();
            //Text = "Test text.";

            SetPosition(new Rect(position, Vector2.one * 50));

            GraphView = dsGraphView;
            _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */

            TextField dialogueNameTextField = LCElementUtility.CreateTextField(NodeName, null, callback =>
            {
                TextField target = (TextField) callback.target;

                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (Group == null)
                {
                    GraphView.RemoveUngroupedNode(this);

                    NodeName = target.value;

                    GraphView.AddUngroupedNode(this);

                    return;
                }

                LCGroup currentGroup = Group;

                GraphView.RemoveGroupedNode(this, Group);

                NodeName = target.value;

                GraphView.AddGroupedNode(this, currentGroup);
            });

            LCStyleUtility.AddClasses(dialogueNameTextField, "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );

            titleContainer.Insert(0, dialogueNameTextField);

            /* INPUT CONTAINER */

            Port inputPort = this.CreatePort("Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            inputContainer.Add(inputPort);

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            ObjectField objectField = LCElementUtility.CreateObjectField(GameObject);
           
            LCStyleUtility.AddClasses(objectField, "ds-node__text-field",
                "ds-node__quote-text-field"
            );
            
            customDataContainer.Add(objectField);
          
            
           // Foldout textFoldout = LCElementUtility.CreateFoldout("Dialogue Text");

           // TextField textTextField = LCElementUtility.CreateTextArea(Text, null, callback => Text = callback.newValue);

            // LCStyleUtility.AddClasses(textTextField, "ds-node__text-field",
            //     "ds-node__quote-text-field"
            // );
            //
            // textFoldout.Add(textTextField);

           // customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (VisualElement visualElement in container.Children())
            {
                Port port = visualElement as Port;
                if (port is not {connected: true}) continue;
                GraphView.DeleteElements(port.connections);
            }
        }

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();

            return !inputPort.connected;
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultBackgroundColor;
        }
    }
}