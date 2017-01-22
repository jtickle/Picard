///
/// Copyright 2017 Jeff Tickle "CMDR VirtualPaper"
/// 
/// This file is part of LibEDJournal.
///
/// LibEDJournal is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// LibEDJournal is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with LibEDJournal.  If not, see<http://www.gnu.org/licenses/>.
///

using LibEDJournal.Entry;

namespace LibEDJournal
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
        public abstract void Handle(ScientificResearch e);
        public abstract void Handle(Synthesis e);
    }
}
