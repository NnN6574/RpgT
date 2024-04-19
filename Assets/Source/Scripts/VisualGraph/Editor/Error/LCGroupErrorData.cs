using System.Collections.Generic;
using LevelsConstructor.New.Editor.Elements;

namespace LevelsConstructor.New.Editor.Error
{
    public class LCGroupErrorData
    {
        public LCErrorData ErrorData { get; set; }
        public List<LCGroup> Groups { get; set; }

        public LCGroupErrorData()
        {
            ErrorData = new LCErrorData();
            Groups = new List<LCGroup>();
        }
    }
}