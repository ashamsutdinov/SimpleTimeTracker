using AutoMapper;
using TimeTracker.Data.Utils;

namespace TimeTracker.Data.Base
{
    public abstract class DataProviderBase
    {
        protected DataProviderBase()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }
    }
}