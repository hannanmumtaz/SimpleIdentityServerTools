namespace SimpleIdentityServer.Eid.Ehealth.Tlv
{
    public sealed class Address
    {
        [TlvField(1)]
        public string StreetAndNumber { get; set; }
        [TlvField(2)]
        public string Zip { get; set; }
        [TlvField(3)]
        public string Municipality { get; set; }
    }
}
