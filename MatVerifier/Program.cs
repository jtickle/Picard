using System;
using System.Collections.Generic;
using LibEDJournal;
using LibEDJournal.State;
using LibEDJournal.Handler;

namespace MatVerifier
{
    class Program
    {
        static void Main(string[] args)
        {
            EliteJournalParser logs = new EliteJournalParser();
            DataMangler dm = DataMangler.GetInstance();
            InventorySet Deltas = new InventorySet();
            IDictionary<string, IList<EliteJournalEntry>> MatSeen =
                new Dictionary<string, IList<EliteJournalEntry>>();

            EliteJournalDebugHandler dhandler
                = new EliteJournalDebugHandler();

            InventoryHandler mhandler
                = new InventoryHandler(dm.EngineerCostLookup);

            mhandler.InventoryChanged += (object sender, InventoryEventArgs e) =>
            {
                Deltas.AddMat(e.Name, e.Delta);
                if (!MatSeen.ContainsKey(e.Name))
                {
                    MatSeen[e.Name] = new List<EliteJournalEntry>();
                }
                MatSeen[e.Name].Add(e.JournalEntry);
            };

            var handlers = new List<EliteJournalHandler>() { dhandler, mhandler };
            
            logs.HandleLogEntries(handlers);

            #region Show unknown log entries

            foreach (var evt in dhandler.UnknownEvents)
            {
                Console.WriteLine("Unknown Event: " + evt);
            }

            #endregion

            #region Show confirmed material names

            var deltas = new InventorySet();
            var seen = new Dictionary<string, IList<EliteJournalEntry>>();

            // Normalize the names to lowercase
            foreach(var mat in Deltas)
            {
                var name = mat.Key.ToLower();
                deltas.AddMat(name, mat.Value);

                if(!seen.ContainsKey(name))
                {
                    seen[name] = new List<EliteJournalEntry>();
                }

                // Insert seen events in the appropriate order
                var i = 0;
                foreach (var e in MatSeen[mat.Key])
                {
                    bool wasInserted = false;

                    while (i < seen[name].Count)
                    {
                        if (seen[name][i].Timestamp > e.Timestamp)
                        {
                            seen[name].Insert(i, e);
                            wasInserted = true;
                            break;
                        }
                        i++;
                    }

                    if(!wasInserted)
                    {
                        seen[name].Add(e);
                    }
                }
            }

            foreach (var mat in deltas)
            {
                if (dm.IgnoreCommodities.Contains(mat.Key))
                {
                    Console.WriteLine("\"Ignored Commodity\",\"{0}\",{1},\"{2}\"",
                        mat.Key.ToLower(), mat.Value,
                        seen[mat.Key][0].Timestamp);
                }
            }

            foreach (var mat in deltas)
            {
                if (!dm.IgnoreCommodities.Contains(mat.Key))
                {
                    Console.WriteLine("\"Confirmed Material\",\"{0}\",{1},\"{2}\"",
                        mat.Key.ToLower(), mat.Value,
                        seen[mat.Key][0].Timestamp);
                }
            }

            #endregion

            // Prevent the console from closing
            Console.ReadLine();
        }
    }
}
