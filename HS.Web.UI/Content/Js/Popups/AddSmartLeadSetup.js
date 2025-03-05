var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var deviceEquipmentList = [];
var CurrentPackageLimit;
var valEquipOptional = [];
var EachvalEquip;
var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}

var MakePaymentMethodParam = function (method) {
    if (method == "EFT") {
        var MMR_EFT_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),
            AccountName: $("#EFT_PaymentInfo_AccountName").val(),
            BankAccountType: $("#EFT_PaymentInfo_BankAccountType").val(),
            RoutingNo: $("#EFT_PaymentInfo_RoutingNo").val(),
            AcountNo: $("#EFT_PaymentInfo_AcountNo").val(),
            CardType: '',
            CardNumber: '',
            CardExpireDate: '',
            CardSecurityCode: '',
            CheckNo: '',
            IsCash: false,
        };
        return MMR_EFT_PaymentInfoModelFromJs;
    }
    if (method == "ACH") {
        var MMR_ACH_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),

            AccountName: $("#ACH_PaymentInfo_AccountName").val(),
            BankAccountType: $("#ACH_PaymentInfo_BankAccountType").val(),
            EcheckType: $("#ACH_PaymentInfo_EcheckType").val().trim(),
            RoutingNo: $("#ACH_PaymentInfo_RoutingNo").val(),
            AcountNo: $("#ACH_PaymentInfo_AcountNo").val(),
            BankName: $("#ACH_PaymentInfo_BankName").val(),
            CardType: '',
            CardNumber: '',
            CardExpireDate: '',
            CardSecurityCode: '',
            CheckNo: '',
            IsCash: false
        };
        return MMR_ACH_PaymentInfoModelFromJs;
    }
    if (method == "Check") {
        var MMR_Check_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),

            AccountName: '',
            BankAccountType: '',
            RoutingNo: '',
            AcountNo: '',
            CardType: '',
            CardNumber: '',
            CardExpireDate: '',
            CardSecurityCode: '',
            CheckNo: $("#Check_PaymentInfo_CheckNo").val(),
            IsCash: false
        };
        return MMR_Check_PaymentInfoModelFromJs;
    }
    if (method == "Credit Card") {
        var MMR_CreditCard_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),

            AccountName: $("#CreditCard_PaymentInfo_AccountName").val(),
            BankAccountType: '',
            RoutingNo: '',
            AcountNo: '',
            CardType: $("#CreditCard_PaymentInfo_CardType").val(),
            CardNumber: $("#CreditCard_PaymentInfo_CardNumber").val(),
            CardExpireDate: $("#CreditCard_PaymentInfo_CardExpireDate").val(),
            CardSecurityCode: $("#CreditCard_PaymentInfo_CardSecurityCode").val(),
            CheckNo: '',
            IsCash: false
        };
        return MMR_CreditCard_PaymentInfoModelFromJs;
    }
    if (method == "Debit Card") {
        var MMR_DebitCard_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),

            AccountName: $("#DebitCard_PaymentInfo_AccountName").val(),
            BankAccountType: '',
            RoutingNo: '',
            AcountNo: '',
            CardType: $("#DebitCard_PaymentInfo_CardType").val(),
            CardNumber: $("#DebitCard_PaymentInfo_CardNumber").val(),
            CardExpireDate: $("#DebitCard_PaymentInfo_CardExpireDate").val(),
            CardSecurityCode: $("#DebitCard_PaymentInfo_CardSecurityCode").val(),
            CheckNo: '',
            IsCash: false
        };
        return MMR_DebitCard_PaymentInfoModelFromJs;
    }
}
var PaymentMethodValidation = function (method) {
    var result = true;
    if (method == "ACH") {
        if ($("#ACH_PaymentInfo_AccountName").val().trim() == "") {
            result = false;
        }
        if ($("#ACH_PaymentInfo_BankAccountType").val().trim() == "-1") {
            result = false;
        }
        if ($("#ACH_PaymentInfo_EcheckType").val().trim() == "-1") {
            result = false;
        }
        if ($("#ACH_PaymentInfo_RoutingNo").val().trim() == "") {
            result = false;
        }
        if ($("#ACH_PaymentInfo_AcountNo").val().trim() == "") {
            result = false;
        }
    } else if (method == "Debit Card") {
        //if($("#DebitCard_PaymentInfo_CardType").val().trim() == ""){
        //    result = false;
        //}
        if ($("#DebitCard_PaymentInfo_CardNumber").val().trim() == "") {
            result = false;
        }
        if ($("#DebitCard_PaymentInfo_CardExpireDate").val().trim() == "") {
            result = false;
        }
        if ($("#DebitCard_PaymentInfo_CardSecurityCode").val().trim() == "") {
            result = false;
        }
    }
    else if (method == "Credit Card") {
        //if($("#CreditCard_PaymentInfo_CardType").val().trim() == ""){
        //    result = false;
        //}
        if ($("#CreditCard_PaymentInfo_CardNumber").val().trim() == "") {
            result = false;
        } else {
            var res = $('#CreditCard_PaymentInfo_CardNumber').validateCreditCard();
            result = res.valid;
        }
        if ($("#CreditCard_PaymentInfo_CardExpireDate").val().trim() == "") {
            result = false;
        }
        if ($("#CreditCard_PaymentInfo_CardSecurityCode").val().trim() == "") {
            result = false;
        }
        if ($("#CreditCard_PaymentInfo_AccountName").val().trim() == "") {
            result = false;
        }
    }
    return result;
}

var MakeAFParam = function (method) {
    if (method == "EFT") {
        var AF_EFT_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),
            AccountName: $("#EFT_ActivationFeePaymentInfoModel_AccountName").val(),
            BankAccountType: $("#EFT_ActivationFeePaymentInfoModel_BankAccountType").val(),
            RoutingNo: $("#EFT_ActivationFeePaymentInfoModel_RoutingNo").val(),
            AcountNo: $("#EFT_ActivationFeePaymentInfoModel_AcountNo").val(),
            CardType: '',
            CardNumber: '',
            CardExpireDate: '',
            CardSecurityCode: '',
            CheckNo: '',
            IsCash: false
        };
        return AF_EFT_PaymentInfoModelFromJs;
    }
    if (method == "ACH") {
        var AF_ACH_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),
            AccountName: $("#ACH_ActivationFeePaymentInfoModel_AccountName").val(),
            BankAccountType: $("#ACH_ActivationFeePaymentInfoModel_BankAccountType").val(),
            RoutingNo: $("#ACH_ActivationFeePaymentInfoModel_RoutingNo").val(),
            AcountNo: $("#ACH_ActivationFeePaymentInfoModel_AcountNo").val(),
            CardType: '',
            CardNumber: '',
            CardExpireDate: '',
            CardSecurityCode: '',
            CheckNo: '',
            IsCash: false
        };
        return AF_ACH_PaymentInfoModelFromJs;
    }
    if (method == "Check") {
        var AF_Check_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),
            AccountName: '',
            BankAccountType: '',
            RoutingNo: '',
            AcountNo: '',
            CardType: '',
            CardNumber: '',
            CardExpireDate: '',
            CardSecurityCode: '',
            CheckNo: $("#Check_ActivationFeePaymentInfoModel_CheckNo").val(),
            IsCash: false
        };
        return AF_Check_PaymentInfoModelFromJs;
    }
    if (method == "Credit Card") {
        var AF_CreditCard_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),
            AccountName: $("#CreditCard_ActivationFeePaymentInfoModel_AccountName").val(),
            BankAccountType: '',
            RoutingNo: '',
            AcountNo: '',
            CardType: $("#CreditCard_ActivationFeePaymentInfoModel_CardType").val(),
            CardNumber: $("#CreditCard_ActivationFeePaymentInfoModel_CardNumber").val(),
            CardExpireDate: $("#CreditCard_ActivationFeePaymentInfoModel_CardExpireDate").val(),
            CardSecurityCode: $("#CreditCard_ActivationFeePaymentInfoModel_CardSecurityCode").val(),
            CheckNo: '',
            IsCash: false
        };
        return AF_CreditCard_PaymentInfoModelFromJs;
    }
    if (method == "Debit Card") {
        var AF_DebitCard_PaymentInfoModelFromJs = {
            Id: $("#Id").val(),
            BillMethod: method,
            PaymentCustomerId: $("#LeadContractCustomerID").val(),
            AccountName: $("#DebitCard_ActivationFeePaymentInfoModel_AccountName").val(),
            BankAccountType: '',
            RoutingNo: '',
            AcountNo: '',
            CardType: $("#DebitCard_ActivationFeePaymentInfoModel_CardType").val(),
            CardNumber: $("#DebitCard_ActivationFeePaymentInfoModel_CardNumber").val(),
            CardExpireDate: $("#DebitCard_ActivationFeePaymentInfoModel_CardExpireDate").val(),
            CardSecurityCode: $("#DebitCard_ActivationFeePaymentInfoModel_CardSecurityCode").val(),
            CheckNo: '',
            IsCash: false
        };
        return AF_DebitCard_PaymentInfoModelFromJs;
    }
}

var SaveLeadSetupPackageOption = function () {
    var LeadPackageDetailList = [];
    var EquipmentListForWorkOrder = [];
    var ServiceList = [];
    var EquipmentPartialProductsList = [];
    var SmartPackageEquipmentServiceEquipmentList = [];

    var FirstBillDate = picker.getDate();
    if (CommonUiValidation()) {
        $('.Package-Include-Equipments').each(function () {
            var tmpIncludeEquipmentId = $(this).attr('id-val');
            var IncludeEquipmentId = $(this).attr('id-PackageEqpId');
            var PacId = $(this).attr('idval-PackageId');
            var IncludeEquipmentNumber = $(this).attr('id-EquipNum');
            EquipmentListForWorkOrder.push({
                SelectedEquipmentId: tmpIncludeEquipmentId,
                SelectedEquipmentPrice: '0.0',
                SelectedEquipmentIsFree: true,
                IsIncluded: true,
                IsOptionalEqp: false,
                IsDevice: false,
                NumOfEquipments: IncludeEquipmentNumber
            });
        });
        $('.Package-Include-Service').each(function () {
            var EquipmentId = $(this).attr('id-val');
            var PacId = $(this).attr('idval-PackageId');
            var SmartPrice = $(this).attr('service-price');
            ServiceList.push({
                EquipmentId: EquipmentId,
                PackageId: PacId,
                MonthlyRate: SmartPrice,
                Total: SmartPrice
            });
        });
        $('.device-equipments').each(function () {
            if ($(this).prop('checked') == true) {
                if (deviceEquipmentList.length <= CurrentPackageLimit) {
                    var deviceEquipmentId = $(this).attr('idval');
                    var deviceEquipmentPrice = $(this).attr('id-eqpPrice');
                    var deviceEquipmentIsFree = $(this).attr('id-eqpIsFree');
                    var IncludeEquipmentId = $(this).attr('id-PackageEqpId');
                    var PacId = $(this).attr('idval-PackageId');
                    var DeviceEptNo = $(this).attr('id-EptNo');
                    EquipmentListForWorkOrder.push({
                        SelectedEquipmentId: deviceEquipmentId,
                        SelectedEquipmentPrice: deviceEquipmentPrice,
                        SelectedEquipmentIsFree: deviceEquipmentIsFree,
                        IsIncluded: false,
                        IsOptionalEqp: false,
                        IsDevice: true,
                        NumOfEquipments: DeviceEptNo
                    });
                }
            }
        });
        $(".text-numoptional").each(function () {
            valEquipOptional.push($(this).val());
        });
        var count = 0;
        $('.optional-equipments').each(function () {
            if ($(this).prop('checked') == true) {
                var optionalEquipmentId = $(this).attr('idval');
                var optionalEquipmentPrice = $(this).attr('id-eqpPrice');
                var optionalEquipmentIsFree = $(this).attr('id-eqpIsFree');
                var IncludeEquipmentId = $(this).attr('id-PackageEqpId');
                var PacId = $(this).attr('idval-PackageId');
                EquipmentListForWorkOrder.push({
                    SelectedEquipmentId: optionalEquipmentId,
                    SelectedEquipmentPrice: optionalEquipmentPrice,
                    SelectedEquipmentIsFree: optionalEquipmentIsFree,
                    IsIncluded: false,
                    IsOptionalEqp: true,
                    IsDevice: false,
                    NumOfEquipments: valEquipOptional[count]
                });
            }
            count++;
        });

        $(".service-equipments:checked").each(function () {
            var serviceEqpId = $(this).attr('idint');
            var EquipmentId = $(this).attr('idval');
            var Quantity = $(".ServiceEqpQuantity_" + serviceEqpId).val();
            var Price = $(".EqpPrice_" + serviceEqpId).val();
            var SmartPackageEquipmentServiceId = $(this).attr('SmartPackageEquipmentServiceId');
            SmartPackageEquipmentServiceEquipmentList.push({
                SmartPackageEquipmentServiceId: SmartPackageEquipmentServiceId,
                EquipmentId: EquipmentId,
                Quantity: Quantity,
                EquipmentPrice: Price
            });

        });
        var url = domainurl + "/SmartLeads/AddSmartLeadSetupPackageOption";
        var PackageParam = [];
        PackageParam = {
            SmartInstallTypeId: $('#SmartInstallTypeId').val(),
            PackageId: $('#PackageId').val(),
            SmartSystemTypeId: $('#SmartSystemTypeId').val(),
            ManufacturerId: $("#ManufacturerId").val(),
            LeadId: LeadId,
            SmartPackageEquipmentServiceEquipmentList: SmartPackageEquipmentServiceEquipmentList,
            EquipmentList: EquipmentListForWorkOrder,
            ServiceList: ServiceList
        };
        var setupParam = JSON.stringify({
            'ModelAddleadPackage': PackageParam
        });
        console.log(JSON.parse(setupParam));
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: setupParam,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}

var SaveLeadSetupService = function () {
    var LeadPackageDetailList = [];
    var EquipmentListForWorkOrder = [];
    var EquipmentPartialProductsList = [];
    var FirstBillDate = picker.getDate();
    if (CommonUiValidation()) {
        var url = domainurl + "/SmartLeads/AddSmartLeadSetupService";
        $(".HasItem").each(function () {
            EquipmentPartialProductsList.push({
                EquipmentId: $(this).attr('data-id'),
                UnitPrice: $(this).find('.txtProductRate').val(),
                DiscountUnitPricce: $(this).find('.txtProductDiscountRate').val(),
                Total: $(this).find('.txtTotalAmount').val()
            });
        });

        var EquipmentParam = {
            EquipmentLeadId: LeadId,
            AddedEquipmetList: EquipmentPartialProductsList
        };

        var setupParam = JSON.stringify({
            'AddedEquipmentsList': EquipmentParam,
            'NonCommissionableServiceList': NonCommissionableServiceList
        });
        console.log(JSON.parse(setupParam));
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: setupParam,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.CustomerAppointmetId != '00000000-0000-0000-0000-000000000000') {
                    var AppointID = data.CustomerAppointmetId;
                }
                if (data.LeadMethodBill) {
                    var v = data.LeadMethodBill;
                    $("#BillMethod").val(v);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}

var SaveLeadSetupEquipment = function (Reload) {
    var LeadPackageDetailList = [];
    var EquipmentListForWorkOrder = [];
    var EquipmentPartialProductsList = [];
    var FirstBillDate = picker.getDate();
    if (CommonUiValidation()) {
        var url = domainurl + "/SmartLeads/AddSmartLeadSetupEquipment";
        $(".HasItem").each(function () {
            var intId = 0;
            if ($(this).attr('dataid') != undefined) {
                intId = $(this).attr('dataid');
            }
            EquipmentPartialProductsList.push({
                Id: intId,
                EquipmentId: $(this).attr('data-id'),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find('.txtProductRate').val(),
                DiscountUnitPricce: parseFloat($(this).find('.txtProductRate').val()) - parseFloat($(this).find('.txtProductDiscountRate').val()),
                DiscountPckage: $(this).find('.txtProductPackageDiscount').val(),
                Total: $(this).find('.txtTotalAmount').val(),
                ServiceId: $(this).attr('data-serviceid'),
                IsPackageEqp: $(this).attr('data-ispackageeqp'),
                IsIncluded: $(this).attr('data-isinclude'),
                IsDevice: $(this).attr('data-isdevice'),
                IsOptionalEqp: $(this).attr('data-isoptionaleqp'),
            });
        });

        var EquipmentParam = {
            EquipmentLeadId: LeadId,
            AddedEquipmetList: EquipmentPartialProductsList
        };

        var setupParam = JSON.stringify({
            'AddedEquipmentsList': EquipmentParam,
            'NonCommissionableEqpList': NonCommissionableEqpList
        });
        console.log(JSON.parse(setupParam));
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: setupParam,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.CustomerAppointmetId != '00000000-0000-0000-0000-000000000000') {
                    var AppointID = data.CustomerAppointmetId;

                }
                if (data.LeadMethodBill) {
                    var v = data.LeadMethodBill;
                    $("#BillMethod").val(v);
                }
                if (typeof (Reload) != "undefined" && Reload == true) {
                    $("#LoadLeadDetail").load($("#LoadEquipment").attr('data-url'));
                }
                var strPayInfo = "";
                var strTotalAmt = parseFloat(data.strTotalAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strCollectedAmt = parseFloat(data.strCollectedAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strAchPayment = data.strAchPayment;
                var strCreditCardPayment = data.strCreditCardPayment;
                var strCashPayment = data.strCashPayment;
                var strCheckPayment = data.strCheckPayment;

                console.log(data.strCashPayment);
                if (data.strCreditCardPayment != '0' && data.strCreditCardPayment != '0.00') {
                    strPayInfo += "CC: " + Currency + strCreditCardPayment + ", ";
                }
                if (data.strAchPayment != '0' && data.strAchPayment != '0.00') {
                    strPayInfo += "ACH: " + Currency + strAchPayment + ", ";
                }
                if (data.strCashPayment != '0' && data.strCashPayment != '0.00') {
                    strPayInfo += "Cash: " + Currency + strCashPayment + ", ";
                }
                if (data.strCheckPayment != '0' && data.strCheckPayment != '0.00') {
                    strPayInfo += "Check: " + Currency + strCheckPayment + ", ";
                }
                if (strPayInfo != "") {
                    $(".total_captured_amount").html("");
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

                    if (strCollectedAmt != "0.00") {
                        strAmountText += " Collecting today: " + Currency + strCollectedAmt;
                    }
                    $(".total_captured_amount").html(strAmountText);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}
var SaveLeadSetupContactVerbal = function () {
    var SSN = $("#SSN").val();
    var SSN2 = $("#SSN2").val();
    var SSN2Clean = typeof SSN2 == "undefined" ? "" : SSN2.replace(/[-  ]/g, '');
    if (SSN2Clean.length == 9) {
        SSN = SSN2;
    }
    if (CommonUiValidation()) {
        var ContractTeam = $("#CustomerModel_ContractTeam").val();
        if (ContractTeam == "Custom") {
            ContractTeam = "";
            var CustomContactTerm = $(".CustomContactTerm").val();
            if (CustomContactTerm != "" && CustomContactTerm != undefined) {
                ContractTeam = parseInt(CustomContactTerm) / parseInt(12);
            }
        }
        var url = domainurl + "/SmartLeads/SaveLeadSetupContactVerbal";
        var param = JSON.stringify({
            Id: $("#LeadContractId").val(),
            CustomerId: $("#LeadContractCustomerID").val(),
            ContractTeam: ContractTeam,
            Passcode: $(".CustomerModel_Passcode").val(),
            RenewalTerm: $("#CustomerModel_RenewalTerm").val(),
            SSN: SSN,
            OriginalContactDate: $("#original_contact_date").val()
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
                //OpenSuccessMessageNew("Success!", "Customer Contract Term and Verbal Password save successfully !");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}
var SaveLeadSetupAggremant = function () {
    var LeadPackageDetailList = [];
    var EquipmentListForWorkOrder = [];
    var EquipmentPartialProductsList = [];
    var FirstBillDate = picker.getDate();
    if (CommonUiValidation()) {
        var url = domainurl + "/SmartLeads/AddSmartLeadSetupAggremant";
        var Param = {
            CustomerId: $("#LeadCustomerID").val()
        };
        if ($("#BillMethod").val() == "ACH") {
            var PaymentParam4 = {
                Id: $("#Id").val(),
                BillMethod: $("#BillMethod").val(),
                RoutingNo: $("#AchRouting").val(),
                AcountNo: $("#AchAcountNo").val(),
                BankAccountType: $("#AchBankAccountType").val(),
                AccountName: $("#AchAccountName").val(),
                CardType: $("#CardType").val(),
                CardNumber: $("#CardNumber").val(),
                CardExpireDate: $("#CardExpireDate").val(),
                CardSecurityCode: $("#CardSecurityCode").val(),
                PaymentCustomerId: $("#PaymentCustomerId").val(),
            };
        }
        if ($("#BillMethod").val() == "EFT") {
            var PaymentParam = {
                Id: $("#Id").val(),
                BillMethod: $("#BillMethod").val(),
                RoutingNo: $("#RoutingNo").val(),
                AcountNo: $("#AcountNo").val(),
                BankAccountType: $("#BankAccountType").val(),
                AccountName: $("#AccountName").val(),
                CardType: $("#CardType").val(),
                CardNumber: $("#CardNumber").val(),
                CardExpireDate: $("#CardExpireDate").val(),
                CardSecurityCode: $("#CardSecurityCode").val(),
                PaymentCustomerId: $("#PaymentCustomerId").val(),
            };
        }
        if ($("#BillMethod").val() == "Credit Card") {
            var PaymentParam2 = {
                Id: $("#Id").val(),
                BillMethod: $("#BillMethod").val(),
                RoutingNo: $("#RoutingNo").val(),
                AcountNo: $("#AcountNo").val(),
                BankAccountType: $("#BankAccountType").val(),
                AccountName: $("#CreditAccountName").val(),
                CardType: $("#CardType").val(),
                CardNumber: $("#CardNumber").val(),
                CardExpireDate: $("#CardExpireDate").val(),
                CardSecurityCode: $("#CardSecurityCode").val(),
                PaymentCustomerId: $("#PaymentCustomerId").val(),
            };
        }
        if ($("#BillMethod").val() == "Debit Card") {
            var PaymentParam1 = {
                Id: $("#Id").val(),
                BillMethod: $("#BillMethod").val(),
                RoutingNo: $("#RoutingNo").val(),
                AcountNo: $("#AcountNo").val(),
                BankAccountType: $("#BankAccountType").val(),
                AccountName: $("#DebitCardName").val(),
                CardType: $("#DebitCardType").val(),
                CardNumber: $("#DebitCardNo").val(),
                CardExpireDate: $("#DebitExpireDate").val(),
                CardSecurityCode: $("#DebitSecurityCode").val(),
                PaymentCustomerId: $("#PaymentCustomerId").val(),
            };
        }
        if ($("#BillMethod").val() == "Cash") {
            var PaymentParamCash = {
                Id: $("#Id").val(),
                BillMethod: $("#BillMethod").val(),
                PaymentCustomerId: $("#PaymentCustomerId").val()
            };
        }
        if ($("#BillMethod").val() == "Check") {
            var PaymentParamCheck = {
                Id: $("#Id").val(),
                BillMethod: $("#BillMethod").val(),
                CheckNo: $("#CheckNo").val(),
                PaymentCustomerId: $("#PaymentCustomerId").val()
            };
        }

        var CustomerModelFromJs = {
            Id: $("#LeadContractId").val(),
            CustomerId: $("#LeadContractCustomerID").val(),
            FirstName: $("#LeadContractFirstName").val(),
            LastName: $("#LeadContractLastName").val(),
            Street: $("#LeadContractStreet").val(),
            ZipCode: $("#LeadContractZip").val(),
            EmailAddress: $("#CusEmail").val(),
            PaymentMethod: AddLeadPaymentMethod,
            Passcode: $(".CustomerModel_Passcode").val(),
            FirstBilling: FirstBillDate,
            ActivationFeePaymentMethod: $("#CustomerModel_ActivationFeePaymentMethod").val()
        };

        var ActivationFeePaymentInfoModelFromJs = MakeAFParam($("#CustomerModel_ActivationFeePaymentMethod").val());

        var CusParam = {
            CustomerModel: CustomerModelFromJs,
        };
        var setupParam = JSON.stringify({
            'ec': Param,
            'pi4': PaymentParam4,
            'CUS': CusParam,
            'pi': PaymentParam,
            'pi2': PaymentParam2,
            'pi1': PaymentParam1,
            'PaymentCash': PaymentParamCash,
            'PaymentCheck': PaymentParamCheck
        });
        console.log(JSON.parse(setupParam));
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: setupParam,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.CustomerAppointmetId != '00000000-0000-0000-0000-000000000000') {
                    var AppointID = data.CustomerAppointmetId;
                }
                if (data.LeadMethodBill) {
                    var v = data.LeadMethodBill;
                    $("#BillMethod").val(v);
                }
                FinalCustomerSetupData();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}
var AddNewSavePaymentInfo = function () {
    var url = domainurl + "/Leads/AddLeadPaymentInfo";
    var PaymentInfoLeadParam = JSON.stringify({
        AddLeadPaymentInfo: $("#payment_profile").val(),
        paymentfor: $("#payment_for").val(),
        //PaymentForMMr: $("#PaymentForMMr").is(":checked"),
        //PaymentForFirstMonth: $("#PaymentForFirstMonth").is(":checked"),
        //PaymetForActivationFee: $("#PaymetForActivationFee").is(":checked"),
        //PaymentForEquipment: $("#PaymentForEquipment").is(":checked"),
        PaymentMethod: "",
        CustomerId: $("#LeadContractCustomerID").val(),
        FileName: $("#UploadedPath").val()
    });
    console.log(PaymentInfoLeadParam);
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: PaymentInfoLeadParam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                //if (CommonUiValidation()) {
                //    SaveLeadSetupAggremant();
                //}
                OpenSuccessMessageNew('Success!', data.message, function () {
                    $("#LoadLeadDetail").html(TabsLoaderText);
                    $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
                });
            } else {
                OpenErrorMessageNew('Error!', data.message, function () {
                    $("#LoadLeadDetail").html(TabsLoaderText);
                    $("#LoadLeadDetail").load($("#LoadFinalize").attr('data-url'));
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var discountAmount_Equip = 0.00;
var DiscountType_Equip = '';
var FinalCustomerSetupData = function () {
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

$(document).ready(function () {

    picker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#FirstBilling')[0] });

    $("#btnSavandNex").click(function () {
        console.log('btnSavandNex4');
        if ($("#TblEmergencyContactList tbody tr").length < parseInt(contactreq) && $("#step").val() == "4") {
            OpenErrorMessageNew("Error!", "You need to add at least " + contactreq + " emergency contacts to proceed.");
        }
        else if ($(".SmartSetUpServiceList tbody tr").length < 1 && oneServicerequired == "True" && $("#step").val() == "2") {
            OpenErrorMessageNew("Error!", "You need to add at least one service.");
        }
        else if ((($("#payment_profile_package").is(":visible") && $("#payment_profile_package").val() == '-1') ||
            ($("#payment_profile_onetime").is(":visible") && $("#payment_profile_onetime").val() == '-1') ||
            ($("#payment_profile_service").is(":visible") && $("#payment_profile_service").val() == '-1') ||
            ($("#payment_profile_equipment").is(":visible") && $("#payment_profile_equipment").val() == '-1')) &&
            $("#ContractType").val() == '-1' &&
            $("#step").val() == "5"
        ) {
            OpenErrorMessageNew("Error!", "Please Select Payment Method & Contract Type");
        }
        else if (($("#payment_profile_package").is(":visible") && $("#payment_profile_package").val() == '-1') ||
            ($("#payment_profile_onetime").is(":visible") && $("#payment_profile_onetime").val() == '-1') ||
            ($("#payment_profile_service").is(":visible") && $("#payment_profile_service").val() == '-1') ||
            ($("#payment_profile_equipment").is(":visible") && $("#payment_profile_equipment").val() == '-1') &&
            $("#step").val() == "5"
        ) {
            OpenErrorMessageNew("Error!", "Please Select Payment Method");
        }
        else if ($("#ContractType").val() == '-1' && $("#step").val() == "5") {
            OpenErrorMessageNew("Error!", "Please Select Contract Type");
        }
        else if ($("#TblEmergencyContactList tbody tr").length < parseInt(contactreq) && $("#step").val() == "5") {
            OpenErrorMessageNew("Error!", "You need to add at least " + contactreq + " emergency contacts to proceed.");
        }
   
        else {
            if (CommonUiValidation()) {
                if ($("#step").val() == "1") {
                    SaveLeadSetupPackageOption();
                }
                if ($("#step").val() == "2") {
                    SaveLeadSetupService();
                }
                if ($("#step").val() == "3") {
                    SaveLeadSetupEquipment();
                }
                if ($("#step").val() == "4") {
                    SaveLeadSetupContactVerbal();
                }
                if ($("#step").val() == "5") {

                    FinalCustomerSetupData();
                }
            }
        }

    });

    $("#btnSavandClose").click(function () {
        if ($("#TblEmergencyContactList tbody tr").length < parseInt(contactreq) && $("#step").val() == "4") {
            OpenErrorMessageNew("Error!", "You need to add at least " + contactreq + " emergency contacts to proceed.");
        }
        else {
            FinalCustomerSetupData();
            LoadLeadsDetail(LeadId, true);
            $("#btnSavandClose").addClass('hidden');
            $("#btnPayNow").addClass('hidden');

            $("#leadToCustomerConvert").addClass('hidden');
            $("#btnSavandNex span").text("Save & Next");
        }
    })
    //$(".LoadAgreementPopUp").click(function () {

    //})
});
var LeadtoCustomerConvert = function (Leadid) {

    var LeadtoCustomerid = Leadid;
    $.ajax({
        url: domainurl + "/Leads/LeadtoCustomerConvertQAEmail/",
        data: { LeadtoCustomerid },
        type: "Post",
        dataType: "Json"
    }).done(function () {

    });
}
var LeadCustomerSetUpEmail = function (LeadId) {

    var LeadCustomerId = LeadId;
    $.ajax({
        url: domainurl + "/Leads/CustomerSetUpEmail/",
        data: { LeadCustomerId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        OpenSuccessMessageNew("Success!", "Customer setup has been done successfully !");
    });
}
var LeadConvertedToCustomer = function (idval) {
    var Id = idval;
    $.ajax({
        url: domainurl + "/SmartLeads/ConvertLeadToCustomer/",
        data: { Id },
        type: "Post",
        dataType: "Json"
    }).done(function (data) {
        console.log(data);
        OpenSuccessMessageNew("Success", "Converted to customer successfully.", function () {
            window.open("/Customer/Customerdetail/?id=" + data.CID);
        });

    });
}

