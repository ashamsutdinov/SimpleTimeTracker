var Event = {
    Loaded: 0,
    LoggedIn: 1,
    LoggedOut: 2,
    TimeRecordCreated: 3,
    TimeRecordUpdated: 4,
    TimeRecordDeleted: 5,
    TimeRecordNoteCreated: 6,
    TimeRecordNoteUpdated: 7,
    TimeRecordNoteDeleted: 8,
    UserCreated: 9,
    UserUpdated: 10,
    UserDeleted: 11,
    LoadFailed: 1000
};

window.MessageBus = function() {
    var self = this;

    var callbacks = [];

    self.subscribe = function(event, callback) {
        var handlers = callbacks[event];
        if (handlers === undefined || handlers === null) {
            handlers = [];
            callbacks[event] = handlers;
        }
        handlers.push(callback);
    };

    self.fire = function(event, data) {
        var handlers = callbacks[event];
        if (handlers !== undefined && handlers !== null) {
            for (var i = 0; i < handlers.length; i++) {
                handlers[i](data);
            }
        }
    };

    return self;
};

window.messageBus = new MessageBus();