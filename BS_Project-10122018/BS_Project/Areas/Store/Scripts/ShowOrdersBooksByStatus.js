function TransferStatus(orderID) {
    $.ajax({
        url: "/Order/TransferStatus",
        type: "POST",
        dataType: "json",
        data: { orderID: orderID },
        success: function (data) {
            if (data != -1) {
                alert("Chuyển Trạng thái Đơn hàng thành công.")
                location.reload();
            }
            else {
                alert("Chuyển trạng thái chưa thành công.")
            }
        }
    });
}

function Sort_Order() {
    var sortID = $('#order_id option:selected').val();
    var search_value = $('#searchString').val();
    if (sortID == 0) {
        window.location.href = window.location.origin + '/Store/Order/Index';
    }
    else if (sortID == 1) {
        //Sort theo status bằng Đặt hàng thành công
        $.ajax({
            url: "/Store/Order/ShowOrderByStatusOne",
            type: "GET",
            data: {
                search_value: search_value, page: 1, pageSize: 5
            },
            datatype: "html",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#list_order').html(data);
                $('#order_id option[value=1]').attr('selected', 'selected');
            },
            error: function (data, errorThrown) {
                alert('request failed :' + errorThrown);
            }
        });
    }
    else if (sortID == 2) {
        //Sort theo status bằng Đã tiếp nhận đơn hàng
        $.ajax({
            url: "/Store/Order/ShowOrderByStatusTwo",
            type: "GET",
            data: {
                search_value: search_value, page: 1, pageSize: 5
            },
            datatype: "html",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#list_order').html(data);
                $('#order_id option[value=2]').attr('selected', 'selected');
            },
            error: function (data, errorThrown) {
                alert('request failed :' + errorThrown);
            }
        });
    }
    else if (sortID == 3) {
        //Sort theo status bằng Đang đóng gói
        $.ajax({
            url: "/Store/Order/ShowOrderByStatusThree",
            type: "GET",
            data: {
                search_value: search_value, page: 1, pageSize: 5
            },
            datatype: "html",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#list_order').html(data);
                $('#order_id option[value=3]').attr('selected', 'selected');
            },
            error: function (data, errorThrown) {
                alert('request failed :' + errorThrown);
            }
        });
    }
    else if (sortID == 4) {
        //Sort theo status bằng Đang đóng gói
        $.ajax({
            url: "/Store/Order/ShowOrderByStatusFour",
            type: "GET",
            data: {
                search_value: search_value, page: 1, pageSize: 5
            },
            datatype: "html",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#list_order').html(data);
                $('#order_id option[value=4]').attr('selected', 'selected');
            },
            error: function (data, errorThrown) {
                alert('request failed :' + errorThrown);
            }
        });
    }
    else if (sortID == 5) {
        //Sort theo status bằng Đang vận chuyển
        $.ajax({
            url: "/Store/Order/ShowOrderByStatusFive",
            type: "GET",
            data: {
                search_value: search_value, page: 1, pageSize: 5
            },
            datatype: "html",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $('#list_order').html(data);
                $('#order_id option[value=5]').attr('selected', 'selected');
            },
            error: function (data, errorThrown) {
                alert('request failed :' + errorThrown);
            }
        });
    }
}