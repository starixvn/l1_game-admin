var exchangeRate = (function ($) {
    return {
        init: function () {
            $('#exchange-rate-table').DataTable({
                lengthMenu: [100, 200, 500]
            });
        }
    }
})(jQuery);

$(document).ready(function () {
    $("#exchange-rate-form").submit();
});