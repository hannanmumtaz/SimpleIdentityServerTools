using System.Runtime.Serialization;

namespace RpEid.Api.DTOs.Parameters
{
    [DataContract]
    public class AddAccountParameter
    {
        [DataMember(Name = Constants.AccountNames.Email)]
        public string Email { get; set; }
    }
}
