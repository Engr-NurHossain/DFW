var SingatureDate_Date;
var SaveW4Form = function () {
    if ($("#Signature").val() == '') {
        OpenErrorMessageNew("Error!", "Please add your signature to continue.");
        return;
    }

    var url = domainurl + "/Recruit/W4Form";
    var param = JSON.stringify({
        Id: $("#ThisId").val(),
        FormId: $("#FormId").val(),
        Address: $("#Address").val(),
        City: $("#City").val(),
        SSN: $("#SSN").val(),
        State: $("#State").val(),
        Zipcode: $("#Zipcode").val(),
        AdditionalAmount6: $("#AdditionalAmount6").val(),
        AdjustWorkSheet1: $("#AdjustWorkSheet1").val(),
        AdjustWorkSheet10: $("#AdjustWorkSheet10").val(),
        AdjustWorkSheet2: $("#AdjustWorkSheet2").val(),
        AdjustWorkSheet3: $("#AdjustWorkSheet3").val(),
        AdjustWorkSheet4: $("#AdjustWorkSheet4").val(),
        AdjustWorkSheet5: $("#AdjustWorkSheet5").val(),
        AdjustWorkSheet6: $("#AdjustWorkSheet6").val(),
        AdjustWorkSheet7: $("#AdjustWorkSheet7").val(),
        AdjustWorkSheet8: $("#AdjustWorkSheet8").val(),
        AdjustWorkSheet9: $("#AdjustWorkSheet9").val(),
        AllowanceWorksheetA: $("#AllowanceWorksheetA").val(),
        AllowanceWorksheetB: $("#AllowanceWorksheetB").val(),
        AllowanceWorksheetC: $("#AllowanceWorksheetC").val(),
        AllowanceWorksheetD: $("#AllowanceWorksheetD").val(),
        AllowanceWorksheetE: $("#AllowanceWorksheetE").val(),
        AllowanceWorksheetF: $("#AllowanceWorksheetF").val(),
        AllowanceWorksheetG: $("#AllowanceWorksheetG").val(),
        AllowanceWorksheetH: $("#AllowanceWorksheetH").val(),
        EmployerEIN: $("#EmployerEIN").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        JobWroksheet1: $("#JobWroksheet1").val(),
        JobWroksheet2: $("#JobWroksheet2").val(),
        JobWroksheet3: $("#JobWroksheet3").val(),
        JobWroksheet4: $("#JobWroksheet4").val(),
        JobWroksheet5: $("#JobWroksheet5").val(),
        JobWroksheet6: $("#JobWroksheet6").val(),
        JobWroksheet7: $("#JobWroksheet7").val(),
        JobWroksheet8: $("#JobWroksheet8").val(),
        JobWroksheet9: $("#JobWroksheet9").val(),
        EmployernameAndAddress: $("#EmployernameAndAddress").val(),
        NoTaxLiability7: $("#NoTaxLiability7").val(),
        Signature: $("#Signature").val(),
        MiddleInitial: $("#MiddleInitial").val(),
        OfficeCode: $("#OfficeCode").val(),
        SingatureDate: SingatureDate_Date.getDate(),
        TotalAllowance5: $("#TotalAllowance5").val(),
        ReplaceSSNCard4: $("#ReplaceSSNCard4").is(":checked"),
        Single: $("#Single").is(":checked"),
        MarriadButSeparated: $("#MarriadButSeparated").is(":checked"),
        Married: $("#Married").is(":checked"),
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
                OpenSuccessMessageNew("Success!", "W4 Form saved successfully.", function () {
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
    SingatureDate_Date = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#SingatureDate')[0]
    });
    $(".btn-SaveSignature").click(function () {
        $(".RecruitmentW4Form-div").addClass('hidden');
        $(".sign-document").removeClass('hidden');
        $(".btn-SaveSignature").addClass('hidden');
        $(".btn-success").addClass('hidden');
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
            var url = domainurl + "/Recruit/LoadRecruitW4SignatureUpload";
            $.ajax({
                url: url,
                data: { data, formid },
                type: "Post",
                dataType: "Json",
                success: function (data) {
                    if (data.uploadImage == true) {
                        parent.OpenSuccessMessageNew("Success!", "Signature added successfully.", function () {
                            $(".sign-document").addClass('hidden');
                            $(".RecruitmentW4Form-div").removeClass('hidden');
                            //$(".LoadImgDiv").removeClass('hidden');
                            //$(".btn-SaveSignature").addClass('hidden');
                            $(".btn-success").removeClass('hidden');
                            $(".btn-success").removeAttr('disabled')
                            /*$(".RecruitmentW4Form-div").load(location.href + " .RecruitmentW4Form-div");*/
                            $(".W4FromSignature").html("<img src='" + data.UploadFilePath + "'/>");
                            $("#Signature").val(data.UploadFilePath)
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