window.DataContainerViewModelBase = function (dataId) {

    var self = this;

    self.loaded = ko.observable(false);

    self.load = function() {

    };

    self.clear = function() {

    };

    window.messageBus.subscribe(Event.DataRequested, function (d) {
        var loaded = d === dataId;
        self.loaded(loaded);
        if (loaded) {
            self.load();
        } else {
            self.clear();
        }
    });

    window.messageBus.subscribe(Event.LoggedOut, function () {
        self.loaded(false);
    });

    return self;
};