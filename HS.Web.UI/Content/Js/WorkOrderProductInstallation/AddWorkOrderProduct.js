var TotalAmount = 0;
var SubTotalAmount = 0;
var TotalTaxAmount = 0;
var InvoiceDatepicker;
var DueDatepicker;
var d = new Date();

var month = d.getMonth()+1;
var day = d.getDate();

var output = (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day + '/' + d.getFullYear();
var start;
var end;
var Fstart;
var Fend;
var Amount;
var SubTotal;
var TaxTotal;
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
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        resultparse[i].ManufacturerName/* ImageSource*/);
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
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
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
            if ($(event.target).hasClass('ProductName')) {
                $($(trselected).next('tr')).find('input.ProductName').focus();
            }
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

    $(".txtProductAmount").each(function () {
        var currAmount = parseFloat($(this).val().trim());
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
    if (tp != "" && tp != "0" && tp!=null) {
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

var SaveWorkOrderEquipmentDetails = function (SendEmail, CreatePdf, CameFrom) {
        var WorkOrderStartTime = $("#WorkOrderStartTime").val();
        var WorkOrderEndTime = $("#WorkOrderEndTime").val();
        var WorkOrderEmployeeId = $(".WorkOrderEmployeeId").val();
        var WorkOrderNote = $("#WorkOrderNote").val();
        //var WorkOrderDate = $("#WorkOrderDate").val();


        var DetailList = [];
        $(".HasItem").each(function () {
            DetailList.push({
                EquipmentId: $(this).attr('data-id'),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find('.txtProductRate').val(),
                TotalPrice: $(this).find('.txtProductQuantity').val() * $(this).find('.txtProductRate').val(),
                EquipmentName: $(this).find('.ProductName').val()
            });
        });

        var url = domainurl + "/WorkOrder/AddCustomerAppointmentDetailWorkOrder/";
        var param = JSON.stringify({
            CustomerAppointmentEquipmentList: DetailList,
            AppointmentIdForAddProduct: AppointmentIdForAddProduct,
            WorkOrderEmployeeId: WorkOrderEmployeeId,
            WorkOrderDate: $("#WorkOrderDate").val(),
            WorkOrderStartTime: WorkOrderStartTime,
            WorkOrderEndTime: WorkOrderEndTime,
            WorkOrderNote: WorkOrderNote,
            ServiceOrderTaxPercent: $("#CustomerAppointment_TaxPercent").val(),
            WorkOrderTaxType: $("#CustomerAppointment_TaxPercent option:selected").text(),
            ServiceOrderTaxTotal: TaxTotal,
            ServiceOrderTotalAmount: SubTotal,
            ServiceOrderTotalAmountTax: Amount,
            ListHelperTech: $("#ListHelperTech").val()
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
                if (data.result == true && data.message1 == "") {
                    CloseTopToBottomModal();
                    setTimeout(function () {
                        OpenSuccessMessageNew("Success!", "Work order successfully done.", "");
                        $("#WorkOrderTab").load(domainurl + "/WorkOrder/WorkOrderPartial?customerid=" + data.cusid);
                    }, 300);
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
var SaveInstall = function () {
    var url = "/CustomerAppoinmentInstallationDetail/AddDetailInstallType";
    //console.log('cid' + $("#CustomerIdVal").val());
    console.log($("App Id" + "#AppointmentId").val());
    console.log($("#InstallType").val());

    var param = JSON.stringify({
        Id: $("#Id").val(),
        AppointmentId: $("#AppointmentId").val(),
        InstallType: $("#InstallType").val(),
        CollectedAmount: $("#CollectedAmount").val()
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
            if ($("#WorkOrderDate").val() != "" && ($("#WorkOrderStartTime").val() < $("#WorkOrderEndTime").val())){
                SaveWorkOrderEquipmentDetails();
            }
            else {
                OpenErrorMessageNew("Error!", "Appointment date couldn't be empty and end time should be greater than start time");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}
$(document).ready(function () {
    //parent.$('.close').click(function () {
    //    parent.$(".modal-body").html('');
    //})
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".WorkOrderPreview", type: 'iframe', width: Popupwidth, height: 600 }
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
    /*Remove Row*/
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
        if ($(ProductRateDom).val() != "" && $(ProductRateDom).val() > 0) {
            var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val($(this).val() * $(ProductRateDom).val());
            var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(($(this).val() * $(ProductRateDom).val()));
            CalculateNewAmount();
        } 
    });
    /*Product Amount Change*/
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
    /*Product Rate Change*/
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
    $("#WorkOrderStartTime").change(function () {
        start = $("#WorkOrderStartTime").val();
        console.log(start);
    })
    $("#WorkOrderEndTime").change(function () {
        end = $("#WorkOrderEndTime").val();
        console.log(end);
    })
    Fstart = $(".AddPickAday1").val() + " " + start;
    Fend = $(".AddPickAday1").val() + " " + end;
    $(".InstallerSaveButton").click(function () {
        var amount = $(".work-order-type").find('input').eq(2).val();
        if ($(".HasItem").length == 0) {
            OpenErrorMessageNew("Error!", "You have to select All Fields to proceed", "");
        }
        //else if ($(".AddPickAday1").val() <= output) {
            //    OpenErrorMessageNew("Error!", "Appointment date couldn't be todays date", "");
        //}
        //else if (Fstart >= Fend) {
        //    OpenConfirmationMessage("Error!", "Appointment start time not greater or equal to Appointment end", "");
        //}
        else if ($(".work-order-type").find('input').eq(2).val() == "") {
            $(".work-order-type").find('input').eq(2).css("border-color", "red");
        }
        else {
            SaveInstall();
        } 
    });
    
    $("#CustomerAppointment_TaxPercent").change(function () {
        CalculateNewAmount();
    });
    CalculateNewAmount();
});
