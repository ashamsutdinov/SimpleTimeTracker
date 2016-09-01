using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class PasswordHashMatchPolicy :
        AnonymousRequestSecurityPolicy<LoginData>
    {
        private readonly CryptographyHelper _cryptographyHelper;

        private readonly IUserDataProvider _userDataProvider;

        public PasswordHashMatchPolicy(CryptographyHelper cryptographyHelper, IUserDataProvider userDataProvider)
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