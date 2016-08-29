using System.Runtime.Serialization;

namespace TimeTracker.Contract.Requests
{
    [DataContract]
    public class DecryptedPasswordData
    {
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string Nonce { get; set; }
    }
}