window.Application = function () {

    var self = this;
    var connected = false;

    self.callApi = function (httpMethod, apiMethod, data, callback) {
        var basicData = {
            ClientId: Config.ClientId,
            Ticket: localStorage[Config.TicketKey]
        }
        var request = null;
        switch (httpMethod)
        {
            case "GET":
                request = $.param($.extend(basicData, data));
                break;
            default:
                basicData.Data = data;
                request = JSON.stringify(basicData);
                break;
        }
        var callbackOk = callback.success || function () { };
        var callbackFail = function (response) {
            var responseJson = response.responseJSON;
            if (callback.fail) {
                callback.fail(responseJson);
            }
        };
        var callbackDone = callback.done || function () { };
        var url = Config.ApiRoot + apiMethod;
        $.ajaxSetup({
            contentType: "application/json; charset=utf-8"
        });
        $.ajax({
            url: url,
            contentType: 'application/json',
            method: httpMethod,
            data: request,
            success: function (response) {
                callbackOk(response.Data);
            }
        }).fail(callbackFail).done(callbackDone);
    };

    self.apiGet = function (method, data, callback) {
        self.callApi('GET', method, data, callback);
    };

    self.apiPost = function (method, data, callback) {
        self.callApi('POST', method, data, callback);
    };

    self.apiPut = function (method, data, callback) {
        self.callApi('PUT', method, data, callback);
    };

    self.apiDelete = function (method, data, callback) {
        self.callApi('DELETE', method, data, callback);
    };

   

    var handshakeFail = function () {
        localStorage.removeItem(Config.TicketKey);
        window.messageBus.fire(Event.LoggedOut);
        window.messageBus.fire(Event.SessionStateChanged, [SessionState.Anonymous]);
    };

    self.handshake = function () {
        self.apiGet("Handshake", {}, {
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

    var heartbitFail = function () {
        window.messageBus.fire(Event.Disconnected);
        connected = false;
    };

    var heartbit = function () {
        self.apiGet("Heartbit", { Tick: new Date().getTime() }, {
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
        setTimeout(function () {
            heartbit();
            setInterval(heartbit, Config.HeartbitInterval);
        }, Config.HeartbitStartAfter);
    };

    window.messageBus.subscribe(Event.Ready, init);

    return self;
};

window.application = new Application();
