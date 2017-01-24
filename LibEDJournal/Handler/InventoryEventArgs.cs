using System;
using LibEDJournal.Entry;

namespace LibEDJournal
{
    public class InventoryEventArgs : EventArgs
    {
        public string Name;
        public int Delta;
        public EliteJournalEntry JournalEntry;

        public InventoryEventArgs(string name, int delta,
            EliteJournalEntry entry)
        {
            Name = name;
            Delta = delta;
            JournalEntry = entry;
        }
    }
}
