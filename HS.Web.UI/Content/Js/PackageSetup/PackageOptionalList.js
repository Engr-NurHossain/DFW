var DataTablePageSize = 50;
var PacakgeDeleteId = 0;
var table = $('#packageOptionalListTable').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});

var LoadpackageOptionalList = function () {
    $(".company-package-optional-list-div").load(domainurl + "/Leads/CompanyPackageOptionalListPartial");
}

var DeletePackageOptional = function () {
    $.ajax({
        url: domainurl + "/Leads/DeletePackageOptional",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadpackageOptionalList();
    });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".edit-package-Optional").click(function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackageOptionalPartial/?Id=" + PackageId);
    })
    $(".delete-package-Optional").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageOptional);
    })
})