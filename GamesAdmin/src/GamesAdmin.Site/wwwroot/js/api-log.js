var apiLog = (function ($) {
    return {
        init: function () {
            $('#api-log').removeAttr('hidden');
            $('#api-log-table').DataTable({
                lengthMenu: [100, 200, 500]
            });
        }
    }
})(jQuery);

$(document).ready(function () {
    $('#api-log-form').submit(function () {
        var logType = $('#LogType').val() >> 0;
        var gameType = $('#GameTypeId').val() >> 0;
        var gameRound = $('#GameRoundId').val() >> 0;
        var memberId = $('#ObCustId').val() >> 0;
        var username = $('#Username').val();
        var $errorMessage = $("#error-message");
        var isInvalid = false;

        switch (logType) {
            case 1:
            case 2:
                isInvalid = (!gameType || !gameRound || !memberId);
                $errorMessage.val("Please input Customer ID, Game and Game Round");
            case 5:
            case 9:
                isInvalid = !memberId || !username;
                $errorMessage.val("Please input Customer ID or Username");
            case 8:
                isInvalid = !username;
                $errorMessage.val("Please input Username");
            default:
        }

        if (isInvalid) {
            $('#error-message').removeAttr('hidden');
        } else {
            $('#error-message').attr('hidden', 'hidden');
        }

        return !isInvalid;
    });
})