window.LoginViewModel = function (loggedInCallback) {
    var self = this;

    self.isAnonymous = ko.observable(false);

    self.loginLogin = ko.observable('');
    self.loginPassword = ko.observable('');

    self.registerLogin = ko.observable('');
    self.registerName = ko.observable('');
    self.registerPassword = ko.observable('');

    self.error = ko.observable(null);
    self.registered = ko.observable(false);

    var unexpectedError = function () {
        self.alert('Unexpected server error');
    };

    var loginNonceReceived = function (nonce) {
        var login = self.loginLogin();
        if (login === null || login === '') {
            self.error('Login required');
            return;
        }
        var password = self.loginPassword();
        if (password === null || password === '') {
            self.error('Password required');
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
                window.messageBus.fire(Event.LoggedIn);
                window.application.handshake();
                if (loggedInCallback !== undefined && loggedInCallback !== null) {
                    loggedInCallback();
                }
            },
            error: function (r) {
                self.error(r.Message);
                setTimeout(function () {
                    self.error(null);
                }, Config.ShowAlertTimeout);
            },
            failure: unexpectedError,
            done: function () {
                self.loginLogin(null);
                self.loginPassword(null);
            }
        });
    }

    self.login = function () {
        self.error(null);
        window.application.apiCall("GetNonce", {}, {
            success: loginNonceReceived,
            failure: unexpectedError,
            error: unexpectedError
        });
    };

    self.logout = function () {
        window.application.apiCall("Logout", {}, {
            done: function() {
                localStorage.removeItem(Config.TicketKey);
                window.messageBus.fire(Event.LoggedOut);
                window.application.handshake();
            }
        });
    };

    var registerNonceReceived = function (nonce) {
        var login = self.registerLogin();
        if (login === null || login === "") {
            self.error("Login required");
            return;
        }
        var password = self.registerPassword();
        if (password === null || password === "") {
            self.error("Password required");
            return;
        }
        var name = self.registerName();
        if (name === null || name === "") {
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
                setTimeout(function() {
                    self.registered(false);
                }, Config.ShowAlertTimeout);
                window.messageBus.fire(Event.UserCreated);
            },
            error: function (r) {
                self.error(r.Message);
                setTimeout(function () {
                    self.error(null);
                }, Config.ShowAlertTimeout);
            },
            failure: unexpectedError,
            done: function () {
                self.registerLogin(null);
                self.registerName(null);
                self.registerPassword(null);
            }
        });
    }

    self.register = function () {
        self.error(null);
        self.registered(false);
        window.application.apiCall("GetNonce", {}, {
            success: registerNonceReceived,
            failure: unexpectedError,
            error: unexpectedError
        });
    };

    var init = function () {
        window.messageBus.subscribe(Event.SessionStateChanged, function (states) {
            self.isAnonymous(false);
            for (var i = 0; i < states.length; i++) {
                if (states[i] === SessionState.Anonymous) {
                    self.isAnonymous(true);
                }
            }
        });
        window.messageBus.subscribe(Event.LogoutRequested, function() {
            self.logout();
        });
    };

    init();

    return self;
};