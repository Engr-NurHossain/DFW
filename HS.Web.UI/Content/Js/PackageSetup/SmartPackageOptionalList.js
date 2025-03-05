var DataTablePageSize = 50;
var PacakgeDeleteId = 0;
var table = $('#packageOptionalListTable').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available",
        searchPlaceholder: "Search By Equipment Name, Package Name"

    }
});

var LoadpackageOptionalList = function () {
    $(".company-package-optional-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageOptionalListPartial");
}

var DeletePackageOptional = function () {
    $.ajax({
        url: domainurl + "/SmartPackageSetup/DeleteSmartPackageEquipmentService",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadpackageOptionalList();
    });
}
$(document).ready(function () {
    $('.dataTables_filter input[type="search"]').css(
        { 'width': '400px', 'display': 'inline-block' }
    );
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".edit-package-Optional").click(function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageOptionalPartial/?Id=" + PackageId);
    })
    $(".delete-package-Optional").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageOptional);
    })
})