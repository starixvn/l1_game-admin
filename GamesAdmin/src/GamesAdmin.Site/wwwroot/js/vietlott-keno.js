var vietlottKeno = (function ($) {
    return {
        init: function () {
            $('#vietlott-keno-table').DataTable({
                "ordering": false,
                "lengthMenu": [100],
            });
        }
    }
}(jQuery));

$(document).ready(function () {
    $("#keno-form").submit();
});