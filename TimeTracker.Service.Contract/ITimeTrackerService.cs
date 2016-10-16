using System.ServiceModel;
using System.ServiceModel.Web;
using TimeTracker.Service.Contract.Data.Authentication;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.Registration;
using TimeTracker.Service.Contract.Data.Session;
using TimeTracker.Service.Contract.Data.TimeRecords;
using TimeTracker.Service.Contract.Data.UserManagement;

namespace TimeTracker.Service.Contract
{
    [ServiceContract(Namespace = "http://simple-time-tracking-service/")]
    public interface ITimeTrackerService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/session/heartbit/{Tick}")]
        Response<HeartbitData> Heartbit(long tick);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/session/handshake")]
        Response<SessionState[]> Handshake();
       
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/login")]
        Response<TicketData> Login(Request<LoginData> request);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/logout")]
        Response<string> Logout(Request request);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/register")]
        Response<int> Register(Request<RegistrationData> request);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/{Id}")]
        Response<int> SaveTimeRecord(Request<TimeRecordData> request);

        [OperationContract]
        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/{Id}")]
        Response<int> DeleteTimeRecord(Request<TimeRecordData> request);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/note/{Id}")]
        Response<int> SaveTimeRecordNote(Request<TimeRecordNoteData> request);

        [OperationContract]
        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/note/{Id}")]
        Response<int> DeleteTimeRecordNote(Request<TimeRecordNoteData> request);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/list/{PageNumber}/{PageSize}/{LoadAll}")]
        Response<TimeRecordItemList> GetTimeRecords(Request<TimeRecordsFilterData> request);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "user/settings/{Id}")]
        Response<UserSettingItemList> GetUserSettings(Request<UserSettingItemList> request);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "user/settings/{Id}")]
        Response<int> SaveUserSettings(Request<UserSettingItemList> request);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/list/{PageNumber}/{PageSize}")]
        Response<UserList> GetUsers(Request<UserListData> request);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/{Id}")]
        Response<UserListItem> GetUser(Request<UserListItem> request);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/{Id}")]
        Response<int> SaveUser(Request<UserListItem> request);

        [OperationContract]
        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/{Id}")]
        Response<int> DeleteUser(Request<UserListItem> request);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/reset-password/{Id}")]
        Response<int> ResetPassword(Request<UserListItem> request);
    }
}
