using System;
using System.Collections.Generic;
using Picard.Lib.JournalEntry;

namespace Picard.Lib
{
    public class EliteJournalMaterialHandler : EliteJournalHandler
    {
        public IDictionary<string, int> Deltas { get; protected set; }

        /// <summary>
        /// Translation table from Elite to Inara
        /// </summary>
        protected IDictionary<string, string> EliteMatsLookup;

        /// <summary>
        /// Commodities that we know are not materials
        /// </summary>
        protected IList<string> IgnoreCommodities;

        /// <summary>
        /// Costs of unlocking the Engineers
        /// </summary>
        protected IDictionary<string, IDictionary<string, int>> EngineerCostLookup;

        public EliteJournalMaterialHandler(
            IDictionary<string, string> EliteMatsLookup,
            IList<string> IgnoreCommodities,
            IDictionary<string, IDictionary<string, int>> EngineerCostLookup)
        {
            Deltas = new Dictionary<string, int>();
            this.EliteMatsLookup = EliteMatsLookup;
            this.IgnoreCommodities = IgnoreCommodities;
            this.EngineerCostLookup = EngineerCostLookup;
        }

        public EliteJournalMaterialHandler(
            IDictionary<string, int> deltas,
            IDictionary<string, string> EliteMatsLookup,
            IList<string> IgnoreCommodities,
            IDictionary<string, IDictionary<string, int>> EngineerCostLookup)
        {
            Deltas = deltas;
        }

        public IDictionary<string, int> FilterOnlyInaraMats(IDictionary<string, int> deltas, IDictionary<string, int> removed)
        {
            // TODO: This should really be looking at the data just pulled
            // from Inara
            var ret = new Dictionary<string, int>();

            foreach (var mat in deltas)
            {
                if (EliteMatsLookup.Values.Contains(mat.Key))
                {
                    ret.Add(mat.Key, mat.Value);
                }
                else if (!IgnoreCommodities.Contains(mat.Key))
                {
                    removed.Add(mat.Key, mat.Value);
                }
            }

            return ret;
        }

        /// <summary>
        /// Uses internal lookup table to translate an Elite: Dangerous material
        /// name into an Inara.cz material name
        /// </summary>
        /// <param name="eliteMat">The Elite material name</param>
        /// <returns>The Inara.cz material name</returns>
        public string TranslateMat(string eliteMat)
        {
            // Sometimes the case changes on these mat names in the log files,
            // so just force them all to lower case and look them up in our
            // look up table
            if (EliteMatsLookup.ContainsKey(eliteMat.ToLower()))
            {
                return EliteMatsLookup[eliteMat.ToLower()];
            }

            // TODO: This should prompt the user to enter the correct information and
            // potentially send it on back to me for inclusion in the official list
            return eliteMat;
        }

        /// <summary>
        /// Handle the case where materials are removed due to an engineer upgrade
        /// </summary>
        public override void Handle(EngineerCraft e)
        {
            foreach (var mat in e.Ingredients)
            {
                DeltaTools.AddMat(Deltas, TranslateMat(mat.Key), -mat.Value);
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
                Deltas = DeltaTools.Add(Deltas,
                    EngineerCostLookup[e.Engineer]);
            }
            // TODO: Log something if there is a new engineer
        }

        public override void Handle(MarketBuy e)
        {
            DeltaTools.AddMat(Deltas, TranslateMat(e.Type), e.Count);
        }

        public override void Handle(MarketSell e)
        {
            DeltaTools.AddMat(Deltas, TranslateMat(e.Type), -e.Count);
        }

        /// <summary>
        /// Handle the case where a Material is Collected in Space
        /// </summary>
        public override void Handle(MaterialCollected e)
        {
            DeltaTools.AddMat(Deltas, TranslateMat(e.Name), e.Count);
        }

        /// <summary>
        /// Handle the case where a material is discarded by the player
        /// </summary>
        public override void Handle(MaterialDiscarded e)
        {
            DeltaTools.AddMat(Deltas, TranslateMat(e.Name), -e.Count);
        }

        public override void Handle(MissionAccepted e)
        {
            if (e.Name.StartsWith("Mission_Delivery"))
                return;

            if(e.CommodityLocalised != null)
            {
                DeltaTools.AddMat(Deltas, e.CommodityLocalised, e.Count);
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
                    DeltaTools.AddMat(Deltas, TranslateMat(mat.Key), mat.Value);
                }
            }
            // If a "CommodityReward" property is set, it is commodities
            else if (e.CommodityReward != null)
            {
                // Handle gaining a commodity through completing a mission
                foreach (var mat in e.CommodityReward)
                {
                    DeltaTools.AddMat(Deltas, TranslateMat(mat.Key), mat.Value);
                }
            }

            if (e.CommodityLocalised != null)
            {
                // Handle losing a commodity as the result of completing
                // a mission
                DeltaTools.AddMat(Deltas, e.CommodityLocalised, -e.Count);
            }
        }

        public override void Handle(Synthesis e)
        {
            if (e.Materials == null) return;

            foreach(var mat in e.Materials)
            {
                DeltaTools.AddMat(Deltas, TranslateMat(mat.Key), -mat.Value);
            }
        }

        public override void HandleUnknown(EliteJournalEntry e)
        {
        }
    }
}
