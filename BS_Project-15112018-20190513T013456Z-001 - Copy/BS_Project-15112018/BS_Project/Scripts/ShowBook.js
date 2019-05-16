var priceFrom = null;
var priceTo = null;
// Read a page's GET URL variables and return them as an associative array.
function getUrlVars() {
    var vars = [], hash;
    //split tách 1 chuỗi thành 1 mãng
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

$(function () {
    var temp = 0;
    $('.showBookFollowCategory').click(function () {
        //quay sort về default khi chuyển thể loại
        $("div.sorting select").val("0");

        // thay đổi condition = 3 để sort cho đúng (vì sử dụng ajax ko thay đổi đường dẫn nên phải dùng cách này)
        //jQuery.param.querystring(window.location.href, 'condition=3');
        //location.href.replace("condition=3");
        if (temp = 0) {
            window.location.href = 'http://localhost:4774/Book/ShowBook?condition=3';
            temp = temp + 1;
        }

        var page = getUrlVars()["page"];

        var idcate = $(this).attr('id');
        $.ajax({
            type: "GET",
            url: "/Book/ShowBookFollowCategory",
            data: {
                page: 0, idCategory: idcate, sortID: 0, condition: 1
            },
            datatype: "html",
            contentType: 'application/x-www-form-urlencoded',
            success: function (data) {
                $('#listBook').html(data);
                $('#' + 0 + 'pagecate').css("background-color", "red");
                $('#' + page + 'pagecate').css("background-color", "transparent");
            },
            error: function () {
                alert("Error occured!!")
            }
        });
    });
    //hiển thị sách đã được filter
    $('.showBookPrice').click(function () {
        $("div.sorting select").val("0");
        if (temp = 0) {
            window.location.href = 'http://localhost:4774/Book/ShowBook?condition=3';
            temp = temp + 1;
        }
        var page = getUrlVars()["page"];
        var filterID = $(this).attr('id');

        
        GetInforFilterPrice(filterID);
        var a = priceFrom;
        var b = priceTo;
        $.ajax({
            type: "GET",
            url: "/Book/ShowBookFollowPrice",
            data: {
                page: 0, priceFrom: priceFrom, priceTo: priceTo, sortID: 0, condition: 5
            },
            datatype: "html",
            contentType: 'application/x-www-form-urlencoded',
            success: function (data) {
                $('#listBook').html(data);
                $('#' + 0 + 'pagecate').css("background-color", "red");
                $('#' + page + 'pagecate').css("background-color", "transparent");
            },
            error: function () {
                alert("Error occured!!")
            }
        });
    });
    $('#country1').on('change', function () {

        //// remove màu đang có
        //var page = getUrlVars()["page"];
        // chuyển thành page đầu tiên khi search của trang showbook

        //alert('#' + page + 'pagecate');
        var condition = getUrlVars()["condition"];
        var filter = this.value;
        if (condition == 1) {
            var idCategory = getUrlVars()["idcategory"];
            //trả về data partialview ShowBookFollowCategory
            $.ajax({
                type: "GET",
                url: "/Book/ShowBookFollowCategory",
                data: {
                    sortID: filter, page: 0, idCategory: idCategory, condition: condition
                },
                datatype: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('#listBook').html(data);
                    // select lại trang đầu tiên khi chọn search
                    $('#' + 0 + 'pagecate').css("background-color", "red");
                    var page = getUrlVars()["page"];
                    $('#' + page + 'pagecate').css("background-color", "transparent");
                    //16/10/2018
                    //---------------------Test

                },
                error: function () {
                    alert("Error occured!!")
                }
            });
        } else if (condition == 2) {
            var idPublisher = getUrlVars()["idpublisher"];
            //trả về data partialview ShowBookFollowPublisher
            $.ajax({
                type: "GET",
                url: "/Book/ShowBookFollowPublisher",
                data: {
                    sortID: filter, page: 0, idpublisher: idPublisher, condition: condition
                },
                datatype: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('#listBook').html(data);
                    $('#' + 0 + 'pagepublisher').css("background-color", "red");
                    var page = getUrlVars()["page"];
                    $('#' + page + 'pagepublisher').css("background-color", "transparent");
                },
                error: function () {
                    alert("Error occured!!")
                }
            });
            //condition = 3 hiển thị tất cả các book
        } else if (condition == 3) {
            $.ajax({
                type: "GET",
                url: "/Book/ShowAllBook",
                data: {
                    sortID: filter, page: 0, condition: condition
                },
                datatype: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('#listBook').html(data);
                    $('#0page').css("background-color", "red");
                    //---------------------Test
                },
                error: function () {
                    alert("Error occured!!")
                }
            });
        }
    });
})

function GetInforFilterPrice(filterID) {
    $.ajax({
        type: "post",
        url: "/Book/GetInforFilterPrice",
        dataType: "json",
        data: { FilterID: filterID },
        async: false,
        success: function (data) {
            if (data != null) {
                priceFrom = data.priceFrom;
                priceTo = data.priceTo;
            }
            return false;
        }
    });
}