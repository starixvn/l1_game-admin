var onlineUsers = (function ($) {
    function convertPartnerSelectToFanexItem(item) {
        return ({
            id: item.value,
            value: item.value,
            text: item.text,
            enable: !item.disabled,
            checked: item.selected
        })
    }

    return {
        init: function () {
            $('#online-users-table').DataTable({
                "order": [[1, "asc"]],
                "lengthMenu": [100, 200, 500]
            });
        },
        getFanexSelectData: () => partners.map(convertPartnerSelectToFanexItem),
    }
}(jQuery));

$(document).ready(function () {
    onlineUsers.init();

    var selections = onlineUsers.getFanexSelectData();

    $("#partners").fanexMultiSelect({
        listData: selections,
        showFilter: true,
        onLeave: function () {
            var value = $("#partners").fanexMultiSelect('getItems', true);

            $("#PartnerIds").val(value);
        },
    });
});