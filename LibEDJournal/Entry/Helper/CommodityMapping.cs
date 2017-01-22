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

namespace LibEDJournal.Entry.Helper
{
    /// <summary>
    /// In some cases, a set of inventory, rather than being stored as a
    /// dictionary of material name to value, is stored as a list of
    /// objects with a name and count.  This is a helper for those cases.
    /// 
    /// In the Parser of LibEDJournal, we wish to keep our data
    /// structures as close to the stored JSON as possible.  For a more
    /// consistent representation of these kinds of things, use a layer
    /// on top of LibEDJournal to translate.
    /// </summary>
    public class CommodityMapping
    {
        /// <summary>
        /// The in-game name of the inventory item
        /// </summary>
        public string Name;

        /// <summary>
        /// The number of this item involved in the current journal event
        /// </summary>
        public int Count;
    }
}
