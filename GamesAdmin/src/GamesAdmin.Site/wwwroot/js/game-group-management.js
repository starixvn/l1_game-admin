var gameGroupManagement = (function ($) {
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
        $modal.modal({ backdrop: 'static', show: true});
        $.validator.unobtrusive.parse(formId);
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

    function openForm(formSelector) {
        var $gameIds = $("#GameIds");
        var selections = gameGroupManagement.getFanexSelectData($gameIds[0]);

        $("#fanexSelectGameId").fanexMultiSelect({
            listData: selections,
            showFilter: true,
            onLeave: function () {
                var value = $("#fanexSelectGameId").fanexMultiSelect('getItems', true);

                $gameIds.val(value);
                $(formSelector).valid();
            },
        });
        openModal(formSelector);
    }

    return {
        init: function () {
            var $table = $('#game-group-table');
            $table.DataTable({
                "order": [[0, "asc"]],
                "lengthMenu": [100, 200, 500]
            });
        },
        openCreateForm: function () {
            openForm("#create-form");
        },
        openUpdateForm: function () {
            openForm("#update-form");
        },
        onCreateSuccess: onUpsertSucess,
        onUpdateSuccess: onUpsertSucess,
        getFanexSelectData: (selectElement) => {
            var data = [];
            for (let options of selectElement.options) {
                data.push(convertPartnerSelectToFanexItem(options));
            }
            return data;
        }
    }
}(jQuery));

$(document).ready(function () {
    gameGroupManagement.init();
})