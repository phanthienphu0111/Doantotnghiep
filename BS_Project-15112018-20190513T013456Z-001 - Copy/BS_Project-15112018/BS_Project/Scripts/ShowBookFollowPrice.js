$(function () {
    $('.filter-section-checkbox').click(function () {
        $('.filter-section-checkbox').not(this).prop('checked', false);
    });
});

function onClickHandler() {
    if ($('#checkbox_1').is(':checked') == true) {
        $('#filter_1').css('visibility', 'visible');
    }
    if ($('#checkbox_1').is(':checked') == false) {
        $('#filter_1').css('visibility', 'hidden');
    }

    if ($('#checkbox_2').is(':checked') == true) {
        $('#filter_2').css('visibility', 'visible');
    }
    if ($('#checkbox_2').is(':checked') == false) {
        $('#filter_2').css('visibility', 'hidden');
    }
}
