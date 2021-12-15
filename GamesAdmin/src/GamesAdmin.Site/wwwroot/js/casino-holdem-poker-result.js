var casinoHoldemPokerResult = (function () {
    var player = [];
    var dealer = [];
    var communityCards = [];
    var index = 0;
    return {
        onInputNickname: function (event) {
            event.preventDefault();
            if (event.currentTarget.value !== "") {
                $('.error').addClass('hidden');
            }
        },
        selectCard: function (event, card) {
            if (index > 8) {
                return;
            }

            $(event.currentTarget).addClass('gray');
            if (index === 8) {
                $('.btn-submit').removeClass('disabled').removeAttr("disabled");
            }

            switch (true) {
                case (index < 2):
                    player.push(card);
                    break;
                case (index < 4):
                    dealer.push(card);
                    break;
                default:
                    communityCards.push(card);
                    break;
            }

            index++;
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
                player: player,
                dealer: dealer,
                communityCards: communityCards
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
