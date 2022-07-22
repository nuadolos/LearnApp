using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Helper.Serialization
{
    static public class JsonSerializer
    {
        private static JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings
        {
            Converters = { new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeLocal } },
            DateParseHandling = DateParseHandling.DateTimeOffset,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string ToJson(this object json) =>
            JsonConvert.SerializeObject(json, JsonSerializerSettings);

        public static T FromJson<T>(this string json) =>
            JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings)!;

    }
}
