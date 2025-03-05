var LoadPackageListManu = function (message) {
    OpenSuccessMessageNew("Success!", message, function () {
        $(".company-package-smart-system-install-type-list-div-manu").load("/SmartPackageSetup/LoadMapTypeManufacturerPartial");
    });
}
var SavePackageManu = function () {
    var url = domainurl + "/SmartPackageSetup/AddMapTypeManufacturer/";
    var param = {
        id: $("#Id").val(),
        SystemId: $("#SystemId").val(),
        ManufacturerId: $("#ManufacturerId").val()
    };
    var Fparam = JSON.stringify({ 'systemTypeManufacturerMap': param })
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                LoadPackageListManu(data.message);
                OpenRightToLeftModal();
            }
            else {
                OpenErrorMessageNew("", data.message);
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#saveMapSystemManu").click(function () {
        if (CommonUiValidation()) {
            SavePackageManu();
        }

        else {
            OpenErrorMessageNew("Error!", "Max value couldn't be smaller than Min Value.", "");
        }
    })
})