using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace Picard
{
    public class EliteLogs
    {
        protected string LogFilePath;
        protected JsonSerializer Serializer;

        protected Dictionary<string, string> EliteMatsLookup;

        public EliteLogs()
        {
            string profile = Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile);
            string eliteLogPath = @"Saved Games\Frontier Developments\Elite Dangerous";
            LogFilePath = Path.Combine(profile, eliteLogPath);

            Serializer = new JsonSerializer();

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
            EliteMatsLookup.Add("unknownfragment", "Unknown Fragment");
            EliteMatsLookup.Add("vanadium", "Vanadium");
            EliteMatsLookup.Add("wornshieldemitters", "Worn Shield Emitters");
            EliteMatsLookup.Add("yttrium", "Yttrium");
            EliteMatsLookup.Add("zinc", "Zinc");
            EliteMatsLookup.Add("zirconium", "Zirconium");
            EliteMatsLookup.Add("aberrantshieldpatternanalysis", "Aberrant Shield Pattern Analysis");
            EliteMatsLookup.Add("abnormalcompactemissiondata", "Abnormal Compact Emission Data");
            EliteMatsLookup.Add("adaptiveencryptorscapture", "Adaptive Encryptors Capture");
            EliteMatsLookup.Add("anomalousbulkscandata", "Anomalous Bulk Scan Data");
            EliteMatsLookup.Add("anomalousfsdtelemetry", "Anomalous FSD Telemetry");
            EliteMatsLookup.Add("atypicaldisruptedwakeechoes", "Atypical Disrupted Wake Echoes");
            EliteMatsLookup.Add("atypicalencryptionarchives", "Atypical Encryption Archives");
            EliteMatsLookup.Add("classifiedscandatabanks", "Classified Scan Databanks");
            EliteMatsLookup.Add("classifiedscanfragment", "Classified Scan Fragment");
            EliteMatsLookup.Add("crackedindustrialfirmware", "Cracked Industrial Firmware");
            EliteMatsLookup.Add("dataminedwakeexceptions", "Datamined Wake Exceptions");
            EliteMatsLookup.Add("decodedemissiondata", "Decoded Emission Data");
            EliteMatsLookup.Add("distortedshieldcyclerecordings", "Distorted Shield Cycle Recordings");
            EliteMatsLookup.Add("divergentscandata", "Divergent Scan Data");
            EliteMatsLookup.Add("eccentrichyperspacetrajectories", "Eccentric Hyperspace Trajectories");
            EliteMatsLookup.Add("exceptionalscrambledemissiondata", "Exceptional Scrambled Emission Data");
            EliteMatsLookup.Add("inconsistentshieldsoakanalysis", "Inconsistent Shield Soak Analysis");
            EliteMatsLookup.Add("irregularemissiondata", "Irregular Emission Data");
            EliteMatsLookup.Add("modifiedconsumerfirmware", "Modified Consumer Firmware");
            EliteMatsLookup.Add("modifiedembeddedfirmware", "Modified Embedded Firmware");
            EliteMatsLookup.Add("opensymmetrickeys", "Open Symmetric Keys");
            EliteMatsLookup.Add("peculiarshieldfrequencydata", "Peculiar Shield Frequency Data");
            EliteMatsLookup.Add("securityfirmwarepatch", "Security Firmware Patch");
            EliteMatsLookup.Add("specialisedlegacyfirmware", "Specialised Legacy Firmware");
            EliteMatsLookup.Add("strangewakesolutions", "Strange Wake Solutions");
            EliteMatsLookup.Add("taggedencryptioncodes", "Tagged Encryption Codes");
            EliteMatsLookup.Add("unexpectedemissiondata", "Unexpected Emission Data");
            EliteMatsLookup.Add("unidentifiedscanarchives", "Unidentified Scan Archives");
            EliteMatsLookup.Add("untypicalshieldscans", "Untypical Shield Scans");
            EliteMatsLookup.Add("unusualencryptedfiles", "Unusual Encrypted Files");
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
            EliteMatsLookup.Add("micro-weavecoolinghoses", "Micro-Weave Cooling Hoses");
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
        }

        public IEnumerable<string> GetLogFiles()
        {
            return Directory.EnumerateFiles(LogFilePath, "*.log");
        }

        public IEnumerable<string> SortLogFiles(IEnumerable<string> logFiles)
        {
            return from logFile in logFiles
                   orderby File.GetLastWriteTime(logFile)
                   select logFile;
        }

        public IEnumerable<IDictionary<string, object>> GetLogEntries(IEnumerable<string> logFiles)
        {
            foreach (var file in logFiles)
            {
                foreach(var line in File.ReadAllLines(file))
                {
                    using (var lineReader = new StringReader(line))
                    using (var jsonReader = new JsonTextReader(lineReader))
                    {
                        var entry = Serializer.Deserialize
                            <Dictionary<string, object>>
                            (jsonReader);

                        if (!entry.ContainsKey("timestamp"))
                        {
                            Console.WriteLine("Log entry in " + file + " has no timestamp");
                            continue;
                        }

                        yield return entry;
                    }
                }
            }
        }

        public DateTime ParseEliteTS(string eliteTS)
        {
            return DateTime.ParseExact(eliteTS, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        public string TranslateMat(string eliteMat)
        {
            if(EliteMatsLookup.ContainsKey(eliteMat))
            {
                return EliteMatsLookup[eliteMat];
            }

            return "unknown:" + eliteMat;
        }

        public IDictionary<string, int> GetDeltasSince(DateTime since)
        {
            var entries = from logEntry in GetLogEntries(SortLogFiles(GetLogFiles()))
                          where (DateTime)logEntry["timestamp"] > since
                          orderby logEntry["timestamp"]
                          select logEntry;

            IDictionary < string, int> result = new Dictionary<string, int>();
            foreach(var entry in entries)
            {
                if (!entry.ContainsKey("event"))
                {
                    Console.WriteLine("Event at {0} has no 'event'");
                    continue;
                }

                IDictionary<string, string> mats;
                if ((string)entry["event"] == "MaterialCollected")
                {
                    // Handle Collecting a Material in Space

                    // TODO: Category?
                    var mat = TranslateMat((string)entry["Name"]);
                    DeltaTools.AddMat(result, mat, (int)(long)entry["Count"]);
                }
                else if((string)entry["event"] == "MissionCompleted")
                {
                    if(entry.ContainsKey("Ingredients"))
                    {
                        // Handle gaining a material or data through completing a mission
                        mats = (IDictionary<string, string>)entry["Ingredients"];
                        foreach (var mat in mats)
                        {
                            DeltaTools.AddMat(result, TranslateMat(mat.Key), int.Parse(mat.Value));
                        }
                    }
                    else if(entry.ContainsKey("CommodityReward"))
                    {
                        // Handle gaining a commodity through completing a mission

                        var arr = (IList<IDictionary<string, string>>)entry["CommodityReward"];
                        foreach(var mat in arr)
                        {
                            DeltaTools.AddMat(result, TranslateMat(mat["name"]), int.Parse(mat.Values));
                        }
                    }
                }
                else if ((string)entry["event"] == "EngineerCraft")
                {
                    var mats = (IDictionary<string, string>)entry["Ingredients"];
                    foreach(var mat in mats)
                    {
                        DeltaTools.AddMat(result, TranslateMat(mat.Key), 0 - int.Parse(mat.Value));
                    }
                }
                else if ((string)entry["event"] == "MaterialDiscarded")
                {
                    var mat = TranslateMat((string)entry["name"]);
                    DeltaTools.AddMat(result, mat, 0 - (int)(long)entry["Count"]);
                }
            }

            return result;
        }
    }
}
