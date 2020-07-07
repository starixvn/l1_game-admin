class GameStatus {
    gameName = ''

    checkAll() {
        var checked = $("#status-all").prop("checked"),
            container = $("#status-tbl");

        container.find('.game-status-chk').prop({
            checked: checked
        });
    }

    uncheckAll(element) {
        if (!$(element).is(":checked")) {
            $("#status-all").prop({
                checked: false
            });
        }
    }

    enable() {
        var checkedGames = this.getCheckedGames();

        if (checkedGames.message.length > 0) {
            var message = "ENABLE games:\n" + checkedGames.message + "Are you sure?";

            var reload = $("#reload").is(":checked");

            if (confirm(message)) {
                $.ajax({
                    method: "POST",
                    url: "/status/enable",
                    data: { games: checkedGames.games, reload: reload }
                }).done(function (updated) {
                    if (updated) {
                        window.location.href = "/status";
                    }
                });
            }
        } else {
            alert("You should choose at least one game.");
        }
    }

    disable() {
        var checkedGames = this.getCheckedGames();

        if (checkedGames.message.length > 0) {
            var message = "DISABLE games:\n" + checkedGames.message + "Are you sure?";

            var reload = $("#reload").is(":checked");

            if (confirm(message)) {
                $.ajax({
                    method: "POST",
                    url: "/status/disable",
                    data: { games: checkedGames.games, reload: reload }
                }).done(function (updated) {
                    if (updated) {
                        window.location.href = "/status";
                    }
                });
            }
        } else {
            alert("You should choose at least one game.");
        }
    }

    reload() {
        if (confirm("Are you sure you want all users to RELOAD?")) {
            $.ajax({
                method: "POST",
                url: "/status/reload"
            }).done(function (updated) {
                if (updated) {
                    alert("Users reloaded!");
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

                gameChosen += "<li> " + displayName.val() + "</li>";
            }
        }

        return {
            games: games,
            message: gameChosen
        };
    }

    showGameForm(gameName, gameDisplayName) {
        $.ajax({
            method: "GET",
            url: "/gamedetails/" + gameName
        }).done(function (content) {
            if (content) {
                $('#gamedetails').html(content);
                $('#gameTitleName').html(gameDisplayName);
                $('#gameModal').modal('show');
            }
        });
    }

    updateBotCount(element) {
        const wrapper = $('#botRatioModal');
        var per = parseInt($(element).parent().find('.ratio').val());
        var totalBots = parseInt(wrapper.find('#total_bots_field').val());
        var currentBot = parseInt(per * totalBots / 100);

        $(element).parent().find('.bot-number').html(`${currentBot} bot(s)`);
    }

    showBotRatioModal(gameName) {
        const wrapper = $('#botRatioModal');

        this.gameName = gameName;
        wrapper.find('#failedMsg').hide();
        wrapper.find('#successMsg').hide();

        $.ajax({
            method: "GET",
            url: "/botratio/" + gameName
        }).done(function (result) {
            const botRatioItems = result.botRatioItems;

            for (let i = 0; i < 24; i++) {
                const setting = wrapper.find(`#bot_setting_${i}`);

                wrapper.find('#total_bots').html(result.botCount);
                wrapper.find('#total_bots_field').val(result.botCount);
                wrapper.find('#modal_title').html(`<b>${result.displayName}</b> - Adjust bots by hour`);

                setting.find('.time-gtm4').html(botRatioItems[i].greenwichHour);
                setting.find('.time-gtm7').html(botRatioItems[i].asiaPacificHour);
                setting.find('.ratio').val(botRatioItems[i].botRatio);
                setting.find('.utc-hour').val(botRatioItems[i].utcHour);
                setting.find('.bot-number').html(`${botRatioItems[i].currentBot} bot(s)`);
                setting.find('#validate_ratio').html('');
            }

            $('#botRatioModal').modal('show');
        });
    }

    updateBotRationItems() {
        const modal = $('#botRatioModal');
        const btnSubmit = modal.find('#updateBotRatioBtn');
        const data = [];
        let isValid = true;

        btnSubmit.prop('disabled', true);

        for (let i = 0; i < 24; i++) {
            const setting = modal.find(`#bot_setting_${i}`);
            const ratio = setting.find('.ratio').val();

            if (ratio < 0 || ratio > 100) {
                isValid = false;
                btnSubmit.prop('disabled', false);

                setting.find('#validate_ratio').html('Number range [0..100]');
            }
            else {
                setting.find('#validate_ratio').html('');
            }

            data.push({
                BotRatio: ratio,
                UtcHour: setting.find('.utc-hour').val()
            });
        }

        isValid && $.ajax({
            method: "POST",
            url: this.gameName,
            data: { BotRatioItems: data }
        }).done(function (result) {
            if (result) {
                modal.find('#successMsg').show();
            } else {
                modal.find('#failedMsg').show();
            }

            setTimeout(function () {
                btnSubmit.prop('disabled', false);
                modal.modal('hide');
            }, 1000);
        });
    }

    startMonitor(gameName) {
        var fetchOptions = {
            method: 'POST',
            body: JSON.stringify({ userAgent: navigator.userAgent }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            },
        };

        var newWindow = window.open(`/game-admin/monitor.html?game=${gameName}&v=${new Date().getTime()}`, "_blank", "width=1020,height=765");

        fetch('/jwt', fetchOptions)
            .then(res => res.json())
            .then(data => {
                newWindow.location;
            });
    }
    submit() {
        $('#updateBtn').prop('disabled', true);
        $.ajax({
            method: "POST",
            url: "/update",
            dataType: 'json',
            data: $('#gameForm').serialize(),
        }).done(function (result) {
            if (result) {
                $('#successMsg').show();
            }
            else {
                $('#failedMsg').show();
            }

            setTimeout(function () {
                $('#updateBtn').prop('disabled', false);
                $('#gameModal').modal('hide');
                location.reload();
            }, 1000);
        });
    }

    updateMessage() {
        var message = $('#textMessage').val();
        var applyAll = $('#applyAllCheckbox').is(":checked");
        var name = $('#msgGameName').val();
        var url = applyAll ? "/status/update" : "/status";
        var data = { name: name, disabledMessage: message };
        $('#updateMsgBtn').prop('disabled', true);

        $.ajax({
            method: "POST",
            url: url,
            dataType: 'json',
            data: data,
        }).done(function (result) {
            if (result) {
                $('#updateMsgSuccess').show();
            }
            else {
                $('#updateMsgFailed').show();
            }

            setTimeout(function () {
                $('#updateMsgBtn').prop('disabled', false);
                $('#updateMsgSuccess').hide();
                $('#updateMsgFailed').hide();
                $('#messageModal').modal('hide');
                location.reload();
            }, 1000);
        });
    }

    showUpdateMessage(textMsg, name, displayName) {
        $('#textMessage').val(textMsg);
        $('#msgGameName').val(name);
        $('#updateMsgGameName').html(displayName);

        $('#messageModal').modal('show');
    }

    submitReload() {
        const modal = $('#reloadModal');

        modal.find('#confirmBtn').prop('disabled', true);
        $.ajax({
            method: "POST",
            url: "/status/reload"
        }).done(function (updated) {
            if (updated) {
                modal.find('#updateMsgSuccess').show();
            }
            else {
                modal.find('#updateMsgFailed').show();
            }

            setTimeout(function () {
                modal.find('#confirmBtn').prop('disabled', false);
                modal.find('#updateMsgSuccess').hide();
                modal.find('#updateMsgFailed').hide();
                modal.modal('hide');
            }, 1000);
        });
    }

    showReload() {
        const modal = $('#reloadModal');
        modal.find('#reloadContent').html("Are you sure you want all users to RELOAD?");
        modal.modal('show');
    }

    showEnableDisable(enabled) {
        const modal = $('#enableDisableModal');
        const label = enabled ? "enable" : "disable";

        var checkedGames = this.getCheckedGames();
        if (checkedGames.message.length > 0) {
            var message = `Are you sure you want to ${label.toLocaleUpperCase()} the following games<br /><ul>` + checkedGames.message + "</ul>";
            modal.find('#enableDisableContent').html(message);
            modal.find('#forceReloadLabel').html(`Force users to reload after ${label} game(s)`);
            modal.find('#enableDisableFlag').val(enabled);
            modal.modal('show');
        } else {
            this.showAlert("You should choose at least one game.");
        }
    }

    submitEnableDisable() {
        const modal = $('#enableDisableModal');
        const enabled = modal.find('#enableDisableFlag').val();
        const label = enabled === "true" ? "enable" : "disable";

        var checkedGames = this.getCheckedGames();
        var isReload = modal.find('#forceReloadCheckbox').is(":checked")
        modal.find('#confirmBtn').prop('disabled', true);
        $.ajax({
            method: "POST",
            url: `/status/${label}`,
            data: { games: checkedGames.games, reload: isReload }
        }).done(function (updated) {
            if (updated) {
                modal.find('#updateMsgSuccess').show();
            }
            else {
                modal.find('#updateMsgFailed').show();
            }

            setTimeout(function () {
                modal.find('#confirmBtn').prop('disabled', false);
                modal.find('#updateMsgSuccess').hide();
                modal.find('#updateMsgFailed').hide();
                modal.modal('hide');
                location.reload();
            }, 1000);
        });
    }

    showAlert(message) {
        const modal = $('#alertModal');
        modal.find('#alertContent').html(message);
        modal.modal('show');
    }
}

var gameStatus = new GameStatus();

$(document).ready(function () {
    $("#status-tbl").DataTable({
        "paging": false,
        "ordering": false
    });

    $("#status-tbl").on("click touchstart", "a.icon-action", function (event) {
        var $this = $(this);
        $this.toggleClass("text-danger");
        const gameName = $this.data("game-name");
        gameStatus.startMonitor(gameName);
    });

})