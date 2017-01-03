﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picard
{
    public sealed partial class MatUpdateForm : Form, IGetData
    {
        private InaraApi api;
        private PersistentState state;
        private EliteLogs logs;

        private bool shouldSave;
        private IDictionary<string, int> last;
        private IDictionary<string, int> deltas;
        private IDictionary<string, int> result;
        private IDictionary<string, int> unknown;

        private IDictionary<string, int> totalsLast;
        private IDictionary<string, int> totalsDeltas;
        private IDictionary<string, int> totalsResult;

        /// <summary>
        /// The type of each material, for accounting
        /// </summary>
        public IDictionary<string, string> MatType;

        public MatUpdateForm(InaraApi api, PersistentState state)
        {
            this.api = api;
            this.state = state;
            logs = new EliteLogs();

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

            InitializeComponent();
        }

        private void addCountTo(IDictionary<string, int> totes, string key, int delta)
        {
            var t = MatType.ContainsKey(key)
                ? MatType[key]
                : "DebugUnknown";

            totes["Grand"] += delta;
            totes[t]             += delta;
        }

        private void ReloadMats()
        {
            MatsView.Items.Clear();
            shouldSave = false;
            okButton.Enabled = false;
            refreshButton.Enabled = false;

            unknown = new Dictionary<string, int>();

            last = state.CalculateCurrentInventory();
            deltas = logs.GetDeltasSince(state.GetLastUpdateTimestamp());
            deltas = logs.FilterOnlyInaraMats(deltas, unknown);
            
            result = DeltaTools.Add(last, deltas);

            totalsLast    = new Dictionary<string, int>();
            totalsLast["Materials"] = 0;
            totalsLast["Data"] = 0;
            totalsLast["Commodities"] = 0;
            totalsLast["DebugUnknown"] = 0;
            totalsLast["Grand"] = 0;
            totalsDeltas  = new Dictionary<string, int>();
            totalsDeltas["Materials"] = 0;
            totalsDeltas["Data"] = 0;
            totalsDeltas["Commodities"] = 0;
            totalsDeltas["DebugUnknown"] = unknown.Count;
            totalsDeltas["Grand"] = 0;
            totalsResult  = new Dictionary<string, int>();
            totalsResult["Materials"] = 0;
            totalsResult["Data"] = 0;
            totalsResult["Commodities"] = 0;
            totalsResult["DebugUnknown"] = 0;
            totalsResult["Grand"] = 0;

            foreach (var r in result)
            {
                var i = new ListViewItem(r.Key);

                if(last.ContainsKey(r.Key))
                {
                    i.SubItems.Add(last[r.Key].ToString());
                    addCountTo(totalsLast, r.Key, last[r.Key]);
                }
                else
                {
                    i.SubItems.Add("0");
                }

                if (deltas.ContainsKey(r.Key))
                {
                    i.SubItems.Add(deltas[r.Key].ToString());
                    addCountTo(totalsDeltas, r.Key, deltas[r.Key]);

                    // Blue for increase, Red for decrease
                    if(deltas[r.Key] > 0)
                    {
                        i.Font = new Font(i.Font, FontStyle.Bold);
                        i.BackColor = Color.LightBlue;
                    }
                    if(deltas[r.Key] < 0)
                    {
                        i.Font = new Font(i.Font, FontStyle.Bold);
                        i.BackColor = Color.Pink;
                    }
                }
                else
                {
                    i.SubItems.Add("0");
                }

                i.SubItems.Add(r.Value.ToString());
                addCountTo(totalsResult, r.Key, result[r.Key]);

                MatsView.Items.Add(i);
            }

            var types = new List<string>();
            types.Add("Materials");
            types.Add("Data");
            types.Add("Commodities");
            types.Add("DebugUnknown");
            types.Add("Grand");
            foreach(var type in types)
            {
                var i = new ListViewItem(type + " Total");
                i.SubItems.Add(totalsLast[type].ToString());
                i.SubItems.Add(totalsDeltas[type].ToString());
                i.SubItems.Add(totalsResult[type].ToString());
                
                // Blue for increase, Red for decrease
                if (totalsDeltas[type] > 0)
                {
                    i.Font = new Font(i.Font, FontStyle.Bold);
                    i.BackColor = Color.LightBlue;
                }
                if (totalsDeltas[type] < 0)
                {
                    i.Font = new Font(i.Font, FontStyle.Bold);
                    i.BackColor = Color.Pink;
                }

                MatsView.Items.Add(i);
            }
            

            refreshButton.Enabled = true;

            // Only enable okButton if there are changes to save.
            if(DeltaTools.IsZero(deltas))
            {
                okButton.Enabled = false;
                okButton.Text = "Nothing To Do";
            }
            else
            {
                okButton.Enabled = true;
                okButton.Text = "Looks Good";
            }
        }

        IDictionary<string, int> IGetData.GetDeltas()
        {
            return this.deltas;
        }

        IDictionary<string, int> IGetData.GetTotals()
        {
            return this.result;
        }

        bool IGetData.ShouldSave()
        {
            return this.shouldSave;
        }

        private void MatUpdateForm_Load(object sender, EventArgs e)
        {
            ReloadMats();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ReloadMats();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (unknown.Count > 0)
            {
                UnrecognizedMaterials m = new UnrecognizedMaterials(unknown);
                m.ShowDialog();
                return;
            }

            if (DeltaTools.IsNegative(result))
            {
                // TODO: all of this, better logging, reporting, etc
                MessageBox.Show("Oh no.  Your Material count has gone into the negative," +
                               "which means something went wrong.  You will need to manually" +
                               "re-synchronize with Inara and reset your Picard State file.",
                               "Very Unfortunate Error");
                return;
            }

            shouldSave = true;
            Close();
        }
    }
}
