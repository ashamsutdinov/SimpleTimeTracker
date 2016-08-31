using System.Runtime.Serialization;

namespace TimeTracker.ServiceBase.Api
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