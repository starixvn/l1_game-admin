var currency = (function ($) {
    var $currencyForm = $("#currency-form");
    var $modal = $('#modal');

    function onUpsertCurrency(data) {
        if (data.success) {
            $currencyForm.submit();
            $modal.modal('hide');
        }
        var label = $("#result-label");

        label.removeClass();
        label.addClass(`text-${data.success ? "message" : "danger"}`);
        label.text(data.message);
    }

    function openModal(formId) {
        return function () {
            $modal.modal('show');
            $.validator.unobtrusive.parse(formId);
        }
    }

    return {
        init: function () {
            var table = $('#currency-table');

            table.DataTable({
                "order": [[0, "asc"]],
                "lengthMenu": [100]
            });
        },
        openAddNewCurrencyForm: openModal("#add-new-currency-form"),
        openUpdateCurrencyForm: openModal("#update-currency-form"),
        onCreateCurrencySuccess: onUpsertCurrency,
        onUpdateCurrencySuccess: onUpsertCurrency
    }
}(jQuery));

$(document).ready(function () {
    $("#currency-form").submit();
})