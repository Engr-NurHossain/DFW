var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#AgingReportTab").html(TabsLoaderText);
    $("#AgingReportTab").load(domainurl + "/Reports/Aging");
});