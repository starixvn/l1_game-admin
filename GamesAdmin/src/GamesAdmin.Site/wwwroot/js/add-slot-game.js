var addSlotGame = (function ($) {

    return {
        onUpdateDisplayName: function (value) {
            $('#Name')[0].value = value.replaceAll(' ','').toLowerCase();
        }
    }
}(jQuery))