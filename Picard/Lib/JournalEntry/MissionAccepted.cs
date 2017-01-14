using System;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MissionAccepted : EliteJournalEntry
    {
        public string Faction { get; internal set; }
        public string Name { get; internal set; }
        public string Commodity { get; internal set; }
        public string CommodityLocalised { get; internal set; }
        public int Count { get; internal set; }
        public string DestinationSystem { get; internal set; }
        public string DestinationStation { get; internal set; }
        public DateTime Expiry { get; internal set; }
        public int PassengerCount { get; internal set; }
        public bool PassengerVIPs { get; internal set; }
        public bool PassengerWanted { get; internal set; }
        public string PassengerType { get; internal set; }
        public int MissionID { get; internal set; }

        public MissionAccepted(JObject json) : base(json) { }
    }
}
