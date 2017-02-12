using System;
using System.Collections.Generic;
using LibEDJournal.State;

namespace Picard.Lib
{
    using HistorySet = Dictionary<DateTime, InventorySet>;
    using IHistorySet = IDictionary<DateTime, InventorySet>;

    public class State
    {
        public string EliteCmdrName = "";
        public string InaraCmdrName = "";
        public string InaraU = "";
        public string InaraP = "";
        public int DataVersion = 0;
        public IHistorySet History;
        public DateTime LastPost;

        public State()
        {
            History = new HistorySet();
        }
    }
}
