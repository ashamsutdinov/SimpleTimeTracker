using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace TimeTracker.RestService.Utils
{
    public class CustomHeaderMessageInspector :
        IDispatchMessageInspector
    {
        readonly Dictionary<string, string> _requiredHeaders;

        public CustomHeaderMessageInspector(Dictionary<string, string> headers)
        {
            _requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            return new
            {
                origin = httpRequest.Headers["Origin"],
                handlePreflight = httpRequest.Method.Equals("OPTIONS", StringComparison.InvariantCultureIgnoreCase)
            };
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var state = (dynamic)correlationState;

            if (state.handlePreflight)
            {
                reply = Message.CreateMessage(MessageVersion.None, "PreflightReturn");
                var httpResponse = new HttpResponseMessageProperty();
                reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);
                httpResponse.SuppressEntityBody = true;
                httpResponse.StatusCode = HttpStatusCode.OK;
            }

            var httpHeader = (HttpResponseMessageProperty)reply.Properties["httpResponse"];
            foreach (var item in _requiredHeaders)
            {
                if (httpHeader != null)
                {
                    httpHeader.Headers.Add(item.Key, item.Value);
                }
            }
        }
    }
}