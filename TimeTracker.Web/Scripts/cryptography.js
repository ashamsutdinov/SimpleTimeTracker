function guid() {
    function p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return p8() + p8(true) + p8(true) + p8();
}

function xor(s, k) {
    var enc = "";
    for (var i = 0; i < s.length; i++) {
        var a = s.charCodeAt(i);
        var b = a ^ k;
        enc = enc + String.fromCharCode(b);
    }
    return enc;
}