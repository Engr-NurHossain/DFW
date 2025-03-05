var SaveLocalizeResource = function (ResourceId) {
    var ResourceName = $("#ResourceName").val();
    var ResourceVal = $("#ResourceVal").val();
    var LanguageId = $("#LanguageId").val();
    $.ajax({
        type: "POST",
        url: "Setup/AddLocalizeResource/",
        data: {
            LanguageId: LanguageId,
            ResourceName: ResourceName,
            ResourceValue: ResourceVal
        },
        success: function (data) {
            OpenSuccessMessageNew("Success", data.message, function () {
                $("#Right-To-Left-Modal-Body .close").click();
            });
        }
    });
}

$(document).ready(function () {
    $("#SaveLocalizeResource").click(function () {
        if (CommonUiValidation()) {
            SaveLocalizeResource();
        }
    });

});