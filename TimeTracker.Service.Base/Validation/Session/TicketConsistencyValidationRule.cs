using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Session
{
    internal class TicketConsistencyValidationRule :
        ValidationRule
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            var sessionTicket = userSession != null ? userSession.Ticket : null;
            if (sessionTicket != request.Ticket)
            {
                throw new InvalidOperationException("Invalid session ticket");
            }
        }
    }
}