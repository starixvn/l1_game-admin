var buyFreeSpinSetting = (function ($) {
    const $modal = $("#modal");
    const siteExclusions = "#siteExclusions";
    const addFreeSpinSettingForm = "#add-free-spin-setting-form";
    const updateFreeSpinSettingForm = "#update-free-spin-setting-form";
    const operatorSelect = "#operator-select";

    let buyFreeSpinSiteExclusions = [];
    function onUpsert(data) {
        if (data.success) {
            location.reload();
        }
        const label = $("#result-label-form");

        label.removeClass();
        label.addClass(`text-${data.success ? "message" : "danger"}`);
        label.text(data.error);
    }

    function openModal(formId) {
        return function () {
            $modal.modal("show");
            $.validator.unobtrusive.parse(formId);

            const isAddNew = formId.startsWith("#add");

            if (isAddNew) {
                buyFreeSpinSiteExclusions = [];
                console.log("add new");
            } else {
                console.log("update");
            }
            initOnChangeOperatorSelect();

        };
    }

    function initOnChangeOperatorSelect() {
        $(operatorSelect).change(function () {
            const operatorId = $(operatorSelect).val();
            const entry = buyFreeSpinSiteExclusions.find((x) => x.operatorId === operatorId);
            $(siteExclusions).val(entry ? entry.siteExclusions : "");
            // Keep hidden inputs updated when operator changes
            syncHiddenSiteExclusionInputs();
        });
    }

    function onChangeSiteExclusions(value) {
        const operatorId = $(operatorSelect).val();
        if (!operatorId) {
            return;
        }

        const index = buyFreeSpinSiteExclusions.findIndex((x) => x.operatorId === operatorId);
        if (index >= 0) {
            buyFreeSpinSiteExclusions[index].siteExclusions = value;
        } else {
            buyFreeSpinSiteExclusions.push({ operatorId: operatorId, siteExclusions: value });
        }

        // Keep hidden inputs in sync as user edits
        syncHiddenSiteExclusionInputs();
    }

    function syncHiddenSiteExclusionInputs() {
        [addFreeSpinSettingForm, updateFreeSpinSettingForm].forEach(function (selector) {
            const $form = $(selector);
            if ($form.length === 0) {
                return;
            }

            // Remove any previously generated hidden inputs for site exclusions
            $form.find("input.generated-site-exclusion").remove();

            // Generate hidden inputs for SiteExclusions to match server model binding
            // Create/Update ViewModel.SiteExclusions -> BuyFreeSpinSiteExclusion[]
            buyFreeSpinSiteExclusions.forEach(function (entry, idx) {
                const operatorId = entry.operatorId;
                const sitesArray = (entry.siteExclusions || "")
                    .split(";")
                    .map(function (s) {
                        return s.trim();
                    })
                    .filter(function (s) {
                        return s.length > 0;
                    });

                //if sitesArray is empty, skip the generation of hidden inputs
                if (sitesArray.length === 0) {
                    return;
                }

                // Set the value of $(siteExclusions) to reflect the current sites for this operator
                if (operatorId === $(operatorSelect).val()) {
                    $(siteExclusions).val(entry.siteExclusions);
                }

                // Explicit index to aid ASP.NET Core model binder
                $("<input/>", {
                    type: "hidden",
                    name: "SiteExclusions.Index",
                    value: idx,
                    class: "generated-site-exclusion",
                }).appendTo($form);

                $("<input/>", {
                    type: "hidden",
                    name: `SiteExclusions[${idx}].OperatorId`,
                    value: operatorId,
                    class: "generated-site-exclusion",
                }).appendTo($form);

                sitesArray.forEach(function (site, j) {
                    $("<input/>", {
                        type: "hidden",
                        name: `SiteExclusions[${idx}].Sites[${j}]`,
                        value: site,
                        class: "generated-site-exclusion",
                    }).appendTo($form);
                });
            });
        });
    }

    function setInitialSiteExclusionsFromModel(modelSiteExclusions) {
        buyFreeSpinSiteExclusions = (modelSiteExclusions || []).map(function (m) {
            // Accept either camelCase or PascalCase
            const operatorId = m.operatorId || m.OperatorId || "";
            const sites = m.sites || m.Sites || [];
            return { operatorId: operatorId, siteExclusions: (sites || []).join(";") };
        });

        if ($(operatorSelect).length && $(siteExclusions).length) {
            const operatorId = $(operatorSelect).val();
            const entry = buyFreeSpinSiteExclusions.find(function (x) {
                return x.operatorId === operatorId;
            });
            $(siteExclusions).val(entry ? entry.siteExclusions : "");
        }

        syncHiddenSiteExclusionInputs();
    }

    function onCreateBegin() {
        try {
            // Ensure the current textarea value is captured for the selected operator
            const currentExclusions = $(siteExclusions).val();
            onChangeSiteExclusions(currentExclusions);

            // Ensure hidden inputs are in the DOM
            syncHiddenSiteExclusionInputs();

            // Manually submit to ensure the generated hidden inputs are sent
            const $form = $(addFreeSpinSettingForm);
            const url = $form.attr("action");
            const data = $form.serialize();

            console.log("Submitting Create payload:", data);

            $.ajax({
                url: url,
                type: "POST",
                data: data,
                success: function (resp) {
                    onUpsert(resp);
                },
                error: function (xhr) {
                    console.error("Create request failed", xhr);
                    onUpsert({ success: false, error: "Create request failed" });
                },
            });
        } catch (e) {
            console.error("onCreateBegin error", e);
        }
        return false;
    }

    function onUpdateBegin() {
        try {
            const currentExclusions = $(siteExclusions).val();
            onChangeSiteExclusions(currentExclusions);

            // Ensure hidden inputs are in the DOM
            syncHiddenSiteExclusionInputs();

            // Manually submit update
            const $form = $(updateFreeSpinSettingForm);
            const url = $form.attr("action");
            const data = $form.serialize();

            console.log("Submitting Update payload:", data);

            $.ajax({
                url: url,
                type: "POST",
                data: data,
                success: function (resp) {
                    onUpsert(resp);
                },
                error: function (xhr) {
                    console.error("Update request failed", xhr);
                    onUpsert({ success: false, error: "Update request failed" });
                },
            });
        } catch (e) {
            console.error("onUpdateBegin error", e);
        }
        return false;
    }

    function toggleAllBuyFreeSpinCheckboxes() {
        const checked = $("#select-all-buy-free-spins").prop("checked");
        $(".buy-free-spin-checkbox").prop("checked", checked);
    }

    return {
        init: function () {
            $(document).on("change", "#select-all-buy-free-spins", toggleAllBuyFreeSpinCheckboxes);
        },
        openAddBuyFreeSpinSettingForm: openModal(addFreeSpinSettingForm),
        openUpdateBuyFreeSpinSettingForm: openModal(updateFreeSpinSettingForm),
        onCreateBuyFreeSpinSettingSuccess: onUpsert,
        onUpdateBuyFreeSpinSettingSuccess: onUpsert,
        onChangeSiteExclusions: onChangeSiteExclusions,
        setInitialSiteExclusionsFromModel: setInitialSiteExclusionsFromModel,
        onCreateBegin: onCreateBegin,
        onUpdateBegin: onUpdateBegin,
        toggleAllBuyFreeSpinCheckboxes: toggleAllBuyFreeSpinCheckboxes,
        enableBuyFreeSpins: function enableBuyFreeSpins(isEnabled) {
            // get all checked buy free spin checkboxes
            const checkedBuyFreeSpins = $(".buy-free-spin-checkbox:checked");
            const gameIds = $.makeArray(
                checkedBuyFreeSpins.map(function () {
                    return $(this).data("game-id");
                })
            );

            if (gameIds.length === 0) {
                onUpsert({ success: false, error: "Please select at least one game" });
                return;
            }

            // send request to enable buy free spins
            $.ajax({
                method: "POST",
                url: "/buyFreeSpin/enable",
                data: {
                    gameIds: gameIds,
                    isEnabled,
                },
                success: function (resp) {
                    onUpsert(resp);
                },
                error: function (xhr) {
                    console.error("Enable request failed", xhr);
                    onUpsert({ success: false, error: "Enable request failed" });
                },
            });
        },

        getCheckedGames: function () {
            const checkedBuyFreeSpins = $(".buy-free-spin-checkbox:checked");
            const gameIds = [];
            let gameChosen = "";

            checkedBuyFreeSpins.each(function () {
                const checkbox = $(this);
                const row = checkbox.closest("tr");
                const gameId = checkbox.data("game-id");
                const gameName = row.find("td:eq(1)").text(); // Get game name from the second column

                gameIds.push(gameId);
                gameChosen += `<li>${gameName}</li>`;
            });

            return {
                gameIds: gameIds,
                message: gameChosen,
            };
        },
        showEnableDisable: function (enabled) {
            const modal = $("#enableDisableModal");
            const label = enabled ? "enable" : "disable";

            const checkedGames = this.getCheckedGames();
            if (checkedGames.message.length > 0) {
                const message = `Are you sure you want to ${label.toLocaleUpperCase()} the following games<br /><ul>${
                    checkedGames.message
                }</ul>`;
                modal.find("#enableDisableContent").html(message);
                modal.find("#enableDisableFlag").val(enabled);
                modal.modal("show");
            } else {
                this.showAlert("You should choose at least one game.");
            }
        },
        submitEnableDisable: function () {
            const modal = $("#enableDisableModal");
            const enabled = modal.find("#enableDisableFlag").val();

            const checkedGames = this.getCheckedGames();
            modal.find("#confirmBtn").prop("disabled", true);
            $.ajax({
                method: "POST",
                url: `/buyFreeSpin/enable`,
                data: { gameIds: checkedGames.gameIds, isEnabled: enabled === "true" },
            }).done(function (updated) {
                if (updated) {
                    modal.find("#updateMsgSuccess").show();
                } else {
                    modal.find("#updateMsgFailed").show();
                }

                setTimeout(function () {
                    modal.find("#confirmBtn").prop("disabled", false);
                    modal.find("#updateMsgSuccess").hide();
                    modal.find("#updateMsgFailed").hide();
                    modal.modal("hide");
                    location.reload();
                }, 1000);
            });
        },

        showAlert: function (message) {
            const modal = $("#alertModal");
            modal.find("#alertContent").html(message);
            modal.modal("show");
        },
    };
})(jQuery);

$(document).ready(function () {
    buyFreeSpinSetting.init();
});
