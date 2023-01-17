var addSlotGame = (function ($) {
    return {
        onUpdateDisplayName: function (value) {
            $('#Name')[0].value = value.replaceAll(' ', '').toLowerCase();
        },
        submitForm: function () {
            $.ajax({
                method: "POST",
                url: "/GameSettings/CloneSlotGame",
                data: $("#cloneForm").serialize()
            }).done(function (msg) {
                if (msg) {
                    alert(msg)
                }
                else {
                    window.location.href = "/GameSettings";
                }
            });
        },
    }
}(jQuery))