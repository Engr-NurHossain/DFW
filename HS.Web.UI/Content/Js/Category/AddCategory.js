var UserFileUploadjqXHRData;
var saveCategory = function () {
    if ($("#UrlSlug").val() != "") {
        weburl = weburlslug + "/" + $("#UrlSlug").val();
    }
    var url = domainurl + "/MenuManagement/AddCategory/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        CategoryName: $("#CategoryName").val(),
        Description: $('#Description').val(),
        TimeFrom: $("#timeFrom").val(),
        TimeTo: $("#timeTo").val(),
        DaysAvailable: String($("#selectDaysAvailable").val()),
        Status: $("#CategoryStatus").val()=="1"? true : false,
        Image: $("#UploadedPath").val(),
        UrlSlug: $("#UrlSlug").val(),
        DaysAvailableOption: $("#day_available_option").val(),
        TimeAvailableOption: $("#time_available_option").val(),
        WebsiteURL: weburl,
        MetaTitle: $("#MetaTitle").val(),
        MetaDescription: $("#MetaDescription").val()
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
            console.log("test");
            if (data.result == true) {

                OpenIeateryPopupModal("Success", "Category saved successfully");
                OpenTopToBottomModal(domainurl + "/MenuManagement/AddCategory/?Id=" + data.id);
            CategoryListLoad(1);
            }
            else
            {
                parent.OpenErrorMessageNew("Error!", "Category saved failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var saveCategoryAndNew = function () {
    if ($("#UrlSlug").val() != "") {
        weburl = weburlslug + "/" + $("#UrlSlug").val();
    }
    var url = domainurl + "/MenuManagement/AddCategory/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        CategoryName: $("#CategoryName").val(),
        Description: $('#Description').val(),
        TimeFrom: $("#timeFrom").val(),
        TimeTo: $("#timeTo").val(),
        DaysAvailable: String($("#selectDaysAvailable").val()),
        Status: $("#CategoryStatus").val() == "1" ? true : false,
        Image: $("#UploadedPath").val(),
        UrlSlug: $("#UrlSlug").val(),
        DaysAvailableOption: $("#day_available_option").val(),
        TimeAvailableOption: $("#time_available_option").val(),
        WebsiteURL: weburl,
        MetaTitle: $("#MetaTitle").val(),
        MetaDescription: $("#MetaDescription").val()
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
            console.log("test");
            if (data.result == true) {

                OpenIeateryPopupModal("Success", "Category saved successfully");
                OpenTopToBottomModal(domainurl + "/MenuManagement/AddCategory/?Id=" + 0);
                CategoryListLoad(1);
            }
            else {
                parent.OpenErrorMessageNew("Error!", "Category saved failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var saveCategoryAndClose = function () {
    if ($("#UrlSlug").val() != "") {
        weburl = weburlslug + "/" + $("#UrlSlug").val();
    }
    var url = domainurl + "/MenuManagement/AddCategory/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        CategoryName: $("#CategoryName").val(),
        Description: $('#Description').val(),
        TimeFrom: $("#timeFrom").val(),
        TimeTo: $("#timeTo").val(),
        DaysAvailable: String($("#selectDaysAvailable").val()),
        Status: $("#CategoryStatus").val() == "1" ? true : false,
        Image: $("#UploadedPath").val(),
        UrlSlug: $("#UrlSlug").val(),
        DaysAvailableOption: $("#day_available_option").val(),
        TimeAvailableOption: $("#time_available_option").val(),
        WebsiteURL: weburl,
        MetaTitle: $("#MetaTitle").val(),
        MetaDescription: $("#MetaDescription").val()
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
            console.log("test");
            if (data.result == true) {

                OpenIeateryPopupModal("Success", "Category saved successfully");
                CloseTopToBottomModal();
                CategoryListLoad(1);
            }
            else {
                parent.OpenErrorMessageNew("Error!", "Category saved failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var DeleteCategoryById = function (cid) {
    $.ajax({
        url: domainurl + "/MenuManagement/DeleteCategory",
        data: { Id: cid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenIeateryPopupModal("Success", data.message)
                CloseTopToBottomModal();
                CategoryListLoad(1);
            } else {
                OpenErrorMessageNew("Error!", data.message)
            }
        }
    });
}
var GetCategoryUrlSlugPermission = function (count) {
    var url = domainurl + "/MenuManagement/GetCategoryUrlSlugPermission";
    var param;
    param = JSON.stringify({
        id: ccategoryid,
        urlslug: $("#UrlSlug").val(),
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                if (count == 1) {
                    saveCategory();
                }
                else if (count == 2) {
                    saveCategoryAndClose();
                }
                else if (count == 3) {
                    saveCategoryAndNew();
                }
            }
            else {
                OpenErrorMessageNew("Error", "UrlSlug must be unique");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    var categoryid = $(".Loadcategoryitem").attr('data-id');
    $(".Loadcategoryitem").load("/MenuManagement/CategoryItemListPartial?categoryid=" + categoryid);
    $(".add_category_height").height(window.innerHeight - 115);
    //$('.selectpicker').selectpicker();
    $("#DeleteThisCategory").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete?", function () {
            DeleteCategoryById($("#Id").val())
        });
    });
    
    $("#saveCategory").click(function () {

        if (CommonUiValidation()) {
            if ($("#selectDaysAvailable").val() != null) {
                GetCategoryUrlSlugPermission(1);
                    
            }
            else {
                OpenErrorMessageNew("Error", "Please select category days available");
            }
        }
    });

    $("#saveCategoryAndClose").click(function () {

        if (CommonUiValidation()) {
            if ($("#selectDaysAvailable").val() != null) {
                GetCategoryUrlSlugPermission(2);
                
            }
            else {
                OpenErrorMessageNew("Error", "Please select category days available");
            }
        }
    });

    $("#saveCategoryAndNew").click(function () {

        if (CommonUiValidation()) {
            if ($("#selectDaysAvailable").val() != null) {
                GetCategoryUrlSlugPermission(3);
                
            }
            else {
                OpenErrorMessageNew("Error", "Please select category days available");
            }
        }
    });

    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_Doc").removeClass('hidden');
            $("#UploadCustomerFileBtn").attr('src', '/Content/img/toppng.com-file-upload-image-icon-980x980.png');
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

    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/MenuManagement/UploadCategoryPhoto',
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if (ext[1] == 'jpg' || ext[1] == 'png' || ext[1] == 'jpeg' || ext[1] == 'gif' || ext[1] == 'JPG' || ext[1] == 'PNG' || ext[1] == 'JPEG') {
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
                $("#UploadedPath").val(data.result.FullFilePath);
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
            else {
                OpenErrorMessageNew("Error", "Image dimension should be 1400 * 930");
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
});


$(window).resize(function () {
    $(".add_category_height").height(window.innerHeight - 115);

});