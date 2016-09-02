﻿window.TimeRecordsViewModel = function (dataId, options) {
    var self = new DataContainerViewModelBase(dataId);
    var containerId = options.container;
    
    self.filterFromDate = ko.observable(new Date());
    self.filterToDate = ko.observable(new Date());
    self.filterLoadAllUsers = ko.observable(false);
    self.filterPageSize = ko.observable(10);
    self.filterPageNumber = ko.observable(1);
    self.filterPageSizeOptions = ko.observableArray([5, 10, 15, 20, 25]);

    self.timeRecords = ko.observableArray([]);
    self.pages = ko.observableArray([]);

    self.timeRecordEntryId = ko.observable(0);
    self.timeRecordEntryDate = ko.observable(new Date());
    self.timeRecordEntryDescription = ko.observable("");
    self.timeRecordEntryHours = ko.observable(1);
    self.timeRecordEntryError = ko.observable(null);

    self.timeRecordNoteDayRecordId = ko.observable(0);
    self.timeRecordNoteText = ko.observable(null);

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

    self.pageSizeChanged = function(a) {
        self.filterPageNumber(1);
        self.filterTimeRecords();
    };

    self.exportCurrentPage = function () {
        printElement(containerId);
    };

    self.editTimeRecord = function(e) {
        self.timeRecordEntryId(e.Id);
        self.timeRecordEntryDate(fromMsDate(e.Date));
        self.timeRecordEntryDescription(e.Caption);
        self.timeRecordEntryHours(e.Hours);
        self.timeRecordEntryError(null);
        window.messageBus.fire(Event.BeginDialog, options.timeRecordWindow);
    };

    self.deleteTimeRecord = function (e) {
        confirmDelete(function() {
            window.application.apiCall("DeleteTimeRecord", e.Id, {
                success: function() {
                    window.messageBus.fire(Event.TimeRecordDeleted);
                }
            });
        });
    };

    self.submitTimeRecord = function () {
        self.timeRecordEntryError(null);
        var date = self.timeRecordEntryDate();
        if (nullObject(date)) {
            self.timeRecordEntryError("Date required");
            return;
        }

        var caption = self.timeRecordEntryDescription();
        if (nullString(caption)) {
            self.timeRecordEntryError("Description required");
            return;
        }

        var hours = self.timeRecordEntryHours();
        if (nullNumber(hours)) {
            self.timeRecordEntryHours("Duration required");
            return;
        }
        var timeRecordDate = getDateOnly(self.timeRecordEntryDate());
        var request = {
            Id: self.timeRecordEntryId(),
            Date: toMsDate(timeRecordDate),
            Caption: self.timeRecordEntryDescription(),
            Hours: self.timeRecordEntryHours()
        };

        window.application.apiCall("SaveTimeRecord", request, {
            success: function () {
                window.messageBus.fire(Event.EndDialog);
                window.messageBus.fire(Event.TimeRecordSaved);
                self.timeRecordEntryError(null);
                self.timeRecordEntryId(0);
                self.timeRecordEntryDescription(null);
                self.timeRecordEntryHours(1);
            },
            error: function (r) {
                if (r !== null && r !== undefined) {
                    self.timeRecordEntryError(r.Message);
                } else {
                    self.timeRecordEntryError("Unexpected error");
                }
            }
        });
    };

    self.addTimeRecordNote = function(e) {
        typeValue("Leave your note for this record", function(result) {
            self.timeRecordNoteDayRecordId(e.Id);
            self.timeRecordNoteText(result);
            self.submitTimeRecordNote();
        });
    };

    self.submitTimeRecordNote = function() {
        var request = {
            Id: 0,
            DayRecordId: self.timeRecordNoteDayRecordId(),
            Text: self.timeRecordNoteText()
        };
        window.application.apiCall("SaveTimeRecordNote", request, {
            success: function() {
                window.messageBus.fire(Event.TimeRecordNoteSaved);
            }
        });
    };

    self.deleteTimeRecordNote = function(e) {
        confirmDelete(function() {
            window.application.apiCall("DeleteTimeRecordNote", e.Id, {
                success: function() {
                    window.messageBus.fire(Event.TimeRecordNoteDeleted);
                }
            });
        });
    };

    var eventsTriggerReload = [Event.TimeRecordSaved, Event.TimeRecordDeleted, Event.TimeRecordNoteSaved, Event.TimeRecordNoteDeleted];

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