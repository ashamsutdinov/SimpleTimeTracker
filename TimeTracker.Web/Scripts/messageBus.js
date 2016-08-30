﻿var Event = {
    Ready : 0,
    Connected: 1,
    Disconnected: 2,
    SessionStateChanged: 3,

    LoggedIn: 11,
    LoggedOut: 12,

    TimeRecordCreated: 21,
    TimeRecordUpdated: 22,
    TimeRecordDeleted: 23,

    TimeRecordNoteCreated: 31,
    TimeRecordNoteUpdated: 32,
    TimeRecordNoteDeleted: 33,

    UserCreated: 41,
    UserUpdated: 42,
    UserDeleted: 43,

    Alert: 51
};

var SessionState = {
    Undefined: 0,
    Anonymous: 1,
    LoggedInUser: 2,
    LoggedInManager: 3,
    LoggedInAdministrator: 4
};

var AlertType = {
    Success: 0,
    Information: 1,
    Warning: 2,
    Error: 3
}

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