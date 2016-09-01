window.TimeRecordsViewModel = function (dataId, containerId) {

    var self = new DataContainerViewModelBase(dataId);
    
    self.filterFromDate = ko.observable(new Date());
    self.filterToDate = ko.observable(new Date());
    self.filterLoadAllUsers = ko.observable(false);
    self.filterPageSize = ko.observable(10);
    self.filterPageNumber = ko.observable(1);
    self.filterPageSizeOptions = ko.observableArray([5, 10, 15, 20, 25]);

    self.timeRecords = ko.observableArray([]);
    self.pages = ko.observableArray([]);

    self.filterTimeRecords = function () {
        var from = getDateOnly(self.filterFromDate());
        var to = getDateOnly(self.filterToDate());
        var request = {
            From: toMsDate(from),
            To: toMsDate(to),
            PageNumber: self.filterPageNumber(),
            PageSize: self.filterPageSize(),
            LoadAllUsers: self.filterLoadAllUsers()
        };
        window.application.apiCall("GetTimeRecords", request, {
            success: function (r) {
                self.pages([]);
                if (r.Items != null) {
                    self.timeRecords(r.Items);
                   for (var i = 0; i < r.TotalPages; i++) {
                       self.pages.push(i+1);
                   }
               } else {
                   self.timeRecords([]);
               }
            }
        });
    };

    self.changePage = function (page) {
        if (page !== self.filterPageNumber()) {
            self.filterPageNumber(page);
            self.filterTimeRecords();
        }
    };

    self.exportCurrentPage = function () {
        printElement(containerId);
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