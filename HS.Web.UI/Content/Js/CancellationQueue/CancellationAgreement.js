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
var LoadHtmlForMobileAgreement = function () {
    var Url = domainurl + "/SmartLeads/SmartInstallationAgreement/?url=";
    $("#AggrementDiv_mobile").load(Url);
}
var EventDocumentClick = function () {
    var inFormOrLink;
    $('a').on('click', function () { inFormOrLink = true; });
    $('form').on('submit', function () { inFormOrLink = true; });

    $(window).on("beforeunload", function () {
        return inFormOrLink ? "Do you really want to close?" : null;
    })
};
var IAgreeSetup = function (idval) {
    var Id = idval;
    $(".LoadImgDiv").removeClass('hidden');
    $(".LoadImgDiv_mobile").removeClass('hidden');
    $.ajax({
        url: AgreeUrl,
        data: { Id },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        $(".LoadImgDiv").addClass('hidden');
        $(".LoadImgDiv_mobile").addClass('hidden');
        if (IsMobile) {
            var ifr = $('iframe')[0];
            if (typeof (ifr) != "undefined" && ifr != null && ifr != "") {
                ifr.src = ifr.src;
            }
            OpenSuccessMessageNew("Success!", "Your form has been signed and sent to our office. You will also receive an email confirmation with your signed form!", function () {
                $(".footerContents-agreement").addClass('hidden');
                $(".sign-div").addClass('hidden');
            })
        }
        else {
            console.log("LoadImgDiv");

            var ifr = $('iframe')[0];
            ifr.src = ifr.src
            OpenSuccessMessageNew("Success!", "Your form has been signed and sent to our office. You will also receive an email confirmation with your signed form!", function () {
                $(".footerContents-agreement").addClass('hidden');
                $(".sign-div").addClass('hidden');
            })
        }
    });
}
var LeadConvertedToCustomer = function (idval) {
    var Id = idval;
    $.ajax({
        url: domainurl + "/SmartLeads/ConvertLeadToCustomer/",
        data: { Id },
        type: "Post",
        dataType: "Json"
    }).done(function () {

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
    $.ajax({
        url: "@MailUrl",
        data: { leadid },
        type: "Post",
        dataType: "Json"
    }).done(function (data) {
        if (data.result == true) {
            alert("Email send successfully.");
        }
        else {
            alert("User email address does not exist");
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

var UpdateCustomerCancellationData = function () {
    if ($("#CacellationReason").val() != null && $("#CacellationReason").val() != "-1") {
        var url = domainurl + "/Customer/UpdateCustomerCancellation";
        var param = JSON.stringify({
            Id: CustomerIdInt,
            CustomerId: CustomerId,
            Reason: $("#CacellationReason").val(),
            OtherReason: $("#OtherReason").val()
        })
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result == true) {
                    $("#CancellationDiv").show();
                    $(".CancellationReasonArea").hide();
                    var ifr = $('iframe')[0];
                    ifr.src = ifr.src
                }
                else {
                    OpenErrorMessageNew('Error!', data.message, "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
    else {
        OpenErrorMessageNew('Error!', "No cacellation reason is selected.", "");
    }
    
}

var counter = 0;
$(document).ready(function () {

    //$(".DisclousureDiv").hide();
    //Currentzone = jstz.determine();
    //timezone = Currentzone.name();
    //$(".sign-head").addClass('hidden');
    //$(".sign-div").addClass('hidden');
    //$(".bodyConentsDiv1").addClass('hidden');
    //$(".bodyConentsDiv1_mobile").addClass('hidden');
    //$(".footerContents-agreement").addClass('hidden');
    if (CancellationReasonFillByCustomer == "True")
    {
        $(".CancellationReasonArea").show();
        $("#CancellationDiv").hide();
    }
    else {
        $(".CancellationReasonArea").hide();
        $("#CancellationDiv").show();
    }
    //console.log(Reason);
    //$("#ReasonArea").addClass('hidden');
    //$("#CacellationReason").change(function () {
    //    if ($("#CacellationReason").val() == "Others")
    //    {
    //        if ($("#ReasonArea").hasClass('hidden')) {
    //            $("#ReasonArea").removeClass('hidden');
    //        }
    //        else {
    //            $("#ReasonArea").addClass('hidden');
    //        }
    //    }
    //    else {
    //        $("#ReasonArea").addClass('hidden');
    //    }
     
    //})
    var calcheight = window.innerHeight - $(".HeadContents").height() - $('.sign-div').height() - $('.footerContents-agreement').height() - $(".navbar").height() - 84;
    $("#AggrementDiv").height(calcheight);
    $(".btnSign").click(function () {
        $(".bodyConentsDiv1").addClass('hidden');
        $(".bodyConentsDiv1_mobile").addClass('hidden');
        $(".sign-document").removeClass('hidden');
        $(".footerContents-agreement").addClass('hidden');
        $(".btnSign").addClass('hidden');
    });
    $("#btnSendMail").click(function () {
        AggrementMail(LeadConvertId);
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
            OpenErrorMessageNew("Error!", "Please Draw Your Signature");
        }
        else {
            sigclicked = true;
            counter += 1;
            var data = signaturePad.toDataURL('image/png');
            var url = domainurl + "/Leads/LoadCancelCustomerSignatureImage";
            $.ajax({
                url: url,
                data: { data, LeadConvertId },
                type: "Post",
                dataType: "Json",
                success: function (data) {
                    if (data.uploadImage == true) {
                        EventDocumentClick();
                        OpenSuccessMessageNew("Success!", "Signature added successfully. Scroll at the bottom to fully review and submit document.", function () {

                            if (IsMobile == "False") {
                                $(".sign-document").addClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile").removeClass('hidden');
                                $(".LoadImgDiv").removeClass('hidden');
                                $(".footerContents-agreement").removeClass('hidden');
                                $(".DisclousureDiv").show();
                                $(".btnSign").removeClass('hidden');
                                var ifr = $('iframe')[0];
                                ifr.src = ifr.src
                            }
                            else {
                                $(".sign-document").addClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile").removeClass('hidden');
                                $(".LoadImgDiv_mobile").removeClass('hidden');
                                $(".footerContents-agreement").removeClass('hidden');
                                $(".DisclousureDiv").show();
                                LoadHtmlForMobileAgreement();
                            }
                        });
                        setTimeout(function () {
                            $(".LoadImgDiv").addClass('hidden');
                            $(".LoadImgDiv_mobile").addClass('hidden');
                        }, 10000);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            })
        }
    })


    $(".btnIAgree").click(function () {
        if (!signaturePad.isEmpty() && counter == 1) {
            clicked = true;
            //$(".Error-warning").hide();
            IAgree();
        }
        else {
            if (signaturePad.isEmpty()) {
                $(".wrapper").css("border-color", "red");
                OpenErrorMessageNew("Error!", "Please press sign document.");
            }
            if (!signaturePad.isEmpty() && counter == 0) {
                OpenErrorMessageNew("Error!", "Please Save your signature first.");
                $(".wrapper").css("border-color", "#ccc");
            }
            //if (!$(".chechbox-IAgree").is(':checked'))
            //    $(".Error-warning").show();
        }
    })
    $("#AggrementDiv").scroll(function () {
        var MathScroll = ($("#AggrementDiv iframe").height() - $("#AggrementDiv").scrollTop());
        console.log(MathScroll);
        //var Globalval = parseInt();
        if (MathScroll <= 550) {
            $(".btnIAgree").removeAttr("disabled");
        }
        else {
            $(".btnIAgree").attr("disabled", true);
            $(".btnIAgree").addClass('DisablebtnAgree');
        }
    })
})