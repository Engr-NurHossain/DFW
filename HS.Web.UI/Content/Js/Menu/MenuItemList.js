var DeleteMenuItemById = function (miId) {
    $.ajax({
        url: domainurl + "/MenuManagement/DeleteMenuItem",
        data: { Id: miId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    LoadMenuItemList();
                })
            } else {
                OpenErrorMessageNew("Error!", data.message)
            }
        }
    });
}

var DeleteMenuItem = function (DeleteId) {
    OpenConfirmationMessageNew("Confirm?", "Do you want to delete this from the list?", function () {
        DeleteMenuItemById(DeleteId);
    })
}
$(document).ready(function () {
    $(".edit-Emg-Contact").click(function () {
     
        var miId = $(this).attr('id-val');
        EditMenuItemDiv(miId);
    });
});