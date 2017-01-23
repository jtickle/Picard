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

using Newtonsoft.Json.Linq;

namespace LibEDJournal.Entry
{
    /// <summary>
    /// This event is recorded when a player progresses or gains rank
    /// with an engineer.
    /// 
    /// Your Rank determines the Level of blueprints that you can get
    /// a particular Engineer to use to modify your ship.  You must go
    /// through a small progression before you will have a Rank.
    /// 
    /// Your progression with an Engineer begins with "Known".  Then you
    /// are "Invited" to visit.  If you come with the right gift, that
    /// engineer will then be "Unlocked" and you will gain a Rank of 1.
    /// 
    /// If Rank > 1, you are assumed to be "Unlocked" and the Progress
    /// field will be null.
    /// 
    /// If your progress is "Known" or "Invited", your rank will be 0,
    /// but this is represented as unset within the underlying json.
    /// 
    /// We assume that there won't be a journal entry where both Progress
    /// and Rank are unset.
    /// 
    /// In addition to tracking your progress with individual engineers,
    /// this information can be useful to determine changes in player
    /// inventory.  Engineers generally require a gift to unlock, and if
    /// so, this can be looked up to determined how the player's
    /// inventory changed.  This only happens in the event that Progress
    /// is "Unlocked" and Rank is 1, which is also the only times that
    /// both Progress and Rank are set to a value.
    /// </summary>
    public class EngineerProgress : EliteJournalEntry
    {
        /// <summary>
        /// The engineer with whom you have increased in rank
        /// </summary>
        public string Engineer;

        /// <summary>
        /// Can be "Known", "Invited", "Unlocked", or null.  See class
        /// description for more information.
        /// TODO: make an Enum.
        /// </summary>
        public string Progress;

        /// <summary>
        /// The new player rank with the engineer
        /// </summary>
        public int Rank;

        public EngineerProgress(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
