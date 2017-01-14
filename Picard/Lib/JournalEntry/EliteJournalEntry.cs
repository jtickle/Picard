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
        public DateTime Timestamp { get; internal set; }

        [JsonProperty("event")]
        public string EventName { get; internal set; }

        [JsonIgnore]
        public string JournalFile { get; internal set; }

        [JsonIgnore]
        public JObject jObject { get; internal set; }

        public EliteJournalEntry(JObject json)
        {
            jObject = json;
        }
    }
}
