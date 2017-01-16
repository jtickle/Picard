using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picard.Lib;
using Picard.Lib.JournalEntry;

namespace MatVerifier
{
    class EliteJournalDebugHandler : EliteJournalHandler
    {
        public ISet<string> UnknownEvents { get; protected set; }
        public ISet<string> ConfirmedMaterials { get; protected set; }

        public EliteJournalDebugHandler()
        {
            UnknownEvents = new HashSet<string>();
            ConfirmedMaterials = new HashSet<string>();
        }

        public override void HandleUnknown(EliteJournalEntry e)
        {
            UnknownEvents.Add(e.EventName);
        }

        public override void Handle(EngineerCraft e)
        {
            Console.WriteLine("Found EngineerCraft: " + e.EventName);
        }

        public override void Handle(EngineerProgress e)
        {
            Console.WriteLine("Found EngineerProgress: " + e.EventName);
        }

        public override void Handle(MarketBuy e)
        {
            Console.WriteLine("Found MarketBuy: " + e.EventName);
        }

        public override void Handle(MarketSell e)
        {
            Console.WriteLine("Found MarketSell: " + e.EventName);
        }

        public override void Handle(MaterialCollected e)
        {
            Console.WriteLine("Found MaterialCollected: " + e.EventName);
        }

        public override void Handle(MaterialDiscarded e)
        {
            Console.WriteLine("Found MaterialDiscarded: " + e.EventName);
        }

        public override void Handle(MissionAccepted e)
        {
            Console.WriteLine("Found MissionAccepted: " + e.EventName);
        }

        public override void Handle(MissionCompleted e)
        {
            Console.WriteLine("Found MissionCompleted: " + e.EventName);
        }

        public override void Handle(Synthesis e)
        {
            Console.WriteLine("Found Synthesis: " + e.EventName);
        }
    }
}
