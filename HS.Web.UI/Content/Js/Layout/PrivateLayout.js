var LoaderDom = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var IsInventoryFilterCookie = false;
var IsServiceFilterCookie = false;
var GlobalSearchXHR;
var DisableElement = function (id) {
    $(id).attr("disabled", "disabled");
};
var EnableElement = function (id) {
    $(id).removeAttr("disabled");
};
function isNumeric(number) {
    console.log(number);
    if (typeof (number) != "undefined" && number != null) {
        var Number = number.replace(/[+-]/g, '');
        var TrimNumber = Number.replace(/[()]/g, '');
        var Num = TrimNumber.replace(' ', '');
        console.log(Number);
        var result = false;
        if (!isNaN(Num)) {
            result = true;
        }
        else {
            result = false;
        }
        return result;
    }
    
}

var LoadUrlContents = function (loadurl, seturl, reload) {
    console.log(loadurl);
    console.log(seturl);
    if (typeof (reload) == "undefined" || reload == null) {
        reload = false;
    }
    if ((location.href.toLowerCase().indexOf(seturl.toLowerCase()) > -1) && !reload) {
        return true;
    }
    $(".LoaderWorkingDiv").show();
    $(".page-wrapper-contents").html("");
    setTimeout(function () {
        $(".page-wrapper-contents").load(loadurl);
    }, 50);
    history.pushState(null, null, seturl);
}

var LoadTitleSetting = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Website/Website";
            var seturl = domainurl + "/title-settings";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Website/Website";
        var seturl = domainurl + "/title-settings";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
Number.prototype.toFixed = function (decimalPoint) {
    var value = 0;
    var target = parseFloat(this);
    if (decimalPoint == 0) {
        if (target == 0) {
            return "0";
        }
        value = 1;
    }
    else if (decimalPoint == 1) {
        if (target == 0) {
            return "0.0";
        }
        value = 10;
    }
    else if (decimalPoint == 2) {
        if (target == 0) {
            return "0.00";
        }
        value = 100;
    }
    else if (decimalPoint == 3) {
        if (target == 0) {
            return "0.000";
        }
        value = 1000;
    }
    else if (decimalPoint == 4) {
        if (target == 0) {
            return "0.0000";
        }
        value = 10000;
    }
    else if (decimalPoint == 5) {
        if (target == 0) {
            return "0.00000";
        }
        value = 100000;
    }
    else if (decimalPoint == 6) {
        if (target == 0) {
            return "0.000000";
        }
        value = 1000000;
    }
    else if (decimalPoint == 7) {
        if (target == 0) {
            return "0.0000000";
        }
        value = 10000000;
    }
    else {
        return "" + target;
    }
    target += 0.00000001;
    var CalculateValue = Math.round(target * value) / value;
    if (CalculateValue % 1 == 0 && decimalPoint!=0) {
        CalculateValue = CalculateValue + ".0";
    }
    var result = "" + CalculateValue;
    var FinalResult = result;
    if (result.split('.').length == 2 && decimalPoint > result.split('.')[1].length) {
        for (i = 0; i < (decimalPoint - result.split('.')[1].length) ; i++) {
            FinalResult += "0";
        }
    }
    return FinalResult;
};
Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + parseInt(days));
    return this;
};
Date.prototype.addMonth = function (month) {
    this.setMonth(this.getMonth() + parseInt(month));
    return this;
};
Date.prototype.getWeek = function () {
    var date = new Date(this.getTime());
    date.setHours(0, 0, 0, 0);
    // Thursday in current week decides the year.
    date.setDate(date.getDate() + 3 - (date.getDay() + 6) % 7);
    // January 4 is always in week 1.
    var week1 = new Date(date.getFullYear(), 0, 4);
    // Adjust to Thursday in week 1 and count number of weeks from date to week1.
    return 1 + Math.round(((date.getTime() - week1.getTime()) / 86400000 - 3 + (week1.getDay() + 6) % 7) / 7);
}
Date.prototype.toFormatedDate = function (days) {
    var Month = this.getMonth();
    var Date = this.getDate();
    var Year = this.getYear();

    if (Month < 10)
        Month = '0' + Month;
    if (Date < 10)
        Date = '0' + Date;

    return Month + '/' + Date + '/' + Year;
};
var ClosePopup = function () {
    $.magnificPopup.close();
}
var DateDiff = { 
    inDays: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000));
    },

    inWeeks: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
    },

    inMonths: function (d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();

        return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
    },

    inYears: function (d1, d2) {
        return d2.getFullYear() - d1.getFullYear();
    }
}
var getDateOfISOWeek = function (w, y) {
    var simple = new Date(y, 0, 1 + (w - 1) * 7);
    var dow = simple.getDay();
    var ISOweekStart = simple;
    if (dow <= 4)
        ISOweekStart.setDate(simple.getDate() - simple.getDay() + 1);
    else
        ISOweekStart.setDate(simple.getDate() + 8 - simple.getDay());

    if (FirstDayOfWeek == 'Saturday') {
        return ISOweekStart.addDays(-2);
    }
    else if (FirstDayOfWeek == 'Sunday') {
        return ISOweekStart.addDays(-1);
    }

    return ISOweekStart;
}
var LoadDashboard = function (reload) {
    var loadurl = domainurl + "/App/DashboardPartial";
    var seturl = domainurl + "/Dashboard";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadScheduleInformationCalendar = function (reload) {
    var loadurl = domainurl + "/Schedule/SchedulePartial";
    var seturl = domainurl + "/ScheduleInfo";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadProductCategory = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Customer/ProductCategoryPartial";
            var seturl = domainurl + "/ProductCategory";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Customer/ProductCategoryPartial";
        var seturl = domainurl + "/ProductCategory";
        LoadUrlContents(loadurl, seturl, reload);
    }
    $("#CustomerSubMenu").slideDown();
}
var LoadProductClass = function (reload) {
    var loadurl = domainurl + "/Inventory/ProductClassPartial";
    var seturl = domainurl + "/ProductClassInfo";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadLeadVerificatonAndSetupDetail = function (reload) {
    var loadurl = domainurl + "/Leads/LeadVerifyAndSetupDetailInformationPartial";
    var seturl = domainurl + "/LeadVerifyAndSetupDetail";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadAdministrator = function (reload) {
    var loadurl = domainurl + "/App/AdmintrationPartial";
    var seturl = domainurl + "/Admintration";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCustomer = function (reload) {
    var loadurl = domainurl + "/Customer/CustomerPartialLite";
    var seturl = domainurl + "/Customer";
    LoadUrlContents(loadurl, seturl, reload);
    $("#CustomerSubMenu").slideDown();
}
var LoadTicket = function (reload) {
    var loadurl = domainurl + "/Ticket/TicketDashboard";
    var seturl = domainurl + "/ticket";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCustomerListLite = function (reload) {
	var loadurl = domainurl + "/Customer/CustomerListLite";
	var seturl = domainurl + "/Customer";
    LoadUrlContents(loadurl, seturl, reload);
    $("#CustomerSubMenu").slideDown();
}
var OpenEmployeeReviewByReviewId = function (ReviewId) {
    var loadurl = domainurl + "/Survey/EmployeeSurvey/?ReviewId=" + ReviewId;
    var seturl = domainurl + "/EmployeeSurvey/" + ReviewId;
    var reload = true;
    LoadUrlContents(loadurl, seturl, reload);
}
var OpenCustomSurveyBySurveyId = function (SurveyId) {
    var loadurl = domainurl + "/Survey/RunSurvey/?SurveyUserId=" + SurveyId;
    var seturl = domainurl + "/RunSurvey/" + SurveyId;
    var reload = true;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadLeadVerificationInfo = function (id, reload) {
    var loadurl = domainurl + "/Leads/LeadVerificationPartial/?id=" + id;
    var seturl = domainurl + "/LeadsVerificationDetail/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadLeadInfov2 = function (id, reload) {
    var loadurl = domainurl + "/Leads/LeadInfoPartial/?id=" + id;
    var seturl = domainurl + "/LeadsInfo/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadExpense = function (reload) {
    var loadurl = domainurl + "/Expense/ExpensePartial";
    var seturl = domainurl + "/Expense";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadConfig = function (reload) {
    var loadurl = domainurl + "/Configuration/ConfigurationPartial";
    var seturl = domainurl + "/Configuration";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadTechSetting = function (reload) {
    var loadurl = domainurl + "/TechScheduleSetting/TechScheduleSettingPartial";
    var seturl = domainurl + "/TechScheduleSettingInfo";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCustomerDetail = function (id, reload) {
    /*var loadurl = "/Customer/CustomerDetails/?id=" + id;
    var seturl = "/Customer/Customerdetail/?id=" + id;*/
    var loadurl = domainurl + "/Customer/DetailTab/?id=" + id;
    var seturl = domainurl + "/Customer/Customerdetail/?id=" + id;

    LoadUrlContents(loadurl, seturl, reload);
    $("#CustomerSubMenu").slideDown();
}

var LoadContactDetail = function (id, reload) {
    /*var loadurl = "/Customer/CustomerDetails/?id=" + id;
    var seturl = "/Customer/Customerdetail/?id=" + id;*/

    var loadurl = domainurl + "/Contact/ContactDetailTab/?id=" + id;
    var seturl = domainurl + "/Contact/ContactDetail/?id=" + id;

    LoadUrlContents(loadurl, seturl, reload);
}
var LoadActivityDetail = function (id, reload) {
    /*var loadurl = "/Customer/CustomerDetails/?id=" + id;
    var seturl = "/Customer/Customerdetail/?id=" + id;*/
    var loadurl = domainurl + "/Activity/ActivityDetails/?id=" + id;
    var seturl = domainurl + "/Activity/ActivityDetail/?id=" + id;

    LoadUrlContents(loadurl, seturl, reload);
}
var LoadOpportunityDetail = function (id, reload) {
    /*var loadurl = "/Customer/CustomerDetails/?id=" + id;
    var seturl = "/Customer/Customerdetail/?id=" + id;*/

    var loadurl = domainurl + "/Opportunity/OpportunityDetailTab/?id=" + id;
    var seturl = domainurl + "/Opportunity/OpportunityDetail/?id=" + id;

    LoadUrlContents(loadurl, seturl, reload);
}

var LoadCustomerDraftDetail = function (reload) {
    /*var loadurl = "/Customer/CustomerDetails/?id=" + id;
    var seturl = "/Customer/Customerdetail/?id=" + id;*/

    var loadurl = domainurl + "/App/CustomerDashboard";
    var seturl = domainurl + "/App/CustomerDashboard";

    LoadUrlContents(loadurl, seturl, reload);
}
var LoadInventory = function (reload) {
    
    var loadurl = domainurl + "/App/InventoryPartial";
    var seturl = domainurl + "/Inventory";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadActivity = function (reload) {
    var loadurl = domainurl + "/Activity/Activities";
    var seturl = domainurl + "/Activities";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadContacts = function (reload) {
    var loadurl = domainurl + "/Contact/Contacts";
    var seturl = domainurl + "/Contacts";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadRecruit = function (reload) {
    var loadurl = domainurl + "/Recruit/RecruitTab";
    var seturl = domainurl + "/Recruitment";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadEmailTemplates = function (reload) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Email/Templates";
            var seturl = domainurl + "/Templates";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Email/Templates";
        var seturl = domainurl + "/Templates";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadRestrictedZipCode = function (reload) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/CityTax/RestrictedZipCode";
            var seturl = domainurl + "/RestrictedZipCode";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/CityTax/RestrictedZipCode";
        var seturl = domainurl + "/RestrictedZipCode";
        LoadUrlContents(loadurl, seturl, reload);
    }
}

var LoadCreditGrade = function (reload) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Customer/CreditGrades";
            var seturl = domainurl + "/CreditGrades";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Customer/CreditGrades";
        var seturl = domainurl + "/CreditGrades";
        LoadUrlContents(loadurl, seturl, reload);
    }
    $("#CustomerSubMenu").slideDown();
}
var LoadAggrementQstn = function (reload,item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/SmartLeads/AggrementQstn";
            var seturl = domainurl + "/AggrementQstn";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/SmartLeads/AggrementQstn";
        var seturl = domainurl + "/AggrementQstn";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadLocalizeData = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Setup/LocalizeResource";
            var seturl = domainurl + "/Localization";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Setup/LocalizeResource";
        var seturl = domainurl + "/Localization";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadGlobalSearchByKey = function (reload, key) {
    var loadurl = domainurl + "/App/GlobalSearchResult/?key=" + key;
    var seturl = domainurl + "/Search/?key=" + key;
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadAnnouncement = function (reload, item, e) {
    var loadurl = domainurl + "/Customer/CustomerAnnouncement/";
    var seturl = domainurl + "/CustomerAnnouncement";
    LoadUrlContents(loadurl, seturl, reload);
    $("#CustomerSubMenu").slideDown();

}
var LoadAdvancedSearchByKey = function (reload, key) {
    var loadurl = domainurl + "/App/GlobalSearchResult/?key=" + key;
    var seturl = domainurl + "/Search/?key=" + key;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadGridSettings = function (reload) {
    var loadurl = domainurl + "/Setup/GridSettings";
    var seturl = domainurl + "/GridSettings/";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadTimeClock = function (reload) {
    var loadurl = domainurl + "/TimeClockPto/TimeClockHome";
    var seturl = domainurl + "/TimeClock/";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadTimeClockList = function (reload) {
    //var loadurl = domainurl + "/TimeClockPto/TimeClockHome";
    //var seturl = domainurl + "/TimeClock/";
    //LoadUrlContents(loadurl, seturl, reload);
    window.open('/TimeClock', '_blank');
}
var LoadEquipmentDetail = function (reload, savecookies, id) {
    if (savecookies) {
        IsInventoryFilterCookie = true;
        var FilterInventoryData = JSON.stringify({
            SearchBox: $(".eqpInvetory").val(),
            ActiveStatus: $("#ActiveStatus").val(),
            EquipmentCategory: $("#EquipmentCategory").val(),
            StockStatus: $("#StockStatus").val(),
            SortingVal: $("#SortingVal").val()
        });
        var date = new Date();
        date.setTime(date.getTime() + (600 * 1000));
        $.cookie("FilterInventoryData", FilterInventoryData, { expires: date });
    }
    var loadurl = domainurl + "/Inventory/EquipmentDetailPartial/" + id;
    var seturl = domainurl + "/Inventory/EquipmentDetail/" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadServiceDetail = function (reload, savecookies, id) {
    if (savecookies) {
        IsServiceFilterCookie = true;
        var FilterServiceData = JSON.stringify({
            SearchBox: $(".eqpService").val(),
            ActiveStatus: $("#ActiveStatus").val(),
            EquipmentCategory: $("#EquipmentCategory").val(),
            StockStatus: $("#StockStatus").val(),
            SortingVal: $("#SortingVal").val()
        });
        var date = new Date();
        date.setTime(date.getTime() + (600 * 1000));
        $.cookie("FilterServiceData", FilterServiceData, { expires: date });
    }
    var loadurl = domainurl + "/Inventory/EquipmentDetailPartial/" + id;
    var seturl = domainurl + "/Inventory/ServiceDetail/" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadEmployee = function (reload) {
    var loadurl = domainurl + "/App/EmployeePartial";
    var seturl = domainurl + "/Employee";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadTimeAttendance = function (reload) {
    var loadurl = domainurl + "/App/TimeAttendancePartial";
    var seturl = domainurl + "/TimeAttendance";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadSchedule = function (reload) {
    var loadurl = domainurl + "/App/SchedulePartial";
    var seturl = domainurl + "/Schedule";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadAnalytics = function (reload) {
    var loadurl = domainurl + "/App/AnalyticsPartial";
    var seturl = domainurl + "/Analytics";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadManufacturers = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/App/ManufacturersPartial";
            var seturl = domainurl + "/Manufacturers";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/App/ManufacturersPartial";
        var seturl = domainurl + "/Manufacturers";
        LoadUrlContents(loadurl, seturl, reload);
    }

    /*if (typeof (reload) == "undefined" || reload == null) {
        reload = false;
    }
    if ((location.href.toLowerCase().indexOf(seturl.toLowerCase()) > -1) && !reload) {
        return true;
    }
    $(".page-wrapper-contents").html("");
    $(".page-wrapper-contents").load(loadurl);
    history.pushState(null, null, seturl);*/


}

var LoadMarchents = function (reload) {
    var loadurl = domainurl + "/App/MarchentsPartial";
    var seturl = domainurl + "/Marchents";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadVendor = function (reload) {
    var loadurl = domainurl + "/App/VendorPartial";
    var seturl = domainurl + "/Vendor";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadExpence = function (reload) {
    var loadurl = domainurl + "/App/ExpencePartial";
    var seturl = domainurl + "/Expence";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadPayroll = function (reload) {
    var loadurl = domainurl + "/App/PayrollPartial";
    var seturl = domainurl + "/Payroll";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadBill = function (reload) {
    var loadurl = domainurl + "/App/BillPartial";
    var seturl = domainurl + "/Bill";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadVendorCredit = function (reload) {
    var loadurl = domainurl + "/App/VendorCreditPartial";
    var seturl = domainurl + "/VendorCredit";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadEmployeeSchedule = function (reload) {
    var loadurl = domainurl + "/App/EmployeeSchedulePartial";
    var seturl = domainurl + "/EmployeeSchedule";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadLeads = function (reload) {
    var loadurl = domainurl + "/Leads/LeadsLitePartial";
    var seturl = domainurl + "/Lead";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadVerifyLeads = function (id, reload) {
    var loadurl = domainurl + "/Leads/VerifyLeadPartial/?id=" + id;
    var seturl = domainurl + "/LeadVerifyInfo/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadFormGeneration = function (id, reload) {
    var loadurl = domainurl + "/Leads/LeadFormGenerationPartial/?LeadId=" + id;
    var seturl = domainurl + "/CreateLead/" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadLeadAgreement = function (code, reload) {
    var loadurl = domainurl + "/Public/LeadsAgreementDocument/?code=" + code;
    //var seturl = domainurl + "/Leads-Agreement/?code=" + code;
    var seturl = domainurl + "/Public/LeadsAgreementDocument/?code=" + code;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadLeadSetup = function (id, reload) {
    var loadurl = domainurl + "/Leads/LeadSetupPartial/?id=" + id;
    var seturl = domainurl + "/LeadSetup/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadSmartLeadSetup = function (id, reload) {
    var loadurl = domainurl + "/SmartLeads/SmartLeadSetupPartial/?id=" + id;
    var seturl = domainurl + "/SmartLeadSetup/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadDocumentCenter = function (id, reload) {
    var loadurl = domainurl + "/Leads/DocumentCenterPartial/?id=" + id;
    var seturl = domainurl + "/DocumentCenterInfo/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadLeadsDetail = function (id, reload) {
    var loadurl = domainurl + "/Leads/LeadsDetails/?id=" + id;
    var seturl = domainurl + "/Lead/Leadsdetail/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadReport = function (reload) {
    var loadurl = domainurl + "/Reports/ReportsPartial";
    var seturl = domainurl + "/Reports";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadReportCustomer = function (reload) {
    var loadurl = domainurl + "/App/ReportCustomerPartial";
    var seturl = domainurl + "/ReportCustomer";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadReportSales = function (reload) {
    var loadurl = domainurl + "/App/ReportSalesPartial";
    var seturl = domainurl + "/ReportSales";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadReportEmployee = function (reload) {
    var loadurl = domainurl + "/App/ReportEmployeePartial";
    var seturl = domainurl + "/ReportEmployee";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadReportSchedule = function (reload) {
    var loadurl = domainurl + "/App/ReportSchedulePartial";
    var seturl = domainurl + "/ReportSchedule";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCheck = function (reload) {
    var loadurl = domainurl + "/App/CheckPartial";
    var seturl = domainurl + "/Check";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadTagManager = function (reload) {
    var loadurl = domainurl + "/App/TagManagementPartial";
    var seturl = domainurl + "/ManagerTag";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadBuildVersion = function (reload) {
    var loadurl = domainurl + "/App/BuildVersionPartial";
    var seturl = domainurl + "/BuildVersion";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadServiceProduct = function (reload) {
    var loadurl = domainurl + "/Customer/ServiceProductPartial";
    var seturl = domainurl + "/ServiceProduct";
    LoadUrlContents(loadurl, seturl, reload);
    $("#CustomerSubMenu").slideDown();
}
var LoadBanking = function (reload) {
    var loadurl = domainurl + "/App/BankingPartial";
    var seturl = domainurl + "/Banking";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadThirdPartyApi = function (reload) {
    var loadurl = domainurl + "/App/ThirdPartyApiPartial";
    var seturl = domainurl + "/ThirdPartyApi";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCompany = function (reload) {
    var loadurl = domainurl + "/App/CompanyPartial";
    var seturl = domainurl + "/Company";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadSettings = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Setup/SetupDetails";
            var seturl = domainurl + "/SetupDetailsInfo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Setup/SetupDetails";
        var seturl = domainurl + "/SetupDetailsInfo";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
//var LoadCalendarSettings = function (reload, item, e) {
//    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
//        e.preventDefault();
//        if (cntrlIsPressed) {
//            var href = $(item).attr('href');
//            window.open(href, '_blank');
//        } else {
//            var loadurl = domainurl + "/Calendar/CalendarSettings";
//            var seturl = domainurl + "/calendarsetup";
//            LoadUrlContents(loadurl, seturl, reload);
//        }
//    } else {
//        var loadurl = domainurl + "/Calendar/CalendarSettings";
//        var seturl = domainurl + "/calendarsetup";
//        LoadUrlContents(loadurl, seturl, reload);
//    }
//}
var LoadUISettings = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Setup/SmartUiSetup";
            var seturl = domainurl + "/uisetupdetails";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Setup/SmartUiSetup";
        var seturl = domainurl + "/uisetupdetails";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadDropDownSettings = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Setup/LookupSetup";
            var seturl = domainurl + "/SetupDropdowns";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Setup/LookupSetup";
        var seturl = domainurl + "/SetupDropdowns";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
//var LoadBuildVersion = function (reload, item, e) {
//    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
//        e.preventDefault();
//        if (cntrlIsPressed) {
//            var href = $(item).attr('href');
//            window.open(href, '_blank');
//        } else {
//            var loadurl = domainurl + "/Setup/BuildVersion";
//            var seturl = domainurl + "/BuildVersionDropdowns";
//            LoadUrlContents(loadurl, seturl, reload);
//        }
//    } else {
//        var loadurl = domainurl + "/Setup/BuildVersion";
//        var seturl = domainurl + "/BuildVersion";
//        LoadUrlContents(loadurl, seturl, reload);
//    }
//}

var initHeight = function () {
    var contentHeight = window.innerHeight - ($(".navbar.navbar-inverse.navbar-fixed-top").height() + 5);
    if ($(".announcement-div").is(":visible")) {
        contentHeight = window.innerHeight - ($(".navbar.navbar-inverse.navbar-fixed-top").height() + $(".announcement-div").height() + 10);
    }
    if (Device.MobileGadget()) {
        contentHeight = window.innerHeight - 54;
    }
    $(".page-wrapper-contents").height(contentHeight);
    $(".page-wrapper-contents").css("overflow-y", "auto");
    $(".page-wrapper-contents").css("overflow-x", "hidden");
    if (window.innerWidth <= 768) {
        $(".sidebar_height").height(window.innerHeight);
        //$(".sidebar_height").css("height", "auto");
    }
    else {
        $(".sidebar_height").height(window.innerHeight - $(".navbar-fixed-top").height());
    }

}

var LoadUserMgmt = function (item, e) {
    var reload = true;
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        if (typeof (e) != "undefined") {
            e.preventDefault();
        }
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            if (location.href.toLowerCase().indexOf("menu=recruitment") > -1) {
                LoadRecruit();
            } else {
                var loadurl = domainurl + "/UserMgmt/UserMgmtPartial";
                var seturl = domainurl + "/UserMgmt";

                LoadUrlContents(loadurl, seturl, reload);
            }
        }
    } else {
        if (location.href.toLowerCase().indexOf("menu=recruitment") > -1) {
            LoadRecruit();
        } else {
            var loadurl = domainurl + "/UserMgmt/UserMgmtPartial";
            var seturl = domainurl + "/UserMgmt";

            LoadUrlContents(loadurl, seturl, reload);
        }
    }

}
var LoadUserGroup = function (item, e) {
    var reload = true;
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/UserMgmt/UserGroup";
            var seturl = domainurl + "/UserGroup";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/UserMgmt/UserGroup";
        var seturl = domainurl + "/UserGroup";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadUserTeam = function (item, e) {
    var reload = true;
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/UserMgmt/UserTeam";
            var seturl = domainurl + "/UserTeam";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/UserMgmt/UserTeam";
        var seturl = domainurl + "/UserTeam";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadSupplier = function (reload) {
    var loadurl = domainurl + "/Expense/ExpensePartial";
    var seturl = domainurl + "/Expense";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadSupplierDetails = function (id, reload) {
    var loadurl = domainurl + "/Supplier/SupplierDetails/?id=" + id;
    var seturl = domainurl + "/Supplier/SupplierDetail/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadFees = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Fees/FeesPartial";
            var seturl = domainurl + "/FeesInfo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Fees/FeesPartial";
        var seturl = domainurl + "/FeesInfo";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadUserProfile = function (reload) {
    var loadurl = domainurl + "/UserMgmt/UserProfilePartial";
    var seturl = domainurl + "/UserProfile";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadSalesMatrix = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Matrix/MatrixPartial";
            var seturl = domainurl + "/Matrix";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Matrix/MatrixPartial";
        var seturl = domainurl + "/Matrix";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadAdjusmentOverride = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Adjusment/AdjusmentPartial";
            var seturl = domainurl + "/Adjusment";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Adjusment/AdjusmentPartial";
        var seturl = domainurl + "/Adjusment";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadInstallMatrix = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = "/Matrix/InstallMatrix";
            var seturl = "/installmatrix";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Matrix/InstallMatrix";
        var seturl = domainurl + "/installmatrix";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadVehicleMgmtTab = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/VehicleManagement/ShowVehicleList";
            var seturl = domainurl + "/VehicleManagement";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/VehicleManagement/ShowVehicleList";
        var seturl = "/VehicleManagement";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadTimeClock = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/TimeClockPto/TimeClockHome";
            var seturl = domainurl + "/TimeClock";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/TimeClockPto/TimeClockHome";
        var seturl = "/TimeClock#TimeClock";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadServiceInventory = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Inventory/ServiceListPartial";
            var seturl = domainurl + "/ServiceInventory";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Inventory/ServiceListPartial";
        var seturl = domainurl + "/ServiceInventory";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadCustomerSystemNo = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Customer/CustomerSystemNoPartial";
            var seturl = domainurl + "/InstallCustomerSystemNo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Customer/CustomerSystemNoPartial";
        var seturl = domainurl + "/InstallCustomerSystemNo";
        LoadUrlContents(loadurl, seturl, reload);
    }
    $("#CustomerSubMenu").slideDown();
}

var LoadAccountHolder = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/AccountHolder/AccountHolderPartial";
            var seturl = domainurl + "/AccountHolderInfo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/AccountHolder/AccountHolderPartial";
        var seturl = domainurl + "/AccountHolderInfo";
        LoadUrlContents(loadurl, seturl, reload);
    }
}

var LoadEditCompany = function (reload, item, e) {
      //if (typeof (item) != "undefined" && typeof (item) != "undefined") {
    //    e.preventDefault();
    //    if (cntrlIsPressed) {
    //        var href = $(item).attr('href');
    //        window.open(href, '_blank');
    //    } else {
    //        var loadurl = domainurl + "/Company/CompanyrPartial";
    //        var seturl = domainurl + "/CompanyInfo";
    //        LoadUrlContents(loadurl, seturl, reload);
    //    }
    //} else {

    //}
    var loadurl = domainurl + "/Company/CompanyrPartial";
    var seturl = domainurl + "/CompanyInfo";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCustomerRoute = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Customer/CustomerRoutePartial";
            var seturl = domainurl + "/customer-route";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Customer/CustomerRoutePartial";
        var seturl = domainurl + "/customer-route";
        LoadUrlContents(loadurl, seturl, reload);
    }
    $("#CustomerSubMenu").slideDown();
}
var LoadEditResturant = function (reload, item, e) {
    //if (typeof (item) != "undefined" && typeof (item) != "undefined") {
    //    e.preventDefault();
    //    if (cntrlIsPressed) {
    //        var href = $(item).attr('href');
    //        window.open(href, '_blank');
    //    } else {
    //        var loadurl = domainurl + "/Company/CompanyrPartial";
    //        var seturl = domainurl + "/CompanyInfo";
    //        LoadUrlContents(loadurl, seturl, reload);
    //    }
    //} else {

    //}
    var loadurl = domainurl + "/CreateResturant/Index";
    var seturl = domainurl + "/CreateResturant";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadCompanyBranch = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/CompanyBranch/CompanyBranchPartial";
            var seturl = domainurl + "/CompanyBranchInfo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/CompanyBranch/CompanyBranchPartial";
        var seturl = domainurl + "/CompanyBranchInfo";
        LoadUrlContents(loadurl, seturl, reload);
    }
}

var LoadCityTax = function (reload) {
    var loadurl = domainurl + "/CityTax/CityTaxPartial";
    var seturl = domainurl + "/CityTaxInfo";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadCredentialSetting = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/CredentialSetting/CredentialSettingPartial";
            var seturl = domainurl + "/CredentialSettingInfo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/CredentialSetting/CredentialSettingPartial";
        var seturl = domainurl + "/CredentialSettingInfo";
        LoadUrlContents(loadurl, seturl, reload);
    }
}

var LoadActivationFee = function (reload) {
    var loadurl = domainurl + "/ActivationFee/ActivationFeePartial";
    var seturl = domainurl + "/ActivationFeeInfo";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadServiceFee = function (reload) {
    var loadurl = domainurl + "/ServiceFee/ServiceFeePartial";
    var seturl = domainurl + "/ServiceFeeInfo";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadCustomerSystemNoPrefix = function (reload) {
    var loadurl = domainurl + "/Customer/CustomerSystemNoPrefixPartial";
    var seturl = domainurl + "/InstallCustomerSystemPrefix";
    LoadUrlContents(loadurl, seturl, reload);
    $("#CustomerSubMenu").slideDown();
}

var LoadPanelType = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Panel/CustomerPanelPartial";
            var seturl = domainurl + "/PanelTypeInfo";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Panel/CustomerPanelPartial";
        var seturl = domainurl + "/PanelTypeInfo";
        LoadUrlContents(loadurl, seturl, reload);
    }
}

var LoadPackageSetup = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Leads/PackageSettingsPartial";
            var seturl = domainurl + "/PackageSettings";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Leads/PackageSettingsPartial";
        var seturl = domainurl + "/PackageSettings";
        LoadUrlContents(loadurl, seturl, reload);
    }
}

var LoadSurveySetup = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Survey/SurveySettingsPartial";
            var seturl = domainurl + "/SurveySettings";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Survey/SurveySettingsPartial";
        var seturl = domainurl + "/SurveySettings";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadTicketEmailSetup = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Ticket/TicketEmailSetupPartial";
            var seturl = domainurl + "/TicketEmailSettings";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Ticket/TicketEmailSetupPartial";
        var seturl = domainurl + "/TicketEmailSettings";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadLeadImportFromCMS = function (reload) {
    var loadurl = domainurl + "/Leads/LeadImportFromCMS";
    var seturl = domainurl + "/LeadImportFromCMS";
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadSmartPackageSetup = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/SmartPackageSetup/SmartPackageSettingsPartial";
            var seturl = domainurl + "/smartpackagesettings";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/SmartPackageSetup/SmartPackageSettingsPartial";
        var seturl = domainurl + "/smartpackagesettings";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadAreazipcode = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/ServiceArea/ZipcodeList";
            var seturl = domainurl + "/ServiceAreaZipCode";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/ServiceArea/ZipcodeList";
        var seturl = domainurl + "/ServiceAreaZipCode";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
var LoadFundingCompany = function (reload, item, e) {
    if (typeof (item) != "undefined" && typeof (item) != "undefined") {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(item).attr('href');
            window.open(href, '_blank');
        } else {
            var loadurl = domainurl + "/Funding/FundingCompanyPartial";
            var seturl = domainurl + "/FundingCompany";
            LoadUrlContents(loadurl, seturl, reload);
        }
    } else {
        var loadurl = domainurl + "/Funding/FundingCompanyPartial";
        var seturl = domainurl + "/FundingCompany";
        LoadUrlContents(loadurl, seturl, reload);
    }
}
//var LoadLeads = function (reload) {
//    var loadurl = "/Leads/PackageSettingsPartial";
//    var seturl = "/PackageSettings";
//    LoadUrlContents(loadurl, seturl, reload);
//}
var LoadSales = function (reload) {
    var loadurl = domainurl + "/Sales/AllSalesPartial";
    var seturl = domainurl + "/Sales";
    LoadUrlContents(loadurl, seturl, reload);
}

var LoadUserInfo = function (id, reload) {
    var loadurl = domainurl + "/UserMgmt/UserInformation/?Id=" + id;
    var seturl = domainurl + "/UserInformation/?Id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadRecruitUserInfo = function (id, reload) {
    var loadurl = domainurl + "/UserMgmt/UserInformationRecruit/?id=" + id;
    var seturl = domainurl + "/UserInformationRecruit/?id=" + id;
    LoadUrlContents(loadurl, seturl, reload);
}
var LoadAllNotifications = function () {
    var loadurl = domainurl + "/Notification/AllNotifications";
    var seturl = domainurl + "/Notifications";
    LoadUrlContents(loadurl, seturl, true);
    setTimeout(function () {
        $(".navbar-right .dropdown").removeClass('open');
        $(".flyout-overlay").hide();
    }, 80);
}
var DetailsFromSuggestionList = function (Type, Id, reload) {
    console.log("DetailView");
    if (Type == "Customer") {
        window.location.href = domainurl + "/Customer/CustomerDetail/?id=" + Id;
    }
    if (Type == "Lead") {
        window.location.href = domainurl + "/Lead/LeadsDetail/?id=" + Id;
    }
    if (Type == "Invoice") {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?id=" + Id);
    }
    if (Type == "Estimate") {
        OpenTopToBottomModal(domainurl + "/Estimate/AddEstimate/?id=" + Id);
    }
    if (Type == "Ticket") {
        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?id=" + Id);
    }
    $("#CustomerSubMenu").slideDown();
}
var SetUserCompany = function () {
    //$.cookie("__Customer", "", { expires: -1, path: '/' });
    UserCurrentCompanyId = $("#UserCompanyListLayout").val();
    var url = domainurl + "/UserMgmt/ChangeCurrentUserCompany/";
    var param = JSON.stringify({
        companyId: UserCurrentCompanyId,
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
            console.log("sdfdf");
            if (data.result == true) {
                //if (location.href.indexOf('?') == -1 && location.href.indexOf('#') == -1) {
                //    location.href.reload();
                //} else {
                    
                //}
                location.href = domainurl + '/Dashboard';
            } else {
                OpenErrorMessageNew("", data.message, function () { });
                //parent.ClosePopupGiveError();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var GlobalSearchSuggestionTemplate =
              '<div class="tt-suggestion tt-selectable" data-select="{7}" data-type="{5}" data-id="{0}" data-description="{6}" id="{8}">'
                 + "<span class='EquipmentImage'>{7}</span>"
                 + "<p class='tt-sug-text'>"
                     + "<em class='tt-sug-type'>{5}</em><div>{1}</div>"
                     + "<em class='tt-eq-price'>{2}</em>"
                 + "</p> "
              + "</div>";

var PropertySuggestionclickbind = function (item) {
    $('.GlobalEstInvCustSearchDiv .tt-suggestion').click(function () {
        var clickitem = this;
        $('.GlobalEstInvCustSearchDiv .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var Type = $(clickitem).attr('data-type');
        console.log(Type);
        var Id = $(clickitem).attr('id');
        console.log(Id);
        //GlobalSearchButtonClick();
        DetailsFromSuggestionList(Type, Id);
    });
    $('.GlobalEstInvCustSearchDiv .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.GlobalEstInvCustSearchDiv .tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var GlobalSearchKeyUp = function (item, event) {
    var callingTimeVal = $(item).val();
    setTimeout(function () {
        DOGlobalSearchKeyUp(callingTimeVal, item, event);
    }, 400);
}

var DOGlobalSearchKeyUp = function (callingTimeVal, item, event) {

    var value = $(".GlobalSearchInp").val();
    if (callingTimeVal == value)
    {
        if (typeof (GlobalSearchXHR) != "undefined" && typeof (GlobalSearchXHR.abort) != "undefined") {
            GlobalSearchXHR.abort();
        }
        /*console.log(event.keyCode);*/
        if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
            return false;
        GlobalSearchXHR = $.ajax({
            url: domainurl + "/App/GetGlobalSearchByKey",
            data: {
                key: $(item).val(),
                type: $("#drop_tag").val()
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var resultparse = JSON.parse(data.result);

                if (resultparse.length > 0) {
                    var searchresultstring = "<div class='NewGlobalSuggestion'>";
                    for (var i = 0; i < resultparse.length; i++) {
                        var Name = resultparse[i].Name;
                        /*if (resultparse[i].BusinessName != "") {
                            Name = resultparse[i].BusinessName;
                            if (resultparse[i].Name != "") {
                                Name += " (" + resultparse[i].Name + ")";
                            }
                        } else {
                            Name = resultparse[i].Name;
                        }*/
                        var TypeVal = resultparse[i].Type;
                        var EquipmentDescriptionVal = resultparse[i].EquipmentDescription;
                        if (TypeVal != null && TypeVal != '') {
                            TypeVal = TypeVal.replaceAll('"', '\'\'');
                        }
                        if (EquipmentDescriptionVal != null && EquipmentDescriptionVal != '') {
                            EquipmentDescriptionVal = EquipmentDescriptionVal.replaceAll('"', '\'\'');
                        }
                        var IdVal = resultparse[i].Id;
                        //if (IdVal != null && IdVal != '') {
                        //    IdVal = IdVal.replaceAll('"', '\'\'');
                        //}
                        searchresultstring = searchresultstring + String.format(GlobalSearchSuggestionTemplate,
                            /*0*/ resultparse[i].EquipmentId,
                            /*1*/ resultparse[i].PhoneNumber,
                            /*2*/ resultparse[i].EmailAddress,
                            /*3*/ resultparse[i].Reorderpoint,
                            /*4*/ resultparse[i].QuantityAvailable,
                            /*5*/ TypeVal,
                            /*6*/ EquipmentDescriptionVal,
                            /*7*/ Name.replaceAll('"', '\'\''),
                            /*8*/ IdVal);
                    }
                    searchresultstring += "</div>";
                    var ttdom = $($(item).parent()).find('.tt-menu');
                    var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                    $(ttdomComplete).html(searchresultstring);
                    $(ttdom).show();

                    PropertySuggestionclickbind(item);
                    if (resultparse.length > 4) {
                        $(".NewGlobalSuggestion").css('max-height', '285');
                        $(".NewGlobalSuggestion").css('position', 'relative');
                        $(".NewGlobalSuggestion").perfectScrollbar()
                    }
                }
                if (resultparse.length == 0)
                    $('.tt-menu').hide();
            }
        });
    }
}
var GlobalSearchKeyDown = function (item, event) {

    if (event.keyCode == 13) {
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
        if ($('.tt-suggestion').length > 0) {
            if ($('.tt-suggestion.active').length == 0) {
                $($('.tt-suggestion').get(0)).addClass('active');
                $(item).val($($('.tt-suggestion').get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $('.tt-suggestion');
                var activesuggestion = $('.tt-suggestion.active');
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $('.tt-suggestion').removeClass('active');
                    var possibleactive = $('.tt-suggestion').get(indexactive + 1);
                    $($('.tt-suggestion').get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
        }
        event.preventDefault();
    }
    if (event.keyCode == 38) {
        if ($('.tt-suggestion').length > 0 && $('.tt-suggestion.active').length > 0) {
            var suggestionlist = $('.tt-suggestion');
            var activesuggestion = $('.tt-suggestion.active');
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $('.tt-suggestion').removeClass('active');
                var possibleactive = $('.tt-suggestion').get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
        }
        event.preventDefault();
    }
}

var CheckValue = function (component, text) {
    if (component.value == text)
        component.value = "";
    else if (component.value == "")
        component.value = text;
}
var GlobalSearchButtonClick = function () {
    //var GlobalSearchtext = $(".GlobalSearchInp").val().split(' ')[0];
    console.log("test");
    var GlobalSearchtext = encodeURIComponent($(".GlobalSearchInp").val());
    if (GlobalSearchtext != "") {
        LoadGlobalSearchByKey(true, GlobalSearchtext);
    }
}

var initMenuClicks = function () {
    $("#LoadDashBoard").click(function () {
        LoadDashboard(false);
    });
    $("#LoadScheduleInformationCalendar").click(function () {
        LoadScheduleInformationCalendar(false);
    });
    $("#LoadAdmintration").click(function () {
        LoadAdministrator(false);
    });
    $("#LoadTickets").click(function () {
        LoadTicket(true);
    }); 
    $("#LoadCustomer").click(function () {
        LoadCustomer(true);        
    });
    $("#LoadExpense").click(function () {
        LoadExpense(false);
    });
    $("#LoadInventory").click(function () {
        LoadInventory(false);
    });
    $("#LoadRecruit").click(function () {
        LoadRecruit(false);
    });
    $("#LoadSchedule").click(function () {
        LoadSchedule(false);
    });
    $("#LoadSettings").click(function () {
        LoadSettings(false);
    });
    //$("#LoadCalendarSettings").click(function () {
    //    LoadCalendarSettings(false);
    //});
    $("#LoadAggrementQstn").click(function () {
        LoadAggrementQstn(false);
    });
    $("#LoadAnalytics").click(function () {
        LoadAnalytics(false);
    });
    $("#LoadManufacturers").click(function () {
        LoadManufacturers(false);
    });
    $("#LoadAreazipcode").click(function () {
        LoadAreazipcode(false);
    });
    $("#LoadMarchents").click(function () {
        LoadMarchents(false);
    });
    $("#LoadVendor").click(function () {
        LoadVendor(false);
    });
    $("#LoadLeads").click(function () {
        LoadLeads(true);
    });
    $("#LoadSales").click(function () {
        LoadSales(false);
    });
    $("#LoadEmployee").click(function () {
        LoadEmployee(false);
    });
    $("#LoadTimeAttendance").click(function () {
        LoadTimeAttendance(false);
    });
    $("#LoadEmployeeSchedule").click(function () {
        LoadEmployeeSchedule(false);
    });
    $("#LoadPayroll").click(function () {
        LoadPayroll(false);
    });
    $("#LoadBill").click(function () {
        LoadBill(false);
    });
    $("#LoadTagManager").click(function () {
        LoadTagManager(false);
    });
    $("#LoadBuildVersion").click(function () {
        LoadBuildVersion(false);
    });
    $("#LoadExpence").click(function () {
        LoadExpence(false);
    });
    $("#LoadConfig").click(function () {
        LoadConfig(false);
    });
    $("#LoadCheck").click(function () {
        LoadCheck(false);
    });
    $("#LoadVendorCredit").click(function () {
        LoadVendorCredit(false);
    });
    $("#LoadReport").click(function () {
        LoadReport(false);
    });
    $("#LoadReportSales").click(function () {
        LoadReportSales(false);
    });
    $("#LoadReportCustomer").click(function () {
        LoadReportCustomer(false);
    });
    $("#LoadReportSchedule").click(function () {
        LoadReportSchedule(false);
    });
    $("#LoadReportEmployee").click(function () {
        LoadReportEmployee(false);
    });
    $("#LoadBanking").click(function () {
        LoadBanking(false);
    });
    $("#LoadThirdPartyApi").click(function () {
        LoadThirdPartyApi(false);
    });
    $("#LoadCompany").click(function () {
        LoadCompany(false);
    });

    //$("#LoadUserMgmt").click(function () {
    //    console.log("LoadUserMgmt");
    //    LoadUserMgmt();
    //});
    $("#LoadUserGroup").click(function () {
        LoadUserGroup();
    });
    $("#LoadUserTeam").click(function () {
        LoadUserTeam();
    });
    $("#LoadCompanyBranch").click(function () {
        LoadCompanyBranch(false);
    });
    $("#LoadSupplier").click(function () {
        LoadSupplier(false);
    });
    $("#LoadRecruitUserInfo").click(function () {
        LoadRecruitUserInfo(false);
    });

    $("#LoadMmmrs").click(function () {
        LoadMMR(false);
    });
    //$("#LoadPanelType").click(function () {
    //    console.log("LoadUserMgmt");
    //    LoadMMR(false);
    //});
    $("#LoadInventory").click(function () {
        LoadInventory(false);
    });
    $("#LoadEstimate").click(function () {
        LoadEstimate(false);
    });
    $("#LoadUserProfile").click(function () {
        LoadUserProfile(false);
    })
}
var LoadNotifications = function () {
    $(".LoadNotification_div").html("<div class='notification_loader_container'><div class='notification_loader'></div></div>");
    $(".LoadNotification_div").load(domainurl + "/Notification/GetUserNotifications");
}
var LoadClockInOut = function () {
    $(".LoadClockInOutDiv").load(domainurl + "/TimeClockPto/EmployeeClockInOut");
}
var MarkAllNotificationAsRead = function () {
    $.ajax({
        type: "POST",
        url: domainurl + "/Notification/MarkAllNotificationAsRead",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result == true) {
                $(".notification_counter").text('0');
                $(".notification_counter").addClass("hidden");
            }
        }
    });
}
var initNavbarClicks = function () {
    $(".navbar-right .dropdown .AdvanceSearchSearchBarDiv").click(function (event) {
        event.stopPropagation();
        console.log(event.target.id);
    });
    $(".navbar-right .dropdown").click(function (event) {
        if (($(this).hasClass('open') && $(event.target).hasClass('dropdown-toggle')) ||
            ($(this).hasClass('open') && $(event.target).hasClass('open')) ||
            ($(this).hasClass('open') && $(event.target).hasClass('fa'))) {
            $(this).removeClass('open');
            $(".flyout-overlay").hide();
        } else if ($(this).parent().children('.dropdown').hasClass('open')) {
            $(this).parent().children('.dropdown').removeClass('open');
            $(this).addClass('open');
        } else {
            $(".flyout-overlay").show();
            $(this).addClass('open');
            if ($(this).hasClass('notification_dropdown')) {
                LoadNotifications();
            }
            if ($(this).hasClass('clockinout_dropdown')) {
                LoadClockInOut();
            }
        }
    });
    $(".flyout-overlay, .submenuelement a").click(function () {
        setTimeout(function () {
            $(".navbar-right .dropdown").removeClass('open');
            $(".flyout-overlay").hide();
        }, 80);
    });

}
var AdvancedSearchButtonClick = function () {
    if ($("#AdvancedSearchText").val() == "") {
        return;
    }
    $(".navbar-right .dropdown").removeClass('open');
    $(".flyout-overlay").hide();
    var SearchKey = encodeURIComponent($("#AdvancedSearchText").val());
    SearchKey += "&SearchFor=" + $("#AdvancedSearchOptions").val();
    LoadAdvancedSearchByKey(true, SearchKey);
}
var OpenEstById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal("/Estimate/AddEstimate/?Id=" + invId);
    }
}
var OpenEstimatorById = function (EstimatorId) {
    if (typeof (EstimatorId) != "undefined" && EstimatorId > 0) {
        OpenTopToBottomModal("/Estimator/AddEstimator/?Id=" + EstimatorId);
    }
}
var OpenInvById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?Id=" + invId);
    } else if (typeof (invId) != "undefined" && invId.indexOf('INV') > -1) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?InvoiceId=" + invId);
    }
}
var OpenTicketById = function (ticketId) {
    if (typeof (ticketId) != "undefined" && ticketId > 0) {
        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + ticketId);
    }
}

var OpenTicketByIdL1 = function (ticketId) {
    if (typeof (ticketId) != "undefined" && ticketId > 0) {
        OpenTopToBottomL1Modal(domainurl + "/Ticket/AddTicket/?Id=" + ticketId);
    }
}

var OpenPOById = function (POId) {
    if (typeof (POId) != "undefined" && POId > 0) {
        OpenTopToBottomModal(domainurl + "/PurchaseOrder/AddPurchaseOrder/?Id=" + POId);
    } else if (typeof (POId) != "undefined" && POId.indexOf('PO') > -1) {
        OpenTopToBottomModal(domainurl + "/PurchaseOrder/AddPurchaseOrder/?PurchaseOrderId=" + POId);
    }
}
var OpenBillById = function (BillId) {
    if (typeof (BillId) != "undefined" && BillId > 0) {
        OpenTopToBottomModal(domainurl + "/Expense/AddVendorBill/?id=" + BillId);
    }
    else if (typeof (BillId) != "undefined" && BillId.indexOf("BL") == 0) {
        OpenTopToBottomModal(domainurl + "/Expense/AddVendorBill/?BillID=" + BillId);
    }
}
var ShowCustomerChange = function (CustomerId) {
    window.location.href = domainurl + "/Customer/ShowCustomerChange?CustomerId=" + CustomerId;
}
var OpenUrl = function (newurl) {
    if (typeof (newurl) != "undefined" && newurl != "") {
        if (newurl.toLowerCase().indexOf('/ticket/addticket/?id=') > -1
            && newurl.toLowerCase().split('/ticket/addticket/?id=').length > 1) {
            OpenTicketById(newurl.toLowerCase().split('/ticket/addticket/?id=')[1]);
        } else {
            location.href = newurl;
        }
    }
}
var ClearAutoComplete = function () {
    setTimeout(function () {
        $("input:-webkit-autofill").each(function () {
            $(this).val('');
        });
        //$("input:-moz-autofill").each(function () {
        //    $(this).val('')
        //});
    }, 300);
}
var ShowAnnouncement = function () {
    $.ajax({
        type: "POST",
        url: domainurl + "/Notification/GetAllAnnouncementByCurrentDate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var AnnouncementText = "";
            $.each(data.AnnouncementList, function (i, data) {
                AnnouncementText += "<span style='margin-right:50px'>" + data.Message + "</span>";
            });
            if (AnnouncementText != "") {
                $("#AnnouncementText").html(AnnouncementText);
                $(".announcement-div").show();
                document.getElementById('AnnouncementText').start();
                $.cookie("_AnnouncementOldCount", data.AnnouncementListCount, { expires: 10 });
                $.cookie("_AnnouncementClose", '', { expires: -1 });
                initHeight();
            }
            else {
                $.cookie("_AnnouncementOldCount", 0, { expires: 10 });
            }
        }
    });
}
var cntrlIsPressed = false;
$(document).keydown(function (event) {
    if (event.which == "17")
        cntrlIsPressed = true;
});
$(document).keyup(function (event) {
    cntrlIsPressed = false;
});
var ActiveClassCustomColor = function () {
    console.log("color");
    $(".sidebar ul li a").each(function () {
        $(this).removeClass('active');
        $(this).attr('style', 'color:#454545;');
        if ($(this).attr('href') == window.location.pathname) {
            $(this).attr('style', 'color:#2ca01c');
            $(this).addClass('active');
        }
    })
}
var UnassignedLeadsCount = function () {
    $.ajax({
        type: "POST",
        url: domainurl + "/App/UnassignedLeadsCount",
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data.count != null) {
                if (data.count.TotalLeads > 0) {
                    $("#unassigncount").html("(" + data.count.TotalLeads + ")");
                }
                else {
                    $("#unassigncount").html("");
                }
            }
        }
    });
}
var UnreadArticleCount = function () {
    $.ajax({
        type: "POST",
        url: domainurl + "/App/UnreadArticleCount",
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data.count != null) {
                if (data.count.TotalLeads > 0) {
                    $("#ClassroomUnreadCount").html("(" + data.count.TotalLeads + ")");
                }
                else {
                    $("#ClassroomUnreadCount").html("");
                }
            }
        }
    });
}
$(document).ready(function () {
    UnassignedLeadsCount();
    UnreadArticleCount(); 
    $("#LoadClassroom").click(function () { 
        $("#ClassroomSubMenu").slideDown();
        $("#LoadClassroom").addClass('active');
        location.href = domainurl + "/assignarticles";
    })
    if (window.location.pathname.toLowerCase() == "/lead/leadsdetail/") {
        $(".sidebar ul li a.lead_load").attr('style', 'color:#2ca01c');
        $(".sidebar ul li a.lead_load").addClass('active');
    }
    else if (window.location.pathname.toLowerCase() == "/customer/customerdetail/") {
        $(".sidebar ul li a.customer_load").attr('style', 'color:#2ca01c');
        $(".sidebar ul li a.customer_load").addClass('active');
        $("#CustomerSubMenu").slideDown();
    }
    else if (window.location.pathname.toLowerCase() == "/calendar") {
        $(".sidebar ul li a#LoadCustomCalendarInformation").attr('style', 'color:#2ca01c');
        $(".sidebar ul li a#LoadCustomCalendarInformation").addClass('active');
         $("#ShowCalendarMap").slideDown();
    }
    else if (window.location.pathname.toLowerCase().includes('customer')) {
        $("#CustomerSubMenu").slideDown();
    }
    else {
        ActiveClassCustomColor();
    }
    
    var flag = false;
    //$("#AllReportList").slideUp();
    $("#LoadReport1").click(function () {
        if (flag == false) {
            $("#AllReportList").slideDown();
            flag = true;
        }
        else {
            $("#AllReportList").slideUp();
            flag = false;
        }
    })
    $("#LoadCustomer1").click(function () {
        //$("#CustomerSubMenu").hide();
        $("#LoadCustomer1").addClass('active');
        location.href = domainurl + "/customer";
        $("#CustomerSubMenu").slideDown();
    })
    $("#LoadCustomCalendarInformation").click(function () {  
        $("#LoadCustomCalendarInformation").addClass('active');
        location.href = domainurl + "/calendar";     
        $("#ShowCalendarMap").hide();
    })
    $(".announcement-div").hide();
    if (typeof $.cookie('_AnnouncementClose') == 'undefined' || $.cookie('_AnnouncementClose') == null) {
        ShowAnnouncement();
    }
    $("#AnnouncementClose").click(function () {
        $.cookie("_AnnouncementClose", 1, { expires: 10 });
        $(".announcement-div").hide();
        initHeight();
    })
    $('ul li a').click(function () {
        $('li a').removeClass("focus");
        $(this).addClass("focus");
    });

    $("#UserCompanyListLayout").val(UserCurrentCompanyId);

    $("#UserCompanyListLayout").change(function () {
        if ($(this).val() != UserCurrentCompanyId) {
            SetUserCompany();
        }
    });
    $("#AdvancedSearchBtn").click(function () {
        AdvancedSearchButtonClick();
    });
    $("#AdvancedSearchText").bind('keypress', function (e) {
        if (e.keyCode == 13) {
            AdvancedSearchButtonClick();
        }
    });
    initHeight();
    initMenuClicks();
    initNavbarClicks();
    /*Custom Select class*/
    //var selectlist = $('select');
    //for (var iscount = 0; iscount < $(selectlist).length; iscount++) {
    //    $($(selectlist[iscount]).parent()).addClass('selectstyle');
    //}
    /*Custom Select class*/
    $('ul li a').click(function () {
        $(this).addClass("focus");
    });
    $("#GlobalSearchButton").click(function () {
        $(".GlobalEstInvCustSearchDiv .tt-menu").hide();
        GlobalSearchButtonClick();
    });
    $(document).click(function () { $('.GlobalEstInvCustSearchDiv .tt-menu').hide() });

    $(".flyout-overlay").width(window.innerWidth);
    ClearAutoComplete();
    
});
//$(window).resize(function () {
//    console.log("sdf")
//    setTimeout(function () {
//        $(".page-wrapper-contents").height(contentHeight);
//    }, 100);
//});


$.fn.load = function (url, params, callback) {
    if (typeof url !== "string" && _load) {
        return _load.apply(this, arguments);
    }

    var selector, type, response,
		self = this,
		off = url.indexOf(" ");

    if (off > -1) {
        selector = jQuery.trim(url.slice(off, url.length));
        url = url.slice(0, off);
    }

    // If it's a function
    if (jQuery.isFunction(params)) {

        // We assume that it's the callback
        callback = params;
        params = undefined;

        // Otherwise, build a param string
    } else if (params && typeof params === "object") {
        type = "POST";
    }

    // If we have elements to modify, make the request
    if (self.length > 0) {
        jQuery.ajax({
            url: url,

            // If "type" variable is undefined, then "GET" method will be used.
            // Make value of this field explicit since
            // user can override it through ajaxSetup method
            type: type || "GET",
            dataType: "html",
            data: params
        }).done(function (responseText) {

            // Save response for use in complete callback
            response = arguments;

            self.html(selector ?

				// If a selector was specified, locate the right elements in a dummy div
				// Exclude scripts to avoid IE 'Permission Denied' errors
				jQuery("<div>").append(jQuery.parseHTML(responseText)).find(selector) :

				// Otherwise use the full result
				responseText);
            ClearAutoComplete();
            // If the request succeeds, this function gets "data", "status", "jqXHR"
            // but they are ignored because response was set above.
            // If it fails, this function gets "jqXHR", "status", "error"
        }).always(callback && function (jqXHR, status) {
            self.each(function () {
                callback.apply(this, response || [jqXHR.responseText, status, jqXHR]);
            });
        });
    }

    return this;
}


