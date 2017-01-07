using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Picard.Lib;

namespace Picard.NormalRun
{
    class NormalRunController : IDisposable
    {
        protected InaraApi api;
        protected PersistentState state;
        protected EliteLogs logs;

        protected MatUpdateForm form;

        private IDictionary<string, int> inaraCorrection = null;
        private IDictionary<string, int> last = null;
        private IDictionary<string, int> deltas = null;
        private IDictionary<string, int> result = null;
        private IDictionary<string, int> unknown = null;

        private IDictionary<string, int> totalsLast = null;
        private IDictionary<string, int> totalsDeltas = null;
        private IDictionary<string, int> totalsResult = null;
        private IDictionary<string, int> totalsCorrection = null;

        /// <summary>
        /// The type of each material, for accounting
        /// </summary>
        public IDictionary<string, string> MaterialTypeLookup;

        public IList<string> MaterialTypes;

        public NormalRunController(
            InaraApi api,
            PersistentState state,
            EliteLogs logs,
            IDictionary<string, string> MaterialTypeLookup,
            IList<string> MaterialTypes)
        {
            this.api = api;
            this.state = state;
            this.logs = logs;
            this.MaterialTypeLookup = MaterialTypeLookup;
            this.MaterialTypes = new List<string>(MaterialTypes);

            this.MaterialTypes.Add("DebugUnknown");
            this.MaterialTypes.Add("Grand");

            form = new MatUpdateForm();
            form.ReloadMats += OnReloadMats;
            form.PostAndSave += OnPostAndSave;
            form.CloseWithoutSave += OnCloseWithoutSave;
        }

        private async Task<IDictionary<string, int>> FigureOutInaraCorrection()
        {
            // Get current inventory as represented on Inara.cz
            var inaraMats = await api.GetMaterialsSheet();
            
            // If everything matches, there are no corrections
            if (DeltaTools.IsExactlyEqual(last, inaraMats))
            {
                return null;
            }

            if(form.DoesUserWantInaraCorrection())
            {
                // Apply a correction to Picard's values
                return DeltaTools.Subtract(inaraMats, last);
            }
            else
            {
                // Do not apply a correction to Picard's values (probably bad)
                return null;
            }
        }

        protected IDictionary<string, int> ReinitializeTotal()
        {
            var totals = new Dictionary<string, int>();
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

        /// <summary>
        /// Get the value of a material if set, or zero otherwise
        /// </summary>
        /// <param name="Key">The material name to look for</param>
        /// <param name="dict">The dictionary to search</param>
        /// <returns></returns>
        protected int GetMatValue(string Key, IDictionary<string, int> dict)
        {
            return dict != null && dict.ContainsKey(Key)
                ? dict[Key]
                : 0;
        }

        private void AddToTotal(IDictionary<string, int> totes, string key, int delta)
        {
            var t = MaterialTypeLookup.ContainsKey(key)
                ? MaterialTypeLookup[key]
                : "DebugUnknown";

            totes["Grand"] += delta;
            totes[t] += delta;
        }

        protected void UpdateFormWithCurrentMats()
        {
            int nLast, nCorrection, nDelta, nResult;
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
                nLast = GetMatValue(k, last);
                AddToTotal(totalsLast, k, nLast);

                // Retrieve the "correction" value
                nCorrection = GetMatValue(k, inaraCorrection);
                AddToTotal(totalsCorrection, k, nCorrection);

                // Retrieve the "delta" value
                nDelta = GetMatValue(k, deltas);
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

            // Initialize empty dictionary of unknown "materials"
            unknown = new Dictionary<string, int>();

            // Get last run materials from Picard State
            last = state.CalculateCurrentInventory();

            // Get Inara corrections, if any
            inaraCorrection = await FigureOutInaraCorrection();

            // Parse logs and get the changes to material counts
            // The filtering function adds unrecognized materials to unknown list
            deltas =
                logs.FilterOnlyInaraMats(
                    logs.GetDeltasSince(state.GetLastUpdateTimestamp()),
                    unknown);

            // Apply changes to material counts
            result = DeltaTools.Add(last, deltas);

            // If there is a correction, also add that to the mat counts
            if(inaraCorrection != null)
            {
                result = DeltaTools.Add(result, inaraCorrection);
            }

            // Add all of the data we found above to the form
            UpdateFormWithCurrentMats();

            // Hand control back to the user
            // If deltas are not empty or there is a corretion, we have stuff to save
            form.SetReadyState(
                !DeltaTools.IsZero(deltas) || inaraCorrection != null);
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
                if (!DeltaTools.IsZero(deltas))
                {
                    await api.PostMaterialsSheet(result);
                }

                // Save to History
                state.AddHistory(deltas);

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
                var ackForm = new UnrecognizedMaterials(unknown);
                ackForm.ShowDialog();
                e.Cancel = true;
            }

            // If there are negative values in the result dictionary,
            // something went wrong.  Notify the user and prevent the
            // window close.
            if(DeltaTools.IsNegative(result))
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
