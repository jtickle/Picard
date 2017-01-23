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
using LibEDJournal.Entry;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LibEDJournal
{
    public class EliteJournalEntryConverter
        : JsonCreationConverter<EliteJournalEntry>
    {
        protected override Type GetType(Type objectType, JObject jObject)
        {
            // If the entry does not have an event property,
            // it's not really a journal entry and something is wrong
            var hasEventProperty = false;
            foreach (var property in jObject.Properties())
            {
                if (property.Name == "event")
                {
                    hasEventProperty = true;
                    break;
                }
            }
            if (!hasEventProperty)
                throw new JsonException("Journal entry does not contain key 'event': " + jObject.ToString());

            // I had started on a switch statement but just decided to
            // convert the string to type, shoot me
            var type = Type.GetType("LibEDJournal.Entry." + (string)jObject["event"]);
            
            if(type == null)
            {
                // If we didn't find the type, we don't know that one yet
                return typeof(EliteJournalEntry);
            }

            // Found the type
            return type;
        }
    }
}
