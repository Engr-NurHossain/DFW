var LoadPackageList = function () {
    OpenSuccessMessageNew("Success!", "Package saved successfully.", function () {
        $(".company-packagelist-div").load(domainurl + "/Leads/CompanyPackageListPartial");
    });
}


var SavePackage = function () {
    var url = domainurl + "/Leads/AddCompanyPackage/";
    var param = {
        id: $("#Id").val(),
        name: $("#Name").val(),
        Rate: $("#Rate").val()
        //optionEqpMaxLimit: $("#OptionEqpMaxLimit").val()
    };
    //var MMRparam = {
    //    MaxMMR: $("#MMRRange_MaxMMR").val(),
    //    MinMMR: $("#MMRRange_MinMMR").val()
    //};
    var Fparam = JSON.stringify({ '_Package': param })
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
    $("#savePackage").click(function () {
        if (CommonUiValidation()) {
            //if (parseInt($("#MMRRange_MaxMMR").val()) > parseInt($("#MMRRange_MinMMR").val())) {
                SavePackage();
            //}
            //else {
            //    OpenErrorMessageNew("Error!", "Max value couldn't be smaller than Min Value.", "");
            //}
        }
        else {
            OpenErrorMessageNew("Error!", "Package not save successfully.", "");
        }
    })
})