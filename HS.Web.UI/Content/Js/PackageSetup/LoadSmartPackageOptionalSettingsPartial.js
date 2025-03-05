$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    $("#add-package-optional").click(function () {
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageOptionalPartial/?Id=0");
    });
    $(".company-package-optional-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageOptionalListPartial");
})