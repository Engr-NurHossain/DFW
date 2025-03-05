function FormatePhoneNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
         if (Value.length == 10) {
            
             ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
        }
        else {
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
function FormateCardNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-  ]/g, '');
        if (Value.length == 16) {

            ValueClean = Value.replace(/(\d{4})\-?(\d{4})\-?(\d{4})\-?(\d{4})/, "$1-$2-$3-$4");
        }
        else {
            ValueClean = Value;
        }
    }
    return ValueClean;
}
function FormateCardEX(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-  ]/g, '');
        if (Value.length == 4) {
            ValueClean = Value.replace(/(\d{2})\/?(\d{2})/, "$1/$2");
        }
        else {
            ValueClean = Value;
        }
    }
    return ValueClean;
}
$(document).ready(function () {
    $(".phone-number-format").each(function () {
        var PhoneNumber = $(this).text();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).text(cleanPhoneNumber);
        }
    });
    $(".input-phone-format").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var onlyNumber = PhoneNumber.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3").length;

            if (onlyNumber == 10 && isNumeric(PhoneNumber) == true) {
                console.log("StringCheck");
                $(".input-phone-format").removeClass("required");
                var FirstDigit = PhoneNumber.charAt(0);
                if (FirstDigit == 0) {
                    $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
                }
                else {
                    var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                    $(this).val(cleanPhoneNumber);
                }
            }
            else {
             
                $(".input-phone-format").addClass("required");
            }
         }
        
    });
    $(".input-phone-format").blur(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var FirstDigit = PhoneNumber.charAt(0);
            if (FirstDigit == 0) {
                $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
            }
            else {
                var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });


    $(".phone2-number-format").each(function () {
        var PhoneNumber = $(this).text();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).text(cleanPhoneNumber);
        }
    });
    $(".input-phone2-format").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var onlyNumber = PhoneNumber.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3").length;

            if (onlyNumber == 10 && isNumeric(PhoneNumber) == true) {

                $(".input-phone2-format").removeClass("required");
                var FirstDigit = PhoneNumber.charAt(0);
                if (FirstDigit == 0) {
                    $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
                }
                else {
                    var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                    $(this).val(cleanPhoneNumber);
                }
            }
            else {

                $(".input-phone2-format").addClass("required");
            }
        }

    });
    $(".input-phone2-format").blur(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var FirstDigit = PhoneNumber.charAt(0);
            if (FirstDigit == 0) {
                $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
            }
            else {
                var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });




    $(".phone3-number-format").each(function () {
        var PhoneNumber = $(this).text();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).text(cleanPhoneNumber);
        }
    });
    $(".input-phone3-format").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var onlyNumber = PhoneNumber.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3").length;

            if (onlyNumber == 10 && isNumeric(PhoneNumber) == true) {

                $(".input-phone3-format").removeClass("required");
                var FirstDigit = PhoneNumber.charAt(0);
                if (FirstDigit == 0) {
                    $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
                }
                else {
                    var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                    $(this).val(cleanPhoneNumber);
                }
            }
            else {

                $(".input-phone3-format").addClass("required");
            }
        }

    });
    $(".input-phone3-format").blur(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var FirstDigit = PhoneNumber.charAt(0);
            if (FirstDigit == 0) {
                $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
            }
            else {
                var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });

    $(".Billingphone-number-format").each(function () {
        var PhoneNumber = $(this).text();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).text(cleanPhoneNumber);
        }
    });
    $(".input-Billingphone-format").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var onlyNumber = PhoneNumber.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3").length;

            if (onlyNumber == 10 && isNumeric(PhoneNumber) == true) {

                $(".input-Billingphone-format").removeClass("required");
                var FirstDigit = PhoneNumber.charAt(0);
                if (FirstDigit == 0) {
                    $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
                }
                else {
                    var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                    $(this).val(cleanPhoneNumber);
                }
            }
            else {

                $(".input-Billingphone-format").addClass("required");
            }
        }

    });
    $(".input-Billingphone-format").blur(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var FirstDigit = PhoneNumber.charAt(0);
            if (FirstDigit == 0) {
                $(this).val(PhoneNumber.substring(1, PhoneNumber.length));
            }
            else {
                var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
                $(this).val(cleanPhoneNumber);
            }
        }
    });


    $(".input-ssn-format").keyup(function () {
        var SSN = $(this).val();
        if (SSN != undefined && SSN != null && SSN != "") {
       
                var cleanSSN = FormateSSNNumber(SSN);
                $(this).val(cleanSSN);
            
        }
    });
    $(".input-card-format").keyup(function () {
        var card = $(this).val();
        if (card != undefined && card != null && card != "") {

                var cleancard = FormateCardNumber(card);
                $(this).val(cleancard);
            
        }
    });
    $(".input-cardex-format").keyup(function () {
        var card = $(this).val();
        if (card != undefined && card != null && card != "") {
                var cleancard = FormateCardEX(card);
                $(this).val(cleancard);
            
        }
    });
    //$(".input-phone-format").blur();
});