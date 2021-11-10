var apiLog = (function ($) {
    return {
        init: function () {
            $('#api-log-table').DataTable({
                lengthMenu: [100, 200, 500]
            });
        }
    }
})(jQuery);