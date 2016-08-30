﻿var StatusCode = {
    OK : 200
};

window.Application = function () {
    var self = this;

    var connected = false;

    self.apiCall = function (method, data, callback) {
        var request = {
            Data: data,
            ClientId: Config.ClientId,
            Ticket: $.cookie(Config.TicketKey)
        };
        var callbackOk = callback.success || function() {};
        var callbackFailure = callback.failure || function() {};
        var callbackError = callback.error || function() {};
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
                    callbackFailure(response);
                }
            }
        }).fail(function() {
            callbackError();
        });
    };

    var handshake = function () {
        self.apiCall("Handshake", {}, {
            success: function(response) {
                window.messageBus.fire(Event.SessionStateChanged, response.Data);
            },
            error: function() {
                window.messageBus.fire(Event.SessionStateChanged, [SessionState.Undefined]);
            }
        });
    };

    var heartbit = function() {
        self.apiCall("Heartbit", new Date().getTime(), {
            success: function () {
                window.messageBus.fire(Event.Connected);
                if (!connected) {
                    handshake();
                    connected = true;
                }
            },
            error: function () {
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
