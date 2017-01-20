using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MaterialDiscarded : EliteJournalEntry
    {
        public string Category;
        public string Name;
        public int Count;

        public MaterialDiscarded(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
