var bitcoinMasterReport = (function ($) {
    var showUpDownTable = function () {
        $(".table-oddeven").addClass("display-none");
        $("#spn-bet-choice-oddeven").removeClass("selected");

        $(".table-updown").removeClass("display-none");
        $("#spn-bet-choice-updown").addClass("selected");

    };
    var showOddEvenTable = function () {
        $(".table-updown").addClass("display-none");
        $("#spn-bet-choice-updown").removeClass("selected");

        $(".table-oddeven").removeClass("display-none");
        $("#spn-bet-choice-oddeven").addClass("selected");
    };


    return {
        showUpDownTable: showUpDownTable,
        showOddEvenTable: showOddEvenTable,
    }
}(jQuery));
