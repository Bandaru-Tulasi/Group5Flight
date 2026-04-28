$.validator.addMethod("futuredatewithinyears", function (value, element, params) {
    if (!value) {
        return true;
    }
    var parts = value.split("-");
    var inputYear = parseInt(parts[0]);
    var inputMonth = parseInt(parts[1]);
    var inputDay = parseInt(parts[2]);
    var today = new Date();
    var todayYear = today.getFullYear();
    var todayMonth = today.getMonth() + 1;
    var todayDay = today.getDate();
    var inputDateValue = inputYear * 10000 + inputMonth * 100 + inputDay;
    var todayValue = todayYear * 10000 + todayMonth * 100 + todayDay;
    if (inputDateValue <= todayValue) {
        return false;
    }
    var maxYear = todayYear + parseInt(params.maxyears);
    var maxDateValue = maxYear * 10000 + todayMonth * 100 + todayDay;
    if (inputDateValue > maxDateValue) {
        return false;
    }
    return true;
});

$.validator.unobtrusive.adapters.addSingleVal("futuredatewithinyears", "maxyears");

$.validator.addMethod("maxemission", function (value, element, params) {
    if (!value) {
        return true;
    }
    var emissionValue = parseInt(value);
    if (isNaN(emissionValue)) {
        return true;
    }
    return emissionValue <= parseInt(params.maxvalue);
});

$.validator.unobtrusive.adapters.addSingleVal("maxemission", "maxvalue");

$.validator.addMethod("pricerange", function (value, element, params) {
    if (!value) {
        return true;
    }
    var price = parseFloat(value);
    return !isNaN(price) && price >= parseFloat(params.minvalue) && price <= parseFloat(params.maxvalue);
});

$.validator.unobtrusive.adapters.add("pricerange", ["minvalue", "maxvalue"], function (options) {
    options.rules["pricerange"] = {
        minvalue: options.params.minvalue,
        maxvalue: options.params.maxvalue
    };
    options.messages["pricerange"] = options.message;
});