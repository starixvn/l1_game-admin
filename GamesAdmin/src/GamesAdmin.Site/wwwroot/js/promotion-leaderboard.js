var promotionLeaderboard = (function ($) {
    let leaderboardElements = {
        viewWinningPlayerForm: "#view-winning-player-form",
        resultLabel: "#resultLabel",
        viewWinningPlayerModal: "#award-modal",
    };


    function openModal(formId) {
        return function () {
            $(leaderboardElements.viewWinningPlayerModal).modal('show');
            $(formId).find(leaderboardElements.resultLabel).text('');
            $.validator.unobtrusive.parse(formId);
        }
    }

    return {
        openAwardPlayerModal: openModal(leaderboardElements.viewWinningPlayerForm),
        onAwardSuccess: () => { console.log("onAwardSuccess"); },
        onAwardFailed: () => { console.log("onAwardFailed"); } 
    }
}(jQuery));
