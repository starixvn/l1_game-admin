var promotionLeaderboard = (function ($) {
    const leaderboardElements = {
        viewWinningPlayerForm: "#view-winning-player-form",
        resultLabel: "#resultLabel",
        viewWinningPlayerModal: "#award-modal",
        table: "#leaderboard-table",
        btnConfirmPayment: "#btn-confirm-payment",
        awardTournamentModal: "#award-tournament-modal",
        awardTournamentTable: "#award-tournament-table",
        modalBody: ".modal-body"
    };

    function openModal(formId) {
        return function () {
            $(formId).modal('show');
            $(formId).find(leaderboardElements.resultLabel).text('');
        }
    }

    function openAwardTournamentModal() {
        $(leaderboardElements.awardTournamentModal).find(leaderboardElements.modalBody).html($(leaderboardElements.awardTournamentTable).html());
        openModal(leaderboardElements.awardTournamentModal)();
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
        openAwardPlayerModal: openModal(leaderboardElements.viewWinningPlayerForm),
        onAwardSuccess: () => { console.log("onAwardSuccess"); },
        onAwardFailed: () => { console.log("onAwardFailed"); }
    }
}(jQuery));

$(document).ready(function () {
    $("#leaderboard-form").submit();
})