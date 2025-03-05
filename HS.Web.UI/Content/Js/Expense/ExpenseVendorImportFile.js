var UserFileUploadjqXHRData;
var SaveExpenseVendorFile = function () {
    var url = domainurl + "/Supplier/SaveExpenseVendorImportFile/";

    var param = JSON.stringify({
        File: $("#UploadedPath").val(),
        Platform:"Point Tier32"
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
                //if (data.isCustomer == "true")
                //{
           
                //    window.location.href = "/customer";
                //}
                //else {
                    window.location.reload();
                //}
               
            });
            //$(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}
//var SaveContactFile = function () {
//    var url = domainurl + "/Contact/SaveContactImportFile/";
//    var param = JSON.stringify({
//        File: $("#UploadedPath").val(),
  
//    });
//    $.ajax({
//        type: "POST",
//        ajaxStart: $(".loader-div").show(),
//        url: url,
//        data: param,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        cache: false,
//        success: function (data) {
//            OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
//                $("#Right-To-Left-Modal-Body .close").click();
               
//                    window.location.reload();
                

//            });
//            $(".customer-files-modal-head .close").click();
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            console.log(errorThrown);
//        }
//    });

//}
//var SaveOpportunityFile = function () {
//    var url = domainurl + "/Opportunity/SaveOpportunityImportFile/";
//    var param = JSON.stringify({
//        File: $("#UploadedPath").val(),

//    });
//    $.ajax({
//        type: "POST",
//        ajaxStart: $(".loader-div").show(),
//        url: url,
//        data: param,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        cache: false,
//        success: function (data) {
//            OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
//                $("#Right-To-Left-Modal-Body .close").click();

//                window.location.reload();


//            });
//            $(".customer-files-modal-head .close").click();
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            console.log(errorThrown);
//        }
//    });

//}
var SaveImportFile = function () {
    if (CommonUiValidation() && $("#UploadedPath").val() != "") {
        SaveExpenseVendorFile();
        $(".fileborder").removeClass('red-border');
    }
    if ($("#UploadedPath").val() == "") {
        $("#uploadfileerror").removeClass("hidden");
        $(".fileborder").addClass('red-border');
    }
}
//var SaveContactImportFile = function () {
//    if (CommonUiValidation() && $("#UploadedPath").val() != "") {
//        SaveContactFile();
//        $(".fileborder").removeClass('red-border');
//    }
//    if ($("#UploadedPath").val() == "") {
//        $("#uploadfileerror").removeClass("hidden");
//        $(".fileborder").addClass('red-border');
//    }
//}
//var SaveOpportunityImportFile = function () {
//    if (CommonUiValidation() && $("#UploadedPath").val() != "") {
//        SaveOpportunityFile();
//        $(".fileborder").removeClass('red-border');
//    }
//    if ($("#UploadedPath").val() == "") {
//        $("#uploadfileerror").removeClass("hidden");
//        $(".fileborder").addClass('red-border');
//    }
//}
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
        url: domainurl + '/File/UploadExpenseVendorImportFile',
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
                    $("#UploadCustomerFileBtn").attr('src', domainurl+ '/Content/Icons/pdf.png');
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