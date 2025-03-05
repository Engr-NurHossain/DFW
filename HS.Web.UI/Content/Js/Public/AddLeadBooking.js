var status;
var clicked = false;
var sigclicked = false;
var signaturePad;
var printFrame = function (id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
var EventDocumentClick = function () {
    var inFormOrLink;
    $('a').on('click', function () { inFormOrLink = true; });
    $('form').on('submit', function () { inFormOrLink = true; });

    $(window).on("beforeunload", function () {
        return inFormOrLink ? "Do you really want to close?" : null;
    });
};

var IAgreeSetup = function (status, DeclinedReason) {
    $(".LoadImgDiv").removeClass('hidden');
    $.ajax({
        url: "/Public/LeadsBookingAgree/",
        data: {
            code: Token,
            status: status,
            DeclinedReason: DeclinedReason
        },
        type: "Post",
        dataType: "Json",
        success: function () {
            $(".LoadImgDiv").addClass('hidden');
        }
    }).done(function (data) {
        $(".LoadImgDiv").addClass('hidden');
        if (data.result == true) {
            if (typeof (data.IsReload) != undefined && data.IsReload) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    location.href = location.href;
                });
            } else {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    $(".footerContents-agreement").remove()
                    $(".sign-div").remove();
                    $(".sign_doc_div").remove();
                });
            }
        }
        else {
            $(".LoadImgDiv").addClass('hidden');
            $(".LoadDecline").removeClass('hidden');
            $(".BookingDocument").addClass('hidden')
        }
    });
}
var SignatureInit = function () {
    var canvas = $('#signature-pad');
    var ctx = $('#signature-pad')[0].getContext('2d');
    signaturePad = new SignaturePad($('#signature-pad')[0]);
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
            OpenErrorMessageNew("Error!", "Please Draw Your Signature First.");
        }
        else {
            sigclicked = true;
            counter += 1;
            var data = signaturePad.toDataURL('image/png');
            var url = "/Public/UploadCustomerSignatureImageBooking";
            $.ajax({
                url: url,
                data: {
                    data, Token
                },
                type: "Post",
                dataType: "Json",
                success: function (data) {
                    if (data.uploadImage == true) {
                        OpenSuccessMessageNew("Success!", "Signature added successfully. Scroll to the bottom to fully review your signed booking.");
                        
                        $(".sign-document").addClass('hidden');
                        $(".bodyConentsDiv1").removeClass('hidden');
                        $(".LoadImgDiv").removeClass('hidden');
                        $(".footerContents-agreement").removeClass('hidden');
                        var ifr = $('iframe')[0];
                        ifr.src = ifr.src; 

                        //setTimeout(function () {
                        //    $(".LoadImgDiv").addClass('hidden');
                        //    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
                        //}, 3000);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            })
        }
    })
}
var counter = 0;

$(document).ready(function () {
    //var calcheight = window.innerHeight - $(".HeadContents").height() - $('.sign-div').height() - $('.footerContents-agreement').height() - $(".navbar").height() - 84;

    $(".HeadContents").addClass('hidden');
    if (Status != 'Signed') {
        $(".footerContents-agreement").addClass('hidden');
    } 
    //$("#AggrementDiv").height(calcheight);

    $(".btnSign").click(function () {
        $(".bodyConentsDiv1").addClass('hidden');
        $(".sign-document").removeClass('hidden');
        $(".footerContents-agreement").addClass('hidden');
    });

    $(".btnIAgree").click(function () {
        if ((!signaturePad.isEmpty() && counter == 1) || Status == 'Signed') {
            clicked = true;
            IAgreeSetup(true, "");
        }
        else {
            if (signaturePad.isEmpty()) {
                $(".wrapper").css("border-color", "red");
                OpenErrorMessageNew("Error!", "Please press sign document");
            }
            if (!signaturePad.isEmpty() && counter == 0) {
                OpenErrorMessageNew("Error!", "Please save your signature first");
                $(".wrapper").css("border-color", "#ccc");
            }
        }
    });
    $(".btnDec").click(function () {
        $(".sign-div").addClass('hidden');
        $(".declinationreason").removeClass('hidden');
        //IAgreeSetup(false);
    });
    $(".DeclineCancel").click(function () {
        $(".sign-div").removeClass('hidden');
        $(".declinationreason").addClass('hidden');
    });
    $(".DeclineConfirm").click(function () {
        if ($(".declinationreasontxt").val().trim() == "") {
            $("#DeclinedErr").removeClass('hidden');
        } else {
            $("#DeclinedErr").addClass('hidden');
            IAgreeSetup(false, $(".declinationreasontxt").val());
        }
    });
    $(".declinationreasontxt").blur(function () {
        if ($(".declinationreasontxt").val().trim() == "") {
            $("#DeclinedErr").removeClass('hidden');
        } else {
            $("#DeclinedErr").addClass('hidden');
        }
    });
    $("#Cancel").click(function () {
        $(".sign-document").addClass('hidden');
        $(".bodyConentsDiv1").removeClass('hidden');
    });
    SignatureInit();
})