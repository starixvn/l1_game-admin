var partner = (function ($) {
    return {
        showEditForm: function (partnerId) {
            $.ajax({
                method: "GET",
                url: "/partner/" + partnerId
            }).done(function (content) {
                if (content) {
                    $('#editModalContent').html(content);
                    $('#editModal').modal('show');
                    $.validator.unobtrusive.parse('#editForm');
                }
            });
        },
        upsertResultHandle: function (result) {
            $('#submitBtn').prop('disabled', true);

            if (result.success) {
                $('#successMsg').show();
            }
            else {
                $('#failedMsg').html(result.error);
                $('#failedMsg').show();
            }

            setTimeout(function () {
                $('#submitBtn').prop('disabled', false);
                result.success && location.reload();
            }, 1000);
        },
        showAddForm: function () {
            $.ajax({
                method: "GET",
                url: "/partner/add"
            }).done(function (content) {
                if (content) {
                    $('#addModalContent').html(content);
                    $('#addModal').modal('show');
                    $.validator.unobtrusive.parse('#addForm');
                }});
        }
    }
}(jQuery));

$(document).ready(function () {
    $('#partner-table').DataTable({
        "order": [[0, "asc"]],
        "lengthMenu": [100, 200, 500]
    });
})