var currency = (function ($) {
    var $currencyForm = $("#currency-form");
    var $modal = $('#modal');

    function onUpsertCurrency(data) {
        if (data.success) {
            setTimeout(function () {
                $('.modal-footer .btn-submit').prop('disabled', false);
                $currencyForm.submit();
                $modal.modal('hide');
            }, 1000);
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

    function onClickIsTestCurrency(event) {
        event.target.checked ? $('#isUncountOnTotalSetting').show() : $('#isUncountOnTotalSetting').hide();
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
        onClickIsTestCurrency: onClickIsTestCurrency,
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