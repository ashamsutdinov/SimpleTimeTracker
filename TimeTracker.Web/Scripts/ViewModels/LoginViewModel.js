window.LoginViewModel = function () {

    var self = this;

    self.isAnonymous = ko.observable(false);

    self.loginLogin = ko.observable('');
    self.loginPassword = ko.observable('');

    self.registerLogin = ko.observable('');
    self.registerName = ko.observable('');
    self.registerPassword = ko.observable('');

    self.error = ko.observable(null);
    self.registered = ko.observable(false);

    var errorHandler = function (r) {
        if (r === null || r === undefined) {
            self.error("Unexpected server error");
        } else {
            self.error(r.Message);
            setTimeout(function () {
                self.error(null);
            }, Config.ShowAlertTimeout);
        }
    };

    self.login = function () {
        self.error(null);

        var valid =
            validate(self.loginLogin, self.error, "Login required") &&
            validate(self.loginPassword, self.error, "Password required");
        
        if (!valid) {
            return;
        }
        
        var data = {
            Login: self.loginLogin(),
            Password: self.loginPassword()
        };
        window.application.apiPost("Login", data, {
            success: function (r) {
                localStorage[Config.TicketKey] = r.Ticket;
                window.messageBus.fire(Event.EndDialog);
                window.messageBus.fire(Event.LoggedIn);
                window.application.handshake();
            },
            error: errorHandler,
            done: function () {
                self.loginLogin(null);
                self.loginPassword(null);
            },
            fail: errorHandler
        });
    };

    self.logout = function () {
        window.application.apiPost("Logout", {}, {
            done: function () {
                localStorage.removeItem(Config.TicketKey);
                window.messageBus.fire(Event.LoggedOut);
                window.application.handshake();
            }
        });
    };

    self.register = function () {
        var valid =
            validate(self.registerLogin, self.error, "Login required") &&
            validate(self.registerName, "Name required") &&
            validate(self.registerPassword, self.error, "Password required");

        if (!valid) {
            return;
        }

        var data = {
            Login: self.registerLogin(),
            Name: self.registerName(),
            Password: self.registerPassword()
        };
        window.application.apoPost("Register", data, {
            success: function (r) {
                self.registered(true);
                setTimeout(function () {
                    self.registered(false);
                }, Config.ShowAlertTimeout);
                window.messageBus.fire(Event.UserRegistered);
            },
            error: errorHandler,
            done: function () {
                self.registerLogin(null);
                self.registerName(null);
                self.registerPassword(null);
            },
            fail: errorHandler
        });
    };

    window.messageBus.subscribe(Event.SessionStateChanged, function (states) {
        self.isAnonymous(false);
        for (var i = 0; i < states.length; i++) {
            if (states[i] === SessionState.Anonymous) {
                self.isAnonymous(true);
            }
        }
    });

    window.messageBus.subscribe(Event.LogoutRequested, function () {
        self.logout();
    });

    return self;
};