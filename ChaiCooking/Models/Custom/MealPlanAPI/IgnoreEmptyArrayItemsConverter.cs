using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChaiCooking.Models.Custom
{    class IgnoreEmptyArrayItemsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            bool result = typeof(System.Collections.IEnumerable).IsAssignableFrom(objectType);
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var tokenIndexesToRemove = new List<int>();

            var array = JArray.Load(reader);

            for (int i = 0; i < array.Count; i++)
            {
                var obj = array[i];
                if (!obj.HasValues)
                    tokenIndexesToRemove.Add(i);
            }

            foreach (int index in tokenIndexesToRemove)
                array.RemoveAt(index);

            var result = array.ToObject(objectType, serializer);
            return result;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
