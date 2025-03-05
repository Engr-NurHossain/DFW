var UserFileUploadjqXHRData;
var PhotoIndexNumber = 0;
var count;
var jqXHR = []
//var ReloadFIlesTab = function () {
//    $("#FilesTab").load("/File/CustomerFiles/" + customerId);
//}

$(document).ready(function () {
    $('.parking_lot_prev_img').delegate('.image-delete-parking', 'click', function (evt) {
        evt.preventDefault();
        var idval = $($(this).parent().parent()).attr('data-id');
        OpenConfirmationMessageNew("Delete", "Are you sure, you want to delete this item?", function () {
            $("#parking_path_" + idval).remove();
        })
    });
    $('.elevator_prev_img').delegate('.image-delete-elevator', 'click', function (evt) {
        evt.preventDefault();
        var idval = $($(this).parent().parent()).attr('data-id');
        OpenConfirmationMessageNew("Delete", "Are you sure, you want to delete this item?", function () {
            $("#elevator_path_" + idval).remove();
        })
    });
    $('.additional_prev_img').delegate('.image-delete-additional', 'click', function (evt) {
        evt.preventDefault();
        var idval = $($(this).parent().parent()).attr('data-id');
        OpenConfirmationMessageNew("Delete", "Are you sure, you want to delete this item?", function () {
            $("#additional_path_" + idval).remove();
        })
    });
    $('.blueprint_prev_img').delegate('.image-delete-blueprint', 'click', function (evt) {
        evt.preventDefault();
        var idval = $($(this).parent().parent()).attr('data-id');
        OpenConfirmationMessageNew("Delete", "Are you sure, you want to delete this item?", function () {
            $("#blueprint_path_" + idval).remove();
        })
    });
    $("#SaveCustomerFiles").click(function () {
        if (CommonUiValidation()) {
            if ($("#UploadedPath").val() != "") {
                SaveCustomerFile();
            }
            else {
                OpenConfirmationMessageNew("Error!", "Please upload ERP company logo.")
            }
        }
    });

    $("#UploadOverviewContentFileBtn").click(function () {
        //count = $("#UploadOverviewContentFileBtn").attr('data-count');
        typeval = $("#UploadOverviewContentFileBtn").attr('data-typeval');
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $("#UploadOverviewContentFileBtn_elevator").click(function () {
        //count = $("#UploadOverviewContentFileBtn_elevator").attr('data-count');
        typeval = $("#UploadOverviewContentFileBtn_elevator").attr('data-typeval');
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $("#UploadOverviewContentFileBtn_additional").click(function () {
        //count = $("#UploadOverviewContentFileBtn_additional").attr('data-count');
        typeval = $("#UploadOverviewContentFileBtn_additional").attr('data-typeval');
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $("#UploadOverviewContentFileBtn_blueprint").click(function () {
        //count = $("#UploadOverviewContentFileBtn_blueprint").attr('data-count');
        typeval = $("#UploadOverviewContentFileBtn_blueprint").attr('data-typeval');
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });

    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: '/MenuManagement/UploadMenuItemAdditionalPhoto',
        formData: { type: typeval },
        dataType: 'json',
        add: function (e, data) {
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            UserFileUploadjqXHRData = data;
            data.formData = { type: typeval };
        },
        progress: function (e, data) {
            if (typeval == "Blueprint") {
                var percentVal = parseInt(data.loaded / data.total * 100, 10);
                $(".file-progress_blueprint").show();
                $(".file-progress_blueprint .progress-bar").animate({
                    width: percentVal + "%"
                }, 40);
                $(".file-progress_blueprint .progress-bar span").text(percentVal + '%');
            }
            else {
                var percentVal = parseInt(data.loaded / data.total * 100, 10);
                $(".menu_item_file-progress").show();
                $(".menu_item_file-progress .progress-bar").animate({
                    width: percentVal + "%"
                }, 40);
                $(".menu_item_file-progress .progress-bar span").text(percentVal + '%');
            }
        },
        done: function (event, data) {
            if (typeval == "Blueprint") {
                setTimeout(function () {
                    $(".file-progress_blueprint").hide();
                    $(".file-progress_blueprint .progress-bar").animate({
                        width: 0 + "%"
                    }, 0);
                    $(".file-progress_blueprint .progress-bar span").text(0 + '%');
                }, 500);
            }
            else {
                setTimeout(function () {
                    $(".menu_item_file-progress").hide();
                    $(".menu_item_file-progress .progress-bar").animate({
                        width: 0 + "%"
                    }, 0);
                    $(".menu_item_file-progress .progress-bar span").text(0 + '%');
                }, 500);
            }
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $(".parking_lot_prev_img").append("<div class='prev_image_parking' id='parking_path_" + count + "' data-id=" + count + " data-val=" + typeval + " style='float: left; margin-right: 10px; margin-bottom: 10px;'><div style = 'border:1px solid #ccc;width:150px;height:150px;position:relative' class = 'park_div'><div class='image-delete-parking'><img src='/Content/Icons/cross-image-delete.png' style='background-color: white;width: 20px;border-radius: 10px;' /></div><img class = 'img_div_parking' src = '" + data.result.FullFilePath + "' style='max-width:100%;width:auto;height:auto;position: absolute;top: 0;bottom: 0;left: 0;right: 0;margin: auto;' data-path='" + data.result.filePath + "' data-name='" + data.result.NameFile + "' /></div></div>");
                count = count + 1;
                //if (data.result.typename == "ParkingLot") {
                    
                //}
                //else if (data.result.typename == "Elevator") {
                //    $(".elevator_prev_img").append("<div class='prev_image_elevator' id='elevator_path_" + count + "' data-id=" + count + " data-val=" + typeval + "><div style = 'border:1px solid #ccc;width:100px;height:100px;position:relative' class = 'elevator_div'><div class='image-delete-elevator'><img src='/Content/Icons/cross.png' style='background-color: white;width: 20px;border-radius: 10px;' /></div><img class = 'img_div_elevator' src = '" + data.result.FullFilePath + "' /></div><input class='form-control elevator_caption' placeholder='Caption' style='width:100px;' /></div>");
                //    count = count + 1;
                //}
                //else if (data.result.typename == "Additional") {
                //    $(".additional_prev_img").append("<div class='prev_image_additional' id='additional_path_" + count + "' data-id=" + count + " data-val=" + typeval + "><div style = 'border:1px solid #ccc;width:100px;height:100px;position:relative' class = 'additional_div'><div class='image-delete-additional'><img src='/Content/Icons/cross.png' style='background-color: white;width: 20px;border-radius: 10px;' /></div><img class = 'img_div_additional' src = '" + data.result.FullFilePath + "' /></div><input class='form-control additional_caption' placeholder='Caption' style='width:100px;' /></div>");
                //    count = count + 1;
                //}
                //else if (data.result.typename == "Blueprint") {
                //    $(".blueprint_prev_img").append("<div class='prev_image_blueprint' id='blueprint_path_" + count + "' data-id=" + count + " data-val=" + typeval + "><div style = 'border:1px solid #ccc;width:100px;height:100px;position:relative' class = 'blueprint_div'><div class='image-delete-blueprint'><img src='/Content/Icons/cross.png' style='background-color: white;width: 20px;border-radius: 10px;' /></div><img class = 'img_div_blueprint' src = '" + data.result.FullFilePath + "' /></div><input class='form-control blueprint_caption' placeholder='Caption' style='width:100px;' /></div>");
                //    count = count + 1;
                //}
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