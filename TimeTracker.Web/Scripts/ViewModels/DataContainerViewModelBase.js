window.DataContainerViewModelBase = function(dataId) {
    var self = this;

    self.loaded = ko.observable(false);

    var init = function() {
        window.messageBus.subscribe(Event.DataRequested, function(d) {
            self.loaded(d === dataId);
        });
        window.messageBus.subscribe(Event.LoggedOut, function() {
            self.loaded(false);
        });
    };

    init();

    return self;
};