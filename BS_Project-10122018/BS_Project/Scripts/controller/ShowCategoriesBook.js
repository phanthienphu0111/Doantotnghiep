$(function () {
    $('#keySear').on('input', function (e) {
        var keySearch = $("#keySear").val();
        $.ajax({
            type: "POST",
            url: "/Home/SearchAutoComplete",
            data: {
                keySearch: keySearch
            },
            datatype: "json",
            contentType: 'application/x-www-form-urlencoded',
            success: function (data) {
                $("#keySear").autocomplete({
                    source: data,
                    autoFocus: true,
                });
            },
            error: function () {
                alert("Error occured!!")
            }
        });
    });
});