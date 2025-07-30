var promotionAnnouncement = (function ($) {
    let announcementElement = {
        filterForm: '#announcement-form',
        modalAdd: '#add-modal',
        modalUpdate: '#update-modal',
        timePicker: '.time-picker',
        selectGameName: '.select-promotion-mapping',
        modalAddBody: '#add-modal .modal-body .body',
        itemIndex: '#add-modal .modal-body .body .item-index',
        table: '#announcement-table',
        resultLabel: '.result-label',
        modalDelete: '#announcement-modal-delete',
        deleteId: '#delete-announcement-id',
        inputAnnouncementId: '.input-announcement-id',
        inputAnnouncementUnique: '.input-announcement-unique'
    };

    let inputElement = {
        id: '#id-',
        enabled: "#enabled-",
        startTime: "#startTime-",
        endTime: '#endTime-',
        promotionId: '#promotionId-',
        displayKind: '#displayKind'
    };
    let titleElement = {
        "en-US": '#title-en-US-',
        "vi-VN": '#title-vi-VN-',
        "zh-CN": '#title-zh-CN-',
        "zh-TW": '#title-zh-TW-',
        "th-TH": '#title-th-TH-',
        "id-ID": '#title-id-ID-',
        "ja-JP": '#title-ja-JP-',
        "es-ES": '#title-es-ES-'
    };
    let messagesElement = {
        "en-US": '#message-en-US-',
        "vi-VN": '#message-vi-VN-',
        "zh-CN": '#message-zh-CN-',
        "zh-TW": '#message-zh-TW-',
        "th-TH": '#message-th-TH-',
        "id-ID": '#message-id-ID-',
        "ja-JP": '#message-ja-JP-',
        "es-ES": '#message-es-ES-'
    };
    let announcementMessage = {
        addSuccess: 'Add Successfully!',
        addUnsuccess: 'Cannot add annoucement!',
        editSuccess: 'Edit Successfully!',
        editUnsuccess: 'Cannot edit annoucement',
        deleteUnsuccess: 'Cannot delete announcement'
    };

    function openModal(modalId) {
        return function () {
            $(modalId).find(announcementElement.resultLabel).text('');
            $(modalId).modal('show');
            $(announcementElement.timePicker).datetimepicker({ format: 'm/d/Y H:i', });
            $(announcementElement.selectGameName)
                .select2({
                    dropdownParent: $(modalId),
                    language: {
                        "noResults": function () {
                            return "No promotion found";
                        }
                    },
                });
            updateIndexItem();
        }
    }

    function registerEventForItem() {
        $(announcementElement.timePicker).datetimepicker({ format: 'm/d/Y H:i', });
        $(announcementElement.selectGameName)
            .select2({
                dropdownParent: $(announcementElement.modalAdd),
                language: {
                    "noResults": function () {
                        return "No promotion found";
                    }
                },
            });
    }

    function onUpsert(modalId, data) {
        if (data.success) {
            setTimeout(function () {
                $('.modal-footer .btn-submit').prop('disabled', false);
                $(announcementElement.filterForm).submit();
                $(modalId).modal('hide');
            }, 1500);
        }
        showMessage(modalId, data);
    }

    function addAnnouncement() {
        let { success, data } = transformDataFromModalAdd();
        if (success) {
            $.ajax({
                method: "POST",
                url: "/Promotion/CreateAnnouncement",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(data)
            }).done(function (res) {
                const message = res ? announcementMessage.addSuccess : announcementMessage.addUnsuccess;
                onUpsert(announcementElement.modalAdd, { success: res, message });
            }).fail(function () {
                showMessage(announcementElement.modalAdd, { success: false, message: announcementMessage.addUnsuccess });
            });
        }
    }

    function transformDataFromModalAdd() {
        let success = true;
        let isDuplicate = false;
        let data = [];
        let length = $(announcementElement.itemIndex).length;
        for (let i = 0; i < length; i++) {
            let id = $(announcementElement.inputAnnouncementUnique)[i].value;
            let valid = validateInput(id);
            if (valid.success) {
                data.push(transformData(id));
            } else {
                success = false
                showMessage(announcementElement.modalAdd, valid);
            }
        }
        data.forEach(e => {
            if (data.some(item => item !== e && item.promotionId === e.promotionId)) {
                success = false;
                isDuplicate = true;
            }
        });
        isDuplicate && showMessage(announcementElement.modalAdd, { success: false, message: "Duplicate promotion mapping" });
        return { success, data };
    }

    function transformData(id) {
        let data = { title: {}, messages: {} };
        for (let key in inputElement) {
            let elementId = inputElement[key] + id;
            if (key === 'enabled') {
                data[key] = $(elementId).is(":checked");
            } else {
                if (key === 'startTime' || key === 'endTime') {
                    data[key] = $(elementId).datetimepicker('getValue');
                } else {
                    data[key] = $(elementId).val();
                }
            }
        }

        let promotionListContainer = '#promotionListContainer-' + id;
        let promotionCampaignListContainer = '#promotionCampaignListContainer-' + id;

        if (!$(promotionListContainer).attr('hidden')) {
            data.promotionId = $('#promotionId-' + id + '-promotion').val() ?? data.promotionId;
            data.displayKind = $('#happyhours-' + id).val();
        } else if (!$(promotionCampaignListContainer).attr('hidden')) {
            data.promotionId = $('#promotionId-' + id + '-campaign').val() ?? data.promotionId;
            data.displayKind = $('#tournament-' + id).val();
        }

        for (let t in titleElement) {
            let elementId = titleElement[t] + id;
            data.title[t] = $(elementId).val();
        }
        for (let m in messagesElement) {
            let elementId = messagesElement[m] + id;
            data.messages[m] = $(elementId).val();
        }
        return data;
    }

    function validateInput(id) {
        let startTime = new Date($(inputElement.startTime + id).val());
        let endTime = new Date($(inputElement.endTime + id).val());
        if (startTime >= endTime) {
            return { success: false, message: "Start time cannot be greater than or equal to end time" };
        }
        for (let t in titleElement) {
            let elementId = titleElement[t] + id;
            if ($(elementId).val() === "") {
                return { success: false, message: 'Title cannot be empty' };
            }
        }
        for (let m in messagesElement) {
            let elementId = messagesElement[m] + id;
            if ($(elementId).val() === "") {
                return { success: false, message: 'Content cannot be empty' };
            }
        }
        return { success: true, message: "" };
    }

    function showMessage(modalId, data) {
        let label = $(modalId).find(announcementElement.resultLabel);

        label.removeClass('text-success');
        label.removeClass('text-danger');
        label.addClass(`text-${data.success ? "success" : "danger"}`);
        label.text(data.message);
    }

    function updateAnnouncement() {
        let id = $(announcementElement.modalUpdate).find(announcementElement.inputAnnouncementId).val();
        let valid = validateInput(id);
        if (valid.success) {
            let data = transformData(id);
            $.ajax({
                method: "POST",
                url: "/Promotion/UpdateAnnouncement",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(data)
            }).done(function (res) {
                const message = res ? announcementMessage.editSuccess : announcementMessage.editUnsuccess;
                onUpsert(announcementElement.modalUpdate, { success: res, message });
            }).fail(function () {
                showMessage(announcementElement.modalUpdate, { success: false, message: announcementMessage.editUnsuccess });
            });
        } else {
            showMessage(announcementElement.modalUpdate, valid);
        }
    }

    function deleteItem(unique) {
        let classItem = '.announcement-item-' + unique;
        $(classItem).remove();
        updateIndexItem();
    }

    function updateIndexItem() {
        $(announcementElement.itemIndex)
            .each(function (index, element) {
                $(element).text(index + 1);
            })
    }

    function openModalDelete(id) {
        $(announcementElement.modalDelete).modal('show');
        $(announcementElement.deleteId).val(id);
    }

    function deleteAnnouncement() {
        $.ajax({
            method: "POST",
            url: "/Promotion/DeleteAnnouncement",
            data: { id: $(announcementElement.deleteId).val() }
        }).done(function (deleted) {
            $(announcementElement.modalDelete).modal('hide');
            if (deleted) {
                $(announcementElement.filterForm).submit();
            } else {
                alert(announcementMessage.deleteUnsuccess);
            }
        }).fail(function () {
            alert(announcementMessage.deleteUnsuccess);
        });
    }

    function togglePromotionType(select, unique) {
        var selectedValue = select.value;
        var promotionListContainer = document.getElementById('promotionListContainer-' + unique);
        var promotionCampaignListContainer = document.getElementById('promotionCampaignListContainer-' + unique);

        if (selectedValue == "3") { // PromotionValue
            promotionListContainer.hidden = false;
            promotionCampaignListContainer.hidden = true;
        } else if (selectedValue == "4") { // PromotionCampaignValue
            promotionListContainer.hidden = true;
            promotionCampaignListContainer.hidden = false;
        }
    }


    return {
        init: function () {
            let colLength = $(announcementElement.table).find('th').length;
            let col = [0, 1, 2, 3, 4];
            if (colLength > 7) {
                col.push(7);
            }
            let colDef = [{ "targets": col, "searchable": false }];
            if (colLength > 7) {
                colDef.push({ "targets": [7], "width": '10%' });
            }
            $(announcementElement.table).DataTable({
                "lengthMenu": [[30, 60, -1], [5, 10, "All"]],
                "ordering": false,
                "info": false,
                "columns": [
                    { "width": "3%" },
                    { "width": "2%" },
                    { "width": "5%" },
                    { "width": "15%" },
                    { "width": "35%" },
                    { "width": "15%" },
                    { "width": "15%" },
                ],
                "columnDefs": colDef
            });
        },
        openAddForm: openModal(announcementElement.modalAdd),
        openUpdateForm: openModal(announcementElement.modalUpdate),
        onAddItemSuccess: function () {
            registerEventForItem();
            updateIndexItem();
        },
        deleteItem,
        addAnnouncement,
        updateAnnouncement,
        deleteAnnouncement,
        openModalDelete,
        togglePromotionType
    }
}(jQuery));

$(document).ready(function () {
    $("#announcement-form").submit();
})