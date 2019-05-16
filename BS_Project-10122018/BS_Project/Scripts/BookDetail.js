var comment_form = '#comment-form';
var userName = $('#userName').val();
var detailCommentsDiv = "detailComments";
var replydetailComments = "ReplydetailComments";
var content = '';
var commentsCreatedDate = moment().format("DD/MM/YYYY");

$(function () {
    SetCommentBox();
    InitialShowAnnouncements();
    AutoLoadComment();
});

function ShowModalComment(data) {
    var byID = document.getElementById('modalR');
    if (byID != null) {
        var html = HTMLModalComment(data);
        $(byID).html(html);
        return false;
    }
}


function AutoLoadComment() {
    var bookID = $('#BookID').val();
    $.ajax({
        url: "/Home/GetComments",
        type: "POST",
        dataType: "json",
        data: { bookID: bookID },
        success: function (data) {
            ShowModalComment(data);
        }
    })
}

function HTMLModalComment(data) {
    var html = '';
    if (data != null) {

        for (var i = 0; i < data.length; i++) {
            var idComment = "myModal" + data[i].comment_id;
            var idBook = "RBookID" + data[i].comment_id;
            var idUSer = "RUserID" + data[i].comment_id;
            var contentReply = "ContentR" + data[i].comment_id;
            var idFolder = "RFolderID" + data[i].comment_id;
            var btnReplyComments = "btnReplyComments" + data[i].comment_id;
            var close = "close" + data[i].comment_id;

            html += '<div class="modal fade" id="' + idComment + '" tabindex = "-1" role ="dialog" >';
            html += '    <div class="modal-dialog">';
            html += '    <div class="modal-content" style="height: 265px;">';
            html += '            <div class="modal-header">';
            html += '               <button type="button" class="close" id="' + close + '" data-dismiss="modal">&times;</button>';
            html += '            </div>';
            html += '            <div class="modal-body modal-body-sub_agile">';
            html += '                <div class="col-md-8 modal_body_left modal_body_left1">';
            html += '                    <h3 class="agileinfo_sign">Reply <span>Comment</span></h3>';
            html += '                    <form action="#" method="post">';
            html += '                        <div class="styled-input agile-styled-input-top">';
            html += '                            <input type="text" name="content" id="' + contentReply + '" required="">';
            html += '                            <input type="hidden" name="@idFolder" id="' + idFolder + '" value="' + data[i].comment_id + '" />';
            html += '                            <input type="hidden" id="' + idBook + '" name="bookID" value="' + data[i].book_id +'" />';
            html += '                            <input type="hidden" id="' + idUSer + '" name="userID" value="' + data[i].user_id +'" />';
            html += '                            <label>Nhập</label>';
            html += '                            <span></span>';
            html += '                        </div>';
            html += '                            <button type="submit" class="btn btn-primary" id="' + btnReplyComments + '" onclick="ReplyComments(' + data[i].comment_id +');">Reply Comment</button>';
            html += '                    </form>';
            html += '                </div>';
            html += '            </div>';
            html += '     </div>';
            html += '     </div>';
            html += '</div>';
        }
    }
    return html;
}



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
    var idFolder
    if (data != null) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].folder_id == 0) {
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
                html += '<a href="#" data-toggle="modal" data-target="#myModal' + data[i].comment_id + '"> Trả lời </a></div>';
                html += '</div>';

                const result = data.filter(a => a.folder_id == data[i].comment_id);

                for (var j = 0; j < result.length; j++) {
                    /* ------------------------------ */

                    var ids = 'detailCommentsDiv_' + result[j].comment_id;
                    html += '<div class="sortable-announcement-item" style="margin-left: 50px" id=' + ids + '>';
                    if (result[j].picture != '') {
                        html += '<img class="announcement-userimage" src="' + result[j].picture + '" alt=""/>';
                    }
                    html += '<div>';
                    html += '<span class="sortable-item-date">';
                    html += result[j].date;
                    html += '</span';
                    html += '</div>';

                    if (result[j].picture == '') {
                        html += '<div class="announcement-user-name1">';
                        html += result[j].username;
                        html += '</div>';
                    }   
                    else {
                        html += '<div class="announcement-user-name2">';
                        html += result[j].username;
                        html += '</div>';
                    }

                    ids = 'detailAnnounDivContents_' + result[j].comment_id;
                    html += '</div>';
                    html += '<div id=' + ids + ' class="sortable-item-content">';
                    html += result[j].content.replace(/\n/gi, '<br />');
                    html += '</div>';
                    html += '</div>';
                }
            }
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
            $("#comment_content").val("");
            AutoLoadComment();
            $('#btnRegisterComments').attr("disabled", false);
            return false;
        }
    });
}

function ReplyComments(id) {
    var bookID = $('#RBookID' + id).val();
    var comments = GetReplyComments(id);
    if (comments == null) {
        return false;
    }

    if ($.trim(comments.Content) == '') {
        alert("Nội dung bình luận chưa được nhập.");
        $("#ContentR" + id).focus();
        return false;
    }

    $("#btnReplyComments" + id).attr("disabled", true);

    $.ajax({
        type: "POST",
        url: "/Home/RegisterReplyComments",
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
            $("#btnReplyComments" + id).attr("disabled", false);
            $("#close" + id).click();
            $("#ContentR" + id).val("");
            return false;
        }
    });
}

function GetReplyComments(id) {
    var txtContent = $.trim($('#ContentR' + id).val());
    
    if (txtContent == content) {
        txtContent = '';
    }
    var IDBook = $.trim($('#RBookID' + id).val());
    var IDUser = $.trim($('#RUserID' + id).val());
    var IsLike = '0';
    var RFolderID = $.trim($('#RFolderID' + id).val());

    return {
        BookID: IDBook,
        UserID: IDUser,
        Content: HtmlEncode(txtContent),
        CreatedDate: commentsCreatedDate,
        isLike: IsLike,
        FolderID: RFolderID
    };
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
    };
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