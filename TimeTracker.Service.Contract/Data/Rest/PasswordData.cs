using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Rest
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