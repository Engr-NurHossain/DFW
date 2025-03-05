$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();

    $("#add-package-device").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackageDevicePartial/?Id=0");
    });

    $(".company-package-device-list-div").load(domainurl + "/Leads/CompanyPackageDeviceListPartial");
})