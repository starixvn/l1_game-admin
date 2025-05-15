var crashTurnGameReportController = (function ($) {
    return {
        init: function () {
            $.validator.addMethod("eitherrequired", function (value, element, params) {
                return params.some(x => x.value !== "");
            });

            $.validator.unobtrusive.adapters.add("eitherrequired", function (options) {
                var $roundId = $(options.form).find("#RoundId")[0];
                var $nickname = $(options.form).find("#Nickname")[0];
                var $username = $(options.form).find("#Username")[0];

                options.rules["eitherrequired"] = [$roundId, $nickname, $username];
                options.messages["eitherrequired"] = options.message;
            });

        },
        initResult: function () {
            $('#tbl-crash-turn-game').DataTable({
                "order": [[0, "desc"]],
                "lengthMenu": [[100, 200, 500], [100, 200, 500]],
                "pageLength": 100
            });
        },
    }
}(jQuery));

(function () {
    crashTurnGameReportController.init();
})();