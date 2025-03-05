var SaveAuthorize = function () {
    if (CommonUiValidation()) {
        if (CommonUiValidation()) {
            $.ajax(
                {
                    type: "POST",
                    url: "Setup/AddAuthorizeDotNetSetting/",
                    data: {
                        Id: $("#Id").val(),
                        Name: $("#Name").val(),
                        Value: $("#Value").val(),
                    },
                    success: function () {
                        //$('.inventory-popup').dialog('close');
                        $(".close").trigger("click");
                        LoadSettings(true);
                    }
                });

        }
    }
}
$(document).ready(function () {
    $("#SaveAuthorize").click(function () {
        SaveAuthorize();
    });
    //$('#Name').keyup(function () {
    //    $('#Value').val($(this).val());
    //});
})