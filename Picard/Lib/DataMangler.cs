using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picard.Lib
{
    public class DataMangler
    {
        public DateTime
            LastUpdated;
        public List<string>
            ConfirmedMats;
        public List<string>
            MaterialTypes;
        public List<string>
            IgnoreCommodities;
        public Dictionary<string, string>
            EliteMatsLookup;
        public Dictionary<string, string>
            MaterialTypeLookup;
        public Dictionary<string, Dictionary<string, int>>
            EngineerCostLookup;

        public DataMangler()
        {
            LastUpdated = DateTime.MinValue;
            ConfirmedMats = new List<string>();
            MaterialTypes = new List<string>();
            IgnoreCommodities = new List<string>();
            EliteMatsLookup = new Dictionary<string, string>();
            MaterialTypeLookup = new Dictionary<string, string>();
            EngineerCostLookup = new Dictionary<string, Dictionary<string, int>>();

            // Confirmed Materials that we know are legit

            // Possible Material Types
            MaterialTypes.Add("Materials");
            MaterialTypes.Add("Data");
            MaterialTypes.Add("Commodities");

            // Ignored Commodities
            IgnoreCommodities.Add("Domestic Appliances");
            IgnoreCommodities.Add("Reactive Armour");
            IgnoreCommodities.Add("Tobacco");
            IgnoreCommodities.Add("Consumer Technology");
            IgnoreCommodities.Add("Meta-Alloys");
            IgnoreCommodities.Add("Fish");
            IgnoreCommodities.Add("Clothing");
            IgnoreCommodities.Add("Animal Meat");
            IgnoreCommodities.Add("Progenitor Cells");
            IgnoreCommodities.Add("metaalloys");
            IgnoreCommodities.Add("Food Cartridges");
            IgnoreCommodities.Add("Basic Medicines");
            IgnoreCommodities.Add("Narcotics");
            IgnoreCommodities.Add("Survival Equipment");
            IgnoreCommodities.Add("Tea");
            IgnoreCommodities.Add("cryolite");
            IgnoreCommodities.Add("Black Box");
            IgnoreCommodities.Add("Advanced Medicines");
            IgnoreCommodities.Add("Non-Lethal Weapons");
            IgnoreCommodities.Add("Performance Enhancers");
            IgnoreCommodities.Add("Grain");
            IgnoreCommodities.Add("Beer");
            IgnoreCommodities.Add("Hydrogen Fuel");
            IgnoreCommodities.Add("Fruit and Vegetables");
            IgnoreCommodities.Add("Coffee");
            IgnoreCommodities.Add("Agri-Medicines");
            IgnoreCommodities.Add("occupiedcryopod");
            IgnoreCommodities.Add("Silver");
            IgnoreCommodities.Add("Cobalt");

            // Elite Log Material Name Lookup
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
            EliteMatsLookup.Add("compactemissionsdata", "Abnormal Compact Emission Data");
            EliteMatsLookup.Add("adaptiveencryptorscapture", "Adaptive Encryptors Capture");
            EliteMatsLookup.Add("bulkscandata", "Anomalous Bulk Scan Data");
            EliteMatsLookup.Add("fsdtelemetry", "Anomalous FSD Telemetry");
            EliteMatsLookup.Add("disruptedwakeechoes", "Atypical Disrupted Wake Echoes");
            EliteMatsLookup.Add("atypicalencryptionarchives", "Atypical Encryption Archives");
            EliteMatsLookup.Add("scandatabanks", "Classified Scan Databanks");
            EliteMatsLookup.Add("classifiedscandata", "Classified Scan Fragment");
            EliteMatsLookup.Add("industrialfirmware", "Cracked Industrial Firmware");
            EliteMatsLookup.Add("dataminedwake", "Datamined Wake Exceptions");
            EliteMatsLookup.Add("decodedemissiondata", "Decoded Emission Data");
            EliteMatsLookup.Add("shieldcyclerecordings", "Distorted Shield Cycle Recordings");
            EliteMatsLookup.Add("encodedscandata", "Divergent Scan Data");
            EliteMatsLookup.Add("hyperspacetrajectories", "Eccentric Hyperspace Trajectories");
            EliteMatsLookup.Add("scrambledemissiondata", "Exceptional Scrambled Emission Data");
            EliteMatsLookup.Add("shieldsoakanalysis", "Inconsistent Shield Soak Analysis");
            EliteMatsLookup.Add("archivedemissiondata", "Irregular Emission Data");
            EliteMatsLookup.Add("consumerfirmware", "Modified Consumer Firmware");
            EliteMatsLookup.Add("embeddedfirmware", "Modified Embedded Firmware");
            EliteMatsLookup.Add("symmetrickeys", "Open Symmetric Keys");
            EliteMatsLookup.Add("shieldfrequencydata", "Peculiar Shield Frequency Data");
            EliteMatsLookup.Add("securityfirmware", "Security Firmware Patch");
            EliteMatsLookup.Add("legacyfirmware", "Specialised Legacy Firmware");
            EliteMatsLookup.Add("wakesolutions", "Strange Wake Solutions");
            EliteMatsLookup.Add("encryptioncodes", "Tagged Encryption Codes");
            EliteMatsLookup.Add("emissiondata", "Unexpected Emission Data");
            EliteMatsLookup.Add("scanarchives", "Unidentified Scan Archives");
            EliteMatsLookup.Add("shielddensityreports", "Untypical Shield Scans");
            EliteMatsLookup.Add("encryptedfiles", "Unusual Encrypted Files");
            EliteMatsLookup.Add("articulationmotors", "Articulation Motors");
            EliteMatsLookup.Add("bromellite", "Bromellite");
            EliteMatsLookup.Add("cmmcomposite", "CMM Composite");
            EliteMatsLookup.Add("emergencypowercells", "Emergency Power Cells");
            EliteMatsLookup.Add("powergridassembly", "Energy Grid Assembly");
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

            // Material Types for Counts
            MaterialTypeLookup.Add("Antimony", "Materials");
            MaterialTypeLookup.Add("Arsenic", "Materials");
            MaterialTypeLookup.Add("Basic Conductors", "Materials");
            MaterialTypeLookup.Add("Biotech Conductors", "Materials");
            MaterialTypeLookup.Add("Cadmium", "Materials");
            MaterialTypeLookup.Add("Carbon", "Materials");
            MaterialTypeLookup.Add("Chemical Distillery", "Materials");
            MaterialTypeLookup.Add("Chemical Manipulators", "Materials");
            MaterialTypeLookup.Add("Chemical Processors", "Materials");
            MaterialTypeLookup.Add("Chemical Storage Units", "Materials");
            MaterialTypeLookup.Add("Chromium", "Materials");
            MaterialTypeLookup.Add("Compact Composites", "Materials");
            MaterialTypeLookup.Add("Compound Shielding", "Materials");
            MaterialTypeLookup.Add("Conductive Ceramics", "Materials");
            MaterialTypeLookup.Add("Conductive Components", "Materials");
            MaterialTypeLookup.Add("Conductive Polymers", "Materials");
            MaterialTypeLookup.Add("Configurable Components", "Materials");
            MaterialTypeLookup.Add("Core Dynamics Composites", "Materials");
            MaterialTypeLookup.Add("Crystal Shards", "Materials");
            MaterialTypeLookup.Add("Electrochemical Arrays", "Materials");
            MaterialTypeLookup.Add("Exquisite Focus Crystals", "Materials");
            MaterialTypeLookup.Add("Filament Composites", "Materials");
            MaterialTypeLookup.Add("Flawed Focus Crystals", "Materials");
            MaterialTypeLookup.Add("Focus Crystals", "Materials");
            MaterialTypeLookup.Add("Galvanising Alloys", "Materials");
            MaterialTypeLookup.Add("Germanium", "Materials");
            MaterialTypeLookup.Add("Grid Resistors", "Materials");
            MaterialTypeLookup.Add("Heat Conduction Wiring", "Materials");
            MaterialTypeLookup.Add("Heat Dispersion Plate", "Materials");
            MaterialTypeLookup.Add("Heat Exchangers", "Materials");
            MaterialTypeLookup.Add("Heat Resistant Ceramics", "Materials");
            MaterialTypeLookup.Add("Heat Vanes", "Materials");
            MaterialTypeLookup.Add("High Density Composites", "Materials");
            MaterialTypeLookup.Add("Hybrid Capacitors", "Materials");
            MaterialTypeLookup.Add("Imperial Shielding", "Materials");
            MaterialTypeLookup.Add("Improvised Components", "Materials");
            MaterialTypeLookup.Add("Iron", "Materials");
            MaterialTypeLookup.Add("Manganese", "Materials");
            MaterialTypeLookup.Add("Mechanical Components", "Materials");
            MaterialTypeLookup.Add("Mechanical Equipment", "Materials");
            MaterialTypeLookup.Add("Mechanical Scrap", "Materials");
            MaterialTypeLookup.Add("Mercury", "Materials");
            MaterialTypeLookup.Add("Military Grade Alloys", "Materials");
            MaterialTypeLookup.Add("Military Supercapacitors", "Materials");
            MaterialTypeLookup.Add("Molybdenum", "Materials");
            MaterialTypeLookup.Add("Nickel", "Materials");
            MaterialTypeLookup.Add("Niobium", "Materials");
            MaterialTypeLookup.Add("Pharmaceutical Isolators", "Materials");
            MaterialTypeLookup.Add("Phase Alloys", "Materials");
            MaterialTypeLookup.Add("Phosphorus", "Materials");
            MaterialTypeLookup.Add("Polonium", "Materials");
            MaterialTypeLookup.Add("Polymer Capacitors", "Materials");
            MaterialTypeLookup.Add("Precipitated Alloys", "Materials");
            MaterialTypeLookup.Add("Proprietary Composites", "Materials");
            MaterialTypeLookup.Add("Proto Heat Radiators", "Materials");
            MaterialTypeLookup.Add("Proto Light Alloys", "Materials");
            MaterialTypeLookup.Add("Proto Radiolic Alloys", "Materials");
            MaterialTypeLookup.Add("Refined Focus Crystals", "Materials");
            MaterialTypeLookup.Add("Ruthenium", "Materials");
            MaterialTypeLookup.Add("Salvaged Alloys", "Materials");
            MaterialTypeLookup.Add("Selenium", "Materials");
            MaterialTypeLookup.Add("Shield Emitters", "Materials");
            MaterialTypeLookup.Add("Shielding Sensors", "Materials");
            MaterialTypeLookup.Add("Sulphur", "Materials");
            MaterialTypeLookup.Add("Technetium", "Materials");
            MaterialTypeLookup.Add("Tellurium", "Materials");
            MaterialTypeLookup.Add("Tempered Alloys", "Materials");
            MaterialTypeLookup.Add("Thermic Alloys", "Materials");
            MaterialTypeLookup.Add("Tin", "Materials");
            MaterialTypeLookup.Add("Tungsten", "Materials");
            MaterialTypeLookup.Add("Unknown Fragment", "Materials");
            MaterialTypeLookup.Add("Vanadium", "Materials");
            MaterialTypeLookup.Add("Worn Shield Emitters", "Materials");
            MaterialTypeLookup.Add("Yttrium", "Materials");
            MaterialTypeLookup.Add("Zinc", "Materials");
            MaterialTypeLookup.Add("Zirconium", "Materials");
            MaterialTypeLookup.Add("Aberrant Shield Pattern Analysis", "Data");
            MaterialTypeLookup.Add("Abnormal Compact Emission Data", "Data");
            MaterialTypeLookup.Add("Adaptive Encryptors Capture", "Data");
            MaterialTypeLookup.Add("Anomalous Bulk Scan Data", "Data");
            MaterialTypeLookup.Add("Anomalous FSD Telemetry", "Data");
            MaterialTypeLookup.Add("Atypical Disrupted Wake Echoes", "Data");
            MaterialTypeLookup.Add("Atypical Encryption Archives", "Data");
            MaterialTypeLookup.Add("Classified Scan Databanks", "Data");
            MaterialTypeLookup.Add("Classified Scan Fragment", "Data");
            MaterialTypeLookup.Add("Cracked Industrial Firmware", "Data");
            MaterialTypeLookup.Add("Datamined Wake Exceptions", "Data");
            MaterialTypeLookup.Add("Decoded Emission Data", "Data");
            MaterialTypeLookup.Add("Distorted Shield Cycle Recordings", "Data");
            MaterialTypeLookup.Add("Divergent Scan Data", "Data");
            MaterialTypeLookup.Add("Eccentric Hyperspace Trajectories", "Data");
            MaterialTypeLookup.Add("Exceptional Scrambled Emission Data", "Data");
            MaterialTypeLookup.Add("Inconsistent Shield Soak Analysis", "Data");
            MaterialTypeLookup.Add("Irregular Emission Data", "Data");
            MaterialTypeLookup.Add("Modified Consumer Firmware", "Data");
            MaterialTypeLookup.Add("Modified Embedded Firmware", "Data");
            MaterialTypeLookup.Add("Open Symmetric Keys", "Data");
            MaterialTypeLookup.Add("Peculiar Shield Frequency Data", "Data");
            MaterialTypeLookup.Add("Security Firmware Patch", "Data");
            MaterialTypeLookup.Add("Specialised Legacy Firmware", "Data");
            MaterialTypeLookup.Add("Strange Wake Solutions", "Data");
            MaterialTypeLookup.Add("Tagged Encryption Codes", "Data");
            MaterialTypeLookup.Add("Unexpected Emission Data", "Data");
            MaterialTypeLookup.Add("Unidentified Scan Archives", "Data");
            MaterialTypeLookup.Add("Untypical Shield Scans", "Data");
            MaterialTypeLookup.Add("Unusual Encrypted Files", "Data");
            MaterialTypeLookup.Add("Articulation Motors", "Commodities");
            MaterialTypeLookup.Add("Bromellite", "Commodities");
            MaterialTypeLookup.Add("CMM Composite", "Commodities");
            MaterialTypeLookup.Add("Emergency Power Cells", "Commodities");
            MaterialTypeLookup.Add("Energy Grid Assembly", "Commodities");
            MaterialTypeLookup.Add("Exhaust Manifold", "Commodities");
            MaterialTypeLookup.Add("Hardware Diagnostic Sensor", "Commodities");
            MaterialTypeLookup.Add("Heatsink Interlink", "Commodities");
            MaterialTypeLookup.Add("HN Shock Mount", "Commodities");
            MaterialTypeLookup.Add("Insulating Membrane", "Commodities");
            MaterialTypeLookup.Add("Ion Distributor", "Commodities");
            MaterialTypeLookup.Add("Magnetic Emitter Coil", "Commodities");
            MaterialTypeLookup.Add("Micro Controllers", "Commodities");
            MaterialTypeLookup.Add("Micro-Weave Cooling Hoses", "Commodities");
            MaterialTypeLookup.Add("Modular Terminals", "Commodities");
            MaterialTypeLookup.Add("Nanobreakers", "Commodities");
            MaterialTypeLookup.Add("Neofabric Insulation", "Commodities");
            MaterialTypeLookup.Add("Osmium", "Commodities");
            MaterialTypeLookup.Add("Platinum", "Commodities");
            MaterialTypeLookup.Add("Power Converter", "Commodities");
            MaterialTypeLookup.Add("Power Transfer Bus", "Commodities");
            MaterialTypeLookup.Add("Praseodymium", "Commodities");
            MaterialTypeLookup.Add("Radiation Baffle", "Commodities");
            MaterialTypeLookup.Add("Reinforced Mounting Plate", "Commodities");
            MaterialTypeLookup.Add("Samarium", "Commodities");
            MaterialTypeLookup.Add("Telemetry Suite", "Commodities");

            // Logs do not reflect the cost of unlocking an engineer,
            // so we look them up
            var mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Felicity Farseer", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Elvira Martuuk", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("The Dweller", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Liz Ryder", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Tod McQuinn", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Zacariah Nemo", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Lei Cheung", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Hera Tani", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Juri Ishmaak", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Selene Jean", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Modular Terminals", -25);
            EngineerCostLookup.Add("Marco Qwent", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Classified Scan Databanks", -50);
            EngineerCostLookup.Add("Ram Tah", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Broo Tarquin", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Colonel Bris Dekker", mats);

            mats = new Dictionary<string, int>();
            EngineerCostLookup.Add("Didi Vatermann", mats);

            mats = new Dictionary<string, int>();
            mats.Add("Unknown Fragment", -25);
            EngineerCostLookup.Add("Professor Palin", mats);

            mats = new Dictionary<string, int>();
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
    }
}
