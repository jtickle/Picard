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
        public int
            DataVersion = 1;
        public List<string>
            MaterialTypes;
        public List<string>
            MaterialOrder;
        public List<string>
            IgnoreCommodities;
        public Dictionary<string, int>
            VersionAdded;
        public Dictionary<string, string>
            EliteMatsLookup;
        public Dictionary<string, string>
            MaterialTypeLookup;
        public Dictionary<string, Dictionary<string, int>>
            EngineerCostLookup;

        private static DataMangler INSTANCE = null;

        public static DataMangler GetInstance()
        {
            if(INSTANCE == null)
            {
                INSTANCE = new DataMangler();
            }

            return INSTANCE;
        }

        private DataMangler()
        {
            MaterialTypes = new List<string>();
            MaterialOrder = new List<string>();
            IgnoreCommodities = new List<string>();
            EliteMatsLookup = new Dictionary<string, string>();
            MaterialTypeLookup = new Dictionary<string, string>();
            EngineerCostLookup = new Dictionary<string, Dictionary<string, int>>();
            VersionAdded = new Dictionary<string, int>();

            // Possible Material Types
            MaterialTypes.Add("Materials");
            MaterialTypes.Add("Data");
            MaterialTypes.Add("Commodities");
            
            using (StringReader r = new StringReader(
                Properties.Resources.EliteInaraLookups))
            {
                string line = "";

                int[] begins  = { -1, -1, -1, -1 };
                int[] lengths = { -1, -1, -1 };
                string name, type, journal;
                int version;

                while ((line = r.ReadLine()) != null)
                {
                    // Comment Line
                    if (line[0] == '#' || line.Trim().Length == 0)
                        continue;

                    // Heading Line
                    if (line[0] == ';')
                    {
                        int i = 0, prev = 0, next = 1;
                        while(i < 3)
                        {
                            begins[i] = prev;
                            next = line.IndexOf(';', next);
                            lengths[i] = next - prev - 1;
                            prev = next;
                            next++;  i++;
                        }

                        begins[i] = prev;
                        
                        continue;
                    }

                    // Data Line
                    if (begins[0] == -1)
                    {
                        throw new Exception("Data Manglement Exception - Lookup Table Corrupted");
                    }

                    name = line.Substring(begins[0], lengths[0]).Trim();
                    type = line.Substring(begins[1], lengths[1]).Trim();
                    journal = line.Substring(begins[2], lengths[2]).Trim();
                    var derp = line.Substring(begins[3]).Trim();
                    version = int.Parse(derp);

                    if (type == "Irrelevant Commodity")
                    {
                        IgnoreCommodities.Add(name);
                    }
                    else
                    {
                        MaterialOrder.Add(name);
                        EliteMatsLookup.Add(journal, name);
                        MaterialTypeLookup.Add(name, type);
                        VersionAdded.Add(name, version);
                    }
                }

                DataVersion = VersionAdded.Values.Max();
            }

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

        public IEnumerable<string> GetUpdates(IDictionary<string, int> data, int ver)
        {
            return from version in VersionAdded
                   where version.Value > ver
                   select version.Key;
        }
    }
}
