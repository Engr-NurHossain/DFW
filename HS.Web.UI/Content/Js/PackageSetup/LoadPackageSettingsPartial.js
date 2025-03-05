var LoadpackageOptionalList = function () {
    $(".top_to_bottom_modal_container").load(domainurl + "/Leads/packagesettingslist");
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

$(".delete-package-Optional").click(function () {
    var PackageId = $(this).attr("idval");
    PacakgeDeleteId = PackageId;
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeletePackageOptional);
})

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();

    $("#add-Package").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/AddCompanyPackagePartial/?Id=0");
    });
    $(".company-packagelist-div").load(domainurl + "/Leads/CompanyPackageListPartial");
})