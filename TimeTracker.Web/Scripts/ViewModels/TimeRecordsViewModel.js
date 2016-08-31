window.TimeRecordsViewModel = function (dataId) {

    var self = new DataContainerViewModelBase(dataId);

    var now = new Date();
    self.filterFromDate = ko.observable(new Date(now.setTime(now.getTime() + -30 * 86400000)));
    self.filterToDate = ko.observable(new Date(now.setTime(now.getTime() + 1 * 86400000)));
    self.filterLoadAllUsers = ko.observable(false);
    self.filterPageSize = ko.observable(10);
    self.filterPageNumber = ko.observable(1);
    self.filterPageSizeOptions = ko.observableArray([5, 10, 15, 20, 25]);

    self.timeRecords = ko.observableArray([]);

    self.filterTimeRecords = function() {
        var request = {
            From: toMsDate(self.filterFromDate()),
            To: toMsDate(self.filterToDate()),
            PageNumber: self.filterPageNumber(),
            PageSize: self.filterPageSize(),
            LoadAllUsers: self.filterLoadAllUsers()
        };
        window.application.apiCall("LoadTimeRecords", request, {
            success:function(r) {
                
            }
        });
    };

    var eventsTriggerReload = [Event.TimeRecordCreated, Event.TimeRecordDeleted, Event.TimeRecordUpdated];

    window.messageBus.subscribe(Event.DataRequested, function(d) {
        if (d === dataId) {
            self.filterTimeRecords();
        }
    });

    window.messageBus.subscribeMany(eventsTriggerReload, function() {
        self.filterTimeRecords();
    });

    return self;
};