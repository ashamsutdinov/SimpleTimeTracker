ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().datepickerOptions || {};
        $(element).datepicker(options);
        ko.utils.registerEventHandler(element, "changeDate", function (event) {
            var value = valueAccessor();
            if (ko.isObservable(value)) {
                value(event.date);
            }
        });
    },
    update: function (element, valueAccessor) {
        var widget = $(element).data("datepicker");
        if (widget) {
            widget.date = ko.utils.unwrapObservable(valueAccessor());
            widget.setValue();
        }
    }
};

function nullObject(d) {
    return d === undefined || d === null;
}

function nullString(s) {
    return nullObject(s) || s === "";
}

function nullNumber(n) {
    return nullObject(n) || n === 0;
}

function toMsDate(date) {
    return "\/Date(" + date.valueOf() + ")\/";
}

function fromMsDate(date) {
    var prepared = date.replace("/Date(", "").replace(")/","");
    var operator = 1;
    var utcDate = 0;
    var hours = 0;
    var parts;
    if (prepared.indexOf("+") !== -1) {
        parts = prepared.split("+");
        utcDate = parts[0] * 1;
        hours = (parts[1] * 1) / 100;
    } else if (prepared.indexOf("-") !== -1) {
        parts = prepared.split("-");
        utcDate = parts[0] * 1;
        hours = (parts[1] * 1) / 100;
        operator = -1;
    } else {
        return new Date(prepared);
    }
    var result = utcDate + operator * hours * 3600 * 1000;
    return new Date(result);
}

function getDateOnly(date) {
    date.setHours(0, 0, 0, 0, 0);
    var year = date.getFullYear();
    var month = date.getMonth();
    var day = date.getDate();
    var utcDate = new Date(Date.UTC(year, month, day));
    return utcDate;
}

function popupWindow(data) {
    var wd = $(window).width();
    var ht = $(window).height();
    var printWindow = window.open("", "printWindow", "height="+ht+ ", width="+wd);
    var doc = printWindow.document;
    doc.write("<html><head><title>Export time records</title>");
    doc.write("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css\" type=\"text/css\" />");
    doc.write("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css\" type=\"text/css\" />");
    doc.write("<link rel=\"stylesheet\" href=\"./Styles/Site.css\" type=\"text/css\" />");
    doc.write("</head><body><div class=\"container\"><div class=\"row\">");
    doc.write(data);
    doc.write("</div></div></body></html>");
    doc.close();
    //printWindow.focus();
    //printWindow.print();
    //printWindow.close();
    return true;
}

function printElement(id) {
    var element = $("#" + id);
    var code = element.html();
    var newElement = $(code);
    var cruds = newElement.find(".crud-element");
    cruds.remove();
    popupWindow(newElement.html());
}