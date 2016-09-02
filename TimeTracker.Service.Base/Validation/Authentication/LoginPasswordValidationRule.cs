using System.Security.Authentication;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;
using TimeTracker.Service.Contract.Data.Authentication;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Authentication
{
    internal class LoginPasswordValidationRule :
        AnonymousRequestValidationRule<LoginData>
    {
        private readonly CryptographyHelper _cryptographyHelper;

        public LoginPasswordValidationRule(CryptographyHelper cryptographyHelper)
        {
            _cryptographyHelper = cryptographyHelper;
        }

        protected override void EvaluateAnonymous(Request<LoginData> request)
        {
            var rawPassword = request.Data.Password;
            var passwordData = _cryptographyHelper.DecodeXorPassword(rawPassword);
            var validPasswordSyntax = _cryptographyHelper.VerifyPasswordSyntax(passwordData);
            if (!validPasswordSyntax)
            {
                throw new AuthenticationException("Invalid login request");
            }
        }
    }
}