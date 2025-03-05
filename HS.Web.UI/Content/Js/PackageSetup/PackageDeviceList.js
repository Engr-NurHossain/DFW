var DataTablePageSize = 50;
var PacakgeDeleteId = 0;

var table = $('#packageDeviceListTable').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});

var LoadPackageDeviceList = function () {
    $(".company-package-device-list-div").load(domainurl + "/Leads/CompanyPackageDeviceListPartial");
}
var DeletePackageDevice = function () {
    $.ajax({
        url: domainurl + "/Leads/DeletePackageDevice",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadPackageDeviceList();
    });
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".edit-package-Device").click(function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackageDevicePartial/?Id=" + PackageId);
    })

    $(".delete-package-Device").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageDevice);
    })
})
