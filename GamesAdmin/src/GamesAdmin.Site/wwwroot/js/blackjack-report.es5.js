"use strict";

(function () {
    $.validator.addMethod("eitherrequired", function (value, element, params) {
        return params.every(function (x) {
            return x.value !== "";
        });
    });

    $.validator.unobtrusive.adapters.add("eitherrequired", function (options) {
        var $handId = $(options.form).find("#HandId")[0];
        var $nickname = $(options.form).find("#Nickname")[0];
        var $username = $(options.form).find("#Username")[0];

        options.rules["eitherrequired"] = [$handId, $nickname, $username];
        options.messages["eitherrequired"] = options.message;
    });
})();

