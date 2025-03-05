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
var OpenSecondTab = function () {
    $("#LoadLeadDetail").html(TabsLoaderText);
    $("#LoadLeadDetail").load($("#LoadService").attr('data-url'));
}

var InvoiceEqSuggestionclickbind = function (item) {
    $('.ServiceInfo .tt-suggestion').click(function () {
        console.log("Enter fired");
        var clickitem = this;
        $('.ServiceInfo .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        var ServiceId = $(clickitem).attr('data-id');
        $(item).attr('data-id', ServiceId);
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

        /*Item Rate Set*/
        var txtItemRate = $(item).parent().parent().parent().find('.txtProductRate');
        var ServicePrice = parseFloat($(this).attr('data-price'));
        $(txtItemRate).val(ServicePrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

        /*Item ProductDiscountRate Set*/

        var txtItemRate = $(item).parent().parent().parent().find('.txtProductDiscountRate');
        $(txtItemRate).val(0);

        /*Total Amount Set*/

        var txtItemRate = $(item).parent().parent().parent().find('.txtTotalAmount');
        $(txtItemRate).val(ServicePrice.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        LoadServiceOptionsByServiceId(ServiceId);
    });
    $('.ServiceInfo .tt-suggestion').hover(function () {
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
    $(".Setup-Equipments-Table .serviceRows").each(function () {
        ExistEquipmentInner += "'" + $(this).attr('serviceId') + "',";
    });
    if (ExistEquipmentInner.length > 0) {
        ExistEquipmentInner = ExistEquipmentInner.slice(0, -1);
        ExistEquipment = "(" + ExistEquipmentInner + ")";
    }

    $.ajax({
        url: domainurl + "/Invoice/GetOnlyServiceListByKey",
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

    if (event.keyCode == 13) {/*Enter; select the item*/
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        if (selectedTTMenu.length > 0) {
            setTimeout(function () { $(selectedTTMenu).click(); }, 10)
            $('.tt-menu').hide();
        }
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
                OpenSuccessMessageNew('Success!', data.message, function () {
                    var strPayInfo = "";
                    var strTotalAmt = parseFloat(data.strTotalAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strCollectedAmt = parseFloat(data.strCollectedAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strAchPayment = parseFloat(data.strAchPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strCreditCardPayment = parseFloat(data.strCreditCardPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strCashPayment = parseFloat(data.strCashPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    var strCheckPayment = parseFloat(data.strCheckPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                    if (data.strCreditCardPayment != '0') {
                        strPayInfo += "CC: " + Currency + strCreditCardPayment + ", ";
                    }
                    if (data.strAchPayment != '0') {
                        strPayInfo += "ACH: " + Currency + strAchPayment + ", ";
                    }
                    if (data.strCashPayment != '0') {
                        strPayInfo += "Cash: " + Currency + strCashPayment + ", ";
                    }
                    if (data.strCheckPayment != '0') {
                        strPayInfo += "Check: " + Currency + strCheckPayment + ", ";
                    }
                    if (strPayInfo != "") {
                        $(".total_captured_amount").html("");
                        strPayInfo = strPayInfo.slice(0, -2);
                        var strAmountText = "Total amount to be collected: " + Currency + strTotalAmt + "<br/>";

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
                    //LeadEquipmetnPriceCalculation();

                });
                OpenSecondTab();
            } else {
                OpenErrorMessageNew('Error!', data.message, function () { });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var LoadServiceOptionsByServiceId = function (ServiceId) {
    $(".serviceOptionsDiv").load(domainurl + "/SmartLeads/ShowServiceOptions/?ServiceId=" + ServiceId);
}
var AddServiceWithOptions = function () {
    if (AddServiceValidation()) {
        var url = domainurl + "/SmartLeads/AddCustomerPackageService";
        var Param = JSON.stringify({
            CustomerId: EquipmentPartialLeadGuId,
            EquipmentId: $(".ProductName").val(),
            MonthlyRate: $(".txtProductRate").val().replaceAll(",", ""),
            DiscountRate: $(".txtProductDiscountRate").val().replaceAll(",", ""),
            Total: $(".txtTotalAmount").val().replaceAll(",", ""),
            ManufacturerId: $("#Select_Manufacturer").val(),
            LocationId: $("#Select_Location").val(),
            TypeId: $("#Select_Type").val(),
            ModelId: $("#Select_Model").val(),
            FinishId: $("#Select_Finish").val(),
            CapacityId: $("#Select_Capacity").val(),
            ChargeForFirstEquipment: ChargeForFirstEquipment
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
                $("#LoadLeadDetail").load($("#LoadService").attr('data-url'));
                var strPayInfo = "";
                var strTotalAmt = parseFloat(data.strTotalAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strCollectedAmt = parseFloat(data.strCollectedAmt).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strAchPayment = parseFloat(data.strAchPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strCreditCardPayment = parseFloat(data.strCreditCardPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strCashPayment = parseFloat(data.strCashPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                var strCheckPayment = parseFloat(data.strCheckPayment).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                if (data.strCreditCardPayment != '0') {
                    strPayInfo += "CC: " + Currency + strCreditCardPayment + ", ";
                }
                if (data.strAchPayment != '0') {
                    strPayInfo += "ACH: " + Currency + strAchPayment + ", ";
                }
                if (data.strCashPayment != '0') {
                    strPayInfo += "Cash: " + Currency + strCashPayment + ", ";
                }
                if (data.strCheckPayment != '0') {
                    strPayInfo += "Check: " + Currency + strCheckPayment + ", ";
                }
                if (strPayInfo != "") {
                    $(".total_captured_amount").html("");
                    strPayInfo = strPayInfo.slice(0, -2);
                    var strAmountText = "Total amount to be collected: " + Currency + strTotalAmt + "<br/>";

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
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
}

var AddServiceValidation = function () {
    var result = true;
    if ($(".ProductName").val() == "-1") {
        result = false;
        $(".ProductName").addClass('required');
    } else {
        $(".ProductName").removeClass('required');
    }
    if ($('.txtProductRate').val() == '') {
        result = false;
        $(".txtProductRate").addClass('required');
    }
    else if (isNaN($('.txtProductRate').val().replace(',',''))) {
        result = false;
        $(".txtProductRate").addClass('required');
    } else {
        $(".txtProductRate").removeClass('required');
    }
    return result;
}
var ServiceDropdownValidation = function () {
    var result = true;
    if ($(".ServiceOptionSelect_1").is(":visible")
           && $(".ServiceOptionSelect_1").val() == '00000000-0000-0000-0000-000000000000') {
        //show error
        $(".manufacturerError").removeClass("hidden");
        result = false;
    }else{
        //hide error
        $(".manufacturerError").addClass("hidden");
    }
    if ($(".ServiceOptionSelect_2").is(":visible")
           && $(".ServiceOptionSelect_2").val() == '00000000-0000-0000-0000-000000000000') {
        //show error
        $(".locationError").removeClass("hidden");
        result=  false;
    }else{
        //hide error
        $(".locationError").addClass("hidden");
    }
    if ($(".ServiceOptionSelect_3").is(":visible")
           && $(".ServiceOptionSelect_3").val() == '00000000-0000-0000-0000-000000000000') {
        //show error
        $(".typeError").removeClass("hidden");
       result =  false;
    }else{
        //hide error
        $(".typeError").addClass("hidden");
    }
    if ($(".ServiceOptionSelect_4").is(":visible")
           && $(".ServiceOptionSelect_4").val() == '00000000-0000-0000-0000-000000000000') {
        $(".ModelError").removeClass("hidden");
        return false;
    } else {
        //hide error
        $(".ModelError").addClass("hidden");
    }
    if ($(".ServiceOptionSelect_5").is(":visible")
        && $(".ServiceOptionSelect_5").val() == '00000000-0000-0000-0000-000000000000') {
        $(".finishError").removeClass("hidden");
    return false;
    } else {
        //hide error
        $(".finishError").addClass("hidden");
    }
    if ($(".ServiceOptionSelect_6").is(":visible")
        && $(".ServiceOptionSelect_6").val() == '00000000-0000-0000-0000-000000000000') {
        $(".capacityError").removeClass("hidden");
    return false;
    } else {
        //hide error
        $(".capacityError").addClass("hidden");
    }
    return result;
}

$(document).ready(function () {
    $("#btnSavandNex").removeClass("hidden");
    $("#ServiceInfo").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#ServiceInfo tr:last").after(LeadEquipmentRow);
        var i = 1;
        $(".ServiceInfo tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });

    $(".existing-equipments .tableActions .fa-trash-o").click(function () {
        var id = $(this).attr('dataid');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this?", function () {
            RemoveLeadEquipment(id);
        });
    });
    $("#AddServiceWithOptions").click(function () {
     
        if (ServiceDropdownValidation() == true)
        {
            AddServiceWithOptions();
        }
     

    });
    $(".txtProductDiscountRate,.txtProductRate,.txtTotalAmount").blur(function () {
        //var DiscountVal = $(".txtProductRate").val()*$(".txtProductDiscountRate").val()/100;
        var DiscountAmount = $(".txtProductDiscountRate").val();
        var ProductRate = $(".txtProductRate").val();

        ProductRate = parseFloat(ProductRate.replaceAll(",", ""));
        if (isNaN(ProductRate)) {
            ProductRate = 0;
        }
        DiscountAmount = parseFloat(DiscountAmount.replaceAll(",", ""));
        if (isNaN(DiscountAmount)) {
            DiscountAmount = 0;
        }

        var TotalAmount = ProductRate - DiscountAmount;
        TotalAmount = parseFloat(TotalAmount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

        $(".txtTotalAmount").val(TotalAmount);
        $(".txtProductRate").val(ProductRate.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(".txtProductDiscountRate").val(DiscountAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    });

    $(".ProductName").change(function (e) {
        if ($(e.target).val() == "-1") {
            $(".txtTotalAmount").val('');
            $(".txtProductDiscountRate").val('');
            $(".txtProductRate").val('');
            $("#AddServiceWithOptions").addClass("hidden");
        } else {
            $("#AddServiceWithOptions").removeClass("hidden");
            var SelectedPrice = $(".ProductName option:selected").attr('dataprice');
            SelectedPrice = parseFloat(SelectedPrice).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

            $(".txtTotalAmount").val(SelectedPrice);
            $(".txtProductDiscountRate").val("0.00");
            $(".txtProductRate").val(SelectedPrice);
            LoadServiceOptionsByServiceId($(e.target).val());
        }
    });
});