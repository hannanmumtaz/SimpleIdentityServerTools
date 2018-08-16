namespace SimpleIdentityServer.DocumentManagement.EF.Models
{

    /// <summary>
    /// Key types
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// Ellipse Curve
        /// </summary>
        EC,
        /// <summary>
        /// RSA
        /// </summary>
        RSA,
    }

    /// <summary>
    /// Algorithm for JWE
    /// </summary>
    public enum AllAlg
    {
        RSA1_5,
        RSA_OAEP,
        RSA_OAEP_256,
        A128KW,
        A192KW,
        A256KW,
        DIR,
        ECDH_ES,
        ECDH_ESA_128KW,
        ECDH_ESA_192KW,
        ECDH_ESA_256_KW,
        A128GCMKW,
        A192GCMKW,
        A256GCMKW,
        PBES2_HS256_A128KW,
        PBES2_HS384_A192KW,
        PBES2_HS512_A256KW
    }

    public class OfficeDocumentJsonWebKey
    {
        /// <summary>
        /// Gets or sets the KID (key id). 
        /// </summary>
        public string Kid { get; set; }

        /// <summary>
        /// Gets or sets the cryptographic algorithm family used with the key.
        /// </summary>
        public KeyType Kty { get; set; }

        /// <summary>
        /// Gets or sets the algorithm intended for use with the key
        /// </summary>
        public AllAlg Alg { get; set; }

        /// <summary>
        /// Gets or sets the X5U. It's a URI that refers to a resource for an X509 public key certificate or certificate chain.
        /// </summary>
        public string X5u { get; set; }

        /// <summary>
        /// Gets or sets the X5T. Is a base64url encoded SHA-1 thumbprint of the DER encoding of an X509 certificate.
        /// </summary>
        public string X5t { get; set; }

        /// <summary>
        /// Gets or sets the X5T#S256. Is a base64url encoded SHA-256 thumbprint.
        /// </summary>
        public string X5tS256 { get; set; }

        /// <summary>
        /// Gets or sets the serialized key in XML
        /// </summary>
        public string SerializedKey { get; set; }
    }
}
