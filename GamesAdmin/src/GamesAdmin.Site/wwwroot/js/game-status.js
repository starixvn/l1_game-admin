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

        var action = enabled ? "ENABLE" : "DISABLE";

        if (confirm(action + " game " + name + "?")) {
            $.ajax({
                method: "POST",
                url: "/status/update",
                data: { name: name, enabled: enabled }
            }).done(function (updated) {
                if (updated)
                {
                    alert("Updated!");
                } else
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
