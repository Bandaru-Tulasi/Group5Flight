$.validator.addMethod("futuredatewithinyears", function (value, element, params) {
    if (!value) {
        return true;
    }
    var date = new Date(value);
    var today = new Date();
    today.setHours(0, 0, 0, 0);
    var maxDate = new Date();
    maxDate.setFullYear(today.getFullYear() + parseInt(params.maxyears));
    return date > today && date <= maxDate;
});

$.validator.unobtrusive.adapters.addSingleVal("futuredatewithinyears", "maxyears");

$.validator.addMethod("maxemission", function (value, element, params) {
    if (!value) {
        return true;
    }
    var emissionValue = parseInt(value);
    return !isNaN(emissionValue) && emissionValue <= parseInt(params.maxvalue);
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