$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();

    $("#add-package-device").click(function () {
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageDevicePartial/?Id=0");
    });

    $(".company-package-device-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageDeviceListPartial");
})