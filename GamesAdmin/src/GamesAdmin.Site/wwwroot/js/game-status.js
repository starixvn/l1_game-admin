var gameStatus = (function ($) {
    var currentGameName = '';
    var myChart = null;

    return {
        checkAll: function () {
            var checked = $("#status-all").prop("checked"),
                container = $("#status-tbl");

            container.find('.game-status-chk').prop({
                checked: checked
            });
        },

        updateCheckAllStatus: function (element) {
            var container = $("#status-tbl-body");
            var uncheckItems = container.find("input:checkbox:not(:checked)");

            $("#status-all").prop({
                checked: !uncheckItems.length
            });
        },

        enable: function () {
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
        },

        disable: function () {
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
        },

        reload: function () {
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
        },

        getCheckedGames: function () {
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
        },

        showGameForm: function (gameName, gameDisplayName, view) {
            var settingTitle = "";
            switch (view) {
                case "OddSettings":
                    settingTitle = "Odds";
                    break;
                case "MarginSettings":
                    settingTitle = "Margin Setting";
                    break;
                default:
                    settingTitle = "General Setting";
            }

            $.ajax({
                method: "GET",
                url: "/gamedetails/" + gameName + "?view=" + view
            }).done(function (content) {
                if (content) {
                    $('#gamedetails').html(content);
                    $('#settingTitle').html(`${settingTitle}: ${gameDisplayName}`);
                    $('#gameModal').modal('show');
                }
            });
        },

        showPartnerSetting: function (partnerId, partnerName, gameName, displayName, gameType) {
            $.ajax({
                method: "GET",
                url: "/CurrencyGroupGame/Index",
                data: {
                    partnerId,
                    partnerName,
                    gameName,
                    gameType
                }
            }).done(function (content) {
                if (content) {
                    $('#partnerSettingContent').html(content);
                    $('#gameName').html(displayName);
                    $('#partnerSettingModal').modal('show');
                    currencyGroupGame.loadDefaultSetting();
                }
            });
        },

        updateBotCount: function (element) {
            const wrapper = $('#botRatioModal');
            var per = parseInt($(element).parent().find('.ratio').val());
            var totalBots = parseInt(wrapper.find('#total_bots_field').val());
            var currentBot = parseInt(per * totalBots / 100);

            $(element).parent().find('.bot-number').html(`${currentBot} bot(s)`);
        },

        showBotRatioModal: function (gameName) {
            const wrapper = $('#botRatioModal');

            currentGameName = gameName;
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
        },

        updateBotRationItems: function () {
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
                url: currentGameName,
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
        },

        startMonitor: function (gameName) {
            var fetchOptions = {
                method: 'POST',
                body: JSON.stringify({ userAgent: navigator.userAgent, game: gameName }),
                headers: {
                    'Content-type': 'application/json; charset=UTF-8'
                },
            };

            fetch('/jwt', fetchOptions)
                .then(res => res.json())
                .then(data => {
                    if (data) {
                        window.open(adminMonitoringUrl + "/ResultMonitor/Index" + "?gameName=" + data.gameName + "&token=" + data.token, "_blank", "width=1020,height=765");
                        //window.open(adminMonitoringUrl + data.redirectUrl + "&token=" + data.token, "_blank", "width=1020,height=765");
                    }
                });
        },

        setControlResultHoldemPoker: function (event, gameName) {
            event.preventDefault();
            this.setResultControl("HoldemPokderResult", gameName);
        },

        setControlResultSpeedUpGame: function (event, gameType, gameName, displayName) {
            event.preventDefault();
            const actionName = this.getActionNameByGameType(gameType);
            this.setSlotResultControl(actionName, gameName, displayName);
        },

        getActionNameByGameType: function (gameType) {
            switch (gameType) {
                case "Slots": return "SlotResult";
                case "CascadingSlots": return "CascadingSlotResult";
                case "Crash": return "CrashGameResult";
            }
        },

        setControlResultHiLo: function (event) {
            event.preventDefault();
            this.setResultControl("HiLoResult");
        },
        setControlResultPontoon: function (event) {
            event.preventDefault();
            this.setResultControl("PontoonResult");
        },

        setControlResultChineseBlackjack: function (event) {
            event.preventDefault();
            this.setResultControl("ChineseBlackjackResult");
        },

        setSlotResultControl: function (action, gameName, displayName) {
            window.open(adminMonitoringUrl + "/ResultMonitor/" + action + "?gameConfigName=" + gameName + "&displayName=" + displayName + "&token=" + jwtAdminUserToken, "_blank", "width=1020,height=765");
        },

        setResultControl: function (action, gameName) {
            window.open(adminMonitoringUrl + "/ResultMonitor/" + action + "?gameConfigName=" + gameName + "&token=" + jwtAdminUserToken, "_blank", "width=1020,height=765");
        },

        submit: function () {
            $('#updateBtn').prop('disabled', true);
            if ($('#oddSetting').css('display') && $('#oddSetting').css('display') !== 'none') {
                this.updateOdds();
                return;
            }

            if ($('#marginSettings').css('display') && $('#marginSettings').css('display') !== 'none') {
                this.updateMarginSettings();
                return;
            }

            let requestUrl = "/update";
            $('.txt-bet-level').attr('disabled', null);

            if ($('#botsetting').css('display') && $('#botsetting').css('display') !== 'none') {
                requestUrl = "/betlimit/update";
            }

            if ($('#jackpots').css('display') && $('#jackpots').css('display') !== 'none') {
                requestUrl = "/jackpot/update";
            }

            if ($('#adjustweight').css('display') && $('#adjustweight').css('display') !== 'none') {
                this.updateSymbolWeight();
                return;
            }

            $.ajax({
                method: "POST",
                url: requestUrl,
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
                    $('#successMsg').hide();
                    $('#failedMsg').hide();
                    $('#gameModal').modal('hide');
                    location.reload();
                }, 1000);
            });
        },
        updateSymbolWeight: function () {
            const weights = this.getSymbolWeight();
            const validate = this.validationSymbolWeights(weights);
            if (!validate) {
                setTimeout(function () {
                    $('#checkEmptySymbolWeights').hide();
                    $('#invalidSymbolWeight').hide();
                    $('#notEnoughSymbolWeight').hide();
                    $('#updateBtn').prop('disabled', false);
                }, 1000);
                return;
            }
            const gameName = $('#slotGameName').val();
            const mode = $('#modeItem').val();
            $.ajax({
                method: "POST",
                url: "/symbolweight/update",
                data: {
                    gameName,
                    mode,
                    weights
                },
            }).done(function (result) {
                if (result) {
                    $('#successMsg').show();
                }
                else {
                    $('#failedMsg').show();
                }

                setTimeout(function () {
                    $('#updateBtn').prop('disabled', false);
                    $('#successMsg').hide();
                    $('#failedMsg').hide();
                    $('#gameModal').modal('hide');
                    location.reload();
                }, 1000);
            });
        },

        validationSymbolWeights: function (lines) {
            $('#invalidSymbolWeight').hide();
            $('#notEnoughSymbolWeight').hide();
            $('#checkEmptySymbolWeights').hide();

            for (const line in lines) {
                const symbols = lines[line];
                if (symbols[0] === "") {
                    if ($('#empty').length > 0) {
                        $('#empty').remove();
                    }
                    $('#checkEmptySymbolWeights').append(`<span id="empty">${+line + 1}</span>`).show();
                    return false;
                }
                if (symbols.length > 11 || symbols.length < 11) {
                    if ($('#notEnough').length > 0) {
                        $('#notEnough').remove();
                    }
                    $('#notEnoughSymbolWeight').append(`<span id="notEnough">${+line + 1}</span>`).show();
                    return false;
                }
                for (let i in symbols) {
                    if (!Number.isInteger(+symbols[i])) {
                        const element = $('#adjustweight-id');
                        if (element.length > 0) {
                            $('#adjustweight-id').remove();
                        }
                        $('#invalidSymbolWeight').append(`<span id="adjustweight-id">${+line + 1}: ${symbols[i]}</span>`).show();
                        return false;
                    }
                }
            }
            return true;
        },

        updateOdds: function () {
            $('#update-odds-msg').hide();

            $.ajax({
                method: "POST",
                url: "/odds/update",
                dataType: 'json',
                data: $('#gameForm').serialize(),
            }).done(function (result) {
                if (result) {
                    $('#update-odds-msg').html(result);
                    $('#update-odds-msg').show();
                    $('#updateBtn').prop('disabled', false);
                } else {
                    $('#successMsg').show();
                    setTimeout(function () {
                        $('#updateBtn').prop('disabled', false);
                        $('#successMsg').hide();
                        $('#update-odds-msg').hide();
                        $('#gameModal').modal('hide');
                        location.reload();
                    }, 1000);
                }
            });
        },

        updateMessage: function () {
            var name = $('#applyAllCheckbox').is(":checked") ? "" : $('#msgGameName').val();
            var data = { name, messages: {} };
            Object.keys(supportedLanguages).forEach(l => {
                data.messages[l] = $(`#message-${l.toLowerCase()}`).val();
            });

            $('#updateMsgBtn').prop('disabled', true);

            $.ajax({
                method: "POST",
                url: "/status",
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
        },

        showUpdateMessage: function (textMsg, name, displayName) {
            $.ajax({
                method: "GET",
                url: "/status/disableMessage?name=" + name
            }).done(function (result) {
                $('#msgGameName').val(name);
                $('#updateMsgGameName').html(displayName);

                Object.keys(supportedLanguages).forEach(c => {
                    $(`#message-${c.toLowerCase()}`).val(result[c]);
                })

                $('#messageModal').modal('show');
            });
        },

        submitReload: function () {
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
        },

        showReload: function () {
            const modal = $('#reloadModal');
            modal.find('#reloadContent').html("Are you sure you want all users to RELOAD?");
            modal.modal('show');
        },

        showEnableDisable: function (enabled) {
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
        },

        submitEnableDisable: function () {
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
        },

        showAlert: function (message) {
            const modal = $('#alertModal');
            modal.find('#alertContent').html(message);
            modal.modal('show');
        },

        toogleJackpot: function () {
            if ($('#slots-config').find('#EnableJackpot').is(":checked")) {
                $('.slots-jackpot').show();
            } else {
                $('.slots-jackpot').hide();
            }
        },

        toogleSlotsConfig: function () {
            const type = $('#Type').val();

            switch (type) {
                case '4':
                    $('#cascading-slots-config .common-field').attr('disabled', true);
                    $('#slots-config .common-field').attr('disabled', true);
                    $('#slot-rng-config .common-field').attr('disabled', null);

                    $('#slot-rng-config').show();
                    $('#rng-game').show();
                    $('#slots-config').hide();
                    $('#cascading-slots-config').hide();
                    break;
                case '3':
                    $('#slots-config .common-field').attr('disabled', true);
                    $('#slot-rng-config .common-field').attr('disabled', true);
                    $('#cascading-slots-config .common-field').attr('disabled', null);

                    $('#slots-config').hide();
                    $('#rng-game').hide();
                    $('#cascading-slots-config').show();
                    break;
                case '2':
                    $('#cascading-slots-config .common-field').attr('disabled', true);
                    $('#slot-rng-config .common-field').attr('disabled', true);
                    $('#slots-config .common-field').attr('disabled', null);

                    $('#rng-game').hide();
                    $('#cascading-slots-config').hide();
                    $('#slots-config').show();
                    break;
                default:
                    $('#slots-config').hide();
                    $('#cascading-slots-config').hide();
                    $('#slot-rng-config').hide();
                    $('#rng-game').show();
            }
        },

        toggleSettingTabs: function (name, element) {
            $('#general').hide();
            $('#jackpots').hide();
            $('#botsetting').hide();
            $('#adjustweight').hide();

            $('#general-tab-header').removeClass("active");
            $('#jackpots-tab-header').removeClass("active");
            $('#botsetting-tab-header').removeClass("active");
            $('#adjustweight-tab-header').removeClass("active");

            const id = '#' + name;
            const headerId = '#' + name + '-tab-header';

            $(id).show();
            $(headerId).addClass("active");

            if (name === "adjustweight") {
                this.showSymbolWeight();
            }
        },

        showSymbolWeight: function () {
            const easyMode = $("#weight-easy-mode").val();
            const weight = JSON.parse(easyMode);
            this.setRows(weight);
        },

        setRows: function (weight) {
            if (weight) {
                $("#row1").val(weight[0]);
                $("#row2").val(weight[1]);
                $("#row3").val(weight[2]);
            } else {
                $("#row1").val("");
                $("#row2").val("");
                $("#row3").val("");
            }
        },

        formatNumber: function (id) {
            var elm = $(`#${id}`);
            var val = elm.val().replace(/,/g, '');

            if (val === '') {
                elm.val(0);
                return;
            }

            var parts = val.split(".");
            parts[0] = parseInt(parts[0]).toLocaleString('en');
            elm.val(parts.join("."));
        },

        toggleAddNewGameTabs: function (tabName) {
            $('#addnew').hide();
            $('#clonegame').hide();

            $('#addnew-tab-header').removeClass("active");
            $('#clonegame-tab-header').removeClass("active");

            const id = '#' + tabName;
            const headerId = '#' + tabName + '-tab-header';

            $(id).show();
            $(headerId).addClass("active");
        },

        toggleMode: function () {
            const mode = $('#modeItem').val();
            const easyMode = document.getElementById('weight-easy-mode').value;
            const mediumMode = document.getElementById('weight-medium-mode').value;
            const hardMode = document.getElementById('weight-hard-mode').value;
            let symbolWeight;

            switch (mode) {
                case "1": {
                    symbolWeight = JSON.parse(easyMode);
                    break;
                }
                case "2": {
                    symbolWeight = JSON.parse(mediumMode);
                    break;
                }
                case "3": {
                    symbolWeight = JSON.parse(hardMode);
                    break;
                }
            }
            this.setRows(symbolWeight);
        },

        checkRTP: function () {
            const weight = this.getSymbolWeight();
            const validate = this.validationSymbolWeights(weight);
            if (!validate) {
                setTimeout(function () {
                    $('#checkEmptySymbolWeights').hide();
                    $('#invalidSymbolWeight').hide();
                    $('#notEnoughSymbolWeight').hide();
                }, 1000);
                return;
            }
            const odds = document.getElementById('symbol-odds').value;
            const numOfRounds = document.getElementById('numberOfRounds').value;
            if (numOfRounds === "") {
                $('#checkEmptyNumOfRounds').show();
                setTimeout(function () {
                    $('#checkEmptyNumOfRounds').hide();
                }, 1000);
                return;
            }
            $("#loading-check-rtp").show();
            $.ajax({
                method: "POST",
                url: `symbolWeight/checkRtp`,
                data: {
                    odds,
                    weight,
                    numOfRounds
                }
            }).done(function (data) {
                $("#loading-check-rtp").hide();
                if (data) {
                    const result = data.result;
                    const { rtp, symbolFrequency } = result[0][0];
                    $("#rtp").text(rtp);
                    var xValues = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
                    var barColors = ["red", "green", "blue", "orange", "brown", "red", "green", "blue", "orange", "brown"];
                    if (myChart === null) {
                        myChart = new Chart("myChart", {
                            type: "bar",
                            data: {
                                labels: xValues,
                                datasets: [{
                                    backgroundColor: barColors,
                                    data: symbolFrequency
                                }]
                            },
                            options: {
                                legend: { display: false },
                                title: {
                                    display: true,
                                    text: "Symbol Frequency"
                                }
                            }
                        });
                    } else {
                        myChart.data.datasets[0].data = symbolFrequency;
                        myChart.update();
                    }
                }
            });
        },

        getSymbolWeight: function () {
            const row1 = document.getElementById('row1').value;
            const row2 = document.getElementById('row2').value;
            const row3 = document.getElementById('row3').value;
            const lines = [];
            lines.push(row1);
            lines.push(row2);
            lines.push(row3);
            return lines.map(line => line.split(','));
        },

        addMarginOddsRange: function (oddsNames) {
            $.ajax({
                method: "GET",
                url: "/margin/create-setting?oddsNames=" + oddsNames
            }).done(function (result) {
                $('#marginSettings').append(result);
                const oddsRanges = $(".odds-range");
                $('.range-order:last').text(`Odds Range ${oddsRanges.length}`);
            });
        },

        updateMarginSettings: function () {
            const idMsg = "#update-margin-msg";

            const oddsRanges = $(".odds-range").toArray();
            const marginSettings = [];
            const gameName = $("#marginForm").find('input[name="Name"]').val();
            oddsRanges.forEach(oddsRange => {
                const setting = {}
                $(oddsRange).find('input.form-control').toArray().forEach(input => {
                    setting[input.name] = +input.value;
                })
                marginSettings.push(setting);
            });

            const showMessage = (message) => {
                $(idMsg).html(message);
                $(idMsg).show();
                $('#updateBtn').prop('disabled', false);
            }

            if (marginSettings.some(setting => setting.max <= 0)) {
                showMessage("Max value must be > 0");
                return;
            } else {
                if (Object.keys(marginSettings).some(key => key !== 'max' && marginSettings[key] >= 1)) {
                    showMessage("margin value must be < 1");
                    return;
                }
                $(idMsg).hide();
            }

            $.ajax({
                method: "POST",
                url: "/margin/update",
                data: { gameName, marginSettings }
            }).done(function (result) {
                if (result) {
                    showMessage(result);
                } else {
                    $('#successMsg').show();
                    setTimeout(function () {
                        $('#updateBtn').prop('disabled', false);
                        $('#successMsg').hide();
                        $(idMsg).hide();
                        $('#gameModal').modal('hide');
                        location.reload();
                    }, 1000);
                }
            });
        }
    }
}(jQuery));

$(document).ready(function () {
    if ($("#status-tbl")) {
        $("#status-tbl").DataTable({
            "ordering": false,
            "paging": false,
            "info": false,
            "stateSave": true,
            "columns": [
                { "width": "3%" },
                { "width": "10%" },
                { "width": "5%" },
                { "width": "10%" },
                { "width": "20%" },
                { "width": "5%" },
                { "width": "5%" },
                { "width": "5%" },
                { "width": "5%" },
                { "width": "5%" },
                { "width": "5%" },
            ],
            "columnDefs": [
                { "targets": [3, 4, 5, 6, 7, 8, 9, 10], "searchable": false }
            ]
        });
    }

    $(".bet-monitor").on("click", function (event) {
        var $this = $(this);
        const gameName = $this.data("game-name");
        gameStatus.startMonitor(gameName);
    });
})