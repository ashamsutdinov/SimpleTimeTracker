using System;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Session
{
    internal class RequestTicketSyntaxValidationRule :
        AnonymousRequestValidationRule
    {
        private readonly CryptographyHelper _cryptographyHelper;

        public RequestTicketSyntaxValidationRule(CryptographyHelper helper)
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