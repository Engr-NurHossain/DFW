var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();

    $("#companyPackageSettings").html(TabsLoaderText);
    $("#companyPackageSettings").load(domainurl + "/Leads/LoadCompanyPackageSettingsPartial");

    $("#companyPackageInchudedSettings").html(TabsLoaderText);
    $("#companyPackageInchudedSettings").load(domainurl + "/Leads/LoadcompanyPackageInchudedSettings");

    $("#companyPackageDeviceSettings").html(TabsLoaderText);
    $("#companyPackageDeviceSettings").load(domainurl + "/Leads/LoadCompanyPackageDeviceSettings");

    $("#companyPackageOptionalSettings").html(TabsLoaderText);
    $("#companyPackageOptionalSettings").load(domainurl + "/Leads/LoadCompanyPackageOptionalSettings");

})