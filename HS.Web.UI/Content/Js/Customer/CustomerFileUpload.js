var UserFileUploadjqXHRData;
var UserFileUploadjqXHRData_WM;
//var ReloadFIlesTab = function () {
//    $("#FilesTab").load("/File/CustomerFiles/" + customerId);
//}



var SaveCustomerFile = function () {
    var url = domainurl + "/File/SaveCustomerFile/";
    var param = JSON.stringify({
        File: $("#UploadedPath").val(),
        CustomerId: CustomerLoadId,
        Description: $("#description").val(),
        InvoiceId: "",
        _fileSize: $("#FSize").val(),
        _FullPath: $("#FullPath").val(),

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
                //UpdateCustomerTabCounter();
                //CustomerFileLoad();
                OpenFilesTab();
                history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#FilesTab");
                $("#Right-To-Left-Modal-Body .close").click();
            });
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}


    var SaveWatermarkFile = function () {
        var url = domainurl + "/File/MassWatermark_customerfiles/";
        var param = JSON.stringify({
            filename: $("#FileName").val(),
            
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
              if (data.result_WM == true) {
                  OpenSuccessMessageNew("Success!", data.message_WM, function () {
                        CustomerFileLoad();
                     
                        $("#Right-To-Left-Modal-Body .close").click();
                    });
                    
                }
                else
                {
                    OpenErrorMessageNew("Error!", data.message_WM, function () {

                        $("#Right-To-Left-Modal-Body .close").click();  
                    });
                    $(".customer-files-modal-head .close").click();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });

}

    $(document).ready(function () {

        $("#SaveCustomerFiles").click(function () {
            console.log("test done");

          //  if (CommonUiValidation() && $("#UploadedPath").val() != "") {
            if ($("#UploadedPath").val() != "") {
                SaveCustomerFile();
                //OpenFilesTab();
                $(".fileborder").removeClass('red-border');
            }
            if ($("#UploadedPath").val() == "") {
                $("#uploadfileerror").removeClass("hidden");
                $(".fileborder").addClass('red-border');
            }
        });

        $("#SaveWatermarkFiles").click(function () {
            console.log("watermark test done");

            if (CommonUiValidation() && $("#UploadedPath").val() == "") {
                SaveWatermarkFile();
                $(".fileborder").removeClass('red-border');
            }
            if ($("#FullPath").val() == "") {
                $("#uploadfileerror").removeClass("hidden");
                $(".fileborder").addClass('red-border');
            }
        });

        $("#UploadCustomerWMFileBtn").click(function () {
            console.log("WM Clicked");
            $("#UploadedWMFile").click();
            $("#UploadSuccessMessage").addClass('hidden');
        });

        $("#UploadCustomerFileBtn").click(function () {
            console.log("test 1 Clicked");
            $("#UploadedFile").click();
            $("#UploadSuccessMessage").addClass('hidden');
        });

      

        $("#change-picture-logo-Watermark").click(function () {
            console.log("test WM Clicked");
            $("#UploadedWMFile").click();
            $("#UploadSuccessMessage").addClass('hidden');
        });


        $("#change-picture-logo").click(function () {
            console.log("test 1 Clicked");
            $("#UploadedFile").click();
            $("#UploadSuccessMessage").addClass('hidden');
        });


        $('#UploadedFile').fileupload({
            pasteZone: null,
            url: domainurl + '/File/UploadCustomerFile?CustomerId=' + CustomerLoadId, /* CustomerId*/

            dataType: 'json',
            add: function (e, data) {
                var fnamesplit = data.files[0].name.split(".");
                var ext = fnamesplit[fnamesplit.length - 1].toLowerCase();

                if ($("#description").val() == "") {
                    var filename = data.fileInput[0].value.split('\\').pop();
                    $("#description").val(filename);
                }
                //   if (ext[1].toLowerCase() == 'doc' || ext[1].toLowerCase() == 'docx' || ext[1].toLowerCase() == 'xls' || ext[1].toLowerCase() == 'xlsx' || ext[1].toLowerCase() == 'jpeg' || ext[1].toLowerCase() == 'jpg' || ext[1].toLowerCase()

                if (ext.toLowerCase() == 'doc' || ext.toLowerCase() == 'docx' || ext.toLowerCase() == 'xls' || ext.toLowerCase() == 'xlsx' || ext.toLowerCase() == 'jpeg' || ext.toLowerCase() == 'jpg' || ext.toLowerCase() == 'gif' || ext.toLowerCase() == 'png' || ext.toLowerCase() == 'rtf' || ext.toLowerCase() == 'pdf' || ext.toLowerCase() == 'txt' || ext.toLowerCase() == 'mp4' || ext.toLowerCase() == 'mov') {

                    if (data.files[0].size <= 50000000) {
                        UserFileUploadjqXHRData = data;
                        console.log("success");
                    }
                    else {
                        OpenErrorMessageNew("Error!", "File size is more then 50 mb.", function () {
                            $(".close").click();
                        })
                    }

                }
                else {

                    console.log(filename);

                    OpenErrorMessageNew("Error!", "File format not valid 1.", function () {

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
                    //$("#UploadCustomerFileBtn").addClass('hidden');
                    $("#UploadedPath").val(data.result.filePath);
                    $("#FSize").val(data.result.fileSize);
                    $("#FullPath").val(data.result.FullPath);
                    var spfile = data.result.FullFilePath.split('.');
                    //if (spfile[1] == "png" || spfile[1] == "jpg" || spfile[1] == "jpeg") {
                    //    $(".Upload_Doc").addClass('hidden');

                    //    $(".LoadPreviewDocument").removeClass('hidden');
                    //    $("#Preview_Doc").attr('src', data.result.FullFilePath);
                    //}
                    $(".fileborder").removeClass('red-border');
                    $("#uploadfileerror").addClass("hidden");

                    var index = spfile.length - 1;
                    if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                        //$(".Upload_Doc").addClass('hidden');
                        //$(".LoadPreviewDocument").removeClass('hidden');
                        //$("#Preview_Doc").attr('src', data.result.FullFilePath);
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
                    else if (spfile[index] == "Pdf") {
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
                        //$("#UploadCustomerFileBtn").addClass('otherfileposition');
                        //$("#UploadCustomerFileBtn").removeClass('custom-file');
                        $(".fileborder").removeClass('border_none');
                    }
                    else if (spfile[index] == "xls" || spfile[index] == "xlsx") {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/xls.png');
                        //$("#UploadCustomerFileBtn").addClass('otherfileposition');
                        //$("#UploadCustomerFileBtn").removeClass('custom-file');
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
                    //else {
                    //    $(".Upload_Doc").addClass('hidden');
                    //    $(".LoadPreviewDocument").addClass('hidden');
                    //    $(".LoadPreviewDocument1").removeClass('hidden');
                    //    $("#Frame_Doc").attr('src', data.result.FullFilePath);
                    //}
                }
            },
            fail: function (event, data) {
                //if (data.files[0].error) {
                //    //alert(data.files[0].error);
                //}
            }
        });

        $('#UploadedWMFile').fileupload({
            pasteZone: null,
            url: domainurl + '/File/UploadWMCustomerFile_v2/', /* CustomerId*/

            dataType: 'json',
            add: function (e, data) {
                var fnamesplit = data.files[0].name.split(".");
                var ext = fnamesplit[fnamesplit.length - 1].toLowerCase();

                if ($("#description").val() == "") {
                    var filename = data.fileInput[0].value.split('\\').pop();
                    $("#description").val(filename);
                }
                //   if (ext[1].toLowerCase() == 'doc' || ext[1].toLowerCase() == 'docx' || ext[1].toLowerCase() == 'xls' || ext[1].toLowerCase() == 'xlsx' || ext[1].toLowerCase() == 'jpeg' || ext[1].toLowerCase() == 'jpg' || ext[1].toLowerCase()

                if (ext.toLowerCase() == 'xls' || ext.toLowerCase() == 'xlsx')
                {

                    if (data.files[0].size <= 50000000) {
                        UserFileUploadjqXHRData = data;
                        console.log("success");
                    }
                    else {
                        OpenErrorMessageNew("Error!", "File size is more then 50 mb.", function () {
                            $(".close").click();
                        })
                    }

                }
                else {

                    console.log(filename);

                    OpenErrorMessageNew("Error!", "File format not valid : only excel file allowed", function () {
                        $("#FileName").val("");
                        $("#FullPath").val("");
                        $("#description").val("");
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
              
                console.log(UserFileUploadjqXHRData);
                setTimeout(function () {
                    $(".file-progress").hide();
                    $(".file-progress .progress-bar").animate({
                        width: 0 + "%"
                    }, 0);
                    $(".file-progress .progress-bar span").text(0 + '%');
                }, 500);

                if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true)
                {
                    console.log(data.result.isUploaded);
                    $("#UploadSuccessMessage").removeClass('hidden');
                    //$("#UploadCustomerFileBtn").addClass('hidden');
                    $("#FileName").val(data.result.Watermark_filename);
                    $("#FullPath").val(data.result.FullFilePath);
                 
                    var spfile = data.result.FullFilePath.split('.');
                    console.log(spfile);
                    //if (spfile[1] == "png" || spfile[1] == "jpg" || spfile[1] == "jpeg") {
                    //    $(".Upload_Doc").addClass('hidden');

                    //    $(".LoadPreviewDocument").removeClass('hidden');
                    //    $("#Preview_Doc").attr('src', data.result.FullFilePath);
                    //}
                    $(".fileborder").removeClass('red-border');
                    $("#uploadfileerror").addClass("hidden");
                    
                    var index = spfile.length - 1;
                    if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                        //$(".Upload_Doc").addClass('hidden');
                        //$(".LoadPreviewDocument").removeClass('hidden');
                        //$("#Preview_Doc").attr('src', data.result.FullFilePath);
                        $("#UploadCustomerWMFileBtn").attr('src', data.result.FullFilePath)
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").addClass('custom-file');
                        $("#UploadCustomerWMFileBtn").removeClass('otherfileposition');
                        $(".fileborder").addClass('border_none');
                    }
                    else if (spfile[index] == "pdf") {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                        $("#UploadCustomerWMFileBtn").addClass('otherfileposition');
                        $("#UploadCustomerWMFileBtn").removeClass('custom-file');
                        $(".fileborder").removeClass('border_none');
                    }
                    else if (spfile[index] == "Pdf") {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                        $("#UploadCustomerWMFileBtn").addClass('otherfileposition');
                        $("#UploadCustomerWMFileBtn").removeClass('custom-file');
                        $(".fileborder").removeClass('border_none');
                    }
                    else if (spfile[index] == "doc" || spfile[index] == "docx") {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").attr('src', '/Content/Icons/docx.png');
                        $("#UploadCustomerWMFileBtn").addClass('otherfileposition');
                        $("#UploadCustomerWMFileBtn").removeClass('custom-file');
                        $(".fileborder").removeClass('border_none');
                    }
                    else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").attr('src', '/Content/Icons/mp4.png');
                        //$("#UploadCustomerFileBtn").addClass('otherfileposition');
                        //$("#UploadCustomerFileBtn").removeClass('custom-file');
                        $(".fileborder").removeClass('border_none');
                    }
                    else if (spfile[index] == "xls" || spfile[index] == "xlsx") {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").attr('src', '/Content/Icons/xls.png');
                        //$("#UploadCustomerFileBtn").addClass('otherfileposition');
                        //$("#UploadCustomerFileBtn").removeClass('custom-file');



                        $(".fileborder").removeClass('border_none');
                    }
                    else {
                        $(".chooseFilebtn").addClass("hidden");
                        $(".changeFilebtn").removeClass("hidden");
                        $(".deleteDoc").removeClass("hidden");
                        $("#UploadCustomerWMFileBtn").attr('src', '/Content/Icons/docx.png');
                        $("#UploadCustomerWMFileBtn").addClass('otherfileposition');
                        $("#UploadCustomerWMFileBtn").removeClass('custom-file');
                        $(".fileborder").removeClass('border_none');
                    }



                    //else {
                    //    $(".Upload_Doc").addClass('hidden');
                    //    $(".LoadPreviewDocument").addClass('hidden');
                    //    $(".LoadPreviewDocument1").removeClass('hidden');
                    //    $("#Frame_Doc").attr('src', data.result.FullFilePath);
                    //}
                }
            },
            fail: function (event, data) {
                //if (data.files[0].error) {
                //    //alert(data.files[0].error);
                //}
            }
        });


        $("#UploadedFile").on("change", function () {
            if (UserFileUploadjqXHRData) {
                UserFileUploadjqXHRData.submit();
            }
            return false;
        });

        $(".UploadedWMFile").on("change", function () {
            if (UserFileUploadjqXHRData) {
                UserFileUploadjqXHRData.submit();
            }
            return false;
        });

    
    });
