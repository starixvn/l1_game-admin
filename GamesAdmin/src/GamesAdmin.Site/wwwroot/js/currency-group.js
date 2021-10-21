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

    function updateFanexMultiSelectInModal() {
        var supportedCurrencies = $(".currencies-class option").map((_, item) => item.value).toArray();
        var partnerId = $("#PartnerId").val();
        var currencies = $(".currencies-class").val();
        var selections = filterCurrencies(supportedCurrencies, partnerId).map(
            convertCurrencyToItem(false)
        );

        $("#currencies-add").fanexMultiSelect({
            listData: selections,
            showFilter: true,
            onLeave: function () {
                var value = $("#currencies-add").fanexMultiSelect('getItems', true);

                $(".currencies-class").val(value);
            },
        });

        $("#currencies-update").fanexMultiSelect({
            listData: selections.map(item => {
                item.checked = !!currencies.filter(id => id === item.id).length;

                console.log(item, currencies);
                return item;
            }),
            showFilter: true,
            onLeave: function () {
                var value = $("#currencies-update").fanexMultiSelect('getItems', true);

                $(".currencies-class").val(value);
            },
        });
    }

    function openModal(formId) {
        return function () {
            $modal.modal('show');
            $.validator.unobtrusive.parse(formId);

            $("#PartnerId").change(updateFanexMultiSelectInModal);
            updateFanexMultiSelectInModal();
        }
    }

    const filterCurrencies = (currencyIds, partnerId) =>
        model.currencyModels.filter(
            (currency) => currency.partnerId === partnerId && currencyIds && currencyIds.filter((id) => id === currency.id).length
        );

    const convertCurrencyToItem = (defaultValue) => (currency) => ({
        id: currency.id,
        value: currency.id,
        text: currency.name,
        checked: defaultValue,
        enable: true,
        cssClass: "currency-item",
    });

    const convertPartnerToItem = (currencies, defaultValue) => (partner) => ({
        id: partner.partnerId,
        value: partner.partnerId,
        text: partner.name,
        checked: defaultValue,
        enable: true,
        cssClass: "partner-item",
        items: filterCurrencies(currencies, partner.partnerId).map(
            convertCurrencyToItem(defaultValue)
        ),
    });


    return {
        init: function () {
            $('#currency-group-table').DataTable({
                "lengthMenu": [100]
            });
        },
        openAddNewCurrencyGroupForm: openModal("#add-new-currency-group-form"),
        openUpdateCurrencyGroupForm: openModal("#update-currency-group-form"),
        onCreateCurrencyGroupSuccess: onUpsertCurrencyGroup,
        onUpdateCurrencyGroupSuccess: onUpsertCurrencyGroup,
        getFanexSelectData: (currencies, value = true) =>
            model.partners
                .map(convertPartnerToItem(currencies, value))
                .filter((partner) => partner.items.length)
    }
}(jQuery));

$(document).ready(function () {
    $("#currency-group-form").submit();

    var suportedCurrencies = $("#Currencies option").map((_, item) => item.value).toArray();
    var selections = currencyGroup.getFanexSelectData(suportedCurrencies, false);

    $("#currencies").fanexMultiSelect({
        listData: selections,
        showFilter: true,
        onLeave: function () {
            var value = $("#currencies").fanexMultiSelect('getItems', true);

            $("#Currencies").val(value);
        },
    });
})