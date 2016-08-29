using System.Runtime.Serialization;
using TimeTracker.Contract.Requests.Base;

namespace TimeTracker.Contract.Requests
{
    [DataContract]
    public class LoginRequest  :
        Request<LoginData>
    {
        
    }
}