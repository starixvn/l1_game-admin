var currencyGroup = (function ($) {
    var $currencyForm = $("#currency-group-form");
    var $modal = $('#modal');

    function onUpsertCurrencyGroup(data) {
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
            $('#currency-group-table').DataTable({
                "lengthMenu": [100]
            });
        },
        openAddNewCurrencyGroupForm: openModal("#add-new-currency-group-form"),
        openUpdateCurrencyGroupForm: openModal("#update-currency-group-form"),
        onCreateCurrencySuccess: onUpsertCurrencyGroup,
        onUpdateCurrencySuccess: onUpsertCurrencyGroup
    }
}(jQuery));

$(document).ready(function () {
    $("#currency-group-form").submit();

    $("#partners").select2({
        placeholder: "Select partners",
        allowClear: true
    });
    $("#currencies").select2({
        placeholder: "Select currencies",
        allowClear: true
    });
})