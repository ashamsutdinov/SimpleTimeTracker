window.UserSettingsViewModel = function () {

    var self = new DataContainerViewModelBase(DataRequest.UserSettings);

    self.userSettings = ko.observableArray([]);

    self.loadSettings = function () {
        var request = {
            UserId : 0
        };
        window.application.apiCall("GetUserSettings", request, {
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
        window.application.apiCall("SaveUserSettings", request, {
            success: function() {
                window.messageBus.fire(Event.Alert, {
                    Type: AlertType.Success,
                    Text: "User settings successfully updated"
                });
            }
        });
    };

    window.messageBus.subscribe(Event.UserSettingUpdated, function () {
        self.loadSettings();
    });

    window.messageBus.subscribe(Event.DataRequested, function (d) {
        if (d === DataRequest.UserSettings) {
            self.loadSettings();
        }
    });

    return self;
};