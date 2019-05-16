var priceFrom = null;
var priceTo = null;
var save = null;
var Categoryid = null;
var check = false;
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
        if (save !== null) {
            $('.' + save).css("background-color", "white");
        }
        //Biến lưu giá trị của sortID
        //sortID gồm 5 giá trị: 0(mặc định), 1(Số lượt xem), 2(Ngày xuất bản), 3(Giá giảm dần), 4(Giá tăng dần).
        var sortID = $("div.sorting select").val();

        //Biến lưu giá trị của Thể loại
        var idcate = $(this).attr('id');
        Categoryid = idcate;
        var annouce = $('.' + idcate).text();

        //Biến lưu mã Nhà xuất bản
        var idpublisher = getUrlVars()["idpublisher"];

        //Biến lưu số trang hiện tại
        var page = getUrlVars()["page"];

        if (idpublisher === undefined || idpublisher === null || idpublisher === "") {
            $.ajax({
                type: "GET",
                url: "/Book/ShowBookByCateAndSortID",
                data: {
                    page: 0, sortID: sortID, idcate: idcate, condition: 1
                },
                dataType: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('.' + idcate).css("background-color", "#2fdab8");
                    save = idcate;
                    $('#listBook').html(data);
                    $('#' + 0 + 'pagecate').css("background-color", "red");
                    $('#' + page + 'pagecate').css("background-color", "transparent");
                },
                error: function () {
                    alert("Lỗi ứng dụng.");
                }
            });
        }
        else if (idpublisher !== undefined || idpublisher !== null) {
            $.ajax({
                type: "GET",
                url: "/Book/ShowBookByPublisherCategoryAndSortID",
                data: {
                    page: 0, sortID: sortID, idcate: idcate, idpublisher: idpublisher, condition: 1
                },
                dataType: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('.' + idcate).css("background-color", "#2fdab8");
                    save = idcate;
                    $('#listBook').html(data);
                    $('#' + 0 + 'pagecate').css("background-color", "red");
                    $('#' + page + 'pagecate').css("background-color", "transparent");
                },
                error: function () {
                    alert("Lỗi ứng dụng.");
                }
            });
        }
    });

    function SetAnnounce(annouce) {
        if (annouce != undefined || annouce != null) {
            $('#annoumentSort').html(annouce);
        }
    }

    //hiển thị sách đã được filter
    $('.showBookPrice').click(function () {
        $("div.sorting select").val("0");
        if (temp === 0) {
            window.location.href = 'http://localhost:44324/Book/ShowBook?condition=3';
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

    //$('#country1').on('change', function () {
    //    //alert('#' + page + 'pagecate');
    //    var condition = getUrlVars()["condition"];
    //    var publisher = getUrlVars()["idpublisher"];
    //    SetAnnoumentSort(publisher);
    //    var filter = this.value;
    //    if (condition == 1) {
    //        var idCategory = getUrlVars()["idcategory"];
    //        //trả về data partialview ShowBookFollowCategory
    //        $.ajax({
    //            type: "GET",
    //            url: "/Book/ShowBookFollowCategory",
    //            data: {
    //                sortID: filter, page: 0, idCategory: idCategory, condition: condition
    //            },
    //            datatype: "html",
    //            contentType: 'application/x-www-form-urlencoded',
    //            success: function (data) {
    //                $('#listBook').html(data);
    //                // select lại trang đầu tiên khi chọn search
    //                $('#' + 0 + 'pagecate').css("background-color", "red");
    //                var page = getUrlVars()["page"];
    //                $('#' + page + 'pagecate').css("background-color", "transparent");
    //                //16/10/2018
    //                //---------------------Test

    //            },
    //            error: function () {
    //                alert("Error occured!!")
    //            }
    //        });
    //    }
    //    else if (condition == 2) {
    //        var idPublisher = getUrlVars()["idpublisher"];
    //        //trả về data partialview ShowBookFollowPublisher
    //        $.ajax({
    //            type: "GET",
    //            url: "/Book/ShowBookFollowPublisher",
    //            data: {
    //                sortID: filter, page: 0, idpublisher: idPublisher, condition: condition
    //            },
    //            datatype: "html",
    //            contentType: 'application/x-www-form-urlencoded',
    //            success: function (data) {
    //                $('#listBook').html(data);
    //                $('#' + 0 + 'pagepublisher').css("background-color", "red");
    //                var page = getUrlVars()["page"];
    //                $('#' + page + 'pagepublisher').css("background-color", "transparent");
    //            },
    //            error: function () {
    //                alert("Error occured!!")
    //            }
    //        });
    //        //condition = 3 hiển thị tất cả các book
    //    }
    //    else if (condition == 3) {
    //        $.ajax({
    //            type: "GET",
    //            url: "/Book/ShowAllBook",
    //            data: {
    //                sortID: filter, page: 0, condition: condition
    //            },
    //            datatype: "html",
    //            contentType: 'application/x-www-form-urlencoded',
    //            success: function (data) {
    //                $('#listBook').html(data);
    //                $('#0page').css("background-color", "red");
    //                //---------------------Test
    //            },
    //            error: function () {
    //                alert("Error occured!!")
    //            }
    //        });
    //    }
    //});

    $('#country1').on('change', function () {
        //Biến lưu giá trị của sortID
        //sortID gồm 5 giá trị: 0(mặc định), 1(Số lượt xem), 2(Ngày xuất bản), 3(Giá giảm dần), 4(Giá tăng dần).
        var sortID = $("div.sorting select").val();
        //Biến lưu giá trị của Thể loại
        var idcate = Categoryid;
        //Biến lưu mã Nhà xuất bản
        var idpublisher = getUrlVars()["idpublisher"];
        //Biến lưu số trang hiện tại
        var page = getUrlVars()["page"];
        var condition = getUrlVars()["condition"];
        if ((idpublisher == undefined || idpublisher == null) && (idcate == undefined || idcate == null)) {
            //$.ajax({
            //    type: "GET",
            //    url: "/Book/SortBookBySortID",
            //    data: {
            //        page: 0, sortID: sortID, condition: 0
            //    },
            //    dataType: "html",
            //    contentType: 'application/x-www-form-urlencoded',
            //    success: function (data) {
            //        $('#listBook').html(data);
            //        $('#' + 0 + 'pagecate').css("background-color", "red");
            //        var page = getUrlVars()["page"];
            //        $('#' + page + 'pagecate').css("background-color", "transparent");
            //    },
            //    error: function () {
            //        alert("Lỗi ứng dụng");
            //    }
            //});

            $.ajax({
                type: "GET",
                url: "/Book/ShowAllBook",
                data: {
                    sortID: sortID, page: 0, condition: condition
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
        else if ((idpublisher == undefined || idpublisher == null) && (idcate != undefined || idcate != null)) {
            $.ajax({
                type: "GET",
                url: "/Book/SortBookByCateIDAndSortID",
                data: {
                    page: 0, sortID: sortID, idcate: idcate, condition: 0
                },
                dataType: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('#listBook').html(data);
                    $('#' + 0 + 'pagecate').css("background-color", "red");
                    var page = getUrlVars()["page"];
                    $('#' + page + 'pagecate').css("background-color", "transparent");
                },
                error: function () {
                    alert("Lỗi ứng dụng");
                }
            });
        }
        else if ((idpublisher != undefined || idpublisher != null) && (idcate == undefined || idcate == null)) {
            $.ajax({
                type: "GET",
                url: "/Book/SortBookByPublisherIDAndSortID",
                data: {
                    page: 0, sortID: sortID, idpublisher: idpublisher, condition: 0
                },
                dataType: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('#listBook').html(data);
                    $('#' + 0 + 'pagecate').css("background-color", "red");
                    var page = getUrlVars()["page"];
                    $('#' + page + 'pagecate').css("background-color", "transparent");
                },
                error: function () {
                    alert("Lỗi ứng dụng");
                }
            });
        }
        else if ((idpublisher != undefined || idpublisher != null) && (idcate != undefined || idcate != null)) {
            $.ajax({
                type: "GET",
                url: "/Book/SortBookByPublisherIDAndCateIDAndSortID",
                data: {
                    page: 0, sortID: sortID, idcate: idcate, idpublisher: idpublisher, condition: 0
                },
                dataType: "html",
                contentType: 'application/x-www-form-urlencoded',
                success: function (data) {
                    $('#listBook').html(data);
                    $('#' + 0 + 'pagecate').css("background-color", "red");
                    var page = getUrlVars()["page"];
                    $('#' + page + 'pagecate').css("background-color", "transparent");
                },
                error: function () {
                    alert("Lỗi ứng dụng");
                }
            });
        }
    });

    function SetAnnoumentSort(publisher) {
        if (publisher == null) {
            $('#annoumentSort').html();
            $('#annoumentSort').hide();
        }
        else {
            $('#annoumentSort').html(publisher);
            $('#annoumentSort').show();
        }
    }
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