var VendorSuggestiontemplate =
            "<div class='tt-suggestion tt-selectable' data-street = '{0}' data-city = '{1}' data-state = '{2}' data-zip = '{3}' data-name = '{4}' data-vendorid = '{5}' data-companyname = '{6}' data-phone='{7}' data-EmailAddress='{8}' data-SupplierId='{9}' data-SalesRepName='{10}' data-cost='{11}'>"
               + "<p class='tt-sug-text'>"
                   + "{6}"
               + "</p> "
            + "</div>";
var Vendorclickbind = function (item) {
    $('.Vendor_name_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.Vendor_name_insert_div .tt-menu').hide();
        var VendorNum = $(clickitem).attr("data-companyname").trim();
        var SupplierId = $(clickitem).attr("data-SupplierId").trim();
        var Cost = parseFloat('0');
        if ($(clickitem).attr("data-Cost").trim() != null && $(clickitem).attr("data-Cost").trim() != undefined) {
            Cost = parseFloat($(clickitem).attr("data-Cost").trim());
        }
        var FormateCost = Currency + Cost.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        $(item).val(VendorNum);
        $(item).attr("data-supplierId", SupplierId);
        $(item).parent().parent().find(".txtPrimaryVendor").val(VendorNum);
        $(item).parent().parent().find(".txtPrimaryVendor").attr("data-supplierId", SupplierId);
        $(item).parent().parent().find(".spnPrimaryVendor").text(VendorNum);
        $(item).parent().parent().find(".spnPrimaryVendor").attr("data-supplierId", SupplierId);
        $(item).parent().parent().parent().find(".PriceMassPO").first().text(FormateCost);
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
var VendorSearchKeyUp = function (item, event, equipmentId, fromclick) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;

    var KeyValue = $(item).val();
    if (fromclick == "click") {
        KeyValue = "";
    }
    $.ajax({
        url: domainurl + "/Inventory/GetVendorListByKeyAndEquipmentId",
        data: {
            key: KeyValue,
            EquipmentId: equipmentId
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
                        resultparse[i].SalesRepName,/*10*/
                        resultparse[i].Cost/*11*/
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
var SaveMassPO = function () {
    var url = domainurl + "/Inventory/SaveMassPO";
    var DetailList = [];
    var ValidattionPassed = true;
    $(".HasItem").each(function (e, item) {
        if ($(item).find('.txtQuantity').val() != 0 && $(item).find('.txtQuantity').val() != "" && $(item).find('.txtPrimaryVendor').val() != "") {
            DetailList.push({
                EquipmentId: $(item).attr('data-eqpid'),
                SupplierId: $(item).find('.spnPrimaryVendor').attr("data-supplierId"),
                Quantity: $(item).find('.spnQuantity').text(),
                DemandOrderId: $(item).attr('data-demandorderid')
            });
        }
    });
    var param = JSON.stringify({
        MassPOList: DetailList
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
                OpenSuccessMessageNew("Success", "", function () {
                    window.location.reload();
                });
            } else {
                $(".HasItem").each(function (e, item) {
                    if ($(item).find('.txtPrimaryVendor').val() == "") {
                        OpenErrorMessageNew("Error!", "Please fill up primary vendor.");
                        return 1;
                    }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
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
}

$(document).ready(function () {
    //$("#CustomerMassPOTab tbody").on('blur', 'tr', function (item) {
    //    if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined') {
    //        var trdom = $(item.target).parent().parent();
    //        $(trdom).find("input.txtQuantity").val('');
    //        $(trdom).find("span.spnQuantity").text('');
    //    }
    //});
    $("#CustomerMassPOTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass("spnQuantity") || $(e.target).hasClass("spnPrimaryVendor")) {
            $("#CustomerMassPOTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        } else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerMassPOTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
        if ($(e.target).hasClass("spnPrimaryVendor")) {
            var equipmentId = $(e.target).attr("data-equipmentid");
            VendorSearchKeyUp($(e.target), event, equipmentId, 'click');
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtQuantity", function () {
        var QuantityDom = $(this).parent().find('span.spnQuantity');
        $(QuantityDom).text($(this).val());
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtPrimaryVendor", function () {
        var PrimaryVendorDom = $(this).parent().find('span.spnPrimaryVendor');
        $(PrimaryVendorDom).text($(this).val());
    });
    InitAddPurchaseOrder();
})