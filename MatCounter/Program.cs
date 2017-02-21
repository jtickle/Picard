using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picard.Lib;
using LibEDJournal;
using LibEDJournal.Handler;
using LibEDJournal.State;

namespace MatCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            var dm = DataMangler.GetInstance();
            var journalPath = Filesystem.GuessJournalPath();
            var logs = new EliteJournalParser(journalPath);
            var handler = new InventoryHandler(dm.EngineerCostLookup);
            
            int data = 0;

            handler.InventoryChanged += (o, e) =>
            {
                if(e.Name == "scandatabanks" && e.Delta != 0) {
                    data += e.Delta;
                    Console.WriteLine("scandatabanks: " + data.ToString());
                }

            };

            foreach (var entry in logs.GetLogEntries())
            {
                entry.Accept(handler);
            }

            // Prevent the console from closing
            Console.ReadLine();
        }
    }
}
