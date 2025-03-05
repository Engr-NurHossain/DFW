var PI = 3.14159;
var BalanceDiff = 0;
var AmountChange = 0;
var TotalAmount = 0;

var RugShapeSuggestiontemplate =
            "<div class='tt-suggestion tt-selectable' onclick='' data-displaytext='{0}'>"
                //+ "<img src='{7}' class='EquipmentImage'>"
                + "<p class='tt-sug-text'>"
                    + "<em class='tt-sug-type'></em>{0}" + "<br />"
                + "</p> "
    + "</div>";



/***
 * 
 * Extra Item Search Related
 * */
var PropertyUserSuggestiontemplate =
            '<div style="padding:unset;" class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                //+ "<img src='{7}' class='EquipmentImage'>"
                + "<div style='padding:10px 10px 9px 15px'> <p class='tt-sug-text'>"
                    + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{7}</em>"
                    + "<em class='tt-eq-price'>${2}</em>"
                    + "<br />"
                + "</p> </div>"
            + "</div>";
var PackageSuggestiontemplate =
            "<div class='tt-suggestion tt-selectable' data-package-id='{0}' data-packagename='{1}' data-package-include='{2}' data-package-rate='{3}'>"
                + "<p class='tt-sug-text'>"
                    + "<em class='tt-sug-type'></em>{1}" + "<br />"
                + "</p> "
            + "</div>";

var NewRugRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td class='SettingsCol'></td>"
                        + "<td valign='top'  class='rug-type-suggestion'><input type='text'class='RugShape' onkeydown='SearchKeyDown(this,event)' onclick='SearchKeyUp(this, event)' readonly/>"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnRugShape'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' placeholder='ft' onchange='CircleAreaCal(this)' class='txtRugRadius bkHalf' />"
                            + "<span class='spnRugRadius'></span>"
                            + "<input type='text' placeholder='in' onchange='CircleAreaCal(this)' class='txtRugRadiusInch bkHalf' />"
                            + "<span class='spnRugRadiusInch'></span>"

                            + "<input type='text' placeholder='ft' onchange='RectangleAreaCal(this)' class='txtRugLength bkQuarter hidden'/>"
                            + "<span class='spnRugLength hidden'></span>"
                            + "<input type='text' placeholder='in' onchange='RectangleAreaCal(this)' class='txtRugLengthInch bkQuarter hidden'/>"
                            + "<span class='spnRugLengthInch hidden'></span>"
                            + "<p class='bkQuarterX hidden'>X</p>"
                            + "<input type='text' placeholder='ft' onchange='RectangleAreaCal(this)' class='txtRugWidth bkQuarter hidden'/>"
                            + "<span class='spnRugWidth hidden'></span>"
                            + "<input type='text' placeholder='ft' onchange='RectangleAreaCal(this)' class='txtRugWidthInch bkQuarter hidden'/>"
                            + "<span class='spnRugWidthInch hidden'></span>" 
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input placeholder='ft' type='text' class='txtRugArea' />" /*style='width:50%; float:left;'*/
                            + "<span class='spnrugarea'></span>" /*style='width:50%; float:left;'*/

                            /*+ "<input style='width:50%; float:left;' placeholder='in' type='text' class='txtRugAreaInch' />"
                            + "<span  style='width:50%; float:left;' class='spnrugareaInch'></span>"*/

                        + "</td>"
                        +"<td valign='top'  class='rug-package-suggestion'>"
                                + "<input type='text' class='txtProductPackage' onkeydown='SearchKeyDown(this,event)' onclick='PackageSearchKeyUp(this, event)' readonly />"
                                + "<div class='tt-menu tt-package'> <div class='tt-dataset tt-dataset-autocomplete'> </div> </div>"
                                + "<span class='spnProductPackage'></span>"
                        +"</td>"
                        +"<td valign='top'>"
                                + "<input type='text' value='' class='txtProductInclude' readonly/>"
                                + "<span class='spnProductInclude'></span>"
                        +"</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                                + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductRate' />"
                            + "</div>"
                            /*+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductRate' />"*/
                            + "<span class='spnProductRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                                + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductDiscount' />"
                            + "</div>"
                            /*+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductDiscount' />"*/
                            + "<span class='spnProductDiscount'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                                + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
                            + "</div>"
                            /*+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"*/
                            + "<span class='spnProductAmount'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";

var NewEquipmentRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td valign='top'><input type='text'class='ProductName' onkeydown='ExtraSearchKeyDown(this,event)' onkeyup='ExtraSearchKeyUp(this,event)' />"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnProductName'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='ExtraOthersKeyDown(this,event)' class='txtProductDesc' />"
                            + "<span class='spnProductDesc'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='ExtraOthersKeyDown(this,event)' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<div class='C_S I_G'>"
                                + "<div class='input-group-prepend'>"
                                    + "<div class='input-group-text'>" + Currency + "</div>"
                                + "</div>"
                                + "<input type='text' onkeydown='ExtraOthersKeyDown(this,event)' class='txtProductRate' />"
                            + "</div>"
                            + "<span class='spnProductRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                        + "<div class='C_S I_G'>"
                        + "<div class='input-group-prepend'>"
                        + "<div class='input-group-text'>" + Currency + "</div>"
                        + "</div>"
                        + "<input type='text' onkeydown='ExtraOthersKeyDown(this,event)' class='txtProductAmount' />"
                        + "</div>"
                        //+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
                            + "<span class='spnProductAmount'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";
var CustomerSuggestiontemplate =
              "<div class='tt-suggestion tt-selectable' data-address='{0}' data-address1='{1}' data-street='{2}' data-street1='{3}' data-city='{4}' data-city1='{5}' data-state='{6}' data-state1='{7}' data-zipcode='{8}' data-zipcode1='{9}' data-Bussiness ='{10}' data-firstName='{11}' data-lastName='{12}' data-emailAddress='{13}' data-customerId='{14}' data-type='{15}' >"

                 + "<p class='tt-sug-text'>"
                     + "{16}"
                     + " <em class='tt-eq-price'>{6}</em>"
                 + "</p> "
              + "</div>";

var EstimateEqSuggestionclickbind = function (item) {
    $('.CustomerEstimateTab .tt-suggestion').click(function () {
        var clickitem = this;
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text("$" + $(this).attr('data-price'));
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
        $(spnItemRate).text("$" + $(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val($(this).attr('data-price'));

        CalculateNewBookingAmount();

        setTimeout(function () {
            $('.CustomerEstimateTab .tt-menu').hide();
        }, 100);

    });
    $('.CustomerEstimateTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var CalculateNewBookingAmount = function () {
    BookingSubTotal = 0;
    ExtraItemSubTotal = 0;
    $("#CustomerBkTab tr.HasItem .txtProductAmount").each(function () {
        var CurrentRowAmount = parseFloat($(this).val());
        if (!isNaN(CurrentRowAmount)) {
            BookingSubTotal += CurrentRowAmount;
        }
    });
    /*Booking Subtotal Set*/
    $(".booking_subtotal_text").text(Currency + BookingSubTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    $("#CustomerEstimateTab tr.HasItem .txtProductAmount").each(function () {
        var CurrentRowAmount = parseFloat($(this).val());
        if (!isNaN(CurrentRowAmount)) {
            ExtraItemSubTotal += CurrentRowAmount;
        }
    });
    /*Extra Items Subtotal Set*/
    $(".extra_item_subtotal_text").text(Currency + ExtraItemSubTotal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    TotalAmount = BookingSubTotal + ExtraItemSubTotal;
    if (isNaN(TotalAmount)) {
        TotalAmount = 0;
    }
    $(".FinalTotalTxt").text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    CalculateTax();
    TotalAmount += TotalTax;
    if (isNaN(TotalAmount)) {
        TotalAmount = 0;
    }
    $(".balanceDueAmount,.amount-big").text("$" + TotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    CalculateDiscount();
    if (typeof (BalanceDiffCalc) != "undefined") {
        BalanceDiffCalc();
    }
}
var CalculateTax = function () {
    var TaxPercentage = parseFloat($("#Booking_TaxType").val());
    if (isNaN(TaxPercentage)) {
        TaxPercentage = 0;
    }
    TotalTax = (TotalAmount * TaxPercentage) / 100;
    $(".tax_amount").text(Currency + TotalTax.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
}



var ExtraSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    var ExistEquipment = "";
    var ExistEquipmentInner = "";
    $("#CustomerEstimateTab .HasItem").each(function () {
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
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        /*7*/resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/* ImageSource*/);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                EstimateEqSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }
            if (resultparse.length == 0)
                setTimeout(function () {
                    $('.tt-menu').hide();
                },100);
        }
    });
}
var ExtraSearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
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
            $($(trselected).next('tr')).find('input.ProductName').focus();
        }
    }
    if (event.keyCode == 38) {
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
var ExtraOthersKeyDown = function (item, event) {
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

var InitRowIndex = function () {
    var i = 1;
    $(".CustomerBkTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
    var i = 1;
    $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var SearchKeyUp = function (item, event) {
    //if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
    //    return false;
    $.ajax({
        url: "/Booking/GetRugShapeListByKey",
        data: {
            key: $(item).val(),
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);
            //var resultparse1 = JSON.parse(data.result1);
            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(RugShapeSuggestiontemplate, /*0*/resultparse[i].DisplayText);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                RugShapeSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }

            if (resultparse.length == 0)
                setTimeout(function () {
                    $('.tt-menu').hide();
                }, 100);
        }
    });
}
var SearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
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
            $($(trselected).next('tr')).find('input.RugShape').focus();
        }
    }
    if (event.keyCode == 38) {
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
            $($(trselected).prev('tr')).find('input.RugShape').focus();
        }
    }
}

var OthersKeyDown = function (item, event) {
    if (event.keyCode == 40) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).next('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtRugRadius')) {
            $($(trselected).next('tr')).find('input.txtRugRadius').focus();
        }
        else if ($(event.target).hasClass('txtRugLength')) {
            $($(trselected).next('tr')).find('input.txtRugLength').focus();
        }
        else if ($(event.target).hasClass('txtRugWidth')) {
            $($(trselected).next('tr')).find('input.txtRugWidth').focus();
        }
        else if ($(event.target).hasClass('txtRugArea')) {
            $($(trselected).next('tr')).find('input.txtRugArea').focus();
        }
        else if ($(event.target).hasClass('txtProductQuantity')) {
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
        if ($(event.target).hasClass('txtRugRadius')) {
            $($(trselected).prev('tr')).find('input.txtRugRadius').focus();
        }
        else if ($(event.target).hasClass('txtRugLength')) {
            $($(trselected).next('tr')).find('input.txtRugLength').focus();
        }
        else if ($(event.target).hasClass('txtRugWidth')) {
            $($(trselected).next('tr')).find('input.txtRugWidth').focus();
        }
        else if ($(event.target).hasClass('txtRugArea')) {
            $($(trselected).next('tr')).find('input.txtRugArea').focus();
        }
        else if ($(event.target).hasClass('txtProductQuantity')) {
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
        $($(trfocuseditem).find('input.RugShape')).focus();
        event.preventDefault();
    }
}

var CircleAreaCal = function (item) {
    var parenttr = $(item).parent().parent();
    var Shape = $(parenttr).find("input.RugShape");
    /*if ($(Shape).val() == "Square") {*/
    var Length = parseFloat($($(parenttr).find(".txtRugRadius")).val());
    var LengthInch = parseFloat($($(parenttr).find(".txtRugRadiusInch")).val());
    if (isNaN(LengthInch)) {
        LengthInch = 0;
    }
    if (isNaN(Length)) {
        Length = 0;
    }
    var TotalLengthInch = (Length * 12) + LengthInch;
    var FootAreaInch = (TotalLengthInch * TotalLengthInch);
    var FootArea = ((FootAreaInch / 12) / 12);
    //FootAreaInch = (FootAreaInch % 12).toFixed(2);

    var RugArea = $(parenttr).find(".txtRugArea");
    var SpnRugArea = $(parenttr).find(".spnrugarea");
    //var RugAreaInch = $(parenttr).find(".txtRugAreaInch");
    if (isNaN(FootArea)) {
        FootArea = 0;
    }
    $(RugArea).val(FootArea.toFixed(2));
    $(SpnRugArea).text(FootArea.toFixed(2) + " sf");

    //$(RugAreaInch).val(FootAreaInch); 
    /*} else {
        var Radius = parseFloat($($(parenttr).find(".txtRugRadius")).val());
        var RadiusInch = parseFloat($($(parenttr).find(".txtRugRadiusInch")).val());
        if (isNaN(Radius)) {
            Radius = 0;
        }
        if (isNaN(RadiusInch)) {
            RadiusInch = 0;
        }

        var RadiusInch = (Radius * 12) + RadiusInch;
        RadiusInch = (PI * RadiusInch * RadiusInch) / 144;
         
        var SpnRugArea = $(parenttr).find(".spnrugarea");
        var RugArea = $(parenttr).find(".txtRugArea");
        $(RugArea).val(RadiusInch.toFixed(2));
        $(SpnRugArea).text(RadiusInch.toFixed(2) + " sf");

    }*/
    CalculateRugAreaChange(parenttr);
}

var CalculateDiscount = function () {
    TotalDiscount = 0;
    $("#CustomerBkTab .HasItem").each(function () {
        var RowDiscount = parseFloat($(this).find('input.txtProductDiscount').val());
        if (!isNaN(RowDiscount)) {
            TotalDiscount += RowDiscount;
        }
    });
    $(".TotalDiscount").text(TransCurrency + TotalDiscount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    if (TotalDiscount == 0) {
        $(".total-discount-div").addClass("hidden");
    }
    else {
        $(".total-discount-div").removeClass("hidden");
    }
}

//Package
var PackageSearchKeyUp = function (item, event) {
    //if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
    //    return false;
    $.ajax({
        url: "/Booking/GetPackageList",
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
                    searchresultstring = searchresultstring + String.format(PackageSuggestiontemplate, /*0*/resultparse[i].Id, /*1*/resultparse[i].PackageName, /*2*/resultparse[i].IncludedPack, /*3*/resultparse[i].PackageRate);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                PackageSuggestionclickbind(item);
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

var RugShapeSuggestionclickbind = function (item) {
    $('.CustomerBkTab .rug-type-suggestion .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CustomerBkTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-displaytext'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));

        var itemRugShape = $(item).parent().parent().find('.spnRugShape'); 
        if ($(item).val() == 'Rectangle/Square') { 
            $(itemRugShape).text('Rectangle / Square');
        } else { 
            $(itemRugShape).text($(item).val());
        }
        

        if ($(clickitem).attr('data-displaytext') == 'Rectangle' || $(clickitem).attr('data-displaytext') == 'Rectangle/Square'
            || $(clickitem).attr('data-displaytext') == 'Oval') {
            $(item).parent().parent().find('.txtRugLength').removeClass('hidden');
            $(item).parent().parent().find('.spnRugLength').removeClass('hidden');
            $(item).parent().parent().find('.txtRugLengthInch').removeClass('hidden');
            $(item).parent().parent().find('.spnRugLengthInch').removeClass('hidden');

            $(item).parent().parent().find('.txtRugRadius').addClass('hidden');
            $(item).parent().parent().find('.spnRugRadius').addClass('hidden');
            $(item).parent().parent().find('.txtRugWidth').removeClass('hidden');
            $(item).parent().parent().find('.spnRugWidth').removeClass('hidden');
            $(item).parent().parent().find('.txtRugRadiusInch').addClass('hidden');
            $(item).parent().parent().find('.spnRugRadiusInch').addClass('hidden');
            $(item).parent().parent().find('.txtRugWidthInch').removeClass('hidden');
            $(item).parent().parent().find('.spnRugWidthInch').removeClass('hidden');

            $(item).parent().parent().find('.bkQuarterX').removeClass('hidden');
        }
        else {
            $(item).parent().parent().find('.txtRugRadius').removeClass('hidden');
            $(item).parent().parent().find('.spnRugRadius').removeClass('hidden');
            $(item).parent().parent().find('.txtRugRadiusInch').removeClass('hidden');
            $(item).parent().parent().find('.spnRugRadiusInch').removeClass('hidden');

            $(item).parent().parent().find('.txtRugLength').addClass('hidden');
            $(item).parent().parent().find('.spnRugLength').addClass('hidden');
            $(item).parent().parent().find('.txtRugWidth').addClass('hidden');
            $(item).parent().parent().find('.spnRugWidth').addClass('hidden');
            $(item).parent().parent().find('.txtRugLengthInch').addClass('hidden');
            $(item).parent().parent().find('.spnRugLengthInch').addClass('hidden');
            $(item).parent().parent().find('.txtRugWidthInch').addClass('hidden');
            $(item).parent().parent().find('.spnRugWidthInch').addClass('hidden');

            $(item).parent().parent().find('.bkQuarterX').addClass('hidden');
        }

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
    });

    $('.CustomerBkTab .rug-type-suggestion .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var RectangleAreaCal = function (item) {

    if ($(item).val().trim() == "") {
        $(item).val(0);
    }

    var parenttr = $(item).parent().parent();
    var Length = parseFloat($($(parenttr).find(".txtRugLength")).val());
    var LengthInch = parseFloat($($(parenttr).find(".txtRugLengthInch")).val());
    var Width = parseFloat($($(parenttr).find(".txtRugWidth")).val());
    var WidthInch = parseFloat($($(parenttr).find(".txtRugWidthInch")).val());
    if (isNaN(LengthInch)) {
        LengthInch = 0;
    }
    if (isNaN(Length)) {
        Length = 0;
    }
    if (isNaN(Width)) {
        LengthInch = 0;
    }
    if (isNaN(WidthInch)) {
        WidthInch = 0;
    }
    var RugArea = $(parenttr).find(".txtRugArea");
    //var RugAreaInch = $(parenttr).find(".txtRugAreaInch");
    var SpnRugArea = $(parenttr).find(".spnrugarea");
    var SpnRugAreaInch = $(parenttr).find(".spnrugareaInch");
    var Shape = $(parenttr).find("input.RugShape");
    var TotalLengthInch = (Length * 12) + LengthInch;
    var TotalWidthInch = (Width * 12) + WidthInch;
    var FootAreaInch = (TotalLengthInch * TotalWidthInch);

    if ($(Shape).val() == "Oval") {
        FootAreaInch = (TotalLengthInch * TotalWidthInch * PI);
    }
    var FootArea = ((FootAreaInch / 12) / 12).toFixed(2);

    if (isNaN(FootArea)) {
        FootArea = 0;
    }
    $(RugArea).val(FootArea);
    $(SpnRugArea).text(FootArea + " sf");


    //$(RugAreaInch).val(FootAreaInch);

    //$(SpnRugAreaInch).text(FootAreaInch);
    CalculateRugAreaChange(parenttr);
}

var PackageSuggestionclickbind = function (item) {
    $('.CustomerBkTab .rug-package-suggestion .tt-suggestion').click(function () {
        var clickitem = this;
        $('.CustomerBkTab .rug-package-suggestion .tt-menu').hide();
        $(item).val($(clickitem).attr('data-packagename'));

        $(item).attr('data-id', $(clickitem).attr('data-id'));

        var packageitem = $(item).parent().parent().find('.spnProductPackage');
        $(packageitem).text($(item).val());


        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

        /*Include item*/
        var spnPackageInclude = $(item).parent().parent().find('.spnProductInclude');
        $(spnPackageInclude).text($(this).attr('data-package-include'));
        var txtPackageIncludeTextBox = $(item).parent().parent().find('.txtProductInclude');
        $(txtPackageIncludeTextBox).val($(this).attr('data-package-include'));

        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        //$(spnItemRate).text(($(this).attr('data-package-rate')).toFixed(2));
        //$(txtItemRate).val(($(this).attr('data-package-rate')).toFixed(2));

        var PackageRate = parseFloat($(this).attr('data-package-rate'));
        if (isNaN(PackageRate)) {
            PackageRate = 0.00;
        }

        $(spnItemRate).text(Currency + PackageRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        $(txtItemRate).val(PackageRate.toFixed(2));

        /*Item Quantity Set*/
        var defaultQuantity = 1;
        var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemRate).text(defaultQuantity);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantity');
        $(txtItemRate).val(defaultQuantity);

        /*Get Rug Area value*/
        var AreaOfRug = parseFloat($(item).parent().parent().find('.txtRugArea').val());
        /*Item Amount Set*/
        var DiscountDom = $(item).parent().parent().find('input.txtProductDiscount')
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        var DataPackageRate = parseFloat($(this).attr('data-package-rate'));
        var Discount = parseFloat($(DiscountDom).val());
        if (isNaN(Discount)) {
            Discount = 0;
        }
        var RowTotalAmount = parseFloat((DataPackageRate * AreaOfRug) - Discount);
        if (isNaN(RowTotalAmount)) {
            RowTotalAmount = 0.00;
        }
        $(spnItemRate).text(RowTotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val(RowTotalAmount);

        CalculateNewBookingAmount();

    });
    $('.CustomerBkTab .rug-package-suggestion .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.rug-package-suggestion .tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var CalculateRugAreaChange = function (ParentTr) {
    var spnProductAreaDom = $(ParentTr).find('span.spnrugarea');
    var ProductAreaDom = $(ParentTr).find('input.txtRugArea');
    $(spnProductAreaDom).text($(ProductAreaDom).val() + " sf");
    //Get Quantity
    var ProductQuantityDom = $(ParentTr).find('input.txtProductQuantity');
    //Get the rate 
    var ProductRateDom = $(ParentTr).find('input.txtProductRate');
    //Get the Discount
    var ProductDiscountDom = $(ParentTr).find('input.txtProductDiscount');

    var ProductQuantity = parseFloat($(ProductQuantityDom).val());
    var ProductRate = parseFloat($(ProductRateDom).val());
    var ProductArea = parseFloat($(ProductAreaDom).val());
    var ProductDiscount = parseFloat($(ProductDiscountDom).val());
    if (isNaN(ProductQuantity)) {
        ProductQuantity = 0;
    }
    if (isNaN(ProductRate)) {
        ProductRate = 0;
    }
    if (isNaN(ProductArea)) {
        ProductArea = 0;
    }
    if (isNaN(ProductDiscount)) {
        ProductDiscount = 0;
    }

    var ProductAmount = (ProductQuantity * ProductRate * ProductArea) - ProductDiscount;
    if (isNaN(ProductAmount)) {
        ProductAmount = 0.00;
    }
    var txtProductAmountDom = $(ParentTr).find('input.txtProductAmount');
    $(txtProductAmountDom).val(ProductAmount.toFixed(2));
    var spnProductAmountDom = $(ParentTr).find('span.spnProductAmount');
    $(spnProductAmountDom).text(Currency + ProductAmount.toFixed(2));

    CalculateNewBookingAmount();
}


$(document).ready(function () {
    $("#CustomerBkTab tbody").on('focusout', 'input.RugShape', function () {
        setTimeout(function () {
            $(".tt-menu").hide()
        }, 100);

    }); 

    //package
    $("#CustomerBkTab tbody").on('focusout', 'input.txtProductPackage', function () {
        setTimeout(function () {
            $(".tt-menu").hide()
        }, 100);
    });
    $("#CustomerBkTab tbody").on('blur', 'tr', function (item) {
        var trdom = $(item.target).parent().parent();
    });
    $("#CustomerBkTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        if ($(e.target).hasClass("spnRugShape") || $(e.target).hasClass("spnRugRadius")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount") || $(e.target).hasClass("spnProductDiscount")) {

            $("#CustomerBkTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerBkTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
    });
    $("#CustomerBkTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#CustomerBkTab tbody tr:last").after(NewRugRow);
        var i = 1;
        $(".CustomerBkTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".CustomerBkTab tbody").on('click', 'tr td i.fa-trash-o', function (e) {
        $(this).parent().parent().remove();
        var i = 1;
        if ($(".CustomerBkTab tbody tr").length < 2) {
            $("#CustomerBkTab tbody tr:last").after(NewRugRow);
        }
        $(".CustomerBkTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        CalculateNewBookingAmount();

    });

    //For Quantity change 
    $(".CustomerBkTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        $(ProductQuantityDom).text($(this).val());
        //Get the rate 
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        //Get the area 
        var ProductAreaDom = $(this).parent().parent().find('input.txtRugArea');
        //Get the Discount 
        var ProductDiscountDom = $(this).parent().parent().find('input.txtProductDiscount');

        if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {
            var ProductAmount = ($(this).val() * $(ProductRateDom).val() * $(ProductAreaDom).val()) - $(ProductDiscountDom).val();
            if (isNaN(ProductAmount)) {
                ProductAmount = 0.00;
            }
            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val(ProductAmount.toFixed(2));
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(Currency + ProductAmount.toFixed(2));
            CalculateNewBookingAmount();
        }
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtProductDiscount", function () {

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        //Get the rate 
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        //Get the area 
        var ProductAreaDom = $(this).parent().parent().parent().find('input.txtRugArea');
        //Get the Discount
        var SpnProductDiscountDom = $(this).parent().parent().find('span.spnProductDiscount');
        if ($(this).val() == "" || $(this).val() == "0") {
            $(this).val("0.00");
            $(SpnProductDiscountDom).text(Currency + "0.00");
        }
        else {
            var DiscountAmount = parseFloat($(this).val());
            $(SpnProductDiscountDom).text(Currency + DiscountAmount.toFixed(2));
        }

        if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {
            var ProductAmount = ($(ProductQuantityDom).val() * $(ProductRateDom).val() * $(ProductAreaDom).val()) - $(this).val();
            if (isNaN(ProductAmount)) {
                ProductAmount = 0.00;
            }
            var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val(ProductAmount.toFixed(2));
            var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(Currency + ProductAmount.toFixed(2));
            CalculateNewBookingAmount();
        }
        CalculateDiscount();
    });
    //For Product Amount change
    $(".CustomerBkTab tbody").on('change', "tr td .txtProductAmount", function () {

        var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        $(ProductAmountDom).text($(this).val());

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().parent().find('span.spnProductRate');

        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
            var NewProductRate = ($(this).val() / $(ProductQuantityDom).val());
            if (isNaN(NewProductRate)) {
                NewProductRate = 0;
            }
            $(ProductRateDom).val(NewProductRate);
            $(spnProductRateDom).text(Currency + NewProductRate);
        }
        CalculateNewBookingAmount();
    });
    //For Product Rate Change 
    $(".CustomerBkTab tbody").on('change', "tr td .txtProductRate", function () {
        var ProductRate = parseFloat($(this).val());
        if (isNaN(ProductRate)) {
            ProductRate = 0.00;
        }
        var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        $(ProductRateDom).text(Currency + ProductRate.toFixed(2));
        //Get the area 
        var ProductAreaDom = $(this).parent().parent().parent().find('input.txtRugArea');
        //Get the Discount
        var ProductDiscountDom = $(this).parent().parent().parent().find('input.txtProductDiscount');

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {

            var ProductAmount = ($(ProductQuantityDom).val() * $(this).val() * $(ProductAreaDom).val()) - $(ProductDiscountDom).val();
            if (isNaN(ProductAmount)) {
                ProductAmount = 0.00;
            }
            var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val(ProductAmount.toFixed(2));
            var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(Currency + ProductAmount.toFixed(2));
            CalculateNewBookingAmount();
        }
    });


    //For Width/Height/Radius Change  
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugRadius", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnRugRadius');
        $(ProductQuantityDom).text($(this).val() + "'");
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugRadiusInch", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnRugRadiusInch');
        $(ProductQuantityDom).text($(this).val() + "\"");
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugLength", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnRugLength');
        $(ProductQuantityDom).text($(this).val() + "'");
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugLengthInch", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnRugLengthInch');
        $(ProductQuantityDom).text($(this).val() + "\"");
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugWidth", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnRugWidth');
        $(ProductQuantityDom).text($(this).val() + "'");
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugWidthInch", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnRugWidthInch');
        $(ProductQuantityDom).text($(this).val() + "\"");
    });
    $(".CustomerBkTab tbody").on('change', "tr td .txtRugArea", function () {
        var spnProductAreaDom = $(this).parent().find('span.spnrugarea');
        $(spnProductAreaDom).text($(this).val() + " sqft");
        //Get Quantity
        var ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
        //Get the rate 
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        //Get the Discount
        var ProductDiscountDom = $(this).parent().parent().parent().find('input.txtProductDiscount');

        var ProductAmount = ($(ProductQuantityDom).val() * $(ProductRateDom).val() * $(this).val()) - $(ProductDiscountDom).val();
        if (isNaN(ProductAmount)) {
            ProductAmount = 0.00;
        }
        var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
        $(txtProductAmountDom).val(ProductAmount.toFixed(2));
        var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        $(spnProductAmountDom).text(Currency + ProductAmount.toFixed(2));
        CalculateNewBookingAmount();
    });
    /* End onchange Code  */


/*Extra Item Part*/
    $("#CustomerEstimateTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#CustomerEstimateTab tbody tr:last").after(NewEquipmentRow);
        var i = 1;
        $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".CustomerEstimateTab tbody").on('click', 'tr td i.fa-trash-o', function (e) {
        $(this).parent().parent().remove();
        var i = 1;
        if ($(".CustomerEstimateTab tbody tr").length < 2) {
            $("#CustomerEstimateTab tbody tr:last").after(NewEquipmentRow);
        }
        $(".CustomerEstimateTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        CalculateNewBookingAmount();
    });
    $("#CustomerEstimateTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount")) {

            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerEstimateTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }

    });
    $("#CustomerEstimateTab tbody").on( 'blur', 'tr', function (item) {
        if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined'
            && typeof ($(item.target).parent().parent().parent().attr('data-id')) == 'undefined') {
            var trdom = $(item.target).parent().parent();
            $(trdom).find("input.ProductName").val('');
            $(trdom).find("span.spnProductName").text('');

            $(trdom).find("input.txtProductDesc").val('');
            $(trdom).find("span.spnProductDesc").text('');

            $(trdom).find("input.txtProductQuantity").val('');
            $(trdom).find("span.spnProductQuantity").text('');

            $(trdom).find("input.txtProductRate").val('');
            $(trdom).find("span.spnProductRate").text('');

            $(trdom).find("input.txtProductAmount").val('');
            $(trdom).find("span.spnProductAmount").text('');
            CalculateNewBookingAmount();
        }
    });
    $("#CustomerEstimateTab tbody").on('focusout', 'input.ProductName', function () {
        setTimeout(function () {
            $(".tt-menu").hide();
        }, 100);
        var ProductNameDom = $(this).parent().find('span.spnProductName');
        $(ProductNameDom).text($(this).val());
    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProductQuantity", function () {

        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        var productQuantity = $(this).parent().parent().find('input.txtProductQuantity');
        $(ProductQuantityDom).text($(this).val());
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        if ($(productQuantity).val() > 0) {
            if ($(ProductRateDom).val() != "" && parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) > 0) {
                var NewProductAmount = $(this).val() * parseFloat($(ProductRateDom).val().trim().replaceAll(',', ''));
                if (isNaN(NewProductAmount)) {
                    NewProductAmount = 0.00;
                }
                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(txtProductAmountDom).val(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductAmountDom).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewBookingAmount();
            }
        }
        else {
            $(productQuantity).val("1");
            $(ProductQuantityDom).val("1");

            if ($(ProductRateDom).val() != "" && parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) > 0) {
                var NewProductAmount = $(this).val() * parseFloat($(ProductRateDom).val().trim().replaceAll(',', ''));
                if (isNaN(NewProductAmount)) {
                    NewProductAmount = 0.00;
                }
                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(txtProductAmountDom).val(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductAmountDom).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewBookingAmount();
            }
        }
    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductAmount = $(this).parent().parent().find('input.txtProductAmount');

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().parent().find('span.spnProductRate');
        if ($(ProductAmount).val() != "" && parseFloat($(ProductAmount).val().trim().replaceAll(',', '')) >= 0) {
            var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(ProductAmountDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
                if (isNaN(NewProductRate)) {
                    NewProductRate = 0.00;
                }
                $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductRateDom).text("$" + NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
            CalculateNewBookingAmount();
        }
        else {
            var CalculateAmount = parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();
            if (isNaN(CalculateAmount)) {
                CalculateAmount = 0.00;
            }
            $(ProductAmount).val(CalculateAmount);
            var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(ProductAmountDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
                if (isNaN(NewProductRate)) {
                    NewProductRate = 0.00;
                }
                $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductRateDom).text("$" + NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
            CalculateNewBookingAmount();
        }

    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProductRate", function () {

        /*
         *If product rate changes make change to amount.
         */
        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRate = $(this).parent().parent().find('input.txtProductRate');
        if (isNaN(ProductRate)) {
            ProductRate = 0.00;
        }
        var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
        if ($(ProductRate).val() != "" && parseFloat($(ProductRate).val().trim().replaceAll(',', '')) > 0) {
            var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
            $(ProductRateDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();
                if (isNaN(ProductAmount)) {
                    ProductAmount = 0.00;
                }
                var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text("$" + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewBookingAmount();
            }
        }
        else {

            var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
            var CalculateRate = parseFloat($(txtProductAmountDom).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val();
            if (isNaN(CalculateRate)) {
                CalculateRate = 0.00;
            }
            $(ProductRate).val(CalculateRate);
            var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
            $(ProductRateDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();
                if (isNaN(ProductAmount)) {
                    ProductAmount = 0.00;
                }
                var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text("$" + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewBookingAmount();
            }
        }

    });
    $(".CustomerEstimateTab tbody").on('change', "tr td .txtProductDesc", function () {

        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });
    /*
        *
        *
        * Extra Item Part Ends
        *
        *
    */


});
