var LoadpackageOptionalList = function () {
    var packageId = $("#packageid").val();
    OpenSuccessMessageNew("Success!", "Package optional saved successfully.", function () {
        if (typeof packageId != 'undefined') {
            $(".company-package-optional-list-div").load(domainurl + "/Leads/CompanyPackageOptionalListPartial");
            var InvoiceLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
            $(".TopToBottomModal .ContentsDiv").html(InvoiceLoaderText);
            setTimeout(function () {
                $(".TopToBottomModal .ContentsDiv").load(domainurl + "/Leads/packagesettingslist/" + packageId);
            }, 700);
        }
        else {
            $(".company-package-optional-list-div").load(domainurl + "/Leads/CompanyPackageOptionalListPartial");
        }
    });
}

var savePackageOptional = function () {
    var url = domainurl + "/Leads/AddCompanyPackageOptional/";
    var param = JSON.stringify({
        id: $("#Id").val(),
        PackageId: $("#PackageId").val(),
        EquipmentId: $("#EquipmentId").val()
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
            if (data == false) {
                OpenErrorMessageNew("Error!", "Selected Equipment already taken");
            }
            else {
                LoadpackageOptionalList();
                OpenRightToLeftModal();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
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

$(document).ready(function () {
    InitializeSuburbDropdown($('.dropdown_equipment'), $("#PackageId").val());
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#savePackageOptional").click(function () {
        if (CommonUiValidation()) {
            savePackageOptional();
        }
    })

    /* $(".PackageId_select2").select2({})
     $(".EquipmentId_select2").select2({})*/
})