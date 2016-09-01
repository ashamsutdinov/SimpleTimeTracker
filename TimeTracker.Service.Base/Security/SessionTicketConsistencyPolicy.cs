using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class SessionTicketConsistencyPolicy :
        SecurityPolicy
    {
        private readonly string _requestTicket ;

        public SessionTicketConsistencyPolicy(string requestTicket)
        {
            _requestTicket = requestTicket;
            
        }
      
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            var sessionTicket = userSession != null ? userSession.Ticket : null;
            if (sessionTicket != _requestTicket)
            {
                throw new InvalidOperationException("Invalid session ticket");
            }
        }
    }
}