
var LoadEmergencyContactList = function () {
    $("#EmergencyContactList").load(domainurl + "/Leads/EmergencyContactListPartial?LeadId=" + CurrentCustomerId);
}
var OpenACHAddView = function () {
    OpenRightToLeftModal(domainurl + "/SmartLeads/ACHAddViewPaymentMethod?customerid=" + CurrentCustomerId);
}
var OpenCCAddView = function () {
    OpenRightToLeftModal(domainurl + "/SmartLeads/CCAddViewPaymentMethod?customerid=" + CurrentCustomerId);
}
var OpenCHKAddView = function () {
    OpenRightToLeftModal(domainurl + "/SmartLeads/CHKAddViewPaymentMethod?customerid=" + CurrentCustomerId);
}

var AddInvoiceOption = function (invoiceFor) {
    var url = domainurl + "/SmartLeads/SavePaymentMethod";
    var param = JSON.stringify({
        Id: 0,
        CardType: PaymentMethodInvoice,
        CardNumber: "",
        CardExpireDate: "",
        CardSecurityCode: "",
        AccountName: PaymentMethodInvoice,
        MethodType: PaymentMethodInvoice,
        CustomerId: CurrentCustomerId
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                if (typeof (data.PaymentProfileId) != "undefined") { 
                    if (invoiceFor == 'Package') {
                        $("#payment_profile_package").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'ServiceRMR') {
                        $("#payment_profile_mmr").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'Onetime') {
                        $("#payment_profile_onetime").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'ServiceToday') {
                        $("#payment_profile_service").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'Equipment') {
                        $("#payment_profile_equipment").val(data.PaymentProfileId);
                    }
                }
                else {
                    OpenFifthTab();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var AddPromoOption = function (invoiceFor) {
    var url = domainurl + "/SmartLeads/SavePaymentMethod";
    var param = JSON.stringify({
        Id: 0,
        CardType: PaymentMethodPromo,
        CardNumber: "",
        CardExpireDate: "",
        CardSecurityCode: "",
        AccountName: PaymentMethodPromo,
        MethodType: PaymentMethodPromo,
        CustomerId: CurrentCustomerId
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                if (typeof (data.PaymentProfileId) != "undefined") {
                    if (invoiceFor == 'Package') {
                        $("#payment_profile_package").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'ServiceRMR') {
                        $("#payment_profile_mmr").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'Onetime') {
                        $("#payment_profile_onetime").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'ServiceToday') {
                        $("#payment_profile_service").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'Equipment') {
                        $("#payment_profile_equipment").val(data.PaymentProfileId);
                    }
                }
                else {
                    OpenFifthTab();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var AddFinancedOption = function (invoiceFor) {
    var url = domainurl + "/SmartLeads/SavePaymentMethod";
    var param = JSON.stringify({
        Id: 0,
        CardType: PaymentMethodFinanced,
        CardNumber: "",
        CardExpireDate: "",
        CardSecurityCode: "",
        AccountName: PaymentMethodFinanced,
        MethodType: PaymentMethodFinanced,
        CustomerId: CurrentCustomerId
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                if (typeof (data.PaymentProfileId) != "undefined") {
                    if (invoiceFor == 'Package') {
                        $("#payment_profile_package").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'ServiceRMR') {
                        $("#payment_profile_mmr").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'Onetime') {
                        $("#payment_profile_onetime").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'ServiceToday') {
                        $("#payment_profile_service").val(data.PaymentProfileId);
                    }
                    else if (invoiceFor == 'Equipment') {
                        $("#payment_profile_equipment").val(data.PaymentProfileId);
                    }
                }
                else {
                    OpenFifthTab();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}



var DeletePaymentProfile = function (idval, guidval) {
    $.ajax({
        type: "POST",
        url: domainurl + "/SmartLeads/DeletePaymentProfile",
        data: JSON.stringify({ id: idval }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                //$(".prev_list_view").hide();
                //$(".LoadListViewPaymentMethod").load("/SmartLeads/FilterListViewPaymentMethod?customerid=" + guidval);
                //OpenPaymentMethodList();
                OpenFifthTab();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var AddOnFileOption = function (invoiceFor) {
    var url = domainurl + "/SmartLeads/SavePaymentMethod";
    var param = JSON.stringify({
        Id: 0,
        CardType: PaymentMethodOnFile,
        CardNumber: "",
        CardExpireDate: "",
        CardSecurityCode: "",
        AccountName: PaymentMethodOnFile,
        MethodType: PaymentMethodOnFile,
        CustomerId: CurrentCustomerId
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                if (typeof (data.PaymentProfileId) != "undefined") {
                    console.log(invoiceFor);
                    console.log(data.PaymentProfileId);

                    if (invoiceFor == 'ServiceToday') {
                        $("#payment_profile_service").val(data.PaymentProfileId);
                    }
                }
                else {
                    OpenFifthTab();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var AddCashOption = function (cashFor) {
    var url = domainurl + "/SmartLeads/SavePaymentMethod";
    var param = JSON.stringify({
        Id: 0,
        CardType: PaymentMethodCash,
        CardNumber: "",
        CardExpireDate: "",
        CardSecurityCode: "",
        AccountName: PaymentMethodCash,
        MethodType: PaymentMethodCash,
        CustomerId: CurrentCustomerId
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                if (typeof (data.PaymentProfileId) != "undefined") {
                    console.log(cashFor);
                    console.log(data.PaymentProfileId);

                    if (cashFor == 'Package') {
                        $("#payment_profile_package").val(data.PaymentProfileId);
                    }
                    else if (cashFor == 'ServiceRMR') {
                        $("#payment_profile_mmr").val(data.PaymentProfileId);
                    }
                    else if (cashFor == 'Onetime') {
                        $("#payment_profile_onetime").val(data.PaymentProfileId);
                    }
                    else if (cashFor == 'ServiceToday') {
                        $("#payment_profile_service").val(data.PaymentProfileId);
                    }
                    else if (cashFor == 'Equipment') {
                        $("#payment_profile_equipment").val(data.PaymentProfileId);
                    }
                }
                else {
                    OpenFifthTab();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var OpenPaymentMethodList = function () {
    OpenTopToBottomModal(domainurl + "/SmartLeads/ListViewPaymentMethod?customerid=" + CurrentCustomerId);
}
var OpenFifthTab = function () {
    $("#LoadLeadDetail").html(TabsLoaderText);
    $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
}

var SavePaymentProfile = function (Payfor, PaymentInfoId) {
    var url = domainurl + "/Leads/SavePaymentInfoCustomer";
    var Param = JSON.stringify({
        CustomerId: CurrentCustomerId,
        PaymentInfoId: PaymentInfoId,
        Type: $("#payment_profile_package").val(),
        Payfor: Payfor,
        ForMonths: $("#ServiceMonthCount").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                //OpenSuccessMessageNew('Success!', data.message, function () {
                //    $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
                //});
            }
            else {
                OpenErrorMessageNew('Error!', data.message, function () {

                });
            }
            /*Discount Value Area*/
            if (data.IsACH) {
                $(".ACHDiscountDiv").removeClass('hidden');
                var DiscountAmount = parseFloat($(".ServiceDiscount").attr('total-dis'));
                var SubTotalDis = $(".ServiceSubTotal").attr('sub-total');
                var TotalTax = $(".ServiceTaxTotal").attr('total-tax');
                var MnthVal = parseFloat($("#ServiceMonthCount").val());
                if (DiscountAmount > 0) {
                    SubTotalDis = parseFloat(SubTotalDis) - DiscountAmount;
                    $(".ServiceSubTotal").text(SubTotalDis.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                    TotalTax = (SubTotalDis * parseFloat(TaxSales)) / 100;
                    $(".ServiceTaxTotal").text(TotalTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                    var subMonthlyTotalAmount = SubTotalDis + TotalTax;
                    $(".MonthlyTotalAmount").text(subMonthlyTotalAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                    var Total = MnthVal * (parseFloat(TotalTax) + SubTotalDis);
                    $(".ServiceTotalAmount").text(Total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }
              
            }
            else {
                $(".ACHDiscountDiv").addClass('hidden');
            }
           // $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var discountAmount_Equip = 0.00;
var DiscountType_Equip = '';
// for save setupdata on capturepayment 
var FinalCustomerSetupData1 = function () {
    console.log("FinalCustomerSetupData");
    var url = domainurl + "/SmartLeads/FinalSmartCustomerSetupData";
    var param = JSON.stringify({
        setupid: LeadIdVal,
        contrcatType: $("#ContractType").val(),
        discountAmount: discountAmount_Equip.toString(),
        DiscountType: DiscountType_Equip
    })
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                if (Device.iPad()) {
                  OpenInstallationAgreement();
                  $(".LoadAgreementPopUp1")[0].click();
                }
                else {
                  OpenInstallationAgreement();
                }
            }
            else {
                OpenErrorMessageNew('Error!', data.message, "");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var ReceivePayments = function () {

    FinalCustomerSetupData1();
    var activationMsg = "";
    var serMsg = "";
    var eqpMsg = "";
    var activationValue = $(".ActivationTotalAmount").text().trim();
    if (activationValue != "$0.00") {
        activationMsg = String.format("Activation: {0}", activationValue);
    }
    var serValue = $(".ServiceTotalAmount2").text().trim();
    if (serValue != "$0.00") {
        serMsg = String.format("Services: {0}", serValue);
    }
    var eqpValue = $(".EquipmentTotalAmount").text().trim();
    if (eqpValue != "$0.00") {
        eqpMsg = String.format("Equipment: {0}", eqpValue);
    }

    if ($("#ContractType").val() == '-1' && $("#step").val() == "5") {
        OpenErrorMessageNew("Error!", "Please Select Contract Type");
    }
    else if ($("#TblEmergencyContactList tbody tr").length < parseInt(contactreq) && $("#step").val() == "5") {
            OpenErrorMessageNew("Error!", "You need to add at least " + contactreq + " emergency contacts to proceed.");
    }
    else
    {

    var message = String.format("Payment will be captured for {0} {1} {2}", activationMsg, serMsg, eqpMsg);
    if (($("#payment_profile_package").is(":visible") && $("#payment_profile_package").val() == '-1') ||
        ($("#payment_profile_onetime").is(":visible") && $("#payment_profile_onetime").val() == '-1') ||
        ($("#payment_profile_service").is(":visible") && $("#payment_profile_service").val() == '-1') ||
        ($("#payment_profile_equipment").is(":visible") && $("#payment_profile_equipment").val() == '-1') &&
        $("#step").val() == "5"
    ) {
       
        
            CommonUiValidation();
            OpenErrorMessageNew("Error!", "Please Select Payment Method");
       
        }
       else
        {
                OpenConfirmationMessageNew("Confirmation", message, function () {
               
            ReceivePaymentsConfirm();
        });
        }
    }
}
var ReceivePaymentsConfirm = function () {
    var url = domainurl + "/SmartLeads/ReceivePayment";

    var Param = JSON.stringify({
        CustomerId: CurrentCustomerId,
        AdvancedPaymentMonths: $("#ServiceMonthCount").val(),
        PaymentInfoCustomerPackage: $("#payment_profile_package").val(),
        PaymentInfoCustomerService: $("#payment_profile_service").val(),
        PaymentInfoCustomerEquipment: $("#payment_profile_equipment").val(),


        //MAYUR: for updatting discount in invoice while capture payment ::start
        PaymentInfoTax: $("#EqpTax").val(),
        PaymentInfoDiscountType: $("#EqpDiscountType").val(), 
        PaymentInfoDiscountQty: $("#EqpDiscountQty").val(),
        PaymentInfoDiscountAmount: $("#EqpDiscountQty").val(),
        PaymentInfoDiscountPercentage:$("#EqpDiscountQty").val(),
        PaymentInfoFinalAmount: $("#Eqpfinalamount").val(), 
        // "MAYUR" : for updatting discount in invoice while capture payment ::End

        
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".AgreementSummaryLoader").removeClass("hidden"),
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".AgreementSummaryLoader").addClass("hidden");
            if (data.result) {
                if (data.caution) {
                    OpenCautionMessageNew('Caution!', data.message, function () {
                        if (typeof (data.customerconvert) != "undefined" && data.customerconvert == true) {
                            window.location.href = domainurl + "/Customer/Customerdetail/?id=" + CurrentCustomerIntId;
                        }
                        else if (typeof (data.customerconvert) != "undefined" && data.customerconvert == false) {
                            window.location.href = domainurl + "/Lead/Leadsdetail/?id=" + CurrentCustomerIntId;
                        }
                        $("#LoadLeadDetail").html(TabsLoaderText);
                        $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
                        FinalCustomerSetupData();
                    });
                } else {
                    OpenSuccessMessageNew('Success!', data.message, function () {
                        //if (typeof (data.customerconvert) != "undefined" && data.customerconvert == true) {payment_profile_mmr
                        //    window.location.href = domainurl + "/Customer/Customerdetail/?id=" + CurrentCustomerIntId;
                        //}
                        //else if (typeof (data.customerconvert) != "undefined" && data.customerconvert == false) {
                        //    window.location.href = domainurl + "/Lead/Leadsdetail/?id=" + CurrentCustomerIntId;
                        //}
                        //$("#LoadLeadDetail").html(TabsLoaderText);
                        //$("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
                        FinalCustomerSetupData();
                        $("#btnPayNow").addClass("hidden ");
                        $(".leadToCustomerConvertForAgreement ").removeClass("hidden");
                    });
                }

            } else {
                OpenErrorMessageNew('Error!', data.message, function () {
                    $("#LoadLeadDetail").html(TabsLoaderText);
                    $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
            $(".AgreementSummaryLoader").addClass("hidden");
        }
    });
}

var CalculateDate = function () {
    var PromoMonth = $("#Promomonth").val();
    var PrepaidMonth = $("#Prepaidmonth").val();
    var TotalMonth = parseInt(PromoMonth) + parseInt(PrepaidMonth);
    //var DateTime = new Date(DefaultDate);
    //var CalculatedDate = DateTime.addMonth(TotalMonth);
    //var DateString = CalculatedDate.getMonth() + 1 + "/" + CalculatedDate.getDate() + "/" + CalculatedDate.getFullYear();
    //$("#StartDate").val(DateString);
    //$("#FinishDate").val(DateString);
    DateTime = new Date(DefaultPayEffectiveDate);
    var PayDate = DateTime.addMonth(TotalMonth);
    var PayDateString = PayDate.getMonth() + "/" + PayDate.getDate() + "/" + PayDate.getFullYear();
    $("#PaymentDate").val(PayDateString);
}

var SendEcontractSurvey = function () {
    OpenTopToBottomModal("/API/SendEcontractWithSurvey?CustomerId=" + CurrentCustomerId+"&from=lead");
}
var SendEcontractISPC = function () {
    OpenTopToBottomModal("/API/SendISPCEcontract?CustomerId=" + CurrentCustomerId );
}
var SetFirstPaymentMethod = function () {
    $(".setfirstvalue").each(function () {
        var values = [];
        $(this).find("option").each(function () {
            values.push($(this).attr('value'));
        });
        if (values.length > 1 && $(this).val() == "-1") {
            $(this).val(values[1]);
        }
    });
}
var updateActivationFee = function (e) {
    var PackageId = e;
    var url = domainurl + "/SmartLeads/UpdateActivationFee/";
    var param = JSON.stringify({
        CustomerId: e,
        ActivationFee: $("#ActivationFee").val()
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
            console.log(data);
            if (data.result == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", "", function () {
                    $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartAgreementSummary?id=" + LeadIdVal);

                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var updatelabourFee = function (e) {
    var PackageId = e;
    var url = domainurl + "/SmartLeads/UpdateLabourFee/";
    var param = JSON.stringify({
        CustomerId: e,
        LabourFee: $("#LabourFee").val()
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
            console.log(data);
            if (data.result == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", "", function () {
                    $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartAgreementSummary?id=" + LeadIdVal);

                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var GetFinanceStatus = function () {

    var url = domainurl + "/SmartLeads/GetIsPcFinanceStatus";
    var param = JSON.stringify({
        CustomerId: CurrentCustomerId,
        ApplicationId: IsPcAppId,

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
            console.log(data);
            if (data == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", data.Message, function () {
                  

                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var updateNonCoformingFee = function (e) {
    var PackageId = e;
    var url = domainurl + "/SmartLeads/UpdateNonConformingFee";
    var param = JSON.stringify({
        CustomerId: e,
        NonConformingFee: $("#NonConformingFee").val()
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
            console.log(data);
            if (data == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", "", function () {
                    $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartAgreementSummary?id=" + LeadIdVal);
                    //$("#edit-nonconformin-fee").addClass("hidden");
                    //$("#nonconformingFeelabel").removeClass("hidden");
                    //$("#update-nonconforming-fee").removeClass("hidden");
                    //var NonConformingValue=parseFloat($("#NonConformingFee").val());
                    //$("#nonconformingFeelabel").html(Currency + NonConformingValue.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'))
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () { 

    PaymentDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#PaymentDate')[0],
        trigger: $('#PaymentDateCustom')[0],
        firstDay: 1
    });
    InstallStartDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#StartDate')[0],
        trigger: $('#InstallStartDateCustom')[0],
        firstDay: 1
    });
    InstallFinishDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#FinishDate')[0],
        trigger: $('#InstallFinishDateCustom')[0],
        firstDay: 1
    });
    $("#StartDate").val(DefaultDate);
    $("#FinishDate").val(DefaultDate);
    $("#PaymentDate").val(DefaultPayEffectiveDate);
    $("#Promomonth").change(function () {
        CalculateDate();
    })
    $("#Prepaidmonth").change(function () {
        CalculateDate();
    })

    $(".addFinance_button").click(function () {
        OpenTopToBottomModal("/SmartLeads/AddFinance?CustomerId=" + CurrentCustomerId+"&from=Lead");
    })
    $(".addFinancehpowerpay_button").click(function () {
        OpenTopToBottomModal("/SmartLeads/AddFinancePowerPay?CustomerId=" + CurrentCustomerId);
    })
    if (IsPcAppId == '') {
        $(".finance_status_button").hide();
    }
   
    $(".finance_status_button").click(function () {
        GetFinanceStatusbtnSign();
        //GetFinanceStatus();
    })
    if (IsAgreementSend == "True") {
        $(".leadToCustomerConvertForAgreement").removeClass('hidden');
    }
    LoadEmergencyContactList();
    //SetFirstPaymentMethod();
    console.log("save&next");
    if ((MoveCusId != "" && MoveCusId != "00000000-0000-0000-0000-000000000000") || ((AbleToReceivePayment == "False") || HasAdminPermission == "True")) {
        $("#btnSavandNex").removeClass("hidden");
    } else {
        $("#btnSavandNex").addClass("hidden");
    }

    $("#update-nonconforming-fee").click(function () {
        $("#edit-nonconformin-fee").removeClass("hidden");
        $("#nonconformingFeelabel").addClass("hidden");
        $("#update-nonconforming-fee").addClass("hidden");
    });

    $("#exit-nonconforming").click(function () {
        $("#edit-nonconformin-fee").addClass("hidden");
        $("#nonconformingFeelabel").removeClass("hidden");
        $("#update-nonconforming-fee").removeClass("hidden");
    })
    $("#update-activation-fee").click(function () {
        $("#edit-activatio-fee").removeClass("hidden");
        $("#activationFeelabel").addClass("hidden");
        $("#update-activation-fee").addClass("hidden");
    });
    $("#update-labour-fee").click(function () {
        $("#edit-labour-fee").removeClass("hidden");
        $("#labourFeelabel").addClass("hidden");
        $("#update-labour-fee").addClass("hidden");
    });
    $("#exit-activation").click(function () {
        $("#edit-activatio-fee").addClass("hidden");
        $("#activationFeelabel").removeClass("hidden");
        $("#update-activation-fee").removeClass("hidden");
    })
    $("#exit-lobour").click(function () {
        $("#edit-labour-fee").addClass("hidden");
        $("#labourFeelabel").removeClass("hidden");
        $("#update-labour-fee").removeClass("hidden");
    })
    var strTotalAmt = parseFloat(TotalAmount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    var strCollectedAmt = parseFloat(CollectedAmount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    var strAchPayment = AchPayment;
    var strCreditCardPayment = CreditCardPayment;
    var strCashPayment = CashPayment;
    var strCheckPayment = CheckPayment;
    var strInvoicePayment = InvoicePayment;
    var strPayInfo = "";
    console.log(CreditCardPayment);
    if (CreditCardPayment != '0') {
        strPayInfo += "CC: " + Currency + strCreditCardPayment + ", ";
    }
    if (AchPayment != '0') {
        strPayInfo += "ACH: " + Currency + AchPayment + ", ";
    }
    if (CashPayment != '0') {
        strPayInfo += "Cash: " + Currency + CashPayment + ", ";
    }
    if (CheckPayment != '0') {
        strPayInfo += "Check: " + Currency + CheckPayment + ", ";
    }
    if (strPayInfo != "") {
        strPayInfo = strPayInfo.slice(0, -2);
        var strAmountText = "Total amount to be collected: " + Currency + strTotalAmt + "<br/>";
        console.log(strCollectedAmt);
        if (strCollectedAmt != "0.00") {
            strAmountText += " Collecting today: " + Currency + strCollectedAmt + " (" + strPayInfo + ")";
        }

        $(".total_captured_amount").html(strAmountText);
    }
    else {

        var strAmountText = "Total amount to be collected: " + Currency + strTotalAmt + "<br/>";
        console.log(strCollectedAmt);
        if (strCollectedAmt != "0.00") {
            strAmountText += " Collecting today: " + Currency + strCollectedAmt;
        }
        $(".total_captured_amount").html(strAmountText);
    }

    $("#payment_profile_package").change(function () {
        if ($("#payment_profile_package").val() != "-1") {
            SavePaymentProfile("Activation Fee", $("#payment_profile_package").val());
        }
    });
    $("#payment_profile_equipment").change(function () {
        if ($("#payment_profile_equipment").val() != "-1") {
            SavePaymentProfile("Equipment", $("#payment_profile_equipment").val());
        }
    });
    $("#payment_profile_service").change(function () {
        if ($("#payment_profile_service").val() != "-1") {
            SavePaymentProfile("Service", $("#payment_profile_service").val());
        }
    });
    $("#payment_profile_mmr").change(function () {
        if ($("#payment_profile_mmr").val() != "-1") {
            SavePaymentProfile("MMR", $("#payment_profile_mmr").val());
        }
    });
    $("#payment_profile_onetime").change(function () {
        if ($("#payment_profile_onetime").val() != "-1") {
            SavePaymentProfile("Onetime", $("#payment_profile_onetime").val());
        }
    });
    //$("#SaveActivationFeePayment").click(function(){
    //    if($("#payment_profile_package").val()=="-1"){
    //        OpenErrorMessageNew("","Select a payment method first.");
    //    }else{
    //        SavePaymentProfile("Activation Fee" ,$("#payment_profile_package").val() );
    //    }
    //});
    //$("#SaveEquipmentPayment").click(function(){
    //    if($("#payment_profile_equipment").val()=="-1"){
    //        OpenErrorMessageNew("","Select a payment method first.");
    //    }else{
    //        SavePaymentProfile("Equipment" ,$("#payment_profile_equipment").val());
    //    }
    //});
    $("#SaveServicePayment").click(function () {
        if ($("#payment_profile_service").val() == "-1") {
            OpenErrorMessageNew("", "Select a payment method first.");
        } else {
            SavePaymentProfile("Service", $("#payment_profile_service").val());
        }
    });
    //$("#SaveMMRPaymentInfo").click(function(){
    //    if($("#payment_profile_mmr").val() == "-1"){
    //        OpenErrorMessageNew("","Select a payment method first.");
    //    }else{
    //        SavePaymentProfile("MMR" ,$("#payment_profile_mmr").val());
    //    }
    //});
    $(".profile_item_delete").click(function () {
        var idval = $(this).attr('data-id');
        var guidval = $(this).attr('data-guid');
        OpenConfirmationMessageNew("Confirmation", "Are you sure, you want to delete this item", function () {
            DeletePaymentProfile(idval, guidval);
        })
    });
    $(".IsNFTTicket").change(function () {
        var url = domainurl + "/SmartLeads/SetNFTTicket";
        var param = JSON.stringify({
            CustomerId: CustomerId,
            IsNFTTicket: $(this).is(":checked")
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
                console.log(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    $("#ServiceMonthCount").change(function (e) {
        var MnthVal = $(e.target).val();
        var TotalAmount = $(e.target).attr('total-amount');
        if (MnthVal < 1) {
            $(e.target).addClass("required");
        } else {
            $(e.target).removeClass("required");
        }
        if ($(".ACHDiscountDiv").hasClass('hidden')) {
            var TotalTax = $(".ServiceTaxTotal").attr('total-tax');
            var Subtotal = $(".ServiceSubTotal").attr('sub-total');

            var Total = parseInt(MnthVal) * (parseFloat(TotalTax) + parseFloat(Subtotal));
            $(".ServiceTotalAmount").text(Total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }
        else {
            var discountAmount = $(".ServiceDiscount").attr('total-dis');
            if (parseFloat(discountAmount) > 0) {
                var Subtotal = $(".ServiceSubTotal").attr('sub-total');
                Subtotal = parseFloat(Subtotal) - parseFloat(discountAmount);
                var TotalTax = Subtotal * (parseFloat(TaxSales) / 100);
                var Total = parseInt(MnthVal) * (parseFloat(TotalTax) + Subtotal);

                $(".ServiceTotalAmount").text(Total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
        }
        var pps = $("#payment_profile_service").val();
        if (pps == undefined) {
            pps = 9999999;
        }
        SavePaymentProfile("Service", pps);
    });
    if (AbleToReceivePayment.toLowerCase() == 'true') {
        $("#btnPayNow").removeClass("hidden2");
    } else {
        $("#btnPayNow").addClass("hidden2");
    }
    if ($("#payment_profile_mmr").val() != "-1") {
        var spval = $("#payment_profile_mmr option:selected").text().split('_');
        if (spval.length > 0) {
            if (spval[0] == "ACH") {
                $(".ACHDiscountDiv").removeClass('hidden');
                var DiscountAmount = parseFloat($(".ServiceDiscount").attr('total-dis'));
                var SubTotalDis = $(".ServiceSubTotal").attr('sub-total');
                var TotalTax = $(".ServiceTaxTotal").attr('total-tax');
                var MnthVal = parseFloat($("#ServiceMonthCount").val());
                if (DiscountAmount > 0) {
                    SubTotalDis = parseFloat(SubTotalDis) - DiscountAmount;
                    $(".ServiceSubTotal").text(SubTotalDis.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                    TotalTax = (SubTotalDis * parseFloat(TaxSales)) / 100;
                    $(".ServiceTaxTotal").text(TotalTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                    var subMonthlyTotalAmount = SubTotalDis + TotalTax;
                    $(".MonthlyTotalAmount").text(subMonthlyTotalAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                    var Total = MnthVal * (parseFloat(TotalTax) + SubTotalDis);
                    $(".ServiceTotalAmount").text(Total.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }
            }
            else {
                $(".ACHDiscountDiv").addClass('hidden');
            }
        }
        else {
            $(".ACHDiscountDiv").addClass('hidden');
        }
    }
    else {
        $(".ACHDiscountDiv").addClass('hidden');
    }

    $("#ddlEquipDiscount").change(function () {
        calculateFinalEquipmentAmount();
    });
    $("#discountAmount").blur(function () {

         
            calculateFinalEquipmentAmount();
        
    })
    $("#discountAmount").val(parseFloat($("#discountAmount").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    
    calculateFinalEquipmentAmount();

});
function calculateFinalEquipmentAmount() {

    // "MAYUR" : for updatting discount change in Smart summary ::start
    var ActivationTotalAmount = $("#ActivationTotalAmount").val().replace(" ", "");
        ActivationTotalAmount.replace("$", "");
    var ServiceTotalAmount = $(".ServiceTotalAmount").text().replace("$", "");
    // "MAYUR" : for updatting discount change in Smart summary ::End

    var DiscountTypeVal = $("#ddlEquipDiscount").val();
    var DiscountQuantityVal = $("#discountAmount").val();
    var discountAmount = 0;

    var preTotalAmount = $("#hdEqpRetailTotalAmount").val();
    var preTotalTax = $("#hdEqpTotalTax").val();
    var preTotalDiscount = $("#hdEqpDiscountTotal").val();
    var preTotalPriceTotalAmount = $("#hdEqpPriceTotalAmount").val();

    var finalTax = 0.00;
    var finalsubTotalWithDiscount = 0.00
    var finalTotalAfterTax = 0.00
    var salesTax = $("#hdSalesTax").val();

    if (DiscountQuantityVal.trim() != "" && DiscountQuantityVal.trim() > 0) {
        discountAmount = DiscountQuantityVal;
    }

    else {
        discountAmount = 0;
        $("#discountAmount").val('');
    }

    if (DiscountTypeVal == "amount") {
        finalsubTotalWithDiscount = parseFloat(preTotalPriceTotalAmount) - parseFloat(discountAmount);
        finalTax = (parseFloat(salesTax) * parseFloat(finalsubTotalWithDiscount)) / 100;
        finalTotalAfterTax = finalsubTotalWithDiscount + finalTax;
        $("#lblUserDiscount").text(TransMakeCurrency + parseFloat(discountAmount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));


    }

    if (DiscountTypeVal == "percent") {
        finalsubTotalWithDiscount = parseFloat(preTotalPriceTotalAmount) - ((parseFloat(discountAmount) * parseFloat(preTotalPriceTotalAmount)) / 100);
        finalTax = (parseFloat(salesTax) * parseFloat(finalsubTotalWithDiscount)) / 100;
        finalTotalAfterTax = finalsubTotalWithDiscount + finalTax;
        $("#lblUserDiscount").text(TransMakeCurrency + ((parseFloat(discountAmount) * parseFloat(preTotalPriceTotalAmount)) / 100).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    }
    //$("#hdEqpPriceTotalAmount").val(finalsubTotalWithDiscount);
    //$("#hdEqpFinalTotal").val(finalTotalAfterTax);
    //$("#hdEqpPriceTotalAmount").val(finalsubTotalWithDiscount);

    $("#lblTaxAmount").text(TransMakeCurrency + finalTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))
    $("#lblFinalAmount").text(TransMakeCurrency + finalTotalAfterTax.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))
    $("#lblSubAfterDiscount").text(TransMakeCurrency + parseFloat(finalsubTotalWithDiscount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));


   

     // "MAYUR" : for updatting discount change in Smart summary ::start
    var strAmountText = parseFloat(ServiceTotalAmount) + parseFloat(ActivationTotalAmount) + parseFloat(finalTotalAfterTax);

    var strAmountText = "Total amount to be collected: " + TransMakeCurrency + parseFloat(strAmountText).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
       
    $(".total_captured_amount").html(strAmountText);

    // "MAYUR" : for updatting discount change in Smart summary ::End
            
    discountAmount_Equip = discountAmount;
    DiscountType_Equip = DiscountTypeVal;

     // "MAYUR" : for updatting discount in invoice while capture payment ::start
    Taxamount = $("#lblTaxAmount").text().replace("$","");
    UserDiscount = $("#lblUserDiscount").text().replace("$", "");
    Eqpfinalamount = $("#lblFinalAmount").text().replace("$", "");


    $("#EqpTax").val(Taxamount);
    $("#EqpDiscountPercentage").val(UserDiscount);
    $("#EqpDiscountAmount").val(UserDiscount);
    $("#Eqpfinalamount").val(Eqpfinalamount);
    $("#EqpDiscountQty").val(DiscountQuantityVal);
    $("#EqpDiscountType").val(DiscountTypeVal);
    // "MAYUR" : for updatting discount in invoice while capture payment ::start
}
 