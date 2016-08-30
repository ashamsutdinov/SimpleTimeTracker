using System;
using System.Data;

namespace TimeTracker.Dal.Entities.Base
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }

        protected Entity()
        {
            
        }

        protected Entity(IDataRecord reader)
        {
            Id = Read<TKey>(reader, "Id");
        }

        protected TValue Read<TValue>(IDataRecord reader, string key)
        {
            var value = reader[key];
            if (value == DBNull.Value)
            {
                return default(TValue);
            }
            return (TValue) value;
        }
    }
}