var casinoHoldemPokerResult = (function () {
    var player = [];
    var dealer = [];
    var communityCards = [];
    var index = 0;
    return {
        selectCard: function (event, card) {
            if (index > 8) {
                return;
            }

            $(event.currentTarget).addClass('gray');

            if (index < 2) {
                player.push(card);
            }
            else if (index < 4) {
                dealer.push(card);
            }
            else {
                communityCards.push(card);
            }

            index++;
        },
        submit: function (e) {
            e.preventDefault();
            var data = {
                roundName: $("#nickname").val(),
                player: player,
                dealer: dealer,
                communityCards: communityCards
            };
            $.ajax({
                type: "POST",
                url: "SetCasinoHoldemPokerResult",
                data: data,
                dataType: 'html',
                success: function (html) {
                    $('#modal').modal('show');
                    $('.modal-body').html(html);
                }
            });
        }
    };
})();
