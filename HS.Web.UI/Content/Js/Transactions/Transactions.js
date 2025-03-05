var DataTablePageSize = 50;
var TransDeleteId = 0;
var DeleteTransactionbyId = function (TransDeleteId) {
    $.ajax({
        url: domainurl + "/transaction/DeleteTransaction",
        data: { Id: TransDeleteId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    OpenTransactionTab();
                })
            } else {
                OpenErrorMessageNew("Error!", data.message)
            }
            
           
        }
    });
}
var OpenInvById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?Id=" + invId);
    }
    else if (typeof (invId) != "undefined" && invId.indexOf('INV') > -1) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?InvoiceId=" + invId);
    }
}
var OpenInvByInvId = function (invId) {
    
    if (typeof (invId) != "undefined") {
        if (typeof (customerId) == "undefined") {
            customerId = 0;
        } 
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?customerid=" + customerId + "&InvoiceId=" + invId);
    }
}
var OpenPaymentByTrId = function (trId) {
    if (typeof (trId) != "undefined" && trId > 0) {
        if (typeof (CustomerLoadId) == "undefined") {
            return;
        }
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + CustomerLoadId + "&TransactionId=" + trId);
    }
}
var ReceivePaymentByInv = function (invId) {
    if (typeof (customerId) == "undefined") {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + 0 + "&InvoiceId=" + invId);
    } else {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + customerId + "&InvoiceId=" + invId);
    }
}
var ReceivePaymentByInvAndCus = function (invId,cusId) {
    OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerGuid=" + cusId + "&InvoiceId=" + invId);
}
var DeleteTransaction = function (transactionId) {
    //Transaction/DeleteTransaction
    OpenErrorMessageNew("Error!", "Not yet implemented.");
}




$(document).ready(function () {
    
    //FundPaging(1);
   
    $(".AllPaidInvoice").click(function () {
        InvoiceType = "Paid";
        SetLoaderText();
        $(".AllFunding_Load").load(domainurl + "/Transaction/FundingListPartial/?CustomerId=" + CustomerLoadId);
    });
    $('#tblMmr tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $("#ReceivePayment").click(function () {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + customerId);
    });
    $(".DeleteTransaction").click(function (e) {
        TransDeleteId = $(e.target).parent().attr("data-id");
        console.log(TransDeleteId);
        OpenConfirmationMessageNew("Confirm?", "Are your sure you want to delete?", function () {
            DeleteTransactionbyId(TransDeleteId);
        });
    });
 
});
