var EditZipCode = function (id) {
    OpenRightToLeftModal(domainurl + "/ServiceArea/AddZipCode?Id=" + id);
}
var DeleteZipCode = function(id)
{
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this zip code?", function () {
        DeleteZipCodeById(id)
    });
}
var DeleteZipCodeById = function (id) {
    $.ajax({
        url: domainurl + "/ServiceArea/DeleteZipCode",
        data: { Id: id },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Zip Code deleted successfully.");
                $("#LoadAreaZipCode").load(domainurl + "/ServiceArea/AreaZipCode");
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    
    
});