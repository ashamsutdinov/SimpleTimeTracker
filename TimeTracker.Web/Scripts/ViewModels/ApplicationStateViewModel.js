window.ApplicationStateViewModel = function() {
    var self = this;

    self.loading = ko.observable(true);
    self.networkConnectionError = ko.observable(false);
    self.successMessage = ko.observable(null);
    self.informationMessage = ko.observable(null);
    self.warningMessage = ko.observable(null);
    self.errorMessage = ko.observable(null);

    function init() {
        window.messageBus.subscribe(Event.Message, function(data) {
            var targetObservable = null;
            switch (data.Type) {
                case MessageType.Success:
                    targetObservable = self.successMessage;
                    break;
                case MessageType.Information:
                    targetObservable = self.informationMessage;
                    break;
                case MessageType.Warning:
                    targetObservable = self.warningMessage;
                    break;
                default:
                    targetObservable = self.errorMessage;
                    break;
            }
            targetObservable(data.Text);
            setTimeout(function() {
                targetObservable(null);
            }, Config.ShowAlertTimeout);
        });
        window.messageBus.subscribe(Event.Loaded, function() {
            self.loading(false);
            self.networkConnectionError(false);
        });
        window.messageBus.subscribe(Event.LoadFailed, function() {
            self.networkConnectionError(true);
        });
    };

    init();

    return self;
};