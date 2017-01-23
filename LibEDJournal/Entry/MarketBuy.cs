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
    /// This event is recorded when the player buys an item from the
    /// commodities market.
    /// 
    /// When this happens, a player loses Count * BuyPrice = TotalCost
    /// money and gains Count commodities named Type, consuming Count
    /// tons in their inventory.
    /// 
    /// Note: if you keep track of game state in terms of player
    /// location, you can use this to capture accurate data about the
    /// market in an area.  However, this is only useful for items that
    /// a player has purchased.  You get no information about what is
    /// available on the market in general.
    /// </summary>
    public class MarketBuy : EliteJournalEntry
    {
        /// <summary>
        /// The commodity purchased
        /// </summary>
        public string Type;

        /// <summary>
        /// How many tons of commodity were transacted
        /// </summary>
        public int Count;

        /// <summary>
        /// The per-unit cost of the commodity
        /// </summary>
        public int BuyPrice;

        /// <summary>
        /// The total cost of the transaction
        /// </summary>
        public int TotalCost;

        public MarketBuy(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
