var DataTablePageSize = 50;
var PacakgeDeleteId = 0;

var table = $('#packageIncludeListTable').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});

var LoadPackageIncludeList = function () {
    $(".company-package-include-products-list-div").load(domainurl + "/Leads/CompanyPackageIncludeListPartial");
}

var DeletePackageInclude = function () {
    $.ajax({
        url: domainurl + "/Leads/DeletePackageInclude",
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
    $(".edit-package-include").click(function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackageIncludePartial/?Id=" + PackageId);
    })

    $(".delete-package-include").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageInclude);
    })
})