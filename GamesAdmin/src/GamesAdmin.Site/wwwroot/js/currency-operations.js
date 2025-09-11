/**
 * Currency Operations JavaScript
 * Handles currency import/export and UI interactions
 */

// Global variables
let importResultModal = null;

/**
 * Close import modal
 */
function closeImportModal() {
    importResultModal.hide();
}

/**
 * Format date for file naming
 * @param {Date} date - Date to format
 * @returns {string} Formatted date string
 */
function formatDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${year}${month}${day}_${hours}${minutes}`;
}

/**
 * Update export button state based on selected operators
 * @param {jQuery} $exportButton - Export button element
 * @param {jQuery} $operatorCheckboxes - Operator checkboxes
 */
function updateExportButton($exportButton, $operatorCheckboxes) {
    const hasSelected = $operatorCheckboxes.filter(":checked").length > 0;
    $exportButton.prop("disabled", !hasSelected);
}

/**
 * Validate import data structure
 * @param {Array} data - Import data to validate
 * @throws {Error} If data is invalid
 */
function validateImportData(data) {
    if (!Array.isArray(data)) {
        throw new Error("Import data must be an array");
    }

    for (const item of data) {
        if (!item.operator || !item.currencies) {
            throw new Error("Each item must have operator and currencies properties");
        }
        if (!Array.isArray(item.currencies)) {
            throw new Error("Currencies must be an array");
        }
    }
}

/**
 * Show import result modal with message
 * @param {string} message - Message to display
 */
function showImportResult(message) {
    const $messageContainer = $("#importResultMessage");
    $messageContainer.text(message);

    if (!importResultModal) {
        importResultModal = new bootstrap.Modal(document.getElementById("importResultModal"), {
            keyboard: true,
            backdrop: true,
        });
    }
    importResultModal.show();
}

/**
 * Initialize currency operations functionality
 */
$(document).ready(function () {
    const $selectAll = $("#selectAll");
    const $exportButton = $("#exportSelected");
    const $importButton = $("#importCurrencies");
    const $operatorCheckboxes = $(".operator-checkbox");

    // Create hidden file input
    const $fileInput = $('<input type="file" accept=".json" style="display: none">');
    $("body").append($fileInput);

    // Handle select all checkbox
    $selectAll.change(function () {
        const isChecked = $(this).prop("checked");
        $operatorCheckboxes.prop("checked", isChecked);
        updateExportButton($exportButton, $operatorCheckboxes);
    });

    // Handle individual operator checkboxes
    $operatorCheckboxes.change(function () {
        const allChecked = $operatorCheckboxes.length === $operatorCheckboxes.filter(":checked").length;
        $selectAll.prop("checked", allChecked);
        updateExportButton($exportButton, $operatorCheckboxes);
    });

    // Handle export button click
    $exportButton.click(function () {
        const selectedOperators = $operatorCheckboxes
            .filter(":checked")
            .map(function () {
                return $(this).data("operator");
            })
            .get();

        if (selectedOperators.length === 0) {
            alert("Please select at least one operator to export.");
            return;
        }

        const exportData = selectedOperators.map((operatorId) => {
            const $row = $(`.operator-checkbox[data-operator="${operatorId}"]`).closest("tr");
            const currencyJson = $row.find(".currency-json").text();
            return {
                operator: operatorId,
                currencies: JSON.parse(currencyJson),
            };
        });

        const now = new Date();
        const blob = new Blob([JSON.stringify(exportData, null, 2)], { type: "application/json" });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = `operator-currencies_${formatDate(now)}.json`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
    });

    // Handle import button click
    $importButton.click(function () {
        $fileInput.click();
    });

    // Handle file input change
    $fileInput.change(function (e) {
        const file = e.target.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = async function (e) {
            try {
                const importData = JSON.parse(e.target.result);
                validateImportData(importData);

                const response = await fetch("/api/OperateSettings/ImportCurrencies", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                    },
                    body: JSON.stringify({
                        operatorCurrencies: importData,
                    }),
                });

                if (!response.ok) {
                    throw new Error(`Import failed: ${response.statusText}`);
                }

                const result = await response.json();
                if (typeof result === "object" && Object.keys(result).length > 0) {
                    let message = "<div>Import Results:</div><br>";
                    for (const [operator, details] of Object.entries(result)) {
                        message += `<div>${operator}:</div>`;
                        message += `<div>${details.message}</div>`;
                        message += "<br>";
                    }
                    const $messageContainer = $("#importResultMessage");
                    $messageContainer.html(message);

                    if (!importResultModal) {
                        importResultModal = new bootstrap.Modal(document.getElementById("importResultModal"), {
                            keyboard: true,
                            backdrop: true,
                        });
                    }
                    importResultModal.show();
                } else {
                    showImportResult("Currencies imported successfully");
                }
            } catch (error) {
                showImportResult(`Error importing currencies: ${error.message}`);
            }

            $fileInput.val("");
        };

        reader.onerror = function () {
            showImportResult("Error reading file");
            $fileInput.val("");
        };

        reader.readAsText(file);
    });
});
