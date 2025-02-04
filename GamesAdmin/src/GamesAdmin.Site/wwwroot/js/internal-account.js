$(document).ready(function () {
    $('#internal-test-submit').click(function (e) {
        e.preventDefault(); // Prevent default form submission
        $('#errorMsg').hide();
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

            success: function (response) {
                if (response.success) {
                    $('#successMsg').text("Update successful!").show();
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                } else {
                    if (response.invalidAccounts && response.invalidAccounts.length > 0) {
                        let invalidList = response.invalidAccounts.join(', ');
                        $('#errorMsg').html(
                            `<strong>Invalid Accounts:</strong> ${invalidList}`
                        ).show();
                    } else {
                        $('#errorMsg').text("Update failed due to an unknown error.").show();
                    }
                }
            },

            error: function () {
                $('#errorMsg').text("An error occurred. Please try again.").show();
            }
        });
    });
});