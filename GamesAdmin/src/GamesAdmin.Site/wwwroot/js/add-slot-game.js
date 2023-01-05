var addSlotGame = (function ($) {
    var isCloneSettings = true;

    return {
        onChangeSettingOption: function () {
            isCloneSettings = !isCloneSettings;


            if (isCloneSettings) {
                $('#settings').hide();
                $('#selectgame').show();
            }
            else {
                $('#selectgame').hide();
                $('#settings').show();
            }
        },
        onUpdateDisplayName: function (value) {
            $('#Name')[0].value = value.replaceAll(' ','').toLowerCase();
        }
    }
}(jQuery))