using System;
using Newtonsoft.Json.Linq;

namespace LibEDJournal.Entry
{
    public class LoadGame : EliteJournalEntry
    {
        public string Commander;
        public string Ship;
        public int ShipID;
        public string GameMode;
        public string Group;
        public int Credits;
        public int Loan;

        public LoadGame(JObject json) : base(json) { }

        public override void Accept(EliteJournalHandler handler)
        {
            handler.Handle(this);
        }
    }
}
