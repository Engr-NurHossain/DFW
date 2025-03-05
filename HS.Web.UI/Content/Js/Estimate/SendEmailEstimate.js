var BothSMSAndEmailFunction = function () {
    var url = domainurl + "/SMS/SendEstimateText";
    var param = JSON.stringify({
        CustomerId: LeadId,
        InvoiceId: EstimateId,
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
                var EmailDes = tinyMCE.get('EmailDescription').getContent();
                var EmailSub = $("#EmailSubject").val();
                var CCEmail = $("#CCEmail").val();
                var InvoiceEmailAddress = $("#EmailAddress").val();
                var attachedments = $("#AttachmentsEstimate").val();
                parent.SaveEstimate(true, false, "", EmailDes, EmailSub, CCEmail, false, InvoiceEmailAddress, attachedments);
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
var AddCustomerFile = function() {
    OpenRightToLeftModal(domainurl + "/File/Addfile/" + parent.CustomerLoadId); 
}

$(document).ready(function () {
    if ($(window).width() < 700) {
        $("#pdfView").addClass("hidden");
    }
    else {
        $("#pdfView").removeClass("hidden");
    }
    $(".AttachmentsEstimate").select2();
    $(".btnSaveAndClose").click(function () {
        //parent.SaveAndClose();
        //parent.ClosePopup();
        var EmailDes = tinyMCE.get('EmailDescription').getContent();
        var EmailSub = $("#EmailSubject").val();
        var CCEmail = $("#CCEmail").val();
        var InvoiceEmailAddress = $("#EmailAddress").val();
        var attachedments = $("#AttachmentsEstimate").val();
        console.log("test");
        parent.SaveEstimate(true, false, "", EmailDes, EmailSub, CCEmail, false, InvoiceEmailAddress, attachedments);
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
        var url = domainurl + "/SMS/SendEstimateText";
        var param = JSON.stringify({
            CustomerId: LeadId,
            InvoiceId: EstimateId,
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
                        parent.CloseTopToBottomModal();
                        //parent.OpenLeadEstimateTab();
                        parent.ClosePopup();
                    });
                } else {
                    parent.OpenErrorMessageNew("Error!", data.message, function () {

                    });
                }
            }
        });
    });
    $(".btnSendToBoth").click(function () {
        SendBothEmailAndSMS();
    })

});