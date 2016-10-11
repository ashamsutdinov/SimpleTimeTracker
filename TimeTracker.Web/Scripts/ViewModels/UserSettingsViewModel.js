window.UserSettingsViewModel = function () {

    var self = new DataContainerViewModelBase(DataRequest.UserSettings);

    self.userSettings = ko.observableArray([]);

    self.loadSettings = function () {
        var request = {
            UserId : 0
        };
        window.application.apoPost("GetUserSettings", request, {
            success: function (r) {
                self.userSettings(r.Items);
            }
        });
    };

    self.submitUserSettings = function() {
        var request = {
            UserId: 0,
            Items: self.userSettings()
        };
        window.application.apoPost("SaveUserSettings", request, {
            success: function() {
                window.messageBus.fire(Event.Alert, {
                    Type: AlertType.Success,
                    Text: "User settings successfully updated"
                });
            }
        });
    };

    self.load = function() {
        self.loadSettings();
    };

    self.clear = function() {
        self.userSettings([]);
    };

    window.messageBus.subscribe(Event.UserSettingUpdated, function () {
        self.loadSettings();
    });

    return self;
};