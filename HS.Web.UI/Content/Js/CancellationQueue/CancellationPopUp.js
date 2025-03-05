/// <reference path="E:\Code\RMRCode\HS.Web.UI\App_Data/CancellationTemplate/Cancellation.html" />
/// <reference path="E:\Code\RMRCode\HS.Web.UI\App_Data/CancellationTemplate/Cancellation.html" />
var printFrame = function (id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var IAgree = function () {
    /*LeadConvertedToCustomer(LeadConvertId);
    LeadtoCustomerConvert(LeadConvertId);
    AllScheduleCalendar(LeadConvertId);
    LeadConvertedToCustomerPDF(LeadConvertId);*/
    IAgreeSetup(LeadConvertId);
}

var IAgreeSetup = function (idval) {
    var Id = idval;
    $(".LoadImgDiv").removeClass('hidden');
    $.ajax({
        url: domainurl + "/Leads/IAgreeSetup/",
        data: { Id },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        parent.LoadCustomer(true);
        parent.ClosePopup();
    });
}

var LeadConvertedToCustomerPDF = function (idval) {
    var Id = idval;
    $.ajax({
        url: domainurl + "/Leads/LeadConvertedToCustomerPDF/",
        data: { Id },
        type: "Post",
        dataType: "Json"
    }).done(function () {

    });
}
var AllScheduleCalendar = function (Scheduleidval) {
    var LeadScheduleId = Scheduleidval;
    $.ajax({
        url: domainurl + "/Schedule/AllScheduleCalendar/",
        data: { LeadScheduleId },
        type: "Post",
        dataType: "Json"
    }).done(function () {

    });
}
var AggrementMail = function (Leadid) {
    var leadid = Leadid;
    var PrefferedEmail = $("#EmailAddress").val();
    var URL = domainurl + "/Customer/CustomerCancellationPDFMail/";
    $(".LoadImgDiv").removeClass('hidden');
    $.ajax({
        url: URL,
        data: JSON.stringify({
            CustomerId: leadid,
            PrefferedEmail: PrefferedEmail
        }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.result == true) {
            console.log("hlwww"); 
            //parent.$("#LoadLeadDetail").load("/SmartLeads/SmartAgreementSummary?id=" + LeadConvertId);
            //parent.$("#leadToCustomerConvertForAgreement").removeClass('hidden');
            parent.OpenSuccessMessageNew("Success!", data.message, parent.ClosePopup());
            window.location.reload();

        }
        else {
            parent.OpenErrorMessageNew("Error!", data.message, function () {
                $(".LoadImgDiv").addClass('hidden');
            });
        }
    });
}

var SendAgreementLinkBySMS = function (Leadid) {
    var URL = domainurl + "/Customer/SMSCancellationAgreementLinkForPrintBlank/";
    var leadid = Leadid;
    var PrefferedNO = $("#Phone_no").val();
    console.log(leadid+ $("#Phone_no").val());
    $(".LoadImgDiv").removeClass('hidden');
    $.ajax({
        url: URL,
        data: JSON.stringify({
            leadid: leadid,
            PrefferedNO: PrefferedNO
        }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.result == true) {
            parent.OpenSuccessMessageNew("Success!", data.message, function () {
                console.log("hlwww");
                parent.$("#LoadLeadDetail").load("/SmartLeads/SmartAgreementSummary?id=" + LeadConvertId);
                parent.$("#leadToCustomerConvertForAgreement").removeClass('hidden');
                parent.ClosePopup()

            });
        }
        else {
            parent.OpenErrorMessageNew("Error!", data.message, function () {
                $(".LoadImgDiv").addClass('hidden');
            });
        }
    });
}
var SendBothMailAndSMS = function (Leadid) {
    var leadid = Leadid;
    var PrefferedEmail = $("#EmailAddress").val();
    var PrefferedNO = $("#Phone_no").val();
    var URL = domainurl + "/Customer/CustomerCancellationPDFMailAndSMS/";
    $(".LoadImgDiv").removeClass('hidden');
    $.ajax({
        url: URL,
        data: JSON.stringify({
            leadid: leadid,
            PrefferedEmail: PrefferedEmail,
            PrefferedNO: PrefferedNO
        }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (data) {
        if (data.result == true) {
            parent.$("#LoadLeadDetail").load("/SmartLeads/SmartAgreementSummary?id=" + LeadConvertId);
            parent.$("#leadToCustomerConvertForAgreement").removeClass('hidden');
            parent.OpenSuccessMessageNew("Success!", data.message, parent.ClosePopup());
        }
        else {
            parent.OpenErrorMessageNew("Error!", data.message, function () {
                $(".LoadImgDiv").addClass('hidden');
            });
        }
    });
}
var LeadtoCustomerConvert = function (Leadid) {

    var LeadtoCustomerid = Leadid;
    $.ajax({
        url: domainurl + "/Leads/LeadtoCustomerConvertQAEmail/",
        data: { LeadtoCustomerid },
        type: "Post",
        dataType: "Json"
    }).done(function () {

    });
}
var LoadCustomerSignature = function () {

}
var PrimaryPhoneFormat = function () {
    var phonenumber = $("#Phone_no").val().replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3");
    var FormatedPhoneNo = $("#Phone_no").val();
}
var counter = 0;
$(document).ready(function () {
    $(".btnSign").click(function () {
        $(".bodyConentsDiv").addClass('hidden');
        $(".sign-document").removeClass('hidden');
    });
    $("#btnSendMail").click(function () {
        AggrementMail(LeadConvertId);
    });
    $("#btnSendSMS").click(function () {
        SendAgreementLinkBySMS(LeadIdInt);
    });
    var canvas = $('#signature-pad');
    var ctx = $('#signature-pad')[0].getContext('2d');
    var signaturePad = new SignaturePad($('#signature-pad')[0]);
    function resizeCanvas() {
        var ratio = Math.max(1, 1);
        canvas.width = canvas.offsetWidth * ratio;
        canvas.height = canvas.offsetHeight * ratio;
        canvas[0].getContext("2d").scale(ratio, ratio);
    }
    resizeCanvas();
    var img = new Image();
    img.onload = function () {
        ctx.drawImage(img, 0, 0);
    };
    img.crossOrigin = 'anonymous';
    function download(dataURL, filename) {
        var blob = dataURLToBlob(dataURL);
        var url = window.URL.createObjectURL(blob);

        var a = document.createElement("a");
        a.style = "display: none";
        a.href = url;
        a.download = filename;

        document.body.appendChild(a);
        a.click();

        window.URL.revokeObjectURL(url);
    }
    function dataURLToBlob(dataURL) {
        var parts = dataURL.split(';base64,');
        var contentType = parts[0].split(":")[1];
        var raw = window.atob(parts[1]);
        var rawLength = raw.length;
        var uInt8Array = new Uint8Array(rawLength);

        for (var i = 0; i < rawLength; ++i) {
            uInt8Array[i] = raw.charCodeAt(i);
        }

        return new Blob([uInt8Array], { type: contentType });
    }
    $('#clear').click(function () {
        signaturePad.clear();
    });
    $("#save-png").click(function () {
        if (signaturePad.isEmpty()) {
            parent.OpenErrorMessageNew("Error!", "Please Draw Your Signature", "");
        }
        else {
            counter += 1;
            var data = signaturePad.toDataURL('image/png');
            var url = domainurl + "/SmartLeads/LoadCustomerSignatureImage";
            $.ajax({
                url: url,
                data: { data, LeadConvertId },
                type: "Post",
                dataType: "Json",
                success: function (data) {
                    if (data.uploadImage == true) {
                        parent.OpenSuccessMessageNew("Success!", "Signature added successfully. Scroll at the bottom to fully review and submit document.", function () {
                            $(".sign-document").addClass('hidden');
                            $(".bodyConentsDiv").removeClass('hidden');
                            $(".LoadImgDiv").removeClass('hidden');
                            var ifr = $('iframe')[0];
                            ifr.src = ifr.src
                        });
                        setTimeout(function () {
                            $(".LoadImgDiv").addClass('hidden');
                        }, 10000);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            })
        }
    });
    //$(".warning-error").hide();
    $(".btnIAgree").attr("disabled", true);

    $(".btnIAgree").click(function () {
        if (!signaturePad.isEmpty() && counter == 1) {
            //$(".warning-error").hide();
            IAgree();
        }
        else {
            if (signaturePad.isEmpty()) {
                $(".wrapper").css("border-color", "red");
                parent.OpenErrorMessageNew("Error!", "Please press sign document", "");
            }
            if (!signaturePad.isEmpty() && counter == 0) {
                parent.OpenErrorMessageNew("Error!", "Please Save your signature first", "");
                $(".wrapper").css("border-color", "#ccc");
            }
            //if (!$(".chechbox-IAgree").is(':checked'))
            //    $(".warning-error").show();
        }
    })
    $("#AggrementDiv").scroll(function () {
        var MathScroll = ($("#AggrementDiv iframe").height() - $("#AggrementDiv").scrollTop());
        console.log(MathScroll);
        //var Globalval = parseInt();
        if (MathScroll <= 500) {
            $(".btnIAgree").removeAttr("disabled");
        }
        else {
            $(".btnIAgree").attr("disabled", true);

            $(".btnIAgree").addClass('DisablebtnAgree');
        }
    })
    $("#btnSendMailAndSMS").click(function () {
        SendBothMailAndSMS(LeadIdInt);
    })
})
