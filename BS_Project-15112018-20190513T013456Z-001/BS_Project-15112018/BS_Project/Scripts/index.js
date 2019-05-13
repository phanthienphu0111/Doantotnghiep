var userErrMsg = "Vui lòng nhập Tên đăng nhập.";

var passErrMsg = "Vui lòng nhập mật khẩu.";

var errMessage = "ID hoặc mật khẩu không khớp.";

$(function () {
    // Toggle Function
    $('.toggle').click(function () {
        $(this).children('i').toggleClass('fa-pencil');
        $('.form').animate({
            height: "toggle",
            'padding-top': 'toggle',
            'padding-bottom': 'toggle',
            opacity: "toggle"
        }, "slow");
    });
    if (($('#Username').val()) == "") {
        $('#Username').focus();
    }
    else if ((($('#Username').val()) != "") && (($('#Password').val()) != "")) {
        $('#Username').focus();
    }
    else if (($('#Username').val()) != "") {
        $('#Password').focus();
    }
    if ($('#Username').val() != "" && $('#Password').val() != "" && $('#Password').val() != "") {
        Login();
    }
    $("#chkRememberMe").removeClass("form-module input");
});

function Login() {
    if (CheckInput()) {
        $('#errMessage').html("");
        var User = ($('#Username').val());
        var Password = ($('#Password').val());
        $.ajax({
            data: {
                Username: User,
                Password: Password
            },
            url: '/AccountAdmin/ExecuteLogin',
            dataType: 'json',
            type: 'post',
            cache: false,
            success: function (res) {
                if (res.status == true) {
                    window.location = "/Store/Home/Index";
                }
                else if (res.status == false) {
                    $('#errMessage').css("color", "red");
                    $('#errMessage').html(errMessage).show();
                    $('#Username').val("");
                    $('#Password').val("");
                    $('#Username').focus();
                }
            }
        });
    }
}

function CheckInput() {
    if (($('#Username').val()) == "") {

        $('#errMessage').css("color", "red");
        $('#errMessage').html(userErrMsg).show();

        $('#Username').val("");

        $('#Username').focus();
        return false;
    }
    if (($('#Password').val()) == "") {

        $('#errMessage').html(passErrMsg).show();

        $('#Password').val("");

        $('#Password').focus();

        return false;
    }
    return true;
}

function ConvertHeight(height) {
    var newHeight = (screen.height > 768) ? (height + (screen.height - 768)) : height;
    return newHeight;
}

function HtmlEncode(val) {
    if (val == null || val == undefined) {
        return "";
    }
    var list = val.split('\n');
    var text = "";
    for (var i = 0; i < list.length; i++) {
        text += $('<div />').text(list[i]).html();
        if (i < list.length - 1) {
            text += "\n";
        }
    }
    return text;
}

function GotoRegister() {
    debugger
    window.location.assign = "/AccountAdmin/Register";
}