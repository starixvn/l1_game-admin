var vietlottKeno = (function ($) {
    return {
        init: function () {
            $('#vietlott-keno-table').DataTable({
                "ordering": false,
                "lengthMenu": [100],
            });
        },
        onOpenAddNewRoundForm: function () {
            $('#modal').modal('show');
            $.validator.unobtrusive.parse("#add-new-round-form");
        }
    }
}(jQuery));

$(document).ready(function () {
    $("#keno-form").submit();
});