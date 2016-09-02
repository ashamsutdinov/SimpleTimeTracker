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

    var loginNonceReceived = function (nonce) {
        var login = self.loginLogin();
        if (nullString(login)) {
            self.error("Login required");
            return;
        }
        var password = self.loginPassword();
        if (nullString(password)) {
            self.error("Password required");
            return;
        }
        var passwordData = password + "#" + Config.ClientId + "#" + nonce;
        var encryptedPasswordData = Base64.encode(xor(passwordData, Config.ApiKey));
        var data = {
            Login: login,
            Password: encryptedPasswordData
        };
       
        window.application.apiCall("Login", data, {
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
    }

    self.login = function () {
        self.error(null);
        window.application.apiCall("GetNonce", {}, {
            success: loginNonceReceived,
            error: errorHandler,
            fail: errorHandler
        });
    };

    self.logout = function () {
        window.application.apiCall("Logout", {}, {
            done: function () {
                localStorage.removeItem(Config.TicketKey);
                window.messageBus.fire(Event.LoggedOut);
                window.application.handshake();
            }
        });
    };

    var registerNonceReceived = function (nonce) {
        var login = self.registerLogin();
        if (nullString(login)) {
            self.error("Login required");
            return;
        }
        var password = self.registerPassword();
        if (nullString(password)) {
            self.error("Password required");
            return;
        }
        var name = self.registerName();
        if (nullString(name)) {
            self.error("Name required");
            return;
        }
        var passwordData = password + "#" + Config.ClientId + "#" + nonce;
        var encryptedPasswordData = Base64.encode(xor(passwordData, Config.ApiKey));
        var data = {
            Login: login,
            Name: name,
            Password: encryptedPasswordData
        };
        window.application.apiCall("Register", data, {
            success: function (r) {
                self.registered(true);
                setTimeout(function () {
                    self.registered(false);
                }, Config.ShowAlertTimeout);
                window.messageBus.fire(Event.UserSaved);
            },
            error: errorHandler,
            done: function () {
                self.registerLogin(null);
                self.registerName(null);
                self.registerPassword(null);
            },
            fail: errorHandler
        });
    }

    self.register = function () {
        self.error(null);
        self.registered(false);
        window.application.apiCall("GetNonce", {}, {
            success: registerNonceReceived,
            error: errorHandler,
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