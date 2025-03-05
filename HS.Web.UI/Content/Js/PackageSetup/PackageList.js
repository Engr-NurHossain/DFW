var DataTablePageSize = 50;
var PacakgeDeleteId = 0;

var LoadPackageList = function () {
    $(".company-packagelist-div").load(domainurl + "/Leads/CompanyPackageListPartial");
}
var ManagePackage = function (packageId) {
    OpenTopToBottomModal(domainurl + "/Leads/PackageSettingsList?id=" + packageId);
}
var DeletePackage = function () {
    $.ajax({
        url: domainurl + "/Leads/DeletePackage",
        data: { id: PacakgeDeleteId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        LoadPackageList();
    });
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    var table = $('#packageListTable').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });

    $(".edit-package").click(function () {
        var PackageId = $(this).attr("idval");
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackagePartial/?Id=" + PackageId);
    })

    $(".delete-package").click(function () {
        var PackageId = $(this).attr("idval");
        PacakgeDeleteId = PackageId;
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackage);
    })
})