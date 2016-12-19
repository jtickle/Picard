using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Picard
{
    public class EliteLogs
    {
        /// <summary>
        /// The absolute path to the Elite log files
        /// </summary>
        protected string LogFilePath;

        /// <summary>
        /// The JsonSerializer that will be used to read log entries
        /// </summary>
        protected JsonSerializer Serializer;

        /// <summary>
        /// Translation table from Elite to Inara
        /// </summary>
        protected Dictionary<string, string> EliteMatsLookup;

        /// <summary>
        /// Costs of unlocking the Engineers
        /// </summary>
        protected Dictionary<string, Dictionary<string, int>> EngineerCostLookup;

        public EliteLogs()
        {
            // Get User Profile Folder
            string profile = Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile);

            // Elite Log Path relative to User Profile Folder
            string eliteLogPath = @"Saved Games\Frontier Developments\Elite Dangerous";

            // Combine to get the log file path
            LogFilePath = Path.Combine(profile, eliteLogPath);
            
            // Initialize our serializer which can be reused
            Serializer = new JsonSerializer();

            // Yay, Elite's mat names are inconsistent!
            EliteMatsLookup = new Dictionary<string, string>();
            EliteMatsLookup.Add("antimony", "Antimony");
            EliteMatsLookup.Add("arsenic", "Arsenic");
            EliteMatsLookup.Add("basicconductors", "Basic Conductors");
            EliteMatsLookup.Add("biotechconductors", "Biotech Conductors");
            EliteMatsLookup.Add("cadmium", "Cadmium");
            EliteMatsLookup.Add("carbon", "Carbon");
            EliteMatsLookup.Add("chemicaldistillery", "Chemical Distillery");
            EliteMatsLookup.Add("chemicalmanipulators", "Chemical Manipulators");
            EliteMatsLookup.Add("chemicalprocessors", "Chemical Processors");
            EliteMatsLookup.Add("chemicalstorageunits", "Chemical Storage Units");
            EliteMatsLookup.Add("chromium", "Chromium");
            EliteMatsLookup.Add("compactcomposites", "Compact Composites");
            EliteMatsLookup.Add("compoundshielding", "Compound Shielding");
            EliteMatsLookup.Add("conductiveceramics", "Conductive Ceramics");
            EliteMatsLookup.Add("conductivecomponents", "Conductive Components");
            EliteMatsLookup.Add("conductivepolymers", "Conductive Polymers");
            EliteMatsLookup.Add("configurablecomponents", "Configurable Components");
            EliteMatsLookup.Add("coredynamicscomposites", "Core Dynamics Composites");
            EliteMatsLookup.Add("crystalshards", "Crystal Shards");
            EliteMatsLookup.Add("electrochemicalarrays", "Electrochemical Arrays");
            EliteMatsLookup.Add("exquisitefocuscrystals", "Exquisite Focus Crystals");
            EliteMatsLookup.Add("filamentcomposites", "Filament Composites");
            EliteMatsLookup.Add("flawedfocuscrystals", "Flawed Focus Crystals");
            EliteMatsLookup.Add("focuscrystals", "Focus Crystals");
            EliteMatsLookup.Add("galvanisingalloys", "Galvanising Alloys");
            EliteMatsLookup.Add("germanium", "Germanium");
            EliteMatsLookup.Add("gridresistors", "Grid Resistors");
            EliteMatsLookup.Add("heatconductionwiring", "Heat Conduction Wiring");
            EliteMatsLookup.Add("heatdispersionplate", "Heat Dispersion Plate");
            EliteMatsLookup.Add("heatexchangers", "Heat Exchangers");
            EliteMatsLookup.Add("heatresistantceramics", "Heat Resistant Ceramics");
            EliteMatsLookup.Add("heatvanes", "Heat Vanes");
            EliteMatsLookup.Add("highdensitycomposites", "High Density Composites");
            EliteMatsLookup.Add("hybridcapacitors", "Hybrid Capacitors");
            EliteMatsLookup.Add("imperialshielding", "Imperial Shielding");
            EliteMatsLookup.Add("improvisedcomponents", "Improvised Components");
            EliteMatsLookup.Add("iron", "Iron");
            EliteMatsLookup.Add("manganese", "Manganese");
            EliteMatsLookup.Add("mechanicalcomponents", "Mechanical Components");
            EliteMatsLookup.Add("mechanicalequipment", "Mechanical Equipment");
            EliteMatsLookup.Add("mechanicalscrap", "Mechanical Scrap");
            EliteMatsLookup.Add("mercury", "Mercury");
            EliteMatsLookup.Add("militarygradealloys", "Military Grade Alloys");
            EliteMatsLookup.Add("militarysupercapacitors", "Military Supercapacitors");
            EliteMatsLookup.Add("molybdenum", "Molybdenum");
            EliteMatsLookup.Add("nickel", "Nickel");
            EliteMatsLookup.Add("niobium", "Niobium");
            EliteMatsLookup.Add("pharmaceuticalisolators", "Pharmaceutical Isolators");
            EliteMatsLookup.Add("phasealloys", "Phase Alloys");
            EliteMatsLookup.Add("phosphorus", "Phosphorus");
            EliteMatsLookup.Add("polonium", "Polonium");
            EliteMatsLookup.Add("polymercapacitors", "Polymer Capacitors");
            EliteMatsLookup.Add("precipitatedalloys", "Precipitated Alloys");
            EliteMatsLookup.Add("proprietarycomposites", "Proprietary Composites");
            EliteMatsLookup.Add("protoheatradiators", "Proto Heat Radiators");
            EliteMatsLookup.Add("protolightalloys", "Proto Light Alloys");
            EliteMatsLookup.Add("protoradiolicalloys", "Proto Radiolic Alloys");
            EliteMatsLookup.Add("refinedfocuscrystals", "Refined Focus Crystals");
            EliteMatsLookup.Add("ruthenium", "Ruthenium");
            EliteMatsLookup.Add("salvagedalloys", "Salvaged Alloys");
            EliteMatsLookup.Add("selenium", "Selenium");
            EliteMatsLookup.Add("shieldemitters", "Shield Emitters");
            EliteMatsLookup.Add("shieldingsensors", "Shielding Sensors");
            EliteMatsLookup.Add("sulphur", "Sulphur");
            EliteMatsLookup.Add("technetium", "Technetium");
            EliteMatsLookup.Add("tellurium", "Tellurium");
            EliteMatsLookup.Add("temperedalloys", "Tempered Alloys");
            EliteMatsLookup.Add("thermicalloys", "Thermic Alloys");
            EliteMatsLookup.Add("tin", "Tin");
            EliteMatsLookup.Add("tungsten", "Tungsten");
            EliteMatsLookup.Add("unknownenergysource", "Unknown Fragment");
            EliteMatsLookup.Add("vanadium", "Vanadium");
            EliteMatsLookup.Add("wornshieldemitters", "Worn Shield Emitters");
            EliteMatsLookup.Add("yttrium", "Yttrium");
            EliteMatsLookup.Add("zinc", "Zinc");
            EliteMatsLookup.Add("zirconium", "Zirconium");
            EliteMatsLookup.Add("shieldpatternanalysis", "Aberrant Shield Pattern Analysis");
            EliteMatsLookup.Add("abnormalcompactemissiondata", "Abnormal Compact Emission Data");
            EliteMatsLookup.Add("adaptiveencryptorscapture", "Adaptive Encryptors Capture");
            EliteMatsLookup.Add("bulkscandata", "Anomalous Bulk Scan Data");
            EliteMatsLookup.Add("fsdtelemetry", "Anomalous FSD Telemetry");
            EliteMatsLookup.Add("disruptedwakeechoes", "Atypical Disrupted Wake Echoes");
            EliteMatsLookup.Add("atypicalencryptionarchives", "Atypical Encryption Archives");
            EliteMatsLookup.Add("classifiedscandata", "Classified Scan Databanks");
            EliteMatsLookup.Add("scanfragment", "Classified Scan Fragment");
            EliteMatsLookup.Add("industrialfirmware", "Cracked Industrial Firmware");
            EliteMatsLookup.Add("dataminedwakeexceptions", "Datamined Wake Exceptions");
            EliteMatsLookup.Add("decodedemissiondata", "Decoded Emission Data");
            EliteMatsLookup.Add("shieldcyclerecordings", "Distorted Shield Cycle Recordings");
            EliteMatsLookup.Add("divergentscandata", "Divergent Scan Data");
            EliteMatsLookup.Add("hyperspacetrajectories", "Eccentric Hyperspace Trajectories");
            EliteMatsLookup.Add("scrambledemissiondata", "Exceptional Scrambled Emission Data");
            EliteMatsLookup.Add("inconsistentshieldsoakanalysis", "Inconsistent Shield Soak Analysis");
            EliteMatsLookup.Add("irregularemissiondata", "Irregular Emission Data");
            EliteMatsLookup.Add("consumerfirmware", "Modified Consumer Firmware");
            EliteMatsLookup.Add("embeddedfirmware", "Modified Embedded Firmware");
            EliteMatsLookup.Add("symmetrickeys", "Open Symmetric Keys");
            EliteMatsLookup.Add("peculiarshieldfrequencydata", "Peculiar Shield Frequency Data");
            EliteMatsLookup.Add("securityfirmware", "Security Firmware Patch");
            EliteMatsLookup.Add("legacyfirmware", "Specialised Legacy Firmware");
            EliteMatsLookup.Add("strangewakesolutions", "Strange Wake Solutions");
            EliteMatsLookup.Add("encryptioncodes", "Tagged Encryption Codes");
            EliteMatsLookup.Add("unexpectedemissiondata", "Unexpected Emission Data");
            EliteMatsLookup.Add("scanarchives", "Unidentified Scan Archives");
            EliteMatsLookup.Add("untypicalshieldscans", "Untypical Shield Scans");
            EliteMatsLookup.Add("encryptedfiles", "Unusual Encrypted Files");
            EliteMatsLookup.Add("articulationmotors", "Articulation Motors");
            EliteMatsLookup.Add("bromellite", "Bromellite");
            EliteMatsLookup.Add("cmmcomposite", "CMM Composite");
            EliteMatsLookup.Add("emergencypowercells", "Emergency Power Cells");
            EliteMatsLookup.Add("energygridassembly", "Energy Grid Assembly");
            EliteMatsLookup.Add("exhaustmanifold", "Exhaust Manifold");
            EliteMatsLookup.Add("hardwarediagnosticsensor", "Hardware Diagnostic Sensor");
            EliteMatsLookup.Add("heatsinkinterlink", "Heatsink Interlink");
            EliteMatsLookup.Add("hnshockmount", "HN Shock Mount");
            EliteMatsLookup.Add("insulatingmembrane", "Insulating Membrane");
            EliteMatsLookup.Add("iondistributor", "Ion Distributor");
            EliteMatsLookup.Add("magneticemittercoil", "Magnetic Emitter Coil");
            EliteMatsLookup.Add("microcontrollers", "Micro Controllers");
            EliteMatsLookup.Add("coolinghoses", "Micro-Weave Cooling Hoses");
            EliteMatsLookup.Add("modularterminals", "Modular Terminals");
            EliteMatsLookup.Add("nanobreakers", "Nanobreakers");
            EliteMatsLookup.Add("neofabricinsulation", "Neofabric Insulation");
            EliteMatsLookup.Add("osmium", "Osmium");
            EliteMatsLookup.Add("platinum", "Platinum");
            EliteMatsLookup.Add("powerconverter", "Power Converter");
            EliteMatsLookup.Add("powertransferbus", "Power Transfer Bus");
            EliteMatsLookup.Add("praseodymium", "Praseodymium");
            EliteMatsLookup.Add("radiationbaffle", "Radiation Baffle");
            EliteMatsLookup.Add("reinforcedmountingplate", "Reinforced Mounting Plate");
            EliteMatsLookup.Add("samarium", "Samarium");
            EliteMatsLookup.Add("telemetrysuite", "Telemetry Suite");

            // Logs do not reflect cost of unlocking an engineer, so we look that up
            EngineerCostLookup = new Dictionary<string, Dictionary<string, int>>();

            var mats = new Dictionary<string, int>();
            mats.Add("Meta Alloys", -1);
            EngineerCostLookup.Add("Felicity Farseer", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Soontill Relics", -1);
            EngineerCostLookup.Add("Elvira Martuuk", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("The Dweller", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Landmines", -200);
            EngineerCostLookup.Add("Liz Ryder", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Tod McQuinn", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Xihe Companions", -25);
            EngineerCostLookup.Add("Zacariah Nemo", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Lei Cheung", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Kamitra Cigars", -50);
            EngineerCostLookup.Add("Hera Tani", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Juri Ishmaak", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Painite", -10);
            EngineerCostLookup.Add("Selene Jean", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Modular Terminals", -25);
            EngineerCostLookup.Add("Marco Qwent", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Classified Scan Databanks", -50);
            EngineerCostLookup.Add("Ram Tah", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Fujin Tea", -50);
            EngineerCostLookup.Add("Broo Tarquin", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Colonel Bris Dekker", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Lavian Brandy", -50);
            EngineerCostLookup.Add("Didi Vatermann", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Unknown Fragment", -25);
            EngineerCostLookup.Add("Professor Palin", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Kongga Ale", -25);
            EngineerCostLookup.Add("Lori Jameson", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Decoded Emission Data", -50);
            EngineerCostLookup.Add("Tiana Fortune", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Aberrant Shield Pattern", -50);
            EngineerCostLookup.Add("The Sarge", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Bromellite", -50);
            EngineerCostLookup.Add("Bill Turner", mats);
        }
        /// <summary>
        /// Get the absolute paths of all log files
        /// </summary>
        /// <returns>
        /// An IEnumerable of all the absolute paths of all Elite log files
        /// </returns>
        public IEnumerable<string> GetLogFiles()
        {
            // Return all log files
            return Directory.EnumerateFiles(LogFilePath, "*.log");
        }

        /// <summary>
        /// Sorts log files by last modified time
        /// </summary>
        /// <param name="logFiles">An IEnumerable of strings of log file names</param>
        /// <returns>The sorted set of log file names</returns>
        public IEnumerable<string> SortLogFiles(IEnumerable<string> logFiles)
        {
            // Sort log files by write time
            return from logFile in logFiles
                   orderby File.GetLastWriteTime(logFile)
                   select logFile;
        }

        /// <summary>
        /// Extracts all entries from a list of absolute paths to log files
        /// </summary>
        /// <param name="logFiles">A list of absolute paths to log files</param>
        /// <returns>A set of log entries represented as parsed JSON</returns>
        public IEnumerable<JObject> GetLogEntries(IEnumerable<string> logFiles)
        {

            // Loop over all log files
            foreach (var file in logFiles)
            {

                // Loop over all lines in the log file
                foreach(var line in File.ReadAllLines(file))
                {

                    // Parse and read a line
                    using (var lineReader = new StringReader(line))
                    using (var jsonReader = new JsonTextReader(lineReader))
                    {
                        JObject entry = (JObject)JToken.ReadFrom(jsonReader);

                        yield return entry;
                    }
                }
            }
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
            if(EliteMatsLookup.ContainsKey(eliteMat.ToLower()))
            {
                return EliteMatsLookup[eliteMat.ToLower()];
            }

            // TODO: This should prompt the user to enter the correct information and
            // potentially send it on back to me for inclusion in the official list
            return "unknown:" + eliteMat;
        }

        /// <summary>
        /// Handle the case where a Material is Collected in Space
        /// </summary>
        /// <param name="entry">An Elite Log Entry as Parsed JSON</param>
        /// <param name="orig">The materials dictionary to modify</param>
        /// <returns>The modified materials dictionary</returns>
        public IDictionary<string, int> HandleMaterialCollected(JObject entry, IDictionary<string, int> orig)
        {
            // Straightforward, Name and Count are directly on the entry
            DeltaTools.AddMat(
                orig,
                TranslateMat((string)entry["Name"]),
                (int)(long)entry["Count"]);

            return orig;
        }

        /// <summary>
        /// Handle the case where a Material is the reward of completing a mission
        /// </summary>
        /// <param name="entry">An Elite Log Entry as Parsed JSON</param>
        /// <param name="orig">The materials dictionary to modify</param>
        /// <returns>The modified materials dictionary</returns>
        public IDictionary<string, int> HandleMissionCompleted(JObject entry, IDictionary<string, int> orig)
        {
            // Due to the way JObject works, we are basically seeing if
            // these keys exist in the entry.
            foreach (var prop in entry.Properties())
            {
                // If the property is called "Ingredients", it is materials and data.
                if (prop.Name == "Ingredients")
                {
                    // Handle gaining a material or data through completing a mission
                    foreach (var mat in ((JObject)entry["Ingredients"]).Properties())
                    {
                        // { event: "MissionCompleted", Ingredients: { mat: delta, ... } }
                        DeltaTools.AddMat(
                            orig,
                            TranslateMat(mat.Name),
                            int.Parse(mat.Value.ToString()));
                    }
                }
                // If the property is called "CommodityReward", it is commodities and
                // also formatted differently.
                else if (prop.Name == "CommodityReward")
                {
                    // Handle gaining a commodity through completing a mission
                    foreach (var mat in entry["CommodityReward"])
                    {
                        // { event: "MissionCompleted", CommodityReward: [ { Name: mat, Count: delta } ] }
                        DeltaTools.AddMat(
                            orig,
                            TranslateMat(mat["Name"].ToString()),
                            int.Parse(mat["Count"].ToString()));
                    }
                }
            }

            return orig;
        }

        /// <summary>
        /// Handle the case where materials are removed due to an engineer upgrade
        /// </summary>
        /// <param name="entry">An Elite Log Entry as Parsed JSON</param>
        /// <param name="orig">The materials dictionary to modify</param>
        /// <returns>The modified materials dictionary</returns>
        public IDictionary<string, int> HandleEngineerCraft(JObject entry, IDictionary<string, int> orig)
        {
            // Loop over Ingredients
            foreach (var mat in ((JObject)entry["Ingredients"]).Properties())
            {
                // { event: "EngineerCraft", Ingredients: { mat: delta, ... } }
                DeltaTools.AddMat(
                    orig,
                    TranslateMat(mat.Name),
                    0 - int.Parse(mat.Value.ToString()));
            }

            return orig;
        }

        /// <summary>
        /// Handle the case where a material is discarded by the player
        /// </summary>
        /// <param name="entry">An Elite Log Entry as Parsed JSON</param>
        /// <param name="orig">The materials dictionary to modify</param>
        /// <returns>The modified materials dictionary</returns>
        public IDictionary<string, int> HandleMaterialDiscarded(JObject entry, IDictionary<string, int> orig)
        {
            // One mat gets destroyed at a time, just get its Name and Count
            // { event: "MaterialDiscarded", Name: mat, Count: delta }
            var mat = TranslateMat((string)entry["Name"]);
            DeltaTools.AddMat(orig,
                TranslateMat((string)entry["Name"]),
                0 - (int)(long)entry["Count"]);

            return orig;
        }

        /// <summary>
        /// Handle the case where engineer progression costs you materials
        /// </summary>
        /// <param name="entry">An Elite Log Entry as Parsed JSON</param>
        /// <param name="orig">The materials dictionary to modify</param>
        /// <returns>The modified materials dictionary</returns>
        public IDictionary<string, int> HandleEngineerProgress(JObject entry, IDictionary<string, int> orig)
        {
            // Only look for log entries with Progress=Unlocked and Rank=1
            // we are only interested in the engineer's unlock event
            bool progress = false, rank = false;
            foreach (var key in entry.Properties())
            {
                if ((string)key == "Progress")
                {
                    if ((string)entry["Progress"] == "Unlocked")
                        progress = true;
                }
                else if ((string)key == "Rank")
                {
                    if (int.Parse((string)entry["Rank"]) == 1)
                        rank = true;
                }
            }
            if (!progress || !rank) return orig;

            // The logs do not provide the actual materials or commodities
            // that are relieved of you buy the engineers.  We look these values
            // up in a table.
            string engineer = (string)entry["Engineer"];
            if (!EngineerCostLookup.ContainsKey(engineer))
            {
                DeltaTools.AddMat(orig, "unknownEngineer:" + engineer, 0);
            }
            else
            {
                orig = DeltaTools.Add(orig, EngineerCostLookup[engineer]);
            }

            return orig;
        }

        /// <summary>
        /// Gets all material deltas that have occurred since the "since" DateTime
        /// and provides them as a materials dictionary
        /// </summary>
        /// <param name="since">All log entries will have taken place after this time</param>
        /// <returns>A materials dictionary of material deltas</returns>
        public IDictionary<string, int> GetDeltasSince(DateTime since)
        {
            // Get all log entries from all log files, sorted, that are newer than "since"
            var entries = from logEntry in GetLogEntries(SortLogFiles(GetLogFiles()))
                          where (DateTime)logEntry["timestamp"] > since
                          orderby logEntry["timestamp"]
                          select logEntry;

            // We will be returning a dictionary of mats and deltas
            IDictionary <string, int> result = new Dictionary<string, int>();

            // Loop over all log entries
            foreach(var entry in entries)
            {

                // Look for events we are interested in
                switch ((string)entry["event"])
                {

                    // Handle collecting a material in space
                    case "MaterialCollected":

                        result = HandleMaterialCollected(entry, result);
                        break;

                    // Handle collecting a material or commodity in completing a mission
                    case "MissionCompleted":

                        result = HandleMissionCompleted(entry, result);
                        break;

                    // Handle losing materials or commodities in buying engineer upgrades
                    case "EngineerCraft":

                        result = HandleEngineerCraft(entry, result);
                        break;

                    // Handle losing materials or commodities in dumping them overboard
                    case "MaterialDiscarded":
                        
                        result = HandleMaterialDiscarded(entry, result);
                        break;

                    // Handle losing materials or commodities in unlocking an engineer
                    case "EngineerProgress":

                        result = HandleEngineerProgress(entry, result);
                        break;
                }
            }

            return result;
        }
    }
}
