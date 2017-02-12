
using System;
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
using LibEDJournal.Entry;
using LibEDJournal.State;

namespace LibEDJournal.Handler
{
    public delegate void InventoryChangedHandler(object sender,
        InventoryEventArgs e);

    public class InventoryHandler : EliteJournalHandler
    {
        public string CurrentCmdr = "";
        public IList<string>
            UnknownEngineers { get; protected set; }

        public event InventoryChangedHandler InventoryChanged;

        /// <summary>
        /// Costs of unlocking the Engineers
        /// </summary>
        protected IDictionary<string, InventorySet> 
            EngineerCostLookup;

        public InventoryHandler(
            IDictionary<string, InventorySet>
                EngineerCostLookup)
        {
            UnknownEngineers = new List<string>();
            this.EngineerCostLookup = EngineerCostLookup;
        }

        protected void NotifyInventory(string mat, int delta, EliteJournalEntry entry)
        {
            // Notify any watchers
            if (InventoryChanged != null)
            {
                var e = new InventoryEventArgs(CurrentCmdr, mat, delta, entry);
                InventoryChanged(this, e);
            }
        }

        protected void AddMats(
            InventorySet mats,
            EliteJournalEntry entry,
            bool subtract = false)
        {
            foreach (var mat in mats)
            {
                NotifyInventory(mat.Key,
                    (subtract ? -1 : 0) + mat.Value,
                    entry);
            }
        }

        public override void Handle(Died e)
        {
        }

        /// <summary>
        /// Handle the case where materials are removed due to an engineer upgrade
        /// </summary>
        public override void Handle(EngineerCraft e)
        {
            foreach (var mat in e.Ingredients)
            {
                NotifyInventory(mat.Key, -mat.Value, e);
            }
        }

        public override void Handle(EngineerProgress e)
        {
            // We are only interested in the engineer's unlock event which
            // is signified by Progress=Unlocked and Rank=1.
            if (e.Progress == null ||
                e.Progress != "Unlocked" ||
                e.Rank != 1)
                return;
            
            // The logs do not provide the actual materials or commodities
            // that are relieved of you buy the engineers.  We look these values
            // up in a table.
            if (EngineerCostLookup.ContainsKey(e.Engineer))
            {
                AddMats(EngineerCostLookup[e.Engineer], e, true);
            }
            else
            {
                UnknownEngineers.Add(e.Engineer);
            }
        }

        public override void Handle(MarketBuy e)
        {
            NotifyInventory(e.Type, e.Count, e);
        }

        public override void Handle(MarketSell e)
        {
            NotifyInventory(e.Type, -e.Count, e);
        }

        /// <summary>
        /// Handle the case where a Material is Collected in Space
        /// </summary>
        public override void Handle(MaterialCollected e)
        {
            NotifyInventory(e.Name, e.Count, e);
        }

        /// <summary>
        /// Handle the case where a material is discarded by the player
        /// </summary>
        public override void Handle(MaterialDiscarded e)
        {
            NotifyInventory(e.Name, -e.Count, e);
        }

        public override void Handle(MissionAccepted e)
        {
            if (e.Name != null && e.Name.StartsWith("Mission_Delivery"))
                return;
            
            if(e.CommodityLocalised != null)
            {
                NotifyInventory(e.CommodityLocalised, e.Count, e);
            }
        }

        /// <summary>
        /// Handle the case where a Material is the reward of completing a mission
        /// Also, handle the very annoying case where a Commodity is consumed in the
        /// completion of a mission
        /// </summary>
        /// <param name="e"></param>
        public override void Handle(MissionCompleted e)
        {
            // If an "Ingredients" property is set, it is materials or data
            if (e.Ingredients != null)
            {
                // Handle gaining a material or data through completing a mission
                foreach (var mat in e.Ingredients)
                {
                    NotifyInventory(mat.Key, mat.Value, e);
                }
            }
            // If a "CommodityReward" property is set, it is commodities
            else if (e.CommodityReward != null)
            {
                // Handle gaining a commodity through completing a mission
                foreach (var mat in e.CommodityReward)
                {
                    NotifyInventory(mat.Name, mat.Count, e);
                }
            }

            if (e.CommodityLocalised != null)
            {
                // Handle losing a commodity as the result of completing
                // a mission
                NotifyInventory(e.CommodityLocalised, -e.Count, e);
            }
        }

        public override void Handle(Resurrect e)
        {
        }

        public override void Handle(ScientificResearch e)
        {
            NotifyInventory(e.Name, -e.Count, e);
        }

        public override void Handle(Synthesis e)
        {
            if (e.Materials == null) return;

            foreach(var mat in e.Materials)
            {
                NotifyInventory(mat.Key, -mat.Value, e);
            }
        }

        public override void Handle(LoadGame e)
        {
            CurrentCmdr = e.Commander;
        }

        public override void Handle(Unknown e)
        {
        }
    }
}
