using System.Collections.Generic;

namespace DS.Data.Error
{
    using Elements;

    public class DSNodeErrorData
    {
        public LCErrorData ErrorData { get; set; }
        public List<DSNode> Nodes { get; set; }

        public DSNodeErrorData()
        {
            ErrorData = new LCErrorData();
            Nodes = new List<DSNode>();
        }
    }
}