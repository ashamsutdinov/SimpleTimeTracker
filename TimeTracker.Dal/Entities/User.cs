using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class User :
        Entity<int>
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public UserState State { get; set; }

        public IList<UserRole> Roles { get; set; }

        public IList<UserToSetting> Settings { get; set; }

        public User()
        {

        }

        public User(IDataRecord reader, bool singleUser = false) :
            base(reader)
        {
            Login = Read<string>(reader, "Login");
            PasswordHash = Read<string>(reader, "PasswordHash");
            PasswordSalt = Read<string>(reader, "PasswordSalt");
            Name = Read<string>(reader, "Name");
            StateId = Read<string>(reader, "StateId");

            State = new UserState
            {
                Id = StateId,
                Description = Read<string>(reader, "StateDescription")
            };

            if (singleUser)
            {
                Roles = new List<UserRole>();
                Settings = new List<UserToSetting>();

                var sqlReader = reader as SqlDataReader;
                if (sqlReader != null && sqlReader.NextResult())
                {
                    while (sqlReader.Read())
                    {
                        Roles.Add(new UserRole(sqlReader));
                    }
                }
                if (sqlReader != null && sqlReader.NextResult())
                {
                    while (sqlReader.Read())
                    {
                        Settings.Add(new UserToSetting(sqlReader));
                    }
                }
            }
        }
    }
}