using SimpleIdentityServer.Eid.Commands;
using SimpleIdentityServer.Eid.Exceptions;
using PcscDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SimpleIdentityServer.Eid
{
    public enum AlgorithmReferences
    {
        RSASSAPSS_SHA1,
        RSASSAPKCS1v15_SHA1,
        RSASSAPKCS1v15_MD5,
        RSAESPKCS1v15,
        RSAESOAEP,
        RSAKEM
    }

    public interface IBeIdCardConnector
    {
        IEnumerable<string> GetReaders();
        PcscConnection Connect(string readerName);
        PcscContext EstablishContext();
        X509Certificate GetSigningCertificate();
        X509Certificate GetAuthenticateCertificate();
        byte[] GetPhoto();
        ResponseAPDU VerifyPin(string pin);
        ResponseAPDU VerifyPin(char[] pin);
        byte[] SignWithNoneRepudationCertificate(byte[] digestValue, BeIDDigest beIDDigest, bool requireSecureReader, string applicationName, string pin);
    }

    public class BeIdCardConnector : IDisposable, IBeIdCardConnector
    {
        private readonly Dictionary<AlgorithmReferences, int> _mappingAlgorithmRefToNumber = new Dictionary<AlgorithmReferences, int>
        {
            { AlgorithmReferences.RSASSAPSS_SHA1, 1 },
            { AlgorithmReferences.RSASSAPKCS1v15_SHA1, 2 },
            { AlgorithmReferences.RSASSAPKCS1v15_MD5, 4 },
            { AlgorithmReferences.RSAESPKCS1v15, 144 },
            { AlgorithmReferences.RSAESOAEP, 160 },
            { AlgorithmReferences.RSAKEM, 176 }
        };
        private static int _maxBufferSize = 10000;
        private PcscContext _context;
        private PcscConnection _connection;

        public BeIdCardConnector() { }

        public PcscContext EstablishContext()
        {
            if (_context != null)
            {
                throw new ConnectionException(Constants.ErrorCodes.ContextExists);
            }
            
            var platform = Environment.OSVersion.Platform;
            if (
                platform == PlatformID.Win32S ||
                platform == PlatformID.Win32Windows ||
                platform == PlatformID.Win32NT ||
                platform == PlatformID.WinCE)
            {
                _context = Pcsc<WinSCard>.EstablishContext(SCardScope.User);
            }
            else
            {
                _context = Pcsc<PCSCliteAPI>.EstablishContext(SCardScope.User);
            }

            return _context;
        }

        public IEnumerable<string> GetReaders()
        {
            CheckContextEstablished();
            return _context.GetReaderNames();
        }

        public PcscConnection Connect(string readerName)
        {
            if (string.IsNullOrWhiteSpace(readerName))
            {
                throw new ArgumentNullException(nameof(readerName));
            }

            CheckContextEstablished();
            if (_connection != null && _connection.IsConnect)
            {
                throw new ConnectionException(Constants.ErrorCodes.ConnectionExists);
            }

            _connection = _context.Connect(readerName, SCardShare.Shared, SCardProtocols.T0);
            return _connection;
        }

        public void Disconnect()
        {
            if (_connection != null)
            {
                if (_connection.IsConnect) { _connection.Disconnect(); }
                _connection.Dispose();
            }

            if (_context != null)
            {
                _context.Release();
                _context.Dispose();
            }
        }

        #region Get files

        public X509Certificate GetSigningCertificate()
        {
            var signingCertificate = ReadFile(FileType.NonRepudiationCertificate);
            return new X509Certificate(signingCertificate);
        }

        public X509Certificate GetAuthenticateCertificate()
        {
            var authenticateCertificate = ReadFile(FileType.AuthentificationCertificate);
            return new X509Certificate(authenticateCertificate);
        }

        public byte[] GetPhoto()
        {
            return ReadFile(FileType.Photo);
        }

        public byte[] GetAddress()
        {
            return ReadFile(FileType.Address);
        }

        public byte[] GetIdentity()
        {
            return ReadFile(FileType.Identity);
        }

        #endregion

        #region Set command

        public void Set()
        {
            // Manage security environment.
        }

        #endregion

        #region Sign

        public byte[] SignWithNoneRepudationCertificate(byte[] digestValue, BeIDDigest beIDDigest, bool requireSecureReader, string applicationName, string pin)
        {
            if (digestValue == null)
            {
                throw new ArgumentNullException(nameof(digestValue));
            }

            if (beIDDigest == null)
            {
                throw new ArgumentNullException(nameof(beIDDigest));
            }

            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var fileType = FileType.NonRepudiationCertificate;
            byte[] result = null;
            BeginExclusive();
            try
            {
                var responseApdu = SendCommand(BeIDCommandAPDU.SELECT_ALGORITHM_AND_PRIVATE_KEY, new byte[] // Select the key & algorithm.
                {
                    4,
                    128,
                    beIDDigest.AlgorithmReference,
                    132,
                    fileType.KeyId
                });
                var verifyPinResponse = VerifyPin(pin);    // Check the PIN.
                var data = new List<byte>();
                data.AddRange(beIDDigest.GetPrefix(digestValue.Length));
                data.AddRange(digestValue);
                var resultCommand = SendCommand(BeIDCommandAPDU.COMPUTE_DIGITAL_SIGNATURE, data.ToArray()); // Compute the signature.
                result = resultCommand.GetApdu();
                // var length = result.Last();
                var getResponse = GetResponse(128);
                result = getResponse.GetData();
            }
            finally
            {
                EndExclusive();
            }

            return result;
        }

        #endregion

        #region Verify pin

        public ResponseAPDU VerifyPin(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            return VerifyPin(pin.ToArray());
        }

        public ResponseAPDU VerifyPin(char[] pin)
        {
            if (pin == null)
            {
                throw new ArgumentNullException(nameof(pin));
            }

            ResponseAPDU result = null;
            var data = new byte[]
            {
                    (byte)(32 | pin.Length),
                    byte.MaxValue,
                    byte.MaxValue,
                    byte.MaxValue,
                    byte.MaxValue,
                    byte.MaxValue,
                    byte.MaxValue,
                    byte.MaxValue
            };
            int index = 0;
            while (index < pin.Length)
            {
                int num2 = (int)(sbyte)((pin[index] - 48 << 4) + ((index + 1 >= pin.Length ? 63 : pin[index + 1]) - 48));
                data[index / 2 + 1] = (byte)num2;
                index += 2;
            }
            
            result = SendCommand(BeIDCommandAPDU.VERIFY_PIN, data);

            return result;
        }

        #endregion

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.IsConnect) { _connection.Disconnect(); }
                _connection.Dispose();
            }

            if (_context != null)
            {
                _context.Release();
                _context.Dispose();
            }
        }

        private byte[] ReadFile(FileType file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            CheckContextEstablished();
            CheckConnection();
            BeginExclusive();
            byte[] result = null;
            try
            {
                SendCommand(BeIDCommandAPDU.SELECT_FILE, file.FileId); // 1. Select a file.
                result = ReadBinary(file); // 2. Read the binaries.
            }
            finally
            {
                EndExclusive();
            }

            return result;
        }

        private byte[] ReadBinary(FileType fileType)
        {
            var result = new List<byte>();
            int num = 0;
            while (true)
            {
                var length = fileType.EstimatedMaxSize > byte.MaxValue ? byte.MaxValue : fileType.EstimatedMaxSize;
                var apduResult = SendCommand(BeIDCommandAPDU.READ_BINARY, num >> 8, num & byte.MaxValue, length);
                int sw = apduResult.GetSw();
                var data = apduResult.GetData();
                result.AddRange(data);
                num += data.Length;
                if (data.Length == byte.MaxValue)
                {
                    continue;
                }

                return result.ToArray();
            }
        }

        private ResponseAPDU GetResponse(int length)
        {
            return SendCommand(BeIDCommandAPDU.GET_RESPONSE, 0, 0, length);
        }

        private void BeginExclusive()
        {
            var error = _context.Provider.SCardBeginTransaction(_connection.Handle);
            if (error != SCardError.Successs)
            {
                throw new ConnectionException(Constants.ErrorCodes.CannotBeginTransaction);
            }
        }

        private void EndExclusive()
        {
            _context.Provider.SCardEndTransaction(_connection.Handle, SCardDisposition.Leave);
        }

        private ResponseAPDU SendCommand(BeIDCommandAPDU command, byte[] data)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var commandAdpu = new CommandAPDU(command.Cla, command.Ins, command.P1, command.P2, data);
            return Transmit(commandAdpu, _maxBufferSize);
        }

        private ResponseAPDU SendCommand(BeIDCommandAPDU command, int p1, int p2, int le)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var commandAdpu = new CommandAPDU(command.Cla, command.Ins, p1, p2, le);
            return Transmit(commandAdpu, _maxBufferSize);
        }

        private ResponseAPDU Transmit(CommandAPDU commandAdpu, int bufferSize)
        {
            if (commandAdpu == null)
            {
                throw new ArgumentNullException(nameof(commandAdpu));
            }

            var result = _connection.Transmit(commandAdpu.Adpu, bufferSize);
            return new ResponseAPDU(result);
        }

        private void CheckContextEstablished()
        {
            if (_context == null || !_context.IsEstablished)
            {
                throw new ConnectionException(Constants.ErrorCodes.NoEstablishedContext);
            }
        }

        private void CheckConnection()
        {
            if (_connection == null || !_connection.IsConnect)
            {
                throw new ConnectionException(Constants.ErrorCodes.NoEstablishedConnection);
            }
        }
    }
}
