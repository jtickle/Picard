using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Picard.Lib
{
    public class EliteLogs
    {
        /// <summary>
        /// The absolute path to the Elite log files
        /// </summary>
        public string LogFilePath { get; protected set; }

        /// <summary>
        /// The JsonSerializer that will be used to read log entries
        /// </summary>
        protected JsonSerializer Serializer;

        /// <summary>
        /// Translation table from Elite to Inara
        /// </summary>
        protected Dictionary<string, string> EliteMatsLookup;

        /// <summary>
        /// Commodities that we know are not materials
        /// </summary>
        protected List<string> IgnoreCommodities;

        /// <summary>
        /// Costs of unlocking the Engineers
        /// </summary>
        protected Dictionary<string, Dictionary<string, int>> EngineerCostLookup;

        public EliteLogs(
            Dictionary<string, string> EliteMatsLookup,
            List<string> IgnoreCommodities,
            Dictionary<string, Dictionary<string, int>> EngineerCostLookup)
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

            // Load Constants
            this.EliteMatsLookup = EliteMatsLookup;
            this.IgnoreCommodities = IgnoreCommodities;
            this.EngineerCostLookup = EngineerCostLookup;
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
                using(var fileReader = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileReader))
                {
                    // Loop over all lines in the log file
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();

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
            return eliteMat;
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
                int.Parse((string)entry["Count"]));

            return orig;
        }

        public IDictionary<string, int> HandleMarketBuy(JObject entry, IDictionary<string, int> orig)
        {
            // Straightforward, Name and Count are directly on the entry
            DeltaTools.AddMat(
                orig,
                TranslateMat((string)entry["Type"]),
                int.Parse((string)entry["Count"]));

            return orig;
        }

        public IDictionary<string, int> HandleMarketSell(JObject entry, IDictionary<string, int> orig)
        {
            // Straightforward, Name and Count are directly on the entry
            DeltaTools.AddMat(
                orig,
                TranslateMat((string)entry["Type"]),
                0 - int.Parse((string)entry["Count"]));

            return orig;
        }

        public IDictionary<string, int> HandleSynthesis(JObject entry, IDictionary<string, int> orig)
        {
            // List of material key : material count in set under key Materials
            foreach(var prop in entry.Properties())
            {
                if(prop.Name == "Materials")
                {
                    foreach(var mat in ((JObject) entry["Materials"]).Properties())
                    {
                        DeltaTools.AddMat(
                            orig,
                            TranslateMat(mat.Name),
                            0 - int.Parse(mat.Value.ToString()));
                    }
                }
            }

            return orig;
        }
        /// <summary>
        /// Boy this has become quite the project...
        /// </summary>
        /// <param name="entry">Log Entry</param>
        /// <param name="orig">Mats Dictionary</param>
        /// <returns>Mats Dictionary Plus Stuff</returns>
        public IDictionary<string, int> HandleMissionAccepted(JObject entry, IDictionary<string, int> orig)
        {
            if (!entry["Name"].ToString().StartsWith("Mission_Delivery"))
                return orig;

            foreach (var prop in entry.Properties())
            {
                if(prop.Name == "Commodity_Localised")
                {
                    DeltaTools.AddMat(
                        orig,
                        entry["Commodity_Localised"].ToString(),
                        int.Parse(entry["Count"].ToString()));
                }
            }

            return orig;
        }

        /// <summary>
        /// Handle the case where a Material is the reward of completing a mission
        /// Also, handle the very annoying case where a Commodity is consumed in the
        /// completion of a mission
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

                if (prop.Name == "Commodity_Localised")
                {
                    // Handle losing a commodity as the result of completing a mission
                    DeltaTools.AddMat(
                        orig,
                        entry["Commodity_Localised"].ToString(),
                        0 - int.Parse(entry["Count"].ToString()));
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
                if (key.Name == "Progress")
                {
                    if (key.Value.ToString() == "Unlocked")
                        progress = true;
                }
                else if (key.Name == "Rank")
                {
                    if (int.Parse(key.Value.ToString()) == 1)
                        rank = true;
                }
            }
            if (!progress || !rank)
            {
                return orig;
            }

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

        public IDictionary<string, int> FilterOnlyInaraMats(IDictionary<string, int> deltas, IDictionary<string, int> removed)
        {
            // TODO: This should really be looking at the data just pulled
            // from Inara
            var ret = new Dictionary<string, int>();

            foreach(var mat in deltas)
            {
                if(EliteMatsLookup.ContainsValue(mat.Key))
                {
                    ret.Add(mat.Key, mat.Value);
                } else if(!IgnoreCommodities.Contains(mat.Key))
                {
                    removed.Add(mat.Key, mat.Value);
                }
            }

            return ret;
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

                    // Handle collecting a commodity as part of a mission

                    case "MissionAccepted":

                        result = HandleMissionAccepted(entry, result);
                        break;

                    // Handle collecting a material or commodity in completing a mission
                    case "MissionCompleted":

                        result = HandleMissionCompleted(entry, result);
                        break;

                    // Handle collecting a commodity through a market buy
                    case "MarketBuy":

                        result = HandleMarketBuy(entry, result);
                        break;

                    // Handle losing a commodity through a market sell
                    case "MarketSell":

                        result = HandleMarketSell(entry, result);
                        break;

                    // Handle losing a mat through synthesis
                    case "Synthesis":

                        result = HandleSynthesis(entry, result);
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

                    // Handle 
                }
            }

            return result;
        }
    }
}
