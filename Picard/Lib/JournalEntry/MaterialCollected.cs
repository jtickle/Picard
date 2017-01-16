using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MaterialCollected : EliteJournalEntry
    {
        public string Category { get; internal set; }
        public string Name { get; internal set; }
        public int Count { get; internal set; }

        public MaterialCollected(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
