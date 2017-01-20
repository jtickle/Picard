using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MarketBuy : EliteJournalEntry
    {
        public string Type;
        public int Count;
        public int BuyPrice;
        public int TotalCost;

        public MarketBuy(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
