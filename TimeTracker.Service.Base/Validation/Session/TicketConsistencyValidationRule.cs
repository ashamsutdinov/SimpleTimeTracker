using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.Session
{
    internal class TicketConsistencyValidationRule :
        ValidationRule
    {
        private readonly string _requestTicket ;

        public TicketConsistencyValidationRule(string requestTicket)
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