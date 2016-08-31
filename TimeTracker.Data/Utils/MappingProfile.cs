using AutoMapper;
using TimeTracker.Contract.Data.Entities;
using DalDayRecord = TimeTracker.Dal.Entities.DayRecord;
using DtoDayRecord = TimeTracker.ServiceBase.Entities.DayRecord;
using DalTimeRecord = TimeTracker.Dal.Entities.TimeRecord;
using DtoTimeRecord = TimeTracker.ServiceBase.Entities.TimeRecord;
using DalTimeRecordItem = TimeTracker.Dal.Entities.TimeRecordItem;
using DtoTimeRecordItem = TimeTracker.ServiceBase.Entities.TimeRecordItem;
using DalTimeRecordNote = TimeTracker.Dal.Entities.TimeRecordNote;
using DtoTimeRecordNote = TimeTracker.ServiceBase.Entities.TimeRecordNote;
using DalTimeRecordNoteItem = TimeTracker.Dal.Entities.TimeRecordNoteItem;
using DtoTimeRecordNoteItem = TimeTracker.ServiceBase.Entities.TimeRecordNoteItem;
using DalUser = TimeTracker.Dal.Entities.User;
using DtoUser = TimeTracker.ServiceBase.Entities.User;
using DalUserRole = TimeTracker.Dal.Entities.UserRole;
using DtoUserRole = TimeTracker.ServiceBase.Entities.UserRole;
using DalUserSession = TimeTracker.Dal.Entities.UserSession;
using DtoUserSession = TimeTracker.ServiceBase.Entities.UserSession;
using DalUserSetting = TimeTracker.Dal.Entities.UserSetting;
using DtoUserSetting = TimeTracker.ServiceBase.Entities.UserSetting;
using DalUserState = TimeTracker.Dal.Entities.UserState;
using DtoUserState = TimeTracker.ServiceBase.Entities.UserState;
using DalUserToSetting = TimeTracker.Dal.Entities.UserToSetting;
using DtoUserToSetting = TimeTracker.ServiceBase.Entities.UserToSetting;


namespace TimeTracker.Data.Utils
{
    public class MappingProfile :
        Profile
    {
        public MappingProfile()
        {
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
        }
    }
}
