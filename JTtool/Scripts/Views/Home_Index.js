$(function () {
    $('#Login').click(function () {
        Login()
    });
});

function Login() {
    $.ajax({
        url: GetFullUrl('Home/Login'),
        method: 'POST',
        data: {
            LoginId: $('#LoginId').val(),
            Password: $('#Password').val(),
        },
        success: function (resault) {
            if (!resault.Success) {
                alert(resault.Message);
            } else {
                location.href = resault.Redirect + '?AId=' + resault.Message;
            }
        },
        error: function () {

        }
    });
}