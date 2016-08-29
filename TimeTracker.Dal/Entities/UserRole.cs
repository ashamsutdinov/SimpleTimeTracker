﻿using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserRole :
        KeyAndDescriptionEntity
    {
        public UserRole()
        {
            
        }

        public UserRole(IDataRecord reader) :
            base(reader)
        {
            
        }
    }
}