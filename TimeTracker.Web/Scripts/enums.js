var StatusCode = {
    OK: 200
};

var Event = {
    Ready: 0,
    Connected: 1,
    Disconnected: 2,
    SessionStateChanged: 3,

    LoggedIn: 11,
    LoggedOut: 12,
    LogoutRequested: 13,

    TimeRecordSaved: 21,
    TimeRecordDeleted: 22,
    TimeRecordNoteSaved: 23,
    TimeRecordNoteDeleted: 24,

    UserRegistered : 41,
    UserSaved: 42,
    UserDeleted: 43,
    UserSettingUpdated: 44,

    Alert: 51,

    DataRequested: 61,

    BeginDialog: 71,
    EndDialog: 72
};

var SessionState = {
    Undefined: 0,
    Anonymous: 1,
    LoggedInUser: 2,
    LoggedInManager: 3,
    LoggedInAdministrator: 4
};

var AlertType = {
    Success: 0,
    Information: 1,
    Warning: 2,
    Error: 3
};

var DataRequest = {
    UserTimesheets: 0,
    ManageUsers: 1,
    ManageTimeshets: 2,
    UserSettings: 3,
    ClearAll: 10
};