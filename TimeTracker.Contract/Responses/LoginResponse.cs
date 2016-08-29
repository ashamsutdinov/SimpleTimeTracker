using System.Runtime.Serialization;
using TimeTracker.Contract.Responses.Base;

namespace TimeTracker.Contract.Responses
{
    [DataContract]
    public class LoginResponse :
        Response<TicketData>
    {
        
    }
}