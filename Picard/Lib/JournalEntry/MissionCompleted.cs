using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Picard.Lib.JournalEntry.EntryHelper;

namespace Picard.Lib.JournalEntry
{
    public class MissionCompleted : EliteJournalEntry
    {
        public string Faction;
        public string Name;
        public int MissionID;
        public string Commodity;
        public string CommodityLocalised;
        public int Count;
        public string DestinationSystem;
        public string DestinationStation;
        public int Reward;
        public IList<CommodityMapping> CommodityReward;
        public IDictionary<string, int> Ingredients;

        public MissionCompleted(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
