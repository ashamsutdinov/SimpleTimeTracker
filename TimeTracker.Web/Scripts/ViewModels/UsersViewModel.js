window.UsersViewModel = function (settings) {

    var self = new DataContainerViewModelBase(DataRequest.ManageUsers);

    self.users = ko.observableArray([]);
    self.pages = ko.observableArray([]);
    self.states = ko.observableArray([]);
    self.roles = ko.observableArray([]);
    self.settings = ko.observableArray([]);
    self.pageSizeOptions = ko.observableArray([5, 10, 15, 25, 50]);

    self.pageSize = ko.observable(10);
    self.pageNumber = ko.observable(1);

    self.editUserError = ko.observable(null);
    self.editUserId = ko.observable(0);
    self.editUserLogin = ko.observable(null);
    self.editUserName = ko.observable(null);
    self.editUserStateId = ko.observable(null);
    self.editUserRoles = ko.observableArray([]);
    self.editUserSettings = ko.observableArray([]);

    self.changePage = function (page) {
        if (page !== self.pageNumber()) {
            self.pageNumber(page);
            self.loadUsers();
        }
    };

    self.pageSizeChanged = function (a) {
        self.pageNumber(1);
        self.loadUsers();
    };

    self.loadUsers = function () {
        var request = {
            PageSize: self.pageSize(),
            PageNumber: self.pageNumber()
        };
        window.application.apoPost("GetUsers", request, {
            success: function (r) {
                self.pages([]);
                if (r.AllRoles !== null) {
                    self.roles(r.AllRoles);
                }
                if (r.AllStates !== null) {
                    self.states(r.AllStates);
                }
                if (r.AllSettings !== null) {
                    self.settings(r.AllSettings);
                }
                if (r.Items !== null) {
                    self.users(r.Items);
                    for (var i = 0; i < r.TotalPages; i++) {
                        self.pages.push(i + 1);
                    }
                } else {
                    self.users([]);
                }
            }
        });
    };

    self.editUser = function (e) {
        self.editUserError(null);
        var request = {
            Id: e.Id
        };
        window.application.apoPost("GetUser", request, {
            success: function (user) {
                self.editUserId(user.Id);
                self.editUserLogin(user.Login);
                self.editUserName(user.Name);
                self.editUserStateId(user.StateId);
                var roles = [];
                for (var i = 0; i < user.Roles.length;i++) {
                    var allRoles = self.roles();
                    var role = user.Roles[i];
                    for (var j = 0; j < allRoles.length; j++) {
                        var allRole = allRoles[j];
                        if (allRole.Id === role.Id) {
                            roles.push(allRole.Id);
                        }
                    }
                }
                self.editUserRoles(roles);
                self.editUserSettings(user.Settings);
                window.messageBus.fire(Event.BeginDialog, settings.userWindow);
            }
        });
    };

    self.deleteUser = function (e) {
        confirmDelete(function () {
            var request = {
                Id: e.Id
            };
            window.application.apoPost("DeleteUser", request, {
                success: function () {
                    window.messageBus.fire(Event.UserDeleted);
                }
            });
        });
    };

    self.saveUser = function () {
        var roles = [];
        var savingRoles = self.editUserRoles();
        for (var i = 0; i < savingRoles.length; i++) {
            roles.push({
                Id: savingRoles[i]
            });
        }
        var request = {
            Id: self.editUserId(),
            Login: self.editUserLogin(),
            Name: self.editUserName(),
            StateId: self.editUserStateId(),
            Roles: roles,
            Settings: self.editUserSettings()
        };
        window.application.apoPost("SaveUser", request, {
            success: function() {
                window.messageBus.fire(Event.UserSaved);
                window.messageBus.fire(Event.EndDialog);
            }
        });
    };

    self.resetPassword = function (e) {
        confirmDialog("Change password to default value?", function () {
            var request = {
                Id: e.Id
            };
            window.application.apoPost("ResetPassword", request, {
                success: function () {

                }
            });
        });
    };

    self.load = function () {
        self.loadUsers();
    };

    self.clear = function () {
        self.users([]);
        self.pages([]);
        self.pageSize(10);
        self.pageNumber(1);

        self.editUserError(null);
        self.editUserId(0);
        self.editUserLogin(null);
        self.editUserName(null);
        self.editUserStateId(null);
        self.editUserRoles([]);
        self.editUserSettings([]);
    };

    var eventsTriggerReload = [Event.UserDeleted, Event.UserSaved];
    window.messageBus.subscribeMany(eventsTriggerReload, function () {
        self.loadUsers();
    });

    return self;
};