using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SimpleIdentityServer.Rms.Client.DTOs.Responses
{
    public class GuidHashResult
    {
        [XmlElement("Guid")]
        public string Guid { get; set; }
        [XmlElement("Hash")]
        public string Hash { get; set; }
    }

    [Serializable]
    [XmlRoot("AcquireTemplateInformationResult", Namespace = "http://microsoft.com/DRM/TemplateDistributionService")]
    public class AcquireTemplatesResponse
    {
        [XmlElement("ServerPublicKey")]
        public string ServerPublicKey { get; set; }
        [XmlElement("GuidHashCount")]
        public string GuidHashCount { get; set; }
        [XmlElement("GuidHash")]
        public List<GuidHashResult> Guids { get; set; }
    }
}
