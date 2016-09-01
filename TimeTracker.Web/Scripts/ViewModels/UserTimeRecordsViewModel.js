window.UserTimeRecordsViewModel = function (containerId) {

    var self = new TimeRecordsViewModel(DataRequest.UserTimesheets, containerId);

    self.newRecordDate = ko.observable(new Date());
    self.newRecordDescription = ko.observable("");
    self.newRecordHours = ko.observable(0);

    self.submitTimeRecord = function () {

        var date = self.newRecordDate();
        if (nullObject(date)) {
            window.messageBus.fire(Event.Alert, {
                Type: AlertType.Error,
                Text: "Date required"
            });
            return;
        }

        var caption = self.newRecordDescription();
        if (nullString(caption)) {
            window.messageBus.fire(Event.Alert, {
                Type: AlertType.Error,
                Text: "Caption required"
            });
            return;
        }

        var hours = self.newRecordHours();
        if (nullNumber(hours)) {
            window.messageBus.fire(Event.Alert, {
                Type: AlertType.Error,
                Text: "Hours required"
            });
            return;
        }
        var newRecordDate = getDateOnly(self.newRecordDate());
        var request = {
            Id: 0,
            Date: toMsDate(newRecordDate),
            Caption: self.newRecordDescription(),
            Hours: self.newRecordHours()
        };

        window.application.apiCall("CreateTimeRecord", request, {
            success: function () {
                window.messageBus.fire(Event.Alert, {
                    Type: AlertType.Success,
                    Text: "Time record successfully created"
                });
                window.messageBus.fire(Event.TimeRecordCreated);
            }
        });
    };

    return self;
};