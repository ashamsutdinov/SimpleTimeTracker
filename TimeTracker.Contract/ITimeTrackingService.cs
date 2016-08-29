using System.ServiceModel;
using System.ServiceModel.Web;
using TimeTracker.Contract.Attributes;
using TimeTracker.Contract.Requests.Base;
using TimeTracker.Contract.Responses.Base;

namespace TimeTracker.Contract
{
    [ServiceContract]
    public interface ITimeTrackingService
    {
        [Anonimous]
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Response<string> GetNonce(Request request);
    }
}
