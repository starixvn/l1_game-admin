class GameSettings {
    setPercentageSelectEvent() {
        $('#MaxBot').change(function () {
            var per = parseInt($(this).val());
            var totalBots = parseInt($('#BotCount').text());
            var currentBot = parseInt(per * totalBots / 100);

            $('#CurrentBot').text(currentBot);
        });
    }
}

let gameSettings = new GameSettings();
gameSettings.setPercentageSelectEvent();
