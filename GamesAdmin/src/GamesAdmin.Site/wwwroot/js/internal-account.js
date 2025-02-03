$(document).ready(function () {
    $('#internal-test-submit').click(function (e) {
        const value = $('#account-test').val().replaceAll(/\s/g, '');
        const currentAccounts = value.split(';').filter(account => account);
        const original = $('#OriginalAccountTest').val();
        const originalAccounts = original.split(';').filter(account => account);

        const deletedAccounts = originalAccounts.filter(account => !currentAccounts.includes(account));
        const addAccounts = currentAccounts.filter(account => !originalAccounts.includes(account));
        $.ajax({
            type: 'POST',
            url: '/internalTestAccount/update',
            data: { addAccounts, deletedAccounts },
            dataType: 'json',

            success: function (msg) {
                $('#successMsg').show();
                setTimeout(function () {
                    location.reload();
                }, 1000);
               
            },
        });
    });
});