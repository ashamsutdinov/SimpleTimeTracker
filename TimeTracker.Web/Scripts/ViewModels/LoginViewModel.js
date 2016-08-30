window.LoginViewModel = function() {
    var self = this;

    self.isAnonymous = ko.observable(false);

    self.loginLogin = ko.observable('');
    self.loginPassword = ko.observable('');
    
    self.registerLogin = ko.observable('');
    self.registerName = ko.observable('');
    self.registerPassword = ko.observable('');

    self.error = ko.observable(null);

    var unexpectedError = function() {
        self.alert('Unexpected server error');
    };

    var nonceReceived = function(nonce) {
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
            success: function(r) {
                localStorage[Config.TicketKey] = r.Ticket;
                window.messageBus.fire(Event.LoggedIn);
                window.application.handshake();
            },
            error: function(r) {
                self.error(r.Message);
            },
            failure: unexpectedError,
            done: function() {
                self.loginLogin(null);
                self.loginPassword(null);
            }
        });
    }

    self.login = function() {
        self.error(null);
        window.application.apiCall("GetNonce", {}, {
            success: nonceReceived,
            failure: unexpectedError,
            error: unexpectedError
        });
    };

    self.register = function() {
        self.error(null);
    };

    var init = function() {
        window.messageBus.subscribe(Event.SessionStateChanged, function (states) {
            self.isAnonymous(false);
            for (var i = 0; i < states.length; i++) {
                if (states[i] === SessionState.Anonymous) {
                    self.isAnonymous(true);
                } 
            }
        });
    };

    init();

    return self;
};