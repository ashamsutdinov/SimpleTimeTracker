using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Authentication
{
    [DataContract]
    public class LoginData
    {
        [DataMember]
        public string Login { get; set; }

        //base64(xor(password+clientId+nonce), apikey)
        [DataMember]
        public string Password { get; set; }
    }
}