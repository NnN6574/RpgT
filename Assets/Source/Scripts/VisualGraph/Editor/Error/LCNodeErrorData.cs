using System.Collections.Generic;
using LevelsConstructor.New.Editor.Elements;
using Source.Scripts.VisualGraph.Editor.Elements;

namespace LevelsConstructor.New.Editor.Error
{
    public class LCNodeErrorData
    {
        public LCErrorData ErrorData { get; set; }
        public List<LCNode> Nodes { get; set; }

        public LCNodeErrorData()
        {
            ErrorData = new LCErrorData();
            Nodes = new List<LCNode>();
        }
    }
}