var DOB_Date;
var SignatureDate_Date;
var TransSignaturedate_Date;
var Exp1ListA_Date;
var Exp1ListB_Date;
var Exp1ListC_Date;
var Exp2ListA_Date;
var Exp2ListB_Date;
var Exp2ListC_Date;
var AuthRepSignatureDate_Date;
var DateOfRehire_Date;
var PrevDocExp_Date;
var AuthRepSignatureDate2_Date;
var BeganEmploymentOn_Date;

var SaveI9Form = function () {
    if ($("#Signature").val() == '') {
        OpenErrorMessageNew("Error!", "Please add your signature to continue.");
        return;
    }

    var url = domainurl + "/Recruit/I9Form";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        FormId: $("#FormId").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        MiddleInitial: $("#MiddleInitial").val(),
        MaidenName: $("#MaidenName").val(),
        DOB: DOB_Date.getDate(),
        SSN: $("#SSN").val(),
        Address: $("#Address").val(),
        Apartment: $("#Apartment").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        USCitizen: $("#USCitizen").is(":checked"),
        NoncitizenNational: $("#NoncitizenNational").is(":checked"),
        LawfulPermanentResident: $("#LawfulPermanentResident").is(":checked"),
        AlienAuthorizedToWork: $("#AlienAuthorizedToWork").is(":checked"),
        UntilExp: $("#UntilExp").is(":checked"),
        Signature: $("#Signature").val(),
        SignatureDate: SignatureDate_Date.getDate(),
        TransSignature: $("#TransSignature").val(),
        TransPrintName: $("#TransPrintName").val(),
        TransAddress: $("#TransAddress").val(),
        TransSignaturedate: TransSignaturedate_Date.getDate(),
        DocTitleListA: $("#DocTitleListA").val(),
        DoctTitleListB: $("#DoctTitleListB").val(),
        DoctTitleListC: $("#DoctTitleListC").val(),
        IssuingAuthorityListA: $("#IssuingAuthorityListA").val(),
        IssuingAuthorityListB: $("#IssuingAuthorityListB").val(),
        IssuingAuthorityListC: $("#IssuingAuthorityListC").val(),
        Doc1ListA: $("#Doc1ListA").val(),
        Doc1ListB: $("#Doc1ListB").val(),
        Doc1ListC: $("#Doc1ListC").val(),
        Exp1ListA: Exp1ListA_Date.getDate(),
        Exp1ListB: Exp1ListB_Date.getDate(),
        Exp1ListC: Exp1ListC_Date.getDate(),
        Doc2ListA: $("#Doc2ListA").val(),
        Doc2ListB: $("#Doc2ListB").val(),
        Doc2ListC: $("#Doc2ListC").val(),
        Exp2ListA: Exp2ListA_Date.getDate(),
        Exp2ListB: Exp2ListB_Date.getDate(),
        Exp2ListC: Exp2ListC_Date.getDate(),
        BeganEmploymentOn: BeganEmploymentOn_Date.getDate(),
        AuthRepresentativeSignature: $("#AuthRepresentativeSignature").val(),
        AuthRepresentativeName: $("#AuthRepresentativeName").val(),
        AuthRepresentativeTitle: $("#AuthRepresentativeTitle").val(),
        AuthRepSignatureDate: AuthRepSignatureDate_Date.getDate(),
        OrgNameAndAddress: $("#OrgNameAndAddress").val(),
        NewName: $("#NewName").val(),
        DateOfRehire: DateOfRehire_Date.getDate(),
        PrevDocTitle: $("#PrevDocTitle").val(),
        PrevDocNo: $("#PrevDocNo").val(),
        PrevDocExp: PrevDocExp_Date.getDate(),
        AuthRepSignature2: $("#AuthRepSignature2").val(),
        AuthRepSignatureDate2: AuthRepSignatureDate2_Date.getDate(),
    });
    console.log(param);
    $.ajax({
        url: url,
        data: param,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "W9 Form saved successfully.", function () {
                    LoadRecruit(true);
                });
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    
    DOB_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        yearRange: [1920, 2010],
        field: $('#DOB')[0]
    });
    SignatureDate_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#SignatureDate')[0]
    });
    TransSignaturedate_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#TransSignaturedate')[0]
    });
    Exp1ListA_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Exp1ListA')[0]
    });
    Exp1ListB_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Exp1ListB')[0]
    });
    Exp1ListC_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Exp1ListC')[0]
    });
    Exp2ListA_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Exp2ListA')[0]
    });
    Exp2ListB_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Exp2ListB')[0]
    });
    Exp2ListC_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#Exp2ListC')[0]
    });
    AuthRepSignatureDate_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#AuthRepSignatureDate')[0]
    });
    DateOfRehire_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#DateOfRehire')[0]
    });
    PrevDocExp_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#PrevDocExp')[0]
    });
    AuthRepSignatureDate2_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#AuthRepSignatureDate2')[0]
    });
    BeganEmploymentOn_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#BeganEmploymentOn')[0]
    });
     
    
    $(".btn-SaveSignature").click(function () {
        $(".RecruitmentFormI9-div").addClass('hidden');
        $(".btn-SaveSignature").addClass('hidden');
        $(".btn-success").addClass('hidden');

        $(".sign-document").removeClass('hidden');
    })
    $('#clear').click(function () {
        signaturePad.clear();
    });
    $("#save-png").click(function () {
        if (signaturePad.isEmpty()) {
            parent.OpenErrorMessageNew("Error!", "Please Draw Your Signature", "");
        }
        else {
            var data = signaturePad.toDataURL('image/png');
            var formid = $("#save-png").attr('data-id');
            var url = domainurl + "/Recruit/LoadRecruitI9SignatureUpload";
            $.ajax({
                url: url,
                data: { data, formid },
                type: "Post",
                dataType: "Json",
                success: function (data) {
                    if (data.uploadImage == true) {
                        parent.OpenSuccessMessageNew("Success!", "Signature added successfully.", function () {
                            $(".sign-document").addClass('hidden');
                            $(".RecruitmentFormI9-div").removeClass('hidden');
                            //$(".LoadImgDiv").removeClass('hidden');
                            //$(".btn-SaveSignature").addClass('hidden');
                            $(".btn-success").removeClass('hidden');
                            $(".btn-success").removeAttr('disabled');
                            $("#Signature").val(data.UploadFilePath);
                            $(".SignatureDiv").html("<img src='" + data.UploadFilePath + "'/>");
                            /*$(".RecruitmentFormI9-div").load(location.href + " .RecruitmentFormI9-div");*/
                        });
                        //setTimeout(function () {
                        //    $(".LoadImgDiv").addClass('hidden');
                        //}, 10000);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            })
        }
    })
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
})