var printFrame = function (id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var IAgree = function () {
    IAgreeSetup(LeadConvertId);
}
var LoadHtmlForMobileAgreement = function () {
    var Url = domainurl + "/SmartLeads/SmartInstallationAgreement/?url=" + docurl;
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
var LoadMobileAgreementViewMethod = function () {
    //$(".btnSign").removeAttr("disabled");
    window.open(domainurl + "/SmartLeads/SmartInstallationAgreement/?url=" + docurl, "_blank");
}
var DownloadMobileAgreement = function () {
    window.open(domainurl + "/SmartLeads/DownloadSmartAgreement/?url=" + docurl, "_blank");
}
var IAgreeSetup = function (idval) {
    var Id = idval;
    $(".LoadImgDiv").removeClass('hidden');
    $(".LoadImgDiv_mobile").removeClass('hidden');
    $.ajax({
        url: AgreeUrlSetup,
        data: { Id, recreate, agreementtempid, firstpage, ticketid, isinvoice, invoiceid, isestimator, estid, userid, commercial },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        $(".LoadImgDiv").addClass('hidden');
        $(".LoadImgDiv_mobile").addClass('hidden');
        if (Device.All()) {
            var ifr = $('iframe')[0];
            if (typeof (ifr) != "undefined" && ifr != null && ifr != "") {
                ifr.src = ifr.src;
            }
            OpenSuccessMessageNew("Success!", "Your agreement has been signed and sent to our office. You will also receive an email confirmation with your signed agreement!", function () {
                $(".footerContents-agreement").addClass('hidden');
                $(".sign-div").addClass('hidden');
            })
        }
        else {
            console.log("LoadImgDiv");

            var ifr = $('iframe')[0];
            ifr.src = ifr.src
            OpenSuccessMessageNew("Success!", "Your agreement has been signed and sent to our office. You will also receive an email confirmation with your signed agreement!", function () {
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

$(document).ready(function () {
    if (Device.All()) {
        $('.mobile-content').remove();
    }
    else {
        $('.mobile-content').remove();
    }
    $(".DisclousureDiv").hide();
    Currentzone = jstz.determine();
    timezone = Currentzone.name();
    if (IsAgreementQuestion == "true") {
        if (NoQues == "False") {
            $(".sign-head").addClass('hidden');
            $(".sign-div").addClass('hidden');
            $(".bodyConentsDiv1").addClass('hidden');
            $(".bodyConentsDiv1_mobile_doc").addClass('hidden');
            $(".footerContents-agreement").addClass('hidden');
        }
        else {
            $(".sign-head").addClass('hidden');
            $(".bodyConentsDiv1_mobile_doc").addClass('hidden');
        }
    }
    else {
        LoadHtmlForMobileAgreement();
    }
    console.log('bipul fol');
    $(".btnYesNo").click(function () {
        var ansval = [];
        var ans = $(this).attr('idval');
        var ques = $(this).attr('idval-qustionId');
        ansval.push({
            SelectedAns: ans.toString(),
            SelectedQues: parseInt(ques)
        });
        console.log(ansval);
        $.ajax({
            url: domainurl + "/Public/AgreementQuesAns",
            data: {
                LeadConvertId,
                ListAgreementAnswer: ansval,
            },
            type: "Post",
            dataType: "Json"
        }).done(function (data) {
            if (data.result1 == false) {
                if (data.Ques1 == true) {
                    if (data.Ques3 == true) {
                        $(".ques2").removeClass('hidden');
                        if (data.Ans1 == true) {
                            $(".YAns1").removeClass('hidden');
                            $(".Ques1").addClass('hidden');
                        }
                        else {
                            $(".NAns1").removeClass('hidden');
                            $(".Ques1").addClass('hidden');
                        }
                    }
                    if (data.Ques4 == true) {
                        $(".ques3").removeClass('hidden');
                        if (data.Ans3 == true) {
                            $(".YAns3").removeClass('hidden');
                            $(".ques2").addClass('hidden');
                        }
                        else {
                            $(".NAns3").removeClass('hidden');
                            $(".ques2").addClass('hidden');
                        }
                    }
                    if (data.Ques5 == true) {
                        if (data.Additional5 == true) {
                            $(".ques4").removeClass('hidden');
                            if (data.Ans3 == true) {
                                $(".YAns3").removeClass('hidden');
                                $(".ques2").addClass('hidden');
                            }
                            else {
                                $(".NAns3").removeClass('hidden');
                                $(".ques2").addClass('hidden');
                            }
                        }
                        else {
                            $(".ques4").removeClass('hidden');
                            if (data.Ans4 == true) {
                                $(".YAns4").removeClass('hidden');
                                $(".ques3").addClass('hidden');
                            }
                            else {
                                $(".NAns4").removeClass('hidden');
                                $(".ques3").addClass('hidden');
                            }
                        }
                    }
                }
                else {
                    OpenErrorMessageNew("Error!", data.message, "");
                }
            }
            else {
                if (data.message5 == false) {
                    if (!Device.All()) {
                        EventDocumentClick();
                        $(".LoadImgDiv").removeClass('hidden');
                        setTimeout(function () {
                            $(".LoadImgDiv").addClass('hidden');
                        }, 5000);
                        $(".sign-head").removeClass('hidden');
                        $(".ques-head").addClass('hidden');
                        $(".answer-content").addClass('hidden');
                        $(".sign-div").removeClass('hidden');
                        $(".bodyConentsDiv1").removeClass('hidden');
                        $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                        var ifr = $('iframe')[0];
                        ifr.src = ifr.src
                    }
                    else {
                        EventDocumentClick();
                        $(".LoadImgDiv_mobile").removeClass('hidden');
                        setTimeout(function () {
                            $(".LoadImgDiv_mobile").addClass('hidden');
                        }, 5000);
                        $(".sign-head").removeClass('hidden');
                        $(".ques-head").addClass('hidden');
                        $(".answer-content").addClass('hidden');
                        $(".sign-div").removeClass('hidden');
                        $(".bodyConentsDiv1").removeClass('hidden');
                        $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                        LoadHtmlForMobileAgreement();
                    }
                    $(".footerContents-agreement").removeClass('hidden');
                }
                else {
                    if (customertype.toLowerCase() == "commercial") {
                        if (typeof (data.profilemessage) != "undefined" && data.profilemessage != null && data.profilemessage != "" && data.profilemessage != "0") {
                            $(".profile_message").removeClass('hidden');
                            $(".profile_message span").text(data.profilemessage);
                            $(".ques3").addClass('hidden');
                            $(".profile_message a").attr('data-id', data.customerid);
                            $(".profile_message a").attr('data-comid', data.companyid);
                            $(".profile_message a").attr('data-type', data.type);
                            if (data.Ans4 == true) {
                                $(".YAns4").removeClass('hidden');
                                $(".NAns4").addClass('hidden');
                            }
                            else {
                                $(".YAns4").addClass('hidden');
                                $(".NAns4").removeClass('hidden');
                            }
                        }
                        else if (typeof (data.profilemessage) != "undefined" && data.profilemessage != null && data.profilemessage != "" && data.profilemessage == "0") {
                            if (!Device.All()) {
                                EventDocumentClick();
                                $(".LoadImgDiv").removeClass('hidden');
                                setTimeout(function () {
                                    $(".LoadImgDiv").addClass('hidden');
                                }, 5000);
                                $(".sign-head").removeClass('hidden');
                                $(".ques-head").addClass('hidden');
                                $(".answer-content").addClass('hidden');
                                $(".sign-div").removeClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                var ifr = $('iframe')[0];
                                ifr.src = ifr.src
                            }
                            else {
                                EventDocumentClick();
                                $(".LoadImgDiv_mobile").removeClass('hidden');
                                setTimeout(function () {
                                    $(".LoadImgDiv_mobile").addClass('hidden');
                                }, 5000);
                                $(".sign-head").removeClass('hidden');
                                $(".ques-head").addClass('hidden');
                                $(".answer-content").addClass('hidden');
                                $(".sign-div").removeClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                LoadHtmlForMobileAgreement();
                            }
                            $(".footerContents-agreement").removeClass('hidden');
                        }
                        else {
                            $(".profile_message").addClass('hidden');
                            OpenErrorMessageNew("Error!", data.message, "");
                        }
                    }
                    else {
                        if (typeof (data.profilemessage) != "undefined" && data.profilemessage != null && data.profilemessage != "" && data.profilemessage != "0") {
                            $(".profile_message").removeClass('hidden');
                            $(".profile_message span").text(data.profilemessage);
                            $(".ques4").addClass('hidden');
                            $(".profile_message a").attr('data-id', data.customerid);
                            $(".profile_message a").attr('data-comid', data.companyid);
                            $(".profile_message a").attr('data-type', data.type);
                            if (data.Ans5 == true) {
                                $(".YAns5").removeClass('hidden');
                                $(".NAns5").addClass('hidden');
                            }
                            else {
                                $(".YAns5").addClass('hidden');
                                $(".NAns5").removeClass('hidden');
                            }
                        }
                        else if (typeof (data.profilemessage) != "undefined" && data.profilemessage != null && data.profilemessage != "" && data.profilemessage == "0") {
                            if (!Device.All()) {
                                EventDocumentClick();
                                $(".LoadImgDiv").removeClass('hidden');
                                setTimeout(function () {
                                    $(".LoadImgDiv").addClass('hidden');
                                }, 5000);
                                $(".sign-head").removeClass('hidden');
                                $(".ques-head").addClass('hidden');
                                $(".answer-content").addClass('hidden');
                                $(".sign-div").removeClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                var ifr = $('iframe')[0];
                                ifr.src = ifr.src
                            }
                            else {
                                EventDocumentClick();
                                $(".LoadImgDiv_mobile").removeClass('hidden');
                                setTimeout(function () {
                                    $(".LoadImgDiv_mobile").addClass('hidden');
                                }, 5000);
                                $(".sign-head").removeClass('hidden');
                                $(".ques-head").addClass('hidden');
                                $(".answer-content").addClass('hidden');
                                $(".sign-div").removeClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                LoadHtmlForMobileAgreement();
                            }
                            $(".footerContents-agreement").removeClass('hidden');
                        }
                        else {
                            $(".profile_message").addClass('hidden');
                            OpenErrorMessageNew("Error!", data.message, "");
                        }
                    }

                }
            }
        });
    })
    var calcheight = window.innerHeight - $(".HeadContents").height() - $('.sign-div').height() - $('.footerContents-agreement').height() - $(".navbar").height() - 84;
    $("#AggrementDiv").height(calcheight);
    $(".btnSign").click(function () {
        $(".bodyConentsDiv1").addClass('hidden');
        $(".bodyConentsDiv1_mobile_doc").addClass('hidden');
        $(".sign-document").removeClass('hidden');
        $(".footerContents-agreement").addClass('hidden');
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
            OpenErrorMessageNew("Error!", "Please Draw Your Signature ");
        }
        else {
            if (ContractSubmit == "True") {
                $(".LoadImgDiv").removeClass('hidden');
                $(".LoadImgDiv_mobile").removeClass('hidden');
            }
            sigclicked = true;
            counter += 1;
            var data = signaturePad.toDataURL('image/png');
            var url = domainurl + "/SmartLeads/LoadCustomerSignatureImage";
            $.ajax({
                url: url,
                data: { data, LeadConvertId, templateid, firstpage, ticketid, recreate, isinvoice, invoiceid, CreateDocument, isestimator, estid, userid, commercial, ContractSubmit,EstimatorId },
                type: "Post",
                dataType: "Json",
                success: function (data) {  
                    if (ContractSubmit == "True") {
                        $(".btnSign").html("<i class='fa fa-sign-in' aria-hidden='true'></i> Re-sign Document");
                        $(".loadcontractsubmitandsend").load("/SmartLeads/SmartIAgreeSetupAndSubmit?Id=" + LeadConvertId + "&recreate=" + recreate + "&agreementtempid=" + templateid + "&firstpage=" + firstpage + "&ticketid=" + ticketid + "&isinvoice=" + isinvoice + "&invoiceid=" + invoiceid + "&isestimator=" + isestimator + "&estid=" + estid + "&userid=" + userid + "&commercial=" + commercial + "&EstimatorId=" + EstimatorId);
                        if (!Device.All()) {
                            $(".LoadImgDiv").addClass('hidden');
                            $(".LoadImgDiv_mobile").addClass('hidden');


                            OpenSuccessMessageNew("Success!", "Your agreement has been signed and sent to our office. You will also receive an email confirmation with your signed agreement!", function () {
                                $(".sign-document").addClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                $(".footerContents-agreement").addClass('hidden');
                                $(".sign-div").addClass('hidden');
                                var ifr = $('iframe')[0];
                                ifr.src = ifr.src
                                //$(".LoadImgDiv").removeClass('hidden');
                            }); 
                            setTimeout(function () {
                                $(".LoadImgDiv").addClass('hidden');
                                var ifr = $('iframe')[0];
                                ifr.src = ifr.src
                            }, 10000);
                        }
                        else {
                            OpenSuccessMessageNew("Success!", "Your agreement has been signed and sent to our office. You will also receive an email confirmation with your signed agreement!", function () {
                                $(".sign-document").addClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                $(".LoadImgDiv_mobile").removeClass('hidden');
                                $(".footerContents-agreement").removeClass('hidden');
                                $(".DisclousureDiv").show();
                                LoadHtmlForMobileAgreement();
                            });
                            
                            setTimeout(function () {
                                $(".LoadImgDiv").addClass('hidden');
                                $(".LoadImgDiv_mobile").addClass('hidden');
                            }, 10000);
                        }
                    }
                    else if (data.uploadImage == true) {
                        issignuploaded = true;
                        EventDocumentClick();
                        OpenSuccessMessageNew("Success!", "Signature added successfully. Scroll at the bottom to fully review and submit document.", function () {
                            $(".btnSign").html("<i class='fa fa-sign-in' aria-hidden='true'></i> Re-sign Document");
                            if (!Device.All()) {
                                $(".sign-document").addClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
                                $(".LoadImgDiv").removeClass('hidden');
                                $(".footerContents-agreement").removeClass('hidden');
                                $(".DisclousureDiv").show();
                                var ifr = $('iframe')[0];
                                ifr.src = ifr.src
                            }
                            else {
                                $(".sign-document").addClass('hidden');
                                $(".bodyConentsDiv1").removeClass('hidden');
                                $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
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
    //$(".Error-warning").hide();
    if (!Device.All()) {
        $(".btnIAgree").attr("disabled", true);
    }
    $(".btnIAgree").click(function () {
        if (!signaturePad.isEmpty() && counter >= 1) {
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

        if (!Device.All()) {
            var MathScroll = ($("#AggrementDiv iframe").height() - $("#AggrementDiv").scrollTop());
            console.log(MathScroll);
            //var Globalval = parseInt();
            if (MathScroll <= 550) {
                $(".btnIAgree").removeAttr("disabled");
            }
            else {
                if (issignuploaded == false) {
                    $(".btnIAgree").attr("disabled", true);
                    $(".btnIAgree").addClass('DisablebtnAgree');
                }
                else {
                    $(".btnIAgree").removeAttr("disabled");
                }
            }
        }
    })
    $("#SwitchACHPayment").click(function () {
        var idval = $(this).attr('data-id');
        var comid = $(this).attr('data-comid');
        var type = $(this).attr('data-type');
        OpenRightToLeftModal("/Public/PublicACHAddViewPaymentMethod?customerid=" + idval + "&companyid=" + comid + "&type=" + type);
    })
    $("#btn_profile_yes").click(function () {
        $("#SwitchACHPayment").removeClass('hidden');
    })
    $("#btn_profile_no").click(function () {

        if (!Device.All()) {
            $("#SwitchACHPayment").addClass('hidden');
            $(".LoadImgDiv").removeClass('hidden');
            setTimeout(function () {
                $(".LoadImgDiv").addClass('hidden');
            }, 5000);
            $(".sign-head").removeClass('hidden');
            $(".ques-head").addClass('hidden');
            $(".answer-content").addClass('hidden');
            $(".sign-div").removeClass('hidden');
            $(".bodyConentsDiv1").removeClass('hidden');
            $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
            var ifr = $('iframe')[0];
            ifr.src = ifr.src
        }
        else {
            $("#SwitchACHPayment").addClass('hidden');
            $(".LoadImgDiv_mobile").removeClass('hidden');
            setTimeout(function () {
                $(".LoadImgDiv_mobile").addClass('hidden');
            }, 5000);
            $(".sign-head").removeClass('hidden');
            $(".ques-head").addClass('hidden');
            $(".answer-content").addClass('hidden');
            $(".sign-div").removeClass('hidden');
            $(".bodyConentsDiv1_mobile_doc").removeClass('hidden');
            LoadHtmlForMobileAgreement();
        }
        $(".footerContents-agreement").removeClass('hidden');
    })
})