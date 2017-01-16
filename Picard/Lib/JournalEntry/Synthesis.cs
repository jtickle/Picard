using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class Synthesis : EliteJournalEntry
    {
        public string Name { get; internal set; }
        public IDictionary<string, int> Materials { get; internal set; }

        public Synthesis(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
