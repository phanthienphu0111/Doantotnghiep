$(document).ready(function () {
    //if ($('#username').text() !== "") {
    //    $.ajax({
    //        url: "/OrderBook/LoadOrderCart",
    //        data: {
    //        },
    //        datatype: "json",
    //        type: 'POST',
    //        success: function (data) {
    //            if (data >= 3) {
    //                $('div[name=addToCart]').css("background-color", "red");
    //                $('div[name=addToCart]').html("Hết lượt đặt sách");
    //            }
    //            $('#totalBook').text(data);
    //        },
    //        error: function () {
    //            alert("Error occured!!");
    //        }
    //    });
    //} 

    $('form#btnOrderCart').submit(function (e) {
        e.preventDefault();
        $('#viewFail').modal('show');
    });
});