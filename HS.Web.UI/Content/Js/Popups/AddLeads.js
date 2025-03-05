var SaveCustomer = function () {
    console.log("3");
    var url = domainurl + "/Leads/AddLeads/";
    var param = JSON.stringify({
        id: $("#id").val(),
        Title: $("#Title").val(),
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        BusinessName: $("#BusinessName").val(),
        EmailAddress: $("#EmailAddress").val(),
        Type: $("#Type").val(),
        DateofBirth: DobDatepicker.getDate(),
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
        SalesDate: SalesDatepicker.getDate(),
        SoldBy: $("#SoldBy").val(),
        InstallDate: InstallDatepicker.getDate(),
        Installer: $("#Installer").val(),
        CutInDate: CutInDatepicker.getDate(),
        AccountNo: $("#AccountNo").val(),
        CreditScore: $("#CreditScore").val(),
        ContractTeam: $("#ContractTerm").val(),
        FundingCompany: $("#FundingCompany").val(),
        MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        LeadSource: $("#LeadSource").val(),
        IsAlarmCom: $("#IsAlarmCom").val(),
        CellularBackup: $("#CellularBackup").val(),
        CustomerFunded: $("#CustomerFunded").val(),
        Maintenance: $("#Maintenance").val(),
        Note: $("#Note").val(),
        SSN: $("#SSN").val(),
        CityPrevious: $("#CityPrevious").val(),
        StatePrevious: $("#StatePrevious").val(),
        ZipCodePrevious: $("#ZipCodePrevious").val(),
        CountryPrevious: $("#CountryPrevious").val(),
        StreetPrevious: $("#StreetPrevious").val(),
        MiddleName: $("#MiddleName").val(),
        Leadnote: $('#Leadnote').val(),
        EmailVerified: $("#isVerified").val(),
        SalesLocation: $("#SalesLocation").val(),
        Website: $("#Website").val(),
        SalesPerson4: $("#SalesPerson4").val,
        FinanceCompany: $("#FinanceCompany").val()
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
            if (data == true) {
                var leadId = $("#id").val();

                if (leadId == null || leadId == 0) {
                    parent.LoadLeadList();
                    parent.ClosePopup();
                }
                else if (leadId != null) {
                    parent.LoadLeadList();
                    parent.LoadLeadsDetail(leadId, true);
                    parent.ClosePopup();
                }
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
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#PrimaryPhone").css({ "border": "1px solid red" });
        }
        else {
            $("#PrimaryPhone").css({ "border": "1px solid #babec5" });
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
            $("#SSN").css({ "border": "1px solid #babec5" });
            ValueClean = Value;
        }
    }
    return ValueClean;
}

$(document).ready(function () {
    

    $("#PrimaryPhone").keyup(function () {
        var PhoneNumber = $(this).val();
            if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
                var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
    });


    $("#SSN").keyup(function () {
        var SSNNumber = $(this).val();
        if (SSNNumber != undefined && SSNNumber != null && SSNNumber != "") {
            var cleanSSNNumber = FormateSSNNumber(SSNNumber);
            $(this).val(cleanSSNNumber);
        }
    });

    $("#State").keyup(function () {
        var st = $("#State").val();
        if (st.length > 2) {
            $("#State").css({"border": "1px solid red"}); 
        }
        else{
            $("#State").css({ "border": "1px solid #babec5" });
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


        //var Checktitle = $("#title").val();
        var CheckFirstName = $("#FirstName").val();
        var CheckLastName = $("#LastName").val();
        //var CheckEmailAddress = $("#EmailAddress").val();
        var IsValid = true;

        $('#FirstName').each(function () {
            if ($.trim(CheckFirstName) == '') {
                IsValid = false;
                $(this).addClass("class-error");
            }
            else {
                $(this).addClass("class-valid");
            }
        });

        $('#LastName').each(function () {
            if ($.trim(CheckLastName) == '') {
                IsValid = false;
                $(this).addClass("class-error");
            }
            else {
                $(this).addClass("class-valid");
            }
        });

        $("#State").each(function () {
            var st = $("#State").val();
            if (st.length > 2) {
                IsValid = false;
                console.log("wrong");
                $("#State").css({ "border": "1px solid red" });
            }
            else {
                console.log("correct");
                $("#State").css({ "border": "1px solid #babec5" });
            }
        });

        if (IsValid == true) {
            SaveCustomer();
        }
    });
    $("#Cancel").click(function () {
        parent.ClosePopup();
    });

    /*$(".select_search").select2({})*/
});






