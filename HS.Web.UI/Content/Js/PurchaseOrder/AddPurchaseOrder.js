var DeletePurchaseOrderbyId = function (PODeleteId) {
    $.ajax({
        url: domainurl + "/PurchaseOrder/DeletePurchaseOrder",
        data: { Id: PODeleteId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Purchase Order deleted successfully!");
                CloseTopToBottomModal();
                OpenPurchaseOrderTab();
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

        }
    });
}

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
    '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}" data-sku="{7}">'
    /*
    *For Equipment Image
    *+ "<img src='{7}' class='EquipmentImage'>"*/
    + "<p class='tt-sug-text'>" 
    + "<em class='tt-sug-type'>{5}</em>" 
    + "<span>{1}</span>" 
    + "<em class='tt_sug_manufac'>{8}</em>"  
    + "<em class='tt-eq-price'>${2}</em><br /> SKU: {7}"
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
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtSKU' />"
    + "<span class='spnSKU'></span>"
    + "</td>"
    + "<td valign='top'>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
    + "<span class='spnProductQuantity'></span>"
    + "</td>";
if (Receiving) {
    NewEquipmentRow += "<td valign='top'>"
        + "<span class='' readonly></span>"
        + "</td>";
    NewEquipmentRow += "<td valign='top'>"
        + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantityReceived' readonly />"
        + "<span class='spnProductQuantityReceived'></span>"
        + "</td>";

    NewEquipmentRow += "<td valign='top'>"
        + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantityReceiving' />"
        + "<span class='spnProductQuantityReceiving'></span>"
        + "</td>";

}
NewEquipmentRow += "<td valign='top'>"
    + "<div class='pAmount'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>$</div>"
    + "</div>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductRate' />"
    + "</div> "
    + "<span class='spnProductRate'></span>"
    + "</td>" + "<td valign='top'>"
    + "<div class='pAmount'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>$</div>"
    + "</div>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
    + "</div> "
    + "<span class='spnProductAmount'></span>"
    + "</td>"
    + "<td valign='top' class='tableActions'>"
    + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
    + "</td>"
    + "</tr>";

var POEqSuggestionclickbind = function (item) {
    $('.CustomerInvoiceTab .tt-suggestion').click(function () {
        console.log('clicked');
        var clickitem = this;
        $('.CustomerInvoiceTab .tt-menu').hide();
        /*ID*/
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        /*Name*/
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());
        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text($(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        $(txtItemRate).val($(this).attr('data-price'));
        /*Item Description Set*/
        var spnItemRate = $(item).parent().parent().find('.spnSKU');
        $(spnItemRate).text($(this).attr('data-sku'));
        var txtItemRate = $(item).parent().parent().find('.txtSKU');
        $(txtItemRate).val($(this).attr('data-sku'));

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
        /*Item QuantityReceived Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductQuantityReceived');
        $(spnItemRate).text(0);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantityReceived');
        $(txtItemRate).val(0);
        /*Item QuantityReceiving Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductQuantityReceiving');
        $(spnItemRate).text(0);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantityReceiving');
        $(txtItemRate).val(0);


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
        url: domainurl + "/Invoice/GetOnlyEquipmentListByKey",
        data: {
            key: $(item).val(),
            ExistEquipment: ExistEquipment
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);
            console.log("test obayed");
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
                        /*7*/resultparse[i].SKU,
                        /*8*/resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/*ImageSource*/);
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
        if ($(event.target).hasClass('txtSKU')) {
            $($(trselected).next('tr')).find('input.txtSKU').focus();
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
        if ($(event.target).hasClass('txtSKU')) {
            $($(trselected).prev('tr')).find('input.txtSKU').focus();
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
    Amount = parseFloat('0');
    $(".txtProductAmount").each(function () {
        var _CalAmt = $(this).val().trim();
        _CalAmt = _CalAmt.replaceAll(',', '');

        var currAmount = parseFloat(_CalAmt);
        if (!isNaN(currAmount)) {
            Amount += currAmount;
        }
    });
    TotalAmount = Amount;


    /*Output Part Starts*/

    $(".big-amount-top").text(Currency + TotalAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".avp_amount_subtotal").text(Currency + Amount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    /*Output Part Ends*/

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

        tinyMCE.get('PurchaseOrderWarehouse_BillingAddress').setContent(SupplierAddress);

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

var SavePurchaseOrder = function (OpenEmail) {
    //console.log("SavePurchaseOrder");
    var Proceed = true;
    if ($(".HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "No items choosen.");
        return 0;
    }
    //console.log(Amount);
    if (Amount == "0") {
        OpenErrorMessageNew("Error!", "Total Equipment Amount for Purchase order can not be 0 ");
        return 0;

    }

    var url = domainurl + "/PurchaseOrder/AddPurchaseOrder";
    var DetailList = [];
    var ReceivingQuantity = 0;
    $(".HasItem").each(function (e, item) {
        //console.log("final");

        var ProductOrderQuantity = parseInt($(item).find('.txtProductQuantity').val());
        var ProductReceivingQuantity = parseInt($(item).find(".txtProductQuantityReceiving").val());
        var ProductReceivedQuantity = parseInt($(item).find('.txtProductQuantityReceived').val());
        
        var receivedval = parseInt($(item).find('.txtProductQuantityReceived').val());
        ReceivingQuantity += parseInt($(item).find(".txtProductQuantityReceiving").val());
        var BalReceivable = parseInt(ProductOrderQuantity - (ProductReceivedQuantity + ProductReceivingQuantity));
        var SKU = $(item).find('.txtSKU').val();
        console.log(item);
        console.log(SKU, ProductOrderQuantity, ProductReceivedQuantity, ProductReceivingQuantity, BalReceivable);

        if (BalReceivable < 0) {
            console.log('-ve value');
            Proceed = false;
            OpenErrorMessageNew("Error!", "Quantity received(" + ProductReceivedQuantity + ") and receiving(" + ProductReceivingQuantity + ") is more than order quantity(" + ProductOrderQuantity + ") for " + SKU);
            return false;
        }

        receivedval += parseInt($(item).find(".txtProductQuantityReceiving").val());
        
        var TotalPrice = ($(item).find('.txtProductQuantity').val() * parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', ''))).toString();
        DetailList.push({
            PurchaseOrderId: $("#PurchaseOrderWarehouse_PurchaseOrderId").val(),
            EquipmentId: $(item).attr('data-id'),
            EquipName: $(item).find('.ProductName').val(),
            EquipDetail: $(item).find('.txtSKU').val(),
            Quantity: ProductOrderQuantity,
            UnitPrice: $(item).find('.txtProductRate').val().trim().replaceAll(',', ''),
            TotalPrice: TotalPrice,
            RecieveQty: receivedval,
            CreatedDate: '1-1-2017',
            CurrentQty: $(item).find(".txtProductQuantityReceiving").val()
        });
    });

    if (!Proceed) {
        return 0;
    }

    //if (isgreater) {
    //    OpenErrorMessageNew("Error!", "Received Quantity cannot be greater than order");
    //    return 0;
    //}
    if (Receiving && ReceivingQuantity == 0) {
        OpenErrorMessageNew("Error!", "Please fill up receiving quantities.");
        return 1;
    }

    if (Receiving && $("#PurchaseOrderWarehouse_POFor").val() == "00000000-0000-0000-0000-000000000000") {
        OpenErrorMessageNew("Error!", "Please select Received for.");
        return 1;
    }
    if (!Receiving && $("#PurchaseOrderWarehouse_POFor").val() == "00000000-0000-0000-0000-000000000000") {
        OpenErrorMessageNew("Error!", "Please select Received for.");
        return 1;
    }

    if (Proceed) {
        var param = JSON.stringify({
            "PurchaseOrderWarehouse.Id": $("#PurchaseOrderWarehouse_Id").val(),
            "PurchaseOrderWarehouse.PurchaseOrderId": $("#PurchaseOrderWarehouse_PurchaseOrderId").val(),
            "PurchaseOrderWarehouse.SuplierId": $("#PurchaseOrderWarehouse_SuplierId").val(),
            "PurchaseOrderWarehouse.POFor": $("#PurchaseOrderWarehouse_POFor").val(),
            "PurchaseOrderWarehouse.SoldBy": '00000000-0000-0000-0000-000000000000',
            "PurchaseOrderWarehouse.Amount": Amount,
            "PurchaseOrderWarehouse.Tax": $("#PurchaseOrderWarehouse_Tax").val(),
            "PurchaseOrderWarehouse.TaxType": $("#PurchaseOrderWarehouse_TaxType").val(),
            "PurchaseOrderWarehouse.Deposit": $("#PurchaseOrderWarehouse_Deposit").val(),
            "PurchaseOrderWarehouse.TotalAmount": TotalAmount,
            "PurchaseOrderWarehouse.Balance": $("#PurchaseOrderWarehouse_Balance").val(),
            "PurchaseOrderWarehouse.BalanceDue": $("#PurchaseOrderWarehouse_BalanceDue").val(),
            "PurchaseOrderWarehouse.Status": POStatus,
            "PurchaseOrderWarehouse.StrOrderDate": $("#PurchaseOrderWarehouse_OrderDate").val(),
            "PurchaseOrderWarehouse.BillingAddress": tinyMCE.get('PurchaseOrderWarehouse_BillingAddress').getContent(),
            "PurchaseOrderWarehouse.ShippingAddress": tinyMCE.get('PurchaseOrderWarehouse_ShippingAddress').getContent(),
            "PurchaseOrderWarehouse.ShippingVia": $("#PurchaseOrderWarehouse_ShippingVia").val(),
            "PurchaseOrderWarehouse.ShippingCost": $("#PurchaseOrderWarehouse_ShippingCost").val(),
            "PurchaseOrderWarehouse.TrackingNo": $("#PurchaseOrderWarehouse_TrackingNo").val(),
            "PurchaseOrderWarehouse.Message": $("#PurchaseOrderWarehouse_Message").val(),
            "PurchaseOrderWarehouse.Description": $("#PurchaseOrderWarehouse_Description").val(),
            "PurchaseOrderWarehouse.RecieveForUid": $("#PurchaseOrderWarehouse_POFor").val(),
            //"PurchaseOrderWarehouse.RecieveForUid": $("#PurchaseOrderWarehouse_RecieveForUid").val(),
            "PurchaseOrderWarehouse.RecieveDate": $("#PurchaseOrderWarehouse_RecieveDate").val(),
            PurchaseOrderDetail: DetailList,
            ReceiveNow: Receiving,
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
                        if (OpenTab == "Ware") {
                            OpenPurchaseOrderTab();
                        }
                        else if (OpenTab == "Branch") {
                            OpenBranchPurchaseOrderTab();
                        }
                        else if (OpenTab == "Tech") {
                            OpenTechPurchaseOrderTab();
                        }
                    }
                    else {
                        OpenSuccessMessageNew("Success!", data.message, function () {
                            if (OpenTab == "Ware") {
                                POListLoad(currentPage);
                            }
                            else if (OpenTab == "Branch") {
                                OpenBranchPurchaseOrderTab();
                            }
                            else if (OpenTab == "Tech") {
                                OpenTechPurchaseOrderTab();
                            }
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
}

var CreateBillForPo = function () {
    if ($("#PurchaseOrderWarehouse_SuplierId").val() != "00000000-0000-0000-0000-000000000000") {
        OpenConfirmationMessageNew("Confirm?", "Create bill for this PO?", function () {
            CreateBillForPoConfirm();
        });
    }
    else {
        $("#PurchaseOrderWarehouse_SuplierId").css({ "border-color": "red" });
    }
}

var CreateBillForPoConfirm = function () {
    var url = domainurl + "/PurchaseOrder/CreateBillFromPO";
    var param = JSON.stringify({
        POId: $("#PurchaseOrderWarehouse_PurchaseOrderId").val(),
        supid: $("#PurchaseOrderWarehouse_SuplierId").val()
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
        var divHeight = WindowHeight - ($(".avb_div_header").height() + $(".invoice-footer").height() + 16);
        $(".PoContentScroll").css("height", divHeight);
    }, 1000);
    $(".DescStartCount").html($("#PurchaseOrderWarehouse_Description").val().length);
    $("#PurchaseOrderWarehouse_Description").keyup(function () {
        $(".DescStartCount").html($("#PurchaseOrderWarehouse_Description").val().length);
    });
    $(".MsgStartCount").html($("#PurchaseOrderWarehouse_Message").val().length);
    $("#PurchaseOrderWarehouse_Message").keyup(function () {
        $(".MsgStartCount").html($("#PurchaseOrderWarehouse_Message").val().length);
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
    InitRowIndex();
}
var SaveBillFile = function () {
    var url = domainurl + "/Expense/AddVendorBilFile/";
    var param = JSON.stringify({
        File: $("#UploadedPath").val(),
        BillNo: purchaseorderId,
        FileDes: $('#Bill_file_description').val()
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
            OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
                $(".LoadBillFiles").load("/Expense/LoadBillFiles?BillNo=" + purchaseorderId)
                $("#Right-To-Left-Modal-Body .close").click();
            });
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}
var FillMailingAddredd = function () {
    var url = domainurl + "/Supplier/GetSupplierDetailsBySupplierId/";
    var param = JSON.stringify({
        supplierId: $("#PurchaseOrderWarehouse_SuplierId").val()
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            SupplierAddress = String.format(VendorAddressTemplate,
            /*0*/data.supplierDetails.CompanyName,
            /*1*/data.supplierDetails.Street,
            /*2*/data.supplierDetails.City,
            /*3*/data.supplierDetails.State,
            /*4*/data.supplierDetails.Zipcode,
            /*5*/data.supplierDetails.Phone,
            /*6*/data.supplierDetails.EmailAddress,
            /*7*/data.supplierDetails.SalesRepName);

            tinyMCE.get('PurchaseOrderWarehouse_BillingAddress').setContent(SupplierAddress);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

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
    $("#PurchaseOrderWarehouse_SuplierId").change(function () {
        if ($("#PurchaseOrderWarehouse_SuplierId").val() != "00000000-0000-0000-0000-000000000000") {
            FillMailingAddredd();
        }
        else {
            tinyMCE.get('PurchaseOrderWarehouse_BillingAddress').setContent("");
        }
    });
    $(".PurchaseOrderDeleteButton").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this purchase order?", function () {
            DeletePurchaseOrderbyId(PurchaseOrder_int_Id);
        });
    });
    var DueDatepicker = new Pikaday({
        field: $('#PurchaseOrderWarehouse_OrderDate')[0],
        trigger: $('#AddPurOrDate')[0],
        format: 'M/D/YYYY',
        firstDay: 1
    });
    var ReceiveDatepicker = new Pikaday({
        field: $('#PurchaseOrderWarehouse_RecieveDate')[0],
        trigger: $('#ReceivedDate')[0],
        format: 'M/D/YYYY',
        firstDay: 1
    });
    $("#VendorList").focusout(function () {
        setTimeout(function () {
            $(".Vendor_name_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#CustomerInvoiceTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide();
        var ProductNameDom = $(this).parent().find('span.spnProductName');
        $(ProductNameDom).text($(this).val());
    });

    $("#CustomerInvoiceTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        //console.log(e.target);
        if (!(POStatus == "Partial" || POStatus == "Paid")) {
            if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnSKU")
                || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
                || $(e.target).hasClass("spnProductQuantityReceiving")
                || $(e.target).hasClass("spnProductAmount")) {

                $("#CustomerInvoiceTab tr").removeClass("focusedItem");
                $($(e.target).parent().parent()).addClass("focusedItem");
                $(e.target).parent().find('input').focus();

            }
            else if ($(e.target).hasClass("pAmount")) {
                $(".ServiceEqTab tr").removeClass("focusedItem");
                $($(e.target).parent().parent()).addClass("focusedItem");
                $(e.target).find('input').focus();

            }
            else if (e.target.tagName.toUpperCase() == 'INPUT') {
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
        if (IsShowAllPermit.toLocaleLowerCase() == "true") {
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
        }

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
        CalculateNewAmount();
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        $(ProductQuantityDom).text($(this).val());
        if (!Receiving) {
            var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
            console.log(ProductRateDom);
            if ($(ProductRateDom).val() != "" && parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) > 0) {
                var NewProductAmount = $(this).val() * parseFloat($(ProductRateDom).val().trim().replaceAll(',', ''));
                console.log(NewProductAmount);
                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(txtProductAmountDom).val(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductAmountDom).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewAmount();
            }
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantityReceived", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantityReceived');
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

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantityReceiving", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantityReceiving');
        $(ProductQuantityDom).text($(this).val());
        //var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        //if ($(ProductRateDom).val() != "" && parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) > 0) {
        //    var NewProductAmount = $(this).val() * parseFloat($(ProductRateDom).val().trim().replaceAll(',', ''));
        //    console.log(NewProductAmount);
        //    var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
        //    var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        //    $(txtProductAmountDom).val(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        //    $(spnProductAmountDom).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        //    CalculateNewAmount();
        //}
    });

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductAmount", function () {

        var ProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
        console.log(ProductAmountDom);
        $(ProductAmountDom).text(parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        if (Receiving) {
            ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantityReceiving');
        }
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().parent().find('span.spnProductRate');
        console.log(spnProductRateDom);
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
            var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
            $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            $(spnProductRateDom).text(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        CalculateNewAmount();
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductRate", function () {
        /*
        *If product rate changes make change to amount.
        */ 
        var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        $(ProductRateDom).text(parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(3).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');

        console.log($(ProductQuantityDom).val())
        if (Receiving) {
            ProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantityReceiving');
        }
        if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
            var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();

            var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
            $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
            $(spnProductAmountDom).text(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            CalculateNewAmount();
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtSKU", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnSKU');
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

    $(".LoadBillFiles").load("/Expense/LoadBillFiles?BillNo=" + purchaseorderId);

    $("#SaveBillFile").click(function () {
        if (CommonUiValidation() && $("#UploadedPath").val() != "") {
            SaveBillFile();
            $(".fileborder").removeClass('red-border');
        }
        if ($("#UploadedPath").val() == "") {
            $("#uploadfileerror").removeClass("hidden");
            $(".fileborder").addClass('red-border');
        }
    });
    $("#UploadCustomerFileBtn").click(function () {
        console.log("sdfdsf");
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".HasItem").each(function (e, item) {
        var receivedval = parseInt($(item).find('.txtProductQuantityReceived').val());
        var orderval = parseInt($(item).find('.txtProductQuantity').val());
        if (receivedval >= orderval) {
            $(item).find('td').removeClass('hover_green_style');


            $(item).find('.txtProductQuantityReceiving').prop('disabled', true);
            $(item).find('.txtProductQuantityReceiving').css('background-color', '#f0f0ef');

        }



    })
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/Expense/UploadBillFile', /* CustomerId*/
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            if (ext[1] == 'doc' || ext[1] == 'docx' || ext[1] == 'xls' || ext[1] == 'xlsx' || ext[1] == 'jpeg' || ext[1] == 'jpg' || ext[1] == 'gif' || ext[1] == 'png' || ext[1] == 'rtf' || ext[1] == 'pdf' || ext[1] == 'txt' || ext[1] == 'mp4' || ext[1] == 'mov') {

                if (data.files[0].size <= 50000000) {
                    UserFileUploadjqXHRData = data;
                }
                else {
                    OpenErrorMessageNew("Error!", "File size is more then 50 mb.", function () {
                        $(".close").click();
                    })
                }

            }
            else {
                OpenErrorMessageNew("Error!", "File format not valid.", function () {
                    $(".close").click();
                })
            }

        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progress").show();
            $(".file-progress .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progress .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            console.log("dfdf");
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);

            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);
                var spfile = data.result.FullFilePath.split('.');
                //if (spfile[1] == "png" || spfile[1] == "jpg" || spfile[1] == "jpeg") {
                //    $(".Upload_Doc").addClass('hidden');

                //    $(".LoadPreviewDocument").removeClass('hidden');
                //    $("#Preview_Doc").attr('src', data.result.FullFilePath);
                //}
                $(".fileborder").removeClass('red-border');
                $("#uploadfileerror").addClass("hidden");
                $("#Bill_file_description").val(data.result.filedes);
                $(".apo_image_add .fileborder").css("border", "1px solid #ccc");
                var index = spfile.length - 1;
                if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                    //$(".Upload_Doc").addClass('hidden');
                    //$(".LoadPreviewDocument").removeClass('hidden');
                    //$("#Preview_Doc").attr('src', data.result.FullFilePath);
                    $("#UploadCustomerFileBtn").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").addClass('custom-file');
                    $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                    $(".fileborder").addClass('border_none');
                }
                else if (spfile[index] == "pdf") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "doc" || spfile[index] == "docx") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/mp4.png');
                    //$("#UploadCustomerFileBtn").addClass('otherfileposition');
                    //$("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                //else {
                //    $(".Upload_Doc").addClass('hidden');
                //    $(".LoadPreviewDocument").addClass('hidden');
                //    $(".LoadPreviewDocument1").removeClass('hidden');
                //    $("#Frame_Doc").attr('src', data.result.FullFilePath);
                //}
            }
        },
        fail: function (event, data) {
            //if (data.files[0].error) {
            //    //alert(data.files[0].error);
            //}
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });

    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_Doc").removeClass('hidden');
            //$(".LoadPreviewDocument").addClass('hidden');
            //$(".LoadPreviewDocument1").addClass('hidden');
            $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/blank_thumb_file.png');
            $(".chooseFilebtn").removeClass("hidden");
            $(".changeFilebtn").addClass("hidden");
            $(".deleteDoc").addClass("hidden");
            $("#Preview_Doc").attr('src', "");
            $("#Frame_Doc").attr('src', "");
            $("#UploadSuccessMessage").hide();
            $("#Bill_file_description").val("");
            $("#UploadedPath").val('');
            $(".fileborder").addClass('border_none');
            $("#UploadCustomerFileBtn").removeClass('otherfileposition');
            $(".apo_image_add .fileborder").css("border", "none");
        });
    });
    $("#PurchaseOrderWarehouse_SuplierId").change(function () {
        if ($("#PurchaseOrderWarehouse_SuplierId").val() != "00000000-0000-0000-0000-000000000000") {
            $("#PurchaseOrderWarehouse_SuplierId").css({ "border-color": "#ccc" });
        }
        else {
            $("#PurchaseOrderWarehouse_SuplierId").css({ "border-color": "red" });
        }
    });
    $("[type='text'].sanitizeit").focus(function (e) {
        if ($(this).attr('oval') == '-1') {
            $(this).attr('oval', $(this).val().trim());
        }
        if ($(this).val().trim() == '0') {
            $(this).val('');
        }
        else if ($(this).val().trim() == '' && $(this).parent('td').parent('tr').hasClass('HasItem')) {
            $(this).val($(this).attr('oval'));
            $(this).next('span').text($(this).val());
        }
    });
    $("[type='text'].sanitizeit").blur(function (e) {
        
        if ($(this).val().trim() == '' && $(this).parent('td').parent('tr').hasClass('HasItem')) {
            $(this).val($(this).attr('oval'));
            $(this).next('span').text($(this).val());
        }
    })
})