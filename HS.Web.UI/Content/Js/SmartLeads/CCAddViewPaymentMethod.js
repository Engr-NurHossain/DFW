var cusbillstreet = "";
var cusbillcity = "";
var cusbillzip = "";
var cusbillstate = "";
var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
var CCMethodFunction = function () {

    var url = domainurl + "/SmartLeads/SavePaymentMethod";

    var param = JSON.stringify({
        Id: $("#CC_id").val(),
        CardType: $("#CardType").val(),
        CardNumber: $("#CardNumber").val(),
        CardExpireDate: $("#CardExpireDate").val(),
        CardSecurityCode: $("#CardSecurityCode").val(),
        AccountName: $("#AccountName").val(),
        MethodType: PaymentMethodCC,
        CustomerId: customerid,
        CustomerBillingStreet: $("#Street").val(),
        CustomerBillingZip: $("#ZipCode").val(),
        CustomerBillingCity: $("#City").val(),
        CustomerBillingState: $("#State").val(),
        IsForBrinks: $("#IsForBrinks").is(":checked"),
        IsPartialPayment: $("#IsPartialPayment").is(":checked"),
        IsInitialPayment: $("#IsInitialPayment").is(":checked"),
        Payfor: $("#Payfor").val() != null || $("#Payfor").val() != '' || $("#Payfor").val() != 'undefined' ? $("#Payfor").val() : null

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
                if (TYPE == "Customer") {
                    OpenSuccessMessageNew("Success", "Payment Method Saved Successfully", function () {
                        OpenRightToLeftModal(false);
                        $(LoadCustomerDiv + ".CustomerCCTab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + data.customerid);
                        $(LoadCustomerDiv + "#CustomerCCQATab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + data.customerid);
                        $(LoadCustomerDiv + "#CCTab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + data.customerid);
                        RMRTemplatePaymentDropdownLoad(data.paymentInfoId);                        
                    });
                }
                else if (TYPE == "Lead") {
                    OpenSuccessMessageNew("Success", "Payment Method Saved Successfully", function () {
                        OpenRightToLeftModal(false);
                        $("#CustomerCCTab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + data.customerid);
                        $("#CustomerCCQATab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + data.customerid);
                        $("#CCTab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + data.customerid);
                    });
                }
                else {
                    OpenSuccessMessageNew("Success", "Payment Method Saved Successfully", function () {
                        OpenRightToLeftModal(false);
                        OpenFifthTab();
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
function isCreditCard(CC) {
    if (CC.length > 19)
        return (false);

    sum = 0; mul = 1; l = CC.length;
    for (i = 0; i < l; i++) {
        digit = CC.substring(l - i - 1, l - i);
        tproduct = parseInt(digit, 10) * mul;
        if (tproduct >= 10)
            sum += (tproduct % 10) + 1;
        else
            sum += tproduct;
        if (mul == 1)
            mul++;
        else
            mul--;
    }
    if ((sum % 10) == 0)
        return (true);
    else
        return (false);
}
function normalizeYear(year) {
    // Century fix
    var YEARS_AHEAD = 20;
    if (year < 100) {
        var nowYear = new Date().getFullYear();
        year += Math.floor(nowYear / 100) * 100;
        if (year > nowYear + YEARS_AHEAD) {
            year -= 100;
        } else if (year <= nowYear - 100 + YEARS_AHEAD) {
            year += 100;
        }
    }
    return year;
}

function checkExp() {
    var match = $('#CardExpireDate').val().match(/^\s*(0?[1-9]|1[0-2])\/(\d\d|\d{4})\s*$/);
    if (!match) {
        $("#CardExpireDate").css("border-color", "red");
        return false;
    }
    var exp = new Date(normalizeYear(1 * match[2]), 1 * match[1] - 1, 1).valueOf();
    var now = new Date();
    var currMonth = new Date(now.getFullYear(), now.getMonth(), 1).valueOf();
    if (exp <= currMonth) {
        $("#CardExpireDate").css("border-color", "red");
        return false;
    } else {
        $("#CardExpireDate").css("border-color", "#ccc");
        return true;
    };
}
function validate_cvv(cvv) {
    if ($("#card_type_img").hasClass('AMEX')) {
        var myRe = /^[0-9]{4,4}$/;
        var myArray = myRe.exec(cvv);
        if (cvv != myArray) {
            $("#CardSecurityCode").css("border-color", "red");
            return false;
        } else {
            $("#CardSecurityCode").css("border-color", "#ccc");
            return true;
        }
    }
    else {
        var myRe = /^[0-9]{3,3}$/;
        var myArray = myRe.exec(cvv);
        if (cvv != myArray) {
            $("#CardSecurityCode").css("border-color", "red");
            return false;
        } else {
            $("#CardSecurityCode").css("border-color", "#ccc");
            return true;
        }
    }
}
function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}

function validate_AccountName() {
    var AccountName = $("#AccountName").val();
    if (AccountName == undefined || AccountName == null || isEmptyOrSpaces(AccountName)) {
        $("#AccountName").css("border-color", "red");
        return false;
    } else {
        $("#AccountName").css("border-color", "#ccc");
        return true;
    }
}

function validate_CardNumber() {
    var CardNumber = $("#CardNumber").val();
    if (CardNumber == undefined || CardNumber == null || isEmptyOrSpaces(CardNumber)) {
        $("#CardNumber").css("border-color", "red");
        return false;
    } else {
        $("#CardNumber").css("border-color", "#ccc");
        return true;
    }
}


function GetCardType(number) {
    // visa
    var re = new RegExp("^4");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/visa.png');
        $("#CardNumber").attr('maxlength', '19');
        return "Visa";
    }
    // Mastercard
    // Updated for Mastercard 2017 BINs expansion
    if (/^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$/.test(number)) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/mastercard.png');
        $("#CardNumber").attr('maxlength', '19');
        return "Mastercard";
    }
    //Previous one
    if (/^5[1-5][0-9]{14}$/.test(number)) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/mastercard.png');
        $("#CardNumber").attr('maxlength', '19');
        return "Mastercard";
    }
    // AMEX
    re = new RegExp("^3[47]");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/AmericanExpress.png');
        $("#CardNumber").attr('maxlength', '18');
        $("#card_type_img").addClass('AMEX');
        return "AMEX";
    }
    // Discover
    re = new RegExp("^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/discover.png');
        $("#CardNumber").attr('maxlength', '19');
        return "Discover";
    }
    // Diners
    re = new RegExp("^36");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/diners.png');
        $("#CardNumber").attr('maxlength', '17');
        return "Diners";
    }
    // Diners - Carte Blanche
    re = new RegExp("^30[0-5]");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/diners.png');
        $("#CardNumber").attr('maxlength', '17');
        return "Diners - Carte Blanche";
    }
    // JCB
    re = new RegExp("^35(2[89]|[3-8][0-9])");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/jcb.png');
        $("#CardNumber").attr('maxlength', '19');
        return "JCB";
    }
    // Visa Electron
    re = new RegExp("^(4026|417500|4508|4844|491(3|7))");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/visa.png');
        $("#CardNumber").attr('maxlength', '19');
        return "Visa Electron";
    }
    re = new RegExp("^5");
    if (number.match(re) != null) {
        $(".card_type_div").removeClass('hidden');
        $("#card_type_img").attr('src', '/Content/img/mastercard.png');
        $("#CardNumber").attr('maxlength', '19');
        return "Mastercard";
    }
    return "";
}
var GetCardBINDetail = function () {
    var resultCheck = isCreditCard($("#CardNumber").val().split("-").join(""));
    if (!resultCheck) {
        OpenErrorMessageNew("", "Card number is not valid");
        return;
    } else {
        var url = domainurl + "/SmartLeads/BINResult";
        var param = JSON.stringify({
            BIN: $("#CardNumber").val().split("-").join("").substring(0, 6),
        })
        $.ajax({
            type: "POST",
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result && data.data != null && data.data != "") {
                    console.log(data.result);
                    var Response = JSON.parse(data.data);
                    if (typeof (Response.message) != "undefined" && Response.message != "") {
                        OpenErrorMessageNew("", Response.message);
                    }
                    if (typeof (Response.card) != "undefined" && Response.card != "") {
                        $(".tblBINDetails").removeClass("hidden");
                        $(".CardVal").text(Response.card);
                        $(".CardTR").removeClass("hidden");
                    } else {
                        $(".tblBINDetails").addClass("hidden");
                        $(".CardTR").addClass("hidden");
                    }
                    if (typeof (Response.bank) != "undefined" && Response.bank != "") {
                        $(".BankVal").text(Response.bank);
                        $(".BankTR").removeClass("hidden");
                    } else {
                        $(".BankTR").addClass("hidden");
                    }
                    if (typeof (Response.country) != "undefined" && Response.country != "") {
                        $(".CountryVal").text(Response.country);
                        $(".CountryTR").removeClass("hidden");
                    } else {
                        $(".CountryTR").addClass("hidden");
                    }
                    if (typeof (Response.countrycode) != "undefined" && Response.countrycode != "") {
                        $(".CountryCODEVal").attr('src', 'https://www.countryflags.io/' + Response.countrycode + '/flat/32.png');
                        $(".CountryCODEVal").removeClass("hidden");
                    } else {
                        $(".CountryVal").attr('src', '');
                        $(".CountryCODEVal").addClass("hidden");
                    }
                    if (typeof (Response.level) != "undefined" && Response.level != "") {
                        $(".LevelVal").text(Response.level);
                        $(".LevelTR").removeClass("hidden");
                    } else {
                        $(".LevelTR").addClass("hidden");
                    }
                    if (typeof (Response.phone) != "undefined" && Response.phone != "") {
                        $(".PhoneVal").text(Response.phone);
                        $(".PhoneTR").removeClass("hidden");
                    } else {
                        $(".PhoneTR").addClass("hidden");
                    }
                    if (typeof (Response.type) != "undefined" && Response.type != "") {
                        $(".TypeVal").text(Response.type);
                        $(".TypeTR").removeClass("hidden");
                    } else {
                        $(".TypeTR").addClass("hidden");
                    }
                    if (typeof (Response.valid) != "undefined" && Response.valid != "") {

                    }
                    if (typeof (Response.website) != "undefined" && Response.website != "") {
                        $(".WebsiteVal").text(Response.website);
                        $(".WebsiteTR").removeClass("hidden");
                    } else {
                        $(".WebsiteTR").addClass("hidden");
                    }
                }
                else {
                    OpenErrorMessageNew("", data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var RMRTemplatePaymentDropdownLoad = function (ev) {
    if ($('li.CustomerRecurringBillingTab').hasClass('active')) {
        $("#RecurringBillPaymentId").val(ev);
        PaymentDropdownLoad(ev);
    }
}
var ShowAddress = function () {
    if ($('input[name="IsBillAddress"]:checked').val() == 'true') {
        cusbillstate = $("#State").val();
        cusbillcity = $("#City").val();
        cusbillstreet = $("#Street").val();
        cusbillzip = $("#ZipCode").val();
        $(".billing_address_div").removeClass('hidden');
    }
    else
    {
        cusbillstate = "";
        cusbillcity = "";
        cusbillstreet = "";
        cusbillzip = "";
        $(".billing_address_div").addClass('hidden');
    }
}
$(document).ready(function () {
    $("#btnsaveCC").click(function () {
        var validate_CardNumber1 = validate_CardNumber();
        var isCreditCard1 = isCreditCard($("#CardNumber").val().split("-").join(""));
        var checkExp1 = checkExp();
        var validate_cvv1 = validate_cvv($("#CardSecurityCode").val());
        var validate_AccountName1 = validate_AccountName();
        if (validate_CardNumber1 && isCreditCard1 && checkExp1 && validate_cvv1 && validate_AccountName1) {
            CCMethodFunction();
        }
    })
    $("#CardExpireDate").blur(function () {
        checkExp();
    });

    $(".viewSecurityCode").click(function () {
        if ($("#CardSecurityCode").attr("type") == "password") {
            $("#CardSecurityCode").attr("type", "text");
        }
        else {
            $("#CardSecurityCode").attr("type", "password");
        }

    });
    $("#CardNumber").blur(function () {
        var resultCheck = isCreditCard($(this).val().split("-").join(""));
        if (resultCheck) {
            $("#CardNumber").css("border-color", "#ccc");
            $("#CardType").val(GetCardType($("#CardNumber").val()));
        }
        else {
            $("#CardNumber").css("border-color", "red");
        }
    });
    $("#CardNumber").keyup(function (e) {
        var foo = $(this).val().split("-").join("");
        if (foo.length > 0) {
            foo = foo.match(new RegExp('.{1,4}', 'g')).join("-");
        }
        $(this).val(foo);
        if (e.keyCode == 8)
            $("#CardNumber").attr('maxlength', '19');

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
    $("#CardSecurityCode").blur(function () {
        validate_cvv($("#CardSecurityCode").val());
    });
    $(".ShowBINBtn").click(function () {
        GetCardBINDetail();
    });
    setTimeout(function () {
        $(".AddCardInnerContents").height(window.innerHeight - 100);
    }, 500);
    $('#CardExpireDate').bind('keyup', 'keydown', function (event) {
        var inputLength = event.target.value.length;
        if (event.keyCode != 8) {
            if (inputLength == 2) {
                var thisVal = event.target.value;
                thisVal += '/';
                $(event.target).val(thisVal);
            }
        }
    });
    ShowAddress();
    $(".cls_IsBillAddress").change(function () {
        ShowAddress();

        /*if ($(this).prop('checked') && $(this).hasClass('diff_IsBillAddress')) {
            console.log("checked");
            cusbillstate = $("#State").val();
            cusbillcity = $("#City").val();
            cusbillstreet = $("#Street").val();
            cusbillzip = $("#ZipCode").val();
            $(".billing_address_div").removeClass('hidden');
        }
        else {
            cusbillstate = "";
            cusbillcity = "";
            cusbillstreet = "";
            cusbillzip = "";
            $(".billing_address_div").addClass('hidden');
        }*/

    });

    if ($("#CardNumber").val() != "") {
        GetCardType($("#CardNumber").val());

    }
});