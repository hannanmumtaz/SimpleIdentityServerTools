﻿using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class AddOfficeDocumentRequest
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}