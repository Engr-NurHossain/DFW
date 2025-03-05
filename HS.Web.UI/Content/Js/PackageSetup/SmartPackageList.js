
var PacakgeDeleteId = 0;

var LoadPackageList = function () {
    $(".company-packagelist-div").load(domainurl + "/SmartPackageSetup/CompanyPackageListPartial");
}
var ManagePackage = function (packageId) {
    OpenTopToBottomModal(domainurl + "/SmartPackageSetup/PackageSettingsList?id=" + packageId);
}
var DeletePackage = function () {
    $.ajax({
        url: domainurl + "/SmartPackageSetup/DeleteSmartPackage",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadPackageList();
    });
}
var EditPackage = function (PackageId) {
    OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackagePartial/?Id=" + PackageId);
}
$(document).ready(function () {
 
 

    //$(".edit-package").click(function () {
    //    var PackageId = $(this).attr("idval");
    //    OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackagePartial/?Id=" + PackageId);
    //})

    $(".delete-package").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackage);
    })
})