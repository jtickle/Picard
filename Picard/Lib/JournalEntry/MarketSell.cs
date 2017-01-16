using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MarketSell : EliteJournalEntry
    {
        public string Type { get; internal set; }
        public int Count { get; internal set; }
        public int SellPrice { get; internal set; }
        public int TotalSale { get; internal set; }
        public int AvgPricePaid { get; internal set; }

        public MarketSell(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
