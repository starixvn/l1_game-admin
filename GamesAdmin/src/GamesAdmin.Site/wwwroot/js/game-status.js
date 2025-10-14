var gameStatus = (function ($) {
    var currentGameName = '';
    var myChart = null;
    var $oddsMultiplierTemplate;
    var oddsMultiplierIndex;
    var originalTeams = null;

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

                    $oddsMultiplierTemplate = $("#odds-multipliers .multiplier-item").first();
                    oddsMultiplierIndex = $("#odds-multipliers .multiplier-item").length;

                    gameStatus.initFanexSelect("#OtherGameIdsInGroup", "#fanexGameIds");

                    $('#gameModal').modal('show');

                    $.validator.addMethod("unique", function (value, element, params) {
                        for (const coinElement of $(".txt-bot-bet-level", "#bot-bet-levels")) {
                            if (coinElement.value === value && coinElement !== element) {
                                return false;
                            }
                        }
                        return true;
                    });

                    $.validator.unobtrusive.adapters.addBool("unique");
                    $.validator.unobtrusive.parse('#gameForm');
                }
            });
        },

        getFanexSelectData: (selectElement) => {
            var data = [];
            for (let option of selectElement.options) {
                data.push({
                    id: option.value,
                    value: option.value,
                    text: option.text,
                    enable: true,
                    checked: option.selected
                });
            }
            return data;
        },

        initFanexSelect: function (selector, displaySelector) {
            var $element = $(selector);
            if ($element[0]) {
                var selections = gameStatus.getFanexSelectData($element[0]);

                $(displaySelector).fanexMultiSelect({
                    listData: selections,
                    showFilter: true,
                    onLeave: function () {
                        var value = $(displaySelector).fanexMultiSelect('getItems', true);

                        $element.val(value);
                    },
                });
            }
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
                    gameStatus.initFanexSelect("#OtherGameIdsInGroup", "#fanexGameIds");

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
                var $gameIdSelect = $("#bot-otherGameIdsInGroup", wrapper);
                if (result.otherGameIdsInGroup) {
                    $gameIdSelect.empty();
                    for (var game of result.otherGamesInGroup) {
                        $gameIdSelect[0].add(new Option(game.text, game.value, true, true),);
                    }
                    $gameIdSelect.val(result.otherGameIdsInGroup);
                    gameStatus.initFanexSelect($gameIdSelect, $("#bot-fanexGameIds", wrapper));
                    $("#gameids-wrapper", wrapper).show();
                } else {
                    $gameIdSelect.val([]);
                    $("#gameids-wrapper", wrapper).hide();
                }

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
        addBotBetLevel: function () {
            const length = $(".txt-bot-bet-level", "#bot-bet-levels").length;

            $(`<div class="form-group row ml-5">
                            <input id="bot-bet-level-${length}" type="number" name="SlotsConfig.BetLevels[${length}]" class="col-2 form-control txt-bot-bet-level"
                                      step=".01"
                                      data-val-range="The field MaxWinPerRound must be between 5E-324 and 1.7976931348623157E+308."
                                      data-val-range-max="1.7976931348623157E+308"
                                      data-val-range-min="5E-324"
                                      data-val="true"
                                      data-val-number="The field must be a number."
                                      data-val-required="The Double field is required."
                                      data-val-unique="Value should be unique"
                                   oninput="currencyGroupGame.updateTotalBet(this.value,${length})" />
                            <i class="icon-cancel text-error mt-3 ml-4 mr-5" style="cursor: pointer" onclick="gameStatus.removeBotBetLevel(event)"></i>
                            <label class=" mt-3 ml-5 pl-5" id="total-bet-${length}">${0}</label>
                        </div>`).insertBefore($('.add-container'));

            $("#botBetLevel_legend")[0].innerHTML = `Bot Bet Levels (${length + 1} levels)`
            this.reparseFormValidation();
        },
        removeBotBetLevel: function (event) {
            $(event.target).parent().remove();
            const length = $(".txt-bot-bet-level", "#bot-bet-levels").length;

            $("#botBetLevel_legend")[0].innerHTML = `Bot Bet Levels (${length} levels)`

            this.updateBetLevelIndex();
        },
        updateTotalBet: function (value, index) {
            $(`#total-bet-${index}`)[0].innerHTML = `${+(value * 25).toFixed(5)}`;
        },
        updateBetLevelIndex: function () {
            var $betLevels = $(".txt-bot-bet-level", "#bot-bet-levels");

            for (let index = 0; index < $betLevels.length; index++) {
                var $betLevel = $($betLevels[index]);
                $betLevel.attr("name", $betLevel.attr("name").replace(/\d+/, index));
            }
            this.reparseFormValidation();
        },
        reparseFormValidation: function () {
            var form = $("#gameForm")
                .removeData("validator")
                .removeData("unobtrusiveValidation");

            $.validator.unobtrusive.parse(form);
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
                data: { BotRatioItems: data, otherGameIdsInGroup: $("#bot-otherGameIdsInGroup", modal).val() }
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

        setControlResultFootballDice: function (event, gameName) {
            event.preventDefault();
            this.setResultControl("FootballDiceResult", gameName);
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
                case "CrashTurnBase": return "CrashTurnBaseResult"
                case "CascadingClusterSlots": return "CascadingClusterSlotResult"
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
        setResultControlBaccaratX: function (gameName, displayName, enableMultiplier) {
            window.open(adminMonitoringUrl + "/ResultMonitor/BaccaratXResult" + "?gameConfigName=" + gameName + "&displayName=" + displayName + "&enableMultiplier=" + enableMultiplier + "&token=" + jwtAdminUserToken, "_blank", "width=1020,height=765");
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

            if ($('#slotBotSetting').css('display') && $('#slotBotSetting').css('display') !== 'none') {
                requestUrl = "/slotBot/update";
            }

            if ($('#adjustweight').css('display') && $('#adjustweight').css('display') !== 'none') {
                this.updateSymbolWeight();
                return;
            }

            if (!$('#gameForm').valid()) {
                $('#updateBtn').prop('disabled', false);
                return
            }

            this.updateOddsMultipliersIndex();
            this.resetOriginalTeams();

            $.ajax({
                method: "POST",
                url: requestUrl,
                //dataType: 'json',
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
                    weights,
                    otherGameIdsInGroup: $("#OtherGameIdsInGroup").val()
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

            let requestUrl = "/odds/update";

            if ($('#plinkoConfig').css('display')) {
                requestUrl = "/plinko-config/update";
            }

            $.ajax({
                method: "POST",
                url: requestUrl,
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

        toggleGameTypeConfig: function () {
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

            if (type === '7') {
                $('#tournament-config').show();
                $('#video-stream-config').show();
                $('#use-margin').show();
            }
            else {
                $('#tournament-config').hide();
                $('#video-stream-config').hide();
                $('#use-margin').hide();
                $('#have-tournament').prop('checked', false);
                $('#have-video-stream').prop('checked', false);
            }
        },

        toggleSettingTabs: function (name, element) {
            $('#general').hide();
            $('#jackpots').hide();
            $('#botsetting').hide();
            $('#adjustweight').hide();
            $('#slotBotSetting').hide();

            $('#general-tab-header').removeClass("active");
            $('#jackpots-tab-header').removeClass("active");
            $('#botsetting-tab-header').removeClass("active");
            $('#adjustweight-tab-header').removeClass("active");
            $('#slotBotSetting-tab-header').removeClass("active");

            this.showTab(name);

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

            this.showTab(tabName);
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
        },
        addMultiplierItem: function () {
            var $newItem = $oddsMultiplierTemplate.clone();

            var $ratio = $(".ratio", $newItem);
            $ratio.attr("name", $ratio.attr("name").replace(/\d+/, oddsMultiplierIndex)).val(0);

            var $number = $(".number", $newItem);
            $number.attr("name", $number.attr("name").replace(/\d+/, oddsMultiplierIndex)).val(0);

            console.log($newItem.html());

            $("#odds-multipliers").append($newItem);
            oddsMultiplierIndex++;
        },
        removeMultiplierItem: function (event) {
            $(event.target).parent().remove();
        },
        updateOddsMultipliersIndex: function () {
            var $multiplierItems = $(".multiplier-item", "#odds-multipliers");

            for (let index = 0; index < $multiplierItems.length; index++) {
                var item = $multiplierItems[index];
                var $ratio = $(".ratio", item);
                $ratio.attr("name", $ratio.attr("name").replace(/\d+/, index));

                var $number = $(".number", item);
                $number.attr("name", $number.attr("name").replace(/\d+/, index));
            }
        },
        addTournamentTeamItem: function (teamIndex, teamName, currentTeamName) {
            var $newItem = $(`
                <div class="row mt-2 tournament-team-item">
                    <label class="team-index-label control-label col-2 mt-2 mr-2">Team ${teamIndex + 1}</label>
                    <input type="text" name="TournamentConfig.Teams[${teamIndex}]" value="${teamName}" class="form-control col-2" readonly="true" />
                    ${currentTeamName ? `<span class="ml-2 mt-2 current-team" style="color: grey;">(Current: ${currentTeamName})</span>` : ''}
                </div>
            `);

            if (currentTeamName && currentTeamName !== teamName) {
                $newItem.find('input').css('color', 'red');
            }

            $("#tournament-teams").append($newItem);
        },
        removeAllTournamentTeams: function () {
            $("#tournament-teams .tournament-team-item").remove();
        },
        updateTournamentTeams: function () {
            // Get current teams before clearing
            if (originalTeams === null) {
                originalTeams = [];
                $("#tournament-teams .tournament-team-item input").each(function () {
                    originalTeams.push($(this).val());
                });
            }

            this.removeAllTournamentTeams();

            var tournamentTeamsInput = $("#tournament-teams-input").val();
            var tournamentTeams = tournamentTeamsInput.split(/[,;]/).filter(team => team.trim() !== "");

            // Add new/updated teams
            for (let index = 0; index < tournamentTeams.length; index++) {
                var team = tournamentTeams[index];
                var currentTeam = originalTeams[index] || null;
                if (team && team.trim() !== "") {
                    this.addTournamentTeamItem(index, team, currentTeam);
                }
            }

            // Show removed teams
            for (let index = tournamentTeams.length; index < originalTeams.length; index++) {
                var removedTeam = originalTeams[index];
                var $removedItem = $(`
                    <div class="row mt-2 tournament-team-item">
                        <label class="team-index-label control-label col-2 mt-2 mr-2">Team ${index + 1}</label>
                        <input type="text" value="${removedTeam}" class="form-control col-2" readonly="true" style="text-decoration: line-through; color: red;" />
                        <span class="ml-2 mt-2" style="color: red;">(Removed)</span>
                    </div>
                `);
                $("#tournament-teams").append($removedItem);
            }
        },
        rollbackTournamentTeams: function () {
            if (originalTeams === null) {
                return;
            }

            this.removeAllTournamentTeams();

            for (let index = 0; index < originalTeams.length; index++) {
                this.addTournamentTeamItem(index, originalTeams[index], null);
            }

            $("#tournament-teams-input").val("");
        },
        resetOriginalTeams: function () {
            originalTeams = null;
        },
        importVideoStream: function () {
            var jsonInput = $("#json-video-input").val();
            try {
                // Parse the input JSON
                var videoData = JSON.parse(jsonInput);

                // Show the "Updated Number of Videos" column
                $("#updated-number-header").show();

                // Update the table rows with the new JSON data
                var rowsUpdated = new Set();

                for (const [videoName, updatedNumber] of Object.entries(videoData)) {
                    var selector = videoName === "0"
                        ? `#video-stream-rows tr[data-video-name='0']`
                        : `#video-stream-rows tr[data-video-name='${videoName}']`;

                    var $row = $(selector);

                    if ($row.length) {
                        // If the video name exists, update the "Updated Number" column
                        var $currentCell = $(".current-number", $row);
                        var $updatedCell = $(".updated-number", $row);
                        var currentNumber = parseInt($currentCell.text()) || 0;

                        // Show the updated number cell
                        $updatedCell.show();

                        if (currentNumber !== updatedNumber) {
                            // Highlight the current and updated values
                            $currentCell.css({ "font-weight": "bold", "color": "black" });
                            $updatedCell.css({ "font-weight": "bold", "color": "red" }).text(updatedNumber);
                        }
                        else {
                            // Grey out the numbers if no update
                            $currentCell.css({ "font-weight": "normal", "color": "grey" });
                            $updatedCell.css({ "font-weight": "normal", "color": "grey" }).text("-");
                        }
                    } else {
                        // If the video name doesn't exist, add a new row
                        var newRowHtml = `
                            <tr data-video-name="${videoName}">
                                <td>${videoName}</td>
                                <td class="current-number" style="font-weight: normal; color: grey;">-</td>
                            <td class="updated-number" style="font-weight: bold; color: red;">${updatedNumber}</td>
                            </tr>
                        `;
                        $("#video-stream-rows").append(newRowHtml);
                    }

                    rowsUpdated.add(videoName);
                }

                // Mark rows with no updates as unchanged
                $("#video-stream-rows tr").each(function () {
                    var videoName = $(this).data("video-name");

                    if (videoName === undefined) {
                        return;
                    }

                    videoName = videoName.toString();

                    if (!rowsUpdated.has(videoName)) {
                        $(this).css({
                            "text-decoration": "line-through",
                            "font-weight": "bold",
                            "color": "red"
                        });

                    }
                });

                // Save the JSON data to the hidden input field
                $("#video-stream-json").val(jsonInput);
            } catch (error) {
                // Handle invalid JSON format
                alert("Invalid JSON format. Please check and try again.");
                $("#updated-number-header").hide();
                $(".updated-number").hide();

                // Reset any styling on current number cells but keep their values
                $("#video-stream-rows tr").each(function () {
                    $(this).css({
                        "text-decoration": "none",
                        "font-weight": "normal",
                        "color": "inherit"
                    });
                    $(".current-number", this).css({
                        "font-weight": "normal",
                        "color": "grey"
                    });
                });

                // Clear the JSON input
                $("#video-stream-json").val("");
            }
        },

        togglePlinkoSettingTabs: function(tabName) {
            // Hide all tab content
            $('.tab-pane').removeClass('active').hide();

            // Remove active class from all tab headers
            $('#low-tab-header').removeClass('active');
            $('#medium-tab-header').removeClass('active');
            $('#high-tab-header').removeClass('active');

            // Show selected tab content
            this.showTab(tabName);
        },

        showTab(tabName) {
            $(`#${tabName}`).addClass('active').show();
            $(`#${tabName}-tab-header`).addClass("active");
        },

        toggleMirrorValues(riskLevel) {
            const checkbox = $(`#mirror-${riskLevel}`);
            const tabContent = $(`#${riskLevel}`);

            // Get all input fields in the current tab
            const inputs = tabContent.find('input[type="number"]');

            inputs.each((index, input) => {
                // Remove any existing mirror event listener
                if (input.mirrorHandler) {
                    $(input).off('input', input.mirrorHandler);
                }

                if (checkbox.is(':checked')) {
                    // Create new event handler
                    input.mirrorHandler = function () {
                        if (!this.isMirroring) {
                            this.isMirroring = true;
                            gameStatus.mirrorValue(this, riskLevel);
                            this.isMirroring = false;
                        }
                    };
                    // Add event listener for mirror functionality
                    $(input).on('input', input.mirrorHandler);
                }
            });
        },

        mirrorValue(inputElement, riskLevel) {
            const $rowContainer = $(inputElement).closest('fieldset');
            if ($rowContainer.length === 0) {
                return;
            }

            const rowInputs = $rowContainer.find('input[type="number"]').toArray();
            const currentIndex = rowInputs.indexOf(inputElement);


            if (currentIndex === -1) {
                return;
            }

            const mirrorIndex = rowInputs.length - 1 - currentIndex;


            if (currentIndex === mirrorIndex) {
                return;
            }

            const mirrorInput = rowInputs[mirrorIndex];
            if (mirrorInput) {
                const value = $(inputElement).val();
                if (!isNaN(value)) {
                    mirrorInput.isMirroring = true;
                    $(mirrorInput).val(value);
                    mirrorInput.isMirroring = false;
                    
                    // Update RTP for the mirrored input
                    this.updateEstimatedRTP(mirrorInput);
                }
            }
        },

        binomial(n, k) {
            if (k > n) 
            {
                return 0;
            }

            let result = 1;
            for (let i = 0; i < k; i++) {
                result *= (n - i) / (i + 1);
            }
            return result;
        },
    
         calculateProbability(n, k) {
            const binom = this.binomial(n, k);
            const totalPossibilities = Math.pow(2, n);
            return binom / totalPossibilities;
        },
    
         getPlinkoProbabilities(n) {
            const probabilities = [];
            for (let k = 0; k <= n; k++) {
                probabilities.push(this.calculateProbability(n, k));
            }
            return probabilities;
        },
    
        calculateEstimatedRTP(n, multipliers) {
            const probabilities = this.getPlinkoProbabilities(n);
    
            if (multipliers.length !== probabilities.length) {
                throw new Error("Số lượng hệ số thưởng (multipliers) phải bằng n + 1");
            }
    
            // RTP = Σ(P(i) × M(i))
            const rtp = probabilities.reduce((acc, p, i) => acc + p * multipliers[i], 0);
    
            return rtp * 100; // % RTP
        },
    
         updateEstimatedRTP(inputElement) {
            const $rowContainer = $(inputElement).closest('fieldset');
            if ($rowContainer.length === 0) 
            {
                return;
            }
    
            const multiplierInputs = $rowContainer.find('input[type="number"]:not(.estimated-rtp)');
            
            if (multiplierInputs.length === 0) 
            {
                return;
            }
    
            const legend = $rowContainer.find('legend');
            const rowNumber = parseInt(legend.text().replace('Row ', ''));
            
            const tabContainer = $rowContainer.closest('.tab-pane');
            let riskLevel = 'low';
            if (tabContainer.attr('id') === 'medium') {
                riskLevel = 'medium';
            } else if (tabContainer.attr('id') === 'high') {
                riskLevel = 'high';
            }
            
            const rtpElementId = `rtp-${riskLevel}-${rowNumber}`;
            const $rtpInput = $(`#${rtpElementId}`);
            
            if ($rtpInput.length === 0) 
            {
                return;
            }
            
            // Extract multiplier values
            const multipliers = multiplierInputs.map(function() {
                const value = $(this).val();
                return isNaN(value) ? 0 : value;
            }).get();
    
            try {
                // Calculate RTP
                const rtp = this.calculateEstimatedRTP(rowNumber, multipliers);
                
                // Update the RTP display
                $rtpInput.text(rtp.toFixed(2));
                
                // Update average RTP for the current risk level
                this.updateAverageRTP(riskLevel);
            } catch (error) {
                console.error('Error calculating RTP:', error);
                $rtpInput.text('0.00');
            }
        },
    
        updateAverageRTP(riskLevel) {
            // Calculate average RTP for rows 8 to 16
            const startRow = 8;
            const endRow = 16;
            const numberOfRows = endRow - startRow + 1; // 9 rows total
            
            let sumOfRTPs = 0;
            let validRows = 0;
            
            for (let row = startRow; row <= endRow; row++) {
                const rtpElementId = `rtp-${riskLevel}-${row}`;
                const $rtpElement = $(`#${rtpElementId}`);
                
                if ($rtpElement.length > 0) {
                    const rtpValue = parseFloat($rtpElement.text());
                    if (!isNaN(rtpValue)) {
                        sumOfRTPs += rtpValue;
                        validRows++;
                    }
                }
            }
            
            // Calculate average: (1 / number of rows) * sum of all Row RTPs
            const averageRTP = validRows > 0 ? (1 / numberOfRows) * sumOfRTPs : 0;
            
            // Update the average RTP display
            const $averageElement = $(`#average-rtp-${riskLevel}`);
            if ($averageElement.length > 0) {
                $averageElement.text(averageRTP.toFixed(2));
            }
        },
    
        // Initialize RTP calculation for Plinko
        initializePlinkoRTP() {
            const self = this;
            const allInputs = 'input[type="number"]:not(.estimated-rtp)';
            
            // Remove existing event listeners to avoid duplicates
            $(allInputs).off('input.plinko-rtp');
            
            // Add event listener to all multiplier inputs
            $(allInputs).on('input.plinko-rtp', function() {
                self.updateEstimatedRTP(this);
            });

            // Initialize mirror values for all risk levels if checkboxes are checked
            const riskLevels = ['low', 'medium', 'high'];
            riskLevels.forEach(riskLevel => {
                const checkbox = $(`#mirror-${riskLevel}`);
                if (checkbox.is(':checked')) {
                    self.toggleMirrorValues(riskLevel);
                }
            });

            // Calculate initial RTP for all rows in all tabs
            riskLevels.forEach(riskLevel => {
                const tabContent = $(`#${riskLevel}`);
                const allRows = tabContent.find('fieldset');
                allRows.each(function() {
                    const firstInput = $(this).find(allInputs).first();
                    if (firstInput.length > 0) {
                        self.updateEstimatedRTP(firstInput[0]);
                    }
                });
            });

            // Calculate initial average RTP for all risk levels
            self.updateAverageRTP('low');
            self.updateAverageRTP('medium');
            self.updateAverageRTP('high');
        },
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

    $("#gameModal").on("hidden.bs.modal", function () {
        $("#gamedetails").empty();
    });

    $("#gameModal").on("shown.bs.modal", function () {
        // Re-initialize Plinko RTP calculation when modal is shown
        if ($("#plinkoConfig").length > 0) {
            gameStatus.initializePlinkoRTP();
        }
    });

    $("#partnerSettingModal").on("hidden.bs.modal", function () {
        $("#partnerSettingContent").empty();
    });


    $(".bet-monitor").on("click", function (event) {
        var $this = $(this);
        const gameName = $this.data("game-name");
        gameStatus.startMonitor(gameName);
    });
})