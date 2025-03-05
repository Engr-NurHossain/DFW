var TotalAmount = 0;
var InvoiceDatepicker;
var DueDatepicker;


var PropertyUserSuggestiontemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                   + "<img src='{7}' class='EquipmentImage'>"
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'>{5}</em>{1}"
                       + "<em class='tt-eq-price'>${2}</em>"
                   + "</p> "
                + "</div>";
var AccountTypeSuggestiontemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-id="{0}">'
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'></em>{1}"
                       + "<em class='tt-eq-price'>{2}</em>"
                   + "</p> "
                + "</div>";
var NewEquipmentRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td valign='top'><input type='text'class='ProductName' onkeydown='SearchAccountKeyDown(this,event)' onkeyup='SearchAccountKeyUp(this,event)' />"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnProductName'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtProductDesc' />"
                            + "<span class='spnAccountDesc'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtProductAmount' />"
                            + "<span class='spnAccountAmount'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";

//var SaveAndNew = function () {
//    SaveInvoice(false, false, "others");
//    CloseTopToBottomModal();
//    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () {
//        OpenInvoiceTab();
//        OpenTopToBottomModal("/Invoice/AddInvoice/?customerid=" + customerId);
//    });
//}
//var SaveAndClose = function () {
//    SaveInvoice(false, false, "others");
//    CloseTopToBottomModal();
//    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () { OpenInvoiceTab() });
//}
//var SaveAndShare = function () {
//    SaveInvoice(false, true);
//}
//var SaveAndSend = function () {
//    SaveInvoice(true, false, "others");
//    CloseTopToBottomModal();
//    OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved and Send to Customer.", function () { OpenInvoiceTab() });
//}


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
        //var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        //$(spnItemRate).text($(this).attr('data-price'));
        //var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        //$(txtItemRate).val($(this).attr('data-price'));
        //var spnItemRate = $(item).parent().parent().find('.spnProductDesc');
        //$(spnItemRate).text($(this).attr('data-description'));
        //var txtItemRate = $(item).parent().parent().find('.txtProductDesc');
        //$(txtItemRate).val($(this).attr('data-description'));
        //var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        //$(spnItemRate).text(1);
        //var txtItemRate = $(item).parent().parent().find('.txtProductQuantity');
        //$(txtItemRate).val(1);
        //var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        //$(spnItemRate).text($(this).attr('data-price'));
        //var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        //$(txtItemRate).val($(this).attr('data-price'));

        CalculateNewAmount();

    });
    $('.CustomerInvoiceTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var SearchAccountKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Billing/GetAccountTypeListByKey",
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
                    searchresultstring = searchresultstring + String.format(AccountTypeSuggestiontemplate,
                        /*0*/resultparse[i].Id,
                        /*1*/ resultparse[i].Name.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].Type);
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
var CalculateNewAmount = function () {
    var amount = parseFloat('0');
    $(".txtProductAmount").each(function () {
        var currAmount = parseFloat($(this).val().trim());
        if (!isNaN(currAmount)) {
            amount += currAmount;
        }
    });
    amount = parseFloat(amount).toFixed(2);
    $(".amount").text("$" + amount);
    TotalAmount = amount;
}
var InitRowIndex = function () {
    var i = 1;
    $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
//var SaveInvoice = function (SendEmail, CreatePdf, CameFrom) {
//    console.log("ok");
//    if (typeof (SendEmail) == "undefined") {
//        SendEmail = false;
//    }
//    if (typeof (CreatePdf) == "undefined") {
//        CreatePdf = false;
//    }
//    if (typeof (CameFrom) == "undefined") {
//        CameFrom = "";
//    }
//    var DetailList = [];
//    $(".HasItem").each(function () {
//        DetailList.push({
//            EquipmentId: $(this).attr('data-id'),
//            Quantity: $(this).find('.txtProductQuantity').val(),
//            UnitPrice: $(this).find('.txtProductRate').val(),
//            TotalPrice: $(this).find('.txtProductQuantity').val() * $(this).find('.txtProductRate').val(),
//            EquipmentDescription: $(this).find('.txtProductDesc').val(),
//            EquipmentName: $(this).find('.ProductName').val(),
//            InventoryId: '00000000-0000-0000-0000-000000000000',
//            CreatedDate: '1-1-2017',
//            InvoiceId: InvoiceId
//        });
//    });
//    var url = "/Invoice/AddInvoice";
//    var param = JSON.stringify({
//        EmailAddress: $("#EmailAddress").val(),
//        "Invoice.InvoiceId": InvoiceId,
//        "Invoice.BillingAddress": $("#Invoice_BillingAddress").val(),
//        "Invoice.CustomerId": $("#CustomerList").val(),
//        "Invoice.CreatedDate": InvoiceDatepicker.getDate(),
//        "Invoice.DueDate": DueDatepicker.getDate(),
//        "Invoice.TotalAmount": TotalAmount,
//        "Invoice.InvoiceMessage": $("textarea#InvoiceMessage").val(),
//        InvoiceDetailList: DetailList,
//        SendEmail: SendEmail,
//        CreatePdf: CreatePdf
//    });

//    $.ajax({
//        type: "POST",
//        ajaxStart: $(".loader-div").show(),
//        url: url,
//        data: param,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        cache: false,
//        success: function (data) {
//            console.log(data);
//            if (data.result && CameFrom != "others") {
//                OpenConfirmationMessage("Invoice Saved", "Invoice Successfully Saved", function () { OpenInvoiceTab() });
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            console.log(errorThrown);
//        }
//    })

//}

var SaveAddCustomerBilling = function () {
    var DetailList = [];
    $(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val(),
            TotalPrice: $(this).find('.txtProductQuantity').val() * $(this).find('.txtProductRate').val(),
            EquipmentDescription: $(this).find('.txtProductDesc').val(),
            EquipmentName: $(this).find('.ProductName').val(),
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
            InvoiceId: InvoiceId
        });
    });
    var url = domainurl + "/Billing/AddBilling";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        BillNo: $("#BillNo").val(),
        CompanyId: $("#CompanyId").val(),
        CustomerId: customerID,
        Type: DetailList,
        Amount: DetailList,
        PaymentMethod: $("#PaymentMethod").val(),
        PaymentStatus: $("#PaymentStatus").val(),
        PaymentDate: paymentdate.getDate(),
        PaymentDueDate: duedate.getDate(),
        BillCycle: $("#BillCycle").val(),
        Notes: DetailList
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
                var cusId = $("#CustomerIdVal").val();
                $("#BillingTab").load(domainurl + "/Billing/BillingPartial/?customerid=" + cusId);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}

$(document).ready(function () {
    $(".BillSaveButton").click(function () {
        SaveAddCustomerBilling();
    })
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
    //InvoiceDatepicker = new Pikaday({
    //    field: $('#Invoice_CreatedDate')[0],
    //    format: 'MM/DD/YYYY'
    //});
    //DueDatepicker = new Pikaday({
    //    field: $('#Invoice_DueDate')[0],
    //    format: 'MM/DD/YYYY'
    //});
    InitRowIndex();

    $("#CustomerList").change(function () {
        var selectedEmail = $(this).find(":selected").attr("data-EmailAddress");

        $("#EmailAddress").val(selectedEmail);
        var Address = $(this).find(":selected").attr("data-Address") + ",";
        var Street = $(this).find(":selected").attr("data-Street") + ",";
        var City = $(this).find(":selected").attr("data-City") + ",";
        var State = $(this).find(":selected").attr("data-State") + ",";
        var ZipCode = $(this).find(":selected").attr("data-Zipcode") + ",";
        var Country = $(this).find(":selected").attr("data-Country");

        var selectedAddress = "";
        if (Address != ",")
            selectedAddress += Address;
        if (Street != ",")
            selectedAddress += Street;
        if (City != ",")
            selectedAddress += City;
        if (State != ",")
            selectedAddress += State;
        if (ZipCode != ",")
            selectedAddress += ZipCode;
        if (Country != ",")
            selectedAddress += Country;

        $("#Invoice_BillingAddress").val(selectedAddress);

    });

    $("#CustomerInvoiceTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#CustomerInvoiceTab tr").removeClass("focusedItem");
        $(this).addClass("focusedItem");

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

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        $(ProductQuantityDom).text($(this).val());
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {
            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val($(this).val() * $(ProductRateDom).val());
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(($(this).val() * $(ProductRateDom).val()));
            CalculateNewAmount();
        }

    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductAmount", function () {

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
        CalculateNewAmount();
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductRate", function () {

        var ProductRateDom = $(this).parent().find('span.spnProductRate');
        $(ProductRateDom).text($(this).val());

        var ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {

            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val($(this).val() * $(ProductQuantityDom).val());
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(($(this).val() * $(ProductQuantityDom).val()));
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
            SaveInvoice();
        }

    });
    /*$("select.dropdown-search").select2();*/
    CalculateNewAmount();
});
