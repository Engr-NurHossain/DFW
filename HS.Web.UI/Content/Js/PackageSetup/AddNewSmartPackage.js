var LoadPackageList = function () {
    OpenSuccessMessageNew("Success!", "Package saved successfully.", function () {
        $(".company-packagelist-div").load(domainurl + "/SmartPackageSetup/CompanyPackageListPartial");
    });
}

var LoadInstallType = function (e) {
    var url = domainurl + "/SmartPackageSetup/LoadInstallType/" + e;
    var equipment = $("#SmartInstallTypeId");
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
var SavePackage = function (savetype) {
    var url = domainurl + "/SmartPackageSetup/AddCompanyPackage/";
    var param = {
        id: $("#Id").val(),
        PackageId: $("#PackageId").val(),
        PackageName: $("#PackageName").val(),
        PackageType: $("#PackageType").val(),
        EquipmentMaxLimit: $("#EquipmentMaxLimit").val(),
        SmartSystemTypeId: $("#SmartSystemTypeId").val(),
        SmartInstallTypeId: $("#SmartInstallTypeId").val(),
        ManufacturerId: $("#ManufacturerId").val(),
        ActivationFee: $("#ActivationFee").val(),
        NonConforming: $("#NonConforming").prop("checked"),
        MinCredit: $("#MinCredit").val(),
        MaxCredit: $("#MaxCredit").val(),
        PackageCode: $("#PackageCode").val(),
        UserType: $("#UserType").val(),
        ConformingFee: $("#ConformingFee").val(),
        CustomerNumber: $(".dropdown_cusno").val()
    };
    var MMRparam = {
        MaxMMR: 0,
        MinMMR: 0
    };
    var Fparam = JSON.stringify({ '_Package': param, '_MMRRange': MMRparam })
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            LoadPackageList();
            OpenRightToLeftModal();
            if(savetype=="saveedit")
            {
                ManagePackage(data.PackageId);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var SmartPackageType = $("#smartpackagetype").val();
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    console.log(SmartPackageType);
    if (SmartPackageType != "") {
        $("#PackageType").val(SmartPackageType);
    }
    var h = window.innerHeight - 80;
    $(".PackageArea").css("height", h);
    $("#savePackage").click(function () {
        if (CommonUiValidation()) {
            SavePackage("save");
        }
    });
    $("#saveEditPackage").click(function () {
        if (CommonUiValidation()) {
            SavePackage("saveedit");
        }
    })
})