using System.Runtime.Serialization;

namespace TimeTracker.Contract.Api
{
    [DataContract]
    public class PasswordData
    {
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string Nonce { get; set; }
    }
}