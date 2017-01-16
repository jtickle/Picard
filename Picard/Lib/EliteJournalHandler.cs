using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picard.Lib.JournalEntry;

namespace Picard.Lib
{
    public abstract class EliteJournalHandler
    {
        public abstract void HandleUnknown(EliteJournalEntry e);
        public abstract void Handle(EngineerCraft e);
        public abstract void Handle(EngineerProgress e);
        public abstract void Handle(MarketBuy e);
        public abstract void Handle(MarketSell e);
        public abstract void Handle(MaterialCollected e);
        public abstract void Handle(MaterialDiscarded e);
        public abstract void Handle(MissionAccepted e);
        public abstract void Handle(MissionCompleted e);
        public abstract void Handle(Synthesis e);
    }
}
