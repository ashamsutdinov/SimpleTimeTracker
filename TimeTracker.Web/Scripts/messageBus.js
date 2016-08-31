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

    self.subscribeMany = function(events, callback) {
        for (var i = 0; i < events.length; i++) {
            self.subscribe(events[i], callback);
        }
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