$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    $("#add-package-services").click(function () {
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageServicesPartial/?Id=0");
    });
    $(".company-package-services-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageServicesListPartial?pageno=1&pagesize=50");
})