(function ($) {
    $('#onlineUserTable').DataTable();
    var $onlineUsers = $("#online-users");

    setInterval(function () {
        $.get("/dashboard/onlineusers", function (data) {
            $onlineUsers.text(data);
        });
    }, 60000)
}(jQuery));