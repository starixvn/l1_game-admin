class GameStatus {
    checkAll() {
        var checked = $("#status-all").prop("checked"),
            container = $("#status-tbl");

        container.find('.game-status-chk').prop({
            checked: checked
        });
    }

    updateAllMessage(element) {
        var text = $(element).val();


        $('.disabled-message').val(text);
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

                var parent = $(checkboxes[i]).parent().parent();
                var displayName = parent.find(".game-display-name");

                gameChosen += "- " + displayName.val() + "\n";
            }
        }

        return {
            games: games,
            message: gameChosen
        };
    }

    toggle(element) {
        var chk = $(element);
        var name = chk.val();
        var enabled = $(element).is(":checked");

        var parent = chk.parent();
        var displayName = parent.find(".game-display-name");

        var action = enabled ? "ENABLE" : "DISABLE";

        if (confirm("Are you sure you want to " + action + " game " + displayName.val() + "?")) {
            $.ajax({
                method: "POST",
                url: "/status/update",
                data: { name: name, enabled: enabled }
            }).done(function (updated) {
                if (!updated)
                {
                    chk.prop({
                        checked: !enabled
                    });
                }                
            });
        } else {
            chk.prop({
                checked: !enabled
            });
        }
    }
}

var gameStatus = new GameStatus();
