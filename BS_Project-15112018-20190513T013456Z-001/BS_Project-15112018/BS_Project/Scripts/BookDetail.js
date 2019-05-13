var comment_form = '#comment-form';
var userName = $('#userName').val();
var detailCommentsDiv = "detailComments";
var content = '';
var commentsCreatedDate = moment().format("DD/MM/YYYY");

$(function () {
    SetCommentBox();
    InitialShowAnnouncements();

});

function LoadOrderbookByUserID() {
    var userID = $('#userID').val();
    var bookID = $('#BookID').val();
    $.ajax({
        url: "/Home/GetOrderbookByUserID",
        type: "POST",
        dataType: "json",
        data: { userID: userID },
        success: function (data) {
            if (data !== null) {
                for (var i = 0; i < data.length; i++) {
                    if (bookID == data[i].bookID) {
                        $(comment_form).css('visibility', 'visible');
                        break;
                    }
                }
            }
        }
    });
}

function SetCommentBox() {
    var userName = $('#userName').val();
    if (userName !== null) {
        LoadOrderbookByUserID();
    }
}

function InitialShowAnnouncements() {
    var bookID = $('#BookID').val();
    $.ajax({
        type: "post",
        url: "/Home/GetComments",
        dataType: "json",
        cache: false,
        data: { bookID: bookID },
        success: function (data) {
            ShowAnnouncements(data);
        }
    });
}
function ShowAnnouncements(data) {
    var byID = document.getElementById('hdnNumberBoardAnnouncement');
    if (byID != null) {
        var html = GetInforAnnouncements(data);
        var arr = $('#hdnNumberBoardAnnouncement').val().split(',');
        var offset = 0;
        for (var i = 0; i < 1; i++) {
            var noId = detailCommentsDiv + arr[i];
            $('#' + noId).html(html);
        }
    }
}

function GetInforAnnouncements(data) {
    var html = '';
    if (data != null) {
        for (var i = 0; i < data.length; i++) {
            var id = 'detailCommentsDiv_' + data[i].comment_id;
            html += '<div class="sortable-announcement-item" id=' + id + '>';
            if (data[i].picture != '') {
                html += '<img class="announcement-userimage" src="' + data[i].picture + '" alt=""/>';
            }
            html += '<div>';
            html += '<span class="sortable-item-date">';
            html += data[i].date;
            html += '</span';
            html += '</div>';

            if (data[i].picture == '') {
                html += '<div class="announcement-user-name1">';
                html += data[i].username;
                html += '</div>';
            }
            else {
                html += '<div class="announcement-user-name2">';
                html += data[i].username;
                html += '</div>';
            }

            id = 'detailAnnounDivContents_' + data[i].comment_id;
            html += '</div>';
            html += '<div id=' + id + ' class="sortable-item-content">';
            html += data[i].content.replace(/\n/gi, '<br />');
            html += '</div>';
            html += '</div>';
        }
    }
    return html;
}

function RegisterComments() {
    var bookID = $('#BookID').val();
    var comments = GetComments();
    if (comments == null) {
        return false;
    }

    if ($.trim(comments.Content) == '') {
        alert("Nội dung bình luận chưa được nhập.");
        $("#comment_content").focus();
        return false;
    }

    $("#btnRegisterComments").attr("disabled", true);
    $.ajax({
        type: "post",
        url: "/Home/RegisterComments",
        dataType: "json",
        cache: false,
        data: { comments: comments, bookID: bookID },
        success: function (data) {
            if (data == null) {
                alert("Đã xảy ra lỗi ứng dụng");
            }
            else {
                ShowAnnouncements(data.cmtData);
            }
            $("#btnRegisterComments").attr("disabled", false);
            $("#comment_content").val("");
            return false;
        }
    });
}

function GetComments() {
    var txtContent = $.trim($('#comment_content').val());
    if (txtContent == content) {
        txtContent = '';
    }
    IDBook = $.trim($('#BookID').val());
    IDUser = $.trim($('#userID').val());

    return {
        BookID: IDBook,
        UserID: IDUser,
        Content: HtmlEncode(txtContent),
        CreatedDate: commentsCreatedDate
    }
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