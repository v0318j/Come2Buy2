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
        success: function (resault) {
            if (resault.Success) {
                location.href = resault.Data.Redirect + '?AId=' + resault.Data.AId;
            } else {
                errorAlert(resault.Message);
            }
        },
        error: function () {

        }
    });
}