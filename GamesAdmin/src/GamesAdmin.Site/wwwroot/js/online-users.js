var onlineUsers = (function ($) {
    return {
        init: function () {
            $('#online-users-table').DataTable({
                "order": [[1, "asc"]]
            });
        }
    }
}(jQuery));

$(document).ready(function () {
    onlineUsers.init();
});