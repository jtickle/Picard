using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picard
{
    using HistorySet = Dictionary<DateTime, IDictionary<string, int>>;
    using IHistorySet = IDictionary<DateTime, IDictionary<string, int>>;

    public class State
    {
        public string CmdrName = "";
        public string InaraU = "";
        public string InaraP = "";
        public IHistorySet History;

        public State()
        {
            History = new HistorySet();
        }
    }
}
