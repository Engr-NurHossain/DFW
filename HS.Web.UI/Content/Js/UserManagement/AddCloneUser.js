var SavePermissionGroup = function () {
    $.ajax(
        {
            type: "POST",
            url: "UserMgmt/AddClonePermission/",
            data: {
                CloneId: $("#Id").val(),
                Name: $("#Name").val(),
            },
            success: function (response) {
                //$('.inventory-popup').dialog('close');
                console.log(response.result);
                if (response.result) {
                    $("#vehicleId").val(response.Id);
                    OpenSuccessMessageNew("Success!", response.message, function () {
                        $(".close").click();
                        LoadUserGroup();
                    });


                }
                else {
                    OpenErrorMessageNew("Error!", response.message);
                }
           
           
            }
        });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    //$('#actName').keypress(function (e) {
    //    if (e.which == 13) {
    //        SaveActivationfee();
    //    }
    //});

    $("#SavePermissionGroup").click(function () {
        if (CommonUiValidation()) {

            SavePermissionGroup();

        }
    });
});
