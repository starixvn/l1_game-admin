var currencyGroupGame = (function ($) {
    return {
        loadConfig: function (gameName, groupId, isCopySetting, gameType) {
            var context = this;
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
                    $.validator.addMethod("unique", function (value, element, params) {
                        for (const coinElement of $(".txt-coin-value", "#coin-values")) {
                            if (coinElement.value === value && coinElement !== element) {
                                return false;
                            }
                        }
                        return true;
                    });
                    $.validator.addMethod("unique", function (value, element, params) {
                        for (const coinElement of $(".txt-bet-level", "#bet-levels")) {
                            if (coinElement.value === value && coinElement !== element) {
                                return false;
                            }
                        }
                        return true;
                    });
                 
                    $.validator.unobtrusive.adapters.addBool("unique");
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

                $("#coin-values").on("change", ".txt-coin-value", context.onCoinValueChange);
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
            var $form = $('#currency-group-game-edit-form');
            if ($form.valid()) {
                this.updateCoinValuesIndex();
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
            }
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
            const length = $(".txt-coin-value", "#coin-values").length;

            $(`<div class="form-group row">
                            <input type="radio" class="mr-3 ml-3" name="DefaultCoinValue" id="DefaultCoinValue" value="0" />
                            <input id="coin-value-${length}" value="0" type="number" name="CoinValues[${length}]" class="col-2 form-control txt-coin-value"
                                  data-val-range="The field MaxWinPerRound must be between 5E-324 and 1.7976931348623157E+308."
                                  data-val-range-max="1.7976931348623157E+308"
                                  data-val-range-min="5E-324"
                                  data-val="true"
                                  data-val-number="The field must be a number."
                                  data-val-required="The Double field is required."
                                  data-val-unique="Value should be unique"
                                  />
                            <i class="icon-cancel text-error mt-3 ml-4" style="cursor: pointer" onclick="currencyGroupGame.removeCoinValue(event)"></i>
                        </div>`).insertBefore($('.add-container'));
            this.reparseFormValidation();
        },
        addBetLevel: function () {
            const length = $(".txt-bet-level", "#bet-levels").length;

            $(`<div class="form-group row">
                            <input type="radio" class="mr-3 ml-3" name="DefaultBetLevel" id="DefaultBetLevel" value="0" />
                            <input id="bet-level-${length}" type="number" name="BetLevels[${length}]" class="col-2 form-control txt-bet-level"
                                      step=".01"
                                      data-val-range="The field MaxWinPerRound must be between 5E-324 and 1.7976931348623157E+308."
                                      data-val-range-max="1.7976931348623157E+308"
                                      data-val-range-min="5E-324"
                                      data-val="true"
                                      data-val-number="The field must be a number."
                                      data-val-required="The Double field is required."
                                      data-val-unique="Value should be unique"
                                   oninput="currencyGroupGame.updateTotalBet(this.value,${length})" />
                            <i class="icon-cancel text-error mt-3 ml-4 mr-5" style="cursor: pointer" onclick="currencyGroupGame.removeBetLevel(event)"></i>
                            <label class=" mt-3 ml-5 pl-5" id="total-bet-${length}"></label>
                        </div>`).insertBefore($('.add-container'));
            this.reparseFormValidation();
        },
        updateTotalBet: function (value, index) {
            $(`#total-bet-${index}`)[0].innerHTML = `${+(value * 25).toFixed(5)}`;
            $(`#bet-level-${index}`).prev().val(value);
        },
        enableDropDownList: function (event) {
            event.target.checked ? $("#GroupIdCopy").prop("disabled", false) : $("#GroupIdCopy").prop("disabled", true);
        },
        removeCoinValue: function (event) {
            $(event.target).parent().remove();
            this.updateCoinValuesIndex();
        },
        onCoinValueChange: function (event) {
            const $parent = $(event.target).parent();
            $("#DefaultCoinValue", $parent).val(event.target.value);
        },
        updateCoinValuesIndex: function () {
            var $coinValues = $(".txt-coin-value", "#coin-values");

            for (let index = 0; index < $coinValues.length; index++) {
                var $coinValue = $($coinValues[index]);
                $coinValue.attr("name", $coinValue.attr("name").replace(/\d+/, index));
            }
            this.reparseFormValidation();
        },
        removeBetLevel: function (event) {
            const val = $(event.target).prev().val();
            const defaultVal = $('input[name="DefaultBetLevel"]:checked').val();

            if (val != defaultVal) {
                $(event.target).parent().remove();
                this.updateBetLevelIndex();
            }
            else {
                alert("Cannot delete the default value. Set a new default first")
            }
    
        },
        onBetLevelChange: function (event) {
            const $parent = $(event.target).parent();
            $("#DefaultBetLevel", $parent).val(event.target.value);
        },
        updateBetLevelIndex: function () {
            var $betLevels = $(".txt-bet-level", "#bet-levels");

            for (let index = 0; index < $betLevels.length; index++) {
                var $betLevel = $($betLevels[index]);
                $betLevel.attr("name", $betLevel.attr("name").replace(/\d+/, index));
            }
            this.reparseFormValidation();
        },
        updateBotBetLevelIndex: function () {
            var $betLevels = $(".txt-bot-bet-level", "#bot-bet-levels");

            for (let index = 0; index < $betLevels.length; index++) {
                var $betLevel = $($betLevels[index]);
                $betLevel.attr("name", $betLevel.attr("name").replace(/\d+/, index));
            }
            this.reparseFormValidation();
        },
        reparseFormValidation: function () {
            var form = $("#currency-group-game-edit-form")
                .removeData("validator")
                .removeData("unobtrusiveValidation"); 

            $.validator.unobtrusive.parse(form);
        }
    }
}(jQuery));
