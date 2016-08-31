using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Rest
{
    [DataContract]
    public class RegistrationData
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}