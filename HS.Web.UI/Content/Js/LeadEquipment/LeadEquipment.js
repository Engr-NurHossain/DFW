String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

var PropertyLeadtemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                   //+ "<img src='{7}' class='EquipmentImage'>"
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{7}</em>"
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
                        //+ "<td valign='top'>"
                        //    + "<input type='text' class='txtProductDesc' />"
                        //    + "<span class='spnProductDesc'></span>"
                        //+ "</td>"
                        + "<td valign='top'>"
                            + "<input type='number' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtProductRate' />"
                            + "<span class='spnProductRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtProductAmount' />"
                            + "<span class='spnProductAmount'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtYourAmount' />"
                            + "<span class='spnYourAmount'></span>"
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
        /*Item Description Set*/
        //var spnItemRate = $(item).parent().parent().find('.spnProductDesc');
        //$(spnItemRate).text($(this).attr('data-description'));
        //var txtItemRate = $(item).parent().parent().find('.txtProductDesc');
        //$(txtItemRate).val($(this).attr('data-description'));
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
        /*Your Amount Set*/
        var spnItemRate = $(item).parent().parent().find('.spnYourAmount');
        $(spnItemRate).text($(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtYourAmount');
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
                    searchresultstring = searchresultstring + String.format(PropertyLeadtemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
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

var RemoveLeadEquipment = function (id,pdcid, appoinmentId, equipmentId) {
    var url = domainurl + "/Leads/DeleteCustomerAppointmentEquipment";
    var Param = JSON.stringify({
        id: id,
        PDCId: pdcid,
        AppoinmentId: appoinmentId,
        EquipmentId: equipmentId
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
                OpenSuccessMessageNew('Success!', data.message, function () {
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
        totalRetailPrice +=  parseFloat($(this).text().trim().replaceAll(',', ''));
    });
    $(".Setup-Equipments-Table .total_price").each(function () {
        totalPrice += parseFloat($(this).text().trim().replaceAll(',', ''));
    });
    
    $(".BottomTotalTr .retail-total").text('$'+totalRetailPrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
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
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
       
        if ($(this).val() > 0)
        {
            $(ProductQuantityDom).text($(this).val());
            var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
            if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {
                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val($(this).val() * $(ProductRateDom).val());
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text(($(this).val() * $(ProductRateDom).val()));

                var ProductYouAmountDom = $(this).parent().parent().find('input.txtYourAmount');
                $(ProductYouAmountDom).val($(this).val() * $(ProductRateDom).val());
                var spnProductYouAmountDom = $(this).parent().parent().find('span.spnYourAmount');
                $(spnProductYouAmountDom).text(($(this).val() * $(ProductRateDom).val()));
            }
        }
        else
        {
            $(this).val("1");
            $(ProductQuantityDom).text("1");
            var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
            if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {
                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val($(this).val() * $(ProductRateDom).val());
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text(($(this).val() * $(ProductRateDom).val()));

                var ProductYouAmountDom = $(this).parent().parent().find('input.txtYourAmount');
                $(ProductYouAmountDom).val($(this).val() * $(ProductRateDom).val());
                var spnProductYouAmountDom = $(this).parent().parent().find('span.spnYourAmount');
                $(spnProductYouAmountDom).text(($(this).val() * $(ProductRateDom).val()));
            }
        }
       

    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductAmountDom = $(this).parent().find('span.spnProductAmount');
     
        $(ProductAmountDom).text($(this).val());
        var ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
            var NewProductRate = ($(this).val() / $(ProductQuantityDom).val());
            $(ProductRateDom).val(NewProductRate);
            $(spnProductRateDom).text(NewProductRate);
        }
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtProductRate", function () {
        var ProductRateDom = $(this).parent().find('span.spnProductRate');
        var ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
    
        if ($(this).val() > 0)
        {
            $(ProductRateDom).text($(this).val());
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {

                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val($(this).val() * $(ProductQuantityDom).val());
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text(($(this).val() * $(ProductQuantityDom).val()));

                var ProductYouAmountDom = $(this).parent().parent().find('input.txtYourAmount');
                $(ProductYouAmountDom).val($(this).val() * $(ProductQuantityDom).val());
                var spnProductYouAmountDom = $(this).parent().parent().find('span.spnYourAmount');
                $(spnProductYouAmountDom).text(($(this).val() * $(ProductQuantityDom).val()));
            }
        }
        else {
            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            var Rate = $(txtProductAmountDom).val() / $(ProductQuantityDom).val();
            $(this).val(Rate);

            $(txtProductAmountDom).val($(this).val() * $(ProductQuantityDom).val());
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(($(this).val() * $(ProductQuantityDom).val()));

            var ProductYouAmountDom = $(this).parent().parent().find('input.txtYourAmount');
            $(ProductYouAmountDom).val($(this).val() * $(ProductQuantityDom).val());
            var spnProductYouAmountDom = $(this).parent().parent().find('span.spnYourAmount');
            $(spnProductYouAmountDom).text(($(this).val() * $(ProductQuantityDom).val()));
        }
       
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtYourAmount", function () {
        //var spanYourAmount = $(this).parent().find('span.spnYourAmount');
        var ProductYouAmountDom = $(this).parent().find('span.spnYourAmount');
        $(ProductYouAmountDom).text($(this).val());
    });

    $(".existing-equipments .tableActions .fa-trash-o").click(function () {
        var id = $(this).attr('dataid');
        var appoinmentId = $(this).attr('appoinmentId');
        var equipmentId = $(this).attr('equipmentId');
        var pdcid = $(this).attr('pdc-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this?", function () {
            RemoveLeadEquipment(id,pdcid,appoinmentId,equipmentId);
        });
    });
});
