var gameStatus = (function ($) {
    var currentGameName = '';

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
            var settingTitle = view == 'OddSettings' ? 'Odds: ' : 'General Setting: ';

            $.ajax({
                method: "GET",
                url: "/gamedetails/" + gameName + "?view=" + view
            }).done(function (content) {
                if (content) {
                    $('#gamedetails').html(content);
                    $('#settingTitle').html(settingTitle + gameDisplayName);
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
                        window.open(data.redirectUrl, "_blank", "width=1020,height=765");
                    }
                });
        },
        submit: function () {
            $('#updateBtn').prop('disabled', true);
            let requestUrl = "/update";

            if ($('#botsetting').css('display') && $('#botsetting').css('display') !== 'none') {
                requestUrl = "/betlimit/update";
            }

            if ($('#oddSetting').css('display') && $('#oddSetting').css('display') !== 'none') {
                requestUrl = "/odds/update";
            }

            if ($('#jackpots').css('display') && $('#jackpots').css('display') !== 'none') {
                requestUrl = "/jackpot/update";
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
            if ($('#Type').val() === "2") {
                $('#slots-config').show();
                $('#rng-game').hide();
            } else {
                $('#slots-config').hide();
                $('#rng-game').show();
            }
        },

        toggleSettingTabs: function (name, element) {
            $('#general').hide();
            $('#jackpots').hide();
            $('#botsetting').hide();

            $('#general-tab-header').removeClass("active");
            $('#jackpots-tab-header').removeClass("active");
            $('#botsetting-tab-header').removeClass("active");

            const id = '#' + name;
            const headerId = '#' + name + '-tab-header';

            $(id).show();
            $(headerId).addClass("active");
        },

        formatNumber: function (id) {
            var elm = $(`#${id}`);
            var val = elm.val().replace(/,/g, '');
            var parts = val.split(".");
            parts[0] = parseInt(parts[0]).toLocaleString('en');
            elm.val(parts.join("."));
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