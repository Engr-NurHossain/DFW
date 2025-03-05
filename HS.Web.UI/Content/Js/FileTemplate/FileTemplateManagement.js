var CustomerConvertId = $("#CustomerId").val();
var printFrame = function (id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var FileMail = function (cusid) {
    var cusid = cusid;
    var fileid = $("#FileTemplateId").val();
    var PrefferedEmail = $("#EmailAddress").val();
    var URL = domainurl + "/File/FileToCustomerPDFMail/";
    $.ajax({
        url: URL,
        data: JSON.stringify({
            cusid: cusid,
            PrefferedEmail: PrefferedEmail,
            fileid:fileid
        }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.result == true) {
            console.log("hlwww");
            parent.OpenSuccessMessageNew("Success!", data.message, function(){
                CloseTopToBottomModal();
                location.reload();
            });
        }
        else {
            parent.OpenErrorMessageNew("Error!", data.message);
        }
    });
}
var FileMailAndSMS = function (cusid) {
    var cusid = cusid;
    var fileid = $("#FileTemplateId").val();
    var PrefferedEmail = $("#EmailAddress").val();
    var PrefferedNO = $("#Phone_no").val();
    var URL = domainurl + "/File/FileToCustomerPDFMailAndSMS/";
    $.ajax({
        url: URL,
        data: JSON.stringify({
            cusid: cusid,
            PrefferedEmail: PrefferedEmail,
            PrefferedNO: PrefferedNO,
            fileid: fileid
        }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.result == true) {
            console.log("hlwww");
            parent.OpenSuccessMessageNew("Success!", data.message, function () {
                CloseTopToBottomModal();
                location.reload();
            });
        }
        else {
            parent.OpenErrorMessageNew("Error!", data.message);
        }
    });
}
var FileLinkSMS = function (cusid) {
    var URL = domainurl + "/File/SMSFileLinkForPrintBlank/";
    var cusid = cusid;
    var PrefferedNO = $("#Phone_no").val();
    var fileid = $("#FileTemplateId").val();
    $.ajax({
        url: URL,
        data: JSON.stringify({
            cusid: cusid,
            PrefferedNO: PrefferedNO,
            fileid: fileid
        }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.result == true) {
            console.log("hlwww");
            parent.OpenSuccessMessageNew("Success!", data.message, function () {
                CloseTopToBottomModal();
                location.reload();
            });
        }
        else {
            parent.OpenErrorMessageNew("Error!", data.message);
        }
    });
}
$(document).ready(function () {
    if (window.innerWidth > 1025) {
        $(".file_template_iframe_height").height(window.innerHeight - 225);
    }
    else {
        $(".file_template_iframe_height").height(300);
    }
    
    $("#btnSendMail").click(function () {
        if ($("#EmailAddress").val() != "") {
            FileMail(CustomerConvertId);
        }
        else {
            parent.OpenErrorMessageNew("Error!", "Cannot send file without an email address.");
        }
    });
    $("#btnSendSMS").click(function () {
        if ($("#Phone_no").val() != "") {
            FileLinkSMS(CustomerConvertId);
        }
        else {
            parent.OpenErrorMessageNew("Error!", "Cannot send file without a phone number.");
        }
    });
    $("#btnSendMailAndSMS").click(function () {
        if ($("#EmailAddress").val() != "" && $("#Phone_no").val() != "") {
            FileMailAndSMS(CustomerConvertId);
        }
        else {
            parent.OpenErrorMessageNew("Error!", "Cannot send file without an email address and phone number.");
        }
    });

    $("#AggrementDiv").scroll(function () {
        var MathScroll = ($("#AggrementDiv iframe").height() - $("#AggrementDiv").scrollTop());
        console.log(MathScroll);
    })
});
$(window).resize(function () {
    if (window.innerWidth > 1025) {
        $(".file_template_iframe_height").height(window.innerHeight - 225);
    }
    else {
        $(".file_template_iframe_height").height(300);
    }
});