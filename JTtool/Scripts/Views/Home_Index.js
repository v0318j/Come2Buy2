﻿$(function () {
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
            if (!resault.Success) {
                alert(resault.Message);
            } else {
                location.href = resault.Data.Redirect + '?AId=' + resault.Data.AId;
            }
        },
        error: function () {

        }
    });
}