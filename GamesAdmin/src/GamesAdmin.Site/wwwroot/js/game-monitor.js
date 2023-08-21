var gameMonitor = (function ($) {
    const modal = $('#check-service-modal');
    const textGameId = $('#gameIdHidden');
    const roundIdText = $('#roundId');
    const loadingIcon = $("#loading-check-service");
    return {
        openCheckServiceStatus: function (id) {
            modal.modal('show');
            textGameId.val(id);
            $('#checkSuccess, #checkFail').hide();
            roundIdText.text("");
        },
        checkServiceStatus: function () {
            const username = $('#username').val();
            const password = $('#password').val();
            const name = textGameId.val();

            loadingIcon.show();
            $('#checkSuccess, #checkFail').hide();
            roundIdText.text("");
            $.ajax({
                method: "POST",
                url: "/serviceStatus/check",
                data: { username, password, name }
            }).done(function (data) {
                loadingIcon.hide();
                const { error, roundId } = data;
                const isError = error !== "";
                if (roundId) {
                    roundIdText.text('RoundId: ' + roundId);
                }

                if (isError) {
                    $('#checkFail').show();
                    $('#roundId').text(error);
                } else {
                    $('#checkSuccess').show();
                }
            })
        },
    }
}(jQuery));
