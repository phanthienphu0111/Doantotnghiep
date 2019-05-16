// ALL RIGHTS RESERVED. COPYRIGHT (C) 2012 ソリマチ株式会社
// ダイアログの共通PluginScript

(function ($) {

    $.alerts = {

        // ダイアログプロパティ
        verticalOffset: -75,
        horizontalOffset: 0,
        repositionOnResize: false,
        overlayOpacity: .01,
        overlayColor: '#FFF',
        draggable: false,
        resizable: false,
        top: 0,
        left: 0,
        width: 0,
        height: 0,
        dialogClass: null,
        hideClose: false,

        // Public methods
        alert: function (message, title, callback) {
            if (title == null) title = 'Alert';
            $.alerts._show(title, message, null, 'alert', function (result) {
                if (callback) callback(result);
            });
        },

        confirm: function (message, title, callback) {
            if (title == null) title = 'Confirm';
            $.alerts._show(title, message, null, 'confirm', function (result) {
                if (callback) callback(result);
            });
        },

        confirm_normal: function (message, title, callback) {
            if (title == null) title = 'Confirm';
            $.alerts._show(title, message, null, 'confirm_normal', function (result) {
                if (callback) callback(result);
            });
        },

        confirm_err: function (message, title, callback) {
            if (title == null) title = 'Error';
            $.alerts._show(title, message, null, 'confirm_err', function (result) {
                if (callback) callback(result);
            });
        },

        confirm_notice: function (message1, message2, title, callback) {
            if (title == null) title = 'Confirm';
            $.alerts._show(title, message1, message2, 'confirm_notice', function (result) {
                if (callback) callback(result);
            });
        },

        confirm_notice_err: function (message1, message2, title, callback) {
            if (title == null) title = 'Error';
            $.alerts._show(title, message1, message2, 'confirm_notice_err', function (result) {
                if (callback) callback(result);
            });
        },

        confirm_import_detail_plan: function (message, title, callback) {
            if (title == null) title = 'Xác nhận';
            $.alerts._show(title, message, null, 'confirm_import_detail_plan', function (result) {
                if (callback) callback(result);
            });
        },

        prompt_password: function (title, callback) {
            if (title == null) title = 'Change password';
            $.alerts._show(title, null, null, 'prompt_password', function (result) {
                if (callback) callback(result);
            });
        },
        
        notice_password: function (title, callback) {
            if (title == null) title = 'Notice password';
            $.alerts._show(title, null, null, 'notice_password', function (result) {
                if (callback) callback(result);
            });
        },

        warning: function (message, title, callback) {
            if (title == null) title = 'Warning';
            $.alerts._show(title, message, null, 'warning', function (result) {
                if (callback) callback(result);
            });
        },

        warning_err: function (message, title, callback) {
            if (title == null) title = 'Error';
            $.alerts._show(title, message, null, 'warning_err', function (result) {
                if (callback) callback(result);
            });
        },

       _show: function (title, msg1, msg2, type, callback) {

            $.alerts._hide();
            $.alerts._overlay('show');

            $("BODY").append(
			  '<div id="popup_container">' +
			    '<div id="popup_title"><div class="title" id="title"></div><div class="titlebar-close" id="popup-close" ></div></div>' +
			    '<div id="popup_content">' +
			      '<div id="popup_message"></div>' +
				'</div>' +
			  '</div>');

            if ($.alerts.dialogClass) $("#popup_container").addClass($.alerts.dialogClass);

            // IE6 Fix
            var pos = ($.browser.msie && parseInt($.browser.version) <= 6) ? 'absolute' : 'fixed';

            // ダイアログコンテナCSSをセットする
            $("#popup_container").css({
                position: pos,
                zIndex: 99999,
                padding: 0,
                margin: 0
            });

            // ダイアログのタイトルをセットする
            $("#title").text(title);
            // ダイアログのメッセージをセットする
            $("#popup_message").text(msg1);
            $("#popup_message").html($("#popup_message").text().replace(/\n/g, '<br />'));

            $.alerts._reposition();
            $.alerts._maintainPosition(true);

            $("#popup-close").click(function () {
                $.alerts._hide();
            });

            if ($.alerts.hideClose) {
                $("#popup-close").hide();
            } else {
                $("#popup-close").show();
            }

            switch (type) {
                case 'alert':

                case 'warning':
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_yes.png" id="popup_ok" /></div>');
                    $("#popup_ok").click(function () {
                        $.alerts._hide();
                        callback(true);
                    });
                    $("#popup_container, #popup_ok").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_ok").trigger('click');
                    });
                    break;
                case 'warning_err':
                    $("#popup_content").addClass('error');
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_yes.png" id="popup_ok" /></div>');
                    $("#popup_ok").click(function () {
                        $.alerts._hide();
                        callback(true);
                    });
                    $("#popup_container, #popup_ok").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_ok").trigger('click');
                    });
                    break;
                case 'confirm':
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_yes.png" id="popup_ok" /> <input type="image" src="../../Content/images/button_small_gray_no.png" id="popup_cancel" /></div>');
                    $("#popup_ok").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });
                    $("#popup_cancel").click(function () {
                        $.alerts._hide();
                        if (callback) callback(false);
                    });

                    $("#popup_container, #popup_ok, #popup_cancel").keypress(function (e) {
                        if (e.keyCode == 27) $("#popup_cancel").trigger('click');
                    });

                    $("#popup_cancel").focus();

                    if ($("#popup_ok").focus() == true) {
                        $("#popup_container, #popup_ok, #popup_cancel").keypress(function (e) {
                            if (e.keyCode == 13) $("#popup_ok").trigger('click');
                        });
                    }

                    if ($("#popup_cancel").focus() == true) {
                        $("#popup_container, #popup_ok, #popup_cancel").keypress(function (e) {
                            if (e.keyCode == 13) $("#popup_cancel").trigger('click');
                        });
                    }

                    break;
                case 'confirm_err':
                    $("#popup_content").addClass('error');
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_confirm.png" id="popup_confirm" /></div>');
                    $("#popup_confirm").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });

                    $("#popup_container, #popup_confirm").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_confirm").trigger('click');
                    });

                    break;
                case 'confirm_normal':
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_confirm.png" id="popup_confirm" /></div>');
                    $("#popup_confirm").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });

                    $("#popup_container, #popup_confirm").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_confirm").trigger('click');
                    });

                    break;
                case 'confirm_notice':
                    $("#popup_message").html('<div id="confirm_name" class ="confirm_name">' + msg2 + '</div><div id="confirm_message" class ="confirm_message">' + msg1 + '</div>');
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_confirm.png" id="popup_confirm" /></div>');
                    $("#popup_confirm").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });

                    $("#popup_container, #popup_confirm").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_confirm").trigger('click');
                    });

                    break;
                case 'confirm_notice_err':
                    $("#popup_message").html('<div id="confirm_name" class ="confirm_name">' + msg2 + '</div><div id="confirm_message_error" class ="confirm_message_error">' + msg1 + '</div>');
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_confirm.png" id="popup_confirm" /></div>');
                    $("#popup_confirm").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });

                    $("#popup_container, #popup_confirm").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_confirm").trigger('click');
                    });

                    break;
                case 'confirm_import_detail_plan':
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_import.png" id="popup_import" /><input type="image" src="../../Content/images/button_small_gray_cancel.png" id="popup_cancel" /></div>');
                    $("#popup_import").click(function () {
                        $.alerts._hide();
                        if (callback) callback(true);
                    });
                    $("#popup_cancel").click(function () {
                        $.alerts._hide();
                        if (callback) callback(false);
                    });
                    $("#popup_container, #popup_import").focus().keypress(function (e) {
                        if (e.keyCode == 13) $("#popup_import").trigger('click');
                    });

                    break;

                case 'prompt_password':
                    var changePassDiv = '<div class="change-pass-input" id="pass_input">';
                    changePassDiv += '<div class="pass-input-error"><span id="pass_err">Thay đổi mật khẩu đăng nhập.</span></div>';
                    changePassDiv += '<div class="change-pass-input-block">';
                    changePassDiv += '<div class="pass-editor-label">Mật khẩu hiện tại</div>';
                    changePassDiv += '<div class="pass-editor-field"><input type="password" name="CurrentPass" id="CurrentPass" maxlength="20" size="25" /></div>';
                    changePassDiv += '</div>';

                    changePassDiv += '<div class="change-pass-input-block">';
                    changePassDiv += '<div class="pass-editor-label">Mật khẩu mới</div>';
                    changePassDiv += '<div class="pass-editor-field"><input type="password" name="NewPass" id="NewPass" maxlength="20" size="25" /></div>';
                    changePassDiv += '</div>';

                    changePassDiv += '<div class="change-pass-input-block">';
                    changePassDiv += '<div class="pass-editor-label">Xác nhận mật khẩu</div>';
                    changePassDiv += '<div class="pass-editor-field"><input type="password" name="NewPassConfirm" id="NewPassConfirm" maxlength="20" size="25" /></div>';
                    changePassDiv += '</div>';

                    changePassDiv += '<div class="change-pass-info">';
                    changePassDiv += '<div>Vui lòng nhập mật khẩu bao gồm chữ cái, số và ký tự đặc biệt.</div>';
                    changePassDiv += '<div>Chú ý các ký tự (/、\\、#、?、&) không được sử dụng cho mật khẩu.</div>';
                    changePassDiv += '<div>Mật khẩu không phân biệt chữ hoa và chữ thường.</div>';
                    changePassDiv += '<div>Ví dụ：abc@12</div>';
                    changePassDiv += '</div>';
                    $("#popup_message").html(changePassDiv);
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_regist.png" id="popup_regis" /><input type="image" src="../../Content/images/button_small_gray_cancel.png" id="popup_cancel" /></div>');

                    $("#popup_regis").click(function () {
                        if (callback) callback(true);
                    });
                    $("#popup_cancel").click(function () {
                        $.alerts._hide();
                    });

                    $("#popup_container, #popup_regis, #popup_cancel").keypress(function (e) {
                        if (e.keyCode == 27) $("#popup_cancel").trigger('click');
                    });

                    $("#popup_cancel").focus().select();

                    if ($("#popup_regis").focus() == true) {
                        $("#popup_container, #popup_regis, #popup_cancel").keypress(function (e) {
                            if (e.keyCode == 13) $("#popup_ok").trigger('click');
                        });
                    }

                    if ($("#popup_cancel").focus() == true) {
                        $("#popup_container, #popup_regis, #popup_cancel").keypress(function (e) {
                            if (e.keyCode == 13) $("#popup_cancel").trigger('click');
                        });
                    }
                    $("#popup_content").css("height", "255px");
                    $("#popup_message").css("height", "210px");
                    $("#popup_container").css("height", "320px");

                    break;
                
                case 'notice_password':

                    var noticePassDiv = '<div class="notice-pass-info">';
                    noticePassDiv += '<div>Vui lòng nhập mật khẩu bao gồm chữ cái, số và ký tự đặc biệt.</div>';
                    noticePassDiv += '<div>Chú ý các ký tự (/、\\、#、?、&) không được sử dụng cho mật khẩu.</div>';
                    noticePassDiv += '<div>　　　　Mật khẩu không phân biệt chữ hoa và chữ thường.</div>';
                    noticePassDiv += '<div>Ví dụ：abc@12</div>';
                    noticePassDiv += '</div>';

                    $("#popup_message").html(noticePassDiv);
                    $("#popup_message").after('<div id="popup_panel"><input type="image" src="../../Content/images/button_small_gray_close.png" id="popup_close" /></div>');

                    $("#popup_close").click(function () {
                        $.alerts._hide();
                    });

                    $("#popup_container, #popup_close").keypress(function (e) {
                        if (e.keyCode == 27) $("#popup_close").trigger('click');
                    });

                    $("#popup_close").focus().select();

                    if ($("#popup_close").focus() == true) {
                        $("#popup_container, #popup_close").keypress(function (e) {
                            if (e.keyCode == 13) $("#popup_close").trigger('click');
                        });
                    }
                    $("#popup_content").css("height", "135px");
                    $("#popup_message").css("height", "105px");
                    $("#popup_container").css("height", "205px");

                    break;
            }

           
            if ($.alerts.draggable) {
                try {
                    $("#popup_container").draggable({ handle: $("#popup_title") });
                    $("#popup_title").css({ cursor: 'move' });
                } catch (e) { /* requires jQuery UI draggables */ }
            }
           
            if ($.alerts.resizable) {
                try {
                    $("#popup_container").resizable();
                } catch (e) { /* requires jQuery UI resizable */ }
            }
        },

        _hide: function () {
            $("#popup_container").remove();
            $.alerts._overlay('hide');
            $.alerts._maintainPosition(false);
        },

        _overlay: function (status) {
            switch (status) {
                case 'show':
                    $.alerts._overlay('hide');
                    $("BODY").append('<div id="popup_overlay"></div>');
                    $("#popup_overlay").css({
                        position: 'fixed',
                        zIndex: 99998,
                        top: '0px',
                        left: '0px',
                        width: '100%',
                        height: '100%',
                        background: $.alerts.overlayColor,
                        opacity: $.alerts.overlayOpacity
                    });
                    break;
                case 'hide':
                    $("#popup_overlay").remove();
                    break;
            }
        },

        _reposition: function () {

            var top = $.alerts.top;
            var left = $.alerts.left;
            var width = $.alerts.width;
            var height = $.alerts.height;

            if (top <= 0) top = (($(window).height() / 2) - ($("#popup_container").outerHeight() / 2));
            if (left <= 0) left = (($(window).width() / 2) - ($("#popup_container").outerWidth() / 2));
            if (width <= 0) width = $("#popup_container").outerWidth();

            // IE6 fix
            if ($.browser.msie && parseInt($.browser.version) <= 6) top = top + $(window).scrollTop();

            if ($.alerts.resizable) {
                height = $("#popup_container").outerHeight() + 27;
                $("#popup_container").css({
                    top: top + 'px',
                    left: left + 'px',
                    width: width + 20 + 'px',
                    minWidth: width + 20 + 'px',
                    minHeight: height + 'px',
                    height: height + 'auto'
                });
            }
            else {
                height = $("#popup_container").outerHeight();
                $("#popup_container").css({
                    top: top + 'px',
                    left: left + 'px',
                    width: width + 'px',
                    height: height + 'auto'
                });


            }

            $("#popup_overlay").height($(document).height());
        },

        _maintainPosition: function (status) {
            if ($.alerts.repositionOnResize) {
                switch (status) {
                    case true:
                        $(window).bind('resize', $.alerts._reposition);
                        break;
                    case false:
                        $(window).unbind('resize', $.alerts._reposition);
                        break;
                }
            }
        }

    }

    // Shortcut functions
    jAlert = function (message, title, callback) {
        $.alerts.alert(message, title, callback);
    }

    jConfirm = function (message, title, callback) {
        $.alerts.confirm(message, title, callback);
    };

    jConfirmNormal = function (message, title, callback) {
        $.alerts.confirm_normal(message, title, callback);
    };

    jConfirmError = function (message, title, callback) {
        $.alerts.confirm_err(message, title, callback);
    };

    jConfirmNotice = function (message1, message2, title, callback) {
        $.alerts.confirm_notice(message1, message2, title, callback);
    };

    jConfirmNoticeError = function (message1, message2, title, callback) {
        $.alerts.confirm_notice_err(message1, message2, title, callback);
    };

    jConfirmImportDetailPlan = function (message1, message2, title, callback) {
        $.alerts.confirm_import_detail_plan(message1, message2, title, callback);
    };

    jPromptPassword = function (title, callback) {
        $.alerts.prompt_password(title, callback);
    };
    
    jNoticePassword = function (title, callback) {
        $.alerts.notice_password(title, callback);
    };

    jWarning = function (message, title, callback) {
        $.alerts.warning(message, title, callback);
    };

    jWarningError = function (message, title, callback) {
        $.alerts.warning_err(message, title, callback);
    };

})(jQuery);