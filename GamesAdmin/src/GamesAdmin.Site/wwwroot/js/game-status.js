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
        var enable = $(element).is(":checked");

        $.ajax({
            method: "POST",
            url: "/status/update",
            data: { name: name, enable: enable }
        })
        .done(function (msg) {
            alert("Data Saved: " + msg);
        });
    }
}

var gameStatus = new GameStatus();
