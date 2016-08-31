window.ManageTimeRecordsViewModel = function () {

    var self = new TimeRecordsViewModel(DataRequest.ManageTimeshets);

    self.filterLoadAllUsers(true);

    return self;
};