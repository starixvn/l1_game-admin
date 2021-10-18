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
                }
            });
        },
        updatePartner: function () {

            var editForm = $('#editForm');
            editForm.validate();
            debugger
            if (editForm.valid()) {
                $.ajax({
                    method: "POST",
                    url: "partner/edit",
                    dataType: 'json',
                    data: editForm.serialize(),
                }).done(function (result) {
                    $('#updateBtn').prop('disabled', true);

                    if (result) {
                        $('#successMsg').show();
                    }
                    else {
                        $('#failedMsg').show();
                    }

                    setTimeout(function () {
                        $('#updateBtn').prop('disabled', false);
                        location.reload();
                    }, 1000);
                });
            }
        }
    }
}(jQuery));