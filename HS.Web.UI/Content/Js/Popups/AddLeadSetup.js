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
            EcheckType:$("#ACH_PaymentInfo_EcheckType").val().trim(),
            RoutingNo: $("#ACH_PaymentInfo_RoutingNo").val(),
            AcountNo: $("#ACH_PaymentInfo_AcountNo").val(),
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
    if(method == "ACH"){
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
        if($("#DebitCard_PaymentInfo_CardNumber").val().trim() == ""){
            result = false;
        }
        if($("#DebitCard_PaymentInfo_CardExpireDate").val().trim() == ""){
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
        if($("#CreditCard_PaymentInfo_CardNumber").val().trim() == ""){
            result = false;
        } else {
            var res = $('#CreditCard_PaymentInfo_CardNumber').validateCreditCard();
            result = res.valid;
        }
        if($("#CreditCard_PaymentInfo_CardExpireDate").val().trim() == ""){
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


var SaveLeadSetup = function () {
    var LeadPackageDetailList = [];
    var EquipmentListForWorkOrder = [];
    var EquipmentPartialProductsList = [];
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
                NumOfEquipments: IncludeEquipmentNumber
            });
            LeadPackageDetailList.push({
                Type: 'Included',
                IsIncluded: true,
                PackageEqpId: IncludeEquipmentId,
                packageId: PacId
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
                    //deviceEquipmentList.push({
                    //    SelectedEquipmentId: deviceEquipmentId,
                    //    SelectedEquipmentPrice: deviceEquipmentPrice,
                    //    SelectedEquipmentIsFree: deviceEquipmentIsFree,
                    //    NumOfEquipments: DeviceEptNo
                    //});
                    EquipmentListForWorkOrder.push({
                        SelectedEquipmentId: deviceEquipmentId,
                        SelectedEquipmentPrice: deviceEquipmentPrice,
                        SelectedEquipmentIsFree: deviceEquipmentIsFree,
                        NumOfEquipments: DeviceEptNo
                    });
                    LeadPackageDetailList.push({
                        Type: 'Device',
                        IsIncluded: false,
                        PackageEqpId: IncludeEquipmentId,
                        packageId: PacId
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
                    NumOfEquipments: valEquipOptional[count]
                });

                LeadPackageDetailList.push({
                    Type: 'Optional',
                    IsIncluded: false,
                    PackageEqpId: IncludeEquipmentId,
                    packageId: PacId
                });
                console.log(EquipmentListForWorkOrder);
            }
            count++;
        });
        var url = domainurl + "/Leads/AddLeadSetup";
        var Param = {
            //Id: $("#Id").val(),
            CustomerId: $("#LeadCustomerID").val(),
            ////CrossSteet: $("#CrossSteet").val(),
            ////FirstName: $("#FirstName").val(),
            ////LastName: $("#LastName").val(),
            ////RelationShip: $("#RelationShip").val(),
            ////Phone: $("#Phone").val(),
            ////HasKey: $("#HasKey").val(),
            ////LeadId: $("#LeadId").val(),
            ////EmergencyNewId: $("#EmergencyNewId").val()
            ////LeadCustomerID
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
            ContractTeam: $("#CustomerModel_ContractTeam").val(),
            EmailAddress: $("#CusEmail").val(),
            MonthlyMonitoringFee: $("#CustomerModel_MonthlyMonitoringFee").val(),
            PaymentMethod: $("#AddLeadPaymentMethod").val(),
            Passcode: $("#CustomerModel_Passcode").val(),
            BillDay: $("#CustomerModel_BillDay").val(),
            ActivationFee: $("#CustomerModel_ActivationFee").val(),
            FirstBilling: FirstBillDate,
            ActivationFeePaymentMethod: $("#CustomerModel_ActivationFeePaymentMethod").val()
        };

        var ActivationFeePaymentInfoModelFromJs = MakeAFParam( $("#CustomerModel_ActivationFeePaymentMethod").val());

        var CusParam = {
            CustomerModel: CustomerModelFromJs,
            /*PaymentInfo: PaymentInfoFromJs,*/
            /*PaymentInfo: PaymentInfoFromJs,
            ActivationFeePaymentInfoModel: ActivationFeePaymentInfoModelFromJs*/
        };


        //var CusParam = {
        //    Id: $("#LeadContractId").val(),
        //    CustomerId: $("#LeadContractCustomerID").val(),
        //    FirstName: $("#LeadContractFirstName").val(),
        //    LastName: $("#LeadContractLastName").val(),
        //    Street: $("#LeadContractStreet").val(),
        //    ZipCode: $("#LeadContractZip").val(),
        //    ContractTeam: $("#ContractTeam").val(),
        //    EmailAddress: $("#CusEmail").val(),
        //    MonthlyMonitoringFee: $("#MonthlyMonitoringFee").val(),
        //    PaymentMethod: $("#PaymentMethod").val(),
        //    Passcode: $("#Passcode").val(),
        //    BillDay: $("#BillDay").val(),
        //    ActivationFee: $("#ActivationFee").val(),
        //    FirstBilling: FirstBillDate,
        //    ActivationFeePaymentMethod: $("#ActivationFeePaymentMethod").val()
        //};
        //var PackageParam = [];
        //if (deviceEquipmentList.length <= CurrentPackageLimit) {
        //    //var url = "/Leads/AddLeadPackageDetails";
            
        //}
        var PackageParam = [];
        if (EquipmentListForWorkOrder.length > 0) {
            PackageParam = {
                InstallType: $('#InstallType').val(),
                PackageIdInt: $('#PackageType').val(),
                SystemTypeId: $('#PackageSystemType').val(),
                LeadId: LeadId,
                //EquipmentArray: EquipmentIdArray,
                EquipmentList: EquipmentListForWorkOrder,
                PackageCustomerEquipmentsList: LeadPackageDetailList
            };
        }
        $(".HasItem").each(function () {
            EquipmentPartialProductsList.push({
                EquipmentId: $(this).attr('data-id'),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find('.txtProductRate').val(),
                TotalPrice: $(this).find('.txtYourAmount').val()
            });
        });

        var EquipmentParam = {
            EquipmentLeadId: LeadId,
            AddedEquipmetList: EquipmentPartialProductsList
        };

        var setupParam = JSON.stringify({
            'ec': Param,
            'pi4': PaymentParam4,
            'CUS': CusParam,
            'ModelAddleadPackage': PackageParam,
            'pi': PaymentParam,
            'pi2': PaymentParam2,
            'pi1': PaymentParam1,
            'AddedEquipmentsList': EquipmentParam,
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

                if (data == true) {
                }
                if (data.LeadIDValue) {
                    //$("#InstallationAgreement")[0].click();
                    if (Device.iPad()) {
                        $(".LoadAgreementPopUp1")[0].click();
                    }
                    else {
                        OpenInstallationAgreement();
                    }
                    
                    //var LeadIdval = data.LeadIDValue;
                    //LeadtoCustomerConvert(LeadIdval);
                    //LeadConvertedToCustomer(LeadIdval);
                    //LeadCustomerSetUpEmail(LeadIdval);

                    //LoadCustomer(true);
                    //LoadLeadsDetail(LeadIdval);
                }
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

var AddNewSavePaymentInfo = function () {
    if (!PaymentMethodValidation($("#AddLeadPaymentMethod").val())) {
        OpenErrorMessageNew("Error!", "Please fillup all data to add payment method.");
        return false;
    }
    var PaymentInfoFromJs = MakePaymentMethodParam($("#AddLeadPaymentMethod").val());
    var url = domainurl + "/Leads/AddLeadPaymentInfo";
    console.log(PaymentInfoFromJs);
    var PaymentInfoLeadParam = JSON.stringify({
        AddLeadPaymentInfo: PaymentInfoFromJs,
        PaymentForMMr: $("#PaymentForMMr").is(":checked"),
        PaymentForFirstMonth: $("#PaymentForFirstMonth").is(":checked"),
        PaymetForActivationFee: $("#PaymetForActivationFee").is(":checked"),
        PaymentForEquipment: $("#PaymentForEquipment").is(":checked"),
        PaymentMethod: $("#AddLeadPaymentMethod").val(),
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
                if (CommonUiValidation()) {
                    SaveLeadSetup();
                }
                OpenSuccessMessageNew('Success!', data.message, function () {
                    $("#LoadLeadDetail").load($("#LoadService").attr('data-url'));
                });
            } else {
                OpenErrorMessageNew('Error!', data.message, function () {
                    $("#LoadLeadDetail").load($("#LoadService").attr('data-url'));
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var FinalCustomerSetupData = function () {
    var url = domainurl + "/Leads/FinalCustomerSetupData";
    var param = JSON.stringify({
        setupid: LeadIdVal
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
                    $(".LoadAgreementPopUp1")[0].click();
                }
                else {
                    if (CommonUiValidation()) {
                        SaveLeadSetup();
                    }
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
        else if ($("#step").val() == "4") {
            FinalCustomerSetupData();
        }
        else {
            if (CommonUiValidation()) {
                SaveLeadSetup();
            }
        }

    });
    $("#btnSavandClose").click(function () {
        if ($("#TblEmergencyContactList tbody tr").length < parseInt(contactreq) && $("#step").val() == "4") {
            OpenErrorMessageNew("Error!", "You need to add at least " + contactreq + " emergency contacts to proceed.");
        }
        else {
            LoadLeadsDetail(LeadId, true);
            $("#btnSavandClose").addClass('hidden');
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
    });
}

