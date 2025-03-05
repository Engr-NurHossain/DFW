var LoadPackageListService = function (message) {
    OpenSuccessMessageNew("Success!", message, function () {
        $(".LoadMapTypeService").load("/SmartPackageSetup/LoadMapTypeServicePartial");
    });
}
var SavePackageService = function () {
    var url = domainurl + "/SmartPackageSetup/AddMapTypeService/";
    var param = {
        id: $("#Id").val(),
        SystemTypeId: $("#SystemTypeId").val(),
        PackageIdList: $("#PackageId").val(),
        EquipmentIdList: $("#EquipmentId").val()
    };
    var Fparam = JSON.stringify({ 'systemTypeServiceMap': param })
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
                LoadPackageListService(data.message);
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
    $('.selectpicker').selectpicker();
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    });
    $("#saveMapSystemService").click(function () {
        if (CommonUiValidation()) {
            SavePackageService();
        }

        else {
            OpenErrorMessageNew("Error!", "", "");
        }
    })
})