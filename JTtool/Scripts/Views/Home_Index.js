$(function () {
    $('#Login').click(function () {
        Login()
    });
});

function Login() {
    $.ajax({
        url: '/Home/Login',
        method: 'POST',
        data: {
            LoginId: $('#LoginId').val(),
            Password: $('#Password').val(),
        },
        success: function (result) {
            if (result.Success) {
                window.location.href = result.Data.Redirect;
            } else {
                errorAlert(result.Message);
            }
        },
        error: function () {

        }
    });
}