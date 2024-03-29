﻿using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.Registration;

namespace TimeTracker.Service.Base.Validation.Registration
{
    internal class LoginCanBeUserValidationRule :
        AnonymousRequestValidationRule<RegistrationData>
    {
        private readonly IUserDataProvider _userDataProvider;

        public LoginCanBeUserValidationRule(IUserDataProvider userDataProvider) : 
            base(true)
        {
            _userDataProvider = userDataProvider;
        }

        protected override void EvaluateAnonymous(Request<RegistrationData> request)
        {
            var login = request.Data.Login;
            var user = _userDataProvider.GetUser(login);
            if (user != null)
            {
                throw new AuthenticationException("Login cannot be used");
            }
        }
    }
}