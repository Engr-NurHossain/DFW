var VendorSuggestiontemplate =
            "<div class='tt-suggestion tt-selectable' data-street = '{0}' data-city = '{1}' data-state = '{2}' data-zip = '{3}' data-name = '{4}' data-vendorid = '{5}' data-companyname = '{6}' data-phone='{7}' data-EmailAddress='{8}' data-SupplierId='{9}' data-SalesRepName='{10}' >"
               + "<p class='tt-sug-text'>"
                   + "{6}"
               + "</p> "
            + "</div>";
var VendorAddressTemplate = "<p>"
                                + "<strong>{0}</strong>"
                                + "<br />"
                                + "<span>In Care of: {7}</span>"
                                + "<br />"
                                + "<span>{1}</span>"
                                + "<br />"
                                + "<span>{2}, {3} {4}</span>"
                                + "<br />"
                                + "<span>Phone: {5}</span>"
                                + "<br />"
                                + "<span>Email: {6}</span>"
                            + "</p>";
var PropertyUserSuggestiontemplate =
            '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
               /*
               *For Equipment Image
               *+ "<img src='{7}' class='EquipmentImage'>"*/
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
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductDesc' />"
                            + "<span class='spnProductDesc'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>";
if (Receiving) {
    NewEquipmentRow += "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantityReceived' />"
                            + "<span class='spnProductQuantityReceived'></span>"
                        + "</td>";
}
NewEquipmentRow += "<td valign='top' class='tableActions'>"
                    + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                + "</td>"
            + "</tr>";

var POEqSuggestionclickbind = function (item) {
    $('.CustomerInvoiceTab .tt-suggestion').click(function () {
        console.log('clicked');
        var clickitem = this;
        $('.CustomerInvoiceTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

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

        /*
        $(item).parent().parent().addClass('focusedItem');
        $(item).focus();
        */

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
        url: domainurl + "/Invoice/GetOnlyEquipmentListByKey",
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
                        /*2*/ resultparse[i].SupplierCost,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType,
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        /*7*/resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/* ImageSource*/);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                POEqSuggestionclickbind(item);
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
        }

    } else if (event.keyCode == 38) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).prev('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtProductDesc')) {
            $($(trselected).prev('tr')).find('input.txtProductDesc').focus();
        } else if ($(event.target).hasClass('txtProductQuantity')) {
            $($(trselected).prev('tr')).find('input.txtProductQuantity').focus();
        }
    }
}

var Vendorclickbind = function (item) {
    $('.Vendor_name_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.Vendor_name_insert_div .tt-menu').hide();
        var VendorNum = $(clickitem).attr("data-companyname").trim();
        $("#VendorList").val(VendorNum);
        var VendorCity = $(clickitem).attr("data-city").trim();
        var VendorState = $(clickitem).attr("data-state").trim();
        SupplierAddress = String.format(VendorAddressTemplate,
            /*0*/$(clickitem).attr("data-companyname").trim(),
            /*1*/$(clickitem).attr("data-street").trim(),
            /*2*/$(clickitem).attr("data-city").trim(),
            /*3*/$(clickitem).attr("data-state").trim(),
            /*4*/$(clickitem).attr("data-zip").trim(),
            /*5*/$(clickitem).attr("data-Phone").trim(),
            /*6*/$(clickitem).attr("data-EmailAddress").trim(),
            /*7*/$(clickitem).attr("data-SalesRepName").trim());

        tinyMCE.get('PurchaseOrder_BillingAddress').setContent(SupplierAddress);

        SupplierId = $(clickitem).attr("data-SupplierId").trim();
    });
    $('.Vendor_name_insert_div .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var VendorSearchKeyDown = function (item, event) {
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
var VendorSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Expense/GetVendorListByKey",
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
                    var name = resultparse[i].Name;

                    searchresultstring = searchresultstring + String.format(VendorSuggestiontemplate,
                        resultparse[i].Street,/*0*/
                        resultparse[i].City == "-1" ? "" : resultparse[i].City,/*1*/
                        resultparse[i].State == "-1" ? "" : resultparse[i].State,/*2*/
                        resultparse[i].Zipcode,/*3*/
                        resultparse[i].Name,/*4*/
                        resultparse[i].Id,/*5*/
                        resultparse[i].CompanyName,/*6*/
                        resultparse[i].Phone,/*7*/
                        resultparse[i].EmailAddress,/*8*/
                        resultparse[i].SupplierId,/*9*/
                        resultparse[i].SalesRepName/*10*/
                        );
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                Vendorclickbind(item);
                if (resultparse.length > 4) {
                    $(".Vendor_name_insert_div .NewProjectSuggestion").height(200);
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}

var InitRowIndex = function () {
    var i = 1;
    $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var SaveDemandOrder = function (OpenEmail) {
    if ($(".HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "No items choosen.");
        return 0;
    }
    var url = domainurl + "/PurchaseOrder/AddDemandOrder";
    var DetailList = [];
    var ValidattionPassed = true;
    $(".HasItem").each(function (e, item) {
        DetailList.push({
            EquipmentId: $(item).attr('data-id'),
            EquipName: $(item).find('.ProductName').val(),
            EquipDetail: $(item).find('.txtProductDesc').val(),
            Quantity: $(item).find('.txtProductQuantity').val(),
            RecieveQty: $(item).find('.txtProductQuantityReceived').val(),
            CreatedDate: '1-1-2017',
        });
    });

    if (!ValidattionPassed) {
        OpenErrorMessageNew("Error!", "Please fill up all received quantities.");
        return 1;
    }

    var param = JSON.stringify({
        "PurchaseOrderTech.DemandOrderId": $("#PurchaseOrderTech_DemandOrderId").val(),
        "PurchaseOrderBranch.DemandOrderId": $("#PurchaseOrderBranch_DemandOrderId").val(),
        PurchaseOrderDetail: DetailList,
        OpenTab: OpenTab
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
            if (data.result) {
                if (OpenEmail) {
                    $(".SendPurchaseOrderPreview").click();
                }
                else {
                    OpenSuccessMessageNew("", "", function () {
                        TechPOListLoadOwn(data.techId, 1);
                        $(".technicianListContentPurchase").each(function () {
                            if ($(this).attr("idval") == data.techId) {
                                $(this).addClass("active");
                            }
                        });
                        $("#TechDOTabli").find('a').click();
                        CloseTopToBottomModal();
                    });
                }
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            /*console.log(errorThrown);*/
            $(".AddInvoiceLoader").addClass('hidden');
            OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
        }
    });

}
var CreateBillForPo = function () {
    OpenConfirmationMessageNew("Confirm?", "Create bill for this PO?", function () {
        CreateBillForPoConfirm();
    });
}

var CreateBillForPoConfirm = function () {
    var url = domainurl + "/PurchaseOrder/CreateBillFromPO";
    var param = JSON.stringify({
        POId: $("#PurchaseOrder_PurchaseOrderId").val(),
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
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            /*console.log(errorThrown);*/
            $(".AddInvoiceLoader").addClass('hidden');
            OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
        }
    });
}


var InitAddPurchaseOrder = function () {
    setTimeout(function () {
        var WindowHeight = window.innerHeight;
        var divHeight = WindowHeight - ($(".avb_div_header").height() + $(".add_demand_order_footer").height() + 16);
        $(".PoContentScroll").css("height", divHeight);
    }, 1000);
    $(".CustomerInvoiceTab tbody").sortable({
        update: function () {
            var i = 1;
            $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
                $(this).text(i);
                i += 1;
            });
        }
    }).disableSelection();
    InitRowIndex();
}

$(document).ready(function () {
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [
        { id: "#InvoicePrintAndPreview", type: 'iframe', width: Popupwidth, height: 600 },
        { id: ".SendPurchaseOrderPreview", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    var DueDatepicker = new Pikaday({
        field: $('#PurchaseOrder_OrderDate')[0],
        format: 'MM/DD/YYYY'
    });

    $("#VendorList").focusout(function () {
        setTimeout(function () {
            $(".Vendor_name_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#CustomerInvoiceTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide();
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
        }
    });
    $("#CustomerInvoiceTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        //console.log(e.target);
        if (!(POStatus == "Partial" || POStatus == "Paid")) {
            if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
                || $(e.target).hasClass("spnProductQuantity")
                || $(e.target).hasClass("spnProductAmount")) {

                $("#CustomerInvoiceTab tr").removeClass("focusedItem");
                $($(e.target).parent().parent()).addClass("focusedItem");
                $(e.target).parent().find('input').focus();
            } else if (e.target.tagName.toUpperCase() == 'INPUT') {
                return;
            }
            else {
                $("#CustomerInvoiceTab tr").removeClass("focusedItem");
                $($(e.target).parent()).addClass("focusedItem");
                $(e.target).find('input').focus();
            }

        }
    });
    /*Add new row*/
    $("#CustomerInvoiceTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        if (POStatus == "Partial" || POStatus == "Paid") {
            return;
        }
        $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
        var i = 1;
        $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    /*Remove last row*/
    $(".CustomerInvoiceTab tbody").on('click', 'tr td i.fa', function (e) {
        if (POStatus == "Partial" || POStatus == "Paid") {
            return;
        }
        $(this).parent().parent().remove();
        if ($(".CustomerInvoiceTab tbody tr").length < 2) {
            $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
        }
        var i = 1;
        $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        $(ProductQuantityDom).text($(this).val());
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantityReceived", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantityReceived');
        $(ProductQuantityDom).text($(this).val());
    });

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductDesc", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });
    $(".CheckPrintAndPreview").click(function () {
        if ($(".HasItem").length == 0) {
            OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
        }
        else {
            $("#InvoicePrintAndPreview").click();
            //if ($(".shippingAddress").val() == "") {
            //    $(".shippingAddress").val($("#Invoice_BillingAddress").val());
            //}
        }
    });
    InitAddPurchaseOrder();
})