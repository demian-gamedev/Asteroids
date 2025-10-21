using System;
using System.Linq;
using System.Reflection;
using CodeBase.Data.StatsSystem.Main;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace CodeBase.Data.Tools
{
    public class IStatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IStat).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            JObject jsonObject = JObject.Load(reader);
            
            if (objectType != typeof(IStat))
            {
                 var instance = Activator.CreateInstance(objectType);
                 serializer.Populate(jsonObject.CreateReader(), instance);
                 return instance;
            }

            if (jsonObject.Properties().Any(p => p.Name.Equals("Value", StringComparison.OrdinalIgnoreCase)))
            {
                var typeName = objectType.Name;
                if (objectType.IsGenericType)
                {
                    typeName = objectType.GenericTypeArguments[0].Name;
                }
                
                var type = DataTools.FindStatTypeByName(typeName);
                if (type == null) return null;
                
                var instance = Activator.CreateInstance(type);
                serializer.Populate(jsonObject.CreateReader(), instance);
                return instance;
            }
            
            var typeProperty = jsonObject.Properties().First();
            var typeNameFromJson = typeProperty.Name;
            
            var typeFromJson = DataTools.FindStatTypeByName(typeNameFromJson);
            if (typeFromJson == null)
                throw new JsonSerializationException($"Unknown type: {typeNameFromJson}");
            
            var instanceFromJson = Activator.CreateInstance(typeFromJson);
            serializer.Populate(typeProperty.Value.CreateReader(), instanceFromJson);
            
            return instanceFromJson;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            
            // create a temporary serializer to avoid recursion
            var tempSerializer = JsonSerializer.CreateDefault();
            tempSerializer.Formatting = serializer.Formatting;
            tempSerializer.NullValueHandling = serializer.NullValueHandling;
            tempSerializer.DefaultValueHandling = serializer.DefaultValueHandling;
            
            // copy existing converters
            foreach (var converter in serializer.Converters)
            {
                if (converter.GetType() != this.GetType())
                {
                    tempSerializer.Converters.Add(converter);
                }
            }
            
            if (IsNestedContext(writer))
            {
                var jObject = JObject.FromObject(value, tempSerializer);
                jObject.WriteTo(writer);
                return;
            }
            
            // for root context, include type name
            var typeName = DataTools.GetStatTypeName(type);
    
            writer.WriteStartObject();
            writer.WritePropertyName(typeName);
            
            // for properties
            var propsObject = new JObject();
            var properties = type.GetProperties()
                .Where(p => p.CanRead && p.GetCustomAttribute<JsonIgnoreAttribute>() == null);
    
            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(value);
                if (propValue != null)
                {
                    propsObject.Add(ToCamelCase(prop.Name), JToken.FromObject(propValue, tempSerializer));
                }
            }
            
            propsObject.WriteTo(writer);
            writer.WriteEndObject();
        }

        private bool IsNestedContext(JsonWriter writer)
        {
            // check if inside Stats
            return !string.IsNullOrEmpty(writer.Path) && 
                   (writer.Path.Contains("stats") || writer.Path.Contains('.'));
        }

        private string ToCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }
}
