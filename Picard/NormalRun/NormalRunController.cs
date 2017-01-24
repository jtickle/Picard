using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Picard.Lib;
using LibEDJournal;
using LibEDJournal.State;
using LibEDJournal.Handler;

namespace Picard.NormalRun
{
    class NormalRunController : IDisposable
    {
        protected InaraApi api;
        protected PersistentState state;
        protected EliteJournalParser logs;
        protected DataMangler dm;

        protected MatUpdateForm form;

        private InventorySet inaraCorrection = null;
        private InventorySet last = null;
        private InventorySet deltas = null;
        private InventorySet result = null;
        private InventorySet unknown = null;

        private InventorySet totalsLast = null;
        private InventorySet totalsDeltas = null;
        private InventorySet totalsResult = null;
        private InventorySet totalsCorrection = null;

        /// <summary>
        /// The type of each material, for accounting
        /// </summary>
        public IDictionary<string, string> MaterialTypeLookup;

        public IList<string> MaterialTypes;

        public IList<string> MaterialOrder;

        public NormalRunController(
            InaraApi api,
            PersistentState state,
            EliteJournalParser logs,
            DataMangler dm)
        {
            this.api = api;
            this.state = state;
            this.logs = logs;
            this.dm = dm;

            MaterialTypeLookup = dm.MaterialTypeLookup;

            MaterialTypes = new List<string>(dm.MaterialTypes);
            MaterialTypes.Add("DebugUnknown");
            MaterialTypes.Add("Grand");

            MaterialOrder = new List<string>(dm.MaterialOrder);
            foreach (var type in MaterialTypes)
            {
                MaterialOrder.Add(type + " Total");
            }

            form = new MatUpdateForm(MaterialOrder);
            form.ReloadMats += OnReloadMats;
            form.PostAndSave += OnPostAndSave;
            form.CloseWithoutSave += OnCloseWithoutSave;
        }

        private async Task<InventorySet> FigureOutInaraCorrection()
        {
            // Get current inventory as represented on Inara.cz
            var inaraMats = await api.GetMaterialsSheet();
            
            // If everything matches, there are no corrections
            if (last.ExactlyEquals(inaraMats))
            {
                return null;
            }

            // Look for new stuff on Inara
            var inaraChange = new HashSet<string>(inaraMats.Keys);
            inaraChange.ExceptWith(new HashSet<string>(last.Keys));

            // If Inara has new mats, cheat until we can get an update out
            if (inaraChange.Count > 0)
            {
                foreach(var mat in inaraMats.Keys)
                {
                    dm.EliteMatsLookup[mat.Replace(" ", "").ToLower()] = mat;
                    dm.MaterialTypeLookup[mat] = "Material";
                }
            }
            
            if(form.DoesUserWantInaraCorrection())
            {
                // Apply a correction to Picard's values
                return inaraMats - last;
            }
            else
            {
                // Quit without saving
                Application.Exit();
                return null;
            }
        }

        protected InventorySet ReinitializeTotal()
        {
            var totals = new InventorySet();
            foreach(var type in MaterialTypes)
            {
                totals[type] = 0;
            }
            return totals;
        }

        protected void ReinitializeTotals()
        {
            totalsLast = ReinitializeTotal();
            totalsCorrection = ReinitializeTotal();
            totalsDeltas = ReinitializeTotal();
            totalsResult = ReinitializeTotal();
        }

        private void AddToTotal(InventorySet totes, string key, int delta)
        {
            var t = MaterialTypeLookup.ContainsKey(key)
                ? MaterialTypeLookup[key]
                : "DebugUnknown";

            totes["Grand"] += delta;
            totes[t] += delta;
        }

        protected void UpdateFormWithCurrentMats()
        {
            int nLast = 0, nCorrection = 0, nDelta = 0, nResult = 0;
            string k;

            // Clear out the existing materials list
            form.ClearList();

            // Set all the totals to zero
            ReinitializeTotals();

            // Loop over result as it contains the sum of everything else
            foreach (var r in result)
            {
                // The name of the material
                k = r.Key;

                // Retrieve the "last" value
                nLast = last.GetMat(k);
                AddToTotal(totalsLast, k, nLast);

                // Retrieve the "correction" value
                if(inaraCorrection != null)
                {
                    nCorrection = inaraCorrection.GetMat(k);
                    AddToTotal(totalsCorrection, k, nCorrection);
                }

                // Retrieve the "delta" value
                nDelta = deltas.GetMat(k);
                AddToTotal(totalsDeltas, k, nDelta);

                // Retrieve the "result" value from foreach
                nResult = r.Value;
                AddToTotal(totalsResult, k, nResult);

                // Add the material to the graphical list
                form.AddMaterial(k, nLast, nCorrection, nDelta, nResult);
            }
            
            if (inaraCorrection == null)
            {
                // If there is no correction column, hide it
                form.ShowCorrectionColumn = false;
            }
            else
            {
                // If there is a correction column, show it
                form.ShowCorrectionColumn = true;
            }

            // Add our totals to the form
            UpdateFormWithCurrentTotals();
        }

        protected void UpdateFormWithCurrentTotals()
        {
            totalsDeltas["DebugUnknown"] = unknown.Count;

            foreach (var type in MaterialTypes)
            {
                form.AddMaterial(type + " Total",
                    totalsLast[type],
                    totalsCorrection[type],
                    totalsDeltas[type],
                    totalsResult[type]);
            }
        }

        protected async void OnReloadMats(object sender, EventArgs e)
        {
            // Indicate that we are loading something
            form.SetLoadingState();

            // Initialize empty InventorySet of unknown "materials"
            unknown = new InventorySet();

            // Initialize empty InventorySet of deltas from log files
            deltas = new InventorySet();

            // Get last run materials from Picard State
            last = state.CalculateCurrentInventory();

            // Apply and store any updates to the data format
            state.ApplyUpdates(last);

            // Get Inara corrections, if any
            inaraCorrection = await FigureOutInaraCorrection();

            // Parse logs and get the changes to material counts
            // The filtering function adds unrecognized materials to unknown list
            var MatHandler = new InventoryHandler(dm.EngineerCostLookup);
            MatHandler.InventoryChanged +=
                (object invSender, InventoryEventArgs ie) =>
                {
                    var mat = ie.Name.ToLower();
                    if (dm.EliteMatsLookup.ContainsKey(mat))
                    {
                        deltas.AddMat(mat, ie.Delta);
                    }
                    else if (!dm.IgnoreCommodities.Contains(mat))
                    {
                        unknown.AddMat(mat, ie.Delta);
                    }
                };

            var ChHandler = new CharacterHandler();
            ChHandler.CharacterDied +=
                (object chSender, DeathEventArgs de) =>
                {
                    Console.WriteLine("OHES NOES");
                    foreach (var comm in dm.MaterialTypeLookup)
                    {
                        if (comm.Value != "Commodities")
                            continue;

                        var c = comm.Key;

                        if (deltas.ContainsKey(c))
                        {
                            deltas[c] = 0;
                        }
                        if (last.ContainsKey(c))
                        {
                            deltas[c] = -last[c];
                        }
                    }
                };
            var handlers = new List<EliteJournalHandler>() {
                MatHandler, ChHandler };
            logs.HandleLogEntries(
                state.GetLastUpdateTimestamp(),
                handlers
                );
            /*deltas = dm.FilterAndTranslateMats(
                MatHandler.Deltas,
                    unknown);*/

            // Apply changes to material counts
            result = last + deltas;

            // If there is a correction, also add that to the mat counts
            if(inaraCorrection != null)
            {
                result = result + inaraCorrection;
            }

            // Add all of the data we found above to the form
            UpdateFormWithCurrentMats();

            // Hand control back to the user
            // If deltas are not empty or there is a corretion, we have stuff to save
            form.SetReadyState(
                !deltas.IsZero || inaraCorrection != null);

            // If there are unknown materials, tell the user the
            // bad news, and prevent the window close.
            if (unknown.Count > 0)
            {
                var ackForm = new UnrecognizedMaterials(unknown);
                ackForm.ShowDialog();
                form.SetBrokenState();
            }
        }

        protected void SaveDataAndClose()
        {
            // Show post status form to indicate we are doing something
            var post = new PostForm();
            post.Loading = true;
            post.Load += async (object sender, EventArgs args) =>
            {
                // If there was an Inara Correction, save that to history first
                if (inaraCorrection != null)
                {
                    state.AddHistory(inaraCorrection);
                }

                // If there were updates to post to Inara, post them
                if (!deltas.IsZero)
                {
                    await api.PostMaterialsSheet(result);

                    // Save to History
                    state.AddHistory(deltas);
                }

                // Update Post Timestamp
                state.UpdateLastPostToCurrent();

                // Persist the state
                state.Persist();

                // Update post status form to indicate we are finished
                post.Loading = false;
            };

            post.ShowDialog();
        }

        protected void OnPostAndSave(object sender, CancelEventArgs e)
        {
            // If there are unknown materials, tell the user the
            // bad news, and prevent the window close.
            if (unknown.Count > 0)
            {
                e.Cancel = true;
                var ackForm = new UnrecognizedMaterials(unknown);
                ackForm.ShowDialog();
            }

            // If there are negative values in the result dictionary,
            // something went wrong.  Notify the user and prevent the
            // window close.
            if(result.IsNegative)
            {
                form.ShowNegativeValueBadNews(state.StateFile);
                e.Cancel = true;
            }

            if (e.Cancel)
            {
                // If cancelling, don't save anything
                return;
            }
            else
            {
                // If not cancelling, do the save
                SaveDataAndClose();
            }
        }

        protected void OnCloseWithoutSave(object sender, CancelEventArgs e)
        {
            form.ShowCloseWithoutSave();
        }

        /// <summary>
        /// Compare current state to Inara values.  Parse Elite Logs and
        /// add to material counts.  Show a summary, and if the user is
        /// OK, post the new data to Inara.cz.
        /// </summary>
        public void Run()
        {
            form.ShowDialog();
        }

        public void Dispose()
        {
            form.Dispose();
        }
    }
}
