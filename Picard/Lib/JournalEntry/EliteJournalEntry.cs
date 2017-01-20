using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Picard.Lib.JournalEntry
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
