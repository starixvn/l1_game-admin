/**
 * Currency Group Game Operations JavaScript
 * Handles currency group game import/export and UI interactions
 */

// Global variables
let selectedData = [];

/**
 * Initialize currency group game operations functionality
 */
$(document).ready(function () {
    // Handle "Select All" checkbox
    $("#selectAll").change(function () {
        $(".export-select").prop("checked", $(this).prop("checked"));
    });

    // Update "Select All" state based on individual checkboxes
    $(".export-select").change(function () {
        var allChecked = $(".export-select:not(:checked)").length === 0;
        $("#selectAll").prop("checked", allChecked);
    });

    // Handle collapse/expand functionality for game settings
    $(".game-setting-summary").click(function () {
        var $summary = $(this);
        var $row = $summary.closest("tr");
        var $details = $row.find(".game-setting-details");

        if ($details.hasClass("show")) {
            // Collapse
            $details.removeClass("show").slideUp(300);
            $summary.removeClass("expanded");
        } else {
            // Expand
            $details.addClass("show").slideDown(300);
            $summary.addClass("expanded");
        }
    });

    // Handle Expand All button
    $("#expandAll").click(function () {
        $(".game-setting-details").each(function () {
            var groupId = $(this).data("group-id");
            var $summary = $('.game-setting-summary[data-group-id="' + groupId + '"]');
            if (!$(this).hasClass("show")) {
                $(this).addClass("show").slideDown(300);
                $summary.addClass("expanded");
            }
        });
    });

    // Handle Collapse All button
    $("#collapseAll").click(function () {
        $(".game-setting-details").each(function () {
            var groupId = $(this).data("group-id");
            var $summary = $('.game-setting-summary[data-group-id="' + groupId + '"]');
            if ($(this).hasClass("show")) {
                $(this).removeClass("show").slideUp(300);
                $summary.removeClass("expanded");
            }
        });
    });

    // Handle export selected button
    $("#exportSelected").click(function () {
        selectedData = [];

        $(".export-select:checked").each(function () {
            var $row = $(this).closest("tr");
            var groupId = $(this).data("group-id");
            var groupData = {
                operatorId: $row.find("td:eq(1)").text(),
                groupName: $row.find("td:eq(2)").text(),
                gameSettings: JSON.parse($row.find('.game-setting-details[data-group-id="' + groupId + '"] pre').text()),
            };
            selectedData.push(groupData);
        });

        if (selectedData.length === 0) {
            alert("Please select at least one currency group to export.");
            return;
        }

        // Generate modal content
        let modalContent =
            '<div class="table-responsive"><table class="table table-striped table-bordered"><thead><tr><th>Operator Id</th><th>Group Name</th><th><div class="d-flex justify-content-between align-items-center"><span>Game Settings</span><div class="btn-group btn-group-sm" role="group"><button id="expandAllGames" class="btn btn-outline-primary btn-sm" title="Expand all game items"><i class="icon-expand-more"></i><span>Expand All</span></button><button id="collapseAllGames" class="btn btn-outline-primary btn-sm" title="Collapse all game items"><i class="icon-expand-less"></i><span>Collapse All</span></button></div></div></th></tr></thead><tbody>';

        selectedData.forEach((item, index) => {
            modalContent += `<tr>
                <td class="align-top text-center">${item.operatorId}</td>
                <td class="align-top text-center">${item.groupName}</td>
                <td>
                    <div class="form-check">
                        <label class="form-check-label">
                            <input class="form-check-input game-setting-select-all" type="checkbox" data-group-index="${index}" checked="checked">
                            Select All
                        </label>
                    </div>
                    ${item.gameSettings
                        .map(
                            (game, gameIndex) => `
                        <div class="game-item-container" style="margin-top: 10px;">
                            <div class="game-item-summary" data-group-index="${index}" data-game-index="${gameIndex}" style="background-color: #f8f9fa; border: 1px solid #dee2e6; border-radius: 4px; padding: 6px; padding-left: 24px; cursor: pointer; margin-bottom: 5px;">
                                <div style="display: flex; justify-content: space-between; align-items: center;">
                                    <div style="display: flex; align-items: center;">
                                        <input class="form-check-input game-setting-checkbox" type="checkbox" value="${gameIndex}" data-group-index="${index}" data-game-index="${gameIndex}" checked style="margin-right: 8px;">
                                        <span style="font-weight: bold; color: #495057;">Game: ${game.GameName || "Unknown"}</span>
                                        <span style="background-color: #007bff; color: white; border-radius: 12px; padding: 2px 8px; font-size: 11px; margin-left: 8px;">ID: ${
                                            game.GameId
                                        }</span>
                                    </div>
                                    <i class="icon-expand-more toggle-icon" style="font-size: 16px; color: #6c757d;"></i>
                                </div>
                                <div style="margin-top: 5px; font-size: 12px; color: #6c757d;">
                                    <span>Enabled: ${game.Enabled ? "Yes" : "No"}</span>
                                    <span> | Min Bet: ${game.MinBet || 0}</span>
                                    <span> | Max Bet: ${game.MaxBet || 0}</span>
                                    ${game.MaxWinPerRound ? ` | Max Win: ${game.MaxWinPerRound}` : ""}
                                </div>
                            </div>
                            <div class="game-item-details" data-group-index="${index}" data-game-index="${gameIndex}" style="display: none; background-color: #ffffff; border: 1px solid #dee2e6; border-radius: 4px; padding: 15px; margin-top: 5px;">
                                <pre style="margin: 0; font-size: 11px; max-height: 200px; overflow-y: auto;">${JSON.stringify(
                                    game,
                                    null,
                                    2
                                )}</pre>
                            </div>
                        </div>
                    `
                        )
                        .join("")}
                </td>
            </tr>`;
        });
        modalContent += "</tbody></table></div>";

        // Handle "Select All" checkbox for game settings
        $(document).on("change", ".game-setting-select-all", function () {
            var groupIndex = $(this).data("group-index");
            $(`.game-setting-checkbox[data-group-index="${groupIndex}"]`).prop("checked", $(this).prop("checked"));
        });

        // Update "Select All" state based on individual game setting checkboxes
        $(document).on("change", ".game-setting-checkbox", function () {
            var groupIndex = $(this).data("group-index");
            var allChecked = $(`.game-setting-checkbox[data-group-index="${groupIndex}"]:not(:checked)`).length === 0;
            $(`.game-setting-select-all[data-group-index="${groupIndex}"]`).prop("checked", allChecked);
        });

        // Handle collapse/expand functionality for individual game items in export modal
        $(document).on("click", ".game-item-summary", function (e) {
            // Prevent checkbox click from triggering collapse/expand
            if (e.target.type === "checkbox") {
                return;
            }

            var groupIndex = $(this).data("group-index");
            var gameIndex = $(this).data("game-index");
            var $details = $(`.game-item-details[data-group-index="${groupIndex}"][data-game-index="${gameIndex}"]`);
            var $summary = $(this);
            var $toggleIcon = $summary.find(".toggle-icon");

            if ($details.is(":visible")) {
                // Collapse
                $details.slideUp(300);
                $toggleIcon.removeClass("icon-expand-less").addClass("icon-expand-more");
            } else {
                // Expand
                $details.slideDown(300);
                $toggleIcon.removeClass("icon-expand-more").addClass("icon-expand-less");
            }
        });

        // Handle Expand All Games button (for export modal)
        $(document).on("click", "#expandAllGames", function () {
            $(".game-item-details").each(function () {
                if (!$(this).is(":visible")) {
                    var $summary = $(this).siblings(".game-item-summary");
                    var $toggleIcon = $summary.find(".toggle-icon");
                    $(this).slideDown(300);
                    $toggleIcon.removeClass("icon-expand-more").addClass("icon-expand-less");
                }
            });
        });

        // Handle Collapse All Games button (for export modal)
        $(document).on("click", "#collapseAllGames", function () {
            $(".game-item-details").each(function () {
                if ($(this).is(":visible")) {
                    var $summary = $(this).siblings(".game-item-summary");
                    var $toggleIcon = $summary.find(".toggle-icon");
                    $(this).slideUp(300);
                    $toggleIcon.removeClass("icon-expand-less").addClass("icon-expand-more");
                }
            });
        });

        $("#exportDataContainer").html(modalContent);
        $("#exportModal").modal("show");
    });

    // Handle confirm export button
    $("#confirmExport").click(function () {
        var finalExportData = selectedData.map((group, groupIndex) => {
            var selectedGameSettings = [];
            $(`.game-setting-checkbox[data-group-index="${groupIndex}"]:checked`).each(function () {
                var gameIndex = $(this).data("game-index");
                selectedGameSettings.push(group.gameSettings[gameIndex]);
            });
            return {
                operatorId: group.operatorId,
                groupName: group.groupName,
                gameSettings: selectedGameSettings,
            };
        });

        // Create JSON file and trigger download
        var jsonContent = JSON.stringify(finalExportData, null, 2);
        var blob = new Blob([jsonContent], { type: "application/json" });
        var url = window.URL.createObjectURL(blob);
        var timestamp = new Date().toISOString().replace(/[:.]/g, "-");
        var a = document.createElement("a");
        a.href = url;
        a.download = `currency-group-games-${timestamp}.json`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        $("#exportModal").modal("hide");
    });

    // Handle import button click
    $("#importCurrencyGroupGames").click(function () {
        $("#fileInput").click();
    });

    let importDataToProcess = null;

    // Handle file selection
    $("#fileInput").change(function (e) {
        if (!e.target.files.length) return;

        const file = e.target.files[0];
        const reader = new FileReader();

        reader.onload = function (event) {
            try {
                const importData = JSON.parse(event.target.result);

                // Basic validation
                if (!Array.isArray(importData)) {
                    throw new Error("Invalid import file format. Expected an array of currency group games.");
                }

                // Validate each entry
                importData.forEach((item) => {
                    if (!item.operatorId || !item.groupName || !Array.isArray(item.gameSettings)) {
                        throw new Error("Invalid data structure in import file.");
                    }
                });

                // Store data for later use
                importDataToProcess = importData;

                // Generate modal content
                let modalContent = "";
                importData.forEach((item, index) => {
                    modalContent += `
                        <div class="import-item mb-4">
                            <h6>Currency Group ${index + 1}</h6>
                            <div class="form-group row">
                                <label class="col-sm-1 col-form-label">Operator ID:</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control operator-id" 
                                           value="${item.operatorId}" 
                                           data-index="${index}">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-1 col-form-label">Group Name:</label>
                                <div class="col-sm-9">
                                    <input type="text" class="form-control group-name" 
                                           value="${item.groupName}" 
                                           data-index="${index}">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-1 col-form-label">Games:</label>
                                <div class="col-sm-9">
                                    <button class="btn btn-link p-0" type="button" 
                                            data-toggle="collapse" 
                                            data-target="#gameDetails${index}" 
                                            aria-expanded="false">
                                        Show ${item.gameSettings.length} games included
                                    </button>
                                    <div class="collapse mt-2" id="gameDetails${index}">
                                        <div class="table-responsive">
                                            <table class="table table-sm table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>Game ID</th>
                                                        <th>Game Name</th>
                                                        <th>Settings</td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    ${item.gameSettings
                                                        .map(
                                                            (game) => `
                                                        <tr>
                                                            <td>${game.GameId}</td>
                                                            <td>${game.GameName}</td>
                                                            <td>${JSON.stringify(game)}</td>
                                                        </tr>
                                                    `
                                                        )
                                                        ?.join("")}
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr>
                        </div>`;
                });

                $("#importDataContainer").html(modalContent);
                $("#importModal").modal("show");
            } catch (error) {
                alert("Error processing import file: " + error.message);
            }

            // Clear the file input
            e.target.value = "";
        };

        reader.readAsText(file);
    });

    // Handle changes to operator ID and group name
    $(document).on("change", ".operator-id, .group-name", function () {
        const index = $(this).data("index");
        const isOperatorId = $(this).hasClass("operator-id");

        if (isOperatorId) {
            importDataToProcess[index].operatorId = $(this).val();
        } else {
            importDataToProcess[index].groupName = $(this).val();
        }
    });

    // Handle confirm import button
    $("#confirmImport").click(async function () {
        if (!importDataToProcess) {
            alert("No data to import");
            return;
        }

        try {
            const response = await fetch("/api/OperateSettings/ImportCurrencyGroupGames", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    currencyGroupGameImportModels: importDataToProcess,
                }),
            });

            const result = await response.json();

            $("#importModal").modal("hide");

            if (response.ok && result.success) {
                alert("Import successful!");
                //location.reload(); // Refresh the page to show updated data
            } else {
                alert("Import failed: " + (result.message || "Unknown error"));
            }
        } catch (error) {
            $("#importModal").modal("hide");
            alert("Error during import: " + error.message);
        }
    });
});
