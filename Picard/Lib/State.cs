using System;
using System.Collections.Generic;

namespace Picard.Lib
{
    using HistorySet = Dictionary<DateTime, IDictionary<string, int>>;
    using IHistorySet = IDictionary<DateTime, IDictionary<string, int>>;

    public class State
    {
        public string CmdrName = "";
        public string InaraU = "";
        public string InaraP = "";
        public int DataVersion = 0;
        public IHistorySet History;

        public State()
        {
            History = new HistorySet();
        }
    }
}
