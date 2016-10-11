using System.Net;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Base
{
    [DataContract]
    public class Response<TData>
    {
        public HttpStatusCode StatusCode { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public TData Data { get; set; }

        public static Response<TData> Success(TData data)
        {
            return PrepareResponse(null, data, HttpStatusCode.OK);
        }

        public static Response<TData> Unauthorized(string message)
        {
            return PrepareResponse(message, default(TData), HttpStatusCode.Unauthorized);
        }

        public static Response<TData> Forbidden(string message)
        {
            return PrepareResponse(message, default(TData), HttpStatusCode.Forbidden);
        }

        public static Response<TData> Fail(string message)
        {
            return PrepareResponse(
#if DEBUG
                message,
#else
                null,
#endif
                default(TData),
                HttpStatusCode.InternalServerError
                );
        }

        public static Response<TData> NotAcceptable(string message)
        {
            return PrepareResponse(message, default(TData), HttpStatusCode.BadRequest);
        }

        private static Response<TData> PrepareResponse(string message, TData data, HttpStatusCode statusCode)
        {
            var response = new Response<TData>
            {
                Message = message,
                Data = data,
                StatusCode = statusCode
            };

            return response;
        }
    }
}