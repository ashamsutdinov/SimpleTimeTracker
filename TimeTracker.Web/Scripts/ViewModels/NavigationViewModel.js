window.NavigationViewModel = function () {

    var self = this;

    self.loggedIn = ko.observable(false);
    self.isUser = ko.observable(false);
    self.isManager = ko.observable(false);
    self.isAdministrator = ko.observable(false);

    self.requestUserTimesheets = function() {
        window.messageBus.fire(Event.DataRequested, DataRequest.UserTimesheets);
    };

    self.requestManageUsers = function() {
        window.messageBus.fire(Event.DataRequested, DataRequest.ManageUsers);
    };

    self.requestManageTimesheets = function() {
        window.messageBus.fire(Event.DataRequested, DataRequest.ManageTimeshets);
    };

    self.requestUserSettings = function() {
        window.messageBus.fire(Event.DataRequested, DataRequest.UserSettings);
    };

    self.requestLogout = function() {
        window.messageBus.fire(Event.LogoutRequested);
    };

    var flush = function() {
        self.loggedIn(false);
        self.isUser(false);
        self.isManager(false);
        self.isAdministrator(false);
    };

    window.messageBus.subscribe(Event.LoggedIn, function () {
        self.loggedIn(true);
    });

    window.messageBus.subscribe(Event.LoggedOut, function () {
        flush();
    });

    window.messageBus.subscribe(Event.SessionStateChanged, function (states) {
        for (var i = 0; i < states.length; i++) {
            switch (states[i]) {
                case SessionState.LoggedInUser:
                    self.isUser(true);
                    break;
                case SessionState.LoggedInManager:
                    self.isManager(true);
                    break;
                case SessionState.LoggedInAdministrator:
                    self.isAdministrator(true);
                    break;
            }
        }
    });

    return self;
};