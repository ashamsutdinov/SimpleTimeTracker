using System.ServiceModel;
using System.ServiceModel.Web;
using TimeTracker.Contract.Requests;
using TimeTracker.Contract.Requests.Base;
using TimeTracker.Contract.Responses;
using TimeTracker.Contract.Responses.Base;

namespace TimeTracker.Contract
{
    [ServiceContract(Namespace = "http://simple-time-tracking-service/")]
    public interface ITimeTrackingService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response<string> GetNonce(Request request);
       
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response<TicketData> Login(Request<LoginData> request);
    }
}
