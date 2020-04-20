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
        var enable = confirm("Enable all?");

        if (enable) {
            alert("Enabled all");
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
