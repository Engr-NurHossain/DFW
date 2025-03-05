function printFrame(id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var CreateServiceOrderPdf = function () {
    var DetailPdfService = [];
    parent.$(".HasItem").each(function () {
        DetailPdfService.push({
            ServiceOrderEquipmentName: parent.$(this).find('.ProductName').val(),
            ServiceOrderQuantity: parent.$(this).find('.txtProductQuantity').val(),
            ServiceOrderUnitPrice: parent.$(this).find('.txtProductRate').val(),
            ServiceOrderTotalPrice: parent.$(this).find('.txtProductQuantity').val() * parent.$(this).find('.txtProductRate').val(),
        })
    });
    var TotalServiceOrderPrice = parent.$(".total").text();
    var url = domainurl + "/ServiceOrder/SaveServiceOrderPdf";
    var param = JSON.stringify({
        AppointmentId: AppointmentId,
        ServiceOrderPdfDetail: DetailPdfService,
        TotalServiceOrderPrice: TotalServiceOrderPrice,
        ServiceOrderTaxPercent: parent.$(".tax-percent").text(),
        ServiceOrderTaxAmount: parent.$(".tax-amount").text(),
        ServiceOrderSubTotalAmount: parent.$(".amount").text()
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
    CreateServiceOrderPdf();
})