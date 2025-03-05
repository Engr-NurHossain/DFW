$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    $("#add-package-optional").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackageOptionalPartial/?Id=0");
    });
    $(".company-package-optional-list-div").load(domainurl + "/Leads/CompanyPackageOptionalListPartial");
})