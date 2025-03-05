$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    $("#add-package-comission").click(function () {
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageComission/?Id=0");
    });
    $(".company-package-comission-div").load(domainurl + "/SmartPackageSetup/CompanyPackageComission");
})
