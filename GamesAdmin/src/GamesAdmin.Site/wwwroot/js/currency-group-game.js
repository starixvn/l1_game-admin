var currencyGroupGame = (function ($) {
    return {
        loadConfig: function (gameName, groupId, gameType) {
            $.ajax({
                method: "GET",
                url: "/currencyGroupGame/Edit",
                data: {
                    gameName,
                    groupId,
                    gameType
                }
            }).done(function (content) {
                if (content) {
                    $('#currency-group-game-setting').html(content);
                    $.validator.unobtrusive.parse('#currency-group-game-edit-form');
                }
            });
        },

        onChangeGroup(gameName, gameType) {
            this.loadConfig(gameName, $('#CurrencyGroupId')[0].value, gameType);
        },

        toggleTab(tabName) {
            $('#setting').hide();
            $('#site-exclusion').hide();

            $('#site-exclusion-tab-header').removeClass("active");
            $('#setting-tab-header').removeClass("active");

            const id = '#' + tabName;
            const headerId = '#' + tabName + '-tab-header';

            tabName == 'setting' && this.loadDefaultSetting();

            $(id).show();
            $(headerId).addClass("active");
        },

        loadDefaultSetting() {
            this.loadConfig($('#GameName')[0].value, $('#firstGroup')[0].value, $('#GameType')[0].value);
        },

        getListSelectedChips() {
            var chips = [];

            $("#chip-collector input[type=checkbox]:checked").map(function () {
                chips.push($(this).val());
            });

            return chips;
        },

        updateSetting(isContinue) {
            var data = $('#currency-group-game-edit-form').serializeArray();
            var chips = this.getListSelectedChips();

            for (var i = 0; i < chips.length; i++) {
                data.push({
                    name: ['Chips[' + i + ']'], value: chips[i]
                });
            }

            $.ajax({
                method: "POST",
                url: 'currencyGroupGame/Edit',
                dataType: 'json',
                data,
            }).done(function (result) {
                if (result.success) {
                    $('#currency-group-game-successMsg').show();
                }
                else {
                    $('#currency-group-game-failedMsg').show();
                }

                setTimeout(function () {
                    $('#updateBtn').prop('disabled', false);
                    $('#currency-group-game-successMsg').hide();
                    $('#currency-group-game-failedMsg').hide();

                    if (!isContinue) {
                        $('#partnerSettingModal').modal('hide');
                        location.reload();
                    }
                }, 1000);
            });
        }
    }
}(jQuery));