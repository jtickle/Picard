///
/// Copyright 2017 Jeff Tickle "CMDR VirtualPaper"
/// 
/// This file is part of LibEDJournal.
///
/// LibEDJournal is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// LibEDJournal is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with LibEDJournal.  If not, see<http://www.gnu.org/licenses/>.
///

using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LibEDJournal.Entry
{
    [JsonConverter(typeof(EliteJournalEntryConverter))]
    public class EliteJournalEntry
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp;

        [JsonProperty("event")]
        public string EventName;

        [JsonIgnore]
        public string JournalFile;

        [JsonIgnore]
        public JObject jObject;

        public EliteJournalEntry(JObject json)
        {
            jObject = json;
        }

        public virtual void Accept(EliteJournalHandler handler)
        {
            handler.HandleUnknown(this);
        }
    }
}
