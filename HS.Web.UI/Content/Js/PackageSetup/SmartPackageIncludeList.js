var DataTablePageSize = 50;
var PacakgeDeleteId = 0;

var table = $('#packageIncludeListTable').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available",
        searchPlaceholder: "Search By Equipment Name, Package Name, Number of Equipment"

    }
});

var LoadPackageIncludeList = function () {
    $(".company-package-include-products-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageIncludeListPartial");
}

var DeletePackageInclude = function () {
    $.ajax({
        url: domainurl + "/SmartPackageSetup/DeleteSmartPackageEquipmentService",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadPackageIncludeList();
    });
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $('.dataTables_filter input[type="search"]').css(
        { 'width': '400px', 'display': 'inline-block' }
    );
    $(document).off("click", ".edit-smart-package-include").on("click", ".edit-smart-package-include", function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackageIncludePartial/?Id=" + PackageId);
    });


  
    $(document).off("click", ".delete-smart-package-include").on("click", ".delete-smart-package-include", function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageInclude);
    });

})