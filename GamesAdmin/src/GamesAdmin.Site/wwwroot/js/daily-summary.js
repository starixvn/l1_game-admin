$(document).ready(function () {
    $('#daily-summary-form').submit(function (event) {
        event.preventDefault();
        $.ajax({
            method: "POST",
            url: "/DailySummary/Index",
            data: $("#daily-summary-form").serialize()
        }).done(function (content) {
            if (content) {
                $('#daily-summary-result').text(JSON.stringify(JSON.parse(content), null, 2));
                $('.daily-summary-field').removeClass('d-none');
            }
        });
    });
})