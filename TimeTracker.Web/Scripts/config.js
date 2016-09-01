﻿// global app configuration
var Config = {
    ApiKey: "d7eb69f5-b0ac-4932-8d80-44159a5fe56b",
    ApiRoot: "http://localhost:8001/",
    TicketKey: "AuthenticationTicket",
    HeartbitStartAfter: 1000,
    HeartbitInterval: 10000,
    ShowAlertTimeout: 5000
};

var clientId = localStorage["ClientId"];
if (clientId === undefined || clientId === null) {
    clientId = guid();
    localStorage["ClientId"] = clientId;
}

Config.ClientId = clientId;

