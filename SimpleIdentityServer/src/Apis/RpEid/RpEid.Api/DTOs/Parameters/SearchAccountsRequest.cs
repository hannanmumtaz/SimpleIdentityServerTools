using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RpEid.Api.DTOs.Parameters
{
    [DataContract]
    public class SearchAccountsRequest
    {
        [DataMember(Name = Constants.SearchNames.StartIndex)]
        public int StartIndex { get; set; }
        [DataMember(Name = Constants.SearchNames.Count)]
        public int Count { get; set; }
        [DataMember(Name = Constants.SearchNames.Order)]
        public int Order { get; set; }
        [DataMember(Name = Constants.SearchAccountsNames.Subjects)]
        public IEnumerable<string> Subjects { get; set; }
        [DataMember(Name = Constants.SearchAccountsNames.IsConfirmed)]
        public bool IsConfirmed { get; set; }
    }
}
