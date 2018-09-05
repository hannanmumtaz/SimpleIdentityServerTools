using System;

namespace SimpleIdentityServer.Eid.Ehealth.Tlv
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TlvFieldAttribute : Attribute
    {
        public TlvFieldAttribute(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}
