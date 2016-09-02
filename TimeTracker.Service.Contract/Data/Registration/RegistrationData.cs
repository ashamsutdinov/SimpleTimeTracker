using System.Runtime.Serialization;
using TimeTracker.Service.Contract.Data.Authentication;

namespace TimeTracker.Service.Contract.Data.Registration
{
    [DataContract]
    public class RegistrationData : 
        LoginData
    {
        [DataMember]
        public string Name { get; set; }
    }
}