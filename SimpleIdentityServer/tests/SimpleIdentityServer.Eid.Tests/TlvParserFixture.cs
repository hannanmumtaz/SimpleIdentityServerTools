using SimpleIdentityServer.Eid.Ehealth.Tlv;
using System;
using System.Linq;
using Xunit;

namespace SimpleIdentityServer.Eid.Tests
{
    public class TlvParserFixture
    {
        [Fact]
        public void WhenDeserializeIdentityTlvThenDataAreCorrect()
        {
            // ARRANGE
            var hexStr = "010a423137343739383734370210534c494e33660013930d2388e90e2f36030a33302e30312e32303134040a33302e30312e32303139050942727578656c6c6573060b38393130303733393537330706486162617274080e5468696572727920526f6265727409000a064672616e63650b0b52616d626f75696c6c65740c0c3037204f43542020313938390d014d0e000f02313510013011140b03673050c41f10d5085d002afd27499b805cb1120230301300";
            var file = StringToByteArray(hexStr);
            var tlvParser = new TlvParser();

            // ACT
            var identity = tlvParser.Parse<Identity>(file);

            // ASSERT
            Assert.NotNull(identity);
            Assert.Equal("89100739573", identity.NationalNumber);
        }

        [Fact]
        public void WhenDeserializeAddressTlvThenDataAreCorrect()
        {
            // ARRANGE
            var hexStr = "011c4176656e7565206465732043726f6978206475204665752032323320020431303230030942727578656c6c65730000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"; var file = StringToByteArray(hexStr);
            var tlvParser = new TlvParser();

            // ACT
            var address = tlvParser.Parse<Address>(file);

            // ASSERT
            Assert.NotNull(address);
            Assert.Equal("Bruxelles", address.Municipality);
            Assert.Equal("Avenue des Croix du Feu 223", address.StreetAndNumber);
            Assert.Equal("1020", address.Zip);
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
