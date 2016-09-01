window.ManageTimeRecordsViewModel = function (containerId) {

    var self = new TimeRecordsViewModel(DataRequest.ManageTimeshets, containerId);

    self.filterLoadAllUsers(true);

    return self;
};