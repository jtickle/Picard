using System;
using LibEDJournal.Entry;

namespace LibEDJournal
{
    public class InventoryEventArgs : EventArgs
    {
        public string Cmdr;
        public string Name;
        public int Delta;
        public EliteJournalEntry JournalEntry;

        public InventoryEventArgs(string cmdr, string name, int delta,
            EliteJournalEntry entry)
        {
            Cmdr = cmdr;
            Name = name;
            Delta = delta;
            JournalEntry = entry;
        }
    }
}
