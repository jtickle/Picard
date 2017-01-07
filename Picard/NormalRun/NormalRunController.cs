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
        public IDictionary<string, string> MatType;

        public NormalRunController(InaraApi api, PersistentState state, EliteLogs logs)
        {
            this.api = api;
            this.state = state;
            this.logs = logs;

            form = new MatUpdateForm();
            form.ReloadMats += OnReloadMats;
            form.PostAndSave += OnPostAndSave;
            form.CloseWithoutSave += OnCloseWithoutSave;

            MatType = new Dictionary<string, string>();
            MatType.Add("Antimony", "Materials");
            MatType.Add("Arsenic", "Materials");
            MatType.Add("Basic Conductors", "Materials");
            MatType.Add("Biotech Conductors", "Materials");
            MatType.Add("Cadmium", "Materials");
            MatType.Add("Carbon", "Materials");
            MatType.Add("Chemical Distillery", "Materials");
            MatType.Add("Chemical Manipulators", "Materials");
            MatType.Add("Chemical Processors", "Materials");
            MatType.Add("Chemical Storage Units", "Materials");
            MatType.Add("Chromium", "Materials");
            MatType.Add("Compact Composites", "Materials");
            MatType.Add("Compound Shielding", "Materials");
            MatType.Add("Conductive Ceramics", "Materials");
            MatType.Add("Conductive Components", "Materials");
            MatType.Add("Conductive Polymers", "Materials");
            MatType.Add("Configurable Components", "Materials");
            MatType.Add("Core Dynamics Composites", "Materials");
            MatType.Add("Crystal Shards", "Materials");
            MatType.Add("Electrochemical Arrays", "Materials");
            MatType.Add("Exquisite Focus Crystals", "Materials");
            MatType.Add("Filament Composites", "Materials");
            MatType.Add("Flawed Focus Crystals", "Materials");
            MatType.Add("Focus Crystals", "Materials");
            MatType.Add("Galvanising Alloys", "Materials");
            MatType.Add("Germanium", "Materials");
            MatType.Add("Grid Resistors", "Materials");
            MatType.Add("Heat Conduction Wiring", "Materials");
            MatType.Add("Heat Dispersion Plate", "Materials");
            MatType.Add("Heat Exchangers", "Materials");
            MatType.Add("Heat Resistant Ceramics", "Materials");
            MatType.Add("Heat Vanes", "Materials");
            MatType.Add("High Density Composites", "Materials");
            MatType.Add("Hybrid Capacitors", "Materials");
            MatType.Add("Imperial Shielding", "Materials");
            MatType.Add("Improvised Components", "Materials");
            MatType.Add("Iron", "Materials");
            MatType.Add("Manganese", "Materials");
            MatType.Add("Mechanical Components", "Materials");
            MatType.Add("Mechanical Equipment", "Materials");
            MatType.Add("Mechanical Scrap", "Materials");
            MatType.Add("Mercury", "Materials");
            MatType.Add("Military Grade Alloys", "Materials");
            MatType.Add("Military Supercapacitors", "Materials");
            MatType.Add("Molybdenum", "Materials");
            MatType.Add("Nickel", "Materials");
            MatType.Add("Niobium", "Materials");
            MatType.Add("Pharmaceutical Isolators", "Materials");
            MatType.Add("Phase Alloys", "Materials");
            MatType.Add("Phosphorus", "Materials");
            MatType.Add("Polonium", "Materials");
            MatType.Add("Polymer Capacitors", "Materials");
            MatType.Add("Precipitated Alloys", "Materials");
            MatType.Add("Proprietary Composites", "Materials");
            MatType.Add("Proto Heat Radiators", "Materials");
            MatType.Add("Proto Light Alloys", "Materials");
            MatType.Add("Proto Radiolic Alloys", "Materials");
            MatType.Add("Refined Focus Crystals", "Materials");
            MatType.Add("Ruthenium", "Materials");
            MatType.Add("Salvaged Alloys", "Materials");
            MatType.Add("Selenium", "Materials");
            MatType.Add("Shield Emitters", "Materials");
            MatType.Add("Shielding Sensors", "Materials");
            MatType.Add("Sulphur", "Materials");
            MatType.Add("Technetium", "Materials");
            MatType.Add("Tellurium", "Materials");
            MatType.Add("Tempered Alloys", "Materials");
            MatType.Add("Thermic Alloys", "Materials");
            MatType.Add("Tin", "Materials");
            MatType.Add("Tungsten", "Materials");
            MatType.Add("Unknown Fragment", "Materials");
            MatType.Add("Vanadium", "Materials");
            MatType.Add("Worn Shield Emitters", "Materials");
            MatType.Add("Yttrium", "Materials");
            MatType.Add("Zinc", "Materials");
            MatType.Add("Zirconium", "Materials");
            MatType.Add("Aberrant Shield Pattern Analysis", "Data");
            MatType.Add("Abnormal Compact Emission Data", "Data");
            MatType.Add("Adaptive Encryptors Capture", "Data");
            MatType.Add("Anomalous Bulk Scan Data", "Data");
            MatType.Add("Anomalous FSD Telemetry", "Data");
            MatType.Add("Atypical Disrupted Wake Echoes", "Data");
            MatType.Add("Atypical Encryption Archives", "Data");
            MatType.Add("Classified Scan Databanks", "Data");
            MatType.Add("Classified Scan Fragment", "Data");
            MatType.Add("Cracked Industrial Firmware", "Data");
            MatType.Add("Datamined Wake Exceptions", "Data");
            MatType.Add("Decoded Emission Data", "Data");
            MatType.Add("Distorted Shield Cycle Recordings", "Data");
            MatType.Add("Divergent Scan Data", "Data");
            MatType.Add("Eccentric Hyperspace Trajectories", "Data");
            MatType.Add("Exceptional Scrambled Emission Data", "Data");
            MatType.Add("Inconsistent Shield Soak Analysis", "Data");
            MatType.Add("Irregular Emission Data", "Data");
            MatType.Add("Modified Consumer Firmware", "Data");
            MatType.Add("Modified Embedded Firmware", "Data");
            MatType.Add("Open Symmetric Keys", "Data");
            MatType.Add("Peculiar Shield Frequency Data", "Data");
            MatType.Add("Security Firmware Patch", "Data");
            MatType.Add("Specialised Legacy Firmware", "Data");
            MatType.Add("Strange Wake Solutions", "Data");
            MatType.Add("Tagged Encryption Codes", "Data");
            MatType.Add("Unexpected Emission Data", "Data");
            MatType.Add("Unidentified Scan Archives", "Data");
            MatType.Add("Untypical Shield Scans", "Data");
            MatType.Add("Unusual Encrypted Files", "Data");
            MatType.Add("Articulation Motors", "Commodities");
            MatType.Add("Bromellite", "Commodities");
            MatType.Add("CMM Composite", "Commodities");
            MatType.Add("Emergency Power Cells", "Commodities");
            MatType.Add("Energy Grid Assembly", "Commodities");
            MatType.Add("Exhaust Manifold", "Commodities");
            MatType.Add("Hardware Diagnostic Sensor", "Commodities");
            MatType.Add("Heatsink Interlink", "Commodities");
            MatType.Add("HN Shock Mount", "Commodities");
            MatType.Add("Insulating Membrane", "Commodities");
            MatType.Add("Ion Distributor", "Commodities");
            MatType.Add("Magnetic Emitter Coil", "Commodities");
            MatType.Add("Micro Controllers", "Commodities");
            MatType.Add("Micro-Weave Cooling Hoses", "Commodities");
            MatType.Add("Modular Terminals", "Commodities");
            MatType.Add("Nanobreakers", "Commodities");
            MatType.Add("Neofabric Insulation", "Commodities");
            MatType.Add("Osmium", "Commodities");
            MatType.Add("Platinum", "Commodities");
            MatType.Add("Power Converter", "Commodities");
            MatType.Add("Power Transfer Bus", "Commodities");
            MatType.Add("Praseodymium", "Commodities");
            MatType.Add("Radiation Baffle", "Commodities");
            MatType.Add("Reinforced Mounting Plate", "Commodities");
            MatType.Add("Samarium", "Commodities");
            MatType.Add("Telemetry Suite", "Commodities");
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
            totals["Materials"] = 0;
            totals["Data"] = 0;
            totals["Commodities"] = 0;
            totals["DebugUnknown"] = 0;
            totals["Grand"] = 0;
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
            var t = MatType.ContainsKey(key)
                ? MatType[key]
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
            var types = new List<string>();
            types.Add("Materials");
            types.Add("Data");
            types.Add("Commodities");
            types.Add("DebugUnknown");
            types.Add("Grand");

            foreach (var type in types)
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

        protected async void SaveDataAndClose()
        {
            // Show post status form to indicate we are doing something
            var post = new PostForm();
            post.Loading = true;
            post.Show();

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

            // Await user response.
            post.Hide();
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
