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
    /// This event is recorded when the player discards a material in
    /// open space.
    /// 
    /// A player ejects a material from their cargo hold, which removes
    /// a count of material from their inventory.
    /// </summary>
    public class MaterialDiscarded : EliteJournalEntry
    {
        /// <summary>
        /// Either "Encoded" or "Manufactured"
        /// TODO: Enum
        /// TODO: I have no idea what this means
        /// </summary>
        public string Category;

        /// <summary>
        /// The material name
        /// </summary>
        public string Name;

        /// <summary>
        /// The number of materials discarded
        /// </summary>
        public int Count;

        public MaterialDiscarded(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
