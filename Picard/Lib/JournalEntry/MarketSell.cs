using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MarketSell : EliteJournalEntry
    {
        public string Type;
        public int Count;
        public int SellPrice;
        public int TotalSale;
        public int AvgPricePaid;

        public MarketSell(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
