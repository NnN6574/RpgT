using LevelsConstructor.New.Editor.Save;
using LevelsConstructor.New.Editor.Utilities;
using LevelsConstructor.New.Editor.Windows;
using LevelsConstructor.New.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Source.Scripts.VisualGraph.Editor.Elements.Variants
{
    public class LcMultipleChoiceNode : LCNode
    {
        public override void Initialize(string nodeName, LcGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            ChoiceType = LCChoiceType.MultipleChoice;

            LCChoiceSaveData choiceData = new LCChoiceSaveData()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */

            Button addChoiceButton = LCElementUtility.CreateButton("Add Choice", () =>
            {
                LCChoiceSaveData choiceData = new LCChoiceSaveData()
                {
                    Text = "New Choice"
                };

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT CONTAINER */

            foreach (LCChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            LCChoiceSaveData choiceData = (LCChoiceSaveData) userData;

            Button deleteChoiceButton = LCElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1) return;
                if (choicePort.connected) GraphView.DeleteElements(choicePort.connections);

                Choices.Remove(choiceData);
                GraphView.RemoveElement(choicePort);
            });

            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = LCElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            LCStyleUtility.AddClasses(choiceTextField, "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }
    }
}