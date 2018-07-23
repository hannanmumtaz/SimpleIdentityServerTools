namespace SimpleIdentityServer.Eid
{
    public class FileType
    {
        private readonly string _name;
        private readonly int _id;
        private readonly byte[] _fileId;
        private readonly byte _keyId;
        private readonly int _estimatedMaxSize;

        public static FileType Identity = new FileType("Identity", 0, new byte[]
        {
            63,
            0,
            223,
            1,
            64,
            49
        }, 179);
        public static FileType IdentitySignature = new FileType("IdentitySignature", 1, new byte[]
        {
            63,
            0,
            223,
            1,
            64,
            50
        }, 128);
        public static FileType Address = new FileType("Address", 2, new byte[]
        {
            63,
            0,
            223,
            1,
            64,
            51

        }, 121);
        public static FileType AddressSignature = new FileType("AddressSignature", 3, new byte[]
        {
            63,
            0,
            223,
            1,
            64,
            52
        }, 128);
        public static FileType Photo = new FileType("Photo", 4, new byte[]
        {
            63,
            0,
            223,
            1,
            64,
            53
        }, 3064);
        public static FileType AuthentificationCertificate = new FileType("AuthentificationCertificate", 5, new byte[]
        {
            63,
            0,
            223,
            0,
            80,
            56
        }, 1061, 130);
        public static FileType NonRepudiationCertificate = new FileType("NonRepudiationCertificate", 6, new byte[]
        {
            63,
            0,
            223,
            0,
            80,
            57
        }, 1082, 131);
        public static FileType CACertificate = new FileType("CACertificate", 7, new byte[]
        {
            63,
            0,
            223,
            0,
            80,
            58
        }, 1044);
        public static FileType RootCertificate = new FileType("RootCertificate", 8, new byte[]
        {
            63,
            0,
            223,
            0,
            80,
            59
        }, 914);
        public static FileType RRNCertificate = new FileType("RRNCertificate", 9, new byte[]
        {
            63,
            0,
            223,
            0,
            80,
            60
        }, 820);

        private FileType(string name, int id)
        {
            _name = name;
            _id = id;
        }

        private FileType(string name, int id, byte[] fileId, int estimatedMaxSize) : this(name, id)
        {
            FileType fileType = this;
            _fileId = fileId;
            _estimatedMaxSize = estimatedMaxSize;
            _keyId = byte.MaxValue;
        }

        private FileType(string name, int id, byte[] fileId, int estimatedMaxSize, byte keyId) : this(name, id, fileId, estimatedMaxSize)
        {
            _keyId = keyId;
        }

        public string Name
        {
            get { return _name; }
        }

        public byte[] FileId
        {
            get { return _fileId; }
        }

        public int EstimatedMaxSize
        {
            get { return _estimatedMaxSize; }
        }

        public byte KeyId
        {
            get { return _keyId; }
        }
    }
}
