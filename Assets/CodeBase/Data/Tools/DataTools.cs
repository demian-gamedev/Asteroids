using System;
using System.Linq;
using System.Reflection;
using CodeBase.Data.StatsSystem.Main;

namespace CodeBase.Data.Tools
{
    public static class DataTools
    {
        // can be optimized with a dictionary by caching the results
        public static Type FindStatTypeByName(string name)
        {
            // search for the type by JsonTypeNameAttribute first
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => 
                    t.GetCustomAttribute<JsonTypeNameAttribute>()?.Name == name && 
                    typeof(IStat).IsAssignableFrom(t));
        
            // if not found, search by type name
            return type ?? AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == name && typeof(IStat).IsAssignableFrom(t));
        }
        public static string GetStatTypeName(Type type)
        {
            var typeNameAttr = type.GetCustomAttribute<JsonTypeNameAttribute>();
            return typeNameAttr?.Name ?? type.Name;
        }
    }
}