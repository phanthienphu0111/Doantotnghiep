

$(function () {

});

function Register() {
    if (CheckInput()) {
        $('#sign_in').submit();
    }
}

function CheckInput() {
    var pass = $('#txtPass').val();
    var rePass = $('#txtRePass').val();

    CheckUsername();
    if (($('#txtName').val()) == "") {
        $('#errMessage').html("Họ tên trống. Vui lòng nhập lại.").show();
        $('#txtName').focus();
        return false;
    }

    if (($('#txtUsername').val()) == "") {
        $('#errMessage').html("Username trống. Vui lòng nhập lại.").show();
        $('#txtUsername').focus();
        return false;
    }

    if (($('#txtEmail').val()) == "") {
        $('#errMessage').html("Email trống. Vui lòng nhập lại.").show();
        $('#txtEmail').focus();
        return false;
    }

    if (($('#txtPhone').val()) == "") {
        $('#errMessage').html("Số điện thoại trống. Vui lòng nhập lại.").show();
        $('#txtPhone').focus();
        return false;
    }

    if (($('#txtPass').val()) == "") {
        $('#errMessage').html("Mật khẩu trống. Vui lòng nhập lại.").show();
        $('#txtPass').focus();
        return false;
    }

    if (($('#txtRePass').val()) == "") {
        $('#errMessage').html("Xác nhận Mật khẩu trống. Vui lòng nhập lại.").show();
        $('#txtRePass').focus();
        return false;
    }

    if (rePass != pass) {
        $('#errMessage').html("Xác nhận Mật khẩu không khớp.").show();
        $('#txtPass').val("");
        $('#txtPass').focus();
        return false;
    }

    $('#errMessage').html("").show();
    return true;
}

function CheckUsername() {
    var username = $('#txtUsername').val().toString();
    $.ajax({
        url: "/Home/CheckUsernameExist",
        type: "post",
        dataType: "json",
        async: false,
        data: { username: username },
        success: function (res) {
            if (!res.status) {
                alert("Tên đăng nhập đã bị trùng. Vui lòng nhập lại");
                $('#txtUsername').val("");
                $('#txtUsername').focus();
            } else {
                $('#errMessage').css('color', 'green').html("Bạn có thể sử dụng Tên đăng nhập này.").show();
                $('#txtEmail').focus();
            }
        }
    });
}