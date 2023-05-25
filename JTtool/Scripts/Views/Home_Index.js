$(function () {
    $('#Login').click(Login);
    $('#registerbtn').click(Register);

    $('#registerModal').on('hidden.bs.modal', function () {
        // 清除所有輸入欄位的值
        $('#registerLoginId').val('');
        $('#registerPassword').val('');
        $('#registerConfirmPassword').val('');
        $('#registerName').val('');
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

function Register() {
    $.ajax({
        url: '/Account/AddAccount',
        method: 'POST',
        data: {
            LoginId: $('#registerLoginId').val(),
            Password: $('#registerPassword').val(),
            ConfirmPassword: $('#registerConfirmPassword').val(),
            Name: $('#registerName').val()
        },
        success: function (result) {
            if (result.Success) {
                // 註冊成功，關閉 modal
                successAlert('註冊成功');
                $('#closeRegisterModal').click();
            } else {
                // 註冊失敗，顯示錯誤訊息
                errorAlert(result.Message);
            }
        },
        error: function () {
            errorAlert('請求失敗');
        }
    });
}