var promotionLeaderboard = (function ($) {
    const leaderboardElements = {
        form: "#leaderboard-form",
        viewWinningPlayerForm: "#view-winning-player-form",
        resultLabel: ".result-label",
        viewWinningPlayerModal: "#award-modal",
        table: "#leaderboard-table",
        btnConfirmPayment: "#btn-confirm-payment",
        awardTournamentModal: "#award-tournament-modal",
        awardTournamentTable: "#award-tournament-table",
        modalBody: ".modal-body",
        rowTestAwardAmount: "#row-test-award-amount",
        awardAmount: "#input-test-award-amount",
        playerId: "#hidden-test-player-id",
        operatorId: "#hidden-test-operator-id",
    };
    const messages = {
        awardedSuccess: "Award Successfully!",
        error: "Error!!!"
    }

    function openModal(formId) {
        return function () {
            $(formId).modal('show');
            $(formId).find(leaderboardElements.resultLabel).text('');
        }
    }

    function openAwardTournamentModal(promotionId) {
        $.ajax({
            method: "GET",
            url: "/Promotion/GetTournamentWinningPlayers",
            data: { promotionId }
        }).done(function (content) {
            if (content) {
                $(leaderboardElements.awardTournamentModal).find(leaderboardElements.modalBody).html(content);
                openModal(leaderboardElements.awardTournamentModal)();
            }
        });
    }

    function openAwardTestPlayer(operatorId, operatorName, playerId, playerName, currency, point, awardedAmount) {
        $.ajax({
            method: "GET",
            url: "/Promotion/ViewAwardTestCurrency",
            data: { operatorId, operatorName, playerId, playerName, currency, point, awardedAmount }
        }).done(function (content) {
            if (content) {
                $(leaderboardElements.awardTournamentModal).find(leaderboardElements.modalBody).html(content);
                openModal(leaderboardElements.awardTournamentModal)();
            }
        });
    }

    function onUpsert(modalId, data) {
        if (data.success) {
            setTimeout(function () {
                $('.modal-buttons .btn-add').prop('disabled', false);
                $(leaderboardElements.form).submit();
                $(modalId).modal('hide');
            }, 1500);
        }
        showMessage(modalId, data);
    }

    function showMessage(modalId, data) {
        let label = $(modalId).find(leaderboardElements.resultLabel);
        label.removeClass('text-success');
        label.removeClass('text-danger');
        label.addClass(`text-${data.success ? "success" : "danger"}`);
        label.text(data.message);
    }

    function awardTournament(promotionId) {
        if ($(promotionLeaderboard.rowTestAwardAmount)) {
            awardTournamentTestCurrency(promotionId);
        } else {
            $.ajax({
                method: "POST",
                url: "/Promotion/AwardTournament",
                data: { promotionId }
            }).done(function (res) {
                const message = res ? messages.awardedSuccess : messages.error;
                onUpsert(leaderboardElements.awardTournamentModal, { success: res, message });
            }).fail(function () {
                showMessage(leaderboardElements.awardTournamentModal, { success: false, message: messages.error });
            });
        }
    }

    function awardTournamentTestCurrency(promotionId) {
        playerId = $(leaderboardElements.awardTournamentModal).find(leaderboardElements.playerId).val();
        awardAmount = $(leaderboardElements.awardTournamentModal).find(leaderboardElements.awardAmount).val();
        operatorId = $(leaderboardElements.awardTournamentModal).find(leaderboardElements.operatorId).val();
        $.ajax({
            method: "POST",
            url: "/Promotion/AwardTestCurrency",
            data: { promotionId, playerId, awardAmount, operatorId }
        }).done(function (res) {
            const message = res ? messages.awardedSuccess : messages.error;
            onUpsert(leaderboardElements.awardTournamentModal, { success: res, message });
        }).fail(function () {
            showMessage(leaderboardElements.awardTournamentModal, { success: false, message: messages.error });
        });
    }

    return {
        init: () => {
            $(leaderboardElements.table).DataTable({
                "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
                "ordering": false,
                "info": false,
                "searching": false
            });
        },
        openAwardTournamentModal,
        openAwardTestPlayer,
        awardTournament,
        awardTournamentTestCurrency,
    }
}(jQuery));

$(document).ready(function () {
    $("#leaderboard-form").submit();
})