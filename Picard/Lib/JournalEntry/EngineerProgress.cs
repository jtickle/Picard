using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class EngineerProgress : EliteJournalEntry
    {
        public string Engineer { get; internal set; }
        public string Progress { get; internal set; }
        public int Rank { get; internal set; }

        public EngineerProgress(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
