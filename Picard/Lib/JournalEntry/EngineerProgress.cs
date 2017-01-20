using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class EngineerProgress : EliteJournalEntry
    {
        public string Engineer;
        public string Progress;
        public int Rank;

        public EngineerProgress(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
