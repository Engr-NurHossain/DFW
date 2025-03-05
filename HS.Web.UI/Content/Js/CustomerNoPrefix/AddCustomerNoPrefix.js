var SaveNoPrefix = function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "Customer/AddCustomerSystemNoPrefix/",
                data: {
                    Id: $("#Id").val(),
                    Name: $("#Name").val(),
                    CentralstationName: $("#CentralstationName").val()
                },
                success: function (data) {
                    if (data == true) {
                        OpenSuccessMessageNew("Success!", "Successfully insertred.");
                        $(".close").trigger("click");
                        LoadCustomerSystemNoPrefix(true);
                    }
                    else if (data == false) {
                        OpenErrorMessageNew("Error!", "Already exist !");
                    }
                }
            });
    
    }
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        /*parent.$(".modal-body").html('');*/
    });
    $('#Name').keypress(function (e) {
        if (e.which == 13) {
            SaveNoPrefix();
        }
    });

    $("#SaveNoPrefix").click(function () {
        SaveNoPrefix();
    });

});
