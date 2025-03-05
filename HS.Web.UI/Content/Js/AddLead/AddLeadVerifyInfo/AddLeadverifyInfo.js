/*var DobDatepicker;*/
var SpouseDate;
var TypeVal;
function FormateSSNNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-  ]/g, '');
        if (Value.length == 9) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{2})\-?(\d{4})/, "$1-$2-$3");
            $("#SSN2").css({ "border": "1px solid #babec5" });
            $("#SpouseSSN").css({ "border": "1px solid #babec5" });
            $("#CustomerExtended_SecondarySSN").css({ "border": "1px solid #babec5" });

        }
        else if (Value.length > 9) {
            ValueClean = Value;
            $("#SSN2").css({ "border": "1px solid red" });
            $("#SpouseSSN").css({ "border": "1px solid red" });
            $("#CustomerExtended_SecondarySSN").css({ "border": "1px solid #babec5" });
        }
        else {
            ValueClean = Value;
            $("#SSN2").css({ "border": "1px solid #babec5" });
            $("#SpouseSSN").css({ "border": "1px solid #babec5" });
            $("#CustomerExtended_SecondarySSN").css({ "border": "1px solid #babec5" });
        }
    }
    return ValueClean;
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
function FormatePhoneNumber1(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10 && isNumeric(Value) == true) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
            $("#SecondaryPhone").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#SecondaryPhone").css({ "border": "1px solid red" });
        }
        else {
            $("#SecondaryPhone").css({ "border": "1px solid red" });
            ValueClean = Value;
        }
    }
    return ValueClean;
}

var format = function (num) {

    var str = num.toString().replace("$", ""), parts = false, output = [], i = 1, formatted = null;
    if (str.indexOf(".") > 0) {
        parts = str.split(".");
        str = parts[0];
    }
    str = str.split("").reverse();
    for (var j = 0, len = str.length; j < len; j++) {
        if (str[j] != ",") {
            output.push(str[j]);
            if (i % 3 == 0 && j < (len - 1)) {
                output.push(",");
            }
            i++;
        }
    }
    formatted = output.reverse().join("");
    console.log(formatted);
    return formatted;

};
var SaveLeadandVerify = function () {
    console.log("SaveLeadandVerify");
    console.log("sagar");
    var SSN = $("#SSN").val();
    var SSN2 = $("#SSN2").val();
    if (SSN2 != null && SSN2 != "undefined") {
        var SSN2Clean = typeof SSN2 == "undefined" ? "" : SSN2.replace(/[-  ]/g, '');
        if (SSN2Clean.length == 9) {
            SSN = SSN2;
        }
    }

    console.log("1");

    var url = domainurl + "/Leads/AddLeads";
    var param = {
        id: $("#id").val(),
        Title: $("#Title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $(".EmailAddress").val(),
        Address2: $("#Address2").val(),
        Type: $("#Type").val(),
        DateofBirth: $("#DateofBirth").val(),
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        Country: $("#Country").val(),
        PrimaryPhone: $("#PrimaryPhone").val().replace(/[^a-zA-Z0-9]/g, ''),
        //SecondaryPhone: $(".SecondaryPhone").val(),
        CellNo: $(".SecondaryPhone").val().replace(/[^a-zA-Z0-9]/g, ''),
        Fax: $("#Fax").val(),
        CallingTime: $("#CallingTime").val(),
        SalesDate: $("#SalesDate").val(),
        Soldby: $("#Soldby").val(),
        InstallDate: $("#InstallDate").val(),
        DoNotCall: $("#DoNotCall").val(),
        Installer: $("#Installer").val(),
        CutInDate: $("#CutInDate").val(),
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScoreValue").val(),
        CreditScoreValue: $("#CustomerCreditScore").val(),
        ContractTeam: $("#ContractTeam").val(),
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        LeadSourceType: $("#LeadSourceType").val(),
        InstalledStatus: $("#InstalledStatus").val(),

        AcquiredFrom: $("#AcquiredFrom").val(),
        FollowUpDate: $("#FollowUpDate").val(),
        BuyoutAmountByADS: $("#BuyoutAmountByADS").val(),
        BuyoutAmountBySalesRep: $("#BuyoutAmountBySalesRep").val(),
        FinancedTerm: $("#FinancedTerm").val(),
        FinancedAmount: $("#FinancedAmount").val(),

        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: parseInt($("#CellularBackup").val()),
        CustomerFunded: parseInt($("#CustomerFunded").val()),
        Maintenance: parseInt($("#Maintenance").val()),
        Note: $("#Note").val(),
        SSN: SSN,
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        CustomerNo: $("#CustomerNum").val(),
        MiddleName: $("#MiddleName").val(),
        BillAmount: $("#BillAmount").val(),
        Levels: $("#Levels").val(),
        SoldAmount: $("#SoldAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillTax: parseInt($("#BillTax").val()),
        BillOutStanding: $("#BillOutStanding").val(),
        QA1: $("#QA1").val(),
        QA1Date: $("#QA1Date").val(),
        QA2: $("#QA2").val(),
        QA2Date: $("#QA2Date").val(),
        SecondCustomerNo: $("#SecondCustomerNo").val(),
        AdditionalCustomerNo: $("#AdditionalCustomerNo").val(),
        FirstBilling: $("#FirstBilling").val(),
        StreetType: $("#StreetType").val(),
        Appartment: $("#Appartment").val(),
        CrossStreet: $("#CrossStreet").val(),
        DBA: $("#DBA").val(),
        PreferedEmail: $("#PreferedEmail").is(":checked"),
        PreferedSms: $("#PreferedSms").is(":checked"),
        PreferedCall: $("#PreferedCall").is(":checked"),
        IsAgreement: $("#IsAgreement").is(":checked"),
        AuthorizeDescription: $("#AuthDescription").val(),
        ServiceDate: $("#ServiceDate").val(),
        ActivationFee: $("#ActivationFee").val(),
        Status: $("#LeadStatus").val(),
        PhoneType: $("#PhoneType").val(),
        Carrier: $("#Carrier").val(),
        ReferringCustomer: $("#ReferringCustomer").val(),
        EstCloseDate: $("#EstCloseDate").val(),
        ProjectWalkDate: $("#ProjectWalkDate").val(),
        BranchId: $("#BranchId").val(),
        PreferredContactMethod: $("#PreferredContactMethod").val(),
        Market: $(".market").val(),
        Passengers: $("#passengers").val(),
        Budget: (typeof ($(".budget").val()) != "undefined") ? $(".budget").val().replaceAll(",", "") : 0,
        LeadSource: $(".source").val(),
        Ownership: $("#Ownership").val(),
        EmailVerified: $("#isVerified").val(),
        IsPrimaryPhoneVerified: $("#isSitePhoneVerified").val(),
        IsCellNoVerified: $("#isCellPhoneVerified").val(),
        HomeOwner: $("#homeowner").val(),
        AccessGivenTo: $("#AccessGivenTo").val(),
        SalesLocation: $("#SalesLocation").val(),
        CustomerAccountTypeList: $("#CustomerAccountType").val(),
        SoldBy2: $("#SoldBy2").val(),
        SoldBy3: $("#SoldBy3").val(),
        MovingDate: $("#MovingDate").val(),
        ContactedPerviously: $("#ContactedPerviously").val(),
        AgreementEmail: $("#AgreementEmail").val(),
        AgreementPhoneNo: $("#AgreementPhoneNo").val(),
        ContractValue: $("#ContractValue").val(),
        MapscoNo: $("#MapscoNo").val(),
        MoveCustomerId: $("#MoveCustomerId").val(),
        Website: $("#Website").val(),
        "CustomerExtended.SalesPerson4": $("#CustomerExtended_SalesPerson4").val(),
        "CustomerExtended.Batch": $("#CustomerExtended_Batch").val(),
        "CustomerExtended.MonthlyBatch": $("#CustomerExtended_MonthlyBatch").val(),
        DrivingLicense: $("#CustomerExtended_DrivingLicense").val(),
        "CustomerExtended.FinanceRep": $("#CustomerExtended_FinanceRep").val(),
        "CustomerExtended.AppoinmentSetBy": $("#CustomerExtended_AppoinmentSetBy").val(),
        "CustomerExtended.FinanceCompany": $("#CustomerExtended_FinanceCompany").val(),
        "CustomerExtended.IsFinanced": $("#CustomerExtended_IsFinanced").val(),
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
        "CustomerExtended.DealerFee": $("#CustomerExtended_DealerFee").val()

    };
    console.log(param);
    var param1 = {
        Id: $("#sysid").val(),
        InstallType: $("#InstallType").val()
    };
    var param2 = {
        Id: $("#SpouseId").val(),
        FirstName: $("#SpouseFirstName").val(),
        LastName: $("#SpouseLastName").val(),
        DateofBirth: SpouseDate.getDate(),
        SSN: $("#SpouseSSN").val(),
        CheckSpouse: $("#leadspouse").prop('checked')
    };
    var SecondarySSN = $("#CustomerExtended_SecondarySSN").val();
    var SecondarySSN2 = $("#CustomerExtended_SecondarySSN2").val();
    if (SecondarySSN2 != null && SecondarySSN2 != "undefined") {
        var SecondarySSN2Clean = typeof SecondarySSN2 == "undefined" ? "" : SecondarySSN2.replace(/[-  ]/g, '');
        if (SecondarySSN2Clean.length == 9) {
            SecondarySSN = SecondarySSN2;
        }
    }
    var CustomerExtended = {
        Id: 0,
        CustomerId: "00000000-0000-0000-0000-000000000000",
        SalesPerson4: $("#CustomerExtended_SalesPerson4").val,
        Batch: $("#CustomerExtended_Batch").val(),
        MonthlyBatch: $("#CustomerExtended_MonthlyBatch").val(),
        FinanceRep: $("#CustomerExtended_FinanceRep").val(),
        AppoinmentSetBy: $("#CustomerExtended_AppoinmentSetBy").val(),
        FinanceCompany: $("#CustomerExtended_FinanceCompany").val(),
        IsFinanced: $("#CustomerExtended_IsFinanced").val(),
        Pets: $("#CustomerExtended_Pets").val(),
        PetsType: $("#CustomerExtended_PetsType").val(),
        Repair: $("#CustomerExtended_Repair").val(),
        VipClubMember: $("#CustomerExtended_VipClubMember").val(),
        SecondaryFirstName: $("#CustomerExtended_SecondaryFirstName").val(),
        SecondaryLastName: $("#CustomerExtended_SecondaryLastName").val(),
        SecondaryEmail: $("#CustomerExtended_SecondaryEmail").val(),
        SecondaryBirthDate: $("#CustomerExtended_SecondaryBirthDate").val(),
        SecondarySSN: SecondarySSN,
        RepairType: $("#CustomerExtended_TypeOfRepair").val(),
        BirthDateCoupon: $("#BirthDateCoupon").val(),
        DrivingLicense: $("#CustomerExtended_DrivingLicense").val(),
        RWST1: $("#CustomerExtended_RWST01").val(),
        RWST2: $("#CustomerExtended_RWST02").val(),
        RWST3: $("#CustomerExtended_RWST03").val(),
        RWST4: $("#CustomerExtended_RWST04").val(),
        RWST5: $("#CustomerExtended_RWST05").val(),
        RWST6: $("#CustomerExtended_RWST06").val(),
        RWST7: $("#CustomerExtended_RWST07").val(),
        RWST8: $("#CustomerExtended_RWST08").val(),
        RWST9: $("#CustomerExtended_RWST09").val(),
        RWST10: $("#CustomerExtended_RWST10").val(),
        RWST11: $("#CustomerExtended_RWST11").val(),
        RWST12: $("#CustomerExtended_RWST12").val(),
        RWST13: $("#CustomerExtended_RWST13").val(),
        RWST14: $("#CustomerExtended_RWST14").val(),
        RWST15: $("#CustomerExtended_RWST15").val(),
        RepsAssignedDate: $("#RepsAssignedDate").val(),
        MonthlyFinanceRate: $("#CustomerExtended_MonthlyFinanceRate").val(),
        GrossFundedAmount: $("#CustomerExtended_GrossFundedAmount").val(),
        NetFundedAmount: $("#CustomerExtended_NetFundedAmount").val(),
        DiscountFundedAmount: $("#CustomerExtended_DiscountFundedAmount").val(),
        DiscountFundedPercentage: $("#CustomerExtended_DiscountFundedPercentage").val(),
        CustomerPaymentAmount: $("#CustomerExtended_CustomerPaymentAmount").val(),
        FinanceRepCommissionRate: $("#CustomerExtended_FinanceRepCommissionRate").val(),
        LoanNumber: $("#CustomerExtended_LoanNumber").val(),
        CreditAppNumber: $("#CustomerExtended_CreditAppNumber").val(),
        Term: $("#CustomerExtended_Term").val(),
        APR: $("#CustomerExtended_APR").val(),
        DealerFee: $("#CustomerExtended_DealerFee").val(),
        MaxCreditLimit: $("#CustomerExtended_MaxCreditLimit").val(),
        ApprovalDate: $("#CustomerExtended_ApprovalDate").val()
    };
    var Fparam = JSON.stringify({
        customer: param,
        csi: param1,
        customerExtended: CustomerExtended
    })
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                var value = data.LeadId;
                //LoadLeadVerificationInfo(value);
                OpenSuccessMessageNew("Success!", "New lead created successfully.", function () {
                    LoadLeadVerificationInfo(0, true);
                })
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var CustomUiValidations = function () {
    console.log("custom UI");
    var result = true;

    if ($("#LeadSource").attr('datarequired') == 'true' && $("#LeadSource").val() == "-1") {
        $($("#LeadSource").next()).addClass('required');
        result = false;
    } else {
        $($("#LeadSource").next()).removeClass('required');
    }
    return result;
}


var SaveLeadandVerifyOnly = function (saveandclose) {
    $("#btnVerifyOnly").attr("disabled", "disabled");
    console.log("SaveLeadandVerifyOnly");
    console.log($("#LeadVersion2").val());
    var SSN = $("#SSN").val();
    var SSN2 = $("#SSN2").val();
    if (SSN2 != null && SSN2 != "undefined") {
        var SSN2Clean = typeof SSN2 == "undefined" ? "" : SSN2.replace(/[-  ]/g, '');
        if (SSN2Clean.length == 9) {
            SSN = SSN2;
        }
    }

    console.log("2");

    var url = domainurl + "/Leads/AddLeads";
    var param = {
        id: $("#id").val(),
        title: $("#title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        BusinessAccountType: $("#BusinessAccountType").val(),
        EmailAddress: $(".EmailAddress").val(),
        Address2: $("#Address2").val(),
        Type: $("#Type").val(),
        DateofBirth: $("#DateofBirth").val(),
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        ZipCode: $("#ZipCode").val(),
        Country: $("#Country").val(),
        County: $("#County").val(),
        PrimaryPhone: $("#PrimaryPhone").val().replace(/[^a-zA-Z0-9]/g, ''),
        SecondaryPhone: $("#SecondaryPhone").val(),
        CellNo: $(".CellPhone").val().replace(/[^a-zA-Z0-9]/g, ''),
        Fax: $("#Fax").val(),
        CallingTime: $("#CallingTime").val(),
        SalesDate: $("#SalesDate").val(),
        Soldby: $("#Soldby").val(),
        InstallDate: $("#InstallDate").val(),
        DoNotCall: $("#DoNotCall").val(),
        Installer: $("#Installer").val(),
        CutInDate: $("#CutInDate").val(),
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScoreValue").val(),
        CreditScoreValue: $("#CustomerCreditScore").val(),
        ContractTeam: $("#ContractTeam").val(),
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        Ownership: $("#Ownership").val(),
        LeadSourceType: $("#LeadSourceType").val(),
        InstalledStatus: $("#InstalledStatus").val(),

        AcquiredFrom: $("#AcquiredFrom").val(),
        FollowUpDate: $("#FollowUpDate").val(),
        BuyoutAmountByADS: $("#BuyoutAmountByADS").val(),
        BuyoutAmountBySalesRep: $("#BuyoutAmountBySalesRep").val(),
        FinancedTerm: $("#FinancedTerm").val(),
        FinancedAmount: $("#FinancedAmount").val(),

        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: parseInt($("#CellularBackup").val()),
        CustomerFunded: parseInt($("#CustomerFunded").val()),
        Maintenance: parseInt($("#Maintenance").val()),
        Note: $("#Note").val(),
        SSN: SSN,
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        CustomerNo: $("#CustomerNum").val(),
        MiddleName: $("#MiddleName").val(),
        BillAmount: $("#BillAmount").val(),
        Levels: $("#Levels").val(),
        SoldAmount: $("#SoldAmount").val(),
        PaymentMethod: $("#PaymentMethod").val(),
        BillCycle: $("#BillCycle").val(),
        BillDay: $("#BillDay").val(),
        BillNotes: $("#BillNotes").val(),
        BillTax: parseInt($("#BillTax").val()),
        BillOutStanding: $("#BillOutStanding").val(),
        QA1: $("#QA1").val(),
        QA1Date: $("#QA1Date").val(),
        QA2: $("#QA2").val(),
        QA2Date: $("#QA2Date").val(),
        AdditionalCustomerNo: $("#AdditionalCustomerNo").val(),
        FirstBilling: $("#FirstBilling").val(),
        StreetType: $("#StreetType").val(),
        Appartment: $("#Appartment").val(),
        CrossStreet: $("#CrossStreet").val(),
        DBA: $("#DBA").val(),
        PreferedEmail: $("#PreferedEmail").is(":checked"),
        PreferedSms: $("#PreferedSms").is(":checked"),
        PreferedCall: $("#PreferedCall").is(":checked"),
        IsAgreement: $("#IsAgreement").is(":checked"),
        AuthorizeDescription: $("#AuthDescription").val(),
        ServiceDate: $("#ServiceDate").val(),
        ActivationFee: $("#ActivationFee").val(),
        Status: $("#LeadStatus").val(),
        PhoneType: $("#PhoneType").val(),
        Carrier: $("#Carrier").val(),
        ReferringCustomer: $("#ReferringCustomer").val(),
        EsistingPanel: $("#EsistingPanel").val(),
        EstCloseDate: $("#EstCloseDate").val(),
        ProjectWalkDate: $("#ProjectWalkDate").val(),
        Passcode: $("#Passcode").val(),
        BranchId: $("#BranchId").val(),
        PreferredContactMethod: $("#PreferredContactMethod").val(),
        CrossStreet: $("#CrossStreet").val(),
        Market: $(".market").val(),
        Passengers: $("#passengers").val(),
        Budget: (typeof ($(".budget").val()) != "undefined") ? $(".budget").val().replaceAll(",", "") : 0,
        LeadSource: $(".source").val(),
        EmailVerified: $("#isVerified").val(),
        IsPrimaryPhoneVerified: $("#isSitePhoneVerified").val(),
        IsCellNoVerified: $("#isCellPhoneVerified").val(),
        IsSecondaryPhoneVerified: $("#isSecondaryPhoneVerified").val(),
        HomeOwner: $("#homeowner").val(),
        AccessGivenTo: $("#AccessGivenTo").val(),
        SalesLocation: $("#SalesLocation").val(),
        CSProvider: $("#CSProvider").val(),
        BestTimeToCall: $("#BestTimeToCall").val(),
        CustomerAccountTypeList: $("#CustomerAccountType").val(),
        DuplicateCustomer: $("#DuplicateCustomer").val(),
        SecondCustomerNo: $("#SecondCustomerNo").val(),
        SoldBy2: $("#SoldBy2").val(),
        SoldBy3: $("#SoldBy3").val(),
        InspectionCompany: $("#InspectionCompany").val(),
        MovingDate: $("#MovingDate").val(),
        ContactedPerviously: $("#ContactedPerviously").val(),
        TaxExemption: $("#TaxExemption").val(),
        AppoinmentSet: $("#AppoinmentSet").val(),
        AgreementEmail: $("#AgreementEmail").val(),
        AgreementPhoneNo: $("#AgreementPhoneNo").val(),
        ContractValue: $("#ContractValue").val(),
        MapscoNo: $("#MapscoNo").val(),
        MoveCustomerId: $("#MoveCustomerId").val(),
        Website: $("#Website").val(),
        SalesPerson4: $("#SalesPerson4").val,
        FinanceCompany: $("#FinanceCompany").val(),
        RWST1: $("#CustomerExtended_RWST01").val(),
        BatchNumber: $("#CustomerExtended_Batch").val(),
        MonthlyBatch: $("#CustomerExtended_MonthlyBatch").val(),
        FinanceRep: $("#CustomerExtended_FinanceRep").val(),
        DrivingLicense: $("#CustomerExtended_DrivingLicense").val(),
        AppoinmentSetBy: $("#CustomerExtended_AppoinmentSetBy").val(),
        RWST2: $("#CustomerExtended_RWST02").val(),
        RWST3: $("#CustomerExtended_RWST03").val(),
        RWST4: $("#CustomerExtended_RWST04").val(),
        RWST5: $("#CustomerExtended_RWST05").val(),
        RWST6: $("#CustomerExtended_RWST06").val(),
        RWST7: $("#CustomerExtended_RWST07").val(),
        RWST8: $("#CustomerExtended_RWST08").val(),
        RWST9: $("#CustomerExtended_RWST09").val(),
        RWST10: $("#CustomerExtended_RWST10").val(),
        RWST11: $("#CustomerExtended_RWST11").val(),
        RWST12: $("#CustomerExtended_RWST12").val(),
        RWST13: $("#CustomerExtended_RWST13").val(),
        RWST14: $("#CustomerExtended_RWST14").val(),
        RWST15: $("#CustomerExtended_RWST15").val()
    };
    console.log(param);
    var param1 = {
        Id: $("#sysid").val(),
        InstallType: $("#InstallType").val()
    };
    var param2 = {
        Id: $("#SpouseId").val(),
        FirstName: $("#SpouseFirstName").val(),
        LastName: $("#SpouseLastName").val(),
        DateofBirth: SpouseDate.getDate(),
        SSN: $("#SpouseSSN").val(),
        CheckSpouse: $("#leadspouse").prop('checked'),
        AddedDate: $("#SpouseAddedDate").val()
    };
    var SecondarySSN = $("#CustomerExtended_SecondarySSN").val();
    var SecondarySSN2 = $("#CustomerExtended_SecondarySSN2").val();
    if (SecondarySSN2 != null && SecondarySSN2 != "undefined") {
        var SecondarySSN2Clean = typeof SecondarySSN2 == "undefined" ? "" : SecondarySSN2.replace(/[-  ]/g, '');
        if (SecondarySSN2Clean.length == 9) {
            SecondarySSN = SecondarySSN2;
        }
    }
    var CustomerExtended = {
        Id: 0,
        CustomerId: "00000000-0000-0000-0000-000000000000",
        SalesPerson4: $("#CustomerExtended_SalesPerson4").val(),
        Batch: $("#CustomerExtended_Batch").val(),
        MonthlyBatch: $("#CustomerExtended_MonthlyBatch").val(),
        DrivingLicense: $("#CustomerExtended_DrivingLicense").val(),
        FinanceRep: $("#CustomerExtended_FinanceRep").val(),
        AppoinmentSetBy: $("#CustomerExtended_AppoinmentSetBy").val(),
        FinanceCompany: $("#CustomerExtended_FinanceCompany").val(),
        IsFinanced: $("#CustomerExtended_IsFinanced").val(),
        Pets: $("#CustomerExtended_Pets").val(),
        PetsType: $("#CustomerExtended_PetsType").val(),
        Repair: $("#CustomerExtended_Repair").val(),
        VipClubMember: $("#CustomerExtended_VipClubMember").val(),
        SecondaryFirstName: $("#CustomerExtended_SecondaryFirstName").val(),
        SecondaryLastName: $("#CustomerExtended_SecondaryLastName").val(),
        SecondaryEmail: $("#CustomerExtended_SecondaryEmail").val(),
        SecondaryBirthDate: $("#CustomerExtended_SecondaryBirthDate").val(),
        SecondarySSN: SecondarySSN,
        RepairType: $("#CustomerExtended_TypeOfRepair").val(),
        BirthDateCoupon: $("#BirthDateCoupon").val(),
        RWST1: $("#CustomerExtended_RWST01").val(),
        RWST2: $("#CustomerExtended_RWST02").val(),
        RWST3: $("#CustomerExtended_RWST03").val(),
        RWST4: $("#CustomerExtended_RWST04").val(),
        RWST5: $("#CustomerExtended_RWST05").val(),
        RWST6: $("#CustomerExtended_RWST06").val(),
        RWST7: $("#CustomerExtended_RWST07").val(),
        RWST8: $("#CustomerExtended_RWST08").val(),
        RWST9: $("#CustomerExtended_RWST09").val(),
        RWST10: $("#CustomerExtended_RWST10").val(),
        RWST11: $("#CustomerExtended_RWST11").val(),
        RWST12: $("#CustomerExtended_RWST12").val(),
        RWST13: $("#CustomerExtended_RWST13").val(),
        RWST14: $("#CustomerExtended_RWST14").val(),
        RWST15: $("#CustomerExtended_RWST15").val(),
        RepsAssignedDate: $("#RepsAssignedDate").val(),
        MonthlyFinanceRate: $("#CustomerExtended_MonthlyFinanceRate").val(),
        GrossFundedAmount: $("#CustomerExtended_GrossFundedAmount").val(),
        NetFundedAmount: $("#CustomerExtended_NetFundedAmount").val(),
        DiscountFundedAmount: $("#CustomerExtended_DiscountFundedAmount").val(),
        DiscountFundedPercentage: $("#CustomerExtended_DiscountFundedPercentage").val(),
        CustomerPaymentAmount: $("#CustomerExtended_CustomerPaymentAmount").val(),
        FinanceRepCommissionRate: $("#CustomerExtended_FinanceRepCommissionRate").val(),
        LoanNumber: $("#CustomerExtended_LoanNumber").val(),
        CreditAppNumber: $("#CustomerExtended_CreditAppNumber").val(),
        Term: $("#CustomerExtended_Term").val(),
        APR: $("#CustomerExtended_APR").val(),
        DealerFee: $("#CustomerExtended_DealerFee").val(),
        MaxCreditLimit: $("#CustomerExtended_MaxCreditLimit").val(),
        ApprovalDate: $("#CustomerExtended_ApprovalDate").val(),
        LeadVersion: $("#LeadVersion2").val()
    };


    var Fparam = JSON.stringify({
        customer: param,
        csi: param1,
        customerExtended: CustomerExtended,

    });
    console.log(param)
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                $("#btnVerifyOnly").removeAttr("disabled", "disabled");
                var value = data.LeadId;
                $("#id").val(value);
                if (saveandclose) {
                    LoadLeadsDetail(value, true);
                }
                else {
                    console.log($("#LeadVersion2").val());
                        OpenSuccessMessageNew("Success!", "Lead saved successfully.", function () {

                        });
                    if ($("#LeadVersion2").val() != "V2") {
                        LoadLeadVerificationInfo(value,true);

                    }
                }
            }
            EnableElement("#btnVerifyOnly");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#btnVerifyOnly").removeAttr("disabled", "disabled");
            EnableElement("#btnVerifyOnly");
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {
    function LeadDropdownValidationForSelect2() {
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
        var soldby = $("#Soldby").val();
        if ($("#Soldby").attr('datarequired') == 'true') {
            if ((soldby == '-1' || soldby == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#Soldby").parent();
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
        var phonetype = $("#PhoneType").val();
        if ($("#PhoneType").attr('datarequired') == 'true') {
            if ((phonetype == '-1' || phonetype == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#PhoneType").parent();
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
        var branchid = $("#BranchId").val();
        if ($("#BranchId").attr('datarequired') == 'true') {
            if ((branchid == '-1' || branchid == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#BranchId").parent();
                domvalue.find('span.select2-container').addClass("required");
                flag = 0;
            }
        }
        //var ownership = $("#Ownership").val();
        //if ($("#Ownership").attr('datarequired') == 'true') {
        //    if ((ownership == '-1' || ownership == '00000000-0000-0000-0000-000000000000')) {
        //        var domvalue = $("#Ownership").parent();
        //        domvalue.find('span.select2-container').addClass("required");
        //        flag = 0;
        //    }
        //}
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
        var status = $("#LeadStatus").val();
        if ($("#LeadStatus").attr('datarequired') == 'true') {
            if ((status == '-1' || status == '00000000-0000-0000-0000-000000000000')) {
                var domvalue = $("#LeadStatus").parent();
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
        //var contractterm = $("#ContractTerm").val();
        //if ($("#ContractTerm").attr('datarequired') == 'true') {
        //    if ((contractterm == '-1' || contractterm == '00000000-0000-0000-0000-000000000000')) {
        //        var domvalue = $("#ContractTerm").parent();
        //        domvalue.find('span.select2-container').addClass("required");
        //        flag = 0;
        //    }
        //}
        //var customerfunded = $("#CustomerFunded").val();
        //if ($("#CustomerFunded").attr('datarequired') == 'true') {
        //    if ((customerfunded == '-1' || customerfunded == '00000000-0000-0000-0000-000000000000')) {
        //        var domvalue = $("#CustomerFunded").parent();
        //        domvalue.find('span.select2-container').addClass("required");
        //        flag = 0;
        //    }
        //}
        //var billcycle = $("#BillCycle").val();
        //if ($("#BillCycle").attr('datarequired') == 'true') {
        //    if ((billcycle == '-1' || billcycle == '00000000-0000-0000-0000-000000000000')) {
        //        var domvalue = $("#BillCycle").parent();
        //        domvalue.find('span.select2-container').addClass("required");
        //        flag = 0;
        //    }
        //}
        //var paymentmethod = $("#PaymentMethod").val();
        //if ($("#PaymentMethod").attr('datarequired') == 'true') {
        //    if ((paymentmethod == '-1' || paymentmethod == '00000000-0000-0000-0000-000000000000')) {
        //        var domvalue = $("#PaymentMethod").parent();
        //        domvalue.find('span.select2-container').addClass("required");
        //        flag = 0;
        //    }
        //}
        //var billtax = $("#BillTax").val();
        //if ($("#BillTax").attr('datarequired') == 'true') {
        //    if ((billtax == '-1' || billtax == '00000000-0000-0000-0000-000000000000')) {
        //        var domvalue = $("#BillTax").parent();
        //        domvalue.find('span.select2-container').addClass("required");
        //        flag = 0;
        //    }
        //}
        if (flag == 0) {
            return false;
        }
        else {
            return true;
        }
    }
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

    $("#FirstName").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#FirstName").val() != "") {
                $("#FirstName").removeClass("required");
            }
            else {
                $("#FirstName").addClass("required");
            }
        }
    })
    $("#LastName").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#LastName").val() != "") {
                $("#LastName").removeClass("required");
            }
            else {
                $("#LastName").addClass("required");
            }
        }
    })
    $("#EsistingPanel").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#EsistingPanel").val() != "") {
                $("#EsistingPanel").removeClass("required");
            }
            else {
                $("#EsistingPanel").addClass("required");
            }
        }
    })
    $("#BusinessName").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BusinessName").val() != "") {
                $("#BusinessName").removeClass("required");
            }
            else {
                $("#BusinessName").addClass("required");
            }
        }
    })
    $("#Street").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Street").val() != "") {
                $("#Street").removeClass("required");
            }
            else {
                $("#Street").addClass("required");
            }
        }
    })
    $("#StreetType").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#StreetType").val() != "") {
                $("#StreetType").removeClass("required");
            }
            else {
                $("#StreetType").addClass("required");
            }
        }
    })
    $("#Appartment").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Appartment").val() != "") {
                $("#Appartment").removeClass("required");
            }
            else {
                $("#Appartment").addClass("required");
            }
        }
    })
    $("#DBA").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#DBA").val() != "") {
                $("#DBA").removeClass("required");
            }
            else {
                $("#DBA").addClass("required");
            }
        }
    })
    $("#CrossStreet").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CrossStreet").val() != "") {
                $("#CrossStreet").removeClass("required");
            }
            else {
                $("#CrossStreet").addClass("required");
            }
        }
    })
    $("#CrossStreet").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CrossStreet").val() != "") {
                $("#CrossStreet").removeClass("required");
            }
            else {
                $("#CrossStreet").addClass("required");
            }
        }
    })
    $("#ZipCode").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ZipCode").val() != "") {
                $("#ZipCode").removeClass("required");
            }
            else {
                $("#ZipCode").addClass("required");
            }
        }
    })
    $("#State").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#State").val() != "") {
                $("#State").removeClass("required");
            }
            else {
                $("#State").addClass("required");
            }
        }
    })
    $("#City").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#City").val() != "") {
                $("#City").removeClass("required");
            }
            else {
                $("#City").addClass("required");
            }
        }
    })
    $("#County").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#County").val() != "") {
                $("#County").removeClass("required");
            }
            else {
                $("#City").addClass("required");
            }
        }
    })
    $("#SSN2").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#SSN2").val() != "") {
                $("#SSN2").removeClass("required");
            }
            else {
                $("#SSN2").addClass("required");
            }
        }
    })
    $("#EstCloseDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#EstCloseDate").val() != "") {
                $("#EstCloseDate").removeClass("required");
            }
            else {
                $("#EstCloseDate").addClass("required");
            }
        }
    })
    $("#ProjectWalkDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ProjectWalkDate").val() != "") {
                $("#ProjectWalkDate").removeClass("required");
            }
            else {
                $("#ProjectWalkDate").addClass("required");
            }
        }
    })
    $("#BranchId").change(function () {
        if ($("#BranchId").attr('datarequired') == 'true') {
            var domvalue = $("#BranchId").parent();
            if ($("#BranchId").val() != '00000000-0000-0000-0000-000000000000' && $("#BranchId").val() != "-1") {
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
    $("#CellNo").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CellNo").val() != "") {
                $("#CellNo").removeClass("required");
            }
            else {
                $("#CellNo").addClass("required");
            }
        }
    })
    $("#PrimaryPhone").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#PrimaryPhone").val() != "") {
                $("#PrimaryPhone").removeClass("required");
            }
            else {
                $("#PrimaryPhone").addClass("required");
            }
        }
    })
    $("#EmailAddress").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#EmailAddress").val() != "") {
                $("#EmailAddress").removeClass("required");
            }
            else {
                $("#EmailAddress").addClass("required");
            }
        }
    })
    $("#PhoneType").change(function () {
        if ($("#PhoneType").attr('datarequired') == 'true') {
            var domvalue = $("#PhoneType").parent();
            if ($("#PhoneType").val() != '00000000-0000-0000-0000-000000000000' && $("#PhoneType").val() != "-1") {
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
    $("#Carrier").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#Carrier").val() != "" && $("#Carrier").val() != "-1") {
                $("#Carrier").removeClass("required");
            }
            else {
                $("#Carrier").addClass("required");
            }
        }
    })
    $("#Soldby").change(function () {
        if ($("#Soldby").attr('datarequired') == 'true') {
            var domvalue = $("#Soldby").parent();
            if ($("#Soldby").val() != '00000000-0000-0000-0000-000000000000' && $("#Soldby").val() != "-1") {
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
    $("#CustomerExtended_RWST01").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST01").val() != "" && $("#CustomerExtended_RWST01").val() != "-1") {
                $("#CustomerExtended_RWST01").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST01").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST02").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST01").val() != "" && $("#CustomerExtended_RWST02").val() != "-1") {
                $("#CustomerExtended_RWST02").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST02").addClass("required");
            }
        }

    })
    $("#CustomerExtended_RWST03").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST03").val() != "" && $("#CustomerExtended_RWST03").val() != "-1") {
                $("#CustomerExtended_RWST03").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST03").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST04").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST04").val() != "" && $("#CustomerExtended_RWST04").val() != "-1") {
                $("#CustomerExtended_RWST04").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST04").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST05").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST05").val() != "" && $("#CustomerExtended_RWST05").val() != "-1") {
                $("#CustomerExtended_RWST05").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST05").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST06").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST06").val() != "" && $("#CustomerExtended_RWST06").val() != "-1") {
                $("#CustomerExtended_RWST06").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST06").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST07").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST07").val() != "" && $("#CustomerExtended_RWST07").val() != "-1") {
                $("#CustomerExtended_RWST07").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST07").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST08").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST08").val() != "" && $("#CustomerExtended_RWST08").val() != "-1") {
                $("#CustomerExtended_RWST08").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST08").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST09").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST09").val() != "" && $("#CustomerExtended_RWST09").val() != "-1") {
                $("#CustomerExtended_RWST09").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST09").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST10").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST10").val() != "" && $("#CustomerExtended_RWST10").val() != "-1") {
                $("#CustomerExtended_RWST10").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST10").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST11").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST11").val() != "" && $("#CustomerExtended_RWST11").val() != "-1") {
                $("#CustomerExtended_RWST11").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST11").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST12").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST12").val() != "" && $("#CustomerExtended_RWST12").val() != "-1") {
                $("#CustomerExtended_RWST12").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST12").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST13").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST13").val() != "" && $("#CustomerExtended_RWST13").val() != "-1") {
                $("#CustomerExtended_RWST13").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST13").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST14").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST14").val() != "" && $("#CustomerExtended_RWST14").val() != "-1") {
                $("#CustomerExtended_RWST14").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST14").addClass("required");
            }
        }
    })
    $("#CustomerExtended_RWST15").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_RWST15").val() != "" && $("#CustomerExtended_RWST15").val() != "-1") {
                $("#CustomerExtended_RWST15").removeClass("required");
            }
            else {
                $("#CustomerExtended_RWST15").addClass("required");
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
    $("#CustomerExtended_FinanceCompany").change(function () {
        if ($("#CustomerExtended_FinanceCompany").attr('datarequired') == 'true') {
            var domvalue = $("#CustomerExtended_FinanceCompany").parent();
            if ($("#CustomerExtended_FinanceCompany").val() != '00000000-0000-0000-0000-000000000000' && $("#CustomerExtended_FinanceCompany").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#CustomerExtended_Pets").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_Pets").val() != "" && $("#CustomerExtended_Pets").val() != "-1") {
                $("#CustomerExtended_Pets").removeClass("required");
            }
            else {
                $("#CustomerExtended_Pets").addClass("required");
            }
        }
    })
    $("#CustomerExtended_PetsType").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_PetsType").val() != "" && $("#CustomerExtended_PetsType").val() != "-1") {
                $("#CustomerExtended_PetsType").removeClass("required");
            }
            else {
                $("#CustomerExtended_PetsType").addClass("required");
            }
        }
    })
    $("#CustomerExtended_Repair").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_Repair").val() != "" && $("#CustomerExtended_Repair").val() != "-1") {
                $("#CustomerExtended_Repair").removeClass("required");
            }
            else {
                $("#CustomerExtended_Repair").addClass("required");
            }
        }
    })
    $("#CustomerExtended_VipClubMember").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_VipClubMember").val() != "" && $("#CustomerExtended_VipClubMember").val() != "-1") {
                $("#CustomerExtended_VipClubMember").removeClass("required");
            }
            else {
                $("#CustomerExtended_VipClubMember").addClass("required");
            }
        }
    })
    $("#CustomerExtended_SecondarySSN2").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_SecondarySSN2").val() != "") {
                $("#CustomerExtended_SecondarySSN2").removeClass("required");
            }
            else {
                $("#CustomerExtended_SecondarySSN2").addClass("required");
            }
        }
    })
    $("#CustomerExtended_SecondaryFirstName").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_SecondaryFirstName").val() != "") {
                $("#CustomerExtended_SecondaryFirstName").removeClass("required");
            }
            else {
                $("#CustomerExtended_SecondaryFirstName").addClass("required");
            }
        }
    })
    $("#CustomerExtended_Batch").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_Batch").val() != "") {
                $("#CustomerExtended_Batch").removeClass("required");
            }
            else {
                $("#CustomerExtended_Batch").addClass("required");
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
    $("#CustomerExtended_SecondaryLastName").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_SecondaryLastName").val() != "") {
                $("#CustomerExtended_SecondaryLastName").removeClass("required");
            }
            else {
                $("#CustomerExtended_SecondaryLastName").addClass("required");
            }
        }
    })
    $("#CustomerExtended_SecondaryEmail").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_SecondaryEmail").val() != "") {
                $("#CustomerExtended_SecondaryEmail").removeClass("required");
            }
            else {
                $("#CustomerExtended_SecondaryEmail").addClass("required");
            }
        }
    })
    $("#CustomerExtended_SecondaryBirthDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_SecondaryBirthDate").val() != "") {
                $("#CustomerExtended_SecondaryBirthDate").removeClass("required");
            }
            else {
                $("#CustomerExtended_SecondaryBirthDate").addClass("required");
            }
        }
    })
    $("#BirthDateCoupon").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BirthDateCoupon").val() != "") {
                $("#BirthDateCoupon").removeClass("required");
            }
            else {
                $("#BirthDateCoupon").addClass("required");
            }
        }
    })
    $("#InspectionCompany").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#InspectionCompany").val() != "") {
                $("#InspectionCompany").removeClass("required");
            }
            else {
                $("#InspectionCompany").addClass("required");
            }
        }
    })
    $("#homeowner").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#homeowner").val() != "") {
                $("#homeowner").removeClass("required");
            }
            else {
                $("#homeowner").addClass("required");
            }
        }
    })
    $("#LeadStatus").change(function () {
        if ($("#LeadStatus").attr('datarequired') == 'true') {
            var domvalue = $("#LeadStatus").parent();
            if ($("#LeadStatus").val() != '00000000-0000-0000-0000-000000000000' && $("#LeadStatus").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#InstalledStatus").change(function () {
        if ($("#InstalledStatus").attr('datarequired') == 'true') {
            var domvalue = $("#InstalledStatus").parent();
            if ($("#InstalledStatus").val() != '00000000-0000-0000-0000-000000000000' && $("#InstalledStatus").val() != "-1") {
                domvalue.find('span.select2-container').removeClass("required");
            }
            else {
                domvalue.find('span.select2-container').addClass("required");
            }
        }
    })
    $("#DuplicateCustomer").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#DuplicateCustomer").val() != "" && $("#DuplicateCustomer").val() != "-1") {
                $("#DuplicateCustomer").removeClass("required");
            }
            else {
                $("#DuplicateCustomer").addClass("required");
            }
        }
    })
    $("#ReferringCustomer").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#ReferringCustomer").val() != "" && $("#ReferringCustomer").val() != "-1") {
                $("#ReferringCustomer").removeClass("required");
            }
            else {
                $("#ReferringCustomer").addClass("required");
            }
        }
    })
    $("#Market").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#Market").val() != "" && $("#Market").val() != "-1") {
                $("#Market").removeClass("required");
            }
            else {
                $("#Market").addClass("required");
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
    $("#passengers").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#passengers").val() != "") {
                $("#passengers").removeClass("required");
            }
            else {
                $("#passengers").addClass("required");
            }
        }
    })
    $("#Budget").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Budget").val() != "") {
                $("#Budget").removeClass("required");
            }
            else {
                $("#Budget").addClass("required");
            }
        }
    })
    $("#CustomerNum").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerNum").val() != "") {
                $("#CustomerNum").removeClass("required");
            }
            else {
                $("#CustomerNum").addClass("required");
            }
        }
    })
    $("#Title").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Title").val() != "") {
                $("#Title").removeClass("required");
            }
            else {
                $("#Title").addClass("required");
            }
        }
    })
    $("#DateofBirth").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#DateofBirth").val() != "") {
                $("#DateofBirth").removeClass("required");
            }
            else {
                $("#DateofBirth").addClass("required");
            }
        }
    })
    $("#SecondaryPhone").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#SecondaryPhone").val() != "") {
                $("#SecondaryPhone").removeClass("required");
            }
            else {
                $("#SecondaryPhone").addClass("required");
            }
        }
    })
    $("#Fax").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Fax").val() != "") {
                $("#Fax").removeClass("required");
            }
            else {
                $("#Fax").addClass("required");
            }
        }
    })
    $("#CallingTime").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CallingTime").val() != "") {
                $("#CallingTime").removeClass("required");
            }
            else {
                $("#CallingTime").addClass("required");
            }
        }
    })
    $("#Address").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Address").val() != "") {
                $("#Address").removeClass("required");
            }
            else {
                $("#Address").addClass("required");
            }
        }
    })
    $("#Address2").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Address2").val() != "") {
                $("#Address2").removeClass("required");
            }
            else {
                $("#Address2").addClass("required");
            }
        }
    })
    $("#StreetPrevious").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#StreetPrevious").val() != "") {
                $("#StreetPrevious").removeClass("required");
            }
            else {
                $("#StreetPrevious").addClass("required");
            }
        }
    })
    $("#CityPrevious").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CityPrevious").val() != "") {
                $("#CityPrevious").removeClass("required");
            }
            else {
                $("#CityPrevious").addClass("required");
            }
        }
    })
    $("#StatePrevious").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#StatePrevious").val() != "") {
                $("#StatePrevious").removeClass("required");
            }
            else {
                $("#StatePrevious").addClass("required");
            }
        }
    })
    $("#ZipCodePrevious").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ZipCodePrevious").val() != "") {
                $("#ZipCodePrevious").removeClass("required");
            }
            else {
                $("#ZipCodePrevious").addClass("required");
            }
        }
    })
    $("#CountryPrevious").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CountryPrevious").val() != "") {
                $("#CountryPrevious").removeClass("required");
            }
            else {
                $("#CountryPrevious").addClass("required");
            }
        }
    })
    $("#AccountNo").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#AccountNo").val() != "") {
                $("#AccountNo").removeClass("required");
            }
            else {
                $("#AccountNo").addClass("required");
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
    $("#CustomerCreditScore").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerCreditScore").val() != "") {
                $("#CustomerCreditScore").removeClass("required");
            }
            else {
                $("#CustomerCreditScore").addClass("required");
            }
        }
    })
    $("#ContractTeam").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ContractTeam").val() != "") {
                $("#ContractTeam").removeClass("required");
            }
            else {
                $("#ContractTeam").addClass("required");
            }
        }
    })
    $("#FundingCompany").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#FundingCompany").val() != "") {
                $("#FundingCompany").removeClass("required");
            }
            else {
                $("#FundingCompany").addClass("required");
            }
        }
    })
    $("#MonthlyMonitoringFee").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#MonthlyMonitoringFee").val() != "") {
                $("#MonthlyMonitoringFee").removeClass("required");
            }
            else {
                $("#MonthlyMonitoringFee").addClass("required");
            }
        }
    })
    $("#SalesDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#SalesDate").val() != "") {
                $("#SalesDate").removeClass("required");
            }
            else {
                $("#SalesDate").addClass("required");
            }
        }
    })
    $("#InstallDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#InstallDate").val() != "") {
                $("#InstallDate").removeClass("required");
            }
            else {
                $("#InstallDate").addClass("required");
            }
        }
    })
    $("#CutInDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CutInDate").val() != "") {
                $("#CutInDate").removeClass("required");
            }
            else {
                $("#CutInDate").addClass("required");
            }
        }
    })
    $("#Installer").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Installer").val() != "") {
                $("#Installer").removeClass("required");
            }
            else {
                $("#Installer").addClass("required");
            }
        }
    })
    $("#MiddleName").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#MiddleName").val() != "") {
                $("#MiddleName").removeClass("required");
            }
            else {
                $("#MiddleName").addClass("required");
            }
        }
    })
    $("#ReminderDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ReminderDate").val() != "") {
                $("#ReminderDate").removeClass("required");
            }
            else {
                $("#ReminderDate").addClass("required");
            }
        }
    })
    $("#QA1").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#QA1").val() != "") {
                $("#QA1").removeClass("required");
            }
            else {
                $("#QA1").addClass("required");
            }
        }
    })
    $("#QA1Date").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#QA1Date").val() != "") {
                $("#QA1Date").removeClass("required");
            }
            else {
                $("#QA1Date").addClass("required");
            }
        }
    })
    $("#QA2").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#QA2").val() != "") {
                $("#QA2").removeClass("required");
            }
            else {
                $("#QA2").addClass("required");
            }
        }
    })
    $("#QA2Date").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#QA2Date").val() != "") {
                $("#QA2Date").removeClass("required");
            }
            else {
                $("#QA2Date").addClass("required");
            }
        }
    })
    $("#Status").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Status").val() != "") {
                $("#Status").removeClass("required");
            }
            else {
                $("#Status").addClass("required");
            }
        }
    })
    $("#BillAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BillAmount").val() != "") {
                $("#BillAmount").removeClass("required");
            }
            else {
                $("#BillAmount").addClass("required");
            }
        }
    })
    $("#Levels").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Levels").val() != "") {
                $("#Levels").removeClass("required");
            }
            else {
                $("#Levels").addClass("required");
            }
        }
    })
    $("#SoldAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#SoldAmount").val() != "") {
                $("#SoldAmount").removeClass("required");
            }
            else {
                $("#SoldAmount").addClass("required");
            }
        }
    })
    $("#AcquiredFrom").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#AcquiredFrom").val() != "") {
                $("#AcquiredFrom").removeClass("required");
            }
            else {
                $("#AcquiredFrom").addClass("required");
            }
        }
    })
    $("#FollowUpDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#FollowUpDate").val() != "") {
                $("#FollowUpDate").removeClass("required");
            }
            else {
                $("#FollowUpDate").addClass("required");
            }
        }
    })
    $("#RepsAssignedDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#RepsAssignedDate").val() != "") {
                $("#RepsAssignedDate").removeClass("required");
            }
            else {
                $("#RepsAssignedDate").addClass("required");
            }
        }
    })
    $("#BuyoutAmountByADS").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BuyoutAmountByADS").val() != "") {
                $("#BuyoutAmountByADS").removeClass("required");
            }
            else {
                $("#BuyoutAmountByADS").addClass("required");
            }
        }
    })
    $("#MapscoNo").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#MapscoNo").val() != "") {
                $("#MapscoNo").removeClass("required");
            }
            else {
                $("#MapscoNo").addClass("required");
            }
        }
    })
    $("#BuyoutAmountBySalesRep").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BuyoutAmountBySalesRep").val() != "") {
                $("#BuyoutAmountBySalesRep").removeClass("required");
            }
            else {
                $("#BuyoutAmountBySalesRep").addClass("required");
            }
        }
    })
    $("#FinancedTerm").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#FinancedTerm").val() != "") {
                $("#FinancedTerm").removeClass("required");
            }
            else {
                $("#FinancedTerm").addClass("required");
            }
        }
    })
    $("#FinancedAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#FinancedAmount").val() != "") {
                $("#FinancedAmount").removeClass("required");
            }
            else {
                $("#FinancedAmount").addClass("required");
            }
        }
    })
    $("#PaymentMethod").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#PaymentMethod").val() != "") {
                $("#PaymentMethod").removeClass("required");
            }
            else {
                $("#PaymentMethod").addClass("required");
            }
        }
    })
    $("#BillCycle").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BillCycle").val() != "") {
                $("#BillCycle").removeClass("required");
            }
            else {
                $("#BillCycle").addClass("required");
            }
        }
    })
    $("#BillDay").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BillDay").val() != "") {
                $("#BillDay").removeClass("required");
            }
            else {
                $("#BillDay").addClass("required");
            }
        }
    })
    $("#BillNotes").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BillNotes").val() != "") {
                $("#BillNotes").removeClass("required");
            }
            else {
                $("#BillNotes").addClass("required");
            }
        }
    })
    $("#BillOutStanding").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BillOutStanding").val() != "") {
                $("#BillOutStanding").removeClass("required");
            }
            else {
                $("#BillOutStanding").addClass("required");
            }
        }
    })
    $("#ServiceDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ServiceDate").val() != "") {
                $("#ServiceDate").removeClass("required");
            }
            else {
                $("#ServiceDate").addClass("required");
            }
        }
    })
    $("#Area").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Area").val() != "") {
                $("#Area").removeClass("required");
            }
            else {
                $("#Area").addClass("required");
            }
        }
    })
    $("#SecondCustomerNo").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#SecondCustomerNo").val() != "") {
                $("#SecondCustomerNo").removeClass("required");
            }
            else {
                $("#SecondCustomerNo").addClass("required");
            }
        }
    })
    $("#AdditionalCustomerNo").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#AdditionalCustomerNo").val() != "") {
                $("#AdditionalCustomerNo").removeClass("required");
            }
            else {
                $("#AdditionalCustomerNo").addClass("required");
            }
        }
    })
    $("#Passcode").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Passcode").val() != "") {
                $("#Passcode").removeClass("required");
            }
            else {
                $("#Passcode").addClass("required");
            }
        }
    })
    $("#ActivationFee").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ActivationFee").val() != "") {
                $("#ActivationFee").removeClass("required");
            }
            else {
                $("#ActivationFee").addClass("required");
            }
        }
    })
    $("#FirstBilling").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#FirstBilling").val() != "") {
                $("#FirstBilling").removeClass("required");
            }
            else {
                $("#FirstBilling").addClass("required");
            }
        }
    })

    $("#ActivationFeePaymentMethod").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ActivationFeePaymentMethod").val() != "") {
                $("#ActivationFeePaymentMethod").removeClass("required");
            }
            else {
                $("#ActivationFeePaymentMethod").addClass("required");
            }
        }
    })
    $("#BusinessAccountType").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#BusinessAccountType").val() != "") {
                $("#BusinessAccountType").removeClass("required");
            }
            else {
                $("#BusinessAccountType").addClass("required");
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
    $("#PurchasePrice").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#PurchasePrice").val() != "") {
                $("#PurchasePrice").removeClass("required");
            }
            else {
                $("#PurchasePrice").addClass("required");
            }
        }
    })
    $("#ContractValue").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ContractValue").val() != "") {
                $("#ContractValue").removeClass("required");
            }
            else {
                $("#ContractValue").addClass("required");
            }
        }
    })
    $("#AnnualRevenue").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#AnnualRevenue").val() != "") {
                $("#AnnualRevenue").removeClass("required");
            }
            else {
                $("#AnnualRevenue").addClass("required");
            }
        }
    })
    $("#Website").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#Website").val() != "") {
                $("#Website").removeClass("required");
            }
            else {
                $("#Website").addClass("required");
            }
        }
    })
    $("#CustomerAccountType").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerAccountType").val() != "" && $("#CustomerAccountType").val() != "-1") {
                $("#CustomerAccountType").removeClass("required");
            }
            else {
                $("#CustomerAccountType").addClass("required");
            }
        }
    })
    $("#DoNotCall").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#DoNotCall").val() != "") {
                $("#DoNotCall").removeClass("required");
            }
            else {
                $("#DoNotCall").addClass("required");
            }
        }
    })
    $("#MovingDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#MovingDate").val() != "") {
                $("#MovingDate").removeClass("required");
            }
            else {
                $("#MovingDate").addClass("required");
            }
        }
    })
    $("#ContactedPerviously").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#ContactedPerviously").val() != "" && $("#ContactedPerviously").val() != "-1") {
                $("#ContactedPerviously").removeClass("required");
            }
            else {
                $("#ContactedPerviously").addClass("required");
            }
        }
    })
    $("#AgreementEmail").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#AgreementEmail").val() != "") {
                $("#AgreementEmail").removeClass("required");
            }
            else {
                $("#AgreementEmail").addClass("required");
            }
        }
    })
    $("#AgreementPhoneNo").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#AgreementPhoneNo").val() != "") {
                $("#AgreementPhoneNo").removeClass("required");
            }
            else {
                $("#AgreementPhoneNo").addClass("required");
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
    $("#BillTax").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#BillTax").val() != "" && $("#BillTax").val() != "-1") {
                $("#BillTax").removeClass("required");
            }
            else {
                $("#BillTax").addClass("required");
            }
        }
    })
    $("#AppoinmentSet").change(function () {
        if ($(this).hasClass('required')) {
            if ($("#AppoinmentSet").val() != "" && $("#AppoinmentSet").val() != "-1") {
                $("#AppoinmentSet").removeClass("required");
            }
            else {
                $("#AppoinmentSet").addClass("required");
            }
        }
    })
    $("#ZipCode").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#ZipCode").val() != "") {
                $("#ZipCode").removeClass("required");
            }
            else {
                $("#ZipCode").addClass("required");
            }
        }
    })
    $("#CustomerExtended_MonthlyFinanceRate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_MonthlyFinanceRate").val() != "") {
                $("#CustomerExtended_MonthlyFinanceRate").removeClass("required");
            }
            else {
                $("#CustomerExtended_MonthlyFinanceRate").addClass("required");
            }
        }
    })
    $("#CustomerExtended_GrossFundedAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_GrossFundedAmount").val() != "") {
                $("#CustomerExtended_GrossFundedAmount").removeClass("required");
            }
            else {
                $("#CustomerExtended_GrossFundedAmount").addClass("required");
            }
        }
    })
    $("#CustomerExtended_NetFundedAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_NetFundedAmount").val() != "") {
                $("#CustomerExtended_NetFundedAmount").removeClass("required");
            }
            else {
                $("#CustomerExtended_NetFundedAmount").addClass("required");
            }
        }
    })
    $("#CustomerExtended_DiscountFundedAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_DiscountFundedAmount").val() != "") {
                $("#CustomerExtended_DiscountFundedAmount").removeClass("required");
            }
            else {
                $("#CustomerExtended_DiscountFundedAmount").addClass("required");
            }
        }
    })
    $("#CustomerExtended_DiscountFundedPercentage").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_DiscountFundedPercentage").val() != "") {
                $("#CustomerExtended_DiscountFundedPercentage").removeClass("required");
            }
            else {
                $("#CustomerExtended_DiscountFundedPercentage").addClass("required");
            }
        }
    })
    $("#CustomerExtended_CustomerPaymentAmount").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_CustomerPaymentAmount").val() != "") {
                $("#CustomerExtended_CustomerPaymentAmount").removeClass("required");
            }
            else {
                $("#CustomerExtended_CustomerPaymentAmount").addClass("required");
            }
        }
    })
    $("#CustomerExtended_FinanceRepCommissionRate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_FinanceRepCommissionRate").val() != "") {
                $("#CustomerExtended_FinanceRepCommissionRate").removeClass("required");
            }
            else {
                $("#CustomerExtended_FinanceRepCommissionRate").addClass("required");
            }
        }
    })
    $("#CustomerExtended_LoanNumber").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_LoanNumber").val() != "") {
                $("#CustomerExtended_LoanNumber").removeClass("required");
            }
            else {
                $("#CustomerExtended_LoanNumber").addClass("required");
            }
        }
    })
    $("#CustomerExtended_CreditAppNumber").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_CreditAppNumber").val() != "") {
                $("#CustomerExtended_CreditAppNumber").removeClass("required");
            }
            else {
                $("#CustomerExtended_CreditAppNumber").addClass("required");
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
    $("#CustomerExtended_APR").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_APR").val() != "") {
                $("#CustomerExtended_APR").removeClass("required");
            }
            else {
                $("#CustomerExtended_APR").addClass("required");
            }
        }
    })
    $("#CustomerExtended_MaxCreditLimit").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_MaxCreditLimit").val() != "") {
                $("#CustomerExtended_MaxCreditLimit").removeClass("required");
            }
            else {
                $("#CustomerExtended_MaxCreditLimit").addClass("required");
            }
        }
    })
    $("#CustomerExtended_ApprovalDate").keyup(function () {
        if ($(this).hasClass('required')) {
            if ($("#CustomerExtended_ApprovalDate").val() != "") {
                $("#CustomerExtended_ApprovalDate").removeClass("required");
            }
            else {
                $("#CustomerExtended_ApprovalDate").addClass("required");
            }
        }
    })


    $("#leadspouse").click(function () {
        if ($("#leadspouse").prop('checked') == true) {
            console.log("true");
            $(".dob-div").removeClass('hidden');
            $(".ssn_div").removeClass('hidden');
        }
        else {
            console.log("false");
            $(".dob-div").addClass('hidden');
            $(".ssn_div").addClass('hidden');
        }
    });
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
    $("#leadpresent").click(function () {
        if ($("#leadpresent").prop('checked') == true) {
            $(".Streetdiv").show();
            $(".zip-div").show();
            $(".city-div").show();
            $(".state-div").show();
        }
        else {
            $(".Streetdiv").hide();
            $(".zip-div").hide();
            $(".city-div").hide();
            $(".state-div").hide();
        }
    })
    $("#CustomerExtended_SecondarySSN,#CustomerExtended_SecondarySSN2").keyup(function () {
        var SSNNumber = $(this).val();
        if (SSNNumber != undefined && SSNNumber != null && SSNNumber != "") {
            var cleanSSNNumber = FormateSSNNumber(SSNNumber);
            $(this).val(cleanSSNNumber);
        }
    });
    $("#SSN,#SSN2").keyup(function () {
        var SSNNumber = $(this).val();
        if (SSNNumber != undefined && SSNNumber != null && SSNNumber != "") {
            var cleanSSNNumber = FormateSSNNumber(SSNNumber);
            $(this).val(cleanSSNNumber);
        }
    });
    $("#SpouseSSN").keyup(function () {
        var SSNNumber1 = $(this).val();
        if (SSNNumber1 != undefined && SSNNumber1 != null && SSNNumber1 != "") {
            var cleanSSNNumber1 = FormateSSNNumber(SSNNumber1);
            $(this).val(cleanSSNNumber1);
        }
    })
    $("#PrimaryPhone").keyup(function (e) {
        var PhoneNumber = $(this).val();
        if (PhoneNumber == "") {
            $(this).css({ "border": "1px solid #ccc" });
        } else {
            if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
                var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });
    $("#SecondaryPhone").keyup(function (e) {
        var PhoneNumber = $(this).val();
        if (PhoneNumber == "") {
            $(this).css({ "border": "1px solid #ccc" });
        } else {
            if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
                var cleanPhoneNumber = FormatePhoneNumber1(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });
    $(".CellPhone").keyup(function (e) {
        var PhoneNumber = $(this).val();
        if (PhoneNumber == "") {
            $(this).css({ "border": "1px solid #ccc" });
        } else {
            if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
                var cleanPhoneNumber = FormatePhoneNumber1(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });
    $("#AgreementPhoneNo").keyup(function (e) {
        var PhoneNumber = $(this).val();
        if (PhoneNumber == "") {
            $(this).css({ "border": "1px solid #ccc" });
        } else {
            if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
                var cleanPhoneNumber = FormateAgreementPhoneNo(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });
    $(".LoaderWorkingDiv").hide();
    var counter = 0;
    $("#btnVerify").click(function () {
        CustomUiValidations();
        if (leadsourcerequired == "True") {
            if (CommonUiValidation() && CustomUiValidations()) {
                SaveLeadandVerify();
            }
        }
        else {
            SaveLeadandVerify();
        }
    })

    function CheckIsNumeric() {
        console.log("CheckPhoneIsNumeric");
        var result = false;
        var flag = 1;
        var SecondaryPhone = $("#SecondaryPhone").val();
        var PrimaryPhone = $("#PrimaryPhone").val();

        if (SecondaryPhone != "undefined") {
            result = isNumeric(SecondaryPhone);
            if (result == false) {
                flag = 0;
                $("#SecondaryPhone").addClass("required");
            }
        }
        if (PrimaryPhone != "undefined") {
            result = isNumeric(PrimaryPhone);
            if (result == false) {
                flag = 0;
                $("#PrimaryPhone").addClass("required");
            }
        }
        if (flag == 0) {
            return false;
        }
        else {
            return true;
        }
    }
    $("#btnVerifyOnly").click(function () {
        CustomUiValidations();
        console.log("Lead Save Click");
        if (leadsourcerequired == "True") {
            if (LeadDropdownValidationForSelect2() && CommonUiValidation() && CustomUiValidations() && CheckIsNumeric()) {
                DisableElement("#btnVerifyOnly");
                SaveLeadandVerifyOnly(false);
                console.log("Lead Save Click");
            }
            else {
                parent.OpenErrorMessageNew("Error!", "Please Fill Up Missing Information Above", function () {

                });
            }
        }
        else {
            if (LeadDropdownValidationForSelect2() && CommonUiValidation() && CustomUiValidations() && CheckIsNumeric() && LeadDropdownValidationForSelect2()) {
                DisableElement("#btnVerifyOnly");
                SaveLeadandVerifyOnly(false);

            }
            else {
                parent.OpenErrorMessageNew("Error!", "Please Fill Up Missing Information Above", function () {

                });
            }

        }

    })

    $("#btnVerifyOnly1").click(function () {
        CustomUiValidations();
        if (leadsourcerequired == "True") {
            if (LeadDropdownValidationForSelect2() && CommonUiValidation() && CustomUiValidations() && CheckIsNumeric()) {
                SaveLeadandVerifyOnly(false);
            }
        }
        else {
            SaveLeadandVerifyOnly(false);
        }
    })
    $("#btnVerifyOnly2").click(function () {
        CustomUiValidations();
        if (leadsourcerequired == "True") {
            if (LeadDropdownValidationForSelect2() && CommonUiValidation() && CustomUiValidations() && CheckIsNumeric()) {
                SaveLeadandVerifyOnly(true);

            }
            else {
                parent.OpenErrorMessageNew("Error!", "Please Fill Up Missing Information Above", function () {

                });
            }
        }
        else {
            if (LeadDropdownValidationForSelect2() && CommonUiValidation() && CustomUiValidations() && CheckIsNumeric()) {
                SaveLeadandVerifyOnly(true);

            }
            else {
                parent.OpenErrorMessageNew("Error!", "Please Fill Up Missing Information Above", function () {

                });
            }
        }
    })
    /*DobDatepicker = new Pikaday({ format: 'MM/DD/YYYY', yearRange: [1920, 1999], field: $('#DateofBirth')[0] });*/
    SpouseDate = new Pikaday({ format: 'MM/DD/YYYY', yearRange: [1920, 1999], field: $('#SpouseDOB')[0] });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#DateofBirth')[0], trigger: $('#DateofBirth_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#CustomerExtended_SecondaryBirthDate')[0], trigger: $('#Secondary_DateofBirth_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#SalesDate')[0], trigger: $('#SalesDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#InstallDate')[0], trigger: $('#InstallDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#CutInDate')[0], trigger: $('#CutInDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#ReminderDate')[0], trigger: $('#ReminderDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#QA1Date')[0], trigger: $('#QA1Date_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#QA2Date')[0], trigger: $('#QA2Date_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#ServiceDate')[0], trigger: $('#ServiceDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#FirstBilling')[0], trigger: $('#FirstBilling_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#DoNotCall')[0], trigger: $('#DoNotCall_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#MovingDate')[0], trigger: $('#MovingDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#CustomerExtended_ApprovalDate')[0], trigger: $('#ApprovalDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#FollowUpDate')[0], trigger: $('#FollowUpDate_custom')[0], firstDay: 1 });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#RepsAssignedDate')[0], trigger: $('#RepsAssignedDate_custom')[0], firstDay: 1 });

    if (typeof (InstalledStatusval) != "undefined" && InstalledStatusval != null && InstalledStatusval != "" && InstalledStatusval != "-1") {
        console.log(InstalledStatusval);
        $("#InstalledStatus").val(InstalledStatusval);
    }

})