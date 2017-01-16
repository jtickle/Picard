using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MarketBuy : EliteJournalEntry
    {
        public string Type { get; internal set; }
        public int Count { get; internal set; }
        public int BuyPrice { get; internal set; }
        public int TotalCost { get; internal set; }

        public MarketBuy(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
