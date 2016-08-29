/**
 * Generates a GUID string.
 * @returns {String} The generated GUID.
 * @example af8a8416-6e18-a307-bd9c-f2c947bbb3aa
 * @author Slavik Meltser (slavik@meltser.info).
 * @link http://slavik.meltser.info/?p=142
 */
function guid() {
    function p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return p8() + p8(true) + p8(true) + p8();
}

// global app configuration
var Config = {
    ApiKey: "d7eb69f5-b0ac-4932-8d80-44159a5fe56b"
};



var clientId = localStorage["ClientId"];
if (clientId === undefined || clientId === null) {
    clientId = guid();
    localStorage["ClientId"] = clientId;
}

Config.ClientId = clientId;