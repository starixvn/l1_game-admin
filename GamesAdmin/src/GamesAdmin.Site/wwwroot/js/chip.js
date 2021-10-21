var chip = (function ($) {
    var $modal = $('#modal');

    function onUpsert(data) {
        if (data.success) {
            location.reload();
        }
        var label = $("#result-label");

        label.removeClass();
        label.addClass(`text-${data.success ? "message" : "danger"}`);
        label.text(data.error);
    }

    function openModal(formId) {
        return function () {
            $modal.modal('show');
            $.validator.unobtrusive.parse(formId);
        }
    }

    return {
        init: function () {
            var table = $('#chip-table');

            table.DataTable({
                "lengthMenu": [100, 200, 500]
            });
        },
        openAddChipForm: openModal("#add-chip-form"),
        openUpdateChipForm: openModal("#update-chip-form"),
        onCreateChipSuccess: onUpsert,
        onUpdateChipSuccess: onUpsert,
    }
}(jQuery));

$(document).ready(function () {
    chip.init();
})