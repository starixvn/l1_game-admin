var gameUser = (function ($) {
    return {

        updateSpin: function () {            
            let message = "Your are going to update member spins. Are you sure?";

            if (confirm(message)) {
                $.ajax({
                    method: "POST",
                    url: "/GameUser/UpdateSpin",
                    dataType: 'json',
                    data: $('#updateSpin').serialize(),
                }).done(function (updated) {
                    if (updated) {                        
                        $('#updateModal').modal('hide');
                    }
                });
            }
        },

        showUpdateSpinForm: function (memberId) {
            $.ajax({
                method: "GET",
                url: "/GameUser/UpdateSpin?memberId=" + memberId
            }).done(function (content) {
                if (content) {
                    $('#updateSpinContent').html(content);
                    $('#updateModal').modal('show');
                }
            });
        },

    }
}(jQuery));
