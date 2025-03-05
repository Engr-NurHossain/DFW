var UserFileUploadjqXHRData;

$(document).ready(function () {
    $("#UploadCustomerFileBtn").click(function () {
        console.log("sdfdsf");
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $("#btnchange").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: "/File/UploadCompanyImage",
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            if (ext[1] == 'doc' || ext[1] == 'docx' || ext[1] == 'xls' || ext[1] == 'xlsx' || ext[1] == 'jpeg' || ext[1] == 'jpg' || ext[1] == 'gif' || ext[1] == 'png' || ext[1] == 'rtf' || ext[1] == 'pdf' || ext[1] == 'txt' || ext[1] == 'mp4' || ext[1] == 'mov') {

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
                    $("#UploadCustomerFileBtn").parent().parent().addClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").addClass('custom-file');
                    $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                    $(".fileborder").addClass('border_none');
                    $(".LoadPreviewDocument").removeClass("hidden");
                    $(".LoadPreviewDocument div img").attr('src', data.result.FullFilePath);
                }
                else if (spfile[index] == "pdf") {
                    $("#UploadCustomerFileBtn").parent().parent().addClass("hidden");
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                    $(".LoadPreviewDocument").removeClass("hidden");
                    $(".LoadPreviewDocument div img").attr('src', data.result.FullFilePath);
                }
                else if (spfile[index] == "doc" || spfile[index] == "docx") {
                    $("#UploadCustomerFileBtn").parent().parent().addClass("hidden");
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                    $(".LoadPreviewDocument").removeClass("hidden");
                    $(".LoadPreviewDocument div img").attr('src', data.result.FullFilePath);
                }
                else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                    $("#UploadCustomerFileBtn").parent().parent().addClass("hidden");
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/mp4.png');
                    $(".fileborder").removeClass('border_none');
                    $(".LoadPreviewDocument").removeClass("hidden");
                    $(".LoadPreviewDocument div img").attr('src', data.result.FullFilePath);
                }
                else {
                    $("#UploadCustomerFileBtn").parent().parent().addClass("hidden");
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                    $(".LoadPreviewDocument").removeClass("hidden");
                    $(".LoadPreviewDocument div img").attr('src', data.result.FullFilePath);
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