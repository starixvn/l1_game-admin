var currencyGroup = (function ($) {
    var $modal = $('#modal');


    function onUpsertCurrencyGroup(data) {
        if (data.success) {
            setTimeout(function () {
                $('.modal-footer .btn-submit').prop('disabled', false);
                location.reload();
                $modal.modal('hide');
            }, 1000);
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

        console.log("selections", selections);

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

    const filterGroups = (partnerId) =>
        model.currencyGroupModels.filter((group) => group.partnerId === partnerId);

    const convertCurrencyToItem = (defaultValue) => (currency) => ({
        id: currency.id,
        value: currency.id,
        text: currency.name,
        checked: defaultValue,
        enable: true,
        cssClass: "currency-item",
    });

    const convertGroupToItem = (defaultValue) => (group) => ({
        id: group.id,
        value: group.id,
        text: group.name,
        checked: defaultValue,
        enable: true,
        cssClass: "group-item",
    });

    const convertPartnerToItem = (defaultValue) => (partner) => ({
        id: partner.partnerId,
        value: partner.partnerId,
        text: partner.name,
        checked: defaultValue,
        enable: true,
        cssClass: "partner-item",
        items: filterGroups(partner.partnerId).map(
            convertGroupToItem(defaultValue)
        ),
    });


    return {
        init: function () {
            $('#currency-group-table').DataTable({
                "lengthMenu": [100, 200, 500]
            });
        },
        openAddNewCurrencyGroupForm: openModal("#add-new-currency-group-form"),
        openUpdateCurrencyGroupForm: openModal("#update-currency-group-form"),
        onCreateCurrencyGroupSuccess: onUpsertCurrencyGroup,
        onUpdateCurrencyGroupSuccess: onUpsertCurrencyGroup,
        getFanexSelectData: (value) =>
            model.partners
                .map(convertPartnerToItem(value))
                .filter((partner) => partner.items.length)
    }
}(jQuery));

$(document).ready(function () {
    $("#currency-group-form").submit();

    var selections = currencyGroup.getFanexSelectData(true);
    console.log("selections", selections);

    $("#groups").fanexMultiSelect({
        listData: selections,
        showFilter: true,
        onLeave: function () {
            var value = $("#groups").fanexMultiSelect('getItems', true);

            $("#CurrencyGroups").val(value);
        },
    });
})