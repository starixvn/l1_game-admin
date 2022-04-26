$(document).ready(function () {
    $('#btn-retrieve-end-round').click(function () {
        $.ajax({
            method: "POST",
            url: "/RetrieveEndRound/Index",
            data: $("#bet-details-form").serialize()
        }).done(function (content) {
            if (content) {
                $('#retrieve-end-round-result').text(JSON.stringify(JSON.parse(content), null, 2));
                $('.retrieve-end-round-result-field').removeClass('d-none');
            }
        });
    });
})