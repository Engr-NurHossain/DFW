var LoadPackageDeviceList = function () {
    var packageId = $("#packageid").val();
    console.log(packageId);
    OpenSuccessMessageNew("Success!", "Devices saved successfully.", function () {
        if (typeof packageId != 'undefined') {
            $(".company-package-device-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageDeviceListPartial");
            var InvoiceLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
            $(".TopToBottomModal .ContentsDiv").html(InvoiceLoaderText);
            setTimeout(function () {
                $(".TopToBottomModal .ContentsDiv").load(domainurl + "/SmartPackageSetup/packagesettingslist/" + packageId);
            }, 700);
        }
        else {
            $(".company-package-device-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageDeviceListPartial");
        }
    })
}
var LoadEquipmentAndService = function (e) {
    var url = domainurl + "/Leads/LoadEquipmentAndService/" + e;
    var equipment = $("#EquipmentId");
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            equipment.empty().append('<option selected="selected" value="-1">Please Select</option>');
            $.each(data, function () {
                equipment.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var savePackageDevice = function () {
    var url = domainurl + "/SmartPackageSetup/AddCompanyPackageDevice/";
    var param = JSON.stringify({
        id: $("#Id").val(),
        PackageId: $("#PackageId").val(),
        EquipmentId: $("#EquipmentId").val(),
        EptNo: $("#EptNo").val()
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
            console.log(data);
            if (data == false) {
                OpenErrorMessageNew("Error!", "Selected Equipment already taken");
            }
            else {
                LoadPackageDeviceList();
                OpenRightToLeftModal();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {
    InitializeSuburbDropdown($('.dropdown_equipment'), $("#PackageId").val(), "1");
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#savePackageDevice").click(function () {
        if (CommonUiValidation()) {
            savePackageDevice();
        }
    })

    /*$(".PackageId_select2").select2({})*/
    /* $(".EquipmentId_select2").select2({})*/
})