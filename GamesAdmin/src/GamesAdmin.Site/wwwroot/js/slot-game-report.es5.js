"use strict";

var slotGameReportController = (function ($) {
    return {
        init: function init() {
            $('#tbl-slot-game').DataTable({
                "lengthMenu": [[100, 200, 500], [100, 200, 500]],
                "pageLength": 100
            });
            $("#card-container").removeClass("display-none");
        }
    };
})(jQuery);

