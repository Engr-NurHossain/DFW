var sendSMSfunction = function () {
    var url = domainurl + "/SMS/SendInvoiceText";
    var param = JSON.stringify({
        CustomerId: CustomerGuid,
        InvoiceId: Invoiceid,
        ContactNumber: $(".ContactNumber").val(),
        Message: $("#SMSDescription").val()
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                parent.OpenSuccessMessageNew("Success!", data.message, function () {
                    CloseTopToBottomModal();
                });
            } else {
                parent.OpenErrorMessageNew("Error!", data.message, function () {

                });
            }
        }
    });
}
var BothSMSAndEmailFunction = function () {
    var url = domainurl + "/SMS/SendInvoiceText";
    var param = JSON.stringify({
        CustomerId: CustomerGuid,
        InvoiceId: Invoiceid,
        ContactNumber: $(".ContactNumber").val(),
        Message: $("#SMSDescription").val(),

    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                var EmailDes = tinyMCE.get('Description').getContent();
                var EmailSub = $("#EmailSubject").val();
                var ccEmail = $("#CCEmail").val();
                var EmailAddress = $("#Invoice_InvoiceEmailAddress").val();
                console.log("tttttt");
                parent.SaveInvoice(true, false, "", EmailDes, null, EmailSub, ccEmail, EmailAddress);
                parent.ClosePopup();
            } else {
                parent.OpenErrorMessageNew("Error!", data.message, function () {

                });
            }
        }
    });
}
var SendBothEmailAndSMS = function () {
    if ($(".ContactNumber").val().trim() == ""
       || $("#SMSDescription").val().trim() == "") {
        parent.OpenErrorMessageNew("Error!", "Contact number and SMS body can't be empty.");
    }
    else {
        BothSMSAndEmailFunction();
    }
}
$(document).ready(function () {
    if ($(window).width() < 700) {
        $("#pdfView").addClass("hidden");
    }
    else {
        $("#pdfView").removeClass("hidden");
    }
    $(".btnSaveAndClose").click(function () {
        //parent.SaveAndClose();
        var EmailDes = tinyMCE.get('Description').getContent();
        var EmailSub = $("#EmailSubject").val();
        var ccEmail = $("#CCEmail").val();
        var EmailAddress = $("#Invoice_InvoiceEmailAddress").val();
        console.log(ccEmail);
        parent.SaveInvoice(true, false, "", EmailDes,Invoiceid, EmailSub, ccEmail, EmailAddress);
        parent.ClosePopup();

    });
    $(".btnSaveAndNew").click(function () {
        parent.SaveAndNew();
        parent.ClosePopup();
    });
    $(".btnSendEmailAndClose").click(function () {
        if ($(".ContactNumber").val().trim() == ""
           || $("#SMSDescription").val().trim() == "") {
            parent.OpenErrorMessageNew("Error!", "Contact number and SMS body can't be empty.");
        }
        else {
            sendSMSfunction()
        }
    });
    $(".btnSendToBoth").click(function () {
        SendBothEmailAndSMS();
    })
});