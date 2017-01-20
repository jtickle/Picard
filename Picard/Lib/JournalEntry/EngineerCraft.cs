using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class EngineerCraft : EliteJournalEntry
    {
        public string Engineer;
        public string Blueprint;
        public int Level;
        public IDictionary<string, int> Ingredients;

        public EngineerCraft(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
