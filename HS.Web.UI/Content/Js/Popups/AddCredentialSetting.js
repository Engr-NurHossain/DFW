
var SaveCredentialSetting = function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "CredentialSetting/AddCredentialSetting/",
                data: {
                    Id: $("#Id").val(),
                    AcountHolderId: $("#AccountHolderNameList").val(),
                    UserName: $("#txtUsername").val(),
                    Password: $("#txtPassword").val(),
                    Description: $("#txtDescription").val()
                },
                success: function () {
                    //$('.inventory-popup').dialog('close');
                    $(".close").trigger("click");
                    LoadCredentialSetting(true);
                }
            });
    }
}
$(document).ready(function () {
    $('#txtUsername').keypress(function (e) {
        if (e.which == 13) {
            SaveCredentialSetting();
        }
    });
    $("#SaveCredetial").click(function () {
        SaveCredentialSetting();
    });
});