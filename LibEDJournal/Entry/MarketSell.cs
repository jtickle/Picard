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
    /// This event is recroded when the player sells an item on the
    /// commodities market.
    /// 
    /// When this happens, a player gains Count * SellPrice = TotalSale
    /// money and loses Count commodities named Type, freeing up Count
    /// tons in their inventory.
    /// 
    /// Note: if you keep track of game state in terms of player
    /// location, you can use this to capture accurate data about the
    /// market in an area.  However, this is only useful for items that
    /// a player has purchased.  You get no information about what is
    /// available on the market in general.
    /// </summary>
    public class MarketSell : EliteJournalEntry
    {
        /// <summary>
        /// The commodity sold
        /// </summary>
        public string Type;

        /// <summary>
        /// How many tons of commodity were transacted
        /// </summary>
        public int Count;

        /// <summary>
        /// The per-unit cost of the commodity
        /// </summary>
        public int SellPrice;

        /// <summary>
        /// The gross profit from the transaction
        /// </summary>
        public int TotalSale;

        /// <summary>
        /// TODO: I do not know exactly what this is.
        /// </summary>
        public int AvgPricePaid;

        public MarketSell(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}