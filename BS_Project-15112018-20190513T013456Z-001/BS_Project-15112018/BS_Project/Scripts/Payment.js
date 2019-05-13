var reg_email = /^[A-Za-z0-9]+([_\.\-]?[A-Za-z0-9])*@[A-Za-z0-9]+([\.\-]?[A-Za-z0-9]+)*(\.[A-Za-z]+)+$/;

$(function () {
    $('#alertAdd').css('display', 'none');
    $('#alertPhone').css('display', 'none');
    $('#alertEmail').css('display', 'none');
});

function Payment() {
    debugger
    var check = true;
    var phone = $('#custphone').val();
    var address = $('#custadd').val();
    var email = $('#custemail').val();

    if (phone == "") {
        $('#alertAdd').css('display', 'auto');
        check = (check && false);
    } else {
        $('#alertAdd').css('display', 'none');
    }

    if (address == "") {
        $('#alertPhone').css('display', 'auto');
        check = (check && false);
    } else {
        $('#alertPhone').css('display', 'none');
    }

    if (email == "") {
        $('#alertEmail').css('display', 'auto');
        check = (check && false);
    } else if (reg_email.test(email) == false) {
        $('#alertEmail').css('display', 'auto');
        check = (check && false);
    } else {
        $('#alertEmail').css('display', 'none');
    }

    //Submit form Payment
    if (check) {
        $('#payment_form').submit();
    }
    
}