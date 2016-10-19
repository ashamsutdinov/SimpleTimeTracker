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
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/Session/Heartbit/{Tick}")]
        Response<HeartbitData> Heartbit(string tick);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/Session/Handshake")]
        Response<SessionState[]> Handshake();
       
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/login")]
        Response<TicketData> Login(LoginData data);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/logout")]
        Response<string> Logout();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user")]
        Response<int> Register(RegistrationData data);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord")]
        Response<int> CreateTimeRecord(TimeRecordData data);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/{Id}")]
        Response<int> SaveTimeRecord(string id, TimeRecordData data);

        [OperationContract]
        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/{Id}")]
        Response<int> DeleteTimeRecord(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/note")]
        Response<int> CreateTimeRecordNote(TimeRecordNoteData data);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/note/{Id}")]
        Response<int> SaveTimeRecordNote(string id, TimeRecordNoteData data);

        [OperationContract]
        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/note/{Id}")]
        Response<int> DeleteTimeRecordNote(string id);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/timerecord/list/{PageNumber}/{PageSize}/{LoadAllUsers}/{From}/{To}")]
        Response<TimeRecordItemList> GetTimeRecords(string pageNumber, string pageSize, string loadAllUsers, string from, string to);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "user/settings/{Id}")]
        Response<UserSettingItemList> GetUserSettings(string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "user/settings/{Id}")]
        Response<int> SaveUserSettings(string id, UserSettingItemList data);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/list/{PageNumber}/{PageSize}")]
        Response<UserList> GetUsers(string pageNumber, string pageSize);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/{Id}")]
        Response<UserListItem> GetUser(string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/{Id}")]
        Response<int> SaveUser(string id, UserListItem data);

        [OperationContract]
        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/{Id}")]
        Response<int> DeleteUser(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/user/resetpassword/{Id}")]
        Response<int> ResetPassword(string id);
    }
}
