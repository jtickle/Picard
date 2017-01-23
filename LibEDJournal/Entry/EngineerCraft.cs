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

namespace LibEDJournal.Entry
{

    /// <summary>
    /// This event is recorded when an engineer crafts a modification.
    /// 
    /// When this happens, certain player materials are used with a
    /// blueprint of a certain level in order to modify ship hardware.
    /// </summary>
    public class EngineerCraft : EliteJournalEntry
    {
        /// <summary>
        /// The engineer that did the crafting
        /// </summary>
        public string Engineer;

        /// <summary>
        /// The blueprint that was used
        /// </summary>
        public string Blueprint;

        /// <summary>
        /// The level of the blueprint
        /// </summary>
        public int Level;

        /// <summary>
        /// The ingredients that were consumed
        /// </summary>
        public IDictionary<string, int> Ingredients;
        
        public EngineerCraft(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
