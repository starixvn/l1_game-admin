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

    function convertPartnerSelectToFanexItem(item) {
        return ({
            id: item.value,
            value: item.value,
            text: item.text,
            enable: !item.disabled,
            checked: item.selected
        })
    }

    return {
        init: function () {
            var table = $('#currency-table');

            table.DataTable({
                "order": [[0, "asc"]],
                "lengthMenu": [100, 200, 500]
            });
        },
        openAddNewCurrencyForm: openModal("#add-new-currency-form"),
        openUpdateCurrencyForm: openModal("#update-currency-form"),
        onCreateCurrencySuccess: onUpsertCurrency,
        onUpdateCurrencySuccess: onUpsertCurrency,
        getFanexSelectData: () => partners.map(convertPartnerSelectToFanexItem)
    }
}(jQuery));

$(document).ready(function () {
    $("#currency-form").submit();

    var selections = currency.getFanexSelectData();

    $("#partners").fanexMultiSelect({
        listData: selections,
        showFilter: true,
        onLeave: function () {
            var value = $("#partners").fanexMultiSelect('getItems', true);

            $("#PartnerIds").val(value);
        },
    });
})