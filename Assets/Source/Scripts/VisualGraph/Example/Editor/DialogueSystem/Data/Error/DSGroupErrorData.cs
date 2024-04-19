using System.Collections.Generic;

namespace DS.Data.Error
{
    using Elements;

    public class DSGroupErrorData
    {
        public LCErrorData ErrorData { get; set; }
        public List<DSGroup> Groups { get; set; }

        public DSGroupErrorData()
        {
            ErrorData = new LCErrorData();
            Groups = new List<DSGroup>();
        }
    }
}