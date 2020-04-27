var dashBoard = (function ($) {
    //$('#onlineUserTable').DataTable();
    var $onlineUsers = $("#online-users");
    var $todayTotalBets = $("#today-total-bets");

    var getOnlineUsers = function () {
        $.get("/dashboard/onlineusers", function (data) {
            $onlineUsers.text(data);
        });
    }

    var getTodayTotalBet = function () {
        $.get("/dashboard/todaytotalbets", function (data) {
            $todayTotalBets.text(data);
        });
    }

    return {
        init: function () {
            getOnlineUsers();
            getTodayTotalBet();


            setInterval(function () {
                getOnlineUsers();
                getTodayTotalBet();
            }, 60000)
        }
    }
}(jQuery));

$(document).ready(function () {
    dashBoard.init();
});