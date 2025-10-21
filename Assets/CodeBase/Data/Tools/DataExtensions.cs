using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Data.StatsSystem.Main;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Converters;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace CodeBase.Data.Tools
{
    public static class DataExtensions
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(),
                new IStatConverter() 
            },
            Formatting = Formatting.Indented, // For better readability
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string ToJson(this object obj) => 
            JsonConvert.SerializeObject(obj, Settings);

        public static T ToDeserialized<T>(this string json) =>
            JsonConvert.DeserializeObject<T>(json, Settings);
        
        public static Type FindTypeByName(string name)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => 
                    t.GetCustomAttribute<JsonTypeNameAttribute>()?.Name == name && 
                    typeof(IStat).IsAssignableFrom(t));
        
            return type ?? AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == name && typeof(IStat).IsAssignableFrom(t));
        }
    }
}