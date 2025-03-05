var UserFileUploadjqXHRData;
var SaveTicketStatusImageSetting = function () {
    var url = domainurl + "/Calendar/SaveTicketStatusImageSetting/";
    var param = JSON.stringify({
        Id: $("#TicketStatusImageSetting_Id").val(),
        TicketStatus: $("#TicketStatusImageSetting_TicketStatus").val(),
        Filename: $("#UploadedPath").val(),
        TicketStatusColor: $("#ColorDisplay").val()
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
            $("#savesign").show();
            $("#savesign").fadeOut(3000);
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}
var SaveTicketStatusColor = function () {
    var url = domainurl + "/Calendar/SaveTicketStatusColor/";
    var param = JSON.stringify({
        Id: $("#TicketStatusImageSetting_Id").val(),
        TicketStatus: $("#TicketStatusImageSetting_TicketStatus").val(),
        TicketStatusColor: $("#ColorDisplay").val()
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
            $("#savesign").show();
            $("#savesign").fadeOut(3000);
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}
$(document).ready(function () {
    $("#savesign").hide();
    $("#btn_imagesetting").click(function () {
        if (CommonUiValidation() && $("#UploadedPath").val() != "") {
            SaveTicketStatusImageSetting();
            $(".fileborder").removeClass('red-border');
        }
        else {
            if ($("#ColorChangeId").val() != $("#ColorDisplay").val() && $("#ColorDisplay").val() != '' && $("#ColorDisplay").val() != null && $("#ColorDisplay").val() != 'undefined') {
                SaveTicketStatusColor();
                $(".fileborder").removeClass('red-border');
            }
            else {
                $("#uploadfileerror").removeClass("hidden");
                $(".fileborder").addClass('red-border');
            }
        }            
    });
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
        url: domainurl + '/File/UploadStatusImageFile', 
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if ( ext[1] == 'png') {

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
                if (spfile[index] == "png" || spfile[index] == "PNG") {
                    
                    $("#UploadCustomerFileBtn").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").addClass('custom-file');
                    $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                    $(".fileborder").addClass('border_none');
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