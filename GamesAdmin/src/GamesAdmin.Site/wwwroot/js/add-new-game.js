$(document).ready(function () {
    // Constants to avoid code duplication
    const GAME_TYPE_AUTO_RNG_INDEX = 10;
    const GAME_TYPE_MARBLE_PRODUCT = 13;
    const GAME_TYPE_PLINKO = 11;
    const SELECTORS = {
        TYPE: "#Type",
        NAME_TEXTBOX: "#name-textbox",
        NAME_SELECT: "#name-select",
        NAME_HELPER_TEXT: "#name-helper-text",
        GAME_ID_CONTAINER: "#game-id-container",
        CHOICES_SECTION: "#choices-section",
        NAME_HIDDEN: "#name-hidden",
        USE_DIFFERENT_ODDS: "#use-different-odds",
        USE_DIFFERENT_ODDS_CONTAINER: "#use-different-odds-container",
        ODDS_SECTION: "#odds-section",
        ODDS_COLUMN: ".odds-column",
        CHOICES_CONTAINER: "#choices-container",
        ODDS_CONTAINER: "#odds-container",
    };

    const MESSAGES = {
        AUTO_RNG_HELPER: "The game name should be provided by Dev",
        PREDEFINED_HELPER: "Select from predefined game names",
        CHOICES_VALIDATION_ERROR: "At least one choice must be configured.",
    };

    // Function to toggle Name field based on GameType
    function toggleNameField() {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isNewGame = selectedType >= 10;

        if (isNewGame) {
            showNewGameFields();
            updateHiddenField();
            return;
        }

        showPredefinedGameFields();
        updateHiddenField();
    }

    function showNewGameFields() {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isAutoRNGIndex = selectedType === GAME_TYPE_AUTO_RNG_INDEX;
        var isMarbleProduct = selectedType === GAME_TYPE_MARBLE_PRODUCT;

        $(SELECTORS.NAME_TEXTBOX).show();
        $(SELECTORS.NAME_SELECT).hide().prop("disabled", true);
        $(SELECTORS.NAME_HELPER_TEXT).text(MESSAGES.AUTO_RNG_HELPER);
        $(SELECTORS.GAME_ID_CONTAINER).show();

        if (isAutoRNGIndex || isMarbleProduct) {
            $(SELECTORS.CHOICES_SECTION).show();
        } else {
            $(SELECTORS.CHOICES_SECTION).hide();
        }

        $(SELECTORS.NAME_TEXTBOX).val("");
    }

    function showPredefinedGameFields() {
        $(SELECTORS.NAME_TEXTBOX).hide().prop("disabled", true);
        $(SELECTORS.NAME_SELECT).show();
        $(SELECTORS.NAME_HELPER_TEXT).text(MESSAGES.PREDEFINED_HELPER);
        $(SELECTORS.GAME_ID_CONTAINER).hide();
        $(SELECTORS.CHOICES_SECTION).hide();

        if ($(SELECTORS.NAME_TEXTBOX).val()) {
            $(SELECTORS.NAME_SELECT).val($(SELECTORS.NAME_TEXTBOX).val());
        }
    }

    // Function to update the hidden field with the current visible field's value
    function updateHiddenField() {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isNewGame = selectedType >= GAME_TYPE_AUTO_RNG_INDEX;

        if (isNewGame) {
            $(SELECTORS.NAME_HIDDEN).val($(SELECTORS.NAME_TEXTBOX).val());
            $(SELECTORS.NAME_TEXTBOX).prop("disabled", false);
        } else {
            $(SELECTORS.NAME_HIDDEN).val($(SELECTORS.NAME_SELECT).val());
            $(SELECTORS.NAME_SELECT).prop("disabled", false);
        }
    }

    // Initial setup
    toggleNameField();
    updateOddsColumnVisibility();

    // Clear textbox on page load if type is AutoRNGIndex or MarbleProduct
    var initialType = parseInt($(SELECTORS.TYPE).val());
    if (initialType === GAME_TYPE_AUTO_RNG_INDEX || initialType === GAME_TYPE_MARBLE_PRODUCT) {
        $(SELECTORS.NAME_TEXTBOX).val("");
    }

    // Handle GameType change
    $(SELECTORS.TYPE).on("change", function () {
        toggleNameField();
        updateOddsColumnVisibility();
        validateGameIdField();
    });

    // Handle input changes to update hidden field
    $(SELECTORS.NAME_TEXTBOX).on("input", updateHiddenField);
    $(SELECTORS.NAME_SELECT).on("change", updateHiddenField);

    // Handle form submission to ensure hidden field is updated
    $("form").on("submit", function (e) {
        updateHiddenField();

        if (!validateNameField() || !validateGameIdField()) {
            e.preventDefault();
            return false;
        }
    });

    // Choice section functionality
    var choiceRowIndex = 1;

    // Add new choice row
    $("#add-choice-row").on("click", function () {
        addChoiceRow();
    });

    function addChoiceRow() {
        var newRow = createChoiceRowHtml(choiceRowIndex);
        $(SELECTORS.CHOICES_CONTAINER).append(newRow);

        // Show/hide odds column based on game type and "Use different odds" checkbox
        updateOddsColumnVisibility();

        choiceRowIndex++;
        updateRemoveButtons();
    }

    // Function to update odds column visibility based on game type
    function updateOddsColumnVisibility() {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isAutoRNGIndex = selectedType === GAME_TYPE_AUTO_RNG_INDEX;
        var isMarbleProduct = selectedType === GAME_TYPE_MARBLE_PRODUCT;
        var isUseDifferentOdds = $(SELECTORS.USE_DIFFERENT_ODDS).is(":checked");

        if (isMarbleProduct) {
            $(SELECTORS.ODDS_COLUMN).hide();
            $(SELECTORS.USE_DIFFERENT_ODDS_CONTAINER).hide();
            $(SELECTORS.ODDS_SECTION).hide();
        } else if (isAutoRNGIndex) {
            // AutoRNGIndex - show "Use different odds" checkbox and show/hide odds column based on checkbox
            $(SELECTORS.USE_DIFFERENT_ODDS_CONTAINER).show();
            if (isUseDifferentOdds) {
                $(SELECTORS.ODDS_COLUMN).hide();
            } else {
                $(SELECTORS.ODDS_COLUMN).show();
            }
        } else {
            // Other game types - hide both
            $(SELECTORS.ODDS_COLUMN).hide();
            $(SELECTORS.USE_DIFFERENT_ODDS_CONTAINER).hide();
        }
    }

    function createChoiceRowHtml(index) {
        return `
            <div class="form-group row choice-row">
                <div class="col-lg-4">
                    <label class="control-label">Key</label>
                    <input type="text" name="Choices[${index}].Key" class="form-control choice-key" placeholder="e.g., high, low, red, black" required />
                    <span class="text-danger field-validation-error" data-valmsg-for="Choices[${index}].Key" data-valmsg-replace="true"></span>
                </div>
                <div class="col-lg-4">
                    <label class="control-label">Name</label>
                    <input type="text" name="Choices[${index}].Name" class="form-control choice-name" placeholder="e.g., High Bet, Low Bet, Red, Black" required />
                    <span class="text-danger field-validation-error" data-valmsg-for="Choices[${index}].Name" data-valmsg-replace="true"></span>
                </div>
                <div class="col-lg-3 odds-column">
                    <label class="control-label">Odds</label>
                    <input type="number" name="Choices[${index}].Odds" class="form-control choice-odds" step="0.01" min="0.01" placeholder="e.g., 2.0, 1.5" required />
                    <span class="text-danger field-validation-error" data-valmsg-for="Choices[${index}].Odds" data-valmsg-replace="true"></span>
                </div>
                <div class="col-lg-1">
                    <label class="control-label">&nbsp;</label>
                    <button type="button" class="btn btn-danger btn-sm remove-choice-row">
                        <i class="fa fa-trash"></i> Remove
                    </button>
                </div>
            </div>
        `;
    }

    // Remove choice row (delegate event for dynamically added elements)
    $(document).on("click", ".remove-choice-row", function () {
        $(this).closest(".choice-row").remove();
        reindexChoiceRows();
        updateRemoveButtons();
    });

    // Function to reindex choice rows after removal
    function reindexChoiceRows() {
        var newIndex = 0;
        $(SELECTORS.CHOICES_CONTAINER + " .choice-row").each(function () {
            reindexChoiceRowInputs($(this), newIndex);
            newIndex++;
        });
        choiceRowIndex = newIndex;
    }

    function reindexChoiceRowInputs(row, index) {
        row.find("input[name^='Choices[']").each(function () {
            var oldName = $(this).attr("name");
            var newName = oldName.replace(/Choices\[\d+\]/, `Choices[${index}]`);
            $(this).attr("name", newName);
        });

        row.find("span[data-valmsg-for^='Choices[']").each(function () {
            var oldFor = $(this).attr("data-valmsg-for");
            var newFor = oldFor.replace(/Choices\[\d+\]/, `Choices[${index}]`);
            $(this).attr("data-valmsg-for", newFor);
        });
    }

    // Function to update remove buttons visibility
    function updateRemoveButtons() {
        updateChoiceRemoveButtons();
        updateOddsRemoveButtons();
    }

    function updateChoiceRemoveButtons() {
        var choiceRows = $(SELECTORS.CHOICES_CONTAINER + " .choice-row");
        if (choiceRows.length > 1) {
            choiceRows.find(".remove-choice-row").show();
        } else {
            choiceRows.find(".remove-choice-row").hide();
        }
    }

    function updateOddsRemoveButtons() {
        var oddsRows = $(".odds-row");
        if (oddsRows.length > 1) {
            oddsRows.find(".remove-odds-row").show();
        } else {
            oddsRows.find(".remove-odds-row").hide();
        }
    }

    // Initialize remove buttons
    updateRemoveButtons();

    // Initialize odds setting visibility based on checkbox state
    function initializeOddsSettingVisibility() {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isAutoRNGIndex = selectedType === GAME_TYPE_AUTO_RNG_INDEX;
        var isMarbleProduct = selectedType === GAME_TYPE_MARBLE_PRODUCT;
        var isUseDifferentOdds = $(SELECTORS.USE_DIFFERENT_ODDS).is(":checked");

        if (isAutoRNGIndex) {
            if (isUseDifferentOdds) {
                $(SELECTORS.ODDS_SECTION).show();
            } else {
                $(SELECTORS.ODDS_SECTION).hide();
            }
        } else if (isMarbleProduct) {
            $(SELECTORS.ODDS_SECTION).hide();
        } else {
            // Non-AutoRNGIndex/MarbleProduct games - always hide odds section
            $(SELECTORS.ODDS_SECTION).hide();
        }

        // Update odds column visibility (handles all game types)
        updateOddsColumnVisibility();
    }

    // Initialize on page load
    initializeOddsSettingVisibility();

    // Handle "Use different Odds setting" checkbox
    $(SELECTORS.USE_DIFFERENT_ODDS).on("change", function () {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isAutoRNGIndex = selectedType === GAME_TYPE_AUTO_RNG_INDEX;
        var isChecked = $(this).is(":checked");

        // Only handle for AutoRNGIndex
        if (isAutoRNGIndex) {
            if (isChecked) {
                $(SELECTORS.ODDS_SECTION).show();
            } else {
                $(SELECTORS.ODDS_SECTION).hide();
            }
        }

        // Update odds column visibility
        updateOddsColumnVisibility();
    });

    // Odds section functionality
    var oddsRowIndex = 1;

    // Add new odds row
    $("#add-odds-row").on("click", function () {
        addOddsRow();
    });

    function addOddsRow() {
        var newRow = createOddsRowHtml(oddsRowIndex);
        $(SELECTORS.ODDS_CONTAINER).append(newRow);
        oddsRowIndex++;
        updateRemoveButtons();
    }

    function createOddsRowHtml(index) {
        return `
            <div class="form-group row odds-row">
                <div class="col-lg-4">
                    <label class="control-label">Key</label>
                    <input type="text" name="OddsKeys[${index}]" class="form-control odds-key" placeholder="e.g., high, low, red, black" />
                </div>
                <div class="col-lg-4">
                    <label class="control-label">Name</label>
                    <input type="text" name="OddsDisplayNames[${index}]" class="form-control odds-name" placeholder="e.g., High Bet, Low Bet, Red, Black" />
                </div>
                <div class="col-lg-3">
                    <label class="control-label">Odds</label>
                    <input type="number" name="OddsValues[${index}]" class="form-control odds-value" step="0.01" min="0.1" placeholder="e.g., 2.0, 1.5" />
                </div>
                <div class="col-lg-1">
                    <label class="control-label">&nbsp;</label>
                    <button type="button" class="btn btn-danger btn-sm remove-odds-row">
                        <i class="fa fa-trash"></i> Remove
                    </button>
                </div>
            </div>
        `;
    }

    // Remove odds row (delegate event for dynamically added elements)
    $(document).on("click", ".remove-odds-row", function () {
        $(this).closest(".odds-row").remove();
        reindexOddsRows();
        updateRemoveButtons();
    });

    // Function to reindex odds rows
    function reindexOddsRows() {
        $(".odds-row").each(function (index) {
            reindexOddsRowInputs($(this), index);
        });
        oddsRowIndex = $(".odds-row").length;
    }

    function reindexOddsRowInputs(row, index) {
        var inputTypes = ["OddsKeys", "OddsDisplayNames", "OddsValues"];
        inputTypes.forEach(function (inputType) {
            row.find(`input[name*='${inputType}[']`).each(function () {
                var name = $(this).attr("name");
                var newName = name.replace(new RegExp(`${inputType}\\[\\d+\\]`), `${inputType}[${index}]`);
                $(this).attr("name", newName);
            });
        });
    }

    function isValidGameName(name) {
        return /^[a-z0-9]+$/.test(name);
    }

    // Function to validate name field
    window.validateNameField = function () {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var nameValue = "";
        var isValid = true;
        var errorText = "The game name must be written in lowercase letters without any spaces.";

        if (selectedType === GAME_TYPE_AUTO_RNG_INDEX || selectedType === GAME_TYPE_MARBLE_PRODUCT) {
            nameValue = $(SELECTORS.NAME_TEXTBOX).val().trim();
            if (!nameValue) {
                $("#name-validation-error").show();
                isValid = false;
            } else {
                $("#name-validation-error").hide();
            }
        } else if (selectedType === GAME_TYPE_PLINKO) {
            if (!isValidGameName($(SELECTORS.NAME_TEXTBOX).val().trim())) {
                $("#name-validation-error").text(errorText);
                $("#name-validation-error").show();
                isValid = false;
            } else {
                $("#name-validation-error").hide();
            }
        } else {
            nameValue = $(SELECTORS.NAME_SELECT).val();
            if (!nameValue) {
                isValid = false;
            } else {
                $("#name-validation-error").hide();
            }
        }

        return isValid;
    };

    // Function to validate Game ID field
    window.validateGameIdField = function () {
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var gameIdValue = $("#game-id-input").val();
        var isValid = true;

        if (selectedType === GAME_TYPE_AUTO_RNG_INDEX || selectedType === GAME_TYPE_MARBLE_PRODUCT) {
            if (!gameIdValue || parseInt(gameIdValue) <= 0) {
                $("#game-id-validation-error").show();
                isValid = false;
            } else {
                $("#game-id-validation-error").hide();
            }
        } else {
            $("#game-id-validation-error").hide();
        }

        return isValid;
    };

    // Function to validate choices configuration
    window.validateChoicesConfiguration = function () {
        var isValid = true;
        var selectedType = parseInt($(SELECTORS.TYPE).val());

        if (selectedType === GAME_TYPE_AUTO_RNG_INDEX || selectedType === GAME_TYPE_MARBLE_PRODUCT) {
            isValid = validateChoicesForAutoRNG();

            // ✅ Also validate separate odds section if "Use different odds" is checked (only for AutoRNGIndex)
            if (selectedType === GAME_TYPE_AUTO_RNG_INDEX) {
                var isUseDifferentOdds = $(SELECTORS.USE_DIFFERENT_ODDS).is(":checked");
                if (isUseDifferentOdds) {
                    isValid = validateOddsSection() && isValid;
                }
            }
        }

        return isValid;
    };

    function validateChoicesForAutoRNG() {
        var isValid = true;
        var choiceRows = $(".choice-row");
        var hasValidChoices = false;
        var selectedType = parseInt($(SELECTORS.TYPE).val());
        var isAutoRNGIndex = selectedType === GAME_TYPE_AUTO_RNG_INDEX;
        var isUseDifferentOdds = $(SELECTORS.USE_DIFFERENT_ODDS).is(":checked");
        // Validate odds for: AutoRNGIndex (without separate odds) only
        var shouldValidateOdds = isAutoRNGIndex && !isUseDifferentOdds;

        choiceRows.each(function () {
            var row = $(this);
            var key = row.find(".choice-key").val().trim();
            var name = row.find(".choice-name").val().trim();
            var odds = row.find(".choice-odds").val();

            // ✅ Check if we have valid choices based on game type and checkbox state
            if (shouldValidateOdds) {
                // AutoRNGIndex (without separate odds) - validate Key, Name, and Odds
                if (key && name && odds) {
                    hasValidChoices = true;
                }
            } else {
                // AutoRNGIndex with separate odds - only validate Key and Name
                if (key && name) {
                    hasValidChoices = true;
                }
            }

            row.find(".field-validation-error").text("");
            isValid = validateChoiceRow(row, key, name, odds, shouldValidateOdds) && isValid;
        });

        if (!hasValidChoices) {
            showChoicesValidationError();
            isValid = false;
        } else {
            $("#choices-validation-error").remove();
        }

        return isValid;
    }

    function validateChoiceRow(row, key, name, odds, shouldValidateOdds) {
        var isValid = true;

        if (!key) {
            row.find("[data-valmsg-for*='.Key']").text("Key is required");
            isValid = false;
        }
        if (!name) {
            row.find("[data-valmsg-for*='.Name']").text("Name is required");
            isValid = false;
        }

        // ✅ Validate odds if required (AutoRNGIndex without separate odds only)
        if (shouldValidateOdds) {
            if (!odds || parseFloat(odds) < 0.01) {
                row.find("[data-valmsg-for*='.Odds']").text("Odds must be at least 0.01");
                isValid = false;
            }
        }

        return isValid;
    }

    function showChoicesValidationError() {
        if ($("#choices-validation-error").length === 0) {
            $(SELECTORS.CHOICES_SECTION).append(
                `<div id="choices-validation-error" class="text-danger">${MESSAGES.CHOICES_VALIDATION_ERROR}</div>`
            );
        }
    }

    // ✅ Function to validate the separate odds section when "Use different odds" is checked
    function validateOddsSection() {
        var isValid = true;
        var oddsRows = $(".odds-row");
        var hasValidOdds = false;

        oddsRows.each(function () {
            var row = $(this);
            var key = row.find(".odds-key").val().trim();
            var name = row.find(".odds-name").val().trim();
            var value = row.find(".odds-value").val();

            if (key && name && value) {
                hasValidOdds = true;
            }

            // Validate each odds row
            if (!key) {
                row.find(".odds-key").addClass("is-invalid");
                isValid = false;
            } else {
                row.find(".odds-key").removeClass("is-invalid");
            }

            if (!name) {
                row.find(".odds-name").addClass("is-invalid");
                isValid = false;
            } else {
                row.find(".odds-name").removeClass("is-invalid");
            }

            if (!value || parseFloat(value) < 0.01) {
                row.find(".odds-value").addClass("is-invalid");
                isValid = false;
            } else {
                row.find(".odds-value").removeClass("is-invalid");
            }
        });

        if (!hasValidOdds) {
            showOddsValidationError();
            isValid = false;
        } else {
            $("#odds-validation-error").remove();
        }

        return isValid;
    }

    function showOddsValidationError() {
        if ($("#odds-validation-error").length === 0) {
            $(SELECTORS.ODDS_SECTION).append(
                `<div id="odds-validation-error" class="text-danger">At least one odds configuration must be provided when using different odds.</div>`
            );
        }
    }

    // Add form validation before submission
    $("form").on("submit", function (e) {
        trimChoiceInputs();
        trimOddsInputs();

        if (!validateNameField() || !validateGameIdField() || !validateChoicesConfiguration()) {
            e.preventDefault();
            console.log("Validation failed");
            return false;
        }
    });

    function trimChoiceInputs() {
        $(".choice-key, .choice-name").each(function () {
            $(this).val($(this).val().trim());
        });
    }

    function trimOddsInputs() {
        $(".odds-key, .odds-name").each(function () {
            $(this).val($(this).val().trim());
        });
    }
});
