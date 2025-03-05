$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();

    $("#add-Package-include-products").click(function () {
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageIncludePartial/?Id=0");
    });

    $(".company-package-include-products-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageIncludeListPartial");
})