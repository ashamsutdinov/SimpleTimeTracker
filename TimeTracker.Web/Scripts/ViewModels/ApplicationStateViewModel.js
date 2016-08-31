window.ApplicationStateViewModel = function () {

    var self = this;

    self.loading = ko.observable(true);
    self.networkConnectionError = ko.observable(false);
    self.successMessage = ko.observable(null);
    self.informationMessage = ko.observable(null);
    self.warningMessage = ko.observable(null);
    self.errorMessage = ko.observable(null);

    window.messageBus.subscribe(Event.Alert, function (data) {
        var targetObservable;
        switch (data.Type) {
            case AlertType.Success:
                targetObservable = self.successMessage;
                break;
            case AlertType.Information:
                targetObservable = self.informationMessage;
                break;
            case AlertType.Warning:
                targetObservable = self.warningMessage;
                break;
            default:
                targetObservable = self.errorMessage;
                break;
        }
        targetObservable(data.Text);
        setTimeout(function () {
            targetObservable(null);
        }, Config.ShowAlertTimeout);
    });

    window.messageBus.subscribe(Event.Connected, function () {
        self.loading(false);
        self.networkConnectionError(false);
    });

    window.messageBus.subscribe(Event.Disconnected, function () {
        self.networkConnectionError(true);
    });

    return self;
};