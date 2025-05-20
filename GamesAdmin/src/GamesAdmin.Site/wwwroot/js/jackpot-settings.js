var jackpotSettings = (function ($) {
    var elements = {
        $modal: $("#modal"),
    };

    function onUpsertSucess(data) {
        elements.$resultLabel
            .removeClass()
            .addClass(`text-${data.success ? "success" : "danger"}`)
            .text(data.message)
            .show();

        if (data.success) {
            elements.$generalSettingsForm
                .find("input, select")
                .prop("disabled", true);

            elements.$modal.find("#bot-ratio-tab").removeClass("disabled");

            if (data.id) {
                $("#Id", elements.$generalSettingsForm).val(data.id);
            }

            setTimeout(function () {
                elements.$resultLabel.hide();
            }, 1000);
        }
    }

    function updateElements() {
        elements.$generalSettingsForm = elements.$modal.find(
            "#general-settings-form"
        );
        elements.$resultLabel = elements.$modal.find("#result-label");
        elements.$botBetLevels = elements.$modal.find("#bot-bet-levels");
        elements.$settingDetails = elements.$modal.find("#setting-details");
        elements.$copySettingForm = elements.$modal.find("#copy-setting-form");
    }

    function setupValidation() {
        $.validator.addMethod(
            "unique",
            (value, element) => {
                const betLevels =
                    elements.$botBetLevels.find(".txt-bot-bet-level");
                return ![...betLevels].some(
                    (el) => el.value === value && el !== element
                );
            },
            "Value must be unique"
        );

        $.validator.unobtrusive.adapters.addBool("unique");
    }

    function openModal() {
        elements.$modal.modal({ backdrop: "static", show: true });
        updateElements();

        setupValidation();

        elements.$modal.find(".btn-submit").on("click", function (e) {
            elements.$settingDetails
                .find(".active.tab-pane form")
                .trigger("submit");
        });

        reparseFormValidation(elements.$modal.find("form"));
        registerFormSubmitEvents();
    }

    function registerFormSubmitEvents() {
        elements.$settingDetails.on("submit", "form", function (event) {
            event.preventDefault();
            var $form = $(this);

            if ($form.valid()) {
                $("#Id", $form).val(
                    $("#Id", elements.$generalSettingsForm).val()
                );
                $("#GameId", $form).val(
                    $("#GameId", elements.$generalSettingsForm).val()
                );
                $("#OperatorId", $form).val(
                    $("#OperatorId", elements.$generalSettingsForm).val()
                );
                $("#SiteNames", $form).val(
                    $("#SiteNames", elements.$generalSettingsForm).val()
                );

                if (confirm("Are you sure you want to submit?")) {
                    return true;
                } else {
                    event.stopImmediatePropagation();
                }
            }
        });
    }

    function convertPartnerSelectToFanexItem(item) {
        return {
            id: item.value,
            value: item.value,
            text: item.text,
            enable: true,
            checked: item.selected,
        };
    }

    function reparseFormValidation(formSelector) {
        var $form = $(formSelector)
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse($form);
    }

    return {
        init: function () {
            var $gameIds = $("#GameIds");
            var selections = jackpotSettings.getFanexSelectData($gameIds[0]);

            $("#fanexSelectGameId").fanexMultiSelect({
                listData: selections,
                showFilter: true,
                onLeave: function () {
                    var value = $("#fanexSelectGameId").fanexMultiSelect(
                        "getItems",
                        true
                    );

                    $gameIds.val(value);
                },
            });
        },
        onCopySettingsSuccess: function () {
            reparseFormValidation(elements.$settingDetails.find("form"));
        },
        onCopySettingsChange: function (event) {
            if (event.target.value) {
                elements.$copySettingForm.trigger("submit");
            }
        },
        toggleCopySettings: function (event) {
            elements.$copySettingForm
                .find("select")
                .prop("disabled", !event.target.checked);
        },
        openCreateForm: function () {
            openModal();
        },
        openUpdateForm: function () {
            openForm("#update-form");
        },
        onCreateSuccess: onUpsertSucess,
        onUpdateSuccess: onUpsertSucess,
        getFanexSelectData: (selectElement) => {
            var data = [];
            for (let options of selectElement.options) {
                data.push(convertPartnerSelectToFanexItem(options));
            }
            return data;
        },
        addBotBetLevel: function () {
            const length =
                elements.$botBetLevels.find(".txt-bot-bet-level").length;

            $(`<div class="form-group row ml-5">
                            <input id="bot-bet-level-${length}" type="number" name="BotBetLevels[${length}]" class="col-2 form-control txt-bot-bet-level"
                                      step=".01"
                                      data-val-range="The field MaxWinPerRound must be between 5E-324 and 1.7976931348623157E+308."
                                      data-val-range-max="1.7976931348623157E+308"
                                      data-val-range-min="5E-324"
                                      data-val="true"
                                      data-val-number="The field must be a number."
                                      data-val-required="The Double field is required."
                                      data-val-unique="Value should be unique"
                                   oninput="jackpotSettings.updateTotalBet(this.value,${length})" />
                            <i class="icon-cancel text-error mt-3 ml-4 mr-5" style="cursor: pointer" onclick="jackpotSettings.removeBotBetLevel(event)"></i>
                            <label class=" mt-3 ml-5 pl-5" id="total-bet-${length}">${0}</label>
                        </div>`).insertBefore(elements.$botBetLevels.find(".add-container"));

            elements.$modal.find("#bot-bet-level-legend")[0].innerHTML = `Bot Bet Levels (${
                length + 1
            } levels)`;
            reparseFormValidation(elements.$settingDetails.find("#bot-settings-form"));
        },
        removeBotBetLevel: function (event) {
            $(event.target).parent().remove();
            const length =
                elements.$botBetLevels.find(".txt-bot-bet-level").length;

            $(
                "#bot-bet-level-legend"
            )[0].innerHTML = `Bot Bet Levels (${length} levels)`;

            this.updateBetLevelIndex();
        },
        updateTotalBet: function (value, index) {
            $(`#total-bet-${index}`)[0].innerHTML = `${+(value * 25).toFixed(
                5
            )}`;
        },
        updateBetLevelIndex: function () {
            var $betLevels = elements.$botBetLevels.find(".txt-bot-bet-level");

            for (let index = 0; index < $betLevels.length; index++) {
                var $betLevel = $($betLevels[index]);
                $betLevel.attr(
                    "name",
                    $betLevel.attr("name").replace(/\d+/, index)
                );
            }
            reparseFormValidation(elements.$settingDetails.find("#bot-settings-form"));
        },
        updateBotCount: function (element) {
            var per = parseFloat(element.value);
            var totalBots = parseInt(elements.$modal.find("#TotalBot").val());
            var currentBot = parseInt((per * totalBots) / 100);

            $(element)
                .parent()
                .find(".bot-number")
                .html(`${currentBot} bot(s)`);
        },
        updateServiceFee: function (event) {
            elements.$modal
                .find("#player-settings-form #JackpotServiceFee")
                .val(100 - parseInt(event.target.value));
        },
    };
})(jQuery);

$(document).ready(function () {
    jackpotSettings.init();
});
