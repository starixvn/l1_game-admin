var promotion = (function ($) {
    let promotionElement = {
        modal: "#promotion-modal",
        filterForm: "#promotion-form",
        formAdd: '#add-new-promotion-form',
        formAddTournament: '#add-new-tournament-form',
        odds: '#promotion-game-odds',
        selectGameName: '#promotionGameName',
        formUpdate: '#update-promotion-form',
        modalDelete: '#promotion-modal-delete',
        deleteId: '#delete-promotion-id',
        resultLabel: '#result-label',
        formTournamentSettings: "#update-currency-group-setting-form",
        formEditTourment: '#edit-tournament-form'
    };
    let promotionMessage = {
        createSuccess: 'Add Successfully!',
        createUnsuccess: 'Cannot Add Promotion!',
        updateSuccess: 'Edit Successfully!',
        updateUnsuccess: 'Cannot Edit Promotion!',
        deleteUnsuccess: 'Cannot Delete Promotion',
        messageUpdateSuccess: "Updated successfully",
        messageUpdateUnsuccess: "Error"
    };

    function openModal(formId) {
        return function () {
            $(formId).find(promotionElement.resultLabel).text('');
            $(promotionElement.modal).modal('show');
            $.validator.unobtrusive.parse(formId);
            $('.time-picker').datetimepicker({
                datepicker: false,
                format: 'H:i'
            });
            $(promotionElement.selectGameName).select2({ dropdownParent: $(promotionElement.modal), width: 'resolve'});
        }
    }

    function getOddsByGame(gameId) {
        $.ajax({
            method: "GET",
            url: "/Promotion/GetOddsByGame",
            data: { gameId }
        }).done(function (content) {
            if (content) {
                $(promotionElement.odds).html(content);
            }
        });
    }

    function onUpsertPromotion(success, message) {
        if (success) {
            setTimeout(function () {
                $('.modal-footer .btn-submit').prop('disabled', false);
                $(promotionElement.filterForm).submit();
                $(promotionElement.modal).modal('hide');
            }, 1000);
        }
        showMessage({ success, message });
    }

    function showMessage(data) {
        let label = $(promotionElement.resultLabel);
        label.removeClass();
        label.addClass(`text-${data.success ? "success" : "danger"}`);
        label.text(data.message);
    }

    function openDeleteModal(id) {
        $(promotionElement.modalDelete).modal('show');
        $(promotionElement.deleteId).val(id);
    }

    function deletePromotion() {
        $.ajax({
            method: "POST",
            url: "/Promotion/Delete",
            data: { id: $(promotionElement.deleteId).val() }
        }).done(function (deleted) {
            $(promotionElement.modalDelete).modal('hide');
            if (deleted) {
                $(promotionElement.filterForm).submit();
            } else {
                alert(promotionMessage.deleteUnsuccess);
            }
        }).fail(function () {
            alert(promotionMessage.deleteUnsuccess);
        });
    }

    function deleteTournament() {
        $.ajax({
            method: "POST",
            url: "/Promotion/DeleteTournament",
            data: { id: $(promotionElement.deleteId).val() }
        }).done(function (deleted) {
            $(promotionElement.modalDelete).modal('hide');
            if (deleted) {
                $(promotionElement.filterForm).submit();
            } else {
                alert(promotionMessage.deleteUnsuccess);
            }
        }).fail(function () {
            alert(promotionMessage.deleteUnsuccess);
        });
    }

    function addRanking() {
        var prizeList = $('#prize-list');
        var prizeIndex = prizeList.children('.form-row').length;

        var newPrizeRow = $(
            '<div class="form-row" id="prize-row-' + prizeIndex + '">' +
            '<div class="form-group col-md-2" id="ranking-col-' + prizeIndex + '">' +
            '<label>Ranking</label>' +
            '<input id="ranking-input-' + prizeIndex + '" type="text" name="WinningPrizes[' + prizeIndex + '].Ranking" class="form-control text-center ranking-input" ' +
            'data-val="true" ' +
            'data-val-required="The Ranking field is required." ' +
            'data-val-range="The field Ranking must be a valid value." ' +
            'data-val-unique="Value should be unique" />' +
            '</div>' +
            '<div class="form-group col-md-3" id="amount-col-' + prizeIndex + '">' +
            '<label>Win Amount</label>' +
            '<input id="win-amount-' + prizeIndex + '" type="number" name="WinningPrizes[' + prizeIndex + '].Value" class="form-control text-center win-amount-input" value="0" ' +
            'data-val="true" ' +
            'data-val-range="The field Value must be between 1 and 2147483647." ' +
            'data-val-range-max="2147483647" ' +
            'data-val-range-min="1" ' +
            'data-val-required="Win Amount is required." />' +
            '</div>' +
            '<div class="form-group col-md-1 d-flex align-items-center">' +
            '<i id="remove-col-' + prizeIndex + '" class="icon-cancel text-error mt-4 ml-4" style="cursor: pointer" onclick="promotion.removeRanking(' + prizeIndex + ')"></i>' +
            '</div>' +
            '</div>'
        );

        prizeList.append(newPrizeRow);
        $('#prizeError').attr('hidden', true)
        $('#orderError').attr('hidden', true);
        // Re-apply validation for the new inputs
        reparseFormValidationPrizes();
    }
    function reparseFormValidationPrizes () {
        var formEdit = $('#edit-tournament-form')
            .removeData('validator')
            .removeData('unobtrusiveValidation');
        var formAdNew = $('#add-new-tournament-form')
            .removeData('validator')
            .removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(formEdit);
        $.validator.unobtrusive.parse(formAdNew);
    }
    function removeRanking(index) {
        $('#orderError').attr('hidden', true);
        $('#prize-row-' + index).remove();
        if ($('#prize-list').children('.form-row').length === 0) {
            $('#prizeError').removeAttr('hidden');
        }
        // Update the indices of the remaining rows
        $('#prize-list .form-row').each(function (i) {
            $(this).attr('id', 'prize-row-' + i);
            $(this).find('.form-group').each(function () {
                var rankingInput = $(this).find('input[id^="ranking-input-"]');
                if (rankingInput.length > 0) {
                    rankingInput.attr('id', 'ranking-input-' + i);
                    rankingInput.attr('name', 'WinningPrizes[' + i + '].Ranking');
                }
                var amountInput = $(this).find('input[id^="win-amount-"]');
                if (amountInput.length > 0) {
                    amountInput.attr('id', 'win-amount-' + i);
                    amountInput.attr('name', 'WinningPrizes[' + i + '].Value');
                }
                var removeIcon = $(this).find('i[id^="remove-col-"]');
                if (removeIcon.length > 0) {
                    removeIcon.attr('id', 'remove-col-' + i);
                    removeIcon.attr('onclick', 'promotion.removeRanking(' + i + ')');
                }
            });
        });

        // Re-apply validation for the updated inputs
        reparseFormValidationPrizes();
    }

    function showOperatorSetting(id, operatorId, operatorName, status) {
        $.ajax({
            method: "GET",
            url: "/Promotion/UpdateOperatorSettings",
            data: {
                id,
                operatorId,
                operatorName,
                status
            }
        }).done(function (content) {
            if (content) {
                $('#operatorSettingContent').html(content);
                $('#operatorSettingModal').modal('show')
                loadDefaultSetting();
            }
        });
    }

    function loadDefaultSetting () {
        loadSettingGroup($('#Id')[0].value, $('#OperatorId')[0].value, $('#groupId')[0].value);
    }

    function loadSettingGroup(id, operatorId, groupId) {
        $.ajax({
            method: "GET",
            url: "/Promotion/GetCurrencyGroupSetting",
            data: {
                id,
                operatorId,
                groupId
            }
        }).done(function (content) {
            if (content) {
                $('#currency-group-game-setting').html(content);  
                loadSiteInclusion();
            }          
        });
    }

    function onChangeGroup(id, operatorId) {
        loadSettingGroup(id, operatorId, $('#GroupId')[0].value);
    }

    function toggleTab (tabName) {
        console.log("ok");
        $('#setting').hide();
        $('#site-inclusions').hide();

        $('#site-inclusions-tab-header').removeClass("active");
        $('#setting-tab-header').removeClass("active");

        const id = '#' + tabName;
        const headerId = '#' + tabName + '-tab-header';

        tabName == 'setting' && loadDefaultSetting();
        tabName == 'site-inclusions' && loadSiteInclusion();


        $(id).show();
        $(headerId).addClass("active");
    }

    function loadSiteInclusion() {     
        var siteInclusions = $('#IncludedSiteNames')[0].value;
        $('#siteInclusions').val(siteInclusions);    
    }

    function onSubmit  (isContinue) {
        if (isContinue) {
            $('#operatorSettingModal').on('hide.bs.modal', function (event) {
                location.reload();
            });
        }

        if ($('#setting').css('display') && $('#setting').css('display') !== 'none') {
            updateSetting(isContinue, modalHandlerOnUpdate);
        }

        if ($('#site-inclusions').css('display') && $('#site-inclusions').css('display') !== 'none') {
            updateSetting(isContinue, modalHandlerOnUpdate);
        }
    }

    function updateSetting (isContinue, modalHandlerOnUpdate) {
        var $form = $('#update-currency-group-setting-form');
        if ($form.valid()) {
            var siteInclusions = $('#siteInclusions').val();        
            var data = $('#update-currency-group-setting-form').serializeArray();
            data = data.filter(item => item.name !== 'IncludedSiteNames');
            data.push({ name: "IncludedSiteNames", value: siteInclusions });
            $.ajax({
                method: "POST",
                url: '/Promotion/UpdateOperatorSettings',
                dataType: 'json',
                data,
            }).done(function (result) {
                if (result.success) {
                    showMessage({ success: true, message: promotionMessage.messageUpdateSuccess });
                }
                else {
                    showMessage({ success: false, message: promotionMessage.messageUpdateUnsuccess });
                }

                modalHandlerOnUpdate(isContinue);
            });
        }
    }

    function modalHandlerOnUpdate (isContinue) {
        setTimeout(function () {
            $('#updateBtn').prop('disabled', false);
            $(promotionElement.resultLabel).hide();

            if (!isContinue) {
                $('#operatorSettingModal').modal('hide');
                location.reload();
            }
        }, 1000);
    }

    function parseRanking(ranking) {
        if (ranking.includes('-')) {
            const [start, end] = ranking.split('-').map(Number);
            return [start, end];
        }
        const rank = Number(ranking);
        return [rank, rank];
    }

    function onEditSubmit() {
        if (!$('#edit-tournament-form').valid()) {
            return
        }
        const nameValue = $('#Name').val()
        if (nameValue == null || nameValue == "") {
            $('#nameError').removeAttr('hidden');
            return false;
        } else {
            $('#nameError').attr('hidden', true);
        }
        var prizeList = $('#prize-list');
        var prizeRows = prizeList.children('.form-row');

        if (prizeRows.length === 0) {
            $('#prizeError').removeAttr('hidden');
            return false;
        } else {
            $('#prizeError').attr('hidden', true);
        }
        var previousRankingEnd = null;
        var previousValue = null;
        var isOrderValid = true;
        $('#orderError').attr('hidden', true);
        prizeRows.each(function (index, row) {
            var rankingText = $(row).find('input[id^="ranking-input-"]').val();
            var value = parseInt($(row).find('input[id^="win-amount-"]').val());

            const [currentRankingStart, currentRankingEnd] = parseRanking(rankingText);

            if (previousRankingEnd !== null && previousValue !== null) {
                if (currentRankingStart <= previousRankingEnd || value > previousValue) {
                    isOrderValid = false;
                    return false; // Exit each loop
                }
            }

            previousRankingEnd = currentRankingEnd;
            previousValue = value;
        });

        if (!isOrderValid) {
            $('#orderError').removeAttr('hidden');
            return false;
        }
        var isRunning = $('#isRunning').val();
        if (isRunning == 'running') {
            if (!confirm('This promotion is running, are you sure to make the change?')) {
                return false;
            }
        }
        var data = $('#edit-tournament-form').serialize();
        console.log("edit promotion", data);
        $.ajax({
            dataType: 'json',
            url: '/Promotion/UpdateTournament',
            type: 'POST',
            data,
            success: function (response) {
                if (response.success) {
                    promotion.onUpdatePromotionSuccess(response);
                } else {
                    promotion.onUpdatePromotionUnsuccess(response);
                }
            },
            error: function (response) {
                promotion.onUpdatePromotionUnsuccess(response);
            }
        });        
    }

    return {
        openAddNewPromotionForm: function () {
            openModal(promotionElement.formAdd)();
            getOddsByGame($(promotionElement.selectGameName).val())
        },
        openAddNewTournamentForm: function () {
            openModal(promotionElement.formAddTournament)();
        },
        getOddsByGame,
        onCreatePromotionSuccess: function (data) {
            const message = data.success ? promotionMessage.createSuccess : data.message;
            onUpsertPromotion(data.success, message);
        },
        openUpdatePromotionForm: openModal(promotionElement.formUpdate),
        onUpdatePromotionSuccess: function (data) {
            const message = data.success ? promotionMessage.updateSuccess : data.message;
            onUpsertPromotion(data.success, message);
        },
        onCreatePromotionUnsuccess: function () {
            showMessage({ success: false, message: promotionMessage.createUnsuccess });
        },
        onUpdatePromotionUnsuccess: function () {
            showMessage({ success: false, message: promotionMessage.updateUnsuccess });
        },
        deletePromotion,
        openDeleteModal,
        addRanking,
        openOperatorSettingsForm: function () {
            openModal(promotionElement.formTournamentSettings)();
        },
        openUpdateTournamentForm: function () {
            reparseFormValidationPrizes();
            openModal(promotionElement.formEditTourment)();
        },
        showOperatorSetting,
        onChangeGroup,
        toggleTab,
        onSubmit,
        deleteTournament,
        onEditSubmit,
        removeRanking,
        reparseFormValidationPrizes
    }
}(jQuery));

$(document).ready(function () {
    $("#promotion-form").submit();
})