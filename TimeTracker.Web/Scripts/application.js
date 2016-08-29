var StatusCode = {
    OK : 200
};

window.Application = function () {
    var self = this;

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

    var heartbit = function() {
        self.apiCall("Heartbit", new Date().getTime(), {
            success: function () {
                messageBus.fire(Event.Loaded);
            },
            error: function () {
                messageBus.fire(Event.LoadFailed);
            }
        });
    };

    var init = function () {
        setTimeout(function() {
            heartbit();
            setInterval(heartbit, Config.HeartbitInterval);
        }, Config.HeartbitStartAfter);
    };

    init();
   
    return self;
};

window.application = new Application();
