using System;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.UserManagement;

namespace TimeTracker.Service.Base.Validation.UserManagement
{
    internal class UserSettingsOwnerExistsValidationRule :
        RequestValidationRule<UserSettingItemList>
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserSettingsOwnerExistsValidationRule(IUserDataProvider userDataProvider) : 
            base(true)
        {
            _userDataProvider = userDataProvider;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<UserSettingItemList> request)
        {
            if (request.Data.UserId > 0)
            {
                var settingsUser = _userDataProvider.GetUser(request.Data.UserId);
                if (settingsUser == null)
                {
                    throw new InvalidOperationException("User was not found");
                }
            }
        }
    }
}