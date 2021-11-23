var report = (function () {
    $.validator.addMethod("eitherrequired", function (value, element, params) {
        return params.some(x => x.value !== "");
    });

    $.validator.unobtrusive.adapters.add("eitherrequired", function (options) {
        var $handId = $(options.form).find("#HandId")[0];
        var $nickname = $(options.form).find("#Nickname")[0];
        var $username = $(options.form).find("#Username")[0];

        options.rules["eitherrequired"] = [$handId, $nickname, $username];
        options.messages["eitherrequired"] = options.message;
    });

    var $currentTable;

    return {
        onSuccess: function () {
            if ($currentTable) {
                $currentTable.DataTable().clear().destroy();
            }

            $currentTable = $('#report-result table');
            $currentTable.DataTable({
                "ordering": false,
                "lengthMenu": [100, 200, 500],
                "language": {
                    "emptyTable": "No data"
                },
                "autoWidth": false
            });
        },
    };
})();