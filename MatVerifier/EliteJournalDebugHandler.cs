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

        public EliteJournalDebugHandler()
        {
            UnknownEvents = new HashSet<string>();
        }

        public override void HandleUnknown(EliteJournalEntry e)
        {
            UnknownEvents.Add(e.EventName);
        }

        public override void Handle(EngineerCraft e)
        {
        }

        public override void Handle(EngineerProgress e)
        {
        }

        public override void Handle(MarketBuy e)
        {
        }

        public override void Handle(MarketSell e)
        {
        }

        public override void Handle(MaterialCollected e)
        {
        }

        public override void Handle(MaterialDiscarded e)
        {
        }

        public override void Handle(MissionAccepted e)
        {
        }

        public override void Handle(MissionCompleted e)
        {
        }

        public override void Handle(Synthesis e)
        {
        }
    }
}
