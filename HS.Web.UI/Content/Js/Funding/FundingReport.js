var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var LoadTermSheetTab = function () {
    $("#PayrollTermSheetTab").html(TabsLoaderText);
    $("#PayrollTermSheetTab").load(domainurl + "/Reports/TermSheet");
}
var LoadSalesPayTab = function () {
    $("#SalesPayTab").html(TabsLoaderText);
    $("#SalesPayTab").load(domainurl + "/Reports/SalesPay");
}
var LoadSettingTab = function () {
    $("#PayrollSettingTab").html(TabsLoaderText);
    $("#PayrollSettingTab").load(domainurl + "/Reports/PayrollSetting");
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    LoadSalesPayTab();

    $(".SalesPayNav").click(function () {
        LoadSalesPayTab();
    });
    $(".PayrollTermSheetNav").click(function () {
        LoadTermSheetTab();
    });
    $(".PayrollSettingNav").click(function () {
        LoadSettingTab();
    });
});