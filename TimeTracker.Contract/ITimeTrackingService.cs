using System.ServiceModel;
using System.ServiceModel.Web;
using TimeTracker.Contract.Attributes;

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
