String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

var PropertyLeadtemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                   //+ "<img src='{7}' class='EquipmentImage'>"
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'>{5}</em>{1}" + "<br />"
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
                            + "<input type='text' class='txtProductRate' />"
                            + "<span class='spnProductRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtProductDiscountRate' />"
                            + "<span class='spnProductDiscountRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtTotalAmount' />"
                            + "<span class='spnTotalAmount'></span>"
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

        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text($(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        $(txtItemRate).val($(this).attr('data-price'));

        /*Item ProductDiscountRate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductDiscountRate');
        $(spnItemRate).text(0);
        var txtItemRate = $(item).parent().parent().find('.txtProductDiscountRate');
        $(txtItemRate).val(0);

        /*Total Amount Set*/
        var spnItemRate = $(item).parent().parent().find('.spnTotalAmount');
        $(spnItemRate).text($(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtTotalAmount');
        $(txtItemRate).val($(this).attr('data-price'));

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
    $.ajax({
        url: domainurl + "/Invoice/GetOnlyServiceListByKey",
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
                    searchresultstring = searchresultstring + String.format(PropertyLeadtemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\'')/* ImageSource*/);
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

var RemoveLeadEquipment = function (id) {
    var url = domainurl + "/SmartLeads/DeleteCustomerAppointmentService";
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
                OpenErrorMessageNew("", data.message, function () {
                    $(".Equipment_" + id).remove();
                    LeadEquipmetnPriceCalculation();
                });
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

$(document).ready(function () {
    InitRowIndex();
    $("#LeadEquipmentTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#LeadEquipmentTab tr").removeClass("focusedItem");
        $(this).addClass("focusedItem");
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
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductDiscountRate", function () {
        var ProductDiscountRateDom = $(this).parent().find('span.spnProductDiscountRate');
        $(ProductDiscountRateDom).text($(this).val());

        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        if ($(this).val() != "" && $(this).val() > 0) {
            var ProductYouAmountDom = $(this).parent().parent().find('input.txtTotalAmount');
            $(ProductYouAmountDom).val($(ProductRateDom).val() - $(this).val());
            var spnProductYouAmountDom = $(this).parent().parent().find('span.spnTotalAmount');
            $(spnProductYouAmountDom).text($(ProductRateDom).val() - $(this).val());
        }
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductRate", function () {
        var ProductRateDom = $(this).parent().find('span.spnProductRate');
        $(ProductRateDom).text($(this).val());

        var ProductDiscountDom = $(this).parent().parent().find('input.txtProductDiscountRate');
        if ($(this).val() != "" && $(this).val() > 0) {
            var ProductYouAmountDom = $(this).parent().parent().find('input.txtTotalAmount');
            $(ProductYouAmountDom).val($(this).val() - $(ProductDiscountDom).val());
            var spnProductYouAmountDom = $(this).parent().parent().find('span.spnTotalAmount');
            $(spnProductYouAmountDom).text($(this).val() - $(ProductDiscountDom).val());
        }
    });

    $(".LeadEquipmentTab tbody").on('change', "tr td .txtTotalAmount", function () {
        //var spanYourAmount = $(this).parent().find('span.spnYourAmount');
        var ProductYouAmountDom = $(this).parent().find('span.spnTotalAmount');
        $(ProductYouAmountDom).text($(this).val());
    });

    $(".existing-equipments .tableActions .fa-trash-o").click(function () {
        var id = $(this).attr('dataid');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this?", function () {
            RemoveLeadEquipment(id);
        });
    });
});
