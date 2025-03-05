var gid = 0;
var LastTabid = 0;
var DobDatepicker;
var SalesDatepicker;
var InstallDatepicker;
var CutInDatepicker;
var FundingDatepicker;
var QA1datepicker;
var QA2datepicker;
var FirstBillingDatePicker;
var FundedCustomer;
var Maintance
var PaymentIncress;
//var SaveCustomerDraftextra = function () {

//    var url = "/CustomerPublic/CheckRoleAndGenerateToken/";

//    var SalesVal = $("#SalesDate").val();
//    var InstallVal = $("#InstallDate").val();
//    var CutinVal = $("#CutInDate").val();
//    var FundingVal = $("#FundingDate").val();
//    var Qa1Val = QA1datepicker.getDate();
//    var Qa2Val = QA2datepicker.getDate();

//    var DobVal = DobDatepicker.getDate();
//    var CurrentDate = new Date();

//    if (DobVal.getUTCDate() == CurrentDate.getUTCDate()
//        && DobVal.getUTCMonth() == CurrentDate.getUTCMonth()
//        && DobVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
//        DobVal = "";
//    }
//    if (Qa1Val.getUTCDate() == CurrentDate.getUTCDate()
//        && Qa1Val.getUTCMonth() == CurrentDate.getUTCMonth()
//        && Qa1Val.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
//        Qa1Val = "";
//    }
//    if (Qa2Val.getUTCDate() == CurrentDate.getUTCDate()
//        && Qa2Val.getUTCMonth() == CurrentDate.getUTCMonth()
//        && Qa2Val.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
//        Qa2Val = "";
//    }

//    var param = {

//        id: $("#id").val(),
//        title: $("#title").val(),
//        FirstName: $("#FirstName").val(),
//        LastName: $("#LastName").val(),
//        BusinessName: $("#BusinessName").val(),
//        EmailAddress: $("#EmailAddress").val(),
//        Type: $("#Type").val(),
//        DateofBirth: DobVal,
//        Street: $("#Street").val(),
//        City: $("#City").val(),
//        State: $("#State").val(),
//        ZipCode: $("#ZipCode").val(),
//        Country: $("#Country").val(),
//        PrimaryPhone: $("#PrimaryPhone").val(),
//        SecondaryPhone: $("#SecondaryPhone").val(),
//        CellNo: $("#CellNo").val(),
//        Fax: $("#Fax").val(),
//        CallingTime: $("#CallingTime").val(),
//        SalesDate: SalesVal,
//        Soldby: $("#SoldBy").val(),
//        InstallDate: InstallVal,
//        Installer: $("#Installer").val(),
//        CutInDate: CutinVal,
//        FundingDate: FundingVal,
//        AccountNo: $("#AccountNo").val(),
//        CreditScore: CreditScoreVal,
//        ContractTeam: ContactTerm,
//        FundingCompany: FundCompany,
//        MonthlyMonitoringFee: MonthlyMonitoringFeeVal,
//        LeadSource: $("#LeadSource").val(),
//        IsAlarmCom: $("#IsAlarmCom").val(),
//        CellularBackup: $("#CellularBackup").val(),
//        CustomerFunded: FundedCustomer,
//        Maintenance: CustomerMaintenance,
//        Note: $("#Note").val(),
//        SSN: $("#SSN").val(),
//        CityPrevious: $("#CityPrevious").val(),
//        StatePrevious: $("#StatePrevious").val(),
//        ZipCodePrevious: $("#ZipCodePrevious").val(),
//        CountryPrevious: $("#CountryPrevious").val(),
//        StreetPrevious: $("#StreetPrevious").val(),
//        CustomerNo: $("#CustomerNum").val(),
//        MiddleName: $("#MiddleName").val(),
//        BillAmount: BillingAmount,
//        PaymentMethod: PaymentMethod,
//        BillCycle: BillingCycle,
//        BillDay: BillDay,
//        BillNotes: BillNote,
//        BillTax: parseInt(CustomerBilltax),
//        BillOutStanding: OutStandingBalance,
//        ReferringCustomer: $('#Ref_customer').val(),
//        QA1: $("#QA1").val(),
//        QA1Date: Qa1Val,
//        QA2: $("#QA2").val(),
//        QA2Date: Qa2Val,
//        SecondCustomerNo: $("#SecondCustomerNo").val(),
//        AdditionalCustomerNo: $("#AdditionalCustomerNo").val(),
//        FirstBilling: GetTimeFormat($("#FirstBilling").val()),
//        StreetType: $("#StreetType").val(),
//        Appartment: $("#Appartment").val(),
//        CrossStreet: $("#CrossStreet").val(),
//        DBA: $("#DBA").val(),
//        PreferedEmail: $("#PreferedEmail").is(":checked"),
//        PreferedSms: $("#PreferedSms").is(":checked"),
//        IsAgreement: $("#IsAgreement").is(":checked"),
//        IsFireAccount: $("#IsFireAccount").is(":checked"),
//        AuthorizeDescription: $("#AuthDescription").val(),
//        Note: $("#Note").val(),
//        BusinessAccountType: $("#BusinessAccountType").val(),
//        Ownership: $("#Ownership").val(),
//        PurchasePrice: $("#PurchasePrice").val(),
//        ContractValue: $("#ContractValue").val(),
//        AssignedTo: $("#AssignedTo").val(),
//        CompletionDate: GetTimeFormat($("#Ticket_CompletionDate").val()),
//        PaymentIncress: PaymentIncress,
//        Passcode: $("#VarbalPassword").val()
//    };
//    console.log(param);
//    var PaymentInfo;
//    if ($("#PaymentMethod").val() == "Credit Card") {
//        PaymentInfo = {
//            Id: $("#debit_payment_info_id").val(),
//            CardNumber: $("#credit_card_number").val(),
//            CardSecurityCode: $("#credit_card_CardSecurityCode").val(),
//            CardExpireDate: $("#credit_card_expireDate").val(),
//            AccountName: $("#Credit-Card_AccountName").val()
//        };
//    }
//    else if ($("#PaymentMethod").val() == "ACH") {
//        PaymentInfo = {
//            Id: $("#ach_payment_info_id").val(),
//            AccountName: $("#ACH_AccountName").val(),
//            BankName: $("#ACH_BankName").val(),
//            RoutingNo: $("#ACH_RoutingNo").val(),
//            AcountNo: $("#ACH_AcountNo").val(),
//            BankAccountType: $("#ACH_BankAccountType").val(),
//            ECheckType: $("#ACH_ECheckType").val(),
//        };
//    }
//    var systemInfo = {
//        Id: $("#Idval").val(),
//        CustomerId: $("#ValCustomerId").val(),
//        panelType: $("#panelType").val(),
//        installType: $("#installType").val(),
//        cellularBackup: $("#cellularBackup").val(),
//        zone1: $("#zone1").val(),
//        zone2: $("#zone2").val(),
//        zone3: $("#zone3").val(),
//        zone4: $("#zone4").val(),
//        zone5: $("#zone5").val(),
//        zone6: $("#zone6").val(),
//        zone7: $("#zone7").val(),
//        zone8: $("#zone8").val(),
//        zone9: $("#zone9").val()
//    };

//    var settingApiAlarm = {
//        Id: $("#Idalarm").val(),
//        CustomerId: $("#Cusalarm").val(),
//        AccountName: $("#AccNameAlarm").val(),
//        Url: $("#UrlAlarm").val(),
//        UserName: $("#UsernameAlarm").val(),
//        Password: $("#PasswordAlarm").val()
//    };
//    var settingApiMoni = {
//        Id: $("#Idmoni").val(),
//        CustomerId: $("#Cusmoni").val(),
//        AccountName: $("#AccNameMoni").val(),
//        Url: $("#UrlMoni").val(),
//        UserName: $("#UsernameMoni").val(),
//        Password: $("#PasswordMoni").val()
//    };
//    var settingApiCentral = {
//        Id: $("#Idcentral").val(),
//        CustomerId: $("#Cuscentral").val(),
//        AccountName: $("#AccNameCentral").val(),
//        Url: $("#UrlCentral").val(),
//        UserName: $("#UsernameCentral").val(),
//        Password: $("#PasswordCentral").val()
//    }

//    var passparam = JSON.stringify({
//        'customer': param,

//        'systemInfo': systemInfo,
//        'apiAlarm': settingApiAlarm,
//        'apiMoni': settingApiMoni,
//        'apiCentral': settingApiCentral,
//        'PaymentInfo': PaymentInfo
//    });

//    $.ajax({
//        type: "POST",
//        ajaxStart: $(".loader-div").show(),
//        url: url,
//        data: passparam,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        cache: false,
//        success: function (data) {
//            console.log("Hi");
//            if (data.result == false && data.timeout == true) {

//                var token = "";
//                parent.OpenTextModal("Give OTP!", "Please put your varbal password here.", function () {
//                    token = $("#CustomerOTP").val()
//                    checkToken(token);
//                });


//            }
//            else if (data.timeout == false) {

//                parent.OpenErrorMessageNew("Error!", data.message);
//            }


//            else if (data.status == true) {
//                var Customerid = data.customerid;
//                gid = $("#id").val(Customerid);
//                //ARB Subscription details
//                if (data.PaymentMethod == "Credit Card" || data.PaymentMethod == "ACH") {
//                    if (data.PaymentMethod == "Credit Card" && data.PaymentInfoId > 0) {
//                        $("#debit_payment_info_id").val(data.PaymentInfoId);
//                    } else if (data.PaymentMethod == "ACH" && data.PaymentInfoId > 0) {
//                        $("#ach_payment_info_id").val(data.PaymentInfoId);
//                    }
//                    if (typeof (data.AuthId) != "undefined" && data.AuthId != "" && data.AuthId != null) {
//                        $("#AuthorizeRefId").val(data.AuthId);
//                        $("#unsubscribe_to_authorize").removeClass('hidden');
//                        $("#subscribe_to_authorize").addClass('hidden');
//                    }

//                    /*if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
//                        parent.OpenConfirmationMessage("Message", data.AuthMessage);
//                    }*/
//                }
//                parent.OpenSuccessMessageNew("Success!", "Customer saved succesfully.Wait for admin approval", function () {
//                    CloseTopToBottomModal();
//                });
//                //parent.OpenCustomerDetailTab(this);
//                //ARB Subscription details ends
//            }
//            else if (data.status == false && typeof (data.message) != "undefined") {
//                OpenErrorMessageNew("Error!", data.message);
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            $(".loader-div").hide();
//            console.log(errorThrown);
//        }
//    });
//}
var SaveCustomerDraft = function () {
    parent.OpenTextModal("Give Verbal Password!", "Give your verbal password here.", function () {
        token = $("#CustomerOTP").val()
        checkToken(token);
     
    });
}
var checkToken = function (token) {
    var SalesVal = $("#SalesDate").val();
    var InstallVal = $("#InstallDate").val();
    var CutinVal = $("#CutInDate").val();
    var Qa1Val = QA1datepicker.getDate();
    var Qa2Val = QA2datepicker.getDate();

    var DobVal = DobDatepicker.getDate();
    var CurrentDate = new Date();

    if (DobVal.getUTCDate() == CurrentDate.getUTCDate()
        && DobVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && DobVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        DobVal = "";
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
        CreditScore: CreditScoreVal,
        ContractTeam: ContactTerm,
        FundingCompany: FundCompany,
        MonthlyMonitoringFee: MonthlyMonitoringFeeVal,
        LeadSource: $("#LeadSource").val(),
        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: $("#CellularBackup").val(),
        CustomerFunded: FundedCustomer,
        Maintenance: CustomerMaintenance,
        Note: $("#Note").val(),
        SSN: $("#SSN").val(),
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        CustomerNo: $("#CustomerNum").val(),
        MiddleName: $("#MiddleName").val(),
        BillAmount: BillingAmount,
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: BillingCycle,
        BillDay: BillDay,
        BillNotes: BillNote,
        BillTax: parseInt(CustomerBilltax),
        BillOutStanding: OutStandingBalance,
        ReferringCustomer: $('#Ref_customer').val(),
        QA1: $("#QA1").val(),
        QA1Date: Qa1Val,
        QA2: $("#QA2").val(),
        QA2Date: Qa2Val,
        SecondCustomerNo: $("#SecondCustomerNo").val(),
        AdditionalCustomerNo: $("#AdditionalCustomerNo").val(),
        FirstBilling: GetTimeFormat($("#FirstBilling").val()),
        StreetType: $("#StreetType").val(),
        Appartment: $("#Appartment").val(),
        CrossStreet: $("#CrossStreet").val(),
        DBA: $("#DBA").val(),
        PreferedEmail: $("#PreferedEmail").is(":checked"),
        PreferedSms: $("#PreferedSms").is(":checked"),
        IsAgreement: $("#IsAgreement").is(":checked"),
        IsFireAccount: $("#IsFireAccount").is(":checked"),
        AuthorizeDescription: $("#AuthDescription").val(),
        Note: $("#Note").val(),
        BusinessAccountType: $("#BusinessAccountType").val(),
        Ownership: $("#Ownership").val(),
        PurchasePrice: $("#PurchasePrice").val(),
        ContractValue: $("#ContractValue").val(),
        AssignedTo: $("#AssignedTo").val(),
        CompletionDate: GetTimeFormat($("#Ticket_CompletionDate").val()),
        PaymentIncress: PaymentIncress,
        Passcode: Passcode
    };

    var PaymentInfo;
    if ($("#PaymentMethod").val() == "Credit Card") {
        PaymentInfo = {
            Id: $("#debit_payment_info_id").val(),
            CardNumber: $("#credit_card_number").val(),
            CardSecurityCode: $("#credit_card_CardSecurityCode").val(),
            CardExpireDate: $("#credit_card_expireDate").val(),
            AccountName: $("#Credit-Card_AccountName").val()
        };
    }
    else if ($("#PaymentMethod").val() == "ACH") {
        PaymentInfo = {
            Id: $("#ach_payment_info_id").val(),
            AccountName: $("#ACH_AccountName").val(),
            BankName: $("#ACH_BankName").val(),
            RoutingNo: $("#ACH_RoutingNo").val(),
            AcountNo: $("#ACH_AcountNo").val(),
            BankAccountType: $("#ACH_BankAccountType").val(),
            ECheckType: $("#ACH_ECheckType").val(),
        };
    }
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
    }
    var passparam = JSON.stringify({
        'customer': param,
        'Token': token,
        'systemInfo': systemInfo,
        'apiAlarm': settingApiAlarm,
        'apiMoni': settingApiMoni,
        'apiCentral': settingApiCentral,
        'PaymentInfo': PaymentInfo
    });

    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: domainurl + "/CustomerPublic/CheckToken/",
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log("Ki re");
            if (data.result == false) {
                parent.OpenErrorMessageNew("Error!", data.message);
            }
            else if (data.status == true ) {
                var Customerid = data.customerid;
                gid = $("#id").val(Customerid);
                //ARB Subscription details
                if (data.PaymentMethod == "Credit Card" || data.PaymentMethod == "ACH") {
                    if (data.PaymentMethod == "Credit Card" && data.PaymentInfoId > 0) {
                        $("#debit_payment_info_id").val(data.PaymentInfoId);
                    } else if (data.PaymentMethod == "ACH" && data.PaymentInfoId > 0) {
                        $("#ach_payment_info_id").val(data.PaymentInfoId);
                    }
                    if (typeof (data.AuthId) != "undefined" && data.AuthId != "" && data.AuthId != null) {
                        $("#AuthorizeRefId").val(data.AuthId);
                        $("#unsubscribe_to_authorize").removeClass('hidden');
                        $("#subscribe_to_authorize").addClass('hidden');
                    }

                    /*if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
                        parent.OpenConfirmationMessage("Message", data.AuthMessage);
                    }*/
                }
                parent.OpenSuccessMessageNew("Success!", "Customer saved succesfully.Wait for admin approval.", function () {
                    CloseTopToBottomModal();
                });
                //parent.OpenCustomerDetailTab(this);
                //ARB Subscription details ends
            }
            else if (data.status == false && typeof (data.message) != "undefined") {
                parent.OpenErrorMessageNew("Error!", data.message);
            }


        }
    })
}
var gid = 0;
var LastTabid = 0;
var DobDatepicker;
var SalesDatepicker;
var InstallDatepicker;
var CutInDatepicker;
var FundingDatepicker;
var QA1datepicker;
var QA2datepicker;
var FirstBillingDatePicker;
var FundedCustomer;
var Maintance;

var LoadScheduleCalendar = function () {
    //GuidCustomer = $('#CustomerList').val();
    //console.log(GuidCustomer);
    var Param = {
        "Id": $("#Ticket_Id").val(),
        "TicketId": $("#Ticket_TicketId").val(),
        "Subject": $("#Ticket_Subject").val(),
        "Message": tinyMCE.get('BodyContent').getContent(),
        "Status": $("#Ticket_Status").val(),
        "CustomerId": GuidCustomer,
        "TicketType": $("#Ticket_TicketType").val(),
        "CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "AppointmentStartTime": $("#AppointmentStartTime").val(),
        "AppointmentEndTime": $("#AppointmentEndTime").val(),
    };
    var url = domainurl + "/Ticket/SaveTicketSession";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                var pathname = window.location.pathname.toLowerCase();
                if (pathname == '/scheduleinfo') {
                    window.location.href = "/ScheduleInfo?date=" + $("#Ticket_CompletionDate").val() + "&viewtype=" + "Daily" + "&TicketId=" + $("#Ticket_Id").val() + "&CustomerId=" + GuidCustomer;
                }
                else {
                    window.location.href = "/calendar?ticketdate=" + $("#Ticket_CompletionDate").val() + "&ticketid=" + $("#Ticket_Id").val() + "&customerid=" + GuidCustomer;
                }
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}




var GetTimeFormat = function (date) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return new Date(date + ' ' + time)
}

var ScrollToError = function () {
    if ($(".required").length > 0) {
        $('.add_customer_wrapper').animate({
            scrollTop: ($('.add_customer_wrapper').scrollTop() + $(".required").offset().top - 100)
        }, 2000);
    }
}


var AuthorizeParamReady = function () {
    var PaymentInfo;
    
    var dataCheck = false;
    if ($("#PaymentMethod").val() == "Credit Card") {
        PaymentInfo = {
            Id: $("#debit_payment_info_id").val(),
            CardNumber: $("#credit_card_number").val(),
            CardSecurityCode: $("#credit_card_CardSecurityCode").val(),
            CardExpireDate: $("#credit_card_expireDate").val(),
            AccountName: $("#Credit-Card_AccountName").val()
        };
        if ($("#credit_card_number").val() != ""
            && $("#credit_card_CardSecurityCode").val() != ""
            && $("#credit_card_expireDate").val() != ""
            && $("#Credit-Card_AccountName").val() != "") {
            dataCheck = true;
        }
    }
    else if ($("#PaymentMethod").val() == "ACH") {
        PaymentInfo = {
            Id: $("#ach_payment_info_id").val(),
            AccountName: $("#ACH_AccountName").val(),
            BankName: $("#ACH_BankName").val(),
            RoutingNo: $("#ACH_RoutingNo").val(),
            AcountNo: $("#ACH_AcountNo").val(),
            BankAccountType: $("#ACH_BankAccountType").val(),
            ECheckType: $("#ACH_ECheckType").val(),
        };

        if ($("#ach_payment_info_id").val() != ""
            && $("#ACH_AccountName").val() != ""
            && $("#ACH_BankName").val() != ""
            && $("#ACH_RoutingNo").val() != ""
            && $("#ACH_AcountNo").val() != ""
            && $("#ACH_BankAccountType").val() != ""
            && $("#ACH_ECheckType").val() != ""
            ) {
            dataCheck = true;
        }
    }
    if (!dataCheck) {
        parent.OpenErrorMessageNew("Error!", "For subscription to authorize.net, you need to select 'Credit Card' or 'ACH' as customers payment method and provide required data.");

        return null;
    }
    var param = {
        id: $("#id").val(),
        CustomerId: CustomerGuidId,
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        EmailAddress: $("#EmailAddress").val(),
        BusinessName: $("#BusinessName").val(),
        ContractTeam: $("#ContractTerm").val(),
        MonthlyMonitoringFee: MonthlyMonitoringFeeVal,
        LeadSource: $("#LeadSource").val(),
        CustomerNo: $("#CustomerNum").val(),
        BillAmount: $("#BillAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillOutStanding: $("#BillOutStanding").val(),
        FirstBilling: GetTimeFormat($("#FirstBilling").val()),
        AuthorizeRefId: $("#AuthorizeRefId").val(),
        AuthorizeDescription: $("#AuthDescription").val()
    };
    var passparam = JSON.stringify({
        'customer': param,
        'PaymentInfo': PaymentInfo
    });
    return passparam;
}

var PaymentMethodValidation = function ()
{
    var result = true;
    $(".payment_control:visible").each(function () {
        if ($(this).val().trim() == "" || $(this).val().trim() == "-1") {
            $(this).addClass('required');
            result = false;
        } else {
            $(this).removeClass('required');
        }
    });
    return result;
}

var BillingStartDayValidation = function () {
    /*This function will check if the start day is in the bill day list*/
    var result = false;
    var dat = new Date($("#FirstBilling").val());
    if ($("#FirstBilling").val() == "") {
        $("#FirstBilling").removeClass("required");
        $("#firstbillingerr").addClass("hidden");
        return true;
    }
    else if (dat == "Invalid Date") {
        $("#FirstBilling").addClass("required");
        $("#firstbillingerr").removeClass("hidden");
        return result;
    }
    $("#BillDay > option").each(function () { 
        if (dat.getDate() == parseInt(this.value)) {
            $("#BillDay").val(this.value);
            $("#FirstBilling").removeClass("required");
            $("#firstbillingerr").addClass("hidden");
            result =  true;
        }
    });
    if (!result) {
        $("#FirstBilling").addClass("required");
        $("#firstbillingerr").removeClass("hidden");
    }
    //return result;//Commenting for data update - Inan-05-15-2018//need to fix again
    return true;
}
var StartDateValidation = function () {
    /*
    This function will check if the start day is less than or equal to Today.
    Only needed when subscribing to authorize.net
    */
    var StartDate = new Date($("#FirstBilling").val());
    if (StartDate == "Invalid Date") {
        return false;
    }
    var todaysDate = new Date();
    todaysDate = new Date(todaysDate.getMonth() + 1 + "/" + todaysDate.getDate() + "/" + todaysDate.getFullYear());

    if (todaysDate <= StartDate) {
        return true;
    }
    return false;
}
function FormatCellNumber(cvalue) {
    var ValueCleanCell = "";
    if (cvalue != undefined && cvalue != "" && cvalue != null) {
        cvalue = cvalue.replace(/[-()  ]/g, '');
        if (cvalue.length == 10) {
            ValueCleanCell = cvalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#CellNo").css({ "border": "1px solid #babec5" });
        }
        else if (cvalue.length > 10) {
            ValueCleanCell = cvalue;
            $("#CellNo").css({ "border": "1px solid red" });
        }
        else {
            $("#CellNo").css({ "border": "1px solid red" });
            ValueCleanCell = cvalue;
        }
    }
    return ValueCleanCell;
}
function FormatSecondaryNumber(svalue) {
    var ValueCleanSecondary = "";
    if (svalue != undefined && svalue != "" && svalue != null) {
        svalue = svalue.replace(/[-()  ]/g, '');
        if (svalue.length == 10) {
            ValueCleanSecondary = svalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#SecondaryPhone").css({ "border": "1px solid #babec5" });
        }
        else if (svalue.length > 10) {
            ValueCleanSecondary = svalue;
            $("#SecondaryPhone").css({ "border": "1px solid red" });
        }
        else {
            $("#SecondaryPhone").css({ "border": "1px solid red" });
            ValueCleanSecondary = svalue;
        }
    }
    return ValueCleanSecondary;
}

function FormatAlarmPhoneNumber(svalue) {
    var ValueCleanAlarmPhone = "";
    if (svalue != undefined && svalue != "" && svalue != null) {
        svalue = svalue.replace(/[-()  ]/g, '');
        if (svalue.length == 10) {
            ValueCleanAlarmPhone = svalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#Alarm_Phone").css({ "border": "1px solid #babec5" });
        }
        else if (svalue.length > 10) {
            ValueCleanAlarmPhone = svalue;
            $("#Alarm_Phone").css({ "border": "1px solid red" });
        }
        else {
            $("#Alarm_Phone").css({ "border": "1px solid red" });
            ValueCleanAlarmPhone = svalue;
        }
    }
    return ValueCleanAlarmPhone;
}

function FormatePhoneNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#PrimaryPhone").css({ "border": "1px solid #babec5" }); 
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#PrimaryPhone").css({ "border": "1px solid red" }); 
        }
        else {
            $("#PrimaryPhone").css({ "border": "1px solid red" }); 
            ValueClean = Value;
        }
    }
    return ValueClean;
}
function FormateSSNNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-  ]/g, '');
        if (Value.length == 9) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{2})\-?(\d{4})/, "$1-$2-$3");
            $("#SSN").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 9) {
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
    var monitoringfee = MonthlyMonitoringFeeVal
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
            var addval = taxvalue * 3;
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
            var addval = parseFloat(monitoringfee) * 3;
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
    var monitoringfee =MonthlyMonitoringFeeVal
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
            var addval = taxvalue * 3;
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
            var addval = parseFloat(monitoringfee) * 3;
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

    var monitoringfee = MonthlyMonitoringFeeVal
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
            var addval = taxvalue * 3;
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
            var addval = parseFloat(monitoringfee) * 3;
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
var initDocReady = function () {

    DobDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        yearRange: [1920, 2010],
        field: $('.dob-datepicker')[0]
    });
    SalesDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#SalesDate')[0]
    });
    InstallDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#InstallDate')[0]
    });
    CutInDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CutInDate')[0]
    });
    QA1datepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('.QA1datepicker')[0]
    });
    QA2datepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('.QA2datepicker')[0]
    });
    FirstBillingDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#FirstBilling')[0]
    });


    if ($("#PaymentMethod").val() == "Credit Card") {
        $(".Customer_Debit_Div").removeClass('hidden');
        $(".Customer_ACH_Div").addClass('hidden');
        $(".Customer_Notes_div").addClass('hidden');

    } else if ($("#PaymentMethod").val() == "ACH") {
        $(".Customer_ACH_Div").removeClass('hidden');
        $(".Customer_Notes_div").addClass('hidden');
        $(".Customer_Debit_Div").addClass('hidden');
    } else {
        $(".Customer_Notes_div").removeClass('hidden');
        $(".Customer_ACH_Div").addClass('hidden');
        $(".Customer_Debit_Div").addClass('hidden');
    }
}


$(document).ready(function () {
    $("#btnSchedule").click(function () {
        LoadScheduleCalendar();
    })
    $("#CustomerFunded").change(function () {
        if ($("#CustomerFunded").val() == "1") {
            FundedCustomer = true;
        }
        else {
            FundedCustomer = false;
        }
    })
    $("#Maintenance").change(function () {
        if ($("#Maintenance").val() == "1") {
            Maintance = true;
        }
        else {
            Maintance = false;
        }
    })
   
    $("#CustomerNum").keyup(function () {
        $(".customerNoSuggestion").show();
        console.log("AddCustomerNumber");
        var url = domainurl + "/Customer/CustomerSystemNumber/";
        var param = {
            KeyValue: $("#CustomerNum").val()
        };
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                console.log("data" + data);
                var template = "";
                if (data != "No Records Found") {
                    for (var i in data) {
                        if ($("#SecondCustomerNo").val() != data[i].CustomerNo && $("#AdditionalCustomerNo").val() != data[i].CustomerNo) {
                            var Onclick = "onclick='SetFCuNo( " + data[i].CustomerNo + ")'";


                            template += '<div class="listsystemno" id="CustomerNumber">' + data[i].CustomerNo + "</div>";
                            //var dataval = data[i].CustomerNo;
                        }
                    }
                    $(".customerNoSuggestion").html(template);
                    $('.listsystemno').on('click', function () {
                        $("#CustomerNum").val($(this).text());
                        setTimeout(function () {
                            $(".customerNoSuggestion").hide();
                        }, 200);
                    })
                }
                else {
                    $(".customerNoSuggestion").html(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    $("#SecondCustomerNo").keyup(function () {
        $(".customerNoSuggestion2").show();
        var url = domainurl + "/Customer/CustomerSystemNumber/";
        var param = {
            KeyValue: $("#SecondCustomerNo").val()
        };
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                console.log("data" + data);
                var template = "";
                if (data != "No Records Found") {
                    for (var i in data) {
                        if ($("#CustomerNum").val() != data[i].CustomerNo && $("#AdditionalCustomerNo").val() != data[i].CustomerNo) {
                            var Onclick = "onclick='SetSecCuNo( " + data[i].CustomerNo + ")'";
                            template += '<div class="listsystemno1" id="CustomerNumber1">' + data[i].CustomerNo + '</div>';
                            //var dataval = data[i].CustomerNo;
                        }
                    }
                    $(".customerNoSuggestion2").html(template);
                    $(".listsystemno1").click(function () {
                        $("#SecondCustomerNo").val($(this).text());
                        setTimeout(function () {
                            $(".customerNoSuggestion2").hide();
                        }, 200);

                    })
                }
                else {
                    $(".customerNoSuggestion2").html(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    $("#AdditionalCustomerNo").keyup(function () {
        $(".customerNoSuggestion3").show();
        var url = domainurl + "/Customer/CustomerSystemNumber/";
        var param = {
            KeyValue: $("#AdditionalCustomerNo").val()
        };
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                console.log("data" + data);
                var template = "";
                if (data != "No Records Found") {
                    for (var i in data) {
                        if ($("#SecondCustomerNo").val() != data[i].CustomerNo && $("#CustomerNum").val() != data[i].CustomerNo) {
                            var Onclick = "onclick='SetAddCuNo( " + data[i].CustomerNo + ")'";
                            template += '<div class="listsystemno2" id="CustomerNumber2" > ' + data[i].CustomerNo + '</div>';
                            //var dataval = data[i].CustomerNo;
                        }
                    }
                    $(".customerNoSuggestion3").html(template);
                    $(".listsystemno2").click(function () {
                        $("#AdditionalCustomerNo").val($(this).text());
                        setTimeout(function () {
                            $(".customerNoSuggestion3").hide();
                        }, 1800);

                    })
                }
                else {
                    $(".customerNoSuggestion3").html(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    $("#FirstBilling").change(function () {
        /*BillingStartDayValidation();*/
    });
    $("#CustomerNum").blur(function () {
        setTimeout(function () {
            $(".customerNoSuggestion").hide()
        }, 200);
    });
    $("#SecondCustomerNo").blur(function () {
        setTimeout(function () {
            $(".customerNoSuggestion2").hide()
        }, 200);

    });
    $("#AdditionalCustomerNo").blur(function () {
        setTimeout(function () {
            $(".customerNoSuggestion3").hide();
        }, 200);

    });
    $("#PaymentMethod").change(function () {
        console.log("ddd");
        if (paymentMethod == "ACH")
        {
            if ($("#PaymentMethod").val() == "Credit Card")
            {
                OpenConfirmationMessageNew("Payment Method Change", " the monthly fee will increase by $1", function () {
                    PaymentIncress = 1;
                    $("#bAmount").html(parseFloat(BillingAmount)+1);
                }, function () {
                    $("#PaymentMethod").val(paymentMethod);
                    $("#bAmount").html(parseFloat(BillingAmount));
                    if ($("#PaymentMethod").val() == "Credit Card") {
                        $(".Customer_Debit_Div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                     

                    } else if ($("#PaymentMethod").val() == "ACH") {
                        $(".Customer_ACH_Div").removeClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');
                     

                    } else {
                        $(".Customer_Notes_div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');

                        $("#subscribe_to_authorize").addClass("hidden");
                        $("#unsubscribe_to_authorize").addClass("hidden");
                    }
                })
            }
        }

        if (paymentMethod == "Credit Card") {
            if ($("#PaymentMethod").val() == "ACH") {

                OpenConfirmationMessageNew("Payment Method Change", " the monthly fee will decrease by $1", function () {
                    PaymentIncress = -1;
                    $("#bAmount").html("");
                    $("#bAmount").html(parseFloat(BillingAmount) - 1);
                }, function () {
                    $("#PaymentMethod").val(paymentMethod);
                    $("#bAmount").html(parseFloat(BillingAmount));
                    if ($("#PaymentMethod").val() == "Credit Card") {
                        $(".Customer_Debit_Div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                      

                    } else if ($("#PaymentMethod").val() == "ACH") {
                        $(".Customer_ACH_Div").removeClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');
                      

                    } else {
                        $(".Customer_Notes_div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');

                        $("#subscribe_to_authorize").addClass("hidden");
                        $("#unsubscribe_to_authorize").addClass("hidden");
                    }
                })
            }
        }
        if (paymentMethod == "ACH") {
            if ($("#PaymentMethod").val() == "Invoice") {
                OpenConfirmationMessageNew("Payment Method Change", " the monthly fee will increase by $2", function () {
                    PaymentIncress = 2;
                    $("#bAmount").html(parseFloat(BillingAmount) +2);
                }, function () {
                    $("#PaymentMethod").val(paymentMethod);
                    $("#bAmount").html(parseFloat(BillingAmount));
                    if ($("#PaymentMethod").val() == "Credit Card") {
                        $(".Customer_Debit_Div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                     

                    } else if ($("#PaymentMethod").val() == "ACH") {
                        $(".Customer_ACH_Div").removeClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');
                      

                    } else {
                        $(".Customer_Notes_div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');

                        $("#subscribe_to_authorize").addClass("hidden");
                        $("#unsubscribe_to_authorize").addClass("hidden");
                    }
                })
            }
        }

        if (paymentMethod == "Invoice") {
            if ($("#PaymentMethod").val() == "ACH") {
                OpenConfirmationMessageNew("Payment Method Change", " the monthly fee will decrease by $2", function () {
                    PaymentIncress = -2;
                    $("#bAmount").html(parseFloat(BillingAmount) - 2);
                }, function () {
                    $("#PaymentMethod").val(paymentMethod);
                    $("#bAmount").html(parseFloat(BillingAmount));
                    if ($("#PaymentMethod").val() == "Credit Card") {
                        $(".Customer_Debit_Div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                       

                    } else if ($("#PaymentMethod").val() == "ACH") {
                        $(".Customer_ACH_Div").removeClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');
                       

                    } else {
                        $(".Customer_Notes_div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');

                        $("#subscribe_to_authorize").addClass("hidden");
                        $("#unsubscribe_to_authorize").addClass("hidden");
                    }
                })
            }
        }

        if (paymentMethod == "Credit Card") {
            if ($("#PaymentMethod").val() == "Invoice") {
                OpenConfirmationMessageNew("Payment Method Change", " the monthly fee will increase by $1", function () {
                    PaymentIncress = 1;
                    $("#bAmount").html(parseFloat(BillingAmount) + 1);
                }, function () {
                    $("#PaymentMethod").val(paymentMethod);
                    $("#bAmount").html(parseFloat(BillingAmount));
                    if ($("#PaymentMethod").val() == "Credit Card") {
                        $(".Customer_Debit_Div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                     

                    } else if ($("#PaymentMethod").val() == "ACH") {
                        $(".Customer_ACH_Div").removeClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');
                     

                    } else {
                        $(".Customer_Notes_div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');

                        $("#subscribe_to_authorize").addClass("hidden");
                        $("#unsubscribe_to_authorize").addClass("hidden");
                    }
                })
            }
        }

        if (paymentMethod == "Invoice") {
            if ($("#PaymentMethod").val() == "Credit Card") {
                OpenConfirmationMessageNew("Payment Method Change", " the monthly fee will decrease by $1", function () {
                    PaymentIncress = -1;
                    $("#bAmount").html(parseFloat(BillingAmount) -1);
                }, function () {
                    $("#PaymentMethod").val(paymentMethod);
                    $("#bAmount").html(parseFloat(BillingAmount));
                    if ($("#PaymentMethod").val() == "Credit Card") {
                        $(".Customer_Debit_Div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                      

                    } else if ($("#PaymentMethod").val() == "ACH") {
                        $(".Customer_ACH_Div").removeClass('hidden');
                        $(".Customer_Notes_div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');
                       

                    } else {
                        $(".Customer_Notes_div").removeClass('hidden');
                        $(".Customer_ACH_Div").addClass('hidden');
                        $(".Customer_Debit_Div").addClass('hidden');

                        $("#subscribe_to_authorize").addClass("hidden");
                        $("#unsubscribe_to_authorize").addClass("hidden");
                    }
                })
            }
        }

        if ($("#PaymentMethod").val() == "Credit Card") {
            $(".Customer_Debit_Div").removeClass('hidden');
            $(".Customer_ACH_Div").addClass('hidden');
            $(".Customer_Notes_div").addClass('hidden');
         

        } else if ($("#PaymentMethod").val() == "ACH") {
            $(".Customer_ACH_Div").removeClass('hidden');
            $(".Customer_Notes_div").addClass('hidden');
            $(".Customer_Debit_Div").addClass('hidden');
           
             
        } else {
            $(".Customer_Notes_div").removeClass('hidden');
            $(".Customer_ACH_Div").addClass('hidden');
            $(".Customer_Debit_Div").addClass('hidden');

            $("#subscribe_to_authorize").addClass("hidden");
            $("#unsubscribe_to_authorize").addClass("hidden");
        }

    });
    $("#BillCycle").change(function () {
        BillAmountBillCycleChangeEffect();
    });
    $("#MonthlyMonitoringFee").change(function () {
        BillAmountMonthlyMonitoringFeeChangeEffect();
    })
    $("#BillTax").change(function () {
        BillAmountBillTaxChangeEffect();
    })
    $("#PrimaryPhone").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).val(cleanPhoneNumber);
        }
    });
    $("#SecondaryPhone").keyup(function () {
        var sPhoneNumber = $(this).val();
        if (sPhoneNumber != undefined && sPhoneNumber != null && sPhoneNumber != "") {
            var scleanPhoneNumber = FormatSecondaryNumber(sPhoneNumber);
            $(this).val(scleanPhoneNumber);
        }
    });
    $("#Alarm_Phone").keyup(function () {
        var aPhoneNumber = $(this).val();
        if (aPhoneNumber != undefined && aPhoneNumber != null && aPhoneNumber != "") {
            var acleanPhoneNumber = FormatAlarmPhoneNumber(aPhoneNumber);
            $(this).val(acleanPhoneNumber);
        }
    });
    $("#CellNo").keyup(function () {
        var cPhoneNumber = $(this).val();
        if (cPhoneNumber != undefined && cPhoneNumber != null && cPhoneNumber != "") {
            var ccleanPhoneNumber = FormatCellNumber(cPhoneNumber);
            $(this).val(ccleanPhoneNumber);
        }
    }); 
    $("#SSN").keyup(function () {
        var SSNNumber = $(this).val();
        if (SSNNumber != undefined && SSNNumber != null && SSNNumber != "") {
            var cleanSSNNumber = FormateSSNNumber(SSNNumber);
            $(this).val(cleanSSNNumber);
        }
    });
    $('#LastName').keyup(function () {
        var CheckLastName = $("#LastName").val();
        if ($.trim(CheckLastName) == '') {
            $("#LastName").css({ "border": "1px solid red" });
        }
        else {
            $("#LastName").css({ "border": "1px solid #babec5" });
        }
    });
    $('#FirstName').keyup(function () {
        var CheckFirstName = $("#FirstName").val();
        if ($.trim(CheckFirstName) == '') {
            $("#FirstName").css({ "border": "1px solid red" });
        }
        else {
            $("#FirstName").css({ "border": "1px solid #babec5" });
        }
    });
    $("#SaveCustomer").click(function () { 
        if (CommonUiValidation() /*&& BillingStartDayValidation()*/) {
            SaveCustomer();
            //OpenSuccessMessageNew("Success!", "Customer saved succesfully.");
            //CloseTopToBottomModal();
        }else{
            ScrollToError();
        }
    });
    $("#SaveCustomerDraft").click(function () {
        if (CommonUiValidation() /*&& BillingStartDayValidation()*/) {
            SaveCustomerDraft();
            //OpenSuccessMessageNew("Success!", "Customer saved succesfully.");
            //CloseTopToBottomModal();
        } else {
            ScrollToError();
        }
    });
    $("#Type").change(function () {
        if ($(this).val() == "Commercial"){
            $("#BusinessName").attr('datarequired', 'true');
        }   
        else {
            $("#BusinessName").attr('datarequired', 'false');
        } 
    });
    setTimeout(function () {
        initDocReady();
    }, 1000);
});

