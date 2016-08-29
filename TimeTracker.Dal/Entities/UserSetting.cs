﻿using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserSetting : 
        IdAndDescriptionEntity
    {
        public UserSetting()
        {
            
        }

        public UserSetting(IDataRecord reader) :
            base(reader)
        {
            
        }
    }
}