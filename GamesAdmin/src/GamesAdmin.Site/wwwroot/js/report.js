class Report {
    showBetHistory(game, roundId, nickname) {
        $.ajax({
            method: "GET",
            url: "/Report/KenoBetHistory/?game=" + game + "&roundId=" + roundId + "&nickname=" + nickname
        }).done(function (content) {
            if (content) {
                $('#kenoBetHistory').html(content);
                $('#reportModal').modal('show');
            }
        });
    }

}

var report = new Report();