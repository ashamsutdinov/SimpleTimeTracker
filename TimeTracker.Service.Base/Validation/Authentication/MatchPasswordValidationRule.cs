using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Authentication;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Authentication
{
    internal class MatchPasswordValidationRule :
        AnonymousRequestValidationRule<LoginData>
    {
        private readonly CryptographyHelper _cryptographyHelper;

        private readonly IUserDataProvider _userDataProvider;

        public MatchPasswordValidationRule(CryptographyHelper cryptographyHelper, IUserDataProvider userDataProvider) :
            base(true)
        {
            _cryptographyHelper = cryptographyHelper;
            _userDataProvider = userDataProvider;
        }

        protected override void EvaluateAnonymous(Request<LoginData> request)
        {
            var passwordData = _cryptographyHelper.DecodeXorPassword(request.Data.Password);
            var user = _userDataProvider.GetUser(request.Data.Login);
            var hash = _cryptographyHelper.HashPassword(passwordData.Password, user.PasswordSalt);
            if (hash != user.PasswordHash)
            {
                throw new AuthenticationException("Invalid login or password");
            }
        }
    }
}