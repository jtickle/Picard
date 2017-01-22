///
/// This file is based upon a response on the JSON.NET CodePlex forums:
///     http://json.codeplex.com/discussions/56031
/// Internet Archive Link of the particular page viewed:
///     http://web.archive.org/web/20160429192643/http://json.codeplex.com/discussions/56031
/// It is from the third response by the user "nonplus" on that page.
/// 
/// This file is part of LibEDJournal.
/// 
/// This file is assumed to be in the Public Domain.
///

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibEDJournal
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract Type GetType(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            Type targetType = GetType(objectType, jObject);

            object target = Activator.CreateInstance(targetType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
