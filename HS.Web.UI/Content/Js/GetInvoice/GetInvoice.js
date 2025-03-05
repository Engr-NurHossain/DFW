String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
function printFrame(id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var CreateEstimatePdf = function () {
    console.log("CreateEstimatePdf");
    var DetailList = [];
    parent.$(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val().trim().replaceAll(',', ''),
            TotalPrice: $(this).find('.txtProductQuantity').val() * parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', '')),
            /*EquipmentDescription: $(this).find('.txtProductDesc').val(),
            EquipmentName: $(this).find('.ProductName').val(),*/
            EquipDetail: $(this).find('.txtProductDesc').val(),
            EquipName: $(this).find('.ProductName').val(),
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
            InvoiceId: parent.InvoiceId
        });
    });
    var url = domainurl + "/Invoice/SaveInvoicePdf";
    var param = JSON.stringify({
        EmailAddress: $("#EmailAddress").val(),
        "Invoice.InvoiceId": parent.InvoiceId,
        "Invoice.BillingAddress": parent.tinyMCE.get('Invoice_BillingAddress').getContent(),
        "Invoice.ShippingAddress": parent.tinyMCE.get('Invoice_ShippingAddress').getContent(),
        "Invoice.CustomerId": parent.$("#InvoiceCustomerId").val(),
        "Invoice.CreatedDate": parent.InvoiceDatepicker.getDate(),
        "Invoice.InvoiceDate": parent.GetTimeFormat(parent.$("#Invoice_InvoiceDate").val()),
        "Invoice.DueDate": parent.GetTimeFormat(parent.$("#Invoice_DueDate").val()),
        "Invoice.TotalAmount": parent.FinalTotal,
        "Invoice.Amount": parent.TotalAmount,
        "Invoice.InvoiceMessage": parent.$("textarea#InvoiceMessage").val(),
        "Invoice.Discountpercent": parent.DiscountDBPercent,
        "Invoice.DiscountAmount": parent.DiscountDBAmount,
        "Invoice.ShippingCost": parseFloat(parent.$("#Invoice_ShippingCost").val().trim().replaceAll(',', '')),
        "Invoice.Deposit": parent.$("#Invoice_Deposit").val(),
        "Invoice.Tax": parent.TaxAmount,
        "Invoice.TaxType": parent.$("#taxType option:selected").text(),
        "Invoice.DiscountType": parent.$("#Invoice_DiscountType option:selected").val(),
        "Invoice.BalanceDue": parent.InvoiceBalanceDue,
        InvoiceDetailList: DetailList,
        InvoiceShipping: parent.$(".shippingAddress").val()
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
            $("#iframePdf").attr('src', data.filePath);
            $("#iframePdf").removeClass('hidden');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    if (typeof (PdfLocation) == 'undefined'
        || PdfLocation.trim() == '' || PdfLocation.trim() == '/') {
        CreateEstimatePdf();
    }
});