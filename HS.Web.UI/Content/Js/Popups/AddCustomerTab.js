var gid = 0;
var tabdata;
var LastTabid = 0;
var SaveCustomerTab = function () {
    var url = domainurl + "/Customer/AddCustomerTab/";

    var DobVal = DobDatepicker.getDate();
    var SalesVal = SalesDatepicker.getDate();
    var InstallVal = InstallDatepicker.getDate();
    var CutinVal = CutInDatepicker.getDate();
    var FundingVal = FundingDatepicker.getDate();
    var Qa1Val = QA1datepicker.getDate();
    var Qa2Val = QA2datepicker.getDate();
    var CurrentDate = new Date();

    if (DobVal.getUTCDate() == CurrentDate.getUTCDate()
        && DobVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && DobVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        DobVal = "";
    }
    if (SalesVal.getUTCDate() == CurrentDate.getUTCDate()
        && SalesVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && SalesVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        SalesVal = "";
    }
    if (InstallVal.getUTCDate() == CurrentDate.getUTCDate()
        && InstallVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && InstallVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        InstallVal = "";
    }
    if (CutinVal.getUTCDate() == CurrentDate.getUTCDate()
        && CutinVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && CutinVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        CutinVal = "";
    }
    if (FundingVal.getUTCDate() == CurrentDate.getUTCDate()
        && FundingVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && FundingVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        FundingVal = "";
    }
    if (Qa1Val.getUTCDate() == CurrentDate.getUTCDate()
        && Qa1Val.getUTCMonth() == CurrentDate.getUTCMonth()
        && Qa1Val.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        Qa1Val = "";
    }
    if (Qa2Val.getUTCDate() == CurrentDate.getUTCDate()
        && Qa2Val.getUTCMonth() == CurrentDate.getUTCMonth()
        && Qa2Val.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        Qa2Val = "";
    }

    var param = {
        id: $("#id").val(),
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $("#EmailAddress").val(),
        Type: $("#Type").val(),
        DateofBirth: DobVal,
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        Country: $("#Country").val(),
        PrimaryPhone: $("#PrimaryPhone").val(),
        SecondaryPhone: $("#SecondaryPhone").val(),
        CellNo: $("#CellNo").val(),
        Fax: $("#Fax").val(),
        CallingTime: $("#CallingTime").val(),
        SalesDate: SalesVal,
        Soldby: $("#SoldBy").val(),
        InstallDate: InstallVal,
        Installer: $("#Installer").val(),
        CutInDate: CutinVal,
        FundingDate: FundingVal,
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScore").val(),
        ContractTeam: $("#ContractTerm").val(),
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: $("#CellularBackup").val(),
        CustomerFunded: parseInt($("#CustomerFunded").val()),
        Maintenance: parseInt($("#Maintenance").val()),
        Note: $("#Note").val(),
        SSN: $("#SSN").val(),
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        CustomerNo: $("#CustomerNum").val(),
        MiddleName: $("#MiddleName").val(),
        BillAmount: $("#BillAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillTax: parseInt($("#BillTax").val()),
        BillOutStanding: $("#BillOutStanding").val(),
        QA1: $("#QA1").val(),
        QA1Date: Qa1Val,
        QA2: $("#QA2").val(),
        QA2Date: Qa2Val,
        ResponsiblePerson1: $("#ResponsiblePerson1").val(),
        ResponsiblePerson2: $("#ResponsiblePerson2").val(),
        ResponsiblePerson3: $("#ResponsiblePerson3").val(),
        ResponsiblePerson4: $("#ResponsiblePerson4").val(),
        ResponsiblePersonContact1: $("#ResponsiblePersonContact1").val(),
        ResponsiblePersonContact2: $("#ResponsiblePersonContact2").val(),
        ResponsiblePersonContact3: $("#ResponsiblePersonContact3").val(),
        ResponsiblePersonContact4: $("#ResponsiblePersonContact4").val(),
        ResponsiblePersonEmail1: $("#ResponsiblePersonEmail1").val(),
        ResponsiblePersonEmail2: $("#ResponsiblePersonEmail2").val(),
        ResponsiblePersonEmail3: $("#ResponsiblePersonEmail3").val(),
        ResponsiblePersonEmail4: $("#ResponsiblePersonEmail4").val()
    };
    var systemInfo = {
        Id: $("#Idval").val(),
        CustomerId: $("#ValCustomerId").val(),
        panelType: $("#panelType").val(),
        installType: $("#installType").val(),
        cellularBackup: $("#cellularBackup").val(),
        zone1: $("#zone1").val(),
        zone2: $("#zone2").val(),
        zone3: $("#zone3").val(),
        zone4: $("#zone4").val(),
        zone5: $("#zone5").val(),
        zone6: $("#zone6").val(),
        zone7: $("#zone7").val(),
        zone8: $("#zone8").val(),
        zone9: $("#zone9").val()
    };
    var settingApiAlarm = {
        Id: $("#Idalarm").val(),
        CustomerId: $("#Cusalarm").val(),
        AccountName: $("#AccNameAlarm").val(),
        Url: $("#UrlAlarm").val(),
        UserName: $("#UsernameAlarm").val(),
        Password: $("#PasswordAlarm").val()
    };
    var settingApiMoni = {
        Id: $("#Idmoni").val(),
        CustomerId: $("#Cusmoni").val(),
        AccountName: $("#AccNameMoni").val(),
        Url: $("#UrlMoni").val(),
        UserName: $("#UsernameMoni").val(),
        Password: $("#PasswordMoni").val()
    };
    var settingApiCentral = {
        Id: $("#Idcentral").val(),
        CustomerId: $("#Cuscentral").val(),
        AccountName: $("#AccNameCentral").val(),
        Url: $("#UrlCentral").val(),
        UserName: $("#UsernameCentral").val(),
        Password: $("#PasswordCentral").val()
    };
    
    var passparam = JSON.stringify({
        'customerTab': param, 'systemInfoTab': systemInfo, 'apiAlarmTab': settingApiAlarm, 'apiMoniTab': settingApiMoni, 'apiCentralTab': settingApiCentral
    });
    console.log(param);
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.status == true && data.tab != "") {
                var Customerid = data.customerid;
                gid = $("#id").val(Customerid);
                parent.LoadCustomerDetail(Customerid);
                parent.ClosePopup();
                
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var SaveCustomerInsertTab = function () {
    console.log("SaveCustomerInsert er vitor")
    var url = domainurl + "/Customer/AddCustomerTab/";

    var DobVal = DobDatepicker.getDate();
    var SalesVal = SalesDatepicker.getDate();
    var InstallVal = InstallDatepicker.getDate();
    var CutinVal = CutInDatepicker.getDate();
    var FundingVal = FundingDatepicker.getDate();
    var Qa1Val = QA1datepicker.getDate();
    var Qa2Val = QA2datepicker.getDate();
    var CurrentDate = new Date();

    if (DobVal.getUTCDate() == CurrentDate.getUTCDate()
        && DobVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && DobVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        DobVal = "";
    }
    if (SalesVal.getUTCDate() == CurrentDate.getUTCDate()
        && SalesVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && SalesVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        SalesVal = "";
    }
    if (InstallVal.getUTCDate() == CurrentDate.getUTCDate()
        && InstallVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && InstallVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        InstallVal = "";
    }
    if (CutinVal.getUTCDate() == CurrentDate.getUTCDate()
        && CutinVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && CutinVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        CutinVal = "";
    }
    if (FundingVal.getUTCDate() == CurrentDate.getUTCDate()
        && FundingVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && FundingVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        FundingVal = "";
    }
    if (Qa1Val.getUTCDate() == CurrentDate.getUTCDate()
        && Qa1Val.getUTCMonth() == CurrentDate.getUTCMonth()
        && Qa1Val.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        Qa1Val = "";
    }
    if (Qa2Val.getUTCDate() == CurrentDate.getUTCDate()
        && Qa2Val.getUTCMonth() == CurrentDate.getUTCMonth()
        && Qa2Val.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        Qa2Val = "";
    }
    var param = {
        id: $("#id").val(),
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $("#EmailAddress").val(),
        Type: $("#Type").val(),
        DateofBirth: DobVal,
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        Country: $("#Country").val(),
        PrimaryPhone: $("#PrimaryPhone").val(),
        SecondaryPhone: $("#SecondaryPhone").val(),
        CellNo: $("#CellNo").val(),
        Fax: $("#Fax").val(),
        CallingTime: $("#CallingTime").val(),
        SalesDate: SalesVal,
        Soldby: $("#SoldBy").val(),
        InstallDate: InstallVal,
        Installer: $("#Installer").val(),
        CutInDate: CutinVal,
        FundingDate: FundingVal,
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScore").val(),
        ContractTeam: $("#ContractTerm").val(),
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: $("#CellularBackup").val(),
        CustomerFunded: parseInt($("#CustomerFunded").val()),
        Maintenance: parseInt($("#Maintenance").val()),
        Note: $("#Note").val(),
        SSN: $("#SSN").val(),
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        CustomerNo: $("#CustomerNum").val(),
        MiddleName: $("#MiddleName").val(),
        BillAmount: $("#BillAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillTax: parseInt($("#BillTax").val()),
        BillOutStanding: $("#BillOutStanding").val(),
        QA1: $("#QA1").val(),
        QA1Date: Qa1Val,
        QA2: $("#QA2").val(),
        QA2Date: Qa2Val,
        ResponsiblePerson1: $("#ResponsiblePerson1").val(),
        ResponsiblePerson2: $("#ResponsiblePerson2").val(),
        ResponsiblePerson3: $("#ResponsiblePerson3").val(),
        ResponsiblePerson4: $("#ResponsiblePerson4").val(),
        ResponsiblePersonContact1: $("#ResponsiblePersonContact1").val(),
        ResponsiblePersonContact2: $("#ResponsiblePersonContact2").val(),
        ResponsiblePersonContact3: $("#ResponsiblePersonContact3").val(),
        ResponsiblePersonContact4: $("#ResponsiblePersonContact4").val(),
        ResponsiblePersonEmail1: $("#ResponsiblePersonEmail1").val(),
        ResponsiblePersonEmail2: $("#ResponsiblePersonEmail2").val(),
        ResponsiblePersonEmail3: $("#ResponsiblePersonEmail3").val(),
        ResponsiblePersonEmail4: $("#ResponsiblePersonEmail4").val()
    };
    var systemInfo = {
        Id: $("#Idval").val(),
        CustomerId: $("#ValCustomerId").val(),
        panelType: $("#panelType").val(),
        installType: $("#installType").val(),
        cellularBackup: $("#cellularBackup").val(),
        zone1: $("#zone1").val(),
        zone2: $("#zone2").val(),
        zone3: $("#zone3").val(),
        zone4: $("#zone4").val(),
        zone5: $("#zone5").val(),
        zone6: $("#zone6").val(),
        zone7: $("#zone7").val(),
        zone8: $("#zone8").val(),
        zone9: $("#zone9").val()
    };
    var settingApiAlarm = {
        Id: $("#Idalarm").val(),
        CustomerId: $("#Cusalarm").val(),
        AccountName: $("#AccNameAlarm").val(),
        Url: $("#UrlAlarm").val(),
        UserName: $("#UsernameAlarm").val(),
        Password: $("#PasswordAlarm").val()
    };
    var settingApiMoni = {
        Id: $("#Idmoni").val(),
        CustomerId: $("#Cusmoni").val(),
        AccountName: $("#AccNameMoni").val(),
        Url: $("#UrlMoni").val(),
        UserName: $("#UsernameMoni").val(),
        Password: $("#PasswordMoni").val()
    };
    var settingApiCentral = {
        Id: $("#Idcentral").val(),
        CustomerId: $("#Cuscentral").val(),
        AccountName: $("#AccNameCentral").val(),
        Url: $("#UrlCentral").val(),
        UserName: $("#UsernameCentral").val(),
        Password: $("#PasswordCentral").val()
    };

    var passparam = JSON.stringify({
        'customerTab': param, 'systemInfoTab': systemInfo, 'apiAlarmTab': settingApiAlarm, 'apiMoniTab': settingApiMoni, 'apiCentralTab': settingApiCentral
    });

    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.status == true && data.tab != "") {
                var Customerid = data.customerid;
                gid = $("#id").val(Customerid);
                LastTabid = data.customerid;
                //removeactive
                $(data.tab).addClass('active');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
function FormatePhoneNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#PrimaryPhone").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact1").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact2").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact3").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact4").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#PrimaryPhone").css({ "border": "1px solid red" });
            $("#ResponsiblePersonContact1").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact2").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact3").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact4").css({ "border": "1px solid #babec5" });
        }
        else {
            $("#PrimaryPhone").css({ "border": "1px solid red" });
            $("#ResponsiblePersonContact1").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact2").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact3").css({ "border": "1px solid #babec5" });
            $("#ResponsiblePersonContact4").css({ "border": "1px solid #babec5" });
            ValueClean = Value;
        }
    }
    return ValueClean;
}
function FormateSSNNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-  ]/g, '');
        if (Value.length == 10) {
            ValueClean = Value.replace(/(\d{4})\-?(\d{2})\-?(\d{4})/, "$1-$2-$3");
            $("#SSN").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#SSN").css({ "border": "1px solid red" });
        }
        else {
            ValueClean = Value;
            $("#SSN").css({ "border": "1px solid #babec5" });
        }
    }
    return ValueClean;
}
function activaTab(tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
};
var BillAmountBillCycleChangeEffect = function () {
    var monitoringfee = $("#MonthlyMonitoringFee").val();
    var cycle = $("#BillCycle").val();
    var tax = parseInt($("#BillTax").val());
    var taxvalue = (parseFloat(monitoringfee) + (parseFloat(monitoringfee) * (TaxAmount / 100)));

    if (monitoringfee != null && tax == 1) {
        if (cycle == "Annual") {
            var addval = taxvalue * 12;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Monthly") {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Quarterly") {
            var addval = taxvalue * 4;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Semi-Annual") {
            var addval = taxvalue * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
    else if (monitoringfee != null && tax == 0) {
        if (cycle == "Annual") {
            var value = parseFloat(monitoringfee) * 12;
            $("#BillAmount").val(value.toFixed(2));
        }
        else if (cycle == "Monthly") {
            var addval = parseFloat(monitoringfee) * 1;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Quarterly") {
            var addval = parseFloat(monitoringfee) * 4;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Semi-Annual") {
            var addval = parseFloat(monitoringfee) * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = parseFloat(monitoringfee);
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
}
var BillAmountMonthlyMonitoringFeeChangeEffect = function () {
    var monitoringfee = $("#MonthlyMonitoringFee").val();
    var cycle = $("#BillCycle").val();
    var tax = parseInt($("#BillTax").val());
    var taxvalue = (parseFloat(monitoringfee) + (parseFloat(monitoringfee) * (TaxAmount / 100)));

    if (monitoringfee != null && tax == 1) {
        if (cycle == "Annual") {
            var addval = taxvalue * 12;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Monthly") {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Quarterly") {
            var addval = taxvalue * 4;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Semi-Annual") {
            var addval = taxvalue * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
    else if (monitoringfee != null && tax == 0) {
        if (cycle == "Annual") {
            var value = parseFloat(monitoringfee) * 12;
            $("#BillAmount").val(value.toFixed(2));
        }
        else if (cycle == "Monthly") {
            var addval = parseFloat(monitoringfee) * 1;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Quarterly") {
            var addval = parseFloat(monitoringfee) * 4;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Semi-Annual") {
            var addval = parseFloat(monitoringfee) * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = parseFloat(monitoringfee);
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
}
var BillAmountBillTaxChangeEffect = function () {

    var monitoringfee = $("#MonthlyMonitoringFee").val();
    var cycle = $("#BillCycle").val();
    var tax = parseInt($("#BillTax").val());
    var taxvalue = (parseFloat(monitoringfee) + (parseFloat(monitoringfee) * (TaxAmount / 100)));

    if (monitoringfee != null && tax == 1) {
        if (cycle == "Annual") {
            var addval = taxvalue * 12;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Monthly") {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Quarterly") {
            var addval = taxvalue * 4;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Semi-Annual") {
            var addval = taxvalue * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
    else if (monitoringfee != null && tax == 0) {
        if (cycle == "Annual") {
            var value = parseFloat(monitoringfee) * 12;
            $("#BillAmount").val(value.toFixed(2));
        }
        else if (cycle == "Monthly") {
            var addval = parseFloat(monitoringfee) * 1;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Quarterly") {
            var addval = parseFloat(monitoringfee) * 4;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else if (cycle == "Semi-Annual") {
            var addval = parseFloat(monitoringfee) * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = parseFloat(monitoringfee);
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
}

$(document).ready(function () {
    $("#BillCycle").change(function () {
        BillAmountBillCycleChangeEffect();
    });
    $("#MonthlyMonitoringFee").change(function () {
        BillAmountMonthlyMonitoringFeeChangeEffect();
    })
    $("#BillTax").change(function () {
        BillAmountBillTaxChangeEffect();
    })

    //$("#BillCycle").change(function () {
    //    var monitoringfee = $("#MonthlyMonitoringFee").val();
    //    var cycle = $("#BillCycle").val();
    //    var tax = parseInt($("#BillTax").val());
    //    var taxvalue = (parseFloat(monitoringfee) + 0.15);

    //    if (monitoringfee != null && tax == 1) {
    //        if (cycle == "Annual") {
    //            var addval = taxvalue * 12;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Monthly") {
    //            var addval = taxvalue;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Quarterly") {
    //            var addval = taxvalue * 4;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Semi-Annual") {
    //            var addval = taxvalue * 6;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else {
    //            var addval = taxvalue;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //    }
    //    else if (monitoringfee != null && tax == 0) {
    //        if (cycle == "Annual") {
    //            var value = parseFloat(monitoringfee) * 12;
    //            $("#BillAmount").val(value.toFixed(2));
    //        }
    //        else if (cycle == "Monthly") {
    //            var addval = parseFloat(monitoringfee) * 1;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Quarterly") {
    //            var addval = parseFloat(monitoringfee) * 4;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Semi-Annual") {
    //            var addval = parseFloat(monitoringfee) * 6;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else {
    //            var addval = parseFloat(monitoringfee);
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //    }
    //})
    //$("#MonthlyMonitoringFee").change(function () {
    //    var monitoringfee = $("#MonthlyMonitoringFee").val();
    //    var cycle = $("#BillCycle").val();
    //    var tax = parseInt($("#BillTax").val());
    //    var taxvalue = (parseFloat(monitoringfee) + 0.15);

    //    if (monitoringfee != null && tax == 1) {
    //        if (cycle == "Annual") {
    //            var addval = taxvalue * 12;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Monthly") {
    //            var addval = taxvalue;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Quarterly") {
    //            var addval = taxvalue * 4;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Semi-Annual") {
    //            var addval = taxvalue * 6;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else {
    //            var addval = taxvalue;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //    }
    //    else if (monitoringfee != null && tax == 0) {
    //        if (cycle == "Annual") {
    //            var value = parseFloat(monitoringfee) * 12;
    //            $("#BillAmount").val(value.toFixed(2));
    //        }
    //        else if (cycle == "Monthly") {
    //            var addval = parseFloat(monitoringfee) * 1;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Quarterly") {
    //            var addval = parseFloat(monitoringfee) * 4;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Semi-Annual") {
    //            var addval = parseFloat(monitoringfee) * 6;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else {
    //            var addval = parseFloat(monitoringfee);
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //    }
    //})
    //$("#BillTax").change(function () {
    //    var monitoringfee = $("#MonthlyMonitoringFee").val();
    //    var cycle = $("#BillCycle").val();
    //    var tax = parseInt($("#BillTax").val());
    //    var taxvalue = (parseFloat(monitoringfee) + 0.15);

    //    if (monitoringfee != null && tax == 1) {
    //        if (cycle == "Annual") {
    //            var addval = taxvalue * 12;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Monthly") {
    //            var addval = taxvalue;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Quarterly") {
    //            var addval = taxvalue * 4;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Semi-Annual") {
    //            var addval = taxvalue * 6;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else {
    //            var addval = taxvalue;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //    }
    //    else if (monitoringfee != null && tax == 0) {
    //        if (cycle == "Annual") {
    //            var value = parseFloat(monitoringfee) * 12;
    //            $("#BillAmount").val(value.toFixed(2));
    //        }
    //        else if (cycle == "Monthly") {
    //            var addval = parseFloat(monitoringfee) * 1;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Quarterly") {
    //            var addval = parseFloat(monitoringfee) * 4;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else if (cycle == "Semi-Annual") {
    //            var addval = parseFloat(monitoringfee) * 6;
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //        else {
    //            var addval = parseFloat(monitoringfee);
    //            $("#BillAmount").val(addval.toFixed(2));
    //        }
    //    }
    //})
});