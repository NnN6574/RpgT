using LevelsConstructor.New.Editor.Save;
using LevelsConstructor.New.Editor.Utilities;
using LevelsConstructor.New.Editor.Windows;
using LevelsConstructor.New.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Source.Scripts.VisualGraph.Editor.Elements.Variants
{
    public class LcSingleChoiceNode : LCNode
    {
        public override void Initialize(string nodeName, LcGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            ChoiceType = LCChoiceType.SingleChoice;

            LCChoiceSaveData choiceData = new LCChoiceSaveData()
            {
                Text = "New platform"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTAINER */

            foreach (LCChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
