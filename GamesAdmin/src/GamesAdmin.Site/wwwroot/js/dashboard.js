(function ($) {
    //$('#onlineUserTable').DataTable();
    var $onlineUsers = $("#online-users");
    var $todayTotalBets  = $("#today-total-bets");

    setInterval(function () {
        $.get("/dashboard/onlineusers", function (data) {
            $onlineUsers.text(data);
        });

        $.get("/dashboard/todaytotalbets", function (data) {
            $todayTotalBets.text(data);
        });
    }, 60000)
}(jQuery));