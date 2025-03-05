var IsChanged = true;

var AddNewRow = "<tr class='HasItem'>"
    + "<td valign='top'></td>"
    + "<td valign='top'>"
    + "<input type='text' class='txtProductName' onkeydown='SearchKeyDown(this, event)' onkeyup='SearchKeyUp(this, event)' /><div class='tt-menu'> <div class='tt-dataset tt-dataset-autocomplete'> </div> </div><span class='spnProductName'></span>"
    + "</td>"
    + "<td valign='top'>"
    + "<input type='text' class='txtProductDesc' />"
    + "</td>"
    + "<td valign='top'>"
    + "<input type='text' id='EffectiveDate2' class='form-control EffectiveDate' placeholder='Effective Date' autocomplete='off' required />"
    + "</td>"
    + "<td valign='top'>"
    + "<input type='text' id='CycleStartDate2' class='form-control CycleStartDate' placeholder='Cycle Date' autocomplete='off' />"
    + "</td>"
    + "<td valign='top' class='retail_area'>"
    + "<input type='number' min='0' class='txtProductQuantity' value='' />"
    + "</td>"
    + "<td valign='top' class='retail_area'>"
    + "<div class='C_S I_G'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>" + Currency + "</div>"
    + "</div>"
    + "<input type='text' class='txtRetailPrice' value='' />"
    + "</div>"
    + "</td>"
    + "<td valign='top' class='retail_area'>"
    + "<div class='C_S I_G'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>" + Currency + "</div>"
    + "</div>"
    + "<input type='text' class='txtTotalRetailPrice' value='' />"
    + "</div>"
    + "</td>"
    + "<td valign='top' class='tableActions'>"
    + "<div class='estimate_action_div'>"
    + "<input type='checkbox' style='display:block;' title='Taxable' class='chkTaxable' />"
    + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
    + "</div>"
    + "</td>"
    + "</tr>";


var ClosingConfirmationMessage = function () {
    if (IsChanged) {
        OpenConfirmationMessageNew("Confirmation", "Do you want to leave? Changes you made may not be saved.", function () {
            CloseTopToBottomModal();
        });
    } else {
        CloseTopToBottomModal();
    }
}

$(".Recurring_BillingTab tbody").on('blur', 'tr td .txtRetailPrice', function (e) {
    var QTY = $(this).parent().parent().parent().find(".txtProductQuantity").val();
    var Rate = $(this).parent().parent().parent().find(".txtRetailPrice").val();
    var Amount = QTY * Rate;

    var TotalRetailPrice = $(this).parent().parent().parent().find('input.txtTotalRetailPrice');
    $(TotalRetailPrice).val(Amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    var BillingAmount = parseFloat('0');
    $(".txtTotalRetailPrice").each(function () {
        var amo = $(this).val().trim();
        amo = amo.replaceAll(',', '');

        var currAmount = parseFloat(amo);
        if (!isNaN(currAmount)) {
            BillingAmount += currAmount;
        }

    });

    var NonTaxBillAmount = parseFloat('0');
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {

            var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
            txamo = txamo.replaceAll(',', '');

            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }

    });
    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));

    var BillingPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerBillingAmount');
    $(BillingPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    if ($("#taxType").val() == 0 || $("#taxType").val() == "" || ForTax == 0) { //$(".CustomerTaxPercentage").val()
        var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
        var TaxAmount = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    else {
        var Price = parseFloat(ForTax); //$(".CustomerBillingAmount").text().replace(Currency, "");
        var Percent = 0;
        if ($("#taxType").val() == "Custom") {
            Percent = $(".CustomerTaxPercentage").val();
        }
        else {
            Percent = $("#taxType").val();
        }
        //var Percent = $("#taxType").val(); //$(".CustomerTaxPercentage").val();
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }

});
// Event listener for change/keyup of product quantity
$(".Recurring_BillingTab tbody").on("change keyup", ".txtProductQuantity", function () {
    var row = $(this).closest("tr"); 
    var quantity = parseFloat($(this).val().trim()) || 0; 
    var retailPrice = parseFloat(row.find(".txtRetailPrice").val().trim().replaceAll(',', '')) || 0;
    
    var totalRetailPrice = quantity * retailPrice;
    row.find(".txtTotalRetailPrice").val(totalRetailPrice.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
    
    updateBillingSummary();
});


$(".Recurring_BillingTab tbody").on('change keyup', "tr td .txtTotalRetailPrice", function (e) {
    var updatedValue = $(this).val().trim().replaceAll(',', '');
    var row = $(this).closest("tr");

 
    updateBillingSummary();
});


function updateBillingSummary() {
    var BillingAmount = 0;
    var NonTaxBillAmount = 0;


    $(".txtTotalRetailPrice").each(function () {
        var amo = $(this).val().trim().replaceAll(',', '');
        var currAmount = parseFloat(amo);
        if (!isNaN(currAmount)) {
            BillingAmount += currAmount;
        }
    });
    
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {
            var txamo = $(this).find(".txtTotalRetailPrice").val().trim().replaceAll(',', '');
            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }
    });
    
    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));
    
    var BillingPrice = $('span.CustomerBillingAmount');
    BillingPrice.text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    
    if ($("#taxType").val() == 0 || $("#taxType").val() == "" || ForTax == 0) {
      
        var TaxPercent = $('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
        var TaxAmount = $('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    } else {
    
        var Price = parseFloat(ForTax);
        var Percent = 0;
        if ($("#taxType").val() == "Custom") {
            Percent = $(".CustomerTaxPercentage").val();
        } else {
            Percent = $("#taxType").val();
        }

        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        
        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
}



$(".Recurring_BillingTab tbody").on('change', "tr td .txtRetailPrice", function (e) {
    var QTY = $(this).parent().parent().parent().find(".txtProductQuantity").val();
    var Rate = $(this).parent().parent().parent().find(".txtRetailPrice").val();
    var Amount = QTY * Rate;

    var TotalRetailPrice = $(this).parent().parent().parent().find('input.txtTotalRetailPrice');
    $(TotalRetailPrice).val(Amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    var BillingAmount = parseFloat('0');
    $(".txtTotalRetailPrice").each(function () {
        var amo = $(this).val().trim();
        amo = amo.replaceAll(',', '');

        var currAmount = parseFloat(amo);
        if (!isNaN(currAmount)) {
            BillingAmount += currAmount;
        }

    });

    var NonTaxBillAmount = parseFloat('0');
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {

            var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
            txamo = txamo.replaceAll(',', '');

            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }

    });

    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));

    var BillingPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerBillingAmount');
    $(BillingPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    if ($("#taxType").val() == 0 || $("#taxType").val() == "" || ForTax == 0) { //$(".CustomerTaxPercentage").val()
        var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
        var TaxAmount = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    else {
        var Price = parseFloat(ForTax); //$(".CustomerBillingAmount").text().replace(Currency, "");
        var Percent = 0;
        if ($("#taxType").val() == "Custom") {
            Percent = $(".CustomerTaxPercentage").val();
        }
        else {
            Percent = $("#taxType").val();
        }
        //var Percent = $("#taxType").val(); //$(".CustomerTaxPercentage").val();
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }

});

$(".Recurring_BillingTab tbody").on('change', "tr td .txtProductQuantity", function (e) {
    var QTY = $(this).parent().parent().find(".txtProductQuantity").val();
    var Rate = $(this).parent().parent().find(".txtRetailPrice").val();
    var Amount = QTY * Rate;
    if (QTY != 0 && Rate != "") {
        var TotalRetailPrice = $(this).parent().parent().find('input.txtTotalRetailPrice');
        $(TotalRetailPrice).val(Amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }


    var BillingAmount = parseFloat('0');
    $(".txtTotalRetailPrice").each(function () {
        var amo = $(this).val().trim();
        amo = amo.replaceAll(',', '');

        var currAmount = parseFloat(amo);
        if (!isNaN(currAmount)) {
            BillingAmount += currAmount;
        }

    });

    var NonTaxBillAmount = parseFloat('0');
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {

            var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
            txamo = txamo.replaceAll(',', '');

            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }

    });

    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));

    var BillingPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerBillingAmount');
    $(BillingPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

    if ($("#taxType").val() == 0 || $("#taxType").val() == "" || ForTax == 0) { //$(".CustomerTaxPercentage").val()
        var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
        var TaxAmount = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    else {
        var Price = parseFloat(ForTax); //$(".CustomerBillingAmount").text().replace(Currency, "");
        var Percent = 0;
        if ($("#taxType").val() == "Custom") {
            Percent = $(".CustomerTaxPercentage").val();
        }
        else {
            Percent = $("#taxType").val();
        }
        //var Percent = $("#taxType").val(); //$(".CustomerTaxPercentage").val();
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }

});

$("#CustomerTaxPercentage").keyup(function () {
    var Amount = $(".CustomerBillingAmount").text().replace(Currency, "");
    var BillingAmount = Amount.replaceAll(',', '');
    var NonTaxBillAmount = parseFloat('0');
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {

            var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
            txamo = txamo.replaceAll(',', '');

            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }

    });

    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));
    var TaxPercent = 0;
    if ($("#taxType").val() == "Custom") {
        TaxPercent = $(".CustomerTaxPercentage").val();
    }
    else {
        TaxPercent = $("#taxType").val();
    }
    if (ForTax > 0 && TaxPercent > 0) {
        var Price = ForTax; //$(".CustomerBillingAmount").text().replace(Currency, "");
        var Percent = TaxPercent;
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    else {
        //var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        //$(TaxPercent).val(0);
        var TaxAmount = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(BillingAmount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
});
$("#taxType").on('change', function (e) {
    if ($("#taxType").val() == "Custom") {
        $(".CustomerTaxPercentageDiv").removeClass('hidden');
        var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
    }
    else {
        $(".CustomerTaxPercentageDiv").addClass('hidden');
    }
    var Amount = $(".CustomerBillingAmount").text().replace(Currency, "");
    var BillingAmount = Amount.replaceAll(',', '');
    var NonTaxBillAmount = parseFloat('0');
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {

            var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
            txamo = txamo.replaceAll(',', '');

            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }

    });

    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));
    var TaxPercent = 0;
    if ($("#taxType").val() == "Custom") {
        TaxPercent = $(".CustomerTaxPercentage").val();
    }
    else {
        TaxPercent = $("#taxType").val();
    }
    if (ForTax > 0 && TaxPercent > 0) {
        var Price = ForTax; //$(".CustomerBillingAmount").text().replace(Currency, "");
        var Percent = TaxPercent;
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    else {
        var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
        var TaxAmount = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(BillingAmount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }

});
/*Taxable CheckBox*/
$(".Recurring_BillingTab tbody").on('click', 'tr td .chkTaxable', function (e) {
    var Amount = $(".CustomerBillingAmount").text().replace(Currency, "");
    var BillingAmount = Amount.replaceAll(',', '');
    var NonTaxBillAmount = parseFloat('0');
    $(".HasItem").each(function () {
        if ($(this).find('.chkTaxable').is(':checked') == false) {

            var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
            txamo = txamo.replaceAll(',', '');

            var txAmount = parseFloat(txamo);
            if (!isNaN(txAmount)) {
                NonTaxBillAmount += txAmount;
            }
        }

    });

    var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));
    if ($("#taxType").val() == "Custom") {
        TaxPercent = $(".CustomerTaxPercentage").val();
    }
    else {
        TaxPercent = $("#taxType").val();
    }
    if (ForTax > 0 && TaxPercent > 0) {
        var Price = ForTax; //$(".CustomerBillingAmount").text().replace(Currency, "");
        var Percent = TaxPercent;
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    else {
        var TaxPercent = $(this).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
        $(TaxPercent).val(0);
        var TaxAmount = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.CustomerTaxAmount');
        $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        var TotalPrice = $(this).parent().parent().parent().parent().parent().parent().parent().find('span.TotalBillingAmount');
        $(TotalPrice).text(Currency + parseFloat(BillingAmount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
 

});
/*Calculate After Remove*/
$(".Recurring_BillingTab tbody").on('click', 'tr td .fa', function (e) {

    var row = $(this).parent().parent().parent();
    OpenConfirmationMessageNew("Delete!", "Are you sure?", function () {
        
        row.find(".txtProductName").val('');
        row.find(".txtProductDesc").val('');
        row.find(".txtProductQuantity").val('');
        row.find(".txtRetailPrice").val('');
        row.find(".txtTotalRetailPrice").val('');
        row.find(".EffectiveDate").val('');
        row.find(".chkTaxable").prop('checked', false);
        
        row.remove();
        
        var BillingAmount = parseFloat('0');
        $(".txtTotalRetailPrice").each(function () {
            var amo = $(this).val().trim();
            amo = amo.replaceAll(',', '');
            var currAmount = parseFloat(amo);
            if (!isNaN(currAmount)) {
                BillingAmount += currAmount;
            }
        });

        var NonTaxBillAmount = parseFloat('0');
        $(".HasItem").each(function () {
            if ($(this).find('.chkTaxable').is(':checked') == false) {
                var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
                txamo = txamo.replaceAll(',', '');
                var txAmount = parseFloat(txamo);
                if (!isNaN(txAmount)) {
                    NonTaxBillAmount += txAmount;
                }
            }
        });

        var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));
        
        var BillingPrice = $(".CustomerBillingAmount");
        $(BillingPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var Price = ForTax;
        var Percent = 0;
        if ($("#taxType").val() == "Custom") {
            Percent = $(".CustomerTaxPercentage").val();
        } else {
            Percent = $("#taxType").val();
        }
        var TaxAmount = (Price * Percent) / 100;
        var TaxPrice = $(".CustomerTaxAmount");
        $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        
        if ($(".CustomerTaxPercentage").val() == 0 || $(".CustomerTaxPercentage").val() == "") {
            var TotalPrice = $(".TotalBillingAmount");
            $(TotalPrice).text(Currency + parseFloat(BillingAmount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        } else {
            var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
            var TotalPrice = $(".TotalBillingAmount");
            $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        
        if ($(".Recurring_BillingTab tbody tr").length < 2) {
            $("#Recurring_BillingTab tbody tr:last").after(AddNewRow);
        }

       
        var i = 1;
        $(".Recurring_BillingTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });

      
        initializeDatePickers();
        initializeDatePickersCycle();

    });
});


var SaveRecurringBilling = function () {
    if ($(".txtProductName").val() == null || $(".txtProductName").val() == "") {
        OpenErrorMessageNew("Error!", "You have to select at least one product/service to proceed", function () { });
        return;
    }
    var AdvanceDay = parseInt($("#Advance").val());
    if (isNaN(AdvanceDay) == true) {
        OpenErrorMessageNew("Error!", "Please enter a valid numeric value in advance days", function () { });
        return;
    }
    else {
        if (AdvanceDay < 16 && AdvanceDay > -1) { }
        else {
            OpenErrorMessageNew("Error!", "Advance days allow maximum value is 15", function () { });
            return;
        }
    }
    if ($("#Advance").val() == null || $("#Advance").val() == "" || $("#Advance").val() == 'undefined') {
        OpenErrorMessageNew("Error!", "Please choose start date", function () { });
        return;
    }
    //if ($("#EndDate").val() != null) {
    //    var startDate = new Date($("#StartDate").val());
    //    var endDate = new Date($("#EndDate").val());
    //    if (startDate >= endDate) {
    //        OpenErrorMessageNew("Error!", "End date must be greater than start date", function () { });
    //        return;
    //    }
    //}

    if (!validateEmail($("#EmailAddress").val())) {
        OpenErrorMessageNew("Error!", "Email address is required", function () { });
        return;
    }
    if ($("#BillCycleVal").val() == "-1") {
        OpenErrorMessageNew("Error!", "Please select a interval period", function () { });
        return;
    }
    if ($("#Month").val() < 1) {
        OpenErrorMessageNew("Error!", "Interval month must be greater than 0", function () { });
        return;
    }
    var TaxPercent = 0;
    if ($("#taxType").val() == "Custom") {
        TaxPercent = $(".CustomerTaxPercentage").val();
    }
    else {
        TaxPercent = $("#taxType").val();
    }
    var ScheduleList = [];
    if ($(this).find('.txtProductName').val() != "") {
        $(".HasItem").each(function () {
            $(this).find('.txtTotalRetailPrice').val($(this).find('.txtTotalRetailPrice').val().replaceAll(',', ''));
        });
        $(".HasItem").each(function () {
            var effectiveDate = $(this).find('.EffectiveDate').val(); 
            var cycleStartDate = $(this).find('.CycleStartDate').val(); 
            ScheduleList.push({
                ScheduleId: $("#ScheduleGuidId").val(),
                ProductName: $(this).find('.txtProductName').val(),
                Description: $(this).find('.txtProductDesc').val(),
                Qty: $(this).find('.txtProductQuantity').val(),
                EffectiveDate: effectiveDate,
                CycleStartDate: cycleStartDate,
                Rate: $(this).find('.txtRetailPrice').val(),
                Amount: $(this).find('.txtTotalRetailPrice').val().replaceAll(',', ''),
                IsTaxable: $(this).find('.chkTaxable').prop("checked")
            });
        });
    }

    var url = domainurl + "/RecurringBilling/AddRecurringBilling";
    var param = JSON.stringify({
        "Schedule.Id": $("#RecurringIntId").val(),
        "Schedule.ScheduleId": $("#ScheduleGuidId").val(),
        "Schedule.CustomerId": $("#CustomerId").val(),
        "Schedule.TemplateName": $("#TemplateName").val(),
        "Schedule.DayInAdvance": $("#Advance").val(),
        "Schedule.EmailAddress": $("#EmailAddress").val(),
        "Schedule.CCEmail": $("#CCEmail").val(),
        "Schedule.BCCEmail": $("#BCCEmail").val(),
        "Schedule.BillingAddress": $("#BillingAddress").val(),
        "Schedule.StartDate": $("#StartDate").val(),
        //"Schedule.NextDate": $("#NextDate").val(),
        "Schedule.EndDate": $("#EndDate").val(),
        "Schedule.PaymentCollectionDate": $("#PaymentCollectionDate").val(),
        "Schedule.AutomaticallySendEmail": $("#PaperlessBilling").prop("checked"),
        "Schedule.IsEInvoice": $("#e-Invoice").prop("checked"),
        "Schedule.IsEReceipt": $("#e-Receipt").prop("checked"),
        "Schedule.IncludeOpenInvoices": $("#UnbilledCharges").prop("checked"),
        "Schedule.OthersUnpaidBill": $("#OthersUnbilledCharges").prop("checked"),
        "Schedule.CustomerPaymentProfileId": $("#CustomerPaymentProfileIdVal").val(),
        "Schedule.PaymentMethod": $("#CustomerPaymentProfileIdVal").find("option:selected").text(),
        "Schedule.Status": $("#BillingStatus").val(),
        "Schedule.BillCycle": $("#BillCycleVal").val(),
        "Schedule.BillAmount": $("#CustomerBillingAmount").text().replace(Currency, "").replaceAll(',', ''),
        "Schedule.MessageOnInvoice": $("#RecurringDescription").val(),
        "Schedule.TaxPercentage": TaxPercent,//$("#taxType").val(),
        "Schedule.TaxAmount": $("#CustomerTaxAmount").text().replace(Currency, "").replaceAll(',', ''),
        "Schedule.TotalBillAmount": $("#TotalBillingAmount").text().replace(Currency, "").replaceAll(',', ''),
        ScheduleItems: ScheduleList
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
            if (data.result) {
                CloseTopToBottomModal();
                OpenSuccessMessageNew("Success!", data.message, function () {
                    location.reload();
                });
            }
            else {
                OpenErrorMessageNew("Error!", data.message, function () { });
                return;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

$(".BillingSaveButton").click(function () {
    var billDate = parseInt($("#BillDay").val()),
        starttimestamp = Date.parse($("#StartDate").val()),
      
        formatdate = null,
        dayVal = 1,
        monthValue = '01';
    if (isNaN(starttimestamp) == false) {
        var timestamp = Date.parse($("#PaymentCollectionDate").val());
        if (isNaN(timestamp) == true) { timestamp = new Date(); }
        formatdate = new Date(timestamp);
        var selectedMonth = parseInt(formatdate.getMonth()) + 1;
        if (selectedMonth < 10) { monthValue = '0' + selectedMonth; }
        else { monthValue = selectedMonth; }
        if (isNaN(billDate)) { billDate = 1; }
        if (billDate > 0 && billDate < 29) {
            if (billDate < 10) { dayVal = String('0' + billDate); }
            else { dayVal = String(billDate); }
        }
        formatdate = formatdate.getFullYear() + "-" + monthValue + "-" + dayVal;
        $("#PaymentCollectionDate").val(formatdate);
    }
    else {
        OpenErrorMessageNew("Error!", "Please set a start  date to proceed.", function () { });
        return;
    }
    var allDatesValid = true;

    $(".CycleStartDate").each(function () {
        var effectiveDateValue = $(this).val();
        var productNameValue = $(this).closest("tr").find(".txtProductName").val().trim();

       
        if (productNameValue.length > 0) {
            if (!effectiveDateValue) {
                allDatesValid = false;
                return false;
            }
            
            if (isNaN(Date.parse(effectiveDateValue))) {
                allDatesValid = false;
                return false;
            }
        }
    });

    if (!allDatesValid) {
        OpenErrorMessageNew("Error!", "Please set a valid cycle date for rows with product names to proceed.", function () { });
        return;
    }
    SaveRecurringBilling();
});


/*Add Row*/
$(".AddRow").click(function () {
    var currentDate = new Date();
    var month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
    var day = currentDate.getDate().toString().padStart(2, '0');
    var year = currentDate.getFullYear();

    var formattedDate = month + '/' + day + '/' + year;

    var newRow = $(AddNewRow);
    newRow.find('#EffectiveDate2').val('');

    $("#Recurring_BillingTab tbody tr:last").after(newRow);

    var i = 1;
    $(".Recurring_BillingTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });

  
    newRow.find('.EffectiveDate').each(function (index, element) {
        var existingDate = $(element).val();
        var defaultDate = existingDate ? new Date(existingDate) : new Date();

     
        var pikadayInstance = new Pikaday({
            field: element,
            format: 'MM/DD/YYYY',
            minDate: new Date(stDate),
            firstDay: 1,
            defaultDate: defaultDate,
            setDefaultDate: false
        });

       
        $(element)[0]._pikaday = pikadayInstance;
    });

    newRow.find('.CycleStartDate').each(function (index, element) {
        var existingDate = $(element).val();
        var defaultDate = existingDate ? new Date(existingDate) : new Date();


        var pikadayInstance = new Pikaday({
            field: element,
            format: 'MM/DD/YYYY',
            minDate: new Date(stDate),
            firstDay: 1,
            defaultDate: defaultDate,
            setDefaultDate: false
        });


        $(element)[0]._pikaday = pikadayInstance;
    });
   
});


function initializeDatePickers() {
    $('.EffectiveDate').each(function (index, element) {
     
        if (!$(element)[0]._pikaday) {
            var existingDate = $(element).val();
            var defaultDate = existingDate ? new Date(existingDate) : null;

       
            var pikadayInstance = new Pikaday({
                field: element,
                format: 'MM/DD/YYYY',
                minDate: new Date(stDate),
                firstDay: 1,
                defaultDate: defaultDate,
                setDefaultDate: !!defaultDate,
                onSelect: function (date) {
                    
                    $(element).val(pikadayInstance.toString());
                }
            });

            
            $(element)[0]._pikaday = pikadayInstance;
        }
    });
}
function initializeDatePickersCycle() {
    $('.CycleStartDate').each(function (index, element) {

        if (!$(element)[0]._pikaday) {
            var existingDate = $(element).val();
            var defaultDate = existingDate ? new Date(existingDate) : null;


            var pikadayInstance = new Pikaday({
                field: element,
                format: 'MM/DD/YYYY',
                firstDay: 1,
                minDate: new Date(stDate),
                defaultDate: defaultDate,
                setDefaultDate: !!defaultDate,
                onSelect: function (date) {

                    $(element).val(pikadayInstance.toString());
                }
            });


            $(element)[0]._pikaday = pikadayInstance;
        }
    });
}


/*Remove Row*/
//$(".Recurring_BillingTab tbody").on('click', 'tr td i.fa', function (e) {

//    $(this).parent().parent().parent().remove();

//    if ($(".Recurring_BillingTab tbody tr").length < 2) {

//        $("#Recurring_BillingTab tbody tr:last").after(AddNewRow);
//    }
//    var i = 1;
//    $(".Recurring_BillingTab tbody tr td:first-child").each(function () {
//        $(this).text(i);
//        i += 1;
//    });

//    initializeDatePickers();
//    initializeDatePickersCycle();

//}); 




var InvoiceEqSuggestionclickbind = function (item) {
    $('.Recurring_BillingTab .tt-suggestion').click(function () {
        var clickitem = this;
        $('.Recurring_BillingTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var txtItemdesc = $(item).parent().parent().find('.txtProductDesc');
        $(txtItemdesc).val($(this).attr('data-description'));
        var txtItemRate = $(item).parent().parent().find('.txtRetailPrice');
        $(txtItemRate).val($(this).attr('data-price'));
        var txtQtyRate = $(item).parent().parent().find('.txtProductQuantity');
        $(txtQtyRate).val(1);
        var txtAmountRate = $(item).parent().parent().find('.txtTotalRetailPrice');
        $(txtAmountRate).val($(this).attr('data-price'));
        var IsTaxableItem = $(this).attr('data-taxable');
        var chkItemTaxable = $(item).parent().parent().find('.chkTaxable');
        $(chkItemTaxable).prop('checked', IsTaxableItem);

        var QTY = $(txtQtyRate).val();
        var Rate = $(txtItemRate).val();
        var Amount = QTY * Rate;
        $(txtAmountRate).val(Amount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var BillingAmount = parseFloat('0');
        $(".txtTotalRetailPrice").each(function () {
            var amo = $(this).val().trim();
            amo = amo.replaceAll(',', '');

            var currAmount = parseFloat(amo);
            if (!isNaN(currAmount)) {
                BillingAmount += currAmount;
            }

        });
     
        var NonTaxBillAmount = parseFloat('0');
        $(".HasItem").each(function () {
            if ($(this).find('.chkTaxable').is(':checked') == false) {

                var txamo = $(this).find(".txtTotalRetailPrice").val().trim();
                txamo = txamo.replaceAll(',', '');

                var txAmount = parseFloat(txamo);
                if (!isNaN(txAmount)) {
                    NonTaxBillAmount += txAmount;
                }
            }

        });
        
        var ForTax = Math.abs(Number(BillingAmount) - Number(NonTaxBillAmount));
        var BillingPrice = $(item).parent().parent().parent().parent().parent().parent().parent().find('.InvoiceCalculationsDiv span.CustomerBillingAmount');
        $(BillingPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        if ($("#taxType").val() == 0 || $("#taxType").val() == "" || ForTax == 0) { //$(".CustomerTaxPercentage").val()
            var TaxPercent = $(item).parent().parent().parent().parent().parent().parent().parent().find('input.CustomerTaxPercentage');
            $(TaxPercent).val(0);
            var TaxAmount = $(item).parent().parent().parent().parent().parent().parent().parent().find('.InvoiceCalculationsDiv span.CustomerTaxAmount');
            $(TaxAmount).text(Currency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            var TotalPrice = $(item).parent().parent().parent().parent().parent().parent().parent().find('.InvoiceCalculationsDiv span.TotalBillingAmount');
            $(TotalPrice).text(Currency + BillingAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        else {
            var Price = parseFloat(ForTax); //$(".CustomerBillingAmount").text().replace(Currency, "");
            var Percent = 0;
            if ($("#taxType").val() == "Custom") {
                Percent = $(".CustomerTaxPercentage").val();
            }
            else {
                Percent = $("#taxType").val();
            }
            //var Percent = $("#taxType").val(); //$(".CustomerTaxPercentage").val();
            var TaxAmount = (Price * Percent) / 100;
            var TaxPrice = $(item).parent().parent().parent().parent().parent().parent().parent().find('.InvoiceCalculationsDiv span.CustomerTaxAmount');
            $(TaxPrice).text(Currency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            var TotalBilling = Number(BillingAmount) + Number(TaxAmount);
            var TotalPrice = $(item).parent().parent().parent().parent().parent().parent().parent().find('.InvoiceCalculationsDiv span.TotalBillingAmount');
            $(TotalPrice).text(Currency + parseFloat(TotalBilling).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        }
        var currentDate = new Date().toLocaleDateString("en-US");
        var ToDate = new Date();
        var starttimestamp = Date.parse($("#StartDate").val());
        if (isNaN(starttimestamp)) { starttimestamp = ToDate; }
        starttimestamp = new Date(starttimestamp);
        if (starttimestamp > ToDate) {
            currentDate = starttimestamp.toLocaleDateString("en-US");
        }
        $(item).parent().parent().find(".EffectiveDate").val(currentDate);
        $(item).parent().parent().find(".CycleStartDate").val(currentDate);
    });
    $('.Recurring_BillingTab .tt-suggestion').hover(function () {
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
        url: domainurl + "/RecurringBilling/GetOnlyRMRServiceListByKey",
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

                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType,
                       /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                        /*7*/resultparse[i].ManufacturerName.replaceAll('"', '\'\''),/* ImageSource*/
                        /*8*/resultparse[i].IsTaxable,
                             /*9*/resultparse[i].SKU,
                             /*10*/resultparse[i].Equipmentvendorcost);

                    var IsTaxableVal = resultparse[i].IsTaxable;
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
        GetProductByBarCode(item, $(item).val());
        $(".NewProjectSuggestion").hide();
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
            if ($(event.target).hasClass('txtProductName')) {
                $($(trselected).next('tr')).find('input.txtProductName').focus();
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
            $($(trselected).prev('tr')).find('input.txtProductName').focus();
        }
    }

}

var PropertyUserSuggestiontemplate =
    '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-taxable="{8}" data-description="{6}" data-equipvendorcost="{10}">'
    /*
    *For Equipment Image
    *+ "<img src='{7}' class='EquipmentImage'>"*/
    + "<p class='tt-sug-text'>"
    + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{7}</em>"
    + "<em class='tt-eq-price'>${2}</em>"
    + "<br />"
    + "</p> "
    + "</div>";

var validateEmail = function (email) {
    var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }
}