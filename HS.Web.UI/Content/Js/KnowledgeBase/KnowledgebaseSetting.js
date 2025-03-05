
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

var LoadKnowledgebaseSettings = function () {
    $(".KnowledgebaseSettings").html(TabsLoaderText);
    $(".KnowledgebaseSettings").load(domainurl + "/Sales/ArticleSettingList");
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    LoadKnowledgebaseSettings();

    $(".KnowledgebaseSettingsTab").click(function () {
        $("#KnowledgebaseSettingsTab").addClass('active');
        $(".KnowledgebaseSettingsTab").addClass('active');
        LoadKnowledgebaseSettings();
    });
});