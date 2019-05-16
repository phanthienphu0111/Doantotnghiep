// Kiểm tra Xác nhận Email khách hàng
var reEmailErr = "Xác nhận Email không chính xác. Vui lòng nhập lại.";
// Kiểm tra Xác nhận lại Mật khẩu khách hàng
var rePassErr = "Xác nhận mật khẩu không khớp. Vui lòng thử lại";
//Lệnh kiểm tra định dạng Email
var reg_email = /^[A-Za-z0-9]+([_\.\-]?[A-Za-z0-9])*@[A-Za-z0-9]+([\.\-]?[A-Za-z0-9]+)*(\.[A-Za-z]+)+$/;
//Thông báo Email định dạng bị sai
var EmailErr = "Định dạng Email bị sai. Vui lòng nhập lại."

$(function () {
    SetMaskedInput();
    //$('#errMessage').html("Đây là thông báo lỗi").show();
});
/************************************************************************/
/* Function Name : SetMaskedInput                                        */
/* Function      : Đăng ký Admin                                         */
/************************************************************************/
function SetMaskedInput() {
    $("#txtPhone").mask("9999999999", { placeholder: "" });
}

/************************************************************************/
/* Function Name : Register                                             */
/* Function      : Đăng ký Admin                                         */
/************************************************************************/
function RegisterAdmin() {
    if (CheckInput()) {
        $('#FormRegister').submit();
    }
}
/************************************************************************/
/* Function Name : checkInput                                           */
/* Function      : Kiểm tra dữ liệu                                     */
/* Return        : false: Sai true: Đúng                                */
/************************************************************************/
function CheckInput() {
    //Lấy Email ban đầu
    var Email = $('#Email').val();
    //Lấy Email xác thực
    var reEmail = $('#reEmail').val();

    //Lấy mật khẩu ban đầu
    var Pass = $('#Pass').val();
    //Lấy Mật khẩu xác thực
    var rePass = $('#rePass').val();
    //Gọi hàm kiểm tra Username bị trùng hay không?
    CheckUsername();

    //Kiểm tra Username rỗng
    if (($('#Username').val()) == "") {
        $('#errMessage').html("Tên đăng nhập không được trống. Vui lòng nhập lại.").show();
        $('#Username').focus();
        return false;
    }

    //Kiểm tra Email rỗng
    if (($('#Email').val()) == "") {
        $('#errMessage').html("Email không được trống. Vui lòng nhập lại.").show();
        $('#Email').focus();
        return false;
    }

    //Kiểm tra xác thực Email rổng
    if (($('#reEmail').val()) == "") {
        $('#errMessage').html("Email xác thực không được trống. Vui lòng nhập lại.").show();
        $('#reEmail').focus();
        return false;
    }

    if (($('#Pass').val() != "")) {
        if (($('#Pass').val()).length < 6 || ($('#Pass').val()).length > 20) {
            $('#errMessage').html("Mật khẩu phải có độ dài từ 6-20 ký tự.").show();
            $('#Pass').focus();
            return false;
        }
    } else {
        $('#errMessage').html("Mật khẩu không được trống. Vui lòng nhập lại.").show();
        $('#Pass').focus();
        return false;
    }

    //Kiểm tra xác thực Mật khẩu rỗng
    if (($('#rePass').val()) == "") {
        $('#errMessage').html("Xác thực Mật khẩu không được trống. Vui lòng nhập lại.").show();
        $('#rePass').focus();
        return false;
    }

    //Kiểm tra Họ tên rỗng
    if (($('#FullName').val()) == "") {
        $('#errMessage').html("Họ tên không được trống. Vui lòng nhập lại.").show();
        $('#FullName').focus();
        return false;
    }

    //Kiểm tra số điện thoại rỗng
    if (($('#txtPhone').val()) == "") {
        $('#errMessage').html("Số điện thoại không được trống. Vui lòng nhập lại.").show();
        $('#txtPhone').focus();
        return false;
    }

    //Kiểm tra Địa chỉ rỗng
    if (($('#txtAddress').val()) == "") {
        $('#errMessage').html("Địa chỉ không được trống. Vui lòng nhập lại.").show();
        $('#txtAddress').focus();
        return false;
    }

    //Kiểm tra ngày sinh rỗng
    if (($('#birthday').val()) == "") {
        $('#errMessage').html("Ngày sinh không được trống. Vui lòng nhập lại.").show();
        $('#birthday').focus();
        return false;
    }

    if (reEmail != Email) {
        $('#errMessage').html(reEmailErr).show();
        $('#reEmail').val("");
        $('#reEmail').focus();
        return false;
    }

    if (rePass != Pass) {
        $('#errMessage').html(rePassErr).show();
        $('#rePass').val("");
        $('#rePass').focus();
        return false;
    }

    if ($('#term_1').is(':checked') == false) {
        $('#errMessage').html("Bạn chưa tick vào ô Điều khoản sử dụng.").show();
        $('#term_1').focus();
        return false;
    }

    $('#errMessage').html("").show();
    return true;
}
/************************************************************************/
/* Function Name : CheckFormatEmail                                      */
/* Function      : Kiểm tra định Dạng Email                              */
/************************************************************************/
function CheckFormatEmail() {
    var Email = $('#Email').val();
    if (reg_email.test(Email) == false) {
        $('#errMessage').html(EmailErr).show();
        $('#Email').val("");
        $('#Email').focus();
    }
}

/************************************************************************/
/* Function Name : CheckUsername                                         */
/* Function      : Kiểm tra định Dạng Email                              */
/************************************************************************/
function CheckUsername() {
    var username = $('#Username').val().toString();
    if (username == "") {
        $('#annoumtUser').css('color', 'red').html("Bạn chưa nhập tên đăng nhập.").show();
        $('#Username').val("");
        $('#Username').focus();
    } else {
        $.ajax({
            url: "/AccountAdmin/CheckUsernameExist",
            type: "post",
            dataType: "json",
            async: false,
            data: { username: username },
            success: function (res) {
                if (!res.status) {
                    $('#annoumtUser').css('color', 'red').html("Tên đăng nhập đã bị trùng.").show();
                    $('#Username').val("");
                    $('#Username').focus();
                } else {
                    $('#annoumtUser').css('color', 'green').html("Bạn có thể sử dụng Tên đăng nhập này.").show();
                    $('#Email').focus();
                }
            }
        });
    }
}