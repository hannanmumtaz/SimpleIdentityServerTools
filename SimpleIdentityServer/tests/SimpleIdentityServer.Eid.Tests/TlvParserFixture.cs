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
            var secondHexStr = "010a423233333633323738320210534c47905100000061767d2e12925169030a30372e30382e32303135040a30372e30382e323032300514576f6c7577652d5361696e742d4c616d62657274060b3737303131353439313034070a446f6e6162656469616e080e4461766964204ac3a972c3b46d6509000a064672616e63650b0f4169782d456e2d50726f76656e63650c0c3135204a414e2020313937370d014d0e000f0231361001301114e23a1410ffd7d3";
            var file = StringToByteArray(hexStr);
            var secondFile = StringToByteArray(secondHexStr);
            var tlvParser = new TlvParser();

            // ACT
            var identity = tlvParser.Parse<Identity>(file);
            var secondIdentity = tlvParser.Parse<Identity>(secondFile);

            // ASSERT
            Assert.NotNull(identity);
            Assert.NotNull(secondIdentity);
            Assert.Equal("89100739573", identity.NationalNumber);
        }

        [Fact]
        public void WhenDeserializeAddressTlvThenDataAreCorrect()
        {
            // ARRANGE
            var hexStr = "011c4176656e7565206465732043726f6978206475204665752032323320020431303230030942727578656c6c65730000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            var secondHexStr = "010e52756520447269657320323037200204313230300314576f6c7577652d5361696e742d4c616d626572740000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            var file = StringToByteArray(hexStr);
            var secondFile = StringToByteArray(secondHexStr);
            var tlvParser = new TlvParser();

            // ACT
            var address = tlvParser.Parse<Address>(file);
            var secondAddress = tlvParser.Parse<Address>(secondFile);

            // ASSERT
            Assert.NotNull(address);
            Assert.NotNull(secondAddress);
            Assert.Equal("Bruxelles", address.Municipality);
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
