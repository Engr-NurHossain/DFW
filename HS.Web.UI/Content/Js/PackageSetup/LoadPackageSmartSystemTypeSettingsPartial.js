$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    $("#add-package-smart-system").click(function () {
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageSmartSystemPartial/?Id=0");
    });
    $(".company-package-smart-system-type-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageSmartSystemListPartial");
})