window.Application = function () {

    var self = this;
    var connected = false;

    self.callApi = function (httpMethod, rawUrl, data, callback) {
        var formattedUrl = rawUrl;
        var requestData = {
        }
        for (var prop in data) {
            if (data.hasOwnProperty(prop)) {
                if (~rawUrl.indexOf(prop)) {
                    formattedUrl = formattedUrl.replace("{" + prop + "}", data[prop]);
                } else {
                    requestData[prop] = data[prop];
                }
            }
        }
        var request;
        if (Object.keys(requestData).length === 0) {
            request = null;
        } else {
            switch (httpMethod) {
                case "GET":
                    request = $.param(requestData);
                    break;
                default:
                    request = JSON.stringify(requestData);
                    break;
            }
        }
        var callbackOk = callback.success || function () { };
        var callbackFail = function (response) {
            var responseJson = response.responseJSON;
            if (callback.fail) {
                callback.fail(responseJson);
            }
        };
        var callbackDone = callback.done || function () { };
        var url = Config.ApiRoot + formattedUrl;
        $.ajaxSetup({
            contentType: "application/json; charset=utf-8"
        });
        $.ajax({
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Ticket", localStorage[Config.TicketKey]);
                xhr.setRequestHeader("ClientId", Config.ClientId);
            },
            url: url,
            method: httpMethod,
            data: request,
            success: function (response) {
                callbackOk(response.Data);
            }
        }).fail(callbackFail).done(callbackDone);
    };

    self.apiGet = function (rawUrl, data, callback) {
        self.callApi("GET", rawUrl, data, callback);
    };

    self.apiPost = function (rawUrl, data, callback) {
        self.callApi("POST", rawUrl, data, callback);
    };

    self.apiPut = function (rawUrl, data, callback) {
        self.callApi("PUT", rawUrl, data, callback);
    };

    self.apiDelete = function (rawUrl, data, callback) {
        self.callApi("DELETE", rawUrl, data, callback);
    };



    var handshakeFail = function () {
        localStorage.removeItem(Config.TicketKey);
        window.messageBus.fire(Event.LoggedOut);
        window.messageBus.fire(Event.SessionStateChanged, [SessionState.Anonymous]);
    };

    self.handshake = function () {
        self.apiGet("/Session/Handshake", {}, {
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
        self.apiGet("/Session/Heartbit/{Tick}", { Tick: new Date().getTime() }, {
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
