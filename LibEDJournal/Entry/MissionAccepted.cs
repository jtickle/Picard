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

using System;
using Newtonsoft.Json.Linq;

namespace LibEDJournal.Entry
{
    /// <summary>
    /// This event is recorded when the player accepts a mission or
    /// passenger.
    /// 
    /// TODO: Break up into additional subclasses
    /// 
    /// TODO: Further research on functionality
    /// 
    /// We assume that if the mission Name is "Mission_Delivery", the
    /// commodities collected are never materials for engineering.
    /// 
    /// </summary>
    public class MissionAccepted : EliteJournalEntry
    {
        /// <summary>
        /// The faction for which you are performing a mission
        /// </summary>
        public string Faction;

        /// <summary>
        /// The name of the mission type
        /// TODO: Collect all the possibilities
        /// </summary>
        public string Name;

        /// <summary>
        /// An internal localization name for the commodity involved
        /// </summary>
        public string Commodity;

        /// <summary>
        /// The localized name for the commodity involved
        /// TODO: see if this causes any problems for our own localization
        /// </summary>
        public string CommodityLocalised;

        /// <summary>
        /// The mass in tons of commodities involved
        /// </summary>
        public int Count;

        /// <summary>
        /// The destionation system of the mission
        /// </summary>
        public string DestinationSystem;

        /// <summary>
        /// The destionation station of the mission
        /// We assume this to be within DestionationSysten
        /// </summary>
        public string DestinationStation;

        /// <summary>
        /// When the mission expires
        /// </summary>
        public DateTime Expiry;

        /// <summary>
        /// How many passengers involved
        /// </summary>
        public int PassengerCount;

        /// <summary>
        /// Are passengers VIPs, which makes it more likely that someone
        /// will try to interdict and destroy you
        /// </summary>
        public bool PassengerVIPs;

        /// <summary>
        /// Are passengers Wanted, which makes travel and docking
        /// difficult
        /// </summary>
        public bool PassengerWanted;

        /// <summary>
        /// The generic type of passneger which seems to affect the
        /// messages that they send you
        /// </summary>
        public string PassengerType;

        /// <summary>
        /// A unique mission ID that we assume can be tracked to either a
        /// MissionAbandoned, MissionCompleted, or MissionFailed.
        /// </summary>
        public int MissionID;

        public MissionAccepted(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
