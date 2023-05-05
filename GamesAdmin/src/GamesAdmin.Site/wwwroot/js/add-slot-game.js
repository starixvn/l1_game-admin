var addSlotGame = (function ($) {
    return {
        onUpdateDisplayName: function (value) {
            $('#GameName')[0].value = value.replaceAll(' ', '').toLowerCase();
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

        onChangeGameType: function () {
            const type = $('#GameType').val();

            $.ajax({
                method: "GET",
                url: "/games?type=" + type,
            }).done(function (games) {  
                var existingGames = $('#SelectedGame');
                $(existingGames).empty();
                for (var key in games) {
                    if (games.hasOwnProperty(key)) {
                        $(existingGames).append($(`<option value="${games[key].value}">${games[key].text}</option>`));
                    }
                }
            });
        },
    }
}(jQuery))