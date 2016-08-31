window.Application = function () {

    var self = this;
    var connected = false;

    self.apiCall = function (method, data, callback) {
        var request = {
            Data: data,
            ClientId: Config.ClientId,
            Ticket: localStorage[Config.TicketKey]
        };
        var callbackOk = callback.success || function () { };
        var callbackFail = callback.fail || function() {
            window.messageBus.fire(Event.Alert, {
                Type: AlertType.Error,
                Text: "Unexpected server error (bad request)"
            });
        };
        var callbackError = callback.error || function(r) {
            var message;
            if (r === null || r === undefined) {
                message = "Unexpected error";
            } else {
                message = r.Message;
            }
            window.messageBus.fire(Event.Alert, {
                Type: AlertType.Error,
                Text: message
            });
        };
        var callbackDone = callback.done || function() {};
        var url = Config.ApiRoot + method;
        $.ajaxSetup({
            contentType: "application/json; charset=utf-8"
        });
        $.ajax({
            url: url,
            contentType: 'application/json',
            method: 'POST',
            data: JSON.stringify(request),
            success: function(response) {
                if (response.StatusCode === StatusCode.OK) {
                    callbackOk(response.Data);
                } else {
                    callbackError(response);
                }
            }
        }).fail(callbackFail).done(callbackDone);
    };

    var handshakeFail = function() {
        localStorage.removeItem(Config.TicketKey);
        window.messageBus.fire(Event.LoggedOut);
        window.messageBus.fire(Event.SessionStateChanged, [SessionState.Anonymous]);
    };

    self.handshake = function () {
        self.apiCall("Handshake", {}, {
            success: function (states) {
                var loggedIn = true;
                for (var i = 0; i < states.length; i++) {
                    if (states[i] === SessionState.Anonymous || states[i] === SessionState.Undefined) {
                        loggedIn = false;
                    }
                }
                if (loggedIn) {
                    window.messageBus.fire(Event.LoggedIn);
                }
                window.messageBus.fire(Event.SessionStateChanged, states);
            },
            error: handshakeFail,
            fail: handshakeFail
        });
    };

    var heartbitFail = function() {
        window.messageBus.fire(Event.Disconnected);
        connected = false;
    };

    var heartbit = function() {
        self.apiCall("Heartbit", new Date().getTime(), {
            success: function () {
                window.messageBus.fire(Event.Connected);
                if (!connected) {
                    self.handshake();
                    connected = true;
                }
            },
            error: heartbitFail,
            fail: heartbitFail
        });
    };

    var init = function () {
        window.messageBus.fire(Event.SessionStateChanged, [SessionState.Undefined]);
        setTimeout(function() {
            heartbit();
            setInterval(heartbit, Config.HeartbitInterval);
        }, Config.HeartbitStartAfter);
    };

    window.messageBus.subscribe(Event.Ready, init);
   
    return self;
};

window.application = new Application();
