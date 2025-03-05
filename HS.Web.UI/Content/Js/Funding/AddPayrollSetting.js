var SaveTermSheet = function () {
    var url = domainurl + "/Reports/AddPayrollTermSheet/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        Name: $("#Name").val(),
        IsBase: $("#IsBase").prop("checked")
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadTermSheetTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveSingleItemSettings = function () {
    var url = domainurl + "/Reports/AddPayrollSingleItemSettings/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        SearchKey: $("#SearchKey").val(),
        SearchValue: $("#SearchValue").val(),
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadSingleSettingsTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveBaseMultiple = function () {
    var url = domainurl + "/Reports/AddPayrollBaseMultiple/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        BaseMultiple: $("#BaseMultiple").val(),
        Amount: $("#Amount").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadBaseMultipleTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveCustomerBillingMethod = function () {
    var url = domainurl + "/Reports/AddPayrollCustomerBillingMethod/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        BillingMethod: $("#BillingMethod").val(),
        Point: $("#Point").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadCustomerBillingMethodTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveMonthlyProductionBonus = function () {
    var url = domainurl + "/Reports/AddPayrollMonthlyProductionBonus/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        MonthlyProductionBonusMin: $("#MonthlyProductionBonusMin").val(),
        MonthlyProductionBonusMax: $("#MonthlyProductionBonusMax").val(),
        Point: $("#Point").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadMonthlyProductionBonusTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveCreditRating = function () {
    var url = domainurl + "/Reports/AddPayrollCreditRating/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        MinCredit: $("#MinCredit").val(),
        MaxCredit: $("#MaxCredit").val(),
        Point: $("#Point").val(),
        ACHBonusWaived: $("#ACHBonusWaived").prop("checked")
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadCreditRatingTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveCustomerType = function () {
    var url = domainurl + "/Reports/AddPayrollCustomerType/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        CustomerType: $("#CustomerType").val(),
        Point: $("#Point").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadCustomerTypeTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveAgreementLength = function () {
    var url = domainurl + "/Reports/AddPayrollAgreementLength/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        AgreementLength: $("#AgreementLength").val(),
        Point: $("#Point").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadAgreementLengthTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SavePassThrus = function () {
    var url = domainurl + "/Reports/AddPayrollPassThrus/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        PassThrus: $("#PassThrus").val(),
        IsBase: $("#IsBase").prop("checked"),
        Amount: $("#Amount").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadPassThrusTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveInstallationFee = function () {
    var url = domainurl + "/Reports/AddPayrollInstallationFee/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        InstallationFeeMin: $("#InstallationFeeMin").val(),
        InstallationFeeMax: $("#InstallationFeeMax").val(),
        Amount: $("#Amount").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadInstallationFeeTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveHoldBack = function () {
    var url = domainurl + "/Reports/AddPayrollHoldBack/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        HoldBack: $("#HoldBack").val(),
        Percentage: $("#Percentage").val(),
        Type: $("#Type").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadHoldBackTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveTermSheetManager = function () {
    var url = domainurl + "/Reports/AddPayrollTermSheetManager/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        ManagerId: $("#ManagerId").val(),
        Value: $("#Value").val(),
        Type: $("#Type").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadTermSheetManagerTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var SaveAdminFee = function () {
    var url = domainurl + "/Reports/AddPayrollAdminFee/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        TermSheetId: TermSheetId,
        AdminFee: $("#AdminFee").val(),
        Amount: $("#Amount").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", new function () {
                CloseRightToLeftModal();
                LoadAdminFeeTab();
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    $("#SaveSingleItemSettings").click(function () {
        if (CommonUiValidation()) {
            SaveSingleItemSettings();
        }
    });
    $("#SaveBaseMultiple").click(function () {
        if (CommonUiValidation()) {
            SaveBaseMultiple();
        }
    });
    $("#SaveCustomerBillingMethod").click(function () {
        if (CommonUiValidation()) {
            SaveCustomerBillingMethod();
        }
    });
    $("#SaveMonthlyProductionBonus").click(function () {
        if (CommonUiValidation()) {
            SaveMonthlyProductionBonus();
        }
    });
    $("#SaveCreditRating").click(function () {
        if (CommonUiValidation()) {
            SaveCreditRating();
        }
    });
    $("#SaveCustomerType").click(function () {
        if (CommonUiValidation()) {
            SaveCustomerType();
        }
    });
    $("#SaveAgreementLength").click(function () {
        if (CommonUiValidation()) {
            SaveAgreementLength();
        }
    });
    $("#SavePassThrus").click(function () {
        if (CommonUiValidation()) {
            SavePassThrus();
        }
    });
    $("#SaveInstallationFee").click(function () {
        if (CommonUiValidation()) {
            SaveInstallationFee();
        }
    });
    $("#SaveHoldBack").click(function () {
        if (CommonUiValidation()) {
            SaveHoldBack();
        }
    });
    $("#SaveAdminFee").click(function () {
        if (CommonUiValidation()) {
            SaveAdminFee();
        }
    });
    $("#SaveTermSheet").click(function () {
        if (CommonUiValidation()) {
            SaveTermSheet();
        }
    });
    $("#SaveTermSheetManager").click(function () {
        if (CommonUiValidation()) {
            SaveTermSheetManager();
        }
    });
});