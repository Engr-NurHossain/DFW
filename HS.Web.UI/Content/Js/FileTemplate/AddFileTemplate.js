var UserFileUploadjqXHRData;

var SaveChanges = function (IsDefault) {
    var Param = {
        Id: $("#TemplateId").val(),
        RestoreDefault: IsDefault,
        Name: $("#TemplateName").val(),
        Description: $("#Description").val(),
        BodyContent: tinyMCE.get('BodyContent').getContent(),
        IsCustomerSignRequired: $("#IsCustomerSignRequired").prop('checked')
    };
    var url = domainurl + "/File/AddFileTemplate/";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    CloseTopToBottomModal();
                });
                OpenFileTemplateTab();
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}


$(document).ready(function () {
    $(".add_file_template_inner").height(window.innerHeight - ($(".addFileTemplateHeader").height() + $(".SaveChangesDiv").height() + 41));
    $("#SaveChanges").click(function () {
        SaveChanges(false);
    });
    $("#RestoreDefault").click(function () {
        SaveChanges(true);
    });
    $("#btn_del_File").click(function () {
        OpenConfirmationMessageNew("Confirmation", "Are you sure, you want to delete this item?", function () {
            var idval = $("#btn_del_File").attr('data-id');
            $.ajax({
                type: "POST",
                ajaxStart: $(".loader-div").show(),
                url: "/File/DeleteFileManagementFile",
                data: JSON.stringify({ id: idval}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                success: function (data) {
                    console.log("delete test");
                    if (data) {
                        OpenSuccessMessageNew("Success", "File Deleted Successfully");
                        CloseTopToBottomModal();
                        OpenFileTemplateTab();

                    } else {
                        OpenErrorMessageNew("Error!", "File Deleted Failed");
                    }
                    
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $(".loader-div").hide();
                    console.log(errorThrown);
                }
            });
        })
    });
    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_Doc").removeClass('hidden');
            $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/blank_thumb_file.png');
            $(".chooseFilebtn").removeClass("hidden");
            $(".changeFilebtn").addClass("hidden");
            $(".deleteDoc").addClass("hidden");
            $("#Preview_Doc").attr('src', "");
            $("#Frame_Doc").attr('src', "");
            $("#UploadSuccessMessage").hide();
            $("#BodyContent").val("");
            $("#UploadedPath").val('');
            $(".fileborder").addClass('border_none');
            $("#UploadCustomerFileBtn").removeClass('otherfileposition');
        });
    });
    
    $("#UploadCustomerFileBtn").click(function () {
        console.log("sdfdsf");
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $("#SaveAndReview").click(function () {
            console.log("test");
            if ($("#TemplateId").val() > 0) {
                console.log($("#customerId").val());

                    var Param = {
                        Id: $("#TemplateId").val(),
                        RestoreDefault: false,
                        FileBody: tinyMCE.get('BodyContent').getContent(),
                    };
                    var url = domainurl + "/File/AddFileTemplate/";
                    $.ajax({
                        type: "POST",
                        ajaxStart: function () { },
                        url: url,
                        data: JSON.stringify(Param),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        cache: false,
                        success: function (data) {
                            if (data.result) {
                                OpenTopToBottomModal(domainurl + "/File/GetFileTemplateForPopUp/?fileTemplateId=" + $("#TemplateId").val() + "&customerId=" + CustomerLoadId);
                            }
                            else {
                                OpenErrorMessageNew("Error!", data.message);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(errorThrown);
                        }
                    });
                
            }
    });
    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/File/UploadFileTemplate',
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
                if (ext[1] == 'html') {
                if (data.files[0].size <= 50000000) {
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
            if ($("#BodyContent").val() == "") {
                var filename = data.result.FullFilePath;
                tinymce.get("BodyContent").setContent(filename);
            }

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

});

$("#BodyContent").blur(function () {
    if ($("#BodyContent").val() == "") {
        $("#DescriptionError").removeClass("hidden");
    }
    if ($("#BodyContent").val() != "") {
        $("#DescriptionError").addClass("hidden");
    }
})
$("#UploadedPath").blur(function () {
    if ($("#UploadedPath").val() == "") {
        $("#uploadfileerror").removeClass("hidden");
        $(".fileborder").addClass('red-border');
    }
    if ($("#UploadedPath").val() != "") {
        $("#uploadfileerror").addClass("hidden");
        $(".fileborder").removeClass('red-border');
    }
})