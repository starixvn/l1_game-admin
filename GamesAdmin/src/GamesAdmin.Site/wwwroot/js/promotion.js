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
        resultLabel: '#result-label'
    };
    let promotionMessage = {
        createSuccess: 'Add Successfully!',
        createUnsuccess: 'Cannot Add Promotion!',
        updateSuccess: 'Edit Successfully!',
        updateUnsuccess: 'Cannot Edit Promotion!',
        deleteUnsuccess: 'Cannot Delete Promotion'
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
        newAmountCol.appendChild(amountLabel);
        newAmountCol.appendChild(amountInput);

        var newSpaceCol = document.createElement('div');
        newSpaceCol.classList.add('form-group', 'col-md-1');     

        prizeList.appendChild(newRankingCol);
        prizeList.appendChild(newAmountCol);
        prizeList.appendChild(newSpaceCol);
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
        addRanking
    }
}(jQuery));

$(document).ready(function () {
    $("#promotion-form").submit();
})