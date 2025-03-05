$(document).ready(function () {
    $("#savesign").hide();
    $("#TicketTypeSettingId").change(function () {
        $(".modal-body").load(domainurl + "/Calendar/TicketTypeColorChange?status=" + encodeURI($(this).val()));
    });
    $("#btntypecolorsetting").click(function () {
        if (CommonUiValidation() && $("#ColorDisplay").val() != "" && $("#lookupId").val() > 0) {
            SaveTicketStatusColor();
        }
    });
    $(".close").click(function () {
        window.location = domainurl + "/calendarsetup";
    });
});
window.addEventListener("load", startup, false);
var colorWell, defaultColor = $("#ColorDisplay").val();
function startup() {
    colorWell = document.querySelector("#ColorDisplay");
    colorWell.value = defaultColor;
    colorWell.addEventListener("input", updateFirst, false);
    colorWell.addEventListener("change", updateAll, false);
    colorWell.select();
}
var SaveTicketStatusColor = function () {
    var url = domainurl + "/Calendar/SaveTicketTypeColorChange/";
    var param = JSON.stringify({
        Id: $("#lookupId").val(),
        AlterDisplayText: $("#ColorDisplay").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $("#savesign").show();
            $("#savesign").fadeOut(3000);
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}