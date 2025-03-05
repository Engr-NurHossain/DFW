var UserFileUploadjqXHRData;
var GetItemUrlSlugPermissionReturn = false;

var HideNewMenuItemPartial = function () {
    $("#AddNewMenuItemPartial").hide();
}

var LoadMenuItemList = function () {
    $("#MenuItemList").load(domainurl + "/MenuManagement/MenuItemListPartial?Id=" + $("#id").val());
}
var SaveMenuItem = function (edititem, type) {
    if ($("#UrlSlug_item").val() != "") {
        weburl = weburlslug + "/" + menunameslug + "/" + $("#UrlSlug_item").val();
    }
    var url = domainurl + "/MenuManagement/AddMenuItem/";
    var param;
    param = {
        Id: $("#MenuItemId").val(),
        MenuId: $("#Menu_id").val(),
        CategoryNameList: $("#Categories").val(),
        ItemName: $("#miName").val(),
        ItemNumber: $("#miNumber").val(),
        ItemLevel: $("#MenuLevel").val(),
        Description: $('#DescriptionForItem').val(),
        Photo: $("#UploadedPathForItem").val(),
        MaxQty: $("#MaxQuantity").val(),
        DaysAvailable: String($("#selectDaysAvailableForItem").val()),
        TimeFrom: $("#timeFromForItem").val(),
        TimeTo: $("#timeToForItem").val(),
        Price: $("#MenuPrice").val(),
        Status: $("#MenuItemStatus").val() == "1" ? true : false,
        ToppingNameList: $("#Toppings").val(),
        DaysAvailableOption: $("#day_available_option_item").val(),
        TimeAvailableOption: $("#time_available_option_item").val(),
        UrlSlug: $("#UrlSlug_item").val(),
        WebsiteURL: weburl,
        MetaTitle: $("#MetaTitle_item").val(),
        MetaDescription: $("#MetaDescription_item").val(),
        DeliveryTime: $("#deliverytime").val(),
        TaxPercentage: $("#taxpercentage").val(),
        IsTax: $("#tax_option_item").val() == "1" ? true : false,
        IsInstruction: $("#IsInstruction").val() == "1" ? true : false,
        PrevMenuId: PrevMenuId
    };
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({ '_MenuItem': param, 'AdditionalContent': AdditionalImageArr }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (response) {

            if (response.result == true) {
                if (typeof (edititem) != "undefined" && edititem != null && edititem != "" && edititem == true) {
                    if (typeof (type) != "undefined" && type != null && type != "" && type == true) {
                        OpenIeateryPopupModal("Success!", response.message);
                        CloseTopToBottomModal();
                        //$(".LoadAllItemsList").load("/MenuManagement/AllItemsTabListPartial");
                        LoadAllItemsTab(1);
                    }
                    else {
                        OpenIeateryPopupModal("Success!", response.message)
                        OpenTopToBottomModal("/MenuManagement/AddMenuItem?menuId=" + response.menuid + "&miId=" + response.id + "&edititem=true");
                        //$(".LoadAllItemsList").load("/MenuManagement/AllItemsTabListPartial");
                        LoadAllItemsTab(1);
                    }
                }
                else {
                    OpenIeateryPopupModal("Success!", response.message);
                    HideNewMenuItemPartial();
                    LoadMenuItemList();
                }
            }
            else if (response.result == false) {
                OpenErrorMessageNew("Error!", "Menu item saved failed.");
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var SaveMenuItemAndNew = function (edititem) {
    if ($("#UrlSlug_item").val() != "") {
        weburl = weburlslug + "/" + menunameslug + "/" + $("#UrlSlug_item").val();
    }
    var url = domainurl + "/MenuManagement/AddMenuItem/";
    var param;
    param = {
        Id: $("#MenuItemId").val(),
        MenuId: $("#Menu_id").val(),
        CategoryNameList: $("#Categories").val(),
        ItemName: $("#miName").val(),
        ItemNumber: $("#miNumber").val(),
        ItemLevel: $("#MenuLevel").val(),
        Description: $('#DescriptionForItem').val(),
        Photo: $("#UploadedPathForItem").val(),
        MaxQty: $("#MaxQuantity").val(),
        DaysAvailable: String($("#selectDaysAvailableForItem").val()),
        TimeFrom: $("#timeFromForItem").val(),
        TimeTo: $("#timeToForItem").val(),
        Price: $("#MenuPrice").val(),
        Status: $("#MenuItemStatus").val() == "1" ? true : false,
        ToppingNameList: $("#Toppings").val(),
        DaysAvailableOption: $("#day_available_option_item").val(),
        TimeAvailableOption: $("#time_available_option_item").val(),
        UrlSlug: $("#UrlSlug_item").val(),
        WebsiteURL: weburl,
        MetaTitle: $("#MetaTitle_item").val(),
        MetaDescription: $("#MetaDescription_item").val(),
        DeliveryTime: $("#deliverytime").val(),
        TaxPercentage: $("#taxpercentage").val(),
        IsTax: $("#tax_option_item").val() == "1" ? true : false,
        IsInstruction: $("#IsInstruction").val() == "1" ? true : false

    };
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify({ '_MenuItem': param, 'AdditionalContent': AdditionalImageArr }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (response) {

            if (response.result == true) {
                if (typeof (edititem) != "undefined" && edititem != null && edititem != "" && edititem == true) {
                    OpenIeateryPopupModal("Success!", response.message)
                    OpenTopToBottomModal("/MenuManagement/AddMenuItem?menuId=" + response.menuid + "&miId=" + 0 + "&edititem=true");
                    //$(".LoadAllItemsList").load("/MenuManagement/AllItemsTabListPartial");
                    LoadAllItemsTab(1);
                }
                else {
                    OpenIeateryPopupModal("Success!", response.message);
                    HideNewMenuItemPartial();
                    LoadMenuItemList();
                }
            }
            else if (response.result == false) {
                OpenErrorMessageNew("Error!", "Menu item saved failed.");
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var GetItemUrlSlugPermission = function (count) {
    var url = domainurl + "/MenuManagement/GetItemUrlSlugPermission";
    var param;
    param = JSON.stringify({
        id: citemid,
        urlslug: $("#UrlSlug_item").val(),
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
                    SaveMenuItem();
                }
                else if (count == 2) {
                    SaveMenuItem(true, false);
                }
                else if (count == 3) {
                    SaveMenuItem(true, true);
                }
                else if (count == 4) {
                    SaveMenuItemAndNew(true);
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
    $('.selectpicker').selectpicker();
    console.log("addMenu");

    $("#btnSaveEmgContact").click(function () {
        console.log("sdfdsf");
        $(".parking_lot_prev_img .prev_image_parking").each(function () {
            AdditionalImageArr.push({
                Name: $($(this).find('.park_div .img_div_parking')).attr('data-name'),
                ImageLoc: $($(this).find('.park_div .img_div_parking')).attr('data-path'),
            });
        })
        if (CommonUiValidation()) {
            if ($("#selectDaysAvailableForItem").val() != null) {
                GetItemUrlSlugPermission(1);
                
                
            }
            else {
                OpenErrorMessageNew("Error", "Please select menu item days available");
            }
        }
        else {
            OpenErrorMessageNew("Error!", "Please Input Required Fields!");
            $(".required-field").css("border", "1px solid red");
            $(".custom-span").show();
        }
    });
    $("#btnSaveEmgContact_edititem").click(function () {
        console.log("sdfdsf");
        $(".parking_lot_prev_img .prev_image_parking").each(function () {
            AdditionalImageArr.push({
                Name: $($(this).find('.park_div .img_div_parking')).attr('data-name'),
                ImageLoc: $($(this).find('.park_div .img_div_parking')).attr('data-path'),
            });
        })
        if (CommonUiValidation()) {
            if ($("#selectDaysAvailableForItem").val() != null) {
                GetItemUrlSlugPermission(2);
                
                
            }
            else {
                OpenErrorMessageNew("Error", "Please select menu item days available");
            }
        }
        else {
            OpenErrorMessageNew("Error!", "Please Input Required Fields!");
            $(".required-field").css("border", "1px solid red");
            $(".custom-span").show();
        }
        
    });
    $("#btnSaveEmgContactAndClose").click(function () {
        console.log("sdfdsf");
        $(".parking_lot_prev_img .prev_image_parking").each(function () {
            AdditionalImageArr.push({
                Name: $($(this).find('.park_div .img_div_parking')).attr('data-name'),
                ImageLoc: $($(this).find('.park_div .img_div_parking')).attr('data-path'),
            });
        })
        if (CommonUiValidation()) {
            if ($("#selectDaysAvailableForItem").val() != null) {
                GetItemUrlSlugPermission(3);
                
                
            }
            else {
                OpenErrorMessageNew("Error", "Please select menu item days available");
            }
        }
        else {
            OpenErrorMessageNew("Error!", "Please Input Required Fields!");
            $(".required-field").css("border", "1px solid red");
            $(".custom-span").show();
        }

    });
    $("#btnSaveEmgContactAndNew").click(function () {
        console.log("sdfdsf");
        $(".parking_lot_prev_img .prev_image_parking").each(function () {
            AdditionalImageArr.push({
                Name: $($(this).find('.park_div .img_div_parking')).attr('data-name'),
                ImageLoc: $($(this).find('.park_div .img_div_parking')).attr('data-path'),
            });
        })
        if (CommonUiValidation()) {
            if ($("#selectDaysAvailableForItem").val() != null) {
                GetItemUrlSlugPermission(4);
                
                
            }
            else {
                OpenErrorMessageNew("Error", "Please select menu item days available");
            }
        }
        else {
            OpenErrorMessageNew("Error!", "Please Input Required Fields!");
            $(".required-field").css("border", "1px solid red");
            $(".custom-span").show();
        }

    });
    $(".deleteDocForItem").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_DocForItem").removeClass('hidden');
            $("#UploadCustomerFileBtnForItem").attr('src', '/Content/img/toppng.com-file-upload-image-icon-980x980.png');
            $(".chooseFilebtnForItem").removeClass("hidden");
            $(".changeFilebtnForItem").addClass("hidden");
            $(".deleteDocForItem").addClass("hidden");
            $("#Preview_DocForItem").attr('src', "");
            $("#Frame_DocForItem").attr('src', "");
            $("#UploadSuccessMessageForItem").hide();
            $("#BodyContentForItem").val("");
            $("#UploadedPathForItem").val('');
            $(".fileborderForItem").addClass('border_noneForItem');
            $("#UploadCustomerFileBtnForItem").removeClass('otherfileposition');
        });
    });

    $("#UploadCustomerFileBtnForItem").click(function () {
        console.log("sdfdsf");
        $("#UploadedFileForItem").click();
        $("#UploadSuccessMessageForItem").addClass('hidden');
    });

    $(".change-picture-logo").click(function () {
        $("#UploadedFileForItem").click();
        $("#UploadSuccessMessageForItem").addClass('hidden');
    });
    $('#UploadedFileForItem').fileupload({
        pasteZone: null,
        url: domainurl + '/MenuManagement/UploadMenuItemPhoto',
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
            if ($("#BodyContentForItem").val() == "") {
                var filename = data.result.FullFilePath;
                tinymce.get("BodyContentForItem").setContent(filename);
            }

            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);

            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessageForItem").removeClass('hidden');
                $("#UploadedPathForItem").val(data.result.FullFilePath);
                var spfile = data.result.FullFilePath.split('.');
                $(".fileborderForItem").removeClass('red-border');
                $("#uploadfileerrorForItem").addClass("hidden");

                var index = spfile.length - 1;
                if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                    $("#UploadCustomerFileBtnForItem").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtnForItem").addClass("hidden");
                    $(".changeFilebtnForItem").removeClass("hidden");
                    $(".deleteDocForItem").removeClass("hidden");
                    $("#UploadCustomerFileBtnForItem").addClass('custom-file');
                    $("#UploadCustomerFileBtnForItem").removeClass('otherfileposition');
                    $(".fileborderForItem").addClass('border_noneForItem');
                }
                else if (spfile[index] == "pdf") {
                    $(".chooseFilebtnForItem").addClass("hidden");
                    $(".changeFilebtnForItem").removeClass("hidden");
                    $(".deleteDocForItem").removeClass("hidden");
                    $("#UploadCustomerFileBtnForItem").attr('src', domainurl + '/Content/Icons/pdf.png');
                    $("#UploadCustomerFileBtnForItem").addClass('otherfileposition');
                    $("#UploadCustomerFileBtnForItem").removeClass('custom-file');
                    $(".fileborderForItem").removeClass('border_noneForItem');
                }
                else if (spfile[index] == "doc" || spfile[index] == "docx") {
                    $(".chooseFilebtnForItem").addClass("hidden");
                    $(".changeFilebtnForItem").removeClass("hidden");
                    $(".deleteDocForItem").removeClass("hidden");
                    $("#UploadCustomerFileBtnForItem").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtnForItem").addClass('otherfileposition');
                    $("#UploadCustomerFileBtnForItem").removeClass('custom-file');
                    $(".fileborderForItem").removeClass('border_noneForItem');
                }
                else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                    $(".chooseFilebtnForItem").addClass("hidden");
                    $(".changeFilebtnForItem").removeClass("hidden");
                    $(".deleteDocForItem").removeClass("hidden");
                    $("#UploadCustomerFileBtnForItem").attr('src', '/Content/Icons/mp4.png');
                    $(".fileborderForItem").removeClass('border_noneForItem');
                }
                else {
                    $(".chooseFilebtnForItem").addClass("hidden");
                    $(".changeFilebtnForItem").removeClass("hidden");
                    $(".deleteDocForItem").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborderForItem").removeClass('border_noneForItem');
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
    $("#UploadedFileForItem").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });

    $("#UploadedPathForItem").blur(function () {
        if ($("#UploadedPathForItem").val() == "") {
            $("#uploadfileerrorForItem").removeClass("hidden");
            $(".fileborderForItem").addClass('red-border');
        }
        if ($("#UploadedPathForItem").val() != "") {
            $("#uploadfileerrorForItem").addClass("hidden");
            $(".fileborderForItem").removeClass('red-border');
        }
    })
    $("#btnCancelEmgContact").click(function () {
        HideNewMenuItemPartial();
    });
    $("#btnCancelEmgContact_edititem").click(function () {
        $(".LoadMenuItemEdit").addClass("hidden");
        $(".menuitem_list_allitem").removeClass("hidden");
    })
});