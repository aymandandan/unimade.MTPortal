document.addEventListener('DOMContentLoaded', function () {
    $('#PasswordVisibilityButton').on("click", function () {
        const passwordInput = $('input[asp-for="UserInfo.Password"]');
        const icon = $(this).find('i');

        if (passwordInput.attr('type') === 'password') {
            passwordInput.attr('type', 'text');
            icon.removeClass('fa-eye-slash').addClass('fa-eye');
        } else {
            passwordInput.attr('type', 'password');
            icon.removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });
});