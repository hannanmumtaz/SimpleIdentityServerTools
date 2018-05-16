using SimpleIdentityServer.Eid.Exceptions;
using SimpleIdentityServer.Eid.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Eid
{
    public class ResponseAPDU
    {
        private static Dictionary<byte[], string> _mappingAdpuResponseToErrorNames = new Dictionary<byte[], string>
        {
            { new byte[] { 98, 131 }, Constants.ErrorCodes.SelectedFileNotActivated }, // 62 83
            { new byte[] { 100, 131 }, Constants.ErrorCodes.NoPreciseDiagnostic }, // 64 00
            { new byte[] { 101, 129 }, Constants.ErrorCodes.EEPromCorrupted }, // 65 81
            { new byte[] { 106, 130 }, Constants.ErrorCodes.SelectedFileNotActivated }, // 6A 82
            { new byte[] { 106, 134 }, Constants.ErrorCodes.SelectedFileNotActivated }, // 6A 86
            { new byte[] { 106, 135 }, Constants.ErrorCodes.SelectedFileNotActivated }, // 6A 87 
            { new byte[] { 109, 0 }, Constants.ErrorCodes.SelectedFileNotActivated }, // 6D 00
            { new byte[] { 110, 0 }, Constants.ErrorCodes.SelectedFileNotActivated } // 6E 00
        };

        private readonly byte[] _apdu;

        public ResponseAPDU(byte[] apdu)
        {
            Check(apdu);
            _apdu = apdu;
        }

        public byte[] GetApdu()
        {
            return _apdu;
        }

        public int GetSW1()
        {
            return _apdu[_apdu.Length - 2];
        }

        public int GetSW2()
        {
            return _apdu[_apdu.Length - 1];
        }

        public int GetSw()
        {
            return GetSW1() << 8 | GetSW2();
        }

        public int GetNr()
        {
            return _apdu.Length - 2;
        }

        public byte[] GetData()
        {
            byte[] numArray = new byte[_apdu.Length - 2];
            ByteCodeHelper.ArrayCopy(_apdu, 0, numArray, 0, numArray.Length);
            return numArray;
        }

        private static void Check(byte[] apdu)
        {
            if (apdu.Count() != 2)
            {
                return;
            }

            var rec = _mappingAdpuResponseToErrorNames.FirstOrDefault(kvp => kvp.Key.SequenceEqual(apdu));
            if (!rec.Equals(default(KeyValuePair<byte[], string>)) && !string.IsNullOrWhiteSpace(rec.Value))
            {
                throw new TransmitException(rec.Value);
            }
        }
    }
}
