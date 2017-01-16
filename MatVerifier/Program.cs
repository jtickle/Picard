using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picard.Lib;
using Picard.Lib.JournalEntry;

namespace MatVerifier
{
    class Program
    {
        static void Main(string[] args)
        {
            EliteJournalParser logs = new EliteJournalParser();
            EliteJournalDebugHandler handler = new EliteJournalDebugHandler();

            logs.HandleLogEntries(handler);

            foreach (var evt in handler.UnknownEvents)
            {
                Console.WriteLine("Unknown Event: " + evt);
            }
            Console.ReadLine();
        }
    }
}
