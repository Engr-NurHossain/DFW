var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var LoadSingleSettingsTab = function () {
    $("#SingleSettingsTab").html(TabsLoaderText);
    $("#SingleSettingsTab").load(domainurl + "/Reports/SingleSettings?TermSheetId=" + TermSheetId);
}
var LoadBaseMultipleTab = function () {
    $("#BaseMultipleTab").html(TabsLoaderText);
    $("#BaseMultipleTab").load(domainurl + "/Reports/BaseMultiple?TermSheetId=" + TermSheetId);
}
var LoadCustomerBillingMethodTab = function () {
    $("#CustomerBillingMethodTab").html(TabsLoaderText);
    $("#CustomerBillingMethodTab").load(domainurl + "/Reports/CustomerBillingMethod?TermSheetId=" + TermSheetId);
}
var LoadMonthlyProductionBonusTab = function () {
    $("#MonthlyProductionBonusTab").html(TabsLoaderText);
    $("#MonthlyProductionBonusTab").load(domainurl + "/Reports/MonthlyProductionBonus?TermSheetId=" + TermSheetId);
}
var LoadCreditRatingTab = function () {
    $("#CreditRatingTab").html(TabsLoaderText);
    $("#CreditRatingTab").load(domainurl + "/Reports/CreditRating?TermSheetId=" + TermSheetId);
}
var LoadCustomerTypeTab = function () {
    $("#CustomerTypeTab").html(TabsLoaderText);
    $("#CustomerTypeTab").load(domainurl + "/Reports/CustomerType?TermSheetId=" + TermSheetId);
}
var LoadAgreementLengthTab = function () {
    $("#AgreementLengthTab").html(TabsLoaderText);
    $("#AgreementLengthTab").load(domainurl + "/Reports/AgreementLength?TermSheetId=" + TermSheetId);
}
var LoadPassThrusTab = function () {
    $("#PassThrusTab").html(TabsLoaderText);
    $("#PassThrusTab").load(domainurl + "/Reports/PassThrus?TermSheetId=" + TermSheetId);
}
var LoadInstallationFeeTab = function () {
    $("#InstallationFeeTab").html(TabsLoaderText);
    $("#InstallationFeeTab").load(domainurl + "/Reports/InstallationFee?TermSheetId=" + TermSheetId);
}
var LoadHoldBackTab = function () {
    $("#HoldBackTab").html(TabsLoaderText);
    $("#HoldBackTab").load(domainurl + "/Reports/HoldBack?TermSheetId=" + TermSheetId);
}
var LoadAdminFeeTab = function () {
    $("#AdminFeeTab").html(TabsLoaderText);
    $("#AdminFeeTab").load(domainurl + "/Reports/AdminFee?TermSheetId=" + TermSheetId);
}
var LoadTermSheetManagerTab = function () {
    $("#TermSheetManagerTab").html(TabsLoaderText);
    $("#TermSheetManagerTab").load(domainurl + "/Reports/TermSheetManager?TermSheetId=" + TermSheetId);
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    LoadBaseMultipleTab();

    $(".BaseMultipleNav").click(function () {
        LoadBaseMultipleTab();
    });
    $(".SingleSettingsNav").click(function () {
        LoadSingleSettingsTab();
    });
    $(".CustomerBillingMethodNav").click(function () {
        LoadCustomerBillingMethodTab();
    });
    $(".MonthlyProductionBonusNav").click(function () {
        LoadMonthlyProductionBonusTab();
    });
    $(".CreditRatingNav").click(function () {
        LoadCreditRatingTab();
    });
    $(".CustomerTypeNav").click(function () {
        LoadCustomerTypeTab();
    });
    $(".AgreementLengthNav").click(function () {
        LoadAgreementLengthTab();
    });
    $(".PassThrusNav").click(function () {
        LoadPassThrusTab();
    });
    $(".InstallationFeeNav").click(function () {
        LoadInstallationFeeTab();
    });
    $(".HoldBackNav").click(function () {
        LoadHoldBackTab();
    });
    $(".AdminFeeNav").click(function () {
        LoadAdminFeeTab();
    });
    $(".TermSheetManagerNav").click(function () {
        LoadTermSheetManagerTab();
    });
});