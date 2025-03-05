var SaveSettings = function () {
    $.ajax(
        {
            type: "POST",
            url: "Setup/EditSettings/",
            data: {
                Id: $("#Id").val(),
                SearchKey: $("#SearchKey").val(),
                Value: $("#Value").val(),
                CompanyId: $("#CompanyId").val(),
                IsActive: $("#IsActive").val(),
            },
            success: function () {
                $(".close").trigger("click");
                OpenSuccessMessageNew("Success!", "Successfully Update Global Setting", LoadSettings(true));
                
            }
        });
}


$(document).ready(function () {
    $("#SaveSettings").click(function () {
        SaveSettings();
    });

});
