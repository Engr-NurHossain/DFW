var UserFileUploadjqXHRData;
$(document).ready(function () {

    parent.$('.close').click(function () {
        /*parent.$(".modal-body").html('');*/
    })
    var ReloadFIlesTab = function () {
        LoadDocumentCenter(CustomerId, true);
    }
    var SaveCustomerFile = function () {
        var url = domainurl + "/Leads/SaveLeadFile/";
        var param = JSON.stringify({
            File: $("#UploadedPath").val(),
            CustomerId: CustomerId,
            Tag: $("#FileTagList").val(),
            Description: $("#description").val(),
            _fileSize: $("#Uploaded_fileSize").val()
        });
        console.log(param);
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $('.close').trigger('click');
                OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
                    $("#DocumentCenterTab").load(domainurl + "/Leads/DocumentCenterPartial?id=" + LeadId);
                    LeadDetailTabCount();
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });

    }
    $("#SaveCustomerFiles").click(function () {
        if (CommonUiValidation() && $("#UploadedPath").val() != "") {
            SaveCustomerFile();
            $(".fileborder").removeClass('red-border');
        }
        if ($("#UploadedPath").val() == "") {
            $("#uploadfileerror").removeClass("hidden");
            $(".fileborder").addClass('red-border');
        }
    });
    $("#UploadCustomerFileBtn").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });

    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/Leads/UploadLeadFile/?CustomerId=' + CustomerId,
        dataType: 'json',
        add: function (e, data) {
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                filename = filename.replace(/[^a-zA-Z0-9\.\-]/g, '_');
                console.log("filename", filename);
                $("#description").val(filename);
            }
            UserFileUploadjqXHRData = data;
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
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);
            console.log(data.result);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);
                $("#Uploaded_fileSize").val(data.result.fileSize);
                var spfile = data.result.FullFilePath.split('.');
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
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });
});