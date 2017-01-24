using System;
using LibEDJournal.Entry;

namespace LibEDJournal
{
    public class DeathEventArgs : EventArgs
    {
        public EliteJournalEntry JournalEntry;

        public DeathEventArgs(EliteJournalEntry entry)
        {
            JournalEntry = entry;
        }
    }
}
