var DataTablePageSize = 50;
var PacakgeDeleteId = 0;

var table = $('#packageDeviceListTable').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available",
        searchPlaceholder: "Search By Equipment Name, Package Name, Number of Equipment"

    }
});

var LoadPackageDeviceList = function () {
    $(".company-package-device-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageDeviceListPartial");
}
var DeletePackageDevice = function () {
    $.ajax({
        url: domainurl + "/SmartPackageSetup/DeleteSmartPackageEquipmentService",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadPackageDeviceList();
    });
}

$(document).ready(function () {

    $('.dataTables_filter input[type="search"]').css(
        { 'width': '400px', 'display': 'inline-block' }
    );
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".edit-smart-package-Device").click(function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageDevicePartial/?Id=" + PackageId);
    })

    $(".delete-smart-package-Device").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageDevice);
    })
})
