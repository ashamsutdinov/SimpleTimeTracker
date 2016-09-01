using System;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class RequestTicketSyntaxPolicy :
        AnonymousRequestSecurityPolicy
    {
        private readonly CryptographyHelper _cryptographyHelper;

        public RequestTicketSyntaxPolicy(CryptographyHelper helper)
        {
            _cryptographyHelper = helper;
        }

        protected override void EvaluateAnonymous(Request request)
        {
            var sessionData = _cryptographyHelper.DecodeXorTicket(request.Ticket);
            if (!_cryptographyHelper.VerifyTicketSyntax(sessionData))
            {
                throw new InvalidOperationException("Invalid ticket");
            }
        }
    }
}