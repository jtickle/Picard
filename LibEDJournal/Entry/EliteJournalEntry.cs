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
    /// <summary>
    /// The abstract Elite: Dangerous Journal Entry.  It keeps track
    /// of timestamp and event name, as well as a non-serialized
    /// journal file source and the orignial dynamic JSON from the
    /// parser.
    /// </summary>
    [JsonConverter(typeof(EliteJournalEntryConverter))]
    public abstract class EliteJournalEntry
    {
        /// <summary>
        /// The timestamp of the journal entry
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp;

        /// <summary>
        /// The event name, used to determine subclass in deserialization
        /// </summary>
        [JsonProperty("event")]
        public string EventName;

        /// <summary>
        /// The original file source of this journal entry
        /// </summary>
        [JsonIgnore]
        public string JournalFile;

        /// <summary>
        /// The original file line number of this journal entry
        /// </summary>
        [JsonIgnore]
        public int JournalLine;

        /// <summary>
        /// The original JSON data from the event log
        /// </summary>
        [JsonIgnore]
        public JObject jObject;

        /// <summary>
        /// All subclasses must be instiantiated with their orignial
        /// JSON data
        /// </summary>
        /// <param name="json"></param>
        public EliteJournalEntry(JObject json)
        {
            jObject = json;
        }

        public abstract void Accept(EliteJournalHandler handler);
    }
}
