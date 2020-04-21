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
        var container = $("#status-tbl");
        var checkboxes = container.find('.game-status-chk');
        var gameChosen = "";
        var games = [];

        for (var i = 0; i < checkboxes.length; i++) {
            if ($(checkboxes[i]).is(":checked"))
            {
                games.push($(checkboxes[i]).val());

                var parent = $(checkboxes[i]).parent().parent();
                var displayName = parent.find(".game-display-name");

                gameChosen += "- " + displayName.val() + "\n";
            }
        }

        if (gameChosen.length > 0) {
            var message = "ENABLE games:\n" + gameChosen + "Are you sure?";
        }
     
        if (confirm(message)) {
            $.ajax({
                method: "POST",
                url: "/status/enable",
                data: { games: games}
            }).done(function (updated) {
                if (!updated) {
                    chk.prop({
                        checked: !enabled
                    });
                } else {
                    window.location.href = "/status";
                }
            });
        }
    }

    disable()
    {
        var enable = confirm("Disable all?");

        if (enable) {
            alert("Disable all");
        }
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
