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

    function openUpdateBetLimitSuggestionForm() {
        const currencies = [];
        $('#currency-table tbody tr').each((index, element) => {
            currencies.push({
                operatorName: $(element).find('.currency-operator').text(),
                operatorId: $(element).find('.currency-operator').data('operator-id'),
                currencyCode: $(element).find('.currency-code').text(),
                multiplier: Number($(element).find('.currency-multiplier').text()),
                enabled: $(element).find('.currency-enable').data('enabled') === 'True',
                groupName: $(element).find('.group-item').text(),
                groupId: $(element).find('.group-item').data('group-id'),
            });
        });
        const operators = $("#PartnerIds").children().map((index, element) => {
            return {
                operatorName: $(element).text(),
                operatorId: $(element).val(),
            };
        }).toArray();
        globalThis.dispatchEvent(new CustomEvent("showBetLimitSuggestion", {
            detail: {
                currencies: currencies,
                operators: operators,
            }
        }));
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
        openUpdateBetLimitSuggestionForm: openUpdateBetLimitSuggestionForm,
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