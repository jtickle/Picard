using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picard.Lib.JournalEntry;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Picard.Lib
{
    class EliteJournalEntryConverter
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
            var type = Type.GetType((string)jObject["event"]);
            
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
