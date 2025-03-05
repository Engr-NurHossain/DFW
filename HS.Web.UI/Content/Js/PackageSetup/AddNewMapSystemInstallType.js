var LoadPackageList = function (message) {
    OpenSuccessMessageNew("Success!", message, function () {
        $(".company-package-smart-system-install-type-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageMapSmartSystemInstallListPartial");
    });
}

var LoadInstallType = function (e) {
    var url = domainurl + "/SmartPackageSetup/LoadInstallType/" + e;
    var equipment = $("#InstallTypeId");
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
var SavePackage = function () {
    var url = domainurl + "/SmartPackageSetup/AddCompanyPackageMapSmartSystem/";
    var param = {
        id: $("#Id").val(),
        SystemId: $("#SystemId").val(),
        InstallTypeId: $("#InstallTypeId").val()
    };
    var Fparam = JSON.stringify({ 'smartSystemInstallType': param })
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true)
            {
                LoadPackageList(data.message);
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
    $("#saveMapSystem").click(function () {
        if (CommonUiValidation()) {
            SavePackage();
        }

        else {
            OpenErrorMessageNew("Error!", "Max value couldn't be smaller than Min Value.", "");
        }
    })
})