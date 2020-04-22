class GameSettings {
    updateBotRatio(element, index, totalBots) {
        var id = "#bot_number_" + index;
        var currentBot = parseInt(per * totalBots / 100);
        var per = parseInt($(element).val());
        var currentBot = parseInt(per * totalBots / 100);
        var text = "(" + currentBot + " bot(s))";
        $(id).text(text);
    }
}

let gameSettings = new GameSettings();
