using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleIdentityServer.Eid.Sign
{
    public class XmlDsigDocument : XmlDocument
    {
        public const string XmlDsigNamespacePrefix = "ds";

        public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                List<string> SignedInfoAndDescendants = new List<string>();
                SignedInfoAndDescendants.Add("SignedInfo");
                SignedInfoAndDescendants.Add("CanonicalizationMethod");
                SignedInfoAndDescendants.Add("InclusiveNamespaces");
                SignedInfoAndDescendants.Add("SignatureMethod");
                SignedInfoAndDescendants.Add("Reference");
                SignedInfoAndDescendants.Add("Transforms");
                SignedInfoAndDescendants.Add("Transform");
                SignedInfoAndDescendants.Add("InclusiveNamespaces");
                SignedInfoAndDescendants.Add("DigestMethod");
                SignedInfoAndDescendants.Add("DigestValue");
                if (!SignedInfoAndDescendants.Contains(localName))
                {
                    prefix = GetPrefix(namespaceURI);
                }
            }

            return base.CreateElement(prefix, localName, namespaceURI);
        }

        public static XmlNode SetPrefix(string prefix, XmlNode node)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                SetPrefix(prefix, n);
            }
            if (node.NamespaceURI == "http://www.w3.org/2001/10/xml-exc-c14n#")
                node.Prefix = "ec";
            else if ((node.NamespaceURI == SignedXmlWithId.XmlDsigNamespaceUrl) || (string.IsNullOrEmpty(node.Prefix)))
                node.Prefix = prefix;

            return node;
        }

        public static string GetPrefix(string namespaceURI)
        {
            if (namespaceURI == "http://www.w3.org/2001/10/xml-exc-c14n#")
                return "ec";
            else if (namespaceURI == SignedXmlWithId.XmlDsigNamespaceUrl)
                return "ds";

            return string.Empty;
        }
    }
}
