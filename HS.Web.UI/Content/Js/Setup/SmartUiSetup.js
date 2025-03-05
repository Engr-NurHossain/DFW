var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var CSPages = {
    LoadComDocTab: function () {
        $(".LoadCompanyDocument").html(TabsLoaderText);
        $(".LoadCompanyDocument").load(domainurl + "/Setup/CompanyDocumentPartial/");
    },
    LoadCusUi: function () {
        $(".LoadCustomerUiSettings").html(TabsLoaderText);
        $(".LoadCustomerUiSettings").load(domainurl + "/Setup/CustomersmartUiSettingsPartial/");
    },
    LoadLeadUi: function () {
        $(".LoadLeadUiSettings").html(TabsLoaderText);
        $(".LoadLeadUiSettings").load(domainurl + "/Setup/LeadUiSettingsPartial/");
    },
    LoadEqupUi: function () {
        $(".LoadEquipmentUiSettings").html(TabsLoaderText);
        $(".LoadEquipmentUiSettings").load(domainurl + "/Setup/EquipmentUiSettingsPartial/");
    },
    LoadTabUi: function () {
        $(".LoadTabUiSettings").html(TabsLoaderText);
        $(".LoadTabUiSettings").load(domainurl + "/Setup/TabUiSettingsPartial/");
    },
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    //$(".LoadCustomerUiSettings").load(domainurl + "/Setup/CustomersmartUiSettingsPartial/");
    CSPages.LoadCusUi();
});