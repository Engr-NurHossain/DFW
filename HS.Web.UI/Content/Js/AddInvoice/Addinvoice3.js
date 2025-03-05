
    var SaveAndNew = function () {
        SaveInvoice(false, false, "others");
        CloseTopToBottomModal();
        OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () {
            OpenInvoiceTab();
            OpenTopToBottomModal("/Invoice/AddInvoice/?customerid=" + customerId);
        });
         
    }
var SaveAndClose = function () {
    SaveInvoice(false, false, "others");
    CloseTopToBottomModal();
    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () { OpenInvoiceTab() });
}
var SaveAndShare = function () {
    SaveInvoice(false,true);
}
var SaveAndSend = function () {
    SaveInvoice(true, false, "others");
    CloseTopToBottomModal();
    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved and Send to Customer.", function () { OpenInvoiceTab() });
}
