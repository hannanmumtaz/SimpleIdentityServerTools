namespace SimpleIdentityServer.Eid.Ehealth.Tlv
{
    public class Identity
    {
        [TlvField(1)]
        public string CardNumber { get; set; }
        [TlvField(2)]
        public string ChipNumber { get; set; }
        [TlvField(5)]
        public string CardDeliveryMunicipality { get; set; }
        [TlvField(6)]
        public string NationalNumber { get; set; }
        [TlvField(7)]
        public string Name { get; set; }
        [TlvField(8)]
        public string FirstName { get; set; }
        [TlvField(9)]
        public string MiddleName { get; set; }
        [TlvField(10)]
        public string Nationality { get; set; }
        [TlvField(11)]
        public string PlaceOfBirth { get; set; }
        [TlvField(13)]
        public string Gender { get; set; }
    }
}
