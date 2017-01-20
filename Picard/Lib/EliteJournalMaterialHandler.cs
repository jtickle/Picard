using System;
using System.Collections.Generic;
using Picard.Lib.JournalEntry;

namespace Picard.Lib
{
    public class EliteJournalMaterialHandler : EliteJournalHandler
    {
        public IDictionary<string, int>
            Deltas { get; protected set; }

        public IDictionary<string, IList<EliteJournalEntry>>
            MatSeen { get; protected set; }

        public IList<string>
            UnknownEngineers { get; protected set; }

        /// <summary>
        /// Costs of unlocking the Engineers
        /// </summary>
        protected IDictionary<string, IDictionary<string, int>> 
            EngineerCostLookup;

        public EliteJournalMaterialHandler(
            IDictionary<string, IDictionary<string, int>>
                EngineerCostLookup)
        {
            Deltas = new Dictionary<string, int>();
            MatSeen = new Dictionary<string, IList<EliteJournalEntry>>();
            UnknownEngineers = new List<string>();

            this.EngineerCostLookup = EngineerCostLookup;
        }

        public EliteJournalMaterialHandler(
            IDictionary<string, int> deltas,
            IDictionary<string, IDictionary<string, int>>
                EngineerCostLookup)
        {
            Deltas = deltas;
            this.EngineerCostLookup = EngineerCostLookup;
        }

        protected void AddMatSeen(string mat, EliteJournalEntry entry)
        {
            if(!MatSeen.ContainsKey(mat))
            {
                MatSeen[mat] = new List<EliteJournalEntry>();
            }
            MatSeen[mat].Add(entry);
        }

        protected void AddMat(string mat, int count, EliteJournalEntry entry)
        {
            DeltaTools.AddMat(Deltas, mat, count);
            AddMatSeen(mat, entry);
        }

        protected void AddMats(
            IDictionary<string, int> mats,
            EliteJournalEntry entry,
            bool subtract = false)
        {
            foreach (var mat in mats)
            {
                AddMat(mat.Key,
                    (subtract ? -1 : 0) + mat.Value,
                    entry);
            }
        }

        /// <summary>
        /// Handle the case where materials are removed due to an engineer upgrade
        /// </summary>
        public override void Handle(EngineerCraft e)
        {
            foreach (var mat in e.Ingredients)
            {
                AddMat(mat.Key, -mat.Value, e);
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
            AddMat(e.Type, e.Count, e);
        }

        public override void Handle(MarketSell e)
        {
            AddMat(e.Type, -e.Count, e);
        }

        /// <summary>
        /// Handle the case where a Material is Collected in Space
        /// </summary>
        public override void Handle(MaterialCollected e)
        {
            AddMat(e.Name, e.Count, e);
        }

        /// <summary>
        /// Handle the case where a material is discarded by the player
        /// </summary>
        public override void Handle(MaterialDiscarded e)
        {
            AddMat(e.Name, -e.Count, e);
        }

        public override void Handle(MissionAccepted e)
        {
            if (e.Name != null && e.Name.StartsWith("Mission_Delivery"))
                return;
            
            if(e.CommodityLocalised != null)
            {
                AddMat(e.CommodityLocalised, e.Count, e);
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
                    AddMat(mat.Key, mat.Value, e);
                }
            }
            // If a "CommodityReward" property is set, it is commodities
            else if (e.CommodityReward != null)
            {
                // Handle gaining a commodity through completing a mission
                foreach (var mat in e.CommodityReward)
                {
                    AddMat(mat.Name, mat.Count, e);
                }
            }

            if (e.CommodityLocalised != null)
            {
                // Handle losing a commodity as the result of completing
                // a mission
                AddMat(e.CommodityLocalised, -e.Count, e);
            }
        }

        public override void Handle(Synthesis e)
        {
            if (e.Materials == null) return;

            foreach(var mat in e.Materials)
            {
                AddMat(mat.Key, -mat.Value, e);
            }
        }

        public override void HandleUnknown(EliteJournalEntry e)
        {
        }
    }
}
