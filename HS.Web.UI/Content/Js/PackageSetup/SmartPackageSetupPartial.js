var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var ClearAllTabs = function () {
    $("#companySmartPackageInchudedSettings").html(TabsLoaderText);
    $("#companySmartPackageDeviceSettings").html(TabsLoaderText);
    $("#companySmartPackageOptionalSettings").html(TabsLoaderText);
    $("#companySmartPackageServicesSettings").html(TabsLoaderText);
    $("#companySmartSystemTypeSettings").html(TabsLoaderText);
    $("#companySmartInstallTypeSettings").html(TabsLoaderText);
    $("#companySmartSystemInstallTypeSettings").html(TabsLoaderText);
    $("#companyMapTypeManufacturer").html(TabsLoaderText);
    $("#companySmartPackageComission").html(TabsLoaderText);
}

var OpenIncludedEquipmentTab = function () {
    ClearAllTabs();
    $("#companySmartPackageInchudedSettings").load(domainurl + "/SmartPackageSetup/LoadcompanySmartPackageInchudedSettings");
}
var OpenDevicesTab = function () {
    ClearAllTabs();
    $("#companySmartPackageDeviceSettings").load(domainurl + "/SmartPackageSetup/LoadCompanySmartPackageDeviceSettings");
}
var OpenOptionalEquipmentTab = function () {
    ClearAllTabs();
    $("#companySmartPackageOptionalSettings").load(domainurl + "/SmartPackageSetup/LoadCompanySmartPackageOptionalSettings");
}
var OpenServicesTab = function () {
    ClearAllTabs();
    $("#companySmartPackageServicesSettings").load(domainurl + "/SmartPackageSetup/LoadCompanySmartPackageServicesSettings");
}
var OpenSystemTypeTab = function () {
    ClearAllTabs();
    $("#companySmartSystemTypeSettings").load(domainurl + "/SmartPackageSetup/LoadcompanySmartSystemTypeSettings");
}
var OpenInstallTypeTab = function () {
    ClearAllTabs();
    $("#companySmartInstallTypeSettings").load(domainurl + "/SmartPackageSetup/LoadCompanySmartInstallTypeSettings");
}
var MapTypeTab = function () {
    ClearAllTabs();
    $("#companySmartSystemInstallTypeSettings").load(domainurl + "/SmartPackageSetup/LoadMapSystemInstallTypeSettings");
}
var MapTypeTabManufacturer = function () {
    ClearAllTabs();
    $("#companyMapTypeManufacturer").load(domainurl + "/SmartPackageSetup/LoadMapTypeManufacturer");
}
var MapTypeTabService = function () {
    ClearAllTabs();
    $("#companyMapTypeService").load(domainurl + "/SmartPackageSetup/LoadMapTypeService");
}
var PackageCommissionTab = function () {
    ClearAllTabs();
    $("#companySmartPackageComission").load(domainurl + "/SmartPackageSetup/LoadcompanySmartPackageComission");
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();

    $("#companySmartPackageSettings").html(TabsLoaderText);
    $("#companySmartPackageSettings").load(domainurl + "/SmartPackageSetup/LoadCompanySmartPackageSettingsPartial");
});