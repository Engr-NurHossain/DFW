
        function printFrame(id) {
            var frm = document.getElementById(id).contentWindow;
            frm.focus();// focus on contentWindow is needed on some ie versions
            frm.print();
            return false;
        }
var CreateEstimatePdf = function () {
    var DetailList = [];
    parent.$(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val(),
            TotalPrice: $(this).find('.txtProductQuantity').val() * $(this).find('.txtProductRate').val(),
            /*EquipmentDescription: $(this).find('.txtProductDesc').val(),
            EquipmentName: $(this).find('.ProductName').val(),*/

            EquipDetail: $(this).find('.txtProductDesc').val(),
            EquipName: $(this).find('.ProductName').val(),

            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
            InvoiceId: parent.InvoiceId
        });
    });
    var url = "/Estimate/EstimateEmailPreview";
    var param = JSON.stringify({
        EmailAddress: $("#EmailAddress").val(),
        "Invoice.InvoiceId": parent.InvoiceId,
        "Invoice.BillingAddress": parent.$("#Invoice_BillingAddress").val(),
        "Invoice.CustomerId": parent.$("#CustomerList").val(),
        "Invoice.CreatedDate": parent.InvoiceDatepicker.getDate(),
        "Invoice.DueDate": parent.DueDatepicker.getDate(),
        "Invoice.TotalAmount": parent.TotalAmount,
        "Invoice.InvoiceMessage": parent.$("textarea#InvoiceMessage").val(),
        InvoiceDetailList: DetailList,
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
            $("#iframePdf").attr('src',"/"+data.filePath);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    CreateEstimatePdf();
});
