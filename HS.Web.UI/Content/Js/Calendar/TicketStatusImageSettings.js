$(document).ready(function () {
    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_Doc").removeClass('hidden');
            $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/blank_thumb_file.png');
            $(".chooseFilebtn").removeClass("hidden");
            $(".changeFilebtn").addClass("hidden");
            $(".deleteDoc").addClass("hidden");
            $("#Preview_Doc").attr('src', "");
            $("#Frame_Doc").attr('src', "");
            $("#UploadSuccessMessage").hide();
            $("#description").val("");
            $("#UploadedPath").val('');
            $(".fileborder").addClass('border_none');
            $("#UploadCustomerFileBtn").removeClass('otherfileposition');
        });
    });
    $("#TicketStatusImageSetting_TicketStatus").change(function () {
        $(".modal-body").load(domainurl + "/Calendar/TicketStatusImageSettingPartial?status=" + encodeURI($(this).val()));
    });
    $(".close").click(function () {
        window.location = domainurl + "/calendarsetup";
    });
})
window.addEventListener("load", startup, false);
var colorWell, defaultColor = $("#ColorDisplay").val();
function startup() {
    colorWell = document.querySelector("#ColorDisplay");
    colorWell.value = defaultColor;
    colorWell.addEventListener("input", updateFirst, false);
    colorWell.addEventListener("change", updateAll, false);
    colorWell.select();
}