class GameStatus {
    checkAll() {
        var checked = $("#status-all").prop("checked"),
            container = $("#status-tbl");

        container.find('.game-status-chk').prop({
            checked: checked
        });
    }
}

var gameStatus = new GameStatus();
