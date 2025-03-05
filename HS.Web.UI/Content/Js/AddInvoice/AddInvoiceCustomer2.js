var TotalAmount = 0; /*TotalAmount: Invoice Table -> Amount*/
var FinalTotal = 0;/*FinalTotal: Invoice Table -> TotalAmount*/
var DiscountAmount = 0;
var ShippingAmount = 0;
var DepositAmount = 0;
var BalanceDue = 0;
var DiscountDBPercent = 0;
var DiscountDBAmount = 0;
var TaxAmount = 0;
var SendEmailUrl = "";
var mailAdd = "";
var InvoiceDatepicker;
var DueDatepicker;
var shippingDatePicker;
var Fdiscountamount = 0;






//var CheckItemIncludedOrNot = function () {
//    var result = false;
//    var count = 0;
//    $(".HasItem").each(function () {
//        count = $(this).length;
//    })
//    if (count > 0) {
//        result = true;
//    }
//    return result;
//}

var InvoiceCustomerclickbind = function (item) {
    $('.customer_name_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.customer_name_insert_div .tt-menu').hide();

        var selectedEmail = $(clickitem).attr("data-emailAddress").trim();

        var BussiName = $(clickitem).attr("data-Bussiness").trim();
        var Customerfnum = $(clickitem).attr("data-firstName").trim();
        var Customerlnum = $(clickitem).attr("data-lastName").trim();
        var CustomerGuId = $(clickitem).attr("data-customerId").trim();
        var CustomerType = $(clickitem).attr("data-type").trim();
        console.log(CustomerType);
        if (CustomerType == "Commercial") {
            var displayname = BussiName;
        }
        else {
            var displayname = Customerfnum + " " + Customerlnum;
        }

        $("#CustomerList").val(displayname);
        $("#EmailAddress").val(selectedEmail);
        $("#InvoiceCustomerId").val(CustomerGuId);
        $.ajax({
            type: "POST",
            url: domainurl + "/Invoice/GetCustomerAddressByCustomerId",
            data: JSON.stringify({
                CustomerId: CustomerGuId
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result == true) {
                    tinyMCE.get('Invoice_BillingAddress').setContent(data.BillingAddressVal);
                    tinyMCE.get('Invoice_ShippingAddress').setContent(data.ShippingAddressVal);
                }
            }
        });
    });
    $('.customer_name_insert_div .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var CustomerSearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        if (selectedTTMenu.length > 0) {
            setTimeout(function () { $(selectedTTMenu).click(); }, 10)
            $('.tt-menu').hide();
        }
    }
    if (event.keyCode == 40) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
            event.preventDefault();
        }
    }
    if (event.keyCode == 38) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }
    }
}

var CustomerSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Invoice/GetCustomerListByKey",
        data: {
            key: $(item).val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    if (resultparse[i].Type == "Commercial") {
                        var name = resultparse[i].BusinessName;
                    }
                    else {
                        var name = resultparse[i].FirstName + ' ' + resultparse[i].LastName;
                    }

                    searchresultstring = searchresultstring + String.format(CustomerSuggestiontemplate,
                        resultparse[i].Address,/*0*/
                        resultparse[i].Address1,/*1*/
                        resultparse[i].Street, /*2*/
                        resultparse[i].Street1,/*3*/
                        resultparse[i].City,/*4*/
                        resultparse[i].City1,/*5*/
                        resultparse[i].State == "-1" ? "" : resultparse[i].State,/*6*/
                        resultparse[i].State1,/*7*/
                        resultparse[i].ZipCode,/*8*/
                        resultparse[i].ZipCode1,/*9*/
                        resultparse[i].BusinessName,/*10*/
                        resultparse[i].FirstName,/*11*/
                        resultparse[i].LastName,/*12*/
                        resultparse[i].EmailAddress,/*13*/
                        resultparse[i].CustomerId,/*14*/
                        resultparse[i].Type,/*15*/
                        name);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                InvoiceCustomerclickbind(item);
                if (resultparse.length > 5) {
                    $(".customer_name_insert_div .NewProjectSuggestion").height(200);
                    //$(".NewProjectSuggestion").css('position', 'relative');
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}


var SaveAndNew = function () {
    SaveInvoice(false, false, "others", null);
    if ($(".HasItem").length != 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?customerid=" + customerId);
    }
}
var SaveAndClose = function () {
    SaveInvoice(true, false, "others", null);
    //if ($(".HasItem").length != 0) {
    //    CloseTopToBottomModal();
    //    OpenSuccessMessageNew("Success!", "Invoice Successfully Saved.", function () { OpenInvoiceTab() });
    //}
}
var SaveAndShare = function () {
    SaveInvoice(false, true);
}

var makeSendEmailUrl = function () {
    
    mailAdd = encodeURIComponent($("#EmailAddress").val());
    SendEmailUrl = domainurl + "/Invoice/SendEmailInvoice/?Id=" + UrlModelInvoiceId + "&EmailAddress=" + mailAdd;
    console.log("Email link created.:" + SendEmailUrl);

    $("#InvoiceInfoPrintAndSend").attr("href", SendEmailUrl);
}

var SaveAndSend = function () {

    makeSendEmailUrl();
    SaveInvoice(false, true, "preview", null);
    OpenInvoiceTab();
}
var InvoiceEqSuggestionclickbind = function (item) {
    $('.CustomerInvoiceTab .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CustomerInvoiceTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text($(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        $(txtItemRate).val($(this).attr('data-price'));
        /*Item Description Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductDesc');
        $(spnItemRate).text($(this).attr('data-description'));
        var txtItemRate = $(item).parent().parent().find('.txtProductDesc');
        $(txtItemRate).val($(this).attr('data-description'));
        /*Item Quantity Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemRate).text(1);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantity');
        $(txtItemRate).val(1);
        /*Item Amount Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        $(spnItemRate).text($(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val($(this).attr('data-price'));

        /*
        $(item).parent().parent().addClass('focusedItem');
        $(item).focus();
        */

        CalculateNewAmount();

    });
    $('.CustomerInvoiceTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13 || event.keyCode == 9)
        return false;
    var ExistEquipment = "";
    var ExistEquipmentInner = "";
    $(".HasItem").each(function () {
        ExistEquipmentInner += "'" + $(this).attr('data-id') + "',";
    });
    if (ExistEquipmentInner.length > 0) {
        ExistEquipmentInner = ExistEquipmentInner.slice(0, -1);
        ExistEquipment = "(" + ExistEquipmentInner + ")";
    }
    $.ajax({
        url: domainurl + "/Invoice/GetEquipmentListByKey",
        data: {
            key: $(item).val(),
            ExistEquipment: ExistEquipment
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType,
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\'')
                        , resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/* ImageSource*/);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                InvoiceEqSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDown = function (item, event) {

    if (event.keyCode == 13) {/*Enter*/
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {/*Down*/
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugstionDom).is(':visible')) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }

            event.preventDefault();
        }
        else {

            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).next('tr')).addClass('focusedItem');
            if ($(event.target).hasClass('ProductName')) {
                $($(trselected).next('tr')).find('input.ProductName').focus();
            }
        }
    }
    if (event.keyCode == 38) {/*UP*/
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0 && $(ttSugstionDom).is(':visible')) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).prev('tr')).addClass('focusedItem');
            $($(trselected).prev('tr')).find('input.ProductName').focus();
        }
    }

}

var OthersKeyDown = function (item, event) {
    if (event.keyCode == 40) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).next('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtProductDesc')) {
            $($(trselected).next('tr')).find('input.txtProductDesc').focus();
        } else if ($(event.target).hasClass('txtProductQuantity')) {
            $($(trselected).next('tr')).find('input.txtProductQuantity').focus();
        } else if ($(event.target).hasClass('txtProductRate')) {
            $($(trselected).next('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).next('tr')).find('input.txtProductAmount').focus();
        }
    } else if (event.keyCode == 38) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).prev('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtProductDesc')) {
            $($(trselected).prev('tr')).find('input.txtProductDesc').focus();
        } else if ($(event.target).hasClass('txtProductQuantity')) {
            $($(trselected).prev('tr')).find('input.txtProductQuantity').focus();
        } else if ($(event.target).hasClass('txtProductRate')) {
            $($(trselected).prev('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).prev('tr')).find('input.txtProductAmount').focus();
        }
    }
    else if (event.keyCode == 9 && $(event.target).hasClass('txtProductAmount')) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        var trfocuseditem = $(trselected).next('tr');
        $(trfocuseditem).addClass('focusedItem');
        $($(trfocuseditem).find('input.ProductName')).focus();
        event.preventDefault();
    }

}
var CalculateNewAmount = function () {
    var amount = parseFloat('0');
    $(".txtProductAmount").each(function () {
        var _CalAmt = $(this).val().trim();
        _CalAmt = _CalAmt.replaceAll(',', '');

        var currAmount = parseFloat(_CalAmt);
        if (!isNaN(currAmount)) {
            amount += currAmount;
        }
    });
    amount = parseFloat(amount).toFixed(2);
    amount1 = amount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    $(".amount").text(TransMakeCurrency + amount1);
    TotalAmount = amount;
    FinalTotal = amount;
    BalanceDue = amount; 
    if ($("#Invoice_DiscountType").val() == "percent") {
        var a = 0;
        var Fval = 0;
        var discountAmount = 0;
        if ($("#discountAmount").length > 0) {
            if ($("#discountAmount").val() == "") {
                discountAmount = 0;
            }
            else {
                discountAmount = $("#discountAmount").val();
            }
        }
        Fdiscountamount = TotalAmount - ((amount / 100) * discountAmount);
        if (discountAmount != "" && Fdiscountamount > 0) {
            var discountAmountPercent = parseFloat(discountAmount);
            DiscountDBPercent = discountAmountPercent;
            DiscountDBAmount = a;
            DiscountAmount = (amount / 100) * discountAmountPercent;
            FinalTotal = TotalAmount - DiscountAmount;
            BalanceDue = FinalTotal;
            $(".shippingAmountTxt").text(TransMakeCurrency + DiscountAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            if (DiscountAmount == 0) {
                $(".Discount-total").addClass('hidden');
            }
            else {
                $(".DiscountAmountTxt").text(TransMakeCurrency + Fdiscountamount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
        }
        else {
            $(".shippingAmountTxt").text(TransMakeCurrency + "0.00");
            $("#discountAmount").val("");
            $(".DiscountAmountTxt").text(TransMakeCurrency + "0.00");
        }
    }
    if ($("#Invoice_DiscountType").val() == "amount") {
        var a = 0;
        var discountAmount = 0;
        if ($("#discountAmount").length > 0) {
            if ($("#discountAmount").val() == "") {
                discountAmount = 0;
            }
            else {
                discountAmount = $("#discountAmount").val();
            }
        }
        if (discountAmount != "" && DiscountAmount < amount) {
            var discountAmountPercent = parseFloat(discountAmount);
            DiscountDBAmount = discountAmountPercent;
            DiscountDBPercent = a;
            DiscountAmount = discountAmountPercent;
            FinalTotal = TotalAmount - DiscountAmount;
            BalanceDue = FinalTotal;
            Fdiscountamount = amount - DiscountAmount;
            $(".shippingAmountTxt").text(TransMakeCurrency + DiscountAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            if (DiscountAmount == 0) {
                $(".Discount-total").addClass('hidden');
            }
            else {
                $(".DiscountAmountTxt").text(TransMakeCurrency + Fdiscountamount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
            /*$(".FinalTotalTxt").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            
            $(".balanceDueAmount").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));*/

        }
        else {
            $(".shippingAmountTxt").text(TransMakeCurrency + "0.00");
            $("#discountAmount").val("");
            $(".DiscountAmountTxt").text(TransMakeCurrency + "0.00");
        }
    }

    if ($("#taxType").val() != "") {
        CalculateTax();
    } 
    /*
    if ($("#taxType").val() != "") {
        var TPVal = $("#taxType").val();
        var TPercent = parseFloat(TPVal);
        if ($("#discountAmount").val() != "") {
            TaxAmount = (Fdiscountamount / 100) * TPercent;
            $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }
        else {
            TaxAmount = (FinalTotal / 100) * TPercent;
            $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }
        ////was commented before tax changes.
        ////FinalTotal = parseFloat(FinalTotal) + parseFloat(TaxAmount);
        ////$(".FinalTotalTxt").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        ////BalanceDue = FinalTotal;
        ////$(".balanceDueAmount").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }*/
    if ($("#Invoice_ShippingCost").val() != "") {
        var shippingCostString = $("#Invoice_ShippingCost").val();
        ShippingAmount = parseFloat(shippingCostString);

    }
    var DA = 0;
    if ($("#Invoice_Deposit").val() != "") {
        DA = $("#Invoice_Deposit").val();
        if ($("#discountAmount").val() != "") {
            BalanceDue = Fdiscountamount - parseFloat(DA);
        }
        else {
            BalanceDue = FinalTotal - parseFloat(DA);
        }
        /* $(".balanceDueAmount").text("$" + BalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));*/
    }
    if ($("#discountAmount").val() != "") {
        BalanceDue = parseFloat(Fdiscountamount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount) - parseFloat(DA);
        FinalTotal = parseFloat(Fdiscountamount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount);
    }
    else {
        BalanceDue = parseFloat(TotalAmount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount) - parseFloat(DA);
        FinalTotal = parseFloat(TotalAmount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount);
    }

    $(".FinalTotalTxt").text(TransMakeCurrency + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".balanceDueAmount").text(TransMakeCurrency + BalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".amount-big").text(TransMakeCurrency + BalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
}

var CalculateTax = function () {
    var taxableamount = parseFloat('0'); 
    $(".txtProductAmount").each(function () {
        if (!$(this).parent().parent().parent().hasClass("NonTaxable")) {
            var _CalAmt = $(this).val().trim();
            _CalAmt = _CalAmt.replaceAll(',', '');

            var currAmount = parseFloat(_CalAmt);
            if (!isNaN(currAmount)) {
                taxableamount += currAmount;
            }
        }
    });

    var TPVal = $("#taxType").val();
    var TPercent = parseFloat(TPVal);
    if ($("#discountAmount").val() != "") {
        TaxAmount = ((taxableamount - DiscountAmount) / 100) * TPercent;
        $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }
    else {
        TaxAmount = (taxableamount / 100) * TPercent;
        $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }
}

var InitRowIndex = function () {
    var i = 1;
    $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var OpenInvoiceSend = function () {
    /*console.log('Open invoice send');*/
    setTimeout(function () {
        $("#InvoiceInfoPrintAndSend").trigger('click');
    },500);
}
var SaveInvoice = function (SendEmail, CreatePdf, CameFrom, EmailDescription, invoiceid, EmailSubject, ccEmail) {
    var memoval = "";
    console.log("Save invoice fired");
    if ($(".HasItem").length == 0) { 
        OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", function () { });
        return;
    }
    console.log("not returned");
    if ($("#InvoiceDescription").val().trim() == '') {
        var DescriptionText = "";
        $(".txtProductDesc").each(function () {
            if($(this).val().trim()!=''){
                DescriptionText += $(this).val();
                DescriptionText += ", ";
            }
        });
         
        DescriptionText = DescriptionText.slice(0, -2);

        $("#InvoiceDescription").val(DescriptionText);
    }

    if (typeof (SendEmail) == "undefined") {
        SendEmail = false;
    }
    if (typeof (CreatePdf) == "undefined") {
        CreatePdf = false;
    }
    if (typeof (CameFrom) == "undefined") {
        CameFrom = "";
    }
    if (typeof (EmailSubject) == "undefined") {
        EmailSubject = "";
    }
    var DetailList = [];
    $(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            EquipDetail: $(this).find('.txtProductDesc').val(),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val().trim().replaceAll(',', ''),
            TotalPrice: ($(this).find('.txtProductQuantity').val() * parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', ''))).toString(),
            /*EquipmentDescription: $(this).find('.txtProductDesc').val(),*/
            EquipName: $(this).find('.ProductName').val(),
            Taxable: (!$(this).hasClass('NonTaxable')),
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
            InvoiceId: InvoiceId
        });
    });
    if (memopermit == "True") {
        memoval = $("#Memo").val();
    }
    var url = domainurl + "/Invoice/AddInvoice";

    var param = JSON.stringify({
        EmailAddress: $("#EmailAddress").val(),
        "Invoice.InvoiceId": InvoiceId,
        "Invoice.Id": Invoice_int_Id,
        "Invoice.BillingAddress": tinyMCE.get('Invoice_BillingAddress').getContent(),
        "Invoice.Terms": $("#Invoice_Terms").val(),
        "Invoice.ShippingAddress": tinyMCE.get('Invoice_ShippingAddress').getContent(),
        "Invoice.ShippingAddress": tinyMCE.get('Invoice_ShippingAddress').getContent(),
        "Invoice.ShippingVia": $("#Invoice_ShippingVia").val(),
        "Invoice.ShippingDate": shippingDatePicker.getDate(),
        "Invoice.ShippingCost": $("#Invoice_ShippingCost").val(),
        "Invoice.TrackingNo": $("#Invoice_TrackingNo").val(),
        "Invoice.CustomerId": $("#InvoiceCustomerId").val(),
        "Invoice.InvoiceDate": GetTimeFormat( $("#Invoice_InvoiceDate").val()),
        "Invoice.DueDate": GetTimeFormat( $("#Invoice_DueDate").val()),
        "Invoice.TotalAmount": FinalTotal,
        "Invoice.Amount": TotalAmount,
        "Invoice.Message": $("#InvoiceMessage").val(),
        "Invoice.Deposit": $("#Invoice_Deposit").val(),
        "Invoice.DiscountAmount": DiscountDBAmount,
        "Invoice.Discountpercent": DiscountDBPercent,
        "Invoice.BalanceDue": BalanceDue,
        "Invoice.Tax": TaxAmount,
        "Invoice.TaxType": $("#taxType option:selected").text(),
        "Invoice.Memo": memoval,
        "Invoice.Description": $("#InvoiceDescription").val(),
        "Invoice.DiscountType": $("#Invoice_DiscountType").val(),
   //     "Invoice.Id": parseInt(invoiceid),

        InvoiceDetailList: DetailList,
        SendEmail: SendEmail,
        invoiceid: invoiceid,
        CreatePdf: CreatePdf,
        EmailDescription: EmailDescription,
        EmailSubject: EmailSubject,
        ccEmail: ccEmail
    });

    $.ajax({
        type: "POST",
        ajaxStart: $(".AddInvoiceLoader").removeClass('hidden'),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".AddInvoiceLoader").addClass('hidden');
            if (CameFrom == "preview") { 
                OpenInvoiceSend();
            }
            else if (data.result && CameFrom != "others" && data.EmailSent == false) {
                OpenSuccessMessageNew("Success!", "Invoice saved successfully!", function () { OpenInvoiceTab() });
                CloseTopToBottomModal();
            }
            else if (data.result && CameFrom != "others" && data.EmailSent == true) {
                OpenSuccessMessageNew("Success!", data.message, function () { OpenInvoiceTab() });
                CloseTopToBottomModal();
            } else if (data.result)
            {
                OpenSuccessMessageNew("Success!", data.message, function () { OpenInvoiceTab() });
                CloseTopToBottomModal();
            }
            else if (!data.result) {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
            $(".AddInvoiceLoader").addClass('hidden');
        }
    });

}
var LoadDiscount = function () {
    if (DiscountAmountDbValue != "0") {
        $("#discountAmount").val(DiscountAmountDbValue);
        $('#discountType').val('amount');
        DiscountDBPercent = 0;
    }
    if (DiscountPercentDbValue != "0") {
        $("#discountAmount").val(DiscountPercentDbValue);
        $('#discountType').val('percent');
        DiscountDBAmount = 0;
    }
}
var LoadTax = function () {
    console.log("TaxTypeDbValue" + TaxTypeDbValue);
    if (TaxTypeDbValue != null && TaxTypeDbValue != "") {
        console.log("TaxType" + TaxTypeDbValue)
        $("#taxType option").each(function () {
            if ($(this).text() == TaxTypeDbValue) {
                console.log("TaxType ok" + TaxTypeDbValue)
                $(this).prop('selected', 'selected');
            }
        });
    }
}

var Showshipping = function () {
    //$(".shipping").show();
    $(".Shipping").show();
}
var Hideshipping = function () {
    //$(".shipping").hide();
    $(".Shipping").hide();
}
var ShowDiposit = function () {
    $(".Diposit").show();
}
var HideDiposit = function () {
    $(".Diposit").hide();
}
var ShowDiscount = function () {
    $(".Discount").show();
    $(".Discount-total").removeClass('hidden');
}
var HideDiscount = function () {
    $(".Discount").hide();
}
var ShowShippingDiv = function () {
    $(".shipping-div").show();
    $(".shipping-amount-div").show();
}
var HideShippingDiv = function () {
    $(".shipping-div").hide();
    $(".shipping-amount-div").hide();
}
var ShowDiscountDiv = function () {
    $(".discount-amount-div").show();
    $(".Discount-total").removeClass('hidden');
}
var HideDiscountDiv = function () {
    $(".discount-amount-div").hide();
}
var ShowDepositDiv = function () {
    $(".deposit-amount-div").show();
}
var HideDepositDiv = function () {
    $(".deposit-amount-div").hide();
}

var InvoiceSettingsInitialLoad = function () {
    if (InvoiceShippingSetting == "True") {
        setTimeout(function () {
            $(".shipping-div").show();
            $(".shipping-amount-div").show();
            $(".Shipping").show();
        }, 10);
        //ShowShippingDiv();
        //Showshipping();
    }
    else {
        setTimeout(function () {
            $(".shipping-div").hide();
            $(".shipping-amount-div").hide();
            $(".Shipping").hide();
            $(".shippingAddress").val("");
        }, 10);
        //HideShippingDiv();
        //Hideshipping();
    }
    if (InvoiceDiscountSetting == "True") {
        setTimeout(function () {
            $(".discount-amount-div").show();
            $(".Discount").show();
        }, 10);
        //ShowDiscountDiv();
        //ShowDiscount();
    }
    else {
        setTimeout(function () {
            $(".discount-amount-div").hide();
            $(".Discount").hide();
        }, 10)
        //HideDiscountDiv();
        //HideDiscount();
    }
    if (InvoiceDepositSetting == "True") {
        setTimeout(function () {
            $(".deposit-amount-div").show();
            $(".Diposit").show();
        }, 10)
        //ShowDepositDiv();
        //ShowDiposit();
    }
    else {
        setTimeout(function () {
            $(".deposit-amount-div").hide();
            $(".Diposit").hide();
        }, 10)
        //HideDepositDiv();
        //HideDiposit();
    }
}

$(document).ready(function () {
    $("#discountAmount").keyup(function () {
        if ($("#discountAmount").val() != "") {
            $(".Discount-total").removeClass('hidden');
        }
        else {
            $(".Discount-total").addClass('hidden');
        }
    })
    makeSendEmailUrl();
    InvoiceSettingsInitialLoad();
    $("#EmailAddress").keydown(function () {
        makeSendEmailUrl();
    });
    $("#EmailAddress").keyup(function () {
        makeSendEmailUrl();
    });
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".InvEstPreview", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".CustomerInvoiceTab tbody").sortable({
        update: function () {
            var i = 1;
            $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
                $(this).text(i);
                i += 1;
            });
        }
    }).disableSelection();
    InvoiceDatepicker = new Pikaday({
        field: $('#Invoice_InvoiceDate')[0],
        format: 'MM/DD/YYYY'
    });
    DueDatepicker = new Pikaday({
        field: $('#Invoice_DueDate')[0],
        format: 'MM/DD/YYYY'
    });
    shippingDatePicker = new Pikaday({
        field: $('.ShippingDatePicker')[0],
        format: 'MM/DD/YYYY'
    });
    LoadDiscount();
    LoadTax();
    InitRowIndex();
    $(".DescStartCount").html($("#InvoiceDescription").val().length);
    $("#InvoiceDescription").keyup(function () {
        $(".DescStartCount").html($("#InvoiceDescription").val().length);
    });
    $(".StartCount").html($("#InvoiceMessage").val().length);
    $("#InvoiceMessage").keyup(function () {
        $(".StartCount").html($("#InvoiceMessage").val().length);
    });
    if (memopermit == "True") {
        $(".MemoStartCount").html($("#Memo").val().length);
        $("#Memo").keyup(function () {
            $(".MemoStartCount").html($("#Memo").val().length);
        });
    }

    $("#CustomerList").focusout(function () {
        setTimeout(function () {
            $(".customer_name_insert_div .tt-menu").hide();
        }, 200);
    });

 
  
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductRate", function () {
        /*
        *If product rate changes make change to amount.
        */
        var ProductRateDom = $(this).parent().find('span.spnProductRate');
        $(ProductRateDom).text(parseFloat($(this).val().trim().replaceAll(',','')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
            var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();


            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            CalculateNewAmount();
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductDesc", function () {

        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });
    $(".InvoiceSaveButton").click(function () {
        
        if ($(".HasItem").length == 0) {
            OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
        } else {
            SaveInvoice(false, true, "others", null);
            var Invoval = $("#Invoice_Status").val();
            if (Invoval == "Cancel") {
                $.ajax({
                    url: domainurl + "/Invoice/ConvertInvoiceStatus/",
                    data: { Invoval, Invoice_int_Id },
                    type: "Post",
                    dataType: "Json",
                    success: function (data) {
                        if (data == true) {
                            CloseTopToBottomModal();
                            OpenInvoiceTab();
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }
                })
            }
        }
    });
    $("#Invoice_DiscountType").change(function () {
        CalculateNewAmount();
    });
    $("#discountAmount").change(function () {
        CalculateNewAmount();
    });
    $('#Invoice_ShippingCost').focusout(function () {
        CalculateNewAmount();
    });
    $('#discountAmount').focusout(function () {
        CalculateNewAmount();
    });
    $('#Invoice_Deposit').focusout(function () {
        CalculateNewAmount();
    });
    $("#taxType").change(function () {
        if ($("#taxType").val() == "") {
            $(".tax ").text(TransMakeCurrency + "0.00");
            CalculateNewAmount();
        }
        else {
            CalculateNewAmount();
        }
    });
 
    CalculateNewAmount();
    CalculateNewAmount();
    if (InvoiceStatus == "Partial") {
        var invoicedom = $(".invoice-informations");
        invoicedom.find("input").prop("disabled", true);
        invoicedom.find("select").prop("disabled", true);
        invoicedom.find("textarea").prop("disabled", true);
        $(".balanceDueAmount").text(TransMakeCurrency + InvoiceBalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(".big-amount-top.amount").text(TransMakeCurrency + InvoiceBalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    } else if (InvoiceStatus == "Paid") {
        var invoicedom = $(".invoice-informations");
        invoicedom.find("input").prop("disabled", true);
        invoicedom.find("select").prop("disabled", true);
        invoicedom.find("textarea").prop("disabled", true);
        $(".balanceDueAmount").text(TransMakeCurrency + InvoiceBalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }
});

