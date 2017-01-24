using System;
using System.Collections.Generic;
using LibEDJournal;
using LibEDJournal.Entry;

namespace MatVerifier
{
    class EliteJournalDebugHandler : EliteJournalHandler
    {
        public ISet<string> UnknownEvents { get; protected set; }

        public EliteJournalDebugHandler()
        {
            UnknownEvents = new HashSet<string>();
        }

        public override void Handle(Unknown e)
        {
            UnknownEvents.Add(e.EventName);
        }

        public override void Handle(Died e)
        {
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

        public override void Handle(Resurrect e)
        {
        }

        public override void Handle(ScientificResearch e)
        {
        }

        public override void Handle(Synthesis e)
        {
        }
    }
}
