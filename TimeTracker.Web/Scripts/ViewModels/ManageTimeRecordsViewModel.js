window.ManageTimeRecordsViewModel = function (options) {

    var self = new TimeRecordsViewModel(DataRequest.ManageTimeshets, options);

    window.messageBus.subscribe(Event.SessionStateChanged, function (s) {
        self.filterLoadAllUsers(false);
        for (var i = 0; i < s.length; i++) {
            if (s[i] === SessionState.LoggedInAdministrator) {
                self.filterLoadAllUsers(true);
            }
        }
    });

    window.messageBus.subscribe(Event.LoggedOut, function() {
        self.filterLoadAllUsers(true);
    });

    return self;
};