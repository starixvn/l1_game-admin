class GameSettings {
    updateBotRatio(element, index, totalBots) {
        var id = "#bot_number_" + index;
        var currentBot = parseInt(per * totalBots / 100);
        var per = parseInt($(element).val());
        var currentBot = parseInt(per * totalBots / 100);
        var text = "(" + currentBot + " bot(s))";
        $(id).text(text);
    }
    startMonitor(gameName) {
        var fetchOptions = {
            method: 'POST',
            body: JSON.stringify({ userAgent: navigator.userAgent }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            },
        };

        fetch('/jwt', fetchOptions)
            .then(res => res.json())
            .then(data => {
                window.open(`/game-admin/monitor.html?game=${gameName}`, "_blank", "width=1020,height=765");
            });
    }
}

let gameSettings = new GameSettings();
