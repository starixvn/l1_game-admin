var jackpotSettings = (function ($) {
    var $modal = $("#modal");

    function onUpsertSucess(data) {
        if (data.success) {
            $("input, select", "#general-settings-form").prop("disabled", true);
            $("#bot-ratio-tab").removeClass("disabled");

            if (data.id) {
                $("#Id", "#general-settings-form").val(data.id);
            }
        }

        var label = $("#result-label");

        label.removeClass();

        label.addClass(`text-${data.success ? "success" : "danger"}`);
        label.text(data.message);
        label.show();
        setTimeout(function () {
            label.hide();
        }, 1000);
    }

    function openModal() {
        $modal.modal({ backdrop: "static", show: true });

        var $generalSettingsForm = $("#general-settings-form");
        $.validator.unobtrusive.parse($generalSettingsForm);

        $forms = $("form", "#modal .tab-pane");
        $.validator.unobtrusive.parse($forms);
        $forms.on("submit", function (event) {
            event.preventDefault();
            var $form = $(this);
            if ($form.valid() && $generalSettingsForm.valid()) {
                $("#Id", $form).val($("#Id", $generalSettingsForm).val());
                $("#GameId", $form).val(
                    $("#GameId", $generalSettingsForm).val()
                );
                $("#OperatorId", $form).val(
                    $("#OperatorId", $generalSettingsForm).val()
                );
                $("#SiteNames", $form).val(
                    $("#SiteNames", $generalSettingsForm).val()
                );

                if (confirm("Are you sure you want to submit?")) {
                    return true;
                } else {
                    event.stopImmediatePropagation();
                }
            }
        });

        $(".btn-submit", "#modal").on("click", function (e) {
            $("form", "#modal .active.tab-pane").trigger("submit");
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
            const length = $(".txt-bot-bet-level", "#bot-bet-levels").length;

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
                        </div>`).insertBefore($(".add-container"));

            $("#bot-bet-level-legend")[0].innerHTML = `Bot Bet Levels (${
                length + 1
            } levels)`;
            this.reparseFormValidation("#bot-settings-form");
        },
        removeBotBetLevel: function (event) {
            $(event.target).parent().remove();
            const length = $(".txt-bot-bet-level", "#bot-bet-levels").length;

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
            var $betLevels = $(".txt-bot-bet-level", "#bot-bet-levels");

            for (let index = 0; index < $betLevels.length; index++) {
                var $betLevel = $($betLevels[index]);
                $betLevel.attr(
                    "name",
                    $betLevel.attr("name").replace(/\d+/, index)
                );
            }
            this.reparseFormValidation("#bot-settings-form");
        },
        reparseFormValidation: function (formSelector) {
            var $form = $(formSelector)
                .removeData("validator")
                .removeData("unobtrusiveValidation");

            $.validator.unobtrusive.parse($form);
        },
        updateBotCount: function (element) {
            var per = parseFloat(element.value);
            var totalBots = parseInt($("#TotalBot").val());
            var currentBot = parseInt((per * totalBots) / 100);

            $(element)
                .parent()
                .find(".bot-number")
                .html(`${currentBot} bot(s)`);
        },
        updateServiceFee: function (event) {
            $("#JackpotServiceFee", "#player-settings-form").val(
                100 - parseInt(event.target.value)
            );
        },
    };
})(jQuery);

$(document).ready(function () {
    jackpotSettings.init();
});
