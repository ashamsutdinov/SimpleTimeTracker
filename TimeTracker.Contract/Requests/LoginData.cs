using System.Runtime.Serialization;

namespace TimeTracker.Contract.Requests
{
    [DataContract]
    public class LoginData
    {
        [DataMember]
        public string Login { get; set; }

        //rsa(password+clientid+nonce)
        [DataMember]
        public string Password { get; set; }
    }
}