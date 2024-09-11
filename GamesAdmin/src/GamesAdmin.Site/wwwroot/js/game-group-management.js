var groupGameManagement = (function ($) {
    var $modal = $('#modal');

    function onUpsertSucess(data) {
        if (data.success) {
            setTimeout(function () {
                window.location.reload();
            }, 1000);
        }

        $('.modal-footer .btn-submit').prop('disabled', false);
        $modal.modal('hide');
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
            enable: true,
            checked: item.selected
        })
    }


    return {
        init: function () {
            var $table = $('#game-group-table');

            $table.DataTable({
                "order": [[0, "asc"]],
                "lengthMenu": [100, 200, 500]
            });
        },
        openCreateForm: openModal("#create-form"),
        onCreateSuccess: onUpsertSucess,
        onUpdateSuccess: onUpsertSucess,
        getFanexSelectData: (selectElement) => selectElement.map(convertPartnerSelectToFanexItem)
    }
}(jQuery));

$(document).ready(function () {
    var $gameIds = $("#GameIds");
    var selections = groupGameManagement.getFanexSelectData($gameIds[0]);

    $("#fanexSelectGameId").fanexMultiSelect({
        listData: selections,
        showFilter: true,
        onLeave: function () {
            var value = $("#fanexSelectGameId").fanexMultiSelect('getItems', true);

            $gameIdsval(value);
        },
    });
})