var currencyGroupGame = (function ($) {
    return {
        loadConfig: function (gameName, groupId, isCopySetting, gameType) {
            $.ajax({
                method: "GET",
                url: "/currencyGroupGame/Edit",
                data: {
                    gameName,
                    groupId,
                    isCopySetting,
                    gameType
                }
            }).done(function (content) {
                var isDisable = false;
                if (content) {
                    $('#currency-group-game-setting').html(content);
                    $.validator.unobtrusive.parse('#currency-group-game-edit-form');

                }
                for (var i = 0; i < 4; i++) {
                    if ($('#coin-value-' + i).is('[disabled=disabled]') || $('#stake-level-' + i++).is('[disabled=disabled]')) {
                        isDisable = true;
                        $('#GroupIdCopy').prop("disabled", true);
                        $("#IsCopySetting").prop('checked', false);
                        break;
                    }
                }
                $("#IsCopySetting").prop("disabled", isDisable);
            });
        },

        onChangeGroup: function (gameName, gameType, isCopy) {
            const isNoneValue = $('#GroupIdCopy').val() == '';
            const groupId = !isCopy || isNoneValue ? ('#GroupId') : ('#GroupIdCopy');
            this.loadConfig(gameName, $(groupId)[0].value, isCopy, gameType);
        },

        toggleTab: function (tabName) {
            console.log("ok");
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

        loadDefaultSetting: function () {
            this.loadConfig($('#GameName')[0].value, $('#firstGroup')[0].value, false, $('#GameType')[0].value);
        },

        getListSelectedChips: function () {
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
            $(".txt-coin-value.disabled").attr('disabled', null);
            var data = $('#currency-group-game-edit-form').serializeArray();
            $(".txt-coin-value.disabled").attr('disabled', 'disabled');

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
                this.updateSetting(isContinue, this.modalHandlerOnUpdate);
            }

            if ($('#site-exclusion').css('display') && $('#site-exclusion').css('display') !== 'none') {
                this.updateSiteExclusion(isContinue, this.modalHandlerOnUpdate);
            }
        },

        addCoin: function () {
            const txtCoinLength = $("#coins-length");
            const len = parseInt(txtCoinLength.val(), 0);

            $(`<div class="col-lg-2 col-coin">
                <label class="control-label">Coin ${len + 1}</label>
                <input type="number" name="CoinValues[${len}]" class="form-control txt-coin-value" value="0" />
            </div>`).insertBefore($('.icon-add-container'));

            txtCoinLength.val(len + 1);
        },

        enableDropDownList: function (event) {
            event.target.checked ? $("#GroupIdCopy").prop("disabled", false) : $("#GroupIdCopy").prop("disabled", true);
        }
    }
}(jQuery));
