var promotion = (function ($) {
    let promotionElement = {
        modal: "#promotion-modal",
        filterForm: "#promotion-form",
        formAdd: '#add-new-promotion-form',
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

    return {
        openAddNewPromotionForm: function () {
            openModal(promotionElement.formAdd)();
            getOddsByGame($(promotionElement.selectGameName).val())
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
        openDeleteModal
    }
}(jQuery));

$(document).ready(function () {
    $("#promotion-form").submit();
})