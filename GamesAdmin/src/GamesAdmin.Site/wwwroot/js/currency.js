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

    function openAddNewCurrencyWithSuggestionForm() {
        $("#modal .modal-dialog.currency-model").addClass("modal-lg");
        openModal("#add-new-currency-with-suggestion-form");
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

    function onPartnerChange() {
        var partnerId = $('#PartnerId').val();
        if (partnerId) {
            // Populate currency groups for the selected partner
            populateCurrencyGroups(partnerId);
        }
    }

    function onGetFromAllOperatorsChange() {
        var getFromAllOperators = $('#GetFromAllOperators').val();
        if (getFromAllOperators) {
            $('#partnerSetting').hide();
        } else {
            $('#partnerSetting').show();
        }
    }

    function populateCurrencyGroups(partnerId) {
        $.ajax({
            url: '/Currency/GetCurrencyGroupSuggestions',
            type: 'POST',
            data: { PartnerId: partnerId },
            success: function (data) {
                var $select = $('#selectedCurrencyGroupId');
                $select.empty();
                $select.append('<option value="">Select a currency group...</option>');

                if (data.currencyGroups) {
                    data.currencyGroups.forEach(function (group) {
                        $select.append('<option value="' + group.value + '">' + group.text + '</option>');
                    });
                }
            },
            error: function () {
                console.error('Failed to load currency groups');
            }
        });
    }

    function getCurrencyGroupSuggestions() {
        var partnerId = $('#PartnerId').val();
        var rate = parseFloat($('#suggestedRate').val());
        var getFromAllOperators = $('#GetFromAllOperators').val();

        $.ajax({
            url: '/Currency/GetExistingCurrencyGroupSuggestions',
            type: 'POST',
            data: { PartnerId: partnerId, Rate: rate, GetFromAllOperators: getFromAllOperators },
            success: function (data) {
                $('#suggestionsContent').html(data);
                $('#suggestionsContainer').show();
            },
            error: function () {
                alert('Failed to get suggestions.');
            }
        });
    }

    function getCurrencyGroupGameSuggestions() {
        var currencyGroupId = $('#selectedCurrencyGroupId').val();
        var rate = parseFloat($('#suggestedRate').val());

        if (!currencyGroupId) {
            alert('Please select a currency group first.');
            return;
        }

        if (!rate || rate <= 0) {
            alert('Please enter a valid rate.');
            return;
        }

        $.ajax({
            url: '/Currency/GetCurrencyGroupGameSuggestions',
            type: 'POST',
            data: {
                CurrencyGroupId: currencyGroupId,
                Rate: rate
            },
            success: function (data) {
                $('#suggestionsContent').html(data);
                $('#suggestionsContainer').show();
            },
            error: function () {
                alert('Failed to get suggestions.');
            }
        });
    }

    function onCreateCurrencyWithSuggestionSuccess(data) {
        if (data.success) {
            setTimeout(function () {
                $('.modal-footer .btn-submit').prop('disabled', false);
                $('#add-new-currency-with-suggestion-form').submit();
                $modal.modal('hide');
            }, 1000);
        }
        var label = $("#result-label");

        label.removeClass();
        label.addClass(`text-${data.success ? "message" : "danger"}`);
        label.text(data.message);
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
        openAddNewCurrencyWithSuggestionForm: openAddNewCurrencyWithSuggestionForm,
        openUpdateCurrencyForm: openModal("#update-currency-form"),
        onCreateCurrencySuccess: onUpsertCurrency,
        onUpdateCurrencySuccess: onUpsertCurrency,
        onCreateCurrencyWithSuggestionSuccess: onCreateCurrencyWithSuggestionSuccess,
        onClickIsTestCurrency: onClickIsTestCurrency,
        onPartnerChange: onPartnerChange,
        getCurrencyGroupSuggestions: getCurrencyGroupSuggestions,
        getCurrencyGroupGameSuggestions: getCurrencyGroupGameSuggestions,
        getFanexSelectData: () => partners.map(convertPartnerSelectToFanexItem),
        onGetFromAllOperatorsChange: onGetFromAllOperatorsChange
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