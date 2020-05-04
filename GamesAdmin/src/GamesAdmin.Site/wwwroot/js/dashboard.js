var dashBoard = (function ($) {
    //$('#onlineUserTable').DataTable();
    var $onlineUsers = $("#online-users");

    var getOnlineUsers = function () {
        $.get("/dashboard/realonlineusers", function (data) {
            $onlineUsers.text(data);
        });
    }

    return {
        init: function () {
            getOnlineUsers();

            setInterval(function () {
                getOnlineUsers();
            }, 60000)
        }
    }
}(jQuery));

$(document).ready(function () {
    dashBoard.init();
});