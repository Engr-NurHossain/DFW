String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

var PropertyLeadtemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                   //+ "<img src='{7}' class='EquipmentImage'>"
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'>{5}</em><div>{1}</div>" + "<br /><em class='tt_sug_manufac'>{7}</em>"
                       + "<em class='tt-eq-price'>${2}</em>"
                       + "<br />"
                   + "</p> "
                + "</div>";
var LeadEquipmentRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td valign='top'><input type='text'class='ProductName' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event)' />"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnProductName'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='number' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G hidden'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                                + "<input type='text' class='txtProductRate' />"
                            + "</div>"
                            
                            + "<p class='spnProductRate'></p>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                                + "<input type='text' class='txtProductDiscountRate' />"
                            + "</div>"
                            
                            + "<span class='spnProductDiscountRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G  hidden'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                               + "<input type='text' class='txtProductPackageDiscount' />"
                            + "</div>"

                            + "<p class='spnProductPackageDiscount'></p>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G  hidden'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                               + "<input type='text' class='txtTotalAmount' />"
                            + "</div>"
                            
                            + "<p class='spnTotalAmount'></p>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";

var InvoiceEqSuggestionclickbind = function (item) {
    $('.LeadEquipmentTab .tt-suggestion').click(function () {
        var clickitem = this;
        $('.LeadEquipmentTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

        var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemRate).text(1);
        var txtQuantity = $(item).parent().parent().find('.txtProductQuantity');
        $(txtQuantity).val(1);

        /*Item Rate Set*/
        var ProductRate = parseFloat($(this).attr('data-price'));
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text(Currency + ProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        $(txtItemRate).val(ProductRate);

        /*Item ProductDiscountRate Set*/
        var spnProductDiscount = $(item).parent().parent().find('.spnProductDiscountRate');
        $(spnProductDiscount).text(ProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var txtProductDiscount = $(item).parent().parent().find('.txtProductDiscountRate');
        $(txtProductDiscount).val(ProductRate);

        /*Item ProductPackageDiscount Set*/
        var spnPackageDiscount = $(item).parent().parent().find('.spnProductPackageDiscount');
        $(spnPackageDiscount).text(0);
        var txtPackageDiscount = $(item).parent().parent().find('.txtProductPackageDiscount');
        $(txtPackageDiscount).val(0);

        /*Total Amount Set*/
        var TotalAmount = parseFloat($(this).attr('data-price'));
        var spnTotalAmount = $(item).parent().parent().find('.spnTotalAmount');
        $(spnTotalAmount).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var txtTotalAmount = $(item).parent().parent().find('.txtTotalAmount');
        $(txtTotalAmount).val(TotalAmount);

    });
    $('.LeadEquipmentTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
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
        url: domainurl + "/Invoice/GetOnlyEquipmentListByKey",
        data: {
            key: $(item).val(),
            ExistEquipment: ExistEquipment
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log("Sample Data test ", data);
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyLeadtemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        /*7*/resultparse[i].ManufacturerName.replaceAll('"', '\'\''));
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
                    $(".NewProjectSuggestion").perfectScrollbar()
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDown = function (item, event) {

    if (event.keyCode == 13) {
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
        if ($('.tt-suggestion').length > 0) {
            if ($('.tt-suggestion.active').length == 0) {
                $($('.tt-suggestion').get(0)).addClass('active');
                $(item).val($($('.tt-suggestion').get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $('.tt-suggestion');
                var activesuggestion = $('.tt-suggestion.active');
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $('.tt-suggestion').removeClass('active');
                    var possibleactive = $('.tt-suggestion').get(indexactive + 1);
                    $($('.tt-suggestion').get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
        }
        event.preventDefault();
    }
    if (event.keyCode == 38) {
        if ($('.tt-suggestion').length > 0 && $('.tt-suggestion.active').length > 0) {
            var suggestionlist = $('.tt-suggestion');
            var activesuggestion = $('.tt-suggestion.active');
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $('.tt-suggestion').removeClass('active');
                var possibleactive = $('.tt-suggestion').get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
        }
        event.preventDefault();
    }
}
var InitRowIndex = function () {
    var i = 1;
    $(".LeadEquipmentTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}

var RemoveLeadEquipment = function (id,HideMessage) {
    var url = domainurl + "/SmartLeads/DeleteCustomerAppointmentEquipment";
    var Param = JSON.stringify({
        id: id
    });
    $.ajax({
        type: "POST",
        /*ajaxStart: $(".loader-div").show(),*/
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                $(".Equipment_" + id).remove();
                LeadEquipmetnPriceCalculation();
                if (typeof(HideMessage) == "undefined" || HideMessage == false ) {
                    OpenSuccessMessageNew('Success!', data.message, function () { });
                }
                var strPayInfo = "";
                var strTotalAmt = parseFloat(data.strTotalAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strCollectedAmt = parseFloat(data.strCollectedAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strAchPayment = data.strAchPayment;
                var strCreditCardPayment = data.strCreditCardPayment;
                var strCashPayment = data.strCashPayment;
                var strCheckPayment = data.strCheckPayment;
                if (data.strCreditCardPayment != '0') {
                    strPayInfo += "CC: " + Currency + strCreditCardPayment + ", ";
                }
                if (data.strAchPayment != '0') {
                    strPayInfo += "ACH: " + Currency + strAchPayment + ", ";
                }
                //if (data.strCashPayment != '0') {
                //    strPayInfo += "Cash: " + Currency + strCashPayment + ", ";
                //}
                //if (data.strCheckPayment != '0') {
                //    strPayInfo += "Check: " + Currency + strCheckPayment + ", ";
                //}
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
            

            } else {
                OpenErrorMessageNew('Error!', data.message, function () { });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var LeadEquipmetnPriceCalculation = function () {
    var totalRetailPrice = 0;
    var totalPrice = 0;
    $(".Setup-Equipments-Table .retail_price").each(function () {
        totalRetailPrice += parseFloat($(this).text().trim().replaceAll(',', ''));
    });
    $(".Setup-Equipments-Table .total_price").each(function () {
        totalPrice += parseFloat($(this).text().trim().replaceAll(',', ''));
    });

    $(".BottomTotalTr .retail-total").text('$' + totalRetailPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".BottomTotalTr .sub-total").text('$' + totalPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
}
var SelectAvailableRow = function () {
    var SelectedItem = null;
    $("#LeadEquipmentTab tbody tr").each(function (index ,item) {
        if (!$(this).hasClass('HasItem') && SelectedItem == null) {
            SelectedItem = this;
        }
    });
    if (SelectedItem == null) {
        $("#LeadEquipmentTab tbody tr:last").after(LeadEquipmentRow);
        var i = 1;
        $(".LeadEquipmentTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        SelectedItem = $("#LeadEquipmentTab tbody tr:last");
    }

    return SelectedItem;
}

var SetValuesToGivenRow = function (item, Equipment) {
    $(item).attr('dataid', Equipment.Id);
    $(item).attr('data-id', Equipment.EquipmentId);
    $(item).attr('data-serviceId', Equipment.ServiceId);
    $(item).attr('data-ispackageeqp', Equipment.IsPackageEqp);
    $(item).attr('data-isinclude', Equipment.IsIncluded);
    $(item).attr('data-isdevice', Equipment.IsDevice);
    $(item).attr('data-isoptionaleqp', Equipment.IsOptionalEqp);
    $(item).addClass('HasItem');

    /*Item Name Set*/
    var spnItemName = $(item).find('.spnProductName');
    $(spnItemName).text(Equipment.Name);
    var txtIteName = $(item).find('.ProductName');
    $(txtIteName).val(Equipment.Name);

    /*Item Quantity Set*/
    var spnItemQuantity = $(item).find('.spnProductQuantity');
    $(spnItemQuantity).text(Equipment.Quantity);
    var txtItemQuantity = $(item).find('.txtProductQuantity');
    $(txtItemQuantity).val(Equipment.Quantity);

    /*Item Rate Set*/
    var spnItemRate = $(item).find('.spnProductRate');
    $(spnItemRate).text(parseFloat(Equipment.Price).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    var txtItemRate = $(item).find('.txtProductRate');
    $(txtItemRate).val(Equipment.Price);

    /*Item ProductDiscountRate Set*/
    var spnItemRate = $(item).find('.spnProductDiscountRate');
    $(spnItemRate).text((Equipment.Price - Equipment.DiscounUnitPrice).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    var txtItemRate = $(item).find('.txtProductDiscountRate');
    $(txtItemRate).val(Equipment.Price - Equipment.DiscounUnitPrice);

    /*Item ProductPackageDiscount Set*/
    var spnItemRate = $(item).find('.spnProductPackageDiscount');
    $(spnItemRate).text(parseFloat(Equipment.PackageDiscount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    var txtItemRate = $(item).find('.txtProductPackageDiscount');
    $(txtItemRate).val(Equipment.PackageDiscount);

    /*Total Amount Set*/
    var spnItemRate = $(item).find('.spnTotalAmount');
    $(spnItemRate).text(parseFloat(Equipment.TotalPrice).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    var txtItemRate = $(item).find('.txtTotalAmount');
    $(txtItemRate).val(Equipment.TotalPrice);
}

$(document).ready(function () {
    InitRowIndex();
    $("#LeadEquipmentTab tbody").on('click', 'tr', function (e) {
        $('.LeadEquipmentTab .tt-menu').hide();
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#LeadEquipmentTab tr").removeClass("focusedItem");
        $(this).addClass("focusedItem");
        $(e.target).find('input').focus();
    });
    //$("#LeadEquipmentTab tbody").on('blur', '.ProductName', function (item) {
    //    console.log(item.target);
    //    if (typeof ($(item.target).attr('data-id')) == 'undefined') {
    //        $(item.target).parent().parent().remove();
    //        if ($(".LeadEquipmentTab tbody tr").length < 2) {
    //            $("#LeadEquipmentTab tbody tr:last").after(LeadEquipmentRow);
    //        }
    //        var i = 1;
    //        $(".LeadEquipmentTab tbody tr td:first-child").each(function () {
    //            $(this).text(i);
    //            i += 1;
    //        });
    //    }
    //});
    $("#LeadEquipmentTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#LeadEquipmentTab tbody tr:last").after(LeadEquipmentRow);
        var i = 1;
        $(".LeadEquipmentTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".LeadEquipmentTab tbody").on('click', 'tr td i.fa', function (e) {
        $(this).parent().parent().remove();
        if ($(".LeadEquipmentTab tbody tr").length < 2) {
            $("#LeadEquipmentTab tbody tr:last").after(LeadEquipmentRow);
        }
        var i = 1;
        $(".LeadEquipmentTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        var ProductDiscountDom = $(this).parent().parent().find('input.txtProductDiscountRate');
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        var ProductYouAmountDom = $(this).parent().parent().find('input.txtTotalAmount');
        var spnProductYouAmountDom = $(this).parent().parent().find('p.spnTotalAmount');

        if ($(this).val() > 0)
        {
            $(ProductQuantityDom).text($(this).val());
            if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {

                //var TotalAmount = ($(this).val() * $(ProductRateDom).val()) - ($(this).val() * $(ProductDiscountDom).val());
                var TotalAmount = ($(this).val() * $(ProductDiscountDom).val());

                $(ProductYouAmountDom).val(TotalAmount); 
                $(spnProductYouAmountDom).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
        }
        else {
            $(this).val(1);
            $(ProductQuantityDom).text(1);
            if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) { 
                //var TotalAmount = (1 * $(ProductRateDom).val()) - (1 * $(ProductDiscountDom).val());
                var TotalAmount = (1 * $(ProductDiscountDom).val());

                $(ProductYouAmountDom).val(TotalAmount); 
                $(spnProductYouAmountDom).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
        }
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductDiscountRate", function () {
        var ProductDiscountRateDom = $(this).parent().parent().find('span.spnProductDiscountRate');
        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var ProductYouAmountDom = $(this).parent().parent().parent().find('input.txtTotalAmount');
        var spnProductYouAmountDom = $(this).parent().parent().parent().find('p.spnTotalAmount');
        var PackageDiscountDom = $(this).parent().parent().parent().find('input.txtProductPackageDiscount');
        var spnPackageDiscountDom = $(this).parent().parent().parent().find('p.spnProductPackageDiscount');

        if ($(this).val() > -1)
        {
            var ProductDiscountRate = parseFloat($(this).val());
            $(ProductDiscountRateDom).text(Currency + ProductDiscountRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            
            var PackageDiscount = parseFloat($(ProductRateDom).val() - ProductDiscountRate);
            //removing 0.00
            var PackageDiscountVal = PackageDiscount.toFixed(2);
            if (PackageDiscountVal.indexOf(".00") > -1) {
                PackageDiscountVal = PackageDiscount.toFixed();
            }

            $(PackageDiscountDom).val(PackageDiscountVal);
            $(spnPackageDiscountDom).text(Currency + PackageDiscount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                //var TotalAmount = ($(ProductRateDom).val() * $(ProductQuantityDom).val()) - ($(this).val() * $(ProductQuantityDom).val());

                var TotalAmount = ($(this).val() * $(ProductQuantityDom).val()); 
                $(ProductYouAmountDom).val(TotalAmount);
                $(spnProductYouAmountDom).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
        }
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductRate", function () {
        var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        var ProductRate = parseFloat($(this).val());
        if (isNaN(ProductRate)) {
            ProductRate = 0;
        }
        $(ProductRateDom).text(Currency + ProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var ChargeUnitPriceDom = $(this).parent().parent().parent().find('input.txtProductDiscountRate');
        var ChargeUnitPriceSpn = $(this).parent().parent().parent().find('span.spnProductDiscountRate');

        var PackageDiscountDom = $(this).parent().parent().parent().find('input.txtProductPackageDiscount');
        var PackageDiscountSpn = $(this).parent().parent().parent().find('span.spnProductPackageDiscount');

        var ChargeUnitPrice = parseFloat($(ChargeUnitPriceDom).val());
        if (isNaN(ChargeUnitPrice)) {
            ChargeUnitPrice = 0;
        }
        var PackageDiscount = ProductRate - ChargeUnitPrice;

        $(PackageDiscountDom).val(PackageDiscount);
        $(PackageDiscountSpn).text(Currency + PackageDiscount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    });

    $(".LeadEquipmentTab tbody").on('change', "tr td .txtTotalAmount", function () {
        //var spanYourAmount = $(this).parent().find('span.spnYourAmount');
        var TotalAmountDom = $(this).parent().parent().find('span.spnTotalAmount');
        var ProductYouAmount = $(this).parent().parent().parent().find('input.txtTotalAmount');

        var txtProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var txtProductDiscountDom = $(this).parent().parent().parent().find('input.txtProductDiscountRate');
        var txtProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var txtPackageDiscountDom = $(this).parent().parent().parent().find('input.txtProductPackageDiscount');

        var spnProductQuantityDom = $(this).parent().parent().parent().find('span.spnProductQuantity');
        var spnProductDiscountDom = $(this).parent().parent().parent().find('span.spnProductDiscountRate');
        var spnProductRateDom = $(this).parent().parent().parent().find('span.spnProductRate');
        var spnPackageDiscountDom = $(this).parent().parent().parent().find('span.spnProductPackageDiscount');


        var ProductRate = parseFloat($(txtProductRateDom).val());
        var ProductQuantity = parseFloat($(txtProductQuantityDom).val());
        var ProductDiscount = parseFloat($(txtProductDiscountDom).val());
       

        if ($(this).val() > 0)
        {
            var TotalAmount = parseFloat($(this).val());
            $(TotalAmountDom).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            var ChargeAmount = ($(this).val() / ProductQuantity);
            var PackageDiscount = ProductRate - ChargeAmount;


            //removing 0.00
            var ChargeAmountVal = ChargeAmount.toFixed(2);
            if (ChargeAmountVal.indexOf(".00") > -1) {
                ChargeAmountVal = ChargeAmount.toFixed();
            } 
            $(txtProductDiscountDom).val(ChargeAmountVal);
            $(spnProductDiscountDom).text(Currency + ChargeAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

            //removing 0.00
            var PackageDiscountVal = PackageDiscount.toFixed(2);
            if (PackageDiscountVal.indexOf(".00") > -1) {
                PackageDiscountVal = PackageDiscount.toFixed();
            }

            $(txtPackageDiscountDom).val(PackageDiscountVal);
            $(spnPackageDiscountDom).text(Currency + PackageDiscount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
             
            //if ((ProductRate * ProductQuantity).toFixed(2) == TotalAmount.toFixed(2)) {
            //    //Discount =0
            //    $(txtProductDiscountDom).val(0);
            //    $(spnProductDiscountDom).text(0);
            //    //Done
            //}
            //else if ((ProductRate * ProductQuantity) > TotalAmount) { 
            //    //Discount = TotalAmount - (Rate*Quantity)
            //    ProductDiscount = -1 * (TotalAmount - (ProductRate * ProductQuantity));
            //    $(txtProductDiscountDom).val(ProductDiscount);
            //    $(spnProductDiscountDom).text(Currency + ProductDiscount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'))
            //}
            //else if ((ProductRate * ProductQuantity) < TotalAmount) {
            //    //Discount = 0
            //    //Rate = Total/Quantity
            //    $(txtProductDiscountDom).val(0);
            //    $(spnProductDiscountDom).text(0);

            //    ProductRate = TotalAmount / ProductQuantity;
            //    $(txtProductRateDom).val(ProductRate);
            //    $(spnProductRateDom).text(Currency + ProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            //}

        }
        else {
             
            //var TotalAmount = (ProductRate * ProductQuantity) - ProductDiscount;
            var TotalAmount = (ProductDiscount * ProductQuantity) ;
            $(TotalAmountDom).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(ProductYouAmount).val(TotalAmount);
        }
       
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductPackageDiscount", function () {
        console.log("txtProductPackageDiscount");

        var DiscountDom = $(this).parent().parent().find('span.spnProductPackageDiscount'); 
        var PackageDiscount = parseFloat($(this).val());  
        $(DiscountDom).text(Currency + PackageDiscount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var ChargeUnitPriceDom = $(this).parent().parent().parent().find('input.txtProductDiscountRate');
        var ChargeUnitPriceSpn = $(this).parent().parent().parent().find('span.spnProductDiscountRate');

        var UnitPriceDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var QuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');

        var TotalAmountDom = $(this).parent().parent().parent().find('input.txtTotalAmount');
        var TotalAmountSpn = $(this).parent().parent().parent().find('span.spnTotalAmount');

        var ChargeUnitPrice = parseFloat($(ChargeUnitPriceDom).val());
        var UnitPrice = parseFloat($(UnitPriceDom).val());
        var Quantity = parseInt($(QuantityDom).val());
        var TotalAmount = parseFloat($(TotalAmountDom).val());

        //Charge Unitprice Calculation
        ChargeUnitPrice = UnitPrice - PackageDiscount;
        $(ChargeUnitPriceDom).val(ChargeUnitPrice);
        $(ChargeUnitPriceSpn).text(Currency + ChargeUnitPrice.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        //Total Amount Calculation 
        TotalAmount = Quantity * ChargeUnitPrice;
        $(TotalAmountDom).val(TotalAmount);
        $(TotalAmountSpn).text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));


    });

    $(".existing-equipments .tableActions .fa-trash-o").click(function () {
        var id = $(this).attr('dataid');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this?", function () {
            RemoveLeadEquipment(id);
        });
    });
    $("#SaveEquipments").click(function () {
        if ($("#LeadEquipmentTab tr.HasItem").length > 0) {
            SaveLeadSetupEquipment(true);
        } else {
            OpenErrorMessageNew("","You need to select equipment first.");
        }
    });
    $(".existing-equipments .editEquipment").click(function (e) {

        var Equipment = {
            Id: $(e.target).attr("dataid"),
            EquipmentId : $(e.target).attr("data-equipId"),
            Name : $(e.target).attr("data-EquipName"),
            Quantity : $(e.target).attr("data-quantity"),
            Price : $(e.target).attr("data-unitprice"),
            DiscounUnitPrice : $(e.target).attr("data-discount"),
            PackageDiscount : $(e.target).attr("data-packagediscount"),
            TotalPrice: $(e.target).attr("data-total"),
            ServiceId: $(e.target).attr("data-serviceId"),
            IsPackageEqp: $(e.target).attr("data-ispackageeqp"),
            IsIncluded: $(e.target).attr("data-isinclude"),
            IsDevice: $(e.target).attr("data-isdevice"),
            IsOptionalEqp: $(e.target).attr("data-isoptionaleqp")
        }
        console.log(Equipment);
        var AvailableRow = SelectAvailableRow();
        SetValuesToGivenRow(AvailableRow, Equipment);

        /*Delete Existing Row*/
        //var id = $(this).attr('dataid'); 
        //RemoveLeadEquipment(id,true); 

    });
});
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           