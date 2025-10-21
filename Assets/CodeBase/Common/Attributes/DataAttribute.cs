using System;

namespace CodeBase.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataAttribute : Attribute
    {
        public string Id { get; }

        public DataAttribute(string id)
        {
            Id = id;
        }
    }
}