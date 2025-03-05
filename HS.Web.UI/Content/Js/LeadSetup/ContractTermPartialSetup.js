var UserFileUploadjqXHRData;

var CardExValidation = function () {
    var result = true;
    var CardEx = $("#CardExpireDate").val();
    var arry = CardEx.split("/");
    var num = 12;
    var n = num.toString();
    if (jQuery.inArray("0", arry[0]) !== -1) {
        var s = '0';
        var currentmonth = s + (new Date().getMonth() + 1).toString();
    }
    else {
        var currentmonth = (new Date().getMonth() + 1).toString();
    }
    if (arry[0] > n) {
        result = false;
    }
    else {
        if (arry[1] < new Date().getFullYear().toString().substr(-2)) {
            result = false;
        }
        else if (arry[1] == new Date().getFullYear().toString().substr(-2)) {

            if (arry[0] < currentmonth) {
                result = false;
            }
            else if (arry[0] == currentmonth) {
                result = true;
            }
            else {
                result = true;
            }
        }
    }
    return result;
}
var IsValidCreditCard = function () {
    var result = $('#CreditCard_PaymentInfo_CardNumber').validateCreditCard();
    return result.valid;
}
var InitialViewLoad = function () {
    $(".mmr_payment_method_detils").show();
    //var PaymentMethod = $("#BillMethod").val();

    var PaymentMethod = $("#AddLeadPaymentMethod").val();

    if (PaymentMethod == '-1') {
        NoItemSelect();
    }
    if (PaymentMethod == 'ACH') {
        ACHViewShow();
    }
    if (PaymentMethod == 'EFT') {
        EFTViewShow();
    }
    if (PaymentMethod == 'Cash') {
        CashViewShow();
    }
    if (PaymentMethod == 'Check') {
        CheckViewShow();
    }
    if (PaymentMethod == 'Credit Card') {
        CreditCardViewShow();
    }
    if (PaymentMethod == 'Debit Card') {
        DebitCardViewShow();
    }
    if (PaymentMethod == 'Invoice') {
        InvoiceViewShow();
    }
}
var NoItemSelect = function () {
    $("#EFTForm").hide();
    $("#ACHForm").hide();
    $("#CashForm").hide();
    $("#CheckForm").hide();
    $("#CreditForm").hide();
    $("#DebitForm").hide();
    $("#InvoiceForm").hide();
}
var ACHViewShow = function () {
    $("#EFTForm").hide();
    $("#ACHForm").show();
    $("#CashForm").hide();
    $("#CheckForm").hide();
    $("#CreditForm").hide();
    $("#DebitForm").hide();
    $("#InvoiceForm").hide();
}
var EFTViewShow = function () {
    $("#EFTForm").show();
    $("#ACHForm").hide();
    $("#CashForm").hide();
    $("#CheckForm").hide();
    $("#CreditForm").hide();
    $("#DebitForm").hide();
    $("#InvoiceForm").hide();
}
var CashViewShow = function () {
    $("#EFTForm").hide();
    $("#ACHForm").hide();
    $("#CashForm").show();
    $("#CheckForm").hide();
    $("#CreditForm").hide();
    $("#DebitForm").hide();
    $("#InvoiceForm").hide();
}
var CheckViewShow = function () {
    $("#EFTForm").hide();
    $("#ACHForm").hide();
    $("#CashForm").hide();
    $("#CheckForm").show();
    $("#CreditForm").hide();
    $("#DebitForm").hide();
    $("#InvoiceForm").hide();
}
var CreditCardViewShow = function () {
    $("#EFTForm").hide();
    $("#ACHForm").hide();
    $("#CashForm").hide();
    $("#CheckForm").hide();
    $("#CreditForm").show();
    $("#DebitForm").hide();
    $("#InvoiceForm").hide();
}
var DebitCardViewShow = function () {
    $("#EFTForm").hide();
    $("#ACHForm").hide();
    $("#CashForm").hide();
    $("#CheckForm").hide();
    $("#CreditForm").hide();
    $("#DebitForm").show();
    $("#InvoiceForm").hide();
}
var InvoiceViewShow = function () {
    $("#EFTForm").hide();
    $("#ACHForm").hide();
    $("#CashForm").hide();
    $("#CheckForm").hide();
    $("#CreditForm").hide();
    $("#DebitForm").hide();
    $("#InvoiceForm").show();
}

var InitialAFViewLoad = function () {
    $(".activationFee_payment_method_detils").show();
    var PaymentMethod = $("#CustomerModel_ActivationFeePaymentMethod").val();
    if (PaymentMethod == '-1') {
        AFNoItemSelect();
    }
    if (PaymentMethod == 'ACH') {
        AF_ACHViewShow();
    }
    if (PaymentMethod == 'EFT') {
        AF_EFTViewShow();
    }
    if (PaymentMethod == 'Cash') {
        AF_CashViewShow();
    }
    if (PaymentMethod == 'Check') {
        AF_CheckViewShow();
    }
    if (PaymentMethod == 'Credit Card') {
        AF_CreditCardViewShow();
    }
    if (PaymentMethod == 'Debit Card') {
        AF_DebitCardViewShow();
    }
}

var AFNoItemSelect = function () {
    $("#AF_EFTForm").hide();
    $("#AF_ACHForm").hide();
    $("#AF_CashForm").hide();
    $("#AF_CheckForm").hide();
    $("#AF_CreditForm").hide();
    $("#AF_DebitForm").hide();
}
var AF_ACHViewShow = function () {
    $("#AF_EFTForm").hide();
    $("#AF_ACHForm").show();
    $("#AF_CashForm").hide();
    $("#AF_CheckForm").hide();
    $("#AF_CreditForm").hide();
    $("#AF_DebitForm").hide();
}
var AF_EFTViewShow = function () {
    $("#AF_EFTForm").show();
    $("#AF_ACHForm").hide();
    $("#AF_CashForm").hide();
    $("#AF_CheckForm").hide();
    $("#AF_CreditForm").hide();
    $("#AF_DebitForm").hide();
}
var AF_CashViewShow = function () {
    $("#AF_EFTForm").hide();
    $("#AF_ACHForm").hide();
    $("#AF_CashForm").show();
    $("#AF_CheckForm").hide();
    $("#AF_CreditForm").hide();
    $("#AF_DebitForm").hide();
    $("#AF_InvoiceForm").hide();
}
var AF_CheckViewShow = function () {
    $("#AF_EFTForm").hide();
    $("#AF_ACHForm").hide();
    $("#AF_CashForm").hide();
    $("#AF_CheckForm").show();
    $("#AF_CreditForm").hide();
    $("#AF_DebitForm").hide();
}
var AF_CreditCardViewShow = function () {
    $("#AF_EFTForm").hide();
    $("#AF_ACHForm").hide();
    $("#AF_CashForm").hide();
    $("#AF_CheckForm").hide();
    $("#AF_CreditForm").show();
    $("#AF_DebitForm").hide();
}
var AF_DebitCardViewShow = function () {
    $("#AF_EFTForm").hide();
    $("#AF_ACHForm").hide();
    $("#AF_CashForm").hide();
    $("#AF_CheckForm").hide();
    $("#AF_CreditForm").hide();
    $("#AF_DebitForm").show();
}

var CardExValidation = function () {
    var result = true;
    var CardEx = $("#CardExpireDate").val();
    var arry = CardEx.split("/");
    var num = 12;
    var n = num.toString();
    if (jQuery.inArray("0", arry[0]) !== -1) {
        var s = '0';
        var currentmonth = s + (new Date().getMonth() + 1).toString();
    }
    else {
        var currentmonth = (new Date().getMonth() + 1).toString();
    }
    if (arry[0] > n) {
        result = false;
    }
    else {
        if (arry[1] < new Date().getFullYear().toString().substr(-2)) {
            result = false;
        }
        else if (arry[1] == new Date().getFullYear().toString().substr(-2)) {

            if (arry[0] < currentmonth) {
                result = false;
            }
            else if (arry[0] == currentmonth) {
                result = true;
            }
            else {
                result = true;
            }
        }
    }
    return result;
}


var SavePaymentInfoValidation = function () {
    if ($("#AddLeadPaymentMethod").val() == "-1") {
        return false;
    }
    else if ($("#PaymentForMMr").is(":checked") == false
        && $("#PaymentForFirstMonth").is(":checked") == false
        && $("#PaymentForEquipment").is(":checked") == false
        && $("#PaymetForActivationFee").is(":checked") == false) {

        return false
    }
    return true;

}

var DeleteLeadPaymentMethod = function (id) {
    var url = domainurl + "/Leads/DeleteLeadPaymentInfo";
    var param = JSON.stringify({
        CustomerId: $("#LeadContractCustomerID").val(),
        PaymentInfoId: id
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
            if (data.result) {
                OpenSuccessMessageNew('Success!', data.message, function () {
                    $("#LoadLeadDetail").load($("#LoadService").attr('data-url'));
                });
            } else {
                OpenErrorMessageNew('Error!', data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
 
$(document).ready(function () {
    $("#BtnSkip").click(function () {
        //$("#AddLeadPaymentMethod").val('-1');
        $(".payment_info_field").hide();
    });
    $("#BtnSkip1").click(function () {
        //$("#AddLeadPaymentMethod").val('-1');
        $(".payment_info_field").hide();
    })
    $("#BtnSkip2").click(function () {
        //$("#AddLeadPaymentMethod").val('-1');
        $(".payment_info_field_1").hide();
    })
    $("#BtnSkip3").click(function () {
        //$("#AddLeadPaymentMethod").val('-1');
        $(".payment_info_field_1").hide();
    })
    $("#Credittest3").click(function () {
        $("#CreditCard_PaymentInfo_CardType").val($("#Credittest3").val());
    })
    $("#Credittest2").click(function () {
        $("#CreditCard_PaymentInfo_CardType").val($("#Credittest2").val())
    })
    $("#Credittest1").click(function () {
        $("#CreditCard_PaymentInfo_CardType").val($("#Credittest1").val());
    })
    $("#Debittest3").click(function () {
        $("#DebitCard_PaymentInfo_CardType").val($("#Debittest3").val());
    })
    $("#Debittest2").click(function () {
        $("#DebitCard_PaymentInfo_CardType").val($("#Debittest2").val());
    })
    $("#Debittest1").click(function () {
        $("#DebitCard_PaymentInfo_CardType").val($("#Debittest1").val());
    })
    $("#AF_Credittest3").click(function () {
        $("#CreditCard_ActivationFeePaymentInfoModel_CardType").val($("#AF_Credittest3").val());
    })
    $("#AF_Credittest2").click(function () {
        $("#CreditCard_ActivationFeePaymentInfoModel_CardType").val($("#AF_Credittest2").val());
    })
    $("#AF_Credittest1").click(function () {
        $("#CreditCard_ActivationFeePaymentInfoModel_CardType").val($("#AF_Credittest1").val());
    })
    $("#AF_Debittest3").click(function () {
        $("#DebitCard_ActivationFeePaymentInfoModel_CardType").val($("#AF_Debittest3").val());
    })
    $("#AF_Debittest2").click(function () {
        $("#DebitCard_ActivationFeePaymentInfoModel_CardType").val($("#AF_Debittest2").val());
    })
    $("#AF_Debittest1").click(function () {
        $("#DebitCard_ActivationFeePaymentInfoModel_CardType").val($("#AF_Debittest1").val());
    })
    $("#CustomerModel_ContractTeam").change(function () {
        if ($("#CustomerModel_ContractTeam").val() != "-1") {
            $(this).removeClass('required');
        }
    });
    $("#CustomerModel_MonthlyMonitoringFee").change(function () {
        if ($("#CustomerModel_MonthlyMonitoringFee").val() != "-1") {
            $(this).removeClass('required');
        }
    });
    InitialViewLoad(); //Load mmr initial view load
    InitialAFViewLoad(); //Load activation fee initial view load
    $("#AddLeadPaymentMethod").change(function () {
        $(".mmr_payment_method_detils").show();
        var PaymentMethodType = $("#AddLeadPaymentMethod").val();

        if (PaymentMethodType == '-1') {
            NoItemSelect();
        }
        if (PaymentMethodType == 'ACH') {
            ACHViewShow();
        }
        if (PaymentMethodType == 'EFT') {
            EFTViewShow();
        }
        if (PaymentMethodType == 'Cash') {
            CashViewShow();
        }
        if (PaymentMethodType == 'Check') {
            CheckViewShow();
        }
        if (PaymentMethodType == 'Credit Card') {
            CreditCardViewShow();
        }
        if (PaymentMethodType == 'Debit Card') {
            DebitCardViewShow();
        }
        if (PaymentMethodType == 'Invoice') {
            InvoiceViewShow();
        }

    });
    $("#CustomerModel_ActivationFeePaymentMethod").change(function () {
        $(".activationFee_payment_method_detils").show();
        var PaymentMethodType = $("#CustomerModel_ActivationFeePaymentMethod").val();

        if (PaymentMethodType == '-1') {
            AFNoItemSelect();
        }
        if (PaymentMethodType == 'ACH') {
            AF_ACHViewShow();
        }
        if (PaymentMethodType == 'EFT') {
            AF_EFTViewShow();
        }
        if (PaymentMethodType == 'Cash') {
            AF_CashViewShow();
        }
        if (PaymentMethodType == 'Check') {
            AF_CheckViewShow();
        }
        if (PaymentMethodType == 'Credit Card') {
            AF_CreditCardViewShow();
        }
        if (PaymentMethodType == 'Debit Card') {
            AF_DebitCardViewShow();
        }
    });
    $("#CardExpireDate").blur(function () {
        var resultCheck = CardExValidation();

        if (resultCheck) {
            $("#CardExpireError").addClass("hidden");
        }
        else {
            $("#CardExpireError").removeClass("hidden");
        }
    });
    $("#CreditCard_PaymentInfo_CardNumber").blur(function () {
        var resultCheck = IsValidCreditCard();
        if (resultCheck) {
            $("#CardNumber_Error").addClass("hidden");
        }
        else {
            $("#CardNumber_Error").removeClass("hidden");
        }
    });
    $("#Credittest1").change(function () {
        var Creditval = $("#Credittest1").val();
        $("#CardType").val(Creditval);
    });
    $("#Credittest2").change(function () {
        var Creditval1 = $("#Credittest2").val();
        $("#CardType").val(Creditval1);
    });
    $("#Credittest3").change(function () {
        var Creditval2 = $("#Credittest3").val();
        $("#CardType").val(Creditval2);
    });
    $("#Debittest1").change(function () {
        var Debitval = $("#Debittest1").val();
        $(".DebitType").val(Debitval);
    });
    $("#Debittest2").change(function () {
        var Debitval1 = $("#Debittest2").val();
        $(".DebitType").val(Debitval1);
    });
    $("#Debittest3").change(function () {
        var Debitval2 = $("#Debittest3").val();
        $(".DebitType").val(Debitval2);
    });


    /*Payment info upload Starts*/
    $("#UploadCustomerFileBtn").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/File/UploadPaymentInfoFile/?CustomerId=' + LeadId,
        dataType: 'json',
        add: function (e, data) {
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            UserFileUploadjqXHRData = data;
        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progress").show();
            $(".file-progress .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progress .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);
            console.log(data.result);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                /*$("#UploadCustomerFileBtn").addClass('hidden');*/
                $("#UploadedPath").val(data.result.filePath);

            }
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });
    /*Payment info upload Starts*/



})