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
var CycleStartingDatePicker;
var FundedCustomer;
var Maintance;
var MovDatePicker;
var ContractStartDatePicker;
var OriginalContractDatePicker;
var CustomerSinceDatePicker;
var ResignDatePicker;
var ApprovalDateDatePicker;
if (typeof (billtax) != "undefined" && billtax != null && billtax != "" && billtax != "-1") {
    if (billtax.toLocaleLowerCase() == "true") {
        billtax = "1"
    }
    else {
        billtax = "0"
    }
    $("#BillTax").val(billtax);
}
function FormateFax(Value) {
    var CleanValue = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            CleanValue = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#Fax").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 10) {
            CleanValue = Value;
            $("#Fax").css({ "border": "1px solid red" });
        }
        else {
            $("#Fax").css({ "border": "1px solid red" });
            CleanValue = Value;
        }
    }
    return CleanValue;
}
var MapLoad = function () {
    var LoadUrl = domainurl + "/Customer/GeeseCustomerAddressMap";
    $(".AddressMap").load(LoadUrl);
}
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
var ConditionHeight = 600;
if (window.innerHeight < 600) {
    ConditionHeight = window.innerHeight;
}
var idlist = [{ id: ".GenerateProrateBillCustomer", type: 'iframe', width: ConditionHeight, height: 500 }
];
jQuery.each(idlist, function (i, val) {
    magnificPopupObj(val);
});
var GetTimeFormat = function (date) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return new Date(date + ' ' + time)
}
var OpenARBSubscriptionForm = function () {
    var CurrentCustomerId = $("#id").val();
    $("#ARBSubscriptionForm").attr('href', domainurl + '/Customer/SubscribeToAuthorize/?CustomerId=' + CurrentCustomerId);
    $("#ARBSubscriptionForm").trigger('click');
}
var ScrollToError = function () {
    if ($(".required").length > 0) {
        $('.add_customer_wrapper_custom').animate({
            scrollTop: ($('.add_customer_wrapper_custom').scrollTop() + $(".required").offset().top - 100)
        }, 2000);
    }
}


var AuthorizeParamReady = function () {
    var PaymentInfo;
    console.log("Hi");
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
    else if ($("#PaymentMethod").val() != "-1") {
        PaymentInfo = {
            Id: $("#PaymentMethod").val(),
            AccountName: "ProfilePackage",
        };
        dataCheck = true;

    }
    if (!dataCheck) {
        parent.OpenErrorMessageNew("Error!", "For subscription to authorize.net, you need to select 'Credit Card' or 'ACH' as customers payment method and provide required data.");

        return null;
    }
    var ContractTeam = $("#ContractTerm").val(); 
    if (ContractTeam == "Custom") {
        ContractTeam = "";
        var CustomContactTerm = $(".CustomContactTerm").val();
        if (CustomContactTerm != "" && CustomContactTerm != undefined) {
            ContractTeam = parseInt(CustomContactTerm) / parseInt(12);
        }
    }
    var param = {
        id: $("#id").val(),
        CustomerId: CustomerGuidId,
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        EmailAddress: $("#EmailAddress").val(),
        BusinessName: $("#BusinessName").val(),
        ContractTeam: ContractTeam,
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        LeadSourceType: $("#LeadSourceType").val(),
        CustomerNo: $("#CustomerNum").val(),
        BillAmount: $("#BillAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillOutStanding: $("#BillOutStanding").val(),
        //FirstBilling: GetTimeFormat($("#FirstBilling").val()),
        StrFirstBilling: $("#FirstBilling").val(),
        AuthorizeRefId: $("#AuthorizeRefId").val(),
        AuthorizeDescription: $("#AuthDescription").val()
    };
    var passparam = JSON.stringify({
        'customer': param,
        'PaymentInfo': PaymentInfo
    });
    return passparam;
}

var PaymentMethodValidation = function () {
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





var SubscribeToAlarm = function () {

    var url = "/API/IntegrateToAlarm/";
    var param = {
        CustomerId: CustomerGuidId,
        FirstName: $("#FirstName").val(),
        Lastname: $("#LastName").val(),
        Street1: $("#Street").val(),
        City: $("#City").val(),
        Zip: $("#ZipCode").val(),
        State: $("#State").val(),
        PropertyType: $("#Alarm_PropertyType").val(),
        EmailAddress: $("#Alarm_EmailAddress").val(),
        Phone: $("#Alarm_Phone").val(),
        DealerCustomer: $("#Alarm_DealerCustomer").val(),
        LoginName: $("#Alarm_LoginName").val(),
        Password: $("#Alarm_Password").val(),
        SameInsAddress: $("input[name='SameInstallationAddress']:checked").val(),
        InsStreet: $("#Alarm_InsStreet").val(),
        InsCity: $("#Alarm_InsCity").val(),
        InsState: $("#Alarm_InsState").val(),
        InsZip: $("#Alarm_InsZip").val(),
        Culture: $("#Alarm_Culture").val(),
        PanelType: $("#Alarm_PanelType").val(),
        PanelVersion: $("#Alarm_PanelVersion").val(),
        ModelSerialNumber: $("#Alarm_ModelSerialNumber").val(),
        PhoneLinePresent: $("#Alarm_PhoneLinePresent").is(":checked"),
        IgnoreLowCoverageError: $("#Alarm_IgnoreLowCoverageError").is(":checked"),
        CentralStationAccountNo: $("#Alarm_CentralStationAccountNo").val(),
        CentrastationForwardingOption: $("#Alarm_CentrastationForwardingOption").val(),
        CentralStationRecieverNumber: $("#Alarm_CentralStationRecieverNumber").val(),
        CentralStationName: $("#CentralStationName").val(),
        PackageId: $("#Alarm_PackageId").val(),
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
            if (typeof (data) != 'undefined') {
                if (data) {
                    OpenSuccessMessageNew("Success!", data.message, function () {
                        /*Hide add button*/
                        $("#SubscribeToAlarm").addClass("hidden");
                    });
                } else {
                    OpenErrorMessageNew("Error!", data.message, function () {

                    });
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var CreateLeadConfirm = function (CustomerId) {
    //OpenConfirmationMessageNew("Confirmation", MoveText + "<br><input type='checkbox' id='eqp_checkbox' /> <span>Equipment</span><input type='checkbox' id='ser_checkbox' /> <span>Service</span><input type='checkbox' id='bill_checkbox' /> <span>Billing</span>", function () {
    //    CreateLeadFromCustomer(CustomerId);
    //})
    OpenTopToBottomModal("/Customer/LoadMoveCustomerServiceEqpBilling?CustomerId=" + CustomerId + "&type=Move");
}

var TransferLeadConfirm = function (CustomerId) {
    //var transfertext = "Do you want to transfer this customer into lead?";
    //OpenConfirmationMessageNew("Confirmation", transfertext + "<br><input type='checkbox' id='eqp_checkbox' /> <span>Equipment</span><input type='checkbox' id='ser_checkbox' /> <span>Service</span>", function () {
    //    $.ajax({
    //        type: "POST",
    //        ajaxStart: $(".loader-div").show(),
    //        url: "/Customer/TransferCustomerToLead",
    //        data: JSON.stringify({ CustomerId: CustomerId, 'equipment': $("#eqp_checkbox").prop('checked'), 'service': $("#ser_checkbox").prop('checked'), }),
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        cache: false,
    //        success: function (data) {
    //            if (data.result) {
    //                window.location.href = domainurl + "/Lead/Leadsdetail/?id=" + data.id;
    //            }
    //        },
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            $(".loader-div").hide();
    //            console.log(errorThrown);
    //        }
    //    });
    //})
    OpenTopToBottomModal("/Customer/LoadMoveCustomerServiceEqpBilling?CustomerId=" + CustomerId + "&type=Transfer");
}


var CreateLeadFromCustomer = function (CustomerId) {

    var url = "/Customer/CreateLeadFromCustomer";
    var DobVal = DobDatepicker.getDate();
    var CurrentDate = new Date();
    var SalesVal = $("#SalesDate").val();
    var InstallVal = $("#InstallDate").val();
    var CutinVal = $("#CutInDate").val();
    var ContractStartVal = $("#CustomerExtended_ContractStartDate").val();
    var CustomerSinceVal = $("#CustomerExtended_CustomerSince").val();
    var ResignDateVal = $("#CustomerExtended_ResignDate").val();
    var OriginalContractVal = $("#OriginalContractDate").val();
    var FundingVal = $("#CustomerFundedDate").val();

    if (DobVal != null && DobVal.getUTCDate() == CurrentDate.getUTCDate()
        && DobVal.getUTCMonth() == CurrentDate.getUTCMonth()
        && DobVal.getUTCFullYear() == CurrentDate.getUTCFullYear()) {
        DobVal = "";
    }

    var ContractTeam = $("#ContractTerm").val();
    if (ContractTeam == "Custom") {
        ContractTeam = "";
        var CustomContactTerm = $(".CustomContactTerm").val();
        if (CustomContactTerm != "" && CustomContactTerm != undefined) {
            ContractTeam = parseInt(CustomContactTerm) / parseInt(12);
        }
    }
    console.log(CustomerId);
    var param = {
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        MoveCustomerId: CustomerId,
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $("#EmailAddress").val(),
        Type: $("#Type").val(),
        CustomerAccountTypeList: $("#CustomerAccountType").val(),
        PreferredContactMethod: $("#PreferredContactMethod").val(),
        DateofBirth: DobVal,
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        Country: $("#Country").val(),
        County: $("#County").val(),
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
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CustomerCreditScore").val(),
        ContractTeam: ContractTeam,
        FundingCompany: $("#FundingCompany").val(),
        LeadSource: $("#LeadSource").val(),
        LeadSourceType: $("#LeadSourceType").val(),
        Note: $("#Note").val(),
        SSN: $("#SSN").val(),
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        MiddleName: $("#MiddleName").val(),
        StreetType: $("#StreetType").val(),
        Appartment: $("#Appartment").val(),
        CrossStreet: $("#CrossStreet").val(),
        RenewalTerm : $("#RenewalTerm").val(),
        DBA: $("#DBA").val(),
        OriginalContactDate: OriginalContractVal,
        PreferedEmail: false,
        PreferedSms: false,
        PreferedCall: false,
        IsAgreement: false,
        Note: $("#Note").val(),
        RouteId: $("#Route").val(),
        Latlng: $("#LatLng").val(),
        "CustomerExtended.SalesPerson4": $("#CustomerExtended_SalesPerson4").val(),
        "CustomerExtended.Batch": $("#CustomerExtended_Batch").val(),
        "CustomerExtended.MonthlyBatch": $("#CustomerExtended_MonthlyBatch").val(),
        "CustomerExtended.BrinksFundingStatus": $("#CustomerExtended_BrinksFundingStatus").val(),
        "CustomerExtended.FinanceFundingStatus": $("#CustomerExtended_FinanceFundingStatus").val(),
        "CustomerExtended.FinanceRep": $("#CustomerExtended_FinanceRep").val(),
        "CustomerExtended.AppoinmentSetBy": $("#CustomerExtended_AppoinmentSetBy").val(),
        "CustomerExtended.FinanceCompany": $("#CustomerExtended_FinanceCompany").val(),
        "CustomerExtended.ContractStartDate": ContractStartVal,
        "CustomerExtended.CustomerSince": CustomerSinceVal,
        "CustomerExtended.ResignDate": ResignDateVal,
        "CustomerExtended.IsFinanced": $("#CustomerExtended_IsFinanced").val(),
        "CustomerExtended.Pets": $("#CustomerExtended_Pets").val(),
        "CustomerExtended.PetsType": $("#CustomerExtended_PetsType").val(),
        "CustomerExtended.Repair": $("#CustomerExtended_Repair").val(),
        "CustomerExtended.VipClubMember": $("#CustomerExtended_VipClubMember").val(),
        "CustomerExtended.RepairType": $("#CustomerExtended_TypeOfRepair").val(),
        "CustomerExtended.BirthDateCoupon": $("#BirthDateCoupon").val(),
        "CustomerExtended.RWST1": $("#CustomerExtended_RWST01").val(),
        "CustomerExtended.RWST2": $("#CustomerExtended_RWST02").val(),
        "CustomerExtended.RWST3": $("#CustomerExtended_RWST03").val(),
        "CustomerExtended.RWST4": $("#CustomerExtended_RWST04").val(),
        "CustomerExtended.RWST5": $("#CustomerExtended_RWST05").val(),
        "CustomerExtended.RWST6": $("#CustomerExtended_RWST06").val(),
        "CustomerExtended.RWST7": $("#CustomerExtended_RWST07").val(),
        "CustomerExtended.RWST8": $("#CustomerExtended_RWST08").val(),
        "CustomerExtended.RWST9": $("#CustomerExtended_RWST09").val(),
        "CustomerExtended.RWST10": $("#CustomerExtended_RWST10").val(),
        "CustomerExtended.RWST11": $("#CustomerExtended_RWST11").val(),
        "CustomerExtended.RWST12": $("#CustomerExtended_RWST12").val(),
        "CustomerExtended.RWST13": $("#CustomerExtended_RWST13").val(),
        "CustomerExtended.RWST14": $("#CustomerExtended_RWST14").val(),
        "CustomerExtended.RWST15": $("#CustomerExtended_RWST15").val(),
        "CustomerExtended.RepsAssignedDate": $("#RepsAssignedDate").val(),
        "CustomerExtended.GeeseCount": $("#GeeseCount").val()
    };
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify({
            'customer': param,
            'equipment': $("#eqp_checkbox").prop('checked'),
            'service': $("#ser_checkbox").prop('checked'),
            'billing': $("#bill_checkbox").prop('checked'),
            'OldCustomerId': Id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {

                OpenSuccessMessageNew("Success !", data.message, function () {

                    if (data.leadId != null) {

                        parent.LoadLeadsDetail(data.leadId, true);
                        CloseTopToBottomModal();

                    }
                });
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var SaveCustomer = function (isgenerateprorate) {
    var SSN = $("#SSN").val();
    var SSN2 = $("#SSN2").val();
    var SSN2Clean = typeof SSN2 == "undefined" ? "" : SSN2.replace(/[-  ]/g, '');
    if (SSN2Clean.length == 9) {
        SSN = SSN2;
    }
    var url = domainurl + "/Customer/AddCustomer/";

    var SalesVal = $("#SalesDate").val();
    var InstallVal = $("#InstallDate").val();
    var CutinVal = $("#CutInDate").val();
    var ContractStartVal = $("#CustomerExtended_ContractStartDate").val();
    var CustomerSinceVal = $("#CustomerExtended_CustomerSince").val();
    var ResignDateVal = $("#CustomerExtended_ResignDate").val();
    var OriginalContractVal = $("#OriginalContractDate").val();
    var FundingVal = $("#CustomerFundedDate").val();
    var Qa1Val = QA1datepicker.getDate();
    var Qa2Val = QA2datepicker.getDate();
    var DobVal = DobDatepicker.getDate();
    var CurrentDate = new Date();
    var ContractTeam = $("#ContractTerm").val();
    if (ContractTeam == "Custom") {
        ContractTeam = "";
        var CustomContactTerm = $(".CustomContactTerm").val();
        if (CustomContactTerm != "" && CustomContactTerm != undefined) {
            ContractTeam = parseInt(CustomContactTerm) / parseInt(12);
        }
    }
    var param = {

        id: $("#id").val(),
        CustomerId: CustomerGuidId,
        Title: $("#Title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $("#EmailAddress").val(),
        Type: $("#Type").val(),
        CustomerAccountTypeList: $("#CustomerAccountType").val(),
        PreferredContactMethod: $("#PreferredContactMethod").val(),
        DateofBirth: $("#DateofBirth").val(),
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        Country: $("#Country").val(),
        County: $("#County").val(),
        PrimaryPhone: $("#PrimaryPhone").val().replace(/[^a-zA-Z0-9]/g, ''),
        SecondaryPhone: $("#SecondaryPhone").val(),
        CellNo: $("#CellNo").val().replace(/[^a-zA-Z0-9]/g, ''),
        Fax: $("#Fax").val(),
        CallingTime: $("#CallingTime").val(),
        SalesDate: SalesVal,
        Soldby: $("#SoldBy").val(),
        InstallDate: InstallVal,
        Installer: $("#Installer").val(),
        CutInDate: CutinVal,
        CustomerFundedDate: FundingVal,
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScoreValue").val(),
        CreditScoreValue: $("#CustomerCreditScore").val(),
        ContractTeam: ContractTeam,
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        LeadSourceType: $("#LeadSourceType").val(),
        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: $("#CellularBackup").val(),
        CustomerFunded: FundedCustomer,
        Maintenance: Maintance,
        Note: $("#Note").val(),
        SSN: SSN,
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        CustomerNo: $("#CustomerNum").val(),
        MiddleName: $("#MiddleName").val(),
        ReminderDate: $("#ReminderDate").val(),
        Status: $("#Status").val(),
        CustomerStatus: $("#CustomerStatus").val(),
        BillAmount: $("#BillAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillTax: parseInt($("#BillTax").val()),
        BillOutStanding: $("#BillOutStanding").val(),
        ServiceDate: $("#ServiceDate").val(),
        Area: $("#Area").val(),
        ActivationFee: $("#ActivationFee").val(),
        PhoneType: $("#PhoneType").val(),
        ReferringCustomer: $('#Ref_customer').val(),
        EstCloseDate: $("#EstCloseDate").val(),
        ProjectWalkDate: $("#ProjectWalkDate").val(),
        DoNotCall: $("#DoNotCall").val(),
        ChildOf: $("#Child_customer").val(),
        QA1: $("#QA1").val(),
        QA1Date: Qa1Val,
        QA2: $("#QA2").val(),
        QA2Date: Qa2Val,
        SecondCustomerNo: $("#SecondCustomerNo").val(),
        AdditionalCustomerNo: $("#AdditionalCustomerNo").val(),
        //FirstBilling: GetTimeFormat($("#FirstBilling").val()),
        StrFirstBilling: $("#FirstBilling").val(),
        StreetType: $("#StreetType").val(),
        Appartment: $("#Appartment").val(),
        CrossStreet: $("#CrossStreet").val(),
        RenewalTerm: $("#RenewalTerm").val(),
        DBA: $("#DBA").val(),
        PreferedEmail: $("#PreferedEmail").is(":checked"),
        PreferedSms: $("#PreferedSms").is(":checked"),
        PreferedCall: $("#PreferedCall").is(":checked"),
        IsAgreement: $("#IsAgreement").is(":checked"),
        IsFireAccount: $("#IsFireAccount").is(":checked"),
        AuthorizeDescription: $("#AuthDescription").val(),
        Note: $("#Note").val(),
        BusinessAccountType: $("#BusinessAccountType").val(),
        InstalledStatus: $("#InstalledStatus").val(),

        AcquiredFrom: $("#AcquiredFrom").val(),
        FollowUpDate: $("#FollowUpDate").val(),
        BuyoutAmountByADS: $("#BuyoutAmountByADS").val(),
        BuyoutAmountBySalesRep: $("#BuyoutAmountBySalesRep").val(),
        FinancedTerm: $("#FinancedTerm").val(),
        FinancedAmount: $("#FinancedAmount").val(),
        SoldAmount: $("#SoldAmount").val(),
        Levels: $("#Levels").val(),
        Ownership: $("#Ownership").val(),
        PurchasePrice: (typeof ($("#PurchasePrice").val()) != "undefined") ? $("#PurchasePrice").val().replaceAll(",", "") : "",
        ContractValue: $("#ContractValue").val(),
        AssignedTo: $("#AssignedTo").val(),
        CompletionDate: GetTimeFormat($("#Ticket_CompletionDate").val()),
        Passcode: $("#VarbalPassword").val(),
        BranchId: $("#BranchList").val(),
        AnnualRevenue: (typeof ($("#AnnualRevenue").val()) != "undefined") ? $("#AnnualRevenue").val().replaceAll(",", "") : "",
        Website: $("#Website").val(),
        Market: $(".market").val(),
        Passengers: $("#passengers").val(),
        Budget: (typeof ($(".budget").val()) != "undefined") ? $(".budget").val().replaceAll(",", "") : 0,
        EmailVerified: $("#notverified").hasClass("hidden"),
        IsPrimaryPhoneVerified: $("#isSitePhoneVerified").val(),
        IsSecondaryPhoneVerified: $("#isCellPhoneVerified").val(),
        IsCellNoVerified: $("#isCellNoVerified").val(),
        HomeOwner: $("#homeowner").val(),
        AccessGivenTo: $("#AccessGivenTo").val(),
        SalesLocation: $("#SalesLocation").val(),
        IsReceivePaymentMail: $("#IsReceivePayEmail").prop("checked"),
        CSProvider: $("#CSProvider").val(),
        BestTimeToCall: $("#BestTimeToCall").val(),
        DuplicateCustomer: $("#DuplicateCustomer").val(),
        InspectionCompany: $("#InspectionCompany").val(),
        SoldBy2: $("#SoldBy2").val(),
        SoldBy3: $("#SoldBy3").val(),
        MovingDate: $("#MovingDate").val(),
        ContactedPerviously: $("#ContactedPerviously").val(),
        TaxExemption: $("#TaxExemption").val(),
        AppoinmentSet: $("#AppoinmentSet").val(),
        AgreementEmail: $("#AgreementEmail").val(),
        AgreementPhoneNo: $("#AgreementPhoneNo").val(),
        MapscoNo: $("#MapscoNo").val(),
        Password: $("#Password").val(),
        OriginalContactDate: OriginalContractVal,
        MoveCustomerId: $("#MoveCustomerId").val(),
        RouteId: $("#Route").val(),
        Latlng: $("#LatLng").val(),
        "CustomerExtended.SalesPerson4": $("#CustomerExtended_SalesPerson4").val(),
        "CustomerExtended.Batch": $("#CustomerExtended_Batch").val(),
        "CustomerExtended.DrivingLicense": $("#CustomerExtended_DrivingLicense").val(),
        "CustomerExtended.MonthlyBatch": $("#CustomerExtended_MonthlyBatch").val(),
        "CustomerExtended.BrinksFundingStatus": $("#CustomerExtended_BrinksFundingStatus").val(),
        "CustomerExtended.FinanceFundingStatus": $("#CustomerExtended_FinanceFundingStatus").val(),
        "CustomerExtended.FinanceRep": $("#CustomerExtended_FinanceRep").val(),
        "CustomerExtended.AppoinmentSetBy": $("#CustomerExtended_AppoinmentSetBy").val(),
        "CustomerExtended.FinanceCompany": $("#CustomerExtended_FinanceCompany").val(),
        "CustomerExtended.ContractStartDate": ContractStartVal,
        "CustomerExtended.CustomerSince": CustomerSinceVal,
        "CustomerExtended.ResignDate": ResignDateVal,
        "CustomerExtended.IsFinanced": $("#CustomerExtended_IsFinanced").val(),
        "CustomerExtended.Pets": $("#CustomerExtended_Pets").val(),
        "CustomerExtended.PetsType": $("#CustomerExtended_PetsType").val(),
        "CustomerExtended.Repair": $("#CustomerExtended_Repair").val(),
        "CustomerExtended.VipClubMember": $("#CustomerExtended_VipClubMember").val(),
        "CustomerExtended.RepairType": $("#CustomerExtended_TypeOfRepair").val(),
        "CustomerExtended.BirthDateCoupon": $("#BirthDateCoupon").val(),
        "CustomerExtended.ResignedBy": $("#ResignedBy").val(),
        "CustomerExtended.RWST1": $("#CustomerExtended_RWST01").val(),
        "CustomerExtended.RWST2": $("#CustomerExtended_RWST02").val(),
        "CustomerExtended.RWST3": $("#CustomerExtended_RWST03").val(),
        "CustomerExtended.RWST4": $("#CustomerExtended_RWST04").val(),
        "CustomerExtended.RWST5": $("#CustomerExtended_RWST05").val(),
        "CustomerExtended.RWST6": $("#CustomerExtended_RWST06").val(),
        "CustomerExtended.RWST7": $("#CustomerExtended_RWST07").val(),
        "CustomerExtended.RWST8": $("#CustomerExtended_RWST08").val(),
        "CustomerExtended.RWST9": $("#CustomerExtended_RWST09").val(),
        "CustomerExtended.RWST10": $("#CustomerExtended_RWST10").val(),
        "CustomerExtended.RWST11": $("#CustomerExtended_RWST11").val(),
        "CustomerExtended.RWST12": $("#CustomerExtended_RWST12").val(),
        "CustomerExtended.RWST13": $("#CustomerExtended_RWST13").val(),
        "CustomerExtended.RWST14": $("#CustomerExtended_RWST14").val(),
        "CustomerExtended.RWST15": $("#CustomerExtended_RWST15").val(),
        "CustomerExtended.CSAgreement": $("#CSAgreement").val(),
        "CustomerExtended.RepsAssignedDate": $("#RepsAssignedDate").val(),
        "CustomerExtended.MonthlyFinanceRate": $("#CustomerExtended_MonthlyFinanceRate").val(),
        "CustomerExtended.GrossFundedAmount": $("#CustomerExtended_GrossFundedAmount").val(),
        "CustomerExtended.NetFundedAmount": $("#CustomerExtended_NetFundedAmount").val(),
        "CustomerExtended.DiscountFundedAmount": $("#CustomerExtended_DiscountFundedAmount").val(),
        "CustomerExtended.DiscountFundedPercentage": $("#CustomerExtended_DiscountFundedPercentage").val(),
        "CustomerExtended.CustomerPaymentAmount": $("#CustomerExtended_CustomerPaymentAmount").val(),
        "CustomerExtended.FinanceRepCommissionRate": $("#CustomerExtended_FinanceRepCommissionRate").val(),
        "CustomerExtended.LoanNumber": $("#CustomerExtended_LoanNumber").val(),
        "CustomerExtended.CreditAppNumber": $("#CustomerExtended_CreditAppNumber").val(),
        "CustomerExtended.Term": $("#CustomerExtended_Term").val(),
        "CustomerExtended.APR": $("#CustomerExtended_APR").val(),
        "CustomerExtended.MaxCreditLimit": $("#CustomerExtended_MaxCreditLimit").val(),
        "CustomerExtended.ApprovalDate": $("#CustomerExtended_ApprovalDate").val(),
        "CustomerExtended.GeeseCount": $("#GeeseCount").val(),
        "CustomerExtended.CycleStartDate": $("#CycleStartDate").val(),
        "CustomerExtended.DealerFee": $("#CustomerExtended_DealerFee").val()
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
    else if ($("#PaymentMethod").val() != "-1") {
        PaymentInfo = {
            Id: $("#PaymentMethod").val(),
            AccountName: "ProfilePackage",
        };
    }


    var systemInfo = {
        Id: $("#Idval").val(),
        CustomerId: $("#ValCustomerId").val(),
        PanelType: $("#panelType").val(),
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

        'systemInfo': systemInfo,
        'apiAlarm': settingApiAlarm,
        'apiMoni': settingApiMoni,
        'apiCentral': settingApiCentral,
        'PaymentInfo': PaymentInfo,
        'IsIeatery': IsIeatery,
        'IsProrated': isgenerateprorate,
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
            if (data.status == true) {
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
                    else if (typeof (data.ForteId) != "undefined" && data.ForteId != "" && data.ForteId != null) {

                        $("#unsubscribe_to_forte").removeClass('hidden');
                        $("#subscribe_to_forte").addClass('hidden');
                    }
                    /*if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
                        parent.OpenConfirmationMessage("Message", data.AuthMessage);
                    }*/
                }
                if (data.IsProrated) {
                    $(".GenerateProrateBillCustomer").attr("href", domainurl + "/Customer/GetGenerateProrate/?CustomerId=" + CustomerGuidId);
                    $(".GenerateProrateBillCustomer").click();
                }
                else {
                    parent.OpenSuccessMessageNew("Success!", "Customer saved succesfully.", function () {
                        CloseTopToBottomModal();
                    });
                }

                parent.LoadCustomerDetail(Customerid, true);
                //ARB Subscription details ends
            }
            else if (data.status == false && typeof (data.message) != "undefined") {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var checkToken = function (token) {
    var SalesVal = $("#SalesDate").val();
    var InstallVal = $("#InstallDate").val();
    var CutinVal = $("#CutInDate").val();
    var ContractStartVal = $("#CustomerExtended_ContractStartDate").val;
    var CustomerSinceVal = $("#CustomerExtended_CustomerSince").val();
    var ResignDateVal = $("#CustomerExtended_ResignDate").val();
    var OriginalContractVal = $("#OriginalContractDate").val;
    var FundingVal = $("#CustomerFundedDate").val();
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
    var ContractTeam = $("#ContractTerm").val();
    if (ContractTeam == "Custom") {
        ContractTeam = "";
        var CustomContactTerm = $(".CustomContactTerm").val();
        if (CustomContactTerm != "" && CustomContactTerm != undefined) {
            ContractTeam = parseInt(CustomContactTerm) / parseInt(12);
        }
    }
    var param = {

        id: $("#id").val(),
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $("#EmailAddress").val(),
        Type: $("#Type").val(),
        CustomerAccountTypeList: $("#CustomerAccountType").val(),
        PreferredContactMethod: $("#PreferredContactMethod").val(),
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
        CustomerFundedDate: FundingVal,
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScore").val(),
        ContractTeam: ContractTeam,
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        LeadSourceType: $("#LeadSourceType").val(),
        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: $("#CellularBackup").val(),
        CustomerFunded: FundedCustomer,
        Maintenance: Maintance,
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
        ReferringCustomer: $('#Ref_customer').val(),
        ChildOf: $("#Child_customer").val(),
        QA1: $("#QA1").val(),
        QA1Date: Qa1Val,
        QA2: $("#QA2").val(),
        QA2Date: Qa2Val,
        SecondCustomerNo: $("#SecondCustomerNo").val(),
        AdditionalCustomerNo: $("#AdditionalCustomerNo").val(),
        //FirstBilling: GetTimeFormat($("#FirstBilling").val()),
        StrFirstBilling: $("#FirstBilling").val(),
        StreetType: $("#StreetType").val(),
        Appartment: $("#Appartment").val(),
        CrossStreet: $("#CrossStreet").val(),
        RenewalTerm: $("#RenewalTerm").val(),
        DBA: $("#DBA").val(),
        PreferedEmail: $("#PreferedEmail").is(":checked"),
        PreferedSms: $("#PreferedSms").is(":checked"),
        PreferedCall: $("#PreferedCall").is(":checked"),
        IsAgreement: $("#IsAgreement").is(":checked"),
        IsFireAccount: $("#IsFireAccount").is(":checked"),
        AuthorizeDescription: $("#AuthDescription").val(),
        Note: $("#Note").val(),
        BusinessAccountType: $("#BusinessAccountType").val(),
        InstalledStatus: $("#InstalledStatus").val(),

        AcquiredFrom: $("#AcquiredFrom").val(),
        FollowUpDate: $("#FollowUpDate").val(),
        BuyoutAmountByADS: $("#BuyoutAmountByADS").val(),
        BuyoutAmountBySalesRep: $("#BuyoutAmountBySalesRep").val(),
        FinancedTerm: $("#FinancedTerm").val(),
        FinancedAmount: $("#FinancedAmount").val(),
        SoldAmount: $("#SoldAmount").val(),
        Levels: $("#Levels").val(),
        Ownership: $("#Ownership").val(),
        PurchasePrice: (typeof ($("#PurchasePrice").val()) != "undefined") ? $("#PurchasePrice").val().replaceAll(",", "") : "",
        ContractValue: $("#ContractValue").val(),
        AssignedTo: $("#AssignedTo").val(),
        CompletionDate: GetTimeFormat($("#Ticket_CompletionDate").val()),
        Passcode: $("#VarbalPassword").val(),
        BranchId: $("#BranchList").val(),
        AnnualRevenue: (typeof ($("#AnnualRevenue").val()) != "undefined") ? $("#AnnualRevenue").val().replaceAll(",", "") : "",
        Website: $("#Website").val(),
        Market: $(".market").val(),
        Passengers: $("#passengers").val(),
        Budget: (typeof ($(".budget").val()) != "undefined") ? $(".budget").val().replaceAll(",", "") : 0,
        AgreementEmail: $("#AgreementEmail").val(),
        OriginalContactDate: OriginalContractVal,
        AgreementPhoneNo: $("#AgreementPhoneNo").val(),
        RouteId: $("#Route").val(),
        Latlng: $("#LatLng").val(),
        MapscoNo: $("#MapscoNo").val(),
        "CustomerExtended.SalesPerson4": $("#CustomerExtended_SalesPerson4").val(),
        "CustomerExtended.Batch": $("#CustomerExtended_Batch").val(),
        "CustomerExtended.DrivingLicense": $("#CustomerExtended_DrivingLicense").val(),
        "CustomerExtended.MonthlyBatch": $("#CustomerExtended_MonthlyBatch").val(),
        "CustomerExtended.BrinksFundingStatus": $("#CustomerExtended_BrinksFundingStatus").val(),
        "CustomerExtended.FinanceFundingStatus": $("#CustomerExtended_FinanceFundingStatus").val(),
        "CustomerExtended.FinanceRep": $("#CustomerExtended_FinanceRep").val(),
        "CustomerExtended.AppoinmentSetBy": $("#CustomerExtended_AppoinmentSetBy").val(),
        "CustomerExtended.FinanceCompany": $("#CustomerExtended_FinanceCompany").val(),
        "CustomerExtended.ContractStartDate": ContractStartVal,
        "CustomerExtended.CustomerSince": CustomerSinceVal,
        "CustomerExtended.ResignDate": ResignDateVal,
        "CustomerExtended.IsFinanced": $("#CustomerExtended_IsFinanced").val(),
        "CustomerExtended.Pets": $("#CustomerExtended_Pets").val(),
        "CustomerExtended.PetsType": $("#CustomerExtended_PetsType").val(),
        "CustomerExtended.Repair": $("#CustomerExtended_Repair").val(),
        "CustomerExtended.VipClubMember": $("#CustomerExtended_VipClubMember").val(),
        "CustomerExtended.RepairType": $("#CustomerExtended_TypeOfRepair").val(),
        "CustomerExtended.BirthDateCoupon": $("#BirthDateCoupon").val(),
        "CustomerExtended.RWST1": $("#CustomerExtended_RWST01").val(),
        "CustomerExtended.RWST2": $("#CustomerExtended_RWST02").val(),
        "CustomerExtended.RWST3": $("#CustomerExtended_RWST03").val(),
        "CustomerExtended.RWST4": $("#CustomerExtended_RWST04").val(),
        "CustomerExtended.RWST5": $("#CustomerExtended_RWST05").val(),
        "CustomerExtended.RWST6": $("#CustomerExtended_RWST06").val(),
        "CustomerExtended.RWST7": $("#CustomerExtended_RWST07").val(),
        "CustomerExtended.RWST8": $("#CustomerExtended_RWST08").val(),
        "CustomerExtended.RWST9": $("#CustomerExtended_RWST09").val(),
        "CustomerExtended.RWST10": $("#CustomerExtended_RWST10").val(),
        "CustomerExtended.RWST11": $("#CustomerExtended_RWST11").val(),
        "CustomerExtended.RWST12": $("#CustomerExtended_RWST12").val(),
        "CustomerExtended.RWST13": $("#CustomerExtended_RWST13").val(),
        "CustomerExtended.RWST14": $("#CustomerExtended_RWST14").val(),
        "CustomerExtended.RWST15": $("#CustomerExtended_RWST15").val(),
        "CustomerExtended.CSAgreement": $("#CSAgreement").val(),
        "CustomerExtended.RepsAssignedDate": $("#RepsAssignedDate").val(),
        "CustomerExtended.GeeseCount": $("#GeeseCount").val()
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
    else if ($("#PaymentMethod").val() != "-1") {
        PaymentInfo = {
            Id: $("#PaymentMethod").val(),
            AccountName: "ProfilePackage",
        };
    }

    var systemInfo = {
        Id: $("#Idval").val(),
        CustomerId: $("#ValCustomerId").val(),
        PanelType: $("#panelType").val(),
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
        url: domainurl + "/Customer/CheckToken/",
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log("Ki re");
            if (data.result == false) {
                parent.OpenErrorMessageNew("Error!", data.message);
            }
            else if (data.status == true && data.result == true) {
                console.log("hi");
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
                    else if (typeof (data.ForteId) != "undefined" && data.ForteId != "" && data.ForteId != null) {

                        $("#unsubscribe_to_forte").removeClass('hidden');
                        $("#subscribe_to_forte").addClass('hidden');
                    }

                    /*if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
                        parent.OpenConfirmationMessage("Message", data.AuthMessage);
                    }*/
                }
                parent.OpenSuccessMessageNew("Success!", "Customer saved succesfully.", function () {
                    CloseTopToBottomModal();
                });
                parent.LoadCustomerDetail(Customerid, true);
                //ARB Subscription details ends
            }
            else if (data.status == false && typeof (data.message) != "undefined") {
                OpenErrorMessageNew("Error!", data.message);
            }


        }
    })
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
            result = true;
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
function FormatAgreementPhoneNo(avalue) {
    console.log(avalue);
    var ValueCleanAgreement = "";
    if (avalue != undefined && avalue != "" && avalue != null) {
        avalue = avalue.replace(/[-()  ]/g, '');
        if (avalue.length == 10 && isNumeric(avalue) == true) {
            ValueCleanAgreement = avalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
            $("#AgreementPhoneNo").css({ "border": "1px solid #babec5" });
        }
        else if (avalue.length > 10) {
            ValueCleanAgreement = avalue;
            $("#AgreementPhoneNo").css({ "border": "1px solid red" });
        }
        else {
            $("#AgreementPhoneNo").css({ "border": "1px solid red" });
            ValueCleanAgreement = avalue;
        }
    }
    return ValueCleanAgreement;
}
function FormatCellNumber(cvalue) {
    var ValueCleanCell = "";
    if (cvalue != undefined && cvalue != "" && cvalue != null) {
        cvalue = cvalue.replace(/[-()  ]/g, '');
        if (cvalue.length == 10 && isNumeric(cvalue) == true) {
            ValueCleanCell = cvalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
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
        if (svalue.length == 10 && isNumeric(svalue) == true) {
            ValueCleanSecondary = svalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
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
        if (svalue.length == 10 && isNumeric(svalue) == true) {
            ValueCleanAlarmPhone = svalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
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
        if (Value.length == 10 && isNumeric(Value) == true) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
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
            $("#SSN2").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 9) {
            ValueClean = Value;
            $("#SSN2").css({ "border": "1px solid red" });
        }
        else {
            ValueClean = Value;
            $("#SSN2").css({ "border": "1px solid #babec5" });
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
        if (cycle == "Annual" || cycle == "Annually") {
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
        else if (cycle == "Semi-Annual" || cycle == "Semi-Annually") {
            var addval = taxvalue * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
    else if (monitoringfee != null && tax == 0) {
        if (cycle == "Annual" || cycle == "Annually") {
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
        else if (cycle == "Semi-Annual" || cycle == "Semi-Annually") {
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
    console.log("hle");
    var monitoringfee = $("#MonthlyMonitoringFee").val();
    if (monitoringfee == "") {
        monitoringfee = 0;
    }
    var cycle = $("#BillCycle").val();
    var tax = parseInt($("#BillTax").val());
    var taxvalue = (parseFloat(monitoringfee) + (parseFloat(monitoringfee) * (TaxAmount / 100)));

    if (monitoringfee != null && tax == 1 ) {
        if (cycle == "Annual" || cycle == "Annually") {
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
        else if (cycle == "Semi-Annual" || cycle == "Semi-Annually") {
            var addval = taxvalue * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
    else if (monitoringfee != null && tax == 0) {
        if (cycle == "Annual" || cycle == "Annually") {
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
        else if (cycle == "Semi-Annual" || cycle == "Semi-Annually") {
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
        if (cycle == "Annual" || cycle == "Annually") {
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
        else if (cycle == "Semi-Annual" || cycle == "Semi-Annually") {
            var addval = taxvalue * 6;
            $("#BillAmount").val(addval.toFixed(2));
        }
        else {
            var addval = taxvalue;
            $("#BillAmount").val(addval.toFixed(2));
        }
    }
    else if (monitoringfee != null && tax == 0) {
        if (cycle == "Annual" || cycle == "Annually") {
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
        else if (cycle == "Semi-Annual" || cycle == "Semi-Annually") {
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
        field: $('.dob-datepicker')[0],
        trigger: $('#DateofBirth_custom')[0],
        firstDay: 1
    });
    MovDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        yearRange: [1920, 2010],
        field: $('#MovingDate')[0]
    });
    if (typeof (salesdateenabled) != "undefined" && salesdateenabled == "True") {
        SalesDatepicker = new Pikaday({
            format: 'MM/DD/YYYY',
            field: $('#SalesDate')[0],
            trigger: $('#SalesDate_custom')[0],
            firstDay: 1
        });
    }

    FollowUpDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#FollowUpDate')[0],
        trigger: $('#FollowUpDate_custom')[0],
        firstDay: 1
    });

    RepsAssignedDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#RepsAssignedDate')[0],
        trigger: $('#RepsAssignedDate_custom')[0],
        firstDay: 1
    });

    if (typeof (PermissionsCheckForInstallDateEnabled) != "undefined" && PermissionsCheckForInstallDateEnabled == "True") {
        InstallDatepicker = new Pikaday({
            format: 'MM/DD/YYYY',
            field: $('#InstallDate')[0],
            trigger: $('#InstallDate_custom')[0],
            firstDay: 1
        });

    }

    CutInDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CutInDate')[0],
        trigger: $('#CutInDate_custom')[0],
        firstDay: 1
    });
    ApprovalDateDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerExtended_ApprovalDate')[0],
        trigger: $('#ApprovalDate_custom')[0],
        firstDay: 1
    });
    OriginalContractDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#OriginalContractDate')[0],
        trigger: $('#OriginalContractDate_custom')[0],
        firstDay: 1
    });
    ContractStartDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerExtended_ContractStartDate')[0],
        trigger: $('#ContractStartDate_custom')[0],
        firstDay: 1
    });

    CustomerSinceDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerExtended_CustomerSince')[0],
        trigger: $('#CustomerSinceDate_custom')[0],
        firstDay: 1
    });

    ResignDatePicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerExtended_ResignDate')[0],
        trigger: $('#ResignDate_custom')[0],
        firstDay: 1
    });
    FundingDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerFundedDate')[0],
        trigger: $('#FundingDate_custom')[0],
        firstDay: 1
    });

    QA1datepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('.QA1datepicker')[0],
        trigger: $('#QA1Date_custom')[0],
        firstDay: 1
    });
    new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#EstCloseDate')[0],
        firstDay: 1
    });
    new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#ProjectWalkDate')[0],
        firstDay: 1
    });
    new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#DoNotCall')[0],
        firstDay: 1
    });
    QA2datepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('.QA2datepicker')[0],
        trigger: $('#QA2Date_custom')[0],
        firstDay: 1
    });


    if (typeof (PermissionsCheckForStartDateEnabled) != "undefined" && PermissionsCheckForStartDateEnabled == "True") {

        FirstBillingDatePicker = new Pikaday({
            format: 'MM/DD/YYYY',
            field: $('#FirstBilling')[0],
            trigger: $('#FirstBilling_custom')[0],
            firstDay: 1
        });


    }
    if (typeof (PermissionsCheckForStartDateEnabled) != "undefined" && PermissionsCheckForStartDateEnabled == "True") {

        CycleStartingDatePicker = new Pikaday({
            format: 'MM/DD/YYYY',
            field: $('#CycleStartDate')[0],
            trigger: $('#CycleStartDate_custom')[0],
            firstDay: 1
        });


    }
}
$(document).ready(function () {
    MapLoad();
    var PrimaryPhone = $("#PrimaryPhone").val();
    if (PrimaryPhone != undefined && PrimaryPhone.length == 10) {
        $("#PrimaryPhone").val(PrimaryPhone.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var SecondaryPhone = $("#SecondaryPhone").val();
    if (SecondaryPhone != undefined && SecondaryPhone.length == 10) {
        $("#SecondaryPhone").val(SecondaryPhone.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var CellNo = $("#CellNo").val();
    if (CellNo != undefined && CellNo.length == 10) {
        $("#CellNo").val(CellNo.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var AgreementPhoneNo = $("#AgreementPhoneNo").val();
    if (AgreementPhoneNo != undefined && AgreementPhoneNo.length == 10) {
        $("#AgreementPhoneNo").val(AgreementPhoneNo.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    $("#Fax").keyup(function () {
        var Fax = $(this).val();
        if (Fax != undefined && Fax != null && Fax != "") {
            var cleanFax = FormateFax(Fax);
            $(this).val(cleanFax);
        }
    });
    if (originurl.toLowerCase().indexOf("app.ieatery.com") > -1) {
        IsIeatery = true;
    }
    $("#SoldBy").select2();
    $("#SoldBy2").select2();
    $("#SoldBy3").select2();
    $("#AccessGivenTo").select2();
    $("#Installer").select2();
    $("#QA1").select2();
    $("#QA2").select2();
    $("#CustomerExtended_SalesPerson4").select2();
    $("#CustomerExtended_FinanceRep").select2();
    $("#CustomerExtended_AppoinmentSetBy").select2();
    $("#LeadSource").select2().val(leadsourceval).trigger("change");
    $("#Type").select2();
    $("#BestTimeToCall").select2();
    $("#PreferredContactMethod").select2();
    $("#BusinessAccountType").select2();
    $("#CSProvider").select2();
    $("#BranchList").select2();
    $("#Ownership").select2();
    $("#TaxExemption").select2();
    $("#SalesLocation").select2();
    $("#Status").select2();
    $("#LeadSourceType").select2();
    $("#CustomerExtended_FinanceCompany").select2();
    $("#CustomerExtended_IsFinanced").select2();
    $("#CustomerExtended_Term").select2();
    $("#CreditScoreValue").select2();
    $("#ContractTerm").select2();
    $("#CustomerFunded").select2();
    $("#BillCycle").select2();
    $("#PaymentMethod").select2();
    $("#BillTax").select2();
    $('.add_customer_wrapper_custom').height(window.innerHeight - 100);

    $(".DescStartCount").html($("#Note").val().length);
    $("#Note").keyup(function () {
        $(".DescStartCount").html($("#Note").val().length);
    });

    $("#btnSchedule").click(function () {
        LoadScheduleCalendar();
    })

    $("#UseDiffAddress").change(function () {
        if ($("#UseDiffAddress").is(':checked') == true) {
            $("#SecondaryContactList").load("/SmartPackageSetup/SecondaryContactListForCreditCheck?CustomerId=" + CustomerGuidId + "&For=CreditCheck");
        }
        else {
            $("#SecondaryContactList").html("");
        }
    });

    if ($("#ContractTerm").val() == "-1" && CustomerContractTerm != '') {
        $("#ContractTerm").val("Custom");
        $(".custom-contactterm").removeClass("hidden");
        $(".CustomContactTerm").val(parseFloat(CustomerContractTerm) * 12);
    }
    if ($("#ContractTerm").val() == "Custom") {
        $(".custom-contactterm").removeClass("hidden");
    }
    $("#ContractTerm").change(function () {
        if ($(this).val() == "Custom") {
            $(".custom-contactterm").removeClass("hidden");
        }
        else {
            $(".custom-contactterm").addClass("hidden");
          
        }
    });
    //if ($("#branchlistId").val() != '')
    //{
    //    $("#BranchList").val($("#branchlistId").val());
    //}
    //else
    //{
    //    $("#BranchList").val("-1");
    //}

    //$('#Ref_customer').change(function () {
    //    if ($('#Ref_customer').val() != null) {
    //        $("#LeadSource").val("ReferredbyCustomer");
    //    }

    //})
    //if ($('#Ref_customer').val() != null) {
    //    $("#LeadSource").val("ReferredbyCustomer");
    //}
    if (typeof (CreditScoreGradeId) != "undefined") {
        if (CreditScoreGradeId > 0 && CreditScoreGradeId != "") {
            $("#CreditScoreValue").val(CreditScoreGradeId);
        }
        else {
            if (creditGrade != "") {
                $("#CreditScoreValue").val(creditGrade);
            }
            else {
                $("#CreditScoreValue").val("-1");
            }

        }
    }



    $("#BusinessName").blur(function () {
        if ($(this).val() != undefined && $(this).val() != "") {
            $("#Type").val("Commercial");
        }
    });
    $(".budget").blur(function () {
        var formattedAmount = parseFloat($(".budget").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN") {
            formattedAmount = "";
        }
        $(".budget").val(formattedAmount);
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
    var idlist = [{ id: "#ARBSubscriptionForm", type: 'iframe', width: 720, height: 400 }];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $("#AnnualRevenue").blur(function () {
        var formattedAmount = parseFloat($("#AnnualRevenue").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN") {
            formattedAmount = "";
        }
        $("#AnnualRevenue").val(formattedAmount);
    });
    $("#PurchasePrice").blur(function () {
        var formattedAmount = parseFloat($("#PurchasePrice").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN") {
            formattedAmount = "";
        }
        $("#PurchasePrice").val(formattedAmount);
    });

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
    $("#btnProrate").click(function () {
        var monitoringFee = $("#MonthlyMonitoringFee").val();
        var startDate = $("#FirstBilling").val();
        if (monitoringFee == '' || startDate == '') {
            OpenErrorMessageNew("Error!", "Please fill up Monitoring Fee and Start Date");
        }
        else {
            //var url = "/Customer/CheckMonitoringFeeAndStartDate";
            //var param = JSON.stringify({
            //    CustomerId: CustomerGuidId,
            //    MonitoringFee: monitoringFee,
            //    BillingStartDate: startDate
            //});

            //$.ajax({
            //    type: "POST",
            //    ajaxStart: $(".loader-div").show(),
            //    url: url,
            //    data: param,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    cache: false,
            //    success: function (data) {
            //        console.log(data);
            //        if (data.proratemsg != null && data.proratemsg!="") {
            //            OpenErrorMessageNew("Error!", data.proratemsg, "");
            //        }
            //        else {
            //            $(".GenerateProrateBillCustomer").attr("href",domainurl + "/Customer/GetGenerateProrate/?CustomerId=" + CustomerGuidId);
            //            $(".GenerateProrateBillCustomer").click();
            //            //OpenTopToBottomModal(domainurl + "/Customer/GetGenerateProrate/?CustomerId=" + CustomerGuidId);
            //        }

            //    },
            //    error: function (jqXHR, textStatus, errorThrown) {
            //        console.log(errorThrown);
            //    }
            //})
            SaveCustomer(true);
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
    });
    $("#AgreementPhoneNo").keyup(function () {
        var AgreementPhoneNo = $(this).val();
        if (AgreementPhoneNo != undefined && AgreementPhoneNo != null && AgreementPhoneNo != "") {
            var cleanPhoneNumber = FormatAgreementPhoneNo(AgreementPhoneNo);
            $(this).val(cleanPhoneNumber);
        }
    });
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
    $("#SSN,#SSN2").keyup(function () {
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
    function CheckPhoneIsNumeric() {
        console.log("CheckPhoneIsNumeric");
        var result = false;
        var flag = 1;
        var CellNo = $("#CellNo").val();
        var SecondaryPhone = $("#SecondaryPhone").val();
        var AlarmPhone = $("#Alarm_Phone").val();
        var PrimaryPhone = $("#PrimaryPhone").val();
        var AgreementPhoneNo = $("#AgreementPhoneNo").val();
        if(CellNo != "undefined"){
            result = isNumeric(CellNo);
            if (result == false) {
                flag = 0;
                $("#CellNo").addClass("required");
            }
        }
        if(SecondaryPhone != "undefined"){
            result = isNumeric(SecondaryPhone);
            if (result == false) {
                flag = 0;
                $("#SecondaryPhone").addClass("required");
            }
        }
        if(AlarmPhone != "undefined"){
            result = isNumeric(AlarmPhone);
            if (result == false) {
                flag = 0;
                $("#Alarm_Phone").addClass("required");
            }
        }
        if(PrimaryPhone != "undefined"){
            result = isNumeric(PrimaryPhone);
            if (result == false) {
                flag = 0;
                $("#PrimaryPhone").addClass("required");
            }
        }
        if (AgreementPhoneNo != "undefined") {
            result = isNumeric(AgreementPhoneNo);
            if (result == false) {
                flag = 0;
                $("#AgreementPhoneNo").addClass("required");
            }
        }
        if (flag == 0) {
            return false;
        }
        else {
            return true;
        }
    }
    function DropdownValidationForSelect2() {
        var flag = 1;
        console.log("Deopdown Validation");
        var type = $("#Type").val();
        if ($("#Type").attr('datarequired') == 'true') {
            if (type == '-1' || type == '00000000-0000-0000-0000-000000000000') {
                var domvalue = $("#Type").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var besttimetocall = $("#BestTimeToCall").val();
        if ($("#BestTimeToCall").attr('datarequired') == 'true') {
            if (besttimetocall == '-1' || besttimetocall == '00000000-0000-0000-0000-000000000000') {
                var domvalue = $("#BestTimeToCall").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var preferredcontactmethod = $("#PreferredContactMethod").val();
        if ($("#PreferredContactMethod").attr('datarequired') == 'true') {
            if (preferredcontactmethod == '-1' || preferredcontactmethod == '00000000-0000-0000-0000-000000000000') {
                var domvalue = $("#PreferredContactMethod").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var businessaccounttype = $("#BusinessAccountType").val();
        if ($("#BusinessAccountType").attr('datarequired') == 'true') {
            if (businessaccounttype == '-1' || businessaccounttype == '00000000-0000-0000-0000-000000000000') {
                var domvalue = $("#BusinessAccountType").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var leadsource = $("#LeadSource").val();
        if ($("#LeadSource").attr('datarequired') == 'true') {
            if (leadsource == '-1' || leadsource == '00000000-0000-0000-0000-000000000000') {
                var domvalue = $("#LeadSource").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var text = $("#CustomerExtended_AppoinmentSetBy").val();
        if ($("#CustomerExtended_AppoinmentSetBy").attr('datarequired') == 'true') {
            if ((text == '-1' || text == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerExtended_AppoinmentSetBy").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var soldby = $("#SoldBy").val();
        if ($("#SoldBy").attr('datarequired') == 'true') {
            if ((soldby == '-1' || soldby == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#SoldBy").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var soldby2 = $("#SoldBy2").val();
        if ($("#SoldBy2").attr('datarequired') == 'true') {
            if ((soldby2 == '-1' || soldby2 == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#SoldBy2").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var soldby3 = $("#SoldBy3").val();
        if ($("#SoldBy3").attr('datarequired') == 'true') {
            if ((soldby3 == '-1' || soldby3 == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#SoldBy3").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var accessgivento = $("#AccessGivenTo").val();
        if ($("#AccessGivenTo").attr('datarequired') == 'true') {
            if ((accessgivento == '-1' || accessgivento == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#AccessGivenTo").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var installer = $("#Installer").val();
        if ($("#Installer").attr('datarequired') == 'true') {
            if ((installer == '-1' || installer == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#Installer").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var qa1 = $("#QA1").val();
        if ($("#QA1").attr('datarequired') == 'true') {
            if ((qa1 == '-1' || qa1 == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#QA1").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var qa2 = $("#QA2").val();
        if ($("#QA2").attr('datarequired') == 'true') {
            if ((qa2 == '-1' || qa2 == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#QA2").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var salesperson4 = $("#CustomerExtended_SalesPerson4").val();
        if ($("#CustomerExtended_SalesPerson4").attr('datarequired') == 'true') {
            if ((salesperson4 == '-1' || salesperson4 == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerExtended_SalesPerson4").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var financerep = $("#CustomerExtended_FinanceRep").val();
        if ($("#CustomerExtended_FinanceRep").attr('datarequired') == 'true') {
            if ((financerep == '-1' || financerep == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerExtended_FinanceRep").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var csprovider = $("#CSProvider").val();
        if ($("#CSProvider").attr('datarequired') == 'true') {
            if ((csprovider == '-1' || csprovider == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CSProvider").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var branchlist = $("#BranchList").val();
        if ($("#BranchList").attr('datarequired') == 'true') {
            if ((branchlist == '-1' || branchlist == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#BranchList").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var ownership = $("#Ownership").val();
        if ($("#Ownership").attr('datarequired') == 'true') {
            if ((ownership == '-1' || ownership == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#Ownership").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var taxexemption = $("#TaxExemption").val();
        if ($("#TaxExemption").attr('datarequired') == 'true') {
            if ((taxexemption == '-1' || taxexemption == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#TaxExemption").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var saleslocation = $("#SalesLocation").val();
        if ($("#SalesLocation").attr('datarequired') == 'true') {
            if ((saleslocation == '-1' || saleslocation == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#SalesLocation").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var status = $("#Status").val();
        if ($("#Status").attr('datarequired') == 'true') {
            if ((status == '-1' || status == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#Status").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var leadsourectype = $("#LeadSourceType").val();
        if ($("#LeadSourceType").attr('datarequired') == 'true') {
            if ((leadsourectype == '-1' || leadsourectype == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#LeadSourceType").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var financecompany = $("#CustomerExtended_FinanceCompany").val();
        if ($("#CustomerExtended_FinanceCompany").attr('datarequired') == 'true') {
            if ((financecompany == '-1' || financecompany == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerExtended_FinanceCompany").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var isfinanced = $("#CustomerExtended_IsFinanced").val();
        if ($("#CustomerExtended_IsFinanced").attr('datarequired') == 'true') {
            if ((isfinanced == '-1' || isfinanced == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerExtended_IsFinanced").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var term = $("#CustomerExtended_Term").val();
        if ($("#CustomerExtended_Term").attr('datarequired') == 'true') {
            if ((term == '-1' || term == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerExtended_Term").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var creditscorevalue = $("#CreditScoreValue").val();
        if ($("#CreditScoreValue").attr('datarequired') == 'true') {
            if ((creditscorevalue == '-1' || creditscorevalue == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CreditScoreValue").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var contractterm = $("#ContractTerm").val();
        if ($("#ContractTerm").attr('datarequired') == 'true') {
            if ((contractterm == '-1' || contractterm == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#ContractTerm").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var customerfunded = $("#CustomerFunded").val();
        if ($("#CustomerFunded").attr('datarequired') == 'true') {
            if ((customerfunded == '-1' || customerfunded == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#CustomerFunded").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var billcycle = $("#BillCycle").val();
        if ($("#BillCycle").attr('datarequired') == 'true') {
            if ((billcycle == '-1' || billcycle == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#BillCycle").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var paymentmethod = $("#PaymentMethod").val();
        if ($("#PaymentMethod").attr('datarequired') == 'true') {
            if ((paymentmethod == '-1' || paymentmethod == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#PaymentMethod").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        var billtax = $("#BillTax").val();
        if ($("#BillTax").attr('datarequired') == 'true') {
            if ((billtax == '-1' || billtax == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#BillTax").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        if (flag == 0) {
            return false;
        }
        else {
            return true;
        }
    }
    $("#SaveCustomer").click(function () {
        console.log("befor CommonUiValidation");
        CommonUiValidation();
        if (DropdownValidationForSelect2() && CommonUiValidation() && CheckPhoneIsNumeric() /*&& BillingStartDayValidation()*/) {
            console.log("after CommonUiValidation");
            //CheckRestrictedZipCodeSaveClose();
            SaveCustomer();

            //OpenSuccessMessageNew("Success!", "Customer saved succesfully.");
            //CloseTopToBottomModal();
        } else {
            ScrollToError();
        }
    });
    $("#SaveCustomerDraft").click(function () {
        if (CommonUiValidation() && DropdownValidationForSelect2() /*&& BillingStartDayValidation()*/) {
            SaveCustomerDraft();
            //OpenSuccessMessageNew("Success!", "Customer saved succesfully.");
            //CloseTopToBottomModal();
        } else {
            ScrollToError();
        }
    });
    $("#Type").change(function () {
        if ($(this).val() == "Commercial") {
            $("#BusinessName").attr('datarequired', 'true');
        }
        else {
            $("#BusinessName").attr('datarequired', 'false');
        }
    });

    $("#SubscribeToAlarm").click(function () {
        SubscribeToAlarm();
    });
    setTimeout(function () {
        initDocReady();
    }, 1000);

    if (typeof (accountType) != "undefined" && accountType != null && accountType != "") {
        $("#CustomerAccountType").val(accountType);
    }



    $("#SoldBy").change(function () {
        if ($("#SoldBy").attr('datarequired') == 'true') {
            var domvalue = $("#SoldBy").parent();
            if ($("#SoldBy").val() != '00000000-0000-0000-0000-000000000000' && $("#SoldBy").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#SoldBy2").change(function () {
        if ($("#SoldBy2").attr('datarequired') == 'true') {
            var domvalue = $("#SoldBy2").parent();
            if ($("#SoldBy2").val() != '00000000-0000-0000-0000-000000000000' && $("#SoldBy2").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#SoldBy3").change(function () {
        if ($("#SoldBy3").attr('datarequired') == 'true') {
            var domvalue = $("#SoldBy3").parent();
            if ($("#SoldBy3").val() != '00000000-0000-0000-0000-000000000000' && $("#SoldBy3").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#AccessGivenTo").change(function () {
        if ($("#AccessGivenTo").attr('datarequired') == 'true') {
            var domvalue = $("#AccessGivenTo").parent();
            if ($("#AccessGivenTo").val() != '00000000-0000-0000-0000-000000000000' && $("#AccessGivenTo").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#Installer").change(function () {
        if ($("#Installer").attr('datarequired') == 'true') {
            var domvalue = $("#Installer").parent();
            if ($("#Installer").val() != '00000000-0000-0000-0000-000000000000' && $("#Installer").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#QA1").change(function () {
        if ($("#QA1").attr('datarequired') == 'true') {
            var domvalue = $("#QA1").parent();
            if ($("#QA1").val() != '00000000-0000-0000-0000-000000000000' && $("#QA1").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#QA2").change(function () {
        if ($("#QA2").attr('datarequired') == 'true') {
            var domvalue = $("#QA2").parent();
            if ($("#QA2").val() != '00000000-0000-0000-0000-000000000000' && $("#QA2").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_SalesPerson4").change(function () {
        if ($("#CustomerExtended_SalesPerson4").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerExtended_SalesPerson4").parent();
            if ($("#CustomerExtended_SalesPerson4").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_SalesPerson4").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_FinanceRep").change(function () {
        if ($("#CustomerExtended_FinanceRep").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerExtended_FinanceRep").parent();
            if ($("#CustomerExtended_FinanceRep").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_FinanceRep").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_AppoinmentSetBy").change(function () {
        if ($("#CustomerExtended_AppoinmentSetBy").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerExtended_AppoinmentSetBy").parent();
            if ($("#CustomerExtended_AppoinmentSetBy").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_AppoinmentSetBy").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#LeadSource").change(function () {
        if ($("#LeadSource").attr('datarequired') == 'true') {
            var domvalue = $("#LeadSource").parent();
            if ($("#LeadSource").val() != '00000000-0000-0000-0000-000000000000' && $("#LeadSource").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#Type").change(function () {
        if ($("#Type").attr('datarequired') == 'true') {
            var domvalue = $("#Type").parent();
            if ($("#Type").val() != '00000000-0000-0000-0000-000000000000' && $("#Type").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#BestTimeToCall").change(function () {
        if ($("#BestTimeToCall").attr('datarequired') == 'true') {
            var domvalue = $("#BestTimeToCall").parent();
            if ($("#BestTimeToCall").val() != '00000000-0000-0000-0000-000000000000' && $("#BestTimeToCall").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#PreferredContactMethod").change(function () {
        if ($("#PreferredContactMethod").attr('datarequired') == 'true') {
            var domvalue = $("#PreferredContactMethod").parent();
            if ($("#PreferredContactMethod").val() != '00000000-0000-0000-0000-000000000000' && $("#PreferredContactMethod").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#BusinessAccountType").change(function () {
        if ($("#BusinessAccountType").attr('datarequired') == 'true') {
            var domvalue = $("#BusinessAccountType").parent();
            if ($("#BusinessAccountType").val() != '00000000-0000-0000-0000-000000000000' && $("#BusinessAccountType").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CSProvider").change(function () {
        if ($("#CSProvider").attr('datarequired') == 'true') {
            var domvalue = $("#CSProvider").parent();
            if ($("#CSProvider").val() != '00000000-0000-0000-0000-000000000000' && $("#CSProvider").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#BranchList").change(function () {
        if ($("#BranchList").attr('datarequired') == 'true') {
            var domvalue = $("#BranchList").parent();
            if ($("#BranchList").val() != '00000000-0000-0000-0000-000000000000' && $("#BranchList").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#Ownership").change(function () {
        if ($("#Ownership").attr('datarequired') == 'true') {
            var domvalue = $("#Ownership").parent();
            if ($("#Ownership").val() != '00000000-0000-0000-0000-000000000000' && $("#Ownership").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#TaxExemption").change(function () {
        if ($("#TaxExemption").attr('datarequired') == 'true') {
            var domvalue = $("#TaxExemption").parent();
            if ($("#TaxExemption").val() != '00000000-0000-0000-0000-000000000000' && $("#TaxExemption").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#SalesLocation").change(function () {
        if ($("#SalesLocation").attr('datarequired') == 'true') {
            var domvalue = $("#SalesLocation").parent();
            if ($("#SalesLocation").val() != '00000000-0000-0000-0000-000000000000' && $("#SalesLocation").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#Status").change(function () {
        if ($("#Status").attr('datarequired') == 'true') {
            var domvalue = $("#Status").parent();
            if ($("#Status").val() != '00000000-0000-0000-0000-000000000000' && $("#Status").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#LeadSourceType").change(function () {
        if ($("#LeadSourceType").attr('datarequired') == 'true') {
            var domvalue = $("#LeadSourceType").parent();
            if ($("#LeadSourceType").val() != '00000000-0000-0000-0000-000000000000' && $("#LeadSourceType").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_FinanceCompany").change(function () {
        if ($("#CustomerExtended_FinanceCompany").attr('datarequired') == 'true') {
            var domvalue = $("#Status").parent();
            if ($("#CustomerExtended_FinanceCompany").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_FinanceCompany").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_IsFinanced").change(function () {
        if ($("#CustomerExtended_IsFinanced").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerExtended_IsFinanced").parent();
            if ($("#CustomerExtended_IsFinanced").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_IsFinanced").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_Term").change(function () {
        if ($("#CustomerExtended_Term").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerExtended_Term").parent();
            if ($("#CustomerExtended_Term").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_Term").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CreditScoreValue").change(function () {
        if ($("#CreditScoreValue").attr('datarequired') == 'true') {
            var domvalue = $("#CreditScoreValue").parent();
            if ($("#CreditScoreValue").val() != '00000000-0000-0000-0000-000000000000' && $("#CreditScoreValue").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#ContractTerm").change(function () {
        if ($("#ContractTerm").attr('datarequired') == 'true') {
            var domvalue = $("#ContractTerm").parent();
            if ($("#ContractTerm").val() != '00000000-0000-0000-0000-000000000000' && $("#ContractTerm").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerFunded").change(function () {
        if ($("#CustomerFunded").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerFunded").parent();
            if ($("#CustomerFunded").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerFunded").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#BillCycle").change(function () {
        if ($("#BillCycle").attr('datarequired') == 'true') {
            var domvalue = $("#BillCycle").parent();
            if ($("#BillCycle").val() != '00000000-0000-0000-0000-000000000000' && $("#BillCycle").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#PaymentMethod").change(function () {
        if ($("#PaymentMethod").attr('datarequired') == 'true') {
            var domvalue = $("#PaymentMethod").parent();
            if ($("#PaymentMethod").val() != '00000000-0000-0000-0000-000000000000' && $("#PaymentMethod").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#BillTax").change(function () {
        if ($("#BillTax").attr('datarequired') == 'true') {
            var domvalue = $("#BillTax").parent();
            if ($("#BillTax").val() != '00000000-0000-0000-0000-000000000000' && $("#BillTax").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })

});
$(window).resize(function () {
    $('.add_customer_wrapper_custom').height(window.innerHeight - 100);
});