window.LoaderViewModel = function() {
    var self = this;

    self.loading = ko.observable(true);
    self.networkConnectionError = ko.observable(false);

    function init() {
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