using System;
using LibEDJournal.Entry;

namespace LibEDJournal
{
    public class ResurrectEventArgs : EventArgs
    {
        public EliteJournalEntry JournalEntry;

        public ResurrectEventArgs(EliteJournalEntry entry)
        {
            JournalEntry = entry;
        }
    }
}
