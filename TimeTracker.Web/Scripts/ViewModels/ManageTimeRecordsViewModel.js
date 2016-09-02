window.ManageTimeRecordsViewModel = function (options) {

    var self = new TimeRecordsViewModel(DataRequest.ManageTimeshets, options);

    self.filterLoadAllUsers(true);

    return self;
};