var vietlottKeno = (function ($) {
    var $kenoForm = $("#keno-form");
    var $modal = $('#modal');
    return {
        init: function () {
            $('#vietlott-keno-table').DataTable({
                "ordering": false,
                "lengthMenu": [100],
            });
        },
        onOpenAddNewRoundForm: function () {
            $modal.modal('show');
            $.validator.unobtrusive.parse("#add-new-round-form");
        },
        onAddNewRoundSuccess: function (data) {
            if (data.isSuccessful) {
                $modal.modal('hide');
                $kenoForm.submit();
            } else {
                if (data.errors) {
                    debugger;
                    var errorObj = {};
                    for (var i = 0; i < data.errors.length; i++) {
                        var error = data.errors[i];
                        errorObj[error.key] = error.errorMessage;
                    }

                    var validate = $("#add-new-round-form").validate();
                    validate.showErrors(errorObj);
                }
            }
        }
    }
}(jQuery));

$(document).ready(function () {
    $("#keno-form").submit();
});