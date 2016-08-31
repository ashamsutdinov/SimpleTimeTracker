using AutoMapper;
using TimeTracker.Contract.Data.Entities;

using DalUser = TimeTracker.Dal.Entities.User;
using DtoUser = TimeTracker.Service.Contract.Data.Entities.User;
using DalUserRole = TimeTracker.Dal.Entities.UserRole;
using DtoUserRole = TimeTracker.Service.Contract.Data.Entities.UserRole;
using DalUserSession = TimeTracker.Dal.Entities.UserSession;
using DtoUserSession = TimeTracker.Service.Contract.Data.Entities.UserSession;
using DalUserSetting = TimeTracker.Dal.Entities.UserSetting;
using DtoUserSetting = TimeTracker.Service.Contract.Data.Entities.UserSetting;
using DalUserState = TimeTracker.Dal.Entities.UserState;
using DtoUserState = TimeTracker.Service.Contract.Data.Entities.UserState;
using DalUserToSetting = TimeTracker.Dal.Entities.UserToSetting;
using DtoUserToSetting = TimeTracker.Service.Contract.Data.Entities.UserToSetting;

using DalDayRecord = TimeTracker.Dal.Entities.DayRecord;
using DtoDayRecord = TimeTracker.Service.Contract.Data.Entities.DayRecord;
using DalTimeRecord = TimeTracker.Dal.Entities.TimeRecord;
using DtoTimeRecord = TimeTracker.Service.Contract.Data.Entities.TimeRecord;
using DalTimeRecordItem = TimeTracker.Dal.Entities.TimeRecordItem;
using DtoTimeRecordItem = TimeTracker.Service.Contract.Data.Entities.TimeRecordItem;
using DalTimeRecordNote = TimeTracker.Dal.Entities.TimeRecordNote;
using DtoTimeRecordNote = TimeTracker.Service.Contract.Data.Entities.TimeRecordNote;
using DalTimeRecordNoteItem = TimeTracker.Dal.Entities.TimeRecordNoteItem;
using DtoTimeRecordNoteItem = TimeTracker.Service.Contract.Data.Entities.TimeRecordNoteItem;


namespace TimeTracker.Data.Mapping
{
    internal class MappingProfile :
        Profile
    {
        public MappingProfile()
        {
            
            CreateMap<DalUser, IUser>().As<DtoUser>();
            CreateMap<IUser, DalUser>();
            CreateMap<DalUserRole, IUserRole>().As<DtoUserRole>();
            CreateMap<IUserRole, DalUserRole>();
            CreateMap<DalUserSession, IUserSession>().As<DtoUserSession>();
            CreateMap<IUserSession, DalUserSession>();
            CreateMap<DalUserSetting, IUserSetting>().As<DtoUserSetting>();
            CreateMap<IUserSetting, DalUserSetting>();
            CreateMap<DalUserState, IUserState>().As<DtoUserState>();
            CreateMap<IUserState, DalUserState>();
            CreateMap<DalUserToSetting, IUserToSetting>().As<DtoUserToSetting>();
            CreateMap<IUserToSetting, DalUserToSetting>();

            CreateMap<DalDayRecord, IDayRecord>().As<DtoDayRecord>();
            CreateMap<IDayRecord, DalDayRecord>();
            CreateMap<DalTimeRecord, ITimeRecord>().As<DtoTimeRecord>();
            CreateMap<ITimeRecord, DalTimeRecord>();
            CreateMap<DalTimeRecordItem, ITimeRecordItem>().As<DtoTimeRecordItem>();
            CreateMap<ITimeRecordItem, DalTimeRecordItem>();
            CreateMap<DalTimeRecordNote, ITimeRecordNote>().As<DtoTimeRecordNote>();
            CreateMap<ITimeRecordNote, DalTimeRecordNote>();
            CreateMap<DalTimeRecordNoteItem, ITimeRecordNoteItem>().As<DtoTimeRecordNoteItem>();
            CreateMap<ITimeRecordNoteItem, DalTimeRecordNoteItem>();
        }
    }
}
