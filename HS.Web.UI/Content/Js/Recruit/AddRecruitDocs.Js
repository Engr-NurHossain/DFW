var UserFileUploadjqXHRData;
var SaveUserDocFile = function () {
    var url = domainurl + "/Recruit/UploadDocuments";
    var param = JSON.stringify({
        Id: $("#ModelId").val(),
        FormId: $("#FormId").val(),
        FormFor: $("#FormFor").val(),
        Name: $("#Name").val(),
        FileLocation: $("#UploadedPath").val()
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
            $("#Right-To-Left-Modal-Body .close").click();
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    LoadRecruit(true)
                });
            } else {
                OpenErrorMessageNew("Error!", data.message, function () {
                    LoadRecruit(true)
                });
            }
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}
$(document).ready(function () { 
    $("#SaveCustomerFiles").click(function () {
        if (CommonUiValidation()) {
            SaveUserDocFile();
        }
    });
    $("#UploadCustomerFileBtn").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });

    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/HrDoc/UploadUserFile/',
        dataType: 'json',
        add: function (e, data) {
            /*if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }*/
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
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);

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