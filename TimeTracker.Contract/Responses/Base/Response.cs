using System.Net;
using System.Runtime.Serialization;

namespace TimeTracker.Contract.Responses.Base
{
    [DataContract]
    public class Response<TData>
    {
        [DataMember]
        public TData Data { get; set; }

        [DataMember]
        public HttpStatusCode StatusCode { get; set; }

        [DataMember]
        public string Message { get; set; }

        public static Response<TData> Success(TData data)
        {
            return new Response<TData>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static Response<TData> Unauthorized(string message)
        {
            return new Response<TData>
            {
                Message = message,
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        public static Response<TData> Forbidden(string message)
        {
            return new Response<TData>
            {
                Message = message,
                StatusCode = HttpStatusCode.Forbidden
            };
        }

        public static Response<TData> Fail(string message)
        {
            return new Response<TData>
            {
#if DEBUG
                Message = message,
#endif
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
}