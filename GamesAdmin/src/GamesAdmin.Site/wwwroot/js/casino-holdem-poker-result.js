var casinoHoldemPokerResult = (function () {
    var cards = [];
    var index = 0;
    return {
        onInputNickname: function (event) {
            event.preventDefault();
            if (event.currentTarget.value !== "") {
                $('.error').addClass('hidden');
            }
        },
        selectCard: function (event, card) {
            var currentElement = $(event.currentTarget);

            var disableSubmit = function () {
                $('.btn-submit').addClass('disabled').attr("disabled");
            };

            var enableSubmit = function () {
                $('.btn-submit').removeClass('disabled').removeAttr("disabled")
            };

            if (currentElement.hasClass('gray')) {
                currentElement.removeClass('gray');
                disableSubmit();
                cards.splice(cards.indexOf(card), 1);
                index--;
            }
            else {
                if (index > 8) {
                    return;
                }

                index === 8 ? enableSubmit() : disableSubmit();
                currentElement.addClass('gray');
                cards.push(card);
                index++;
            }
        },
        reloadPage: function (e) {
            e.preventDefault();
            location.reload();
        },
        submit: function (e) {
            e.preventDefault();
            var nickame = $("#nickname").val();
            if (nickame === "") {
                $('.error').removeClass('hidden');
                return;
            }

            var data = {
                roundName: nickame,
                player: cards.slice(0, 2),
                dealer: cards.slice(2, 4),
                communityCards: cards.slice(-5)
            };

            $.ajax({
                type: "POST",
                url: "SetCasinoHoldemPokerResult",
                data: data,
                dataType: 'html',
                success: function (response) {
                    $('#modal').modal('show');
                    $('.modal-body').html(response);
                }
            });
        }
    };
})();
