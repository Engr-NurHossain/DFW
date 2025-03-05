var TotalAmount = 0;
var SubTotalAmount = 0;
var TotalTaxAmount = 0;
var InvoiceDatepicker;
var DueDatepicker;
var ServiceOrderDate;
var Amount;
var SubTotal;
var TaxTotal;
var DetailList = [];
var counter = 0;
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var PropertyUserSuggestiontemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                   //+ "<img src='{7}' class='EquipmentImage'>"
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{7}</em>"
                       + "<em class='tt-eq-price'>${2}</em>"
                       + "<br />"
                   + "</p> "
                + "</div>";
var NewEquipmentRow = "<tr>"
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
                            + "<input type='number' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductRate' />"
                            + "<span class='spnProductRate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
                            + "<span class='spnProductAmount'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";

 
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

        CalculateNewAmount();

    });
    $('.CustomerInvoiceTab .tt-suggestion').hover(function () {
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
                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/* ImageSource*/);
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
    var subTotal = parseFloat('0');
    var taxTotal = parseFloat('0');

    $(".txtProductAmount").each(function (e,f) {
        //console.log(e);
        //var currAmountStr = parseFloat($(this).val().trim()); 
        //currAmountStr = currAmountStr.replaceAll(',', '');

        var _CalAmt = $(this).val().trim();
        _CalAmt = _CalAmt.replaceAll(',', '');

        var currAmount = parseFloat(_CalAmt);
        if (!isNaN(currAmount)) {
            amount += currAmount;
            SubTotal = amount;
        }
    });
    amount = parseFloat(amount).toFixed(2);
    amount1 = amount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    $(".amount").text(TransCurrency + amount1);
    $(".total").text(TransCurrency + amount1);
    var tp = $("#CustomerAppointment_TaxPercent").val();
    if (tp != "" && tp != "0" && tp != null) {
        $(".tax-percent").text(tp + "%");
        var ta = (amount) * (tp / 100);
        TaxTotal = ta.toFixed(2);
        $(".tax-amount").text(TransCurrency + ta.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        amount = parseFloat(amount) + parseFloat(ta);
        amount2 = amount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
        $(".total").text(TransCurrency + amount2);
    }
    if (tp == "0") {
        $(".tax-percent").text(0 + "%");
        $(".tax-amount").text(TransCurrency + "0.00");
        TaxTotal = 0.0;
    }
    TotalAmount = amount;
    Amount = amount;
}


var InitRowIndex = function () {
    var i = 1;
    $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var SaveInvoice = function (SendEmail, CreatePdf, CameFrom) {
    console.log("ok");
    if (typeof (SendEmail) == "undefined") {
        SendEmail = false;
    }
    if (typeof (CreatePdf) == "undefined") {
        CreatePdf = false;
    }
    if (typeof (CameFrom) == "undefined") {
        CameFrom = "";
    }
    var DetailList = [];
    $(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val(),
            TotalPrice: $(this).find('.txtProductQuantity').val() * $(this).find('.txtProductRate').val(),
            //EquipmentDescription: $(this).find('.txtProductDesc').val(),
            EquipmentName: $(this).find('.ProductName').val(),
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017'
            //InvoiceId: InvoiceId
        });
    });
    var url = domainurl + "/Invoice/AddInvoice";
    var param = JSON.stringify({
        EmailAddress: $("#EmailAddress").val(),
        "Invoice.InvoiceId": InvoiceId,
        "Invoice.BillingAddress": $("#Invoice_BillingAddress").val(),
        "Invoice.CustomerId": $("#CustomerList").val(),
        "Invoice.CreatedDate": InvoiceDatepicker.getDate(),
        "Invoice.DueDate": DueDatepicker.getDate(),
        "Invoice.TotalAmount": TotalAmount,
        "Invoice.InvoiceMessage": $("textarea#InvoiceMessage").val(),
        InvoiceDetailList: DetailList,
        SendEmail: SendEmail,
        CreatePdf: CreatePdf,
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
            console.log(data);
            if (data.result && CameFrom != "others") {
                OpenSuccessMessageNew("Success!", "Invoice Successfully Saved", function () { OpenInvoiceTab() });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}

var SaveServiceOrderEquipmentDetails = function () {
    $(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val(),
            TotalPrice: $(this).find('.txtProductQuantity').val() * $(this).find('.txtProductRate').val(),
            EquipmentName: $(this).find('.ProductName').val(), 
        });
    });

    var url = domainurl + "/ServiceOrder/AddCustomerAppointmentDetail";
    var param = JSON.stringify({
        "AddCustomerAppointment.CustomerAppointmentEquipmentList": DetailList,
        //CreatePdf: CreatePdf,
        AppointmentIdForAddProduct: AppointmentIdForAddProduct,
        ServiceOrderEmployeeId: $("#ServiceOrderEmployeeType").val(),
        ServiceOrderDate: $("#ServiceOrderDate").val(),
        ServiceOrderAppointmentStartTime : $("#ServiceOrderAppointmentStartTime").val(),
        ServiceOrderAppointmentEndTime : $("#ServiceOrderAppointmentEndTime").val(),
        ServiceOrderNote: $("#ServiceOrderNote").val(),
        ServiceOrderTaxPercent: $("#CustomerAppointment_TaxPercent").val(),
        ServiceOrderTaxType: $("#CustomerAppointment_TaxPercent option:selected").text(),
        ServiceOrderTaxTotal: TaxTotal,
        ServiceOrderTotalAmount: SubTotal,
        ServiceOrderTotalAmountTax: Amount,
        IsComplete: IsComplete,
        ListHelperTech: $("#ListHelperTech").val()
    });
    console.log(ServiceOrderDate);
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true && data.MakeComplete == false) {
                OpenSuccessMessageNew("Success!", "Service order successfully done.");
                setTimeout(function () {
                    CloseTopToBottomModal();
                    $("#ServiceOrderTab").load(domainurl + "/ServiceOrder/ServiceOrderPartial/?customerid=" + data.cusID);
                }, 1000);
            }
            else if (data.result == true && data.MakeComplete == true) {
                OpenSuccessMessageNew("Success!", "Your service order task is completed.");
                setTimeout(function () {
                    CloseTopToBottomModal();
                    $("#ServiceOrderTab").load(domainurl + "/ServiceOrder/ServiceOrderPartial/?customerid=" + data.cusID);
                }, 1000);
            }
            else {
                OpenErrorMessageNew("Error!", data.message1, "");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}

$(document).ready(function () {
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
        field: $('#Invoice_CreatedDate')[0],
        format: 'MM/DD/YYYY'
    });
    DueDatepicker = new Pikaday({
        field: $('#Invoice_DueDate')[0],
        format: 'MM/DD/YYYY'
    });
    ServiceOrderDate = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#ServiceOrderDate')[0]
    });
    InitRowIndex();
    /*Equiepment menu hide on body click*/
    $("#CustomerInvoiceTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide()
    });
    /*Row Click*/
    $("#CustomerInvoiceTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        //console.log(e.target);

        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount")) {

            $("#CustomerInvoiceTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }

        else {
            $("#CustomerInvoiceTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
    });

    $("#CustomerInvoiceTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
        var i = 1;
        $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $("#CustomerInvoiceTab tbody").on('blur', 'tr', function (item) { 
        if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined') {
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
            CalculateNewAmount();
        }
    });
    $(".CustomerInvoiceTab tbody").on('click', 'tr td i.fa', function (e) {
        $(this).parent().parent().remove();
        if ($(".CustomerInvoiceTab tbody tr").length < 2) {
            $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
        }
        var i = 1;
        $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        CalculateNewAmount();
    });
    /*Product Quantity Change*/
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        $(ProductQuantityDom).text($(this).val());
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        if ($(ProductRateDom).val() != "" && parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) > 0) {
            var NewProductAmount = $(this).val() * parseFloat($(ProductRateDom).val().trim().replaceAll(',', ''));
            console.log(NewProductAmount);
            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(txtProductAmountDom).val(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(spnProductAmountDom).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            CalculateNewAmount();
        }

    });
    /*Product Amount Change*/
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductAmount", function () {

        var ProductAmountDom = $(this).parent().find('span.spnProductAmount');
        $(ProductAmountDom).text(parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
            var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
            $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(spnProductRateDom).text(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        CalculateNewAmount();
    });
    /*Product Rate Change*/
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductRate", function () {
        /*
        *If product rate changes make change to amount.
        */
        var ProductRateDom = $(this).parent().find('span.spnProductRate');
        $(ProductRateDom).text(parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

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
    
    $(".InvoiceSaveButton").click(function () {
        counter = counter + 1;
        SaveServiceOrderEquipmentDetails();
    });

    $(".InvoiceCancelButton").click(function () {
        //OpenServiceOrderTab();
        CloseTopToBottomModal();
    });

    /*$("select.dropdown-search").select2();*/
    $("#CustomerAppointment_TaxPercent").change(function () {
        CalculateNewAmount();
    });
    CalculateNewAmount();
});
