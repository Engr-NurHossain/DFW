$(document).ready(function () {
    $('.selectpicker').selectpicker();
    $(".icon_masssort_eq").click(function () {
        console.log("icon_masssort_eq working");
        var orderval = $(this).attr('data-val');
        var techId = $(this).attr('data-techid');
        var Id = $(this).attr('data-id');
        RestockDataLoad(techId, Id, orderval, showallr, isSer);
    });
});