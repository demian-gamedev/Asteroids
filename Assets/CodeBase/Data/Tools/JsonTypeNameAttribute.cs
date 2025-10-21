using System;

namespace CodeBase.Data.Tools
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonTypeNameAttribute : Attribute
    {
        public string Name { get; }

        public JsonTypeNameAttribute(string name)
        {
            Name = name;
        }
    }
}