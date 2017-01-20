using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class Synthesis : EliteJournalEntry
    {
        public string Name;
        public IDictionary<string, int> Materials;

        public Synthesis(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
