$(document).ready(function () {
    $('#domain-submit').click(function (e) {
        const value = $('#domain-value').val().replaceAll(/\s/g, '');
        const domains = value.split(';');
        const domainRegex = /^[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,6}$/;
        $('#invalidDomainMsg').hide();
        for (let i = 0; i < domains.length; i++) {
            if (!domainRegex.test(domains[i]) && domains[i] !== "") {
                const element = $('#domain-id');
                if (element.length > 0) {
                    $('#domain-id').remove();
                }
                $('#invalidDomainMsg').append('<span id="domain-id">' + domains[i] + '</span>').show();             
                return false;
            }
        }
            
        const id = $('#DomainId').val();
        $.ajax({
            type: 'POST',
            url: '/domain/update',
            data: {
                id,
                value
            },
            success: function (msg) {
                $('#successMsg').show();
                setTimeout(function () {
                    $('#successMsg').hide();
                }, 1000);
               
            },
        });
    });
});