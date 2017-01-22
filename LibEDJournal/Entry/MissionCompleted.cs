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

using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using LibEDJournal.Entry.Helper;

namespace LibEDJournal.Entry
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
