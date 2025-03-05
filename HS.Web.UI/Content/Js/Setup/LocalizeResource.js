var CurrentLangId = 1;
var OpenLanguageResource = function (PageNo) {
    var SearchText = $("#LocalizeSearchText").val();
    var LanguageSearchUrl = window.encodeURI(domainurl + "/Setup/ShowResources/?LangId=" + CurrentLangId + "&SearchText=" + SearchText + "&PageNo=" + PageNo);
    $(".LanguageTabLoad_" + CurrentLangId).load(LanguageSearchUrl);
}
var UpdateLocalizeResource = function (ResourceId) {
    var ResourceName = $("#ResourceName_" + ResourceId).text();
    var ResourceVal = $("#ResourceVal_" + ResourceId).val();
    $.ajax(
        {
            type: "POST",
            url: "Setup/UpdateLocalizeResource/",
            data: {
                Id: ResourceId,
                ResourceName: ResourceName,
                ResourceValue: ResourceVal
            },
            success: function (data) {
                OpenSuccessMessageNew("Success", data.message);
            }
        });
}
var DeleteLocalizeResource = function (id) {
    OpenConfirmationMessageNew("Confirmation!", "Are you sure, you want to delete this item?", function () {
        $.ajax(
        {
            type: "POST",
            url: "Setup/DeleteLocalizeResource/",
            data: {
                Id: id
            },
            success: function (data) {
                if (data) {
                    OpenSuccessMessageNew("Success", "Deleted successfully!", function () {
                        OpenLanguageResource(1);
                    });
                }
            }
        });
    })
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $(".LanguageTabs").click(function () {
        CurrentLangId = $(this).attr('lang-id');
        OpenLanguageResource(1);
    });
    $("#LocalizeSearchButton").click(function () {
        OpenLanguageResource(1);
    });
    $("#AddNewLocalizeResource").click(function () {
        OpenRightToLeftModal(domainurl + "/Setup/AddLocalizeResource/");
    });
    OpenLanguageResource(1);
});