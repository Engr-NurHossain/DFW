
    //var domainurl = "@DomainURL";


var SendCustomerEmail = function (EmailDescription, EmailSubject, ccEmail,EmailAddress) {


        console.log("not returned");


    //if (typeof (SendEmail) == "undefined") {
    //    SendEmail = false;
    //}
    //if (typeof (CreatePdf) == "undefined") {
    //    CreatePdf = false;
    //}
    //if (typeof (CameFrom) == "undefined") {
    //    CameFrom = "";
    //}
    //if (typeof (EmailSubject) == "undefined") {
    //    EmailSubject = "";
    //}

    var url = domainurl + "/Customer/SendCustomerInfoEmail";

    var param = JSON.stringify({
        EmailAddress: EmailAddress,
    EmailDescription: EmailDescription,
    EmailSubject: EmailSubject,
    ccEmail: ccEmail,
    CustomerId: CustomerGuid
});

    $.ajax({
        type: "POST",
    ajaxStart: $(".AddInvoiceLoader").removeClass('hidden'),
    url: url,
    data: param,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    cache: false,
        success: function (data) {
        $(".AddInvoiceLoader").addClass('hidden');

    if (data.result)
            {
        parent.OpenSuccessMessageNew("Success!", data.message, function () {
            parent.ClosePopup();
        });
    //  CloseTopToBottomModal();
    }
            else if (!data.result) {
        OpenErrorMessageNew("Error!", data.message);
    }
},
        error: function (jqXHR, textStatus, errorThrown) {
        console.log(errorThrown);
    $(".AddInvoiceLoader").addClass('hidden');
}
});

}

        var BothSMSAndEmailFunction = function () {
    var url = domainurl + "/Customer/SendCustomerInfoEmailandSMS";
    var param = JSON.stringify({
        CustomerId: CustomerGuid,
    EmailAddress:  $("#Customer_EmailAddress").val(),
    EmailDescription: tinyMCE.get('Description').getContent(),
    EmailSubject: $("#EmailSubject").val(),
    ccEmail: $("#CCEmail").val(),
        ContactNumber: $(".customercontactnumber").val(),
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
               parent.OpenSuccessMessageNew("Success!", data.message, function () {
                   parent.ClosePopup();
        });
    } else {
        parent.OpenErrorMessageNew("Error!", data.message, function () {
        });
    }
}
});
}

        var SendBothEmailAndSMS = function () {
            if ($(".customercontactnumber").val().trim() == ""
        || $("#SMSDescription").val().trim() == ""
    || $("#Customer_EmailAddress").val().trim() == "" ) {
        parent.OpenErrorMessageNew("Error!", "Email and Contact Number can't be empty.");
    }
    else {
        BothSMSAndEmailFunction();
    }
}
        var sendSMSfunction = function () {
    var url = domainurl + "/SMS/SendCustomerInfoText";
    var param = JSON.stringify({
        CustomerId: CustomerGuid,
        ContactNumber: $(".customercontactnumber").val(),
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
            parent.ClosePopup();
        });
    } else {
        parent.OpenErrorMessageNew("Error!", data.message, function () {

        });
    }
}
});
}
        $(document).ready(function () {
            $(".share_customer_height").height(window.innerHeight - 104);
        //if ($(window).width() < 700) {
        //    $("#pdfView").addClass("hidden");
        //}
        //else {
        //    $("#pdfView").removeClass("hidden");
        //}
        $(".btnsendcusinfoemail").click(function () {
            //parent.SaveAndClose();
            var EmailDes = tinyMCE.get('Description').getContent();
            var EmailSub = $("#EmailSubject").val();
            var ccEmail = $("#CCEmail").val();
            var EmailAddress = $("#Customer_EmailAddress").val();
            console.log(ccEmail);
            if ($("#Customer_EmailAddress").val().trim() == "") {
                parent.OpenErrorMessageNew("Error!", "Email can't be empty.");
            }
            else {
                SendCustomerEmail(EmailDes, EmailSub, ccEmail, EmailAddress);

            }
            //parent.ClosePopup();

        });
    $(".btnSaveAndNew").click(function () {
        parent.SaveAndNew();
    parent.ClosePopup();
});
    $(".btnsendcusinfosms").click(function () {
        if ($(".customercontactnumber").val().trim() == ""
           || $("#SMSDescription").val().trim() == "") {
        parent.OpenErrorMessageNew("Error!", "Contact number and SMS body can't be empty.");
    }
        else {
        sendSMSfunction();
    }
});
    $(".btncusinfoSendToBoth").click(function () {
        SendBothEmailAndSMS();
    })
});
$(window).resize(function () {
    $(".share_customer_height").height(window.innerHeight - 104);
})