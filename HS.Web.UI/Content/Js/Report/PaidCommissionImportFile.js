var UserFileUploadjqXHRData;
var SaveCustomerFile = function (isCustomer) {
    var url = domainurl + "/ADS/SavePaidCommissionFile/";

    
    var param = JSON.stringify({
        File: $("#UploadedPath").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
                $("#Right-To-Left-Modal-Body .close").click();
                
                if (data.success == "true") {

                    window.location.href = "/reports";
                }
                else {
                    window.location.reload();
                }

            });
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}

var SaveBrinksReportFile = function (ImportFor) {
    var url = "";
    if (ImportFor == 'CancelQueue') {
        url = domainurl + "/ADS/SaveCancelQueueFile/";
    }
    else if (ImportFor == 'CustomerList') {
        url = domainurl + "/ADS/SaveCustomerListFile/";
    }
    else if (ImportFor == 'FundingVerification') {
        url = domainurl + "/ADS/SaveFundingVerificationFile/";
    }


    var param = JSON.stringify({
        File: $("#UploadedPath").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
                $("#Right-To-Left-Modal-Body .close").click();
                if (ImportFor == "CustomerList") {
                    CustomerListReportLoad(1,null);
                }
                else if (ImportFor == "FundingVerification")
                {
                    FundingVerificationReportLoad(1, null);
                }
                else {
                    if (data.success == "true") {

                        window.location.href = "/reports";
                    }
                    else {
                        window.location.reload();
                    }
                }
              

            });
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}

var SaveImportFile = function (isCustomer) {
    if (CommonUiValidation() && $("#UploadedPath").val() != "") {
        SaveCustomerFile(isCustomer);
        $(".fileborder").removeClass('red-border');
    }
    if ($("#UploadedPath").val() == "") {
        $("#uploadfileerror").removeClass("hidden");
        $(".fileborder").addClass('red-border');
    }
}
var SaveImportBrinksReport = function (ImportFrom) {
    if (CommonUiValidation() && $("#UploadedPath").val() != "") {

        SaveBrinksReportFile(ImportFrom);
        $(".fileborder").removeClass('red-border');
    }
    if ($("#UploadedPath").val() == "") {
        $("#uploadfileerror").removeClass("hidden");
        $(".fileborder").addClass('red-border');
    }
}
$(document).ready(function () {


    $("#UploadCustomerFileBtn").click(function () {
        console.log("sdfdsf");
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/File/UploadPaidCommissionImportFile',
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            if (ext[1] == 'xls' || ext[1] == 'xlsx') {

                if (data.files[0].size <= 500000000) {
                    UserFileUploadjqXHRData = data;
                }
                else {
                    OpenErrorMessageNew("Error!", "File size is more then 50 mb.", function () {
                        $(".close").click();
                    })
                }

            }
            else {
                OpenErrorMessageNew("Error!", "File format not valid.", function () {
                    $(".close").click();
                })
            }

        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progress").show();
            $(".file-progress .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progress .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            console.log("dfdf");
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);

            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                $("#UploadedPath").val(data.result.filePath);
                var spfile = data.result.FullFilePath.split('.');
                $(".fileborder").removeClass('red-border');
                $("#uploadfileerror").addClass("hidden");

                var index = spfile.length - 1;
                if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                    $("#UploadCustomerFileBtn").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").addClass('custom-file');
                    $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                    $(".fileborder").addClass('border_none');
                }
                else if (spfile[index] == "pdf") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "doc" || spfile[index] == "docx") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/mp4.png');
                    $(".fileborder").removeClass('border_none');
                }
                else {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
            }
        },
        fail: function (event, data) {

        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });
});