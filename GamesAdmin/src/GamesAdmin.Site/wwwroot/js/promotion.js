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

    function addRanking () {
        var prizeList = document.getElementById('prize-list');
        var prizeIndex = prizeList.querySelectorAll('[id^="ranking-"]').length;

        var newRankingCol = document.createElement('div');
        newRankingCol.classList.add('form-group', 'col-md-2');
        var rankingLabel = document.createElement('label');
        rankingLabel.textContent = 'Ranking';
        var rankingInput = document.createElement('input');
        rankingInput.type = 'text';
        rankingInput.classList.add('form-control', 'text-center');
        rankingInput.name = 'WinningPrizes[' + prizeIndex + '].Ranking';
        newRankingCol.appendChild(rankingLabel);
        newRankingCol.appendChild(rankingInput);

        var newAmountCol = document.createElement('div');
        newAmountCol.classList.add('form-group', 'col-md-3');
        var amountLabel = document.createElement('label');
        amountLabel.textContent = 'Win Amount';
        var amountInput = document.createElement('input');
        amountInput.type = 'number';
        amountInput.value = 0;
        amountInput.classList.add('form-control', 'text-center');
        amountInput.name = 'WinningPrizes[' + prizeIndex + '].Value';
        rankingInput.id = 'ranking-input-' + prizeIndex;
        newAmountCol.appendChild(amountLabel);
        newAmountCol.appendChild(amountInput);

        var newSpaceCol = document.createElement('div');
        newSpaceCol.classList.add('form-group', 'col-md-1');     

        prizeList.appendChild(newRankingCol);
        prizeList.appendChild(newAmountCol);
        prizeList.appendChild(newSpaceCol);
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

    function onEditSubmit() {
        const nameValue = $('#Name').val()
        if (nameValue == null || nameValue == "") {
            $('#nameError').removeAttr('hidden');
            return false;
        } else {
            $('#nameError').attr('hidden', true);
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
        showOperatorSetting,
        onChangeGroup,
        toggleTab,
        onSubmit,
        deleteTournament,
        onEditSubmit
    }
}(jQuery));

$(document).ready(function () {
    $("#promotion-form").submit();
})