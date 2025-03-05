var LoadpackageOptionalList = function () {
    $(".top_to_bottom_modal_container").load(domainurl + "/SmartPackageSetup/packagesettingslist");
}

var DeletePackageOptional = function () {
    $.ajax({
        url: domainurl + "/SmartPackageSetup/DeletePackageOptional",
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
        OpenRightToLeftModal(domainurl + "/SmartPackageSetup/AddCompanyPackagePartial/?Id=0");
    });
    $(".company-packagelist-div").load(domainurl + "/SmartPackageSetup/CompanyPackageListPartial");
    var input = document.getElementById("PackageSearch");
    input.addEventListener("keyup", function (event) {
        // Number 13 is the "Enter" key on the keyboard
        if (event.keyCode === 13) {
            // Cancel the default action, if needed
            event.preventDefault();
            // Trigger the button element with a click
            NavigatePageListing();
        }
    });

})