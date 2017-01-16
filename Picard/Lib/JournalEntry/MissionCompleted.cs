using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MissionCompleted : EliteJournalEntry
    {
        public string Faction { get; internal set; }
        public string Name { get; internal set; }
        public int MissionID { get; internal set; }
        public string Commodity { get; internal set; }
        public string CommodityLocalised { get; internal set; }
        public int Count { get; internal set; }
        public string DestinationSystem { get; internal set; }
        public string DestinationStation { get; internal set; }
        public int Reward { get; internal set; }
        public IDictionary<string, int> Ingredients { get; internal set; }
        public IDictionary<string, int> CommodityReward { get; internal set; }

        public MissionCompleted(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
