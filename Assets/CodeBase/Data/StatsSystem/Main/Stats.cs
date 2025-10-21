using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CodeBase.Data.Tools;

namespace CodeBase.Data.StatsSystem.Main
{
    [Serializable]
    public class Stats : ISerializable
    {
        private Dictionary<Type, IStat> _dictionary;
        
        public Stats()
        {
            _dictionary = new Dictionary<Type, IStat>();
        }
        
        public Stats(SerializationInfo info, StreamingContext context)
        {
            _dictionary = new Dictionary<Type, IStat>();
            foreach (var entry in info)
            {
                Type type = DataTools.FindStatTypeByName(entry.Name);
                if (type != null && typeof(IStat).IsAssignableFrom(type))
                {
                    IStat stat = (IStat)info.GetValue(entry.Name, type);
                    _dictionary.Add(type, stat);
                }
            }
        }
        
        public void AddStat<T>(T stat) where T : IStat
        {
            Type type = typeof(T);
            if (!_dictionary.ContainsKey(type))
            {
                _dictionary.Add(type, stat);
            }
            else
            {
                throw new InvalidOperationException($"Stat of type {type} already exists.");
            }
        }
        
        public T GetStat<T>() where T : IStat
        {
            Type type = typeof(T);
            if (_dictionary.TryGetValue(type, out IStat stat))
            {
                return (T)stat;
            }
            else
            {
                throw new KeyNotFoundException($"Stat of type {type} not found.");
            }
        }
        
        public bool ContainsStat<T>() where T : IStat
        {
            Type type = typeof(T);
            return _dictionary.ContainsKey(type);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in _dictionary)
            {
                string typeName = DataTools.GetStatTypeName(kvp.Key);
                info.AddValue(typeName, kvp.Value, kvp.Key);
            }
        }
    }
}