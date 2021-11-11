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
        var isInvalid = logType <= 2 && (!gameType || !gameRound || !memberId);

        if (isInvalid) {
            $('#error-message').removeAttr('hidden');
        } else {
            $('#error-message').attr('hidden', 'hidden');
        }

        return !isInvalid;
    });
})