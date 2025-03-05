var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}
var DetailPdf = [];
parent.$(".HasItem").each(function () {
    DetailPdf.push({
        WorkOrderEquipmentName: parent.$(this).find('.ProductName').val(),
        WorkOrderQuantity: parent.$(this).find('.txtProductQuantity').val(),
        WorkOrderUnitPrice: parent.$(this).find('.txtProductRate').val(),
        WorkOrderTotalPrice: parent.$(this).find('.txtProductQuantity').val() * parent.$(this).find('.txtProductRate').val(),
    })
});
function printFrame(id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var CreateWorkOrderPdf = function () {
    var TotalWorkOrderPrice = parent.$(".total").text();
    var url = domainurl + "/WorkOrder/SaveWorkOrderPdf";
    var param = JSON.stringify({
        AppointmentId: AppointmentId,
        WorkOrderPdfDetail: DetailPdf,
        TotalWorkOrderPrice: TotalWorkOrderPrice,
        WorkOrderTaxPercent: parent.$(".tax-percent").text(),
        WorkOrderTaxAmount: parent.$(".tax-amount").text(),
        WorkOrderSubTotalAmount: parent.$(".amount").text()
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
            console.log(data.filePath);
            $("#iframePdf").attr('src', "/" + data.filePath);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    CreateWorkOrderPdf();
});