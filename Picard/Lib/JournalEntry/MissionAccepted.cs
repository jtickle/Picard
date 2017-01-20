using System;
using Newtonsoft.Json.Linq;

namespace Picard.Lib.JournalEntry
{
    public class MissionAccepted : EliteJournalEntry
    {
        public string Faction;
        public string Name;
        public string Commodity;
        public string CommodityLocalised;
        public int Count;
        public string DestinationSystem;
        public string DestinationStation;
        public DateTime Expiry;
        public int PassengerCount;
        public bool PassengerVIPs;
        public bool PassengerWanted;
        public string PassengerType;
        public int MissionID;

        public MissionAccepted(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
