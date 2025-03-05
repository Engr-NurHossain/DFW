
    var SaveAndNew = function () {
        SaveEstimate(false, false, "others");
        CloseTopToBottomModal();
        OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () {
            OpenEstimateTab();
            OpenTopToBottomModal("/Invoice/AddInvoice/?customerid=" + customerId);
        });

    }
var SaveAndClose = function () {
    SaveEstimate(false, false, "others");
    CloseTopToBottomModal();
    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () { OpenEstimateTab() });
}
var SaveAndShare = function () {
    SaveEstimate(false, true);
    CloseTopToBottomModal();
    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved and Send to Customer.", function () { OpenEstimateTab() });
}
var SaveAndSend = function () {
    SaveEstimate(true, false, "others");
    CloseTopToBottomModal();
    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved and Send to Customer.", function () { OpenEstimateTab() });
}
