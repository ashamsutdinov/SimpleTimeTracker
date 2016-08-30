var StatusCode = {
    OK : 200
};

window.Application = function () {
    var self = this;

    var connected = false;

    self.apiCall = function (method, data, callback) {
        var request = {
            Data: data,
            ClientId: Config.ClientId,
            Ticket: localStorage[Config.TicketKey]
        };
        var callbackOk = callback.success || function() {};
        var callbackError = callback.error || function () { };
        var callbackFailure = callback.failure || function () { };
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
        }).fail(callbackFailure).done(callbackDone);
    };

    self.handshake = function () {
        self.apiCall("Handshake", {}, {
            success: function(states) {
                window.messageBus.fire(Event.SessionStateChanged, states);
            },
            error: function () {
                localStorage[Config.TicketKey] = null;
                window.messageBus.fire(Event.SessionStateChanged, [SessionState.Anonymous]);
            }
        });
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
            failure: function () {
                window.messageBus.fire(Event.Disconnected);
                connected = false;
            }
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
