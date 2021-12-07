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

        onChangeGroup: function(gameName, gameType) {
            this.loadConfig(gameName, $('#GroupId')[0].value, gameType);
        },

        toggleTab: function(tabName) {
            $('#setting').hide();
            $('#site-exclusion').hide();

            $('#site-exclusion-tab-header').removeClass("active");
            $('#setting-tab-header').removeClass("active");

            const id = '#' + tabName;
            const headerId = '#' + tabName + '-tab-header';

            tabName == 'setting' && this.loadDefaultSetting();
            tabName == 'site-exclusion' && this.loadSiteExclusion();


            $(id).show();
            $(headerId).addClass("active");
        },

        loadSiteExclusion: function () {
            var gameId = $('#GameId')[0].value;
            var partnerId = $('#PartnerId')[0].value;

            $.ajax({
                method: "GET",
                url: "/partner/site_exclusion",
                data: {
                    gameId,
                    partnerId,
                }
            }).done(function (exclusionSites) {
                if (exclusionSites) {
                    $('#siteExclusions').val(exclusionSites.join('; '));
                }
            });
        },

        loadDefaultSetting: function() {
            this.loadConfig($('#GameName')[0].value, $('#firstGroup')[0].value, $('#GameType')[0].value);
        },

        getListSelectedChips: function() {
            var chips = [];

            $("#chip-collector input[type=checkbox]:checked").map(function () {
                chips.push($(this).val());
            });

            return chips;
        },

        updateSiteExclusion: function (isContinue, modalHandlerOnUpdate) {
            var gameId = $('#GameId')[0].value;
            var partnerId = $('#PartnerId')[0].value;
            var exclusionSites = $('#siteExclusions').val().replace(/\s/g, '').split(';');

            $.ajax({
                method: "POST",
                url: "/partner/site_exclusion",
                data: {
                    gameId,
                    partnerId,
                    exclusionSites
                }
            }).done(function (result) {
                if (result) {
                    $('#currency-group-game-successMsg').show();
                }
                else {
                    $('#currency-group-game-failedMsg').show();
                }

                modalHandlerOnUpdate(isContinue);
            });
        },

        updateSetting: function (isContinue, modalHandlerOnUpdate) {
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

                modalHandlerOnUpdate(isContinue);
            });
        },

        modalHandlerOnUpdate: function (isContinue) {
            setTimeout(function () {
                $('#updateBtn').prop('disabled', false);
                $('#currency-group-game-successMsg').hide();
                $('#currency-group-game-failedMsg').hide();

                if (!isContinue) {
                    $('#partnerSettingModal').modal('hide');
                    location.reload();
                }
            }, 1000);
        },

        onSubmit: function (isContinue) {
            if (isContinue) {
                $('#partnerSettingModal').on('hide.bs.modal', function (event) {
                    location.reload();
                });
            }

            if ($('#setting').css('display') && $('#setting').css('display') !== 'none') {
                this.updateSetting(isContinue, this.modalHandlerOnUpdate );
            }

            if ($('#site-exclusion').css('display') && $('#site-exclusion').css('display') !== 'none') {
                this.updateSiteExclusion(isContinue, this.modalHandlerOnUpdate);
            }
        }
    }
}(jQuery));