var partner = (function ($) {
    return {
        checkAll: function () {
            var checked = $("#partner-all").prop("checked"),
                container = $("#partner-body-content");

            container.find('.partner-chk').prop({
                checked: checked
            });
        },

        uncheckAll: function (element) {
            if (!$(element).is(":checked")) {
                $("#partner-all").prop({
                    checked: false
                });
            }
        },

        showEditForm: function (partnerId) {
            $.ajax({
                method: "GET",
                url: "/partner/" + partnerId
            }).done(function (content) {
                if (content) {
                    $('#editModalContent').html(content);
                    $('#editModal').modal('show');
                    $.validator.unobtrusive.parse('#editForm');
                }
            });
        },

        upsertResultHandle: function (result) {
            $('#submitBtn').prop('disabled', true);

            if (result.success) {
                $('#successMsg').show();
            }
            else {
                $('#failedMsg').html(result.error);
                $('#failedMsg').show();
            }

            setTimeout(function () {
                $('#submitBtn').prop('disabled', false);
                result.success && location.reload();
            }, 1000);
        },

        showAddForm: function () {
            $.ajax({
                method: "GET",
                url: "/partner/add"
            }).done(function (content) {
                if (content) {
                    $('#partner-info').hide();
                    $('#addModalContent').html(content);
                    $('#addModal').modal('show');
                    $.validator.unobtrusive.parse('#addForm');
                }
            });
        },

        showPartnerUMForm: function () {
            var partnerNames = this.getSelectedPartners().names;

            if (!partnerNames.length) {
                alert("You should select at least one partner.")
                return
            }

            $.ajax({
                method: "GET",
                url: "/status/um"
            }).done(function (content) {
                if (content) {
                    $('#umTitle').html("Partner Under Maintainance");
                    $('#um').html(content);
                    $('#umModal').modal('show');
                    $('#partner-info').show();
                    $('#list-partners').html(partnerNames.join(', '));
                }
            });
        },

        showSCUMForm: function () {
            $.ajax({
                method: "GET",
                url: "/status/um"
            }).done(function (content) {
                if (content) {
                    $('#umTitle').html("SC Under Maintainance");
                    $('#partner-info').hide();
                    $('#um').html(content);
                    $('#umModal').modal('show');
                }
            });
        },

        underMaintenance: function () {
            const status = $("#IsUM").prop("checked") ? "ON" : "OFF";
            var isPartnerUM = $('#partner-info').css('display') !== 'none';
            var selectedPartners = this.getSelectedPartners();
            var message = isPartnerUM
                ? ("Are you sure you want to TURN " + status + " UM for parnters ? \n" + selectedPartners.names.join('; '))
                : ("You are going to set Under Maintenance to " + status + ". Are you sure?");
            var data = $('#umForm').serializeArray();

            if (isPartnerUM) {
                for (var i = 0; i < selectedPartners.ids.length; i++) {
                    data.push({
                        name: ['Partners[]'], value: selectedPartners.ids[i]
                    });
                }
            }

            if (confirm(message)) {
                $.ajax({
                    method: "POST",
                    url: isPartnerUM ? "partner/um" : "/status/um",
                    dataType: 'json',
                    data: data,
                }).done(function (updated) {
                    if (updated) {
                        alert("Under Maintenance is " + status + " now!");
                        $('#umModal').modal('hide');
                        location.reload();
                    }
                });
            }
        },

        showUpdateSuperPlayersForm: function () {
            $.ajax({
                method: "GET",
                url: "/status/super_players"
            }).done(function (content) {
                if (content) {
                    $('#super-players').html(content);
                    $('#super-players-modal').modal('show');
                }
            });
        },

        updateSuperPlayers: function () {
            const players = $('#super-players-names').val();
            const url = "/status/super_players";
            const data = { superPlayers: players };
            $('#update-super-players-btn').prop('disabled', true);

            $.ajax({
                method: "POST",
                url: url,
                dataType: 'json',
                data: data,
            }).done(function (result) {
                var msg = result ? '#update-super-players-success' : '#update-super-players-failed';
                $(msg).show();
                $("#super-players-msg-container").show();

                setTimeout(function () {
                    $('#update-super-players-btn').prop('disabled', false);
                    $("#super-players-msg-container").hide();
                    $(msg).hide();
                    $('#super-players-modal').modal('hide');
                }, 1000);
            });
        },

        getSelectedPartners: function () {
            var partnerIds = [];
            var partnerNames = [];

            $("#partner-body-content input[type=checkbox]:checked").map(function () {
                partnerIds.push($(this).attr('id'));
                partnerNames.push($(this).val());
            });

            return {
                ids: partnerIds,
                names: partnerNames
            };
        },

    }
}(jQuery));

$(document).ready(function () {
    var dataTable = $('#partner-table').DataTable({
        "paging": false,
        "ordering": false,
        "lengthMenu": [100, 200, 500],
        "info": false,
        "columnDefs": [
            { "targets": [0, 2, 3, 4, 5, 6, 7, 8, 9], "searchable": false }
        ]
    });

    dataTable.on('draw', function () {
        $('.sc-um-info').hide();

        var pageInfo = dataTable.page.info();
        var mergedCell = $('.sc-um-info:hidden')[0];

        if (mergedCell) {
            mergedCell.style.display = 'table-cell';
            mergedCell.rowSpan = pageInfo.end;
        }
    });
})