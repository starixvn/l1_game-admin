var onlineUsers = (function ($) {
    return {
        init: function () {
            $('#online-users-table').DataTable();
        }
    }
}(jQuery));

$(document).ready(function () {
    onlineUsers.init();
});