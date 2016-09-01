using System.Security.Authentication;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class PasswordSyntaxOnRegistrationPolicy :
        AnonymousRequestSecurityPolicy<RegistrationData>
    {
        private readonly CryptographyHelper _cryptographyHelper;

        public PasswordSyntaxOnRegistrationPolicy(CryptographyHelper cryptographyHelper)
        {
            _cryptographyHelper = cryptographyHelper;
        }

        protected override void EvaluateAnonymous(Request<RegistrationData> request)
        {
            var rawPassword = request.Data.Password;
            var passwordData = _cryptographyHelper.DecodeXorPassword(rawPassword);
            var validPasswordSyntax = _cryptographyHelper.VerifyPasswordSyntax(passwordData);
            if (!validPasswordSyntax)
            {
                throw new AuthenticationException("Invalid regisgration request");
            }
        }
    }
}