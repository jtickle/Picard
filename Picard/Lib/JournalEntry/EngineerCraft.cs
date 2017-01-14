using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class EngineerCraft : EliteJournalEntry
    {
        public string Engineer { get; internal set; }
        public string Blueprint { get; internal set; }
        public int Level { get; internal set; }
        public IDictionary<string, int> Ingredients { get; internal set; }

        public EngineerCraft(JObject json) : base(json) { }
    }
}
