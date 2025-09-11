/**
 * Game Operations JavaScript
 * Handles game configuration management, import/export, and UI interactions
 */

// Global variables
let importData = null;
const importModal = new bootstrap.Modal(document.getElementById("importConfirmModal"));
let currentSortColumn = -1;
let isAscending = true;

/**
 * Toggle all game checkboxes based on select all checkbox state
 */
function toggleAllGames() {
    const selectAllCheckbox = document.getElementById("selectAllGames");
    const gameCheckboxes = document.querySelectorAll(".game-checkbox");

    gameCheckboxes.forEach((checkbox) => {
        const row = checkbox.closest("tr");
        if (row.style.display !== "none") {
            // Only toggle visible rows
            checkbox.checked = selectAllCheckbox.checked;
        }
    });
}

/**
 * Handle collapse/expand functionality for individual game settings
 */
$(document).on("click", ".game-settings-summary", function () {
    const gameId = $(this).data("game-id");
    const $details = $(`.game-settings-details[data-game-id="${gameId}"]`);
    const $summary = $(this);
    const $toggleIcon = $summary.find(".toggle-icon");

    if ($details.is(":visible")) {
        // Collapse
        $details.slideUp(300);
        $toggleIcon.removeClass("icon-expand-less").addClass("icon-expand-more");
        $summary.removeClass("expanded");
    } else {
        // Expand
        $details.slideDown(300);
        $toggleIcon.removeClass("icon-expand-more").addClass("icon-expand-less");
        $summary.addClass("expanded");
    }
});

/**
 * Handle Expand All Settings button
 */
$(document).on("click", "#expandAllSettings", function () {
    $(".game-settings-details").each(function () {
        const gameId = $(this).data("game-id");
        const $summary = $(`.game-settings-summary[data-game-id="${gameId}"]`);
        const $toggleIcon = $summary.find(".toggle-icon");

        if (!$(this).is(":visible")) {
            $(this).slideDown(300);
            $toggleIcon.removeClass("icon-expand-more").addClass("icon-expand-less");
            $summary.addClass("expanded");
        }
    });
});

/**
 * Handle Collapse All Settings button
 */
$(document).on("click", "#collapseAllSettings", function () {
    $(".game-settings-details").each(function () {
        const gameId = $(this).data("game-id");
        const $summary = $(`.game-settings-summary[data-game-id="${gameId}"]`);
        const $toggleIcon = $summary.find(".toggle-icon");

        if ($(this).is(":visible")) {
            $(this).slideUp(300);
            $toggleIcon.removeClass("icon-expand-less").addClass("icon-expand-more");
            $summary.removeClass("expanded");
        }
    });
});

/**
 * Export selected game configurations
 */
function exportSelectedGameConfigs() {
    const selectedGameIds = Array.from(document.querySelectorAll(".game-checkbox:checked")).map((checkbox) => parseInt(checkbox.value));

    if (selectedGameIds.length === 0) {
        alert("Please select at least one game to export");
        return;
    }

    fetch("/api/OperateSettings/ExportGameConfigs", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            gameIds: selectedGameIds,
        }),
    })
        .then((response) => response.json())
        .then((data) => {
            const blob = new Blob([JSON.stringify(data, null, 2)], { type: "application/json" });
            const today = new Date();
            const date = today.toISOString().split("T")[0];

            const link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = `game-configs-${date}.json`;

            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        })
        .catch((error) => {
            console.error("Error:", error);
            alert("An error occurred while exporting Game configs");
        });
}

/**
 * Filter games based on search input
 */
function filterGames() {
    const filterValue = document.getElementById("gameNameFilter").value.toLowerCase();
    const tableBody = document.getElementById("gamesTableBody");
    const rows = tableBody.getElementsByTagName("tr");
    let visibleCheckedCount = 0;
    let visibleCount = 0;

    for (let row of rows) {
        const gameName = row.getElementsByTagName("td")[2].textContent.toLowerCase();
        if (gameName.includes(filterValue)) {
            row.style.display = "";
            visibleCount++;
            if (row.querySelector(".game-checkbox").checked) {
                visibleCheckedCount++;
            }
        } else {
            row.style.display = "none";
        }
    }

    // Update select all checkbox state based on visible rows
    const selectAllCheckbox = document.getElementById("selectAllGames");
    if (visibleCount > 0) {
        selectAllCheckbox.checked = visibleCount === visibleCheckedCount;
        selectAllCheckbox.indeterminate = visibleCheckedCount > 0 && visibleCount !== visibleCheckedCount;
    }
}

/**
 * Trigger file input for game config import
 */
function importGameConfigs() {
    document.getElementById("importFileInput").click();
}

/**
 * Handle file import and parse JSON
 */
function handleFileImport(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            try {
                importData = JSON.parse(e.target.result);
                showImportConfirmation(importData);
            } catch (error) {
                console.error("Error parsing JSON:", error);
                alert("Invalid JSON file");
            }
        };

        reader.readAsText(input.files[0]);
    }
}

/**
 * Show import confirmation modal with game list
 */
function showImportConfirmation(data) {
    const gamesList = document.getElementById("importGamesList");
    gamesList.innerHTML = "";

    // Assuming the data structure has games with id and name properties
    data.forEach((game, index) => {
        const listItem = document.createElement("div");
        listItem.className = "list-group-item";
        listItem.innerHTML = `
            <div class="form-check">
                <input type="checkbox" class="form-check-input import-game-checkbox" 
                       id="importGame${index}" value="${index}" checked>
                <label class="form-check-label" for="importGame${index}">
                    ${game.gameId} - ${game.name}
                </label>
            </div>
        `;
        gamesList.appendChild(listItem);
    });

    document.getElementById("selectAllImportGames").checked = true;
    importModal.show();
}

/**
 * Toggle all import game checkboxes
 */
function toggleAllImportGames() {
    const selectAllCheckbox = document.getElementById("selectAllImportGames");
    const gameCheckboxes = document.querySelectorAll(".import-game-checkbox");

    gameCheckboxes.forEach((checkbox) => {
        checkbox.checked = selectAllCheckbox.checked;
    });
}

/**
 * Close import modal and reset data
 */
function closeImportModal() {
    importModal.hide();
    // Clear the file input so the same file can be selected again
    document.getElementById("importFileInput").value = "";
    // Reset importData
    importData = null;
}

/**
 * Submit selected games for import
 */
function submitImport() {
    const selectedIndexes = Array.from(document.querySelectorAll(".import-game-checkbox:checked")).map((checkbox) =>
        parseInt(checkbox.value)
    );

    if (selectedIndexes.length === 0) {
        alert("Please select at least one game to import");
        return;
    }

    const selectedGames = selectedIndexes.map((index) => importData[index]);

    fetch("/api/OperateSettings/ImportGameConfigs", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            gameSettings: selectedGames,
        }),
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Import failed");
            }
            return response.json();
        })
        .then((data) => {
            closeImportModal();

            let successCount = 0;
            let failCount = 0;
            let alertMessage = "Import Result:\n\n";
            let successMessages = [];
            let failMessages = [];

            for (var gameId in data.importResult) {
                // Find the game in selectedGames array
                const game = selectedGames.find((g) => g.gameId.toString() === gameId.toString());
                const gameName = game ? `${game.gameId} - ${game.name}` : gameId;

                if (data.importResult[gameId]) {
                    successCount++;
                    successMessages.push(`✓ ${gameName}`);
                } else {
                    failCount++;
                    failMessages.push(`✗ ${gameName}`);
                }
            }

            alertMessage += `Success: ${successCount}, Failed: ${failCount}\n\n`;

            if (successMessages.length > 0) {
                alertMessage += "Successful:\n" + successMessages.join("\n") + "\n\n";
            }

            if (failMessages.length > 0) {
                alertMessage += "Failed:\n" + failMessages.join("\n");
            }

            alert(alertMessage);
        })
        .catch((error) => {
            console.error("Error:", error);
            alert("Failed to import game settings");
        });
}

/**
 * Sort table by column
 * @param {number} columnIndex - Index of column to sort by
 */
function sortTable(columnIndex) {
    const table = document.getElementById("gamesTableBody");
    const rows = Array.from(table.getElementsByTagName("tr"));
    const headers = document.querySelectorAll("th.sortable");

    // Reset all sort icons
    headers.forEach((header) => {
        header.querySelector(".sort-icon").className = "sort-icon";
    });

    // Update sort direction
    if (currentSortColumn === columnIndex) {
        isAscending = !isAscending;
    } else {
        isAscending = true;
        currentSortColumn = columnIndex;
    }

    // Update sort icon
    const currentHeader = headers[columnIndex - 1];
    currentHeader.querySelector(".sort-icon").className = "sort-icon " + (isAscending ? "sort-asc" : "sort-desc");

    // Sort rows
    rows.sort((a, b) => {
        const aValue = a.cells[columnIndex].textContent.trim();
        const bValue = b.cells[columnIndex].textContent.trim();

        // Handle numeric sorting for ID column
        if (columnIndex === 1) {
            return isAscending ? parseInt(aValue) - parseInt(bValue) : parseInt(bValue) - parseInt(aValue);
        }

        // String sorting for other columns
        return isAscending ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
    });

    // Reorder table rows
    rows.forEach((row) => table.appendChild(row));
}
