class GameStatus {
    checkAll() {
        var checked = $("#status-all").prop("checked"),
            container = $("#status-tbl");

        container.find('.game-status-chk').prop({
            checked: checked
        });
    }

    enable() {       
        var checkedGames = this.getCheckedGames();

        if (checkedGames.message.length > 0) {
            var message = "ENABLE games:\n" + checkedGames.message + "Are you sure?";
        }
     
        if (confirm(message)) {
            $.ajax({
                method: "POST",
                url: "/status/enable",
                data: { games: checkedGames.games}
            }).done(function (updated) {
                if (updated) {
                    window.location.href = "/status";
                }
            });
        }
    }

    disable()
    {
        var checkedGames = this.getCheckedGames();

        if (checkedGames.message.length > 0) {
            var message = "DISABLE games:\n" + checkedGames.message + "Are you sure?";
        }

        if (confirm(message)) {
            $.ajax({
                method: "POST",
                url: "/status/disable",
                data: { games: checkedGames.games }
            }).done(function (updated) {
                if (updated) {                   
                    window.location.href = "/status";
                }
            });
        }
    }

    getCheckedGames() {
        var container = $("#status-tbl");
        var checkboxes = container.find('.game-status-chk');
        var gameChosen = "";
        var games = [];

        for (var i = 0; i < checkboxes.length; i++) {
            if ($(checkboxes[i]).is(":checked")) {
                games.push($(checkboxes[i]).val());

                var parent = $(checkboxes[i]).parent();
                var displayName = parent.find(".game-display-name");

                gameChosen += "- " + displayName.val() + "\n";
            }
        }

        return {
            games: games,
            message: gameChosen
        };
    }

    showForm(element) {
        var div = $(element).parent();
        var form = $(div).parent().find('.form-disabled-message');
        form.show();
        div.hide();
    }
}

var gameStatus = new GameStatus();
