$(document).ready(function () {
    $('#txtInput').on('input', function () {
        var text = $("#txtInput").val();
        if (text === "") {
            $("#nullName").fadeIn(2000);
            $('#txtInput').css("border", "1px solid red");
            $("#btnSubmit").prop("disabled", true);
        } else {
            $.ajax({
                data: {
                    name: text
                },
                url: '/Category/CheckNameInput',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        $("#validateNameBook").hide();
                        $("#nullName").hide();
                        $('#txtInput').css("border", "0");
                        $('#txtInput').css("border", "1px solid #d2d6de");
                        $("#btnSubmit").prop("disabled", false);
                    } else {
                        $("#validateNameBook").fadeIn(2000);
                        $('#txtInput').css("border", "1px solid red");
                        $("#btnSubmit").prop("disabled", true);
                    }
                }
            });
        }

    });

    $('#txtInput').click(function () {
        $("#nullName").fadeIn(2000);
        $('#txtInput').css("border", "1px solid red");
        $("#btnSubmit").prop("disabled", true);
    });

});