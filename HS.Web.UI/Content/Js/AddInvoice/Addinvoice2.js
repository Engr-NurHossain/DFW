var TotalAmount = 0; /*TotalAmount: Invoice Table -> Amount*/
var FinalTotal = 0;/*FinalTotal: Invoice Table -> TotalAmount*/
var NonTaxValue = 0;
var DiscountAmount = 0;
var ShippingAmount = 0;
var DepositAmount = 0;
var BalanceDue = 0;
var DiscountDBPercent = 0;
var DiscountDBAmount = 0;
var TaxAmount = 0;
var TPVal = "0.00";
var SendEmailUrl = "";
var mailAdd = "";
var InvoiceDatepicker;
var DueDatepicker;
var shippingDatePicker;
var Fdiscountamount = 0;
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var GetTimeFormat = function (date) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return new Date(date + ' ' + time)
}
var CustomerSuggestiontemplate =
    '<div class="tt-suggestion tt-selectable" data-address="{0}" data-address1="{1}" data-street="{2}" data-street1="{3}" data-city="{4}" data-city1="{5}" data-state="{6}" data-state1="{7}" data-zipcode="{8}" data-zipcode1="{9}" data-Bussiness ="{10}" data-firstName="{11}" data-lastName="{12}" data-emailAddress="{13}" data-customerId="{14}" data-type="{15}" >'

    + "<p class='tt-sug-text'>"
    + "{16}"
    + " <em class='tt-eq-price'>{6}</em>"
    + "</p> "
    + "</div>";
var PropertyUserSuggestiontemplate =
    '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-taxable="{8}" data-description="{6}" data-equipvendorcost="{10}">'
    /*
    *For Equipment Image
    *+ "<img src='{7}' class='EquipmentImage'>"*/
    + "<p class='tt-sug-text'>"
    + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{7}</em>"
    + "<em class='tt-eq-sku'></br>SKU: {9}</em>"
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
if (ShowInvoiceEquipmentCost == "True") {
    NewEquipmentRow += "<td valign='top'>"
        + "<p class='spnProductEquipmentvendorcost'></p>"
        + "</td>";
}
NewEquipmentRow += "<td valign='top'>"
    + "<div class='C_S I_G'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>" + Currency + "</div>"
    + "</div>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductRate' />"
    + "</div>"
    + "<span class='spnProductRate'></span>"
    + "</td>";
if (ShowInvoiceDetailsLineItemDiscountValue == "True") {
    NewEquipmentRow += "<td valign='top'>"
        + "<p class='spnProductItemDiscountAmount'></p>"
        + "</td>";
}
NewEquipmentRow += "<td valign='top'>"
    + "<div class='C_S I_G'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>" + Currency + "</div>"
    + "</div>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
    + "</div>"
    //+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
    + "<span class='spnProductAmount'></span>"
    + "</td>"
    + "<td valign='top' class='tableActions'>"
    + "<div class='invoice_action_div'>"
    + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
    + "</div>"
    + "</td>"
    + "</tr>";

var NewEquipmentRowTaxable = "<tr>"
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
if (ShowInvoiceEquipmentCost == "True") {
    NewEquipmentRowTaxable += "<td valign='top'>"
        + "<p class='spnProductEquipmentvendorcost'></p>"
        + "</td>";
}

NewEquipmentRowTaxable += "<td valign='top'>"
    + "<div class='C_S I_G'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>" + Currency + "</div>"
    + "</div>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductRate' />"
    + "</div>"
    + "<span class='spnProductRate'></span>"
    + "</td>";
if (ShowInvoiceDetailsLineItemDiscountValue == "True") {
    NewEquipmentRowTaxable += "<td valign='top'>"
        + "<p class='spnProductItemDiscountAmount'></p>"
        + "</td>";
}
NewEquipmentRowTaxable +=  "<td valign='top'>"
    + "<div class='C_S I_G'>"
    + "<div class='input-group-prepend'>"
    + "<div class='input-group-text'>" + Currency + "</div>"
    + "</div>"
    + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
    + "</div>"
    //+ "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
    + "<span class='spnProductAmount'></span>"
    + "</td>"
    + "<td valign='top' class='tableActions'>"
    + "<div class='invoice_action_div'>"
    + "<input type='checkbox' style='display:block;' onkeydown='OthersKeyDown(this,event)' class='chkTaxable' />"
    + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
    + "</div>"
    + "</td>"
    + "</tr>";

var AddInvoice = {
    OnPageDocready: function (){
        var flag = 0;
        if (ShowShipping == "False") {

            $(".shipping").hide();
            $("#ShippingField").hide();
        }
        else {
            flag = 1;
            $(".shipping").show();
            $("#ShippingField").show();
        }
        if ('' == "False") {
            $(".Discount-total").addClass('hidden');

        }
        else {
            $(".Discount-total").removeClass('hidden');

        }
        if (ShowDeposit != "False") {
            flag = 1;
        }

        if (flag == 0) {
            $(".total-amount-div").hide();
        }
        else if (flag = 1) {
            $(".total-amount-div").show();
        }
        var Popupwidth = 920;
        if (window.innerWidth < 920) {
            Popupwidth = window.innerWidth;
        }
        var idlist = [{ id: ".InvEstPreviewPartial", type: 'iframe', width: Popupwidth, height: 600 }
        ];
        jQuery.each(idlist, function (i, val) {
            magnificPopupObj(val);
        });

        if (InvoiceFor == "LaborFee") {
            $("#taxType").val("0");
        }
        $("#Invoice_Terms").change(function () {
            var NewDueDate = new Date($("#Invoice_InvoiceDate").val());
            if (NewDueDate == "Invalid Date") {
                NewDueDate = new Date();
                $("#Invoice_InvoiceDate").val(NewDueDate.getMonth() + 1 + "/" + NewDueDate.getDate() + "/" + NewDueDate.getFullYear());
            }
            NewDueDate = NewDueDate.addDays(parseInt($("#Invoice_Terms").val()));
            NewDueDate = NewDueDate.getMonth() + 1 + "/" + NewDueDate.getDate() + "/" + NewDueDate.getFullYear();
            $(".DueDate").val(NewDueDate);
        });
        if (InvoiceStatus == "Undefined" || InvoiceStatus == "Init") {
            $(".invoice-make-payment-div").hide();
            $(".balance-info-due").show();
            $(".balance-info-paid").hide();
            $(".balance-info-Cancel").hide();
        }
        else if (InvoiceStatus == "Open") {
            $(".invoice-make-payment-div").show();
            $(".balance-info-paid").hide();
            $(".balance-info-due").show();
            $(".balance-info-Cancel").hide();
        }
        else if (InvoiceStatus == "Partial") {
            $(".InvoiceSaveButton").hide();
            $(".AddNewInvNotBtn").hide();
            $(".balance-info-paid").hide();
            $(".balance-info-Cancel").hide();
        }
        else if (InvoiceStatus == "Paid") {
            $(".invoice-make-payment-div").hide();
            $(".balance-info-due").hide();
            $(".balance-info-paid").show();
            $(".InvoiceSaveButton").hide();
            $(".AddNewInvNotBtn").hide();
            $(".balance-info-Cancel").hide();
        }
        else if (InvoiceStatus == "Cancelled"
            || InvoiceStatus == "Cancel"
            || InvoiceStatus == "Declined"
            || InvoiceStatus == "Rolled Over") {
            $(".invoice-make-payment-div").hide();
            $(".balance-info-due").hide();
            $(".balance-info-paid").hide();
            $(".InvoiceSaveButton").hide();
            $(".AddNewInvNotBtn").hide();
            $(".balance-info-Cancel").show();
        }
        $(".CheckPrintAndPreview").click(function () {
            if ($(".HasItem").length == 0) {
                OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
            }
            else {
                $("#InvoicePrintAndPreview").click();
            }
        });
        $(".CloneInvoiceBtn").click(function () {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to make a duplicate copy of this Invoice?", function () {
                CloneDeclinedInvoice(Invoice_int_Id);
            });
        });
        $(".Clone_Invoice").click(function () {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to make a copy of this Invoice for the selected Customer?", function () {
                var CusId = $("#InvoiceCustomerId").val();
                CloneSelectCustomerInvoice(Invoice_int_Id, CusId);
            });
        });
        $(".CancelReCreate").click(function () {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to cancel & delete all transactions regarding this Invoice and make a duplicate copy?", function () {
                CancelAndRecreateInvoice(Invoice_int_Id);
            });
        });
        
        $(".InvoiceDeleteButton").click(function () {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this invoice?", function () {
                DeleteInvoicebyId(Invoice_int_Id);
            });
        });
        /*Set DueDate and Invoice Date*/
        if ($("#Invoice_Terms").val() != '' && InvoiceStatus == "Init" && $("#Invoice_Terms").val() != "0") {
            console.log($("#Invoice_Terms").val());
            $("#Invoice_Terms").val(DefaultDueDate);
            var NewInvoiceDate = new Date($("#Invoice_InvoiceDate").val());
            if (NewInvoiceDate == "Invalid Date") {
                NewInvoiceDate = new Date();
                $("#Invoice_InvoiceDate").val(NewInvoiceDate.getMonth() + 1 + "/" + NewInvoiceDate.getDate() + "/" + NewInvoiceDate.getFullYear());
            }
            if (!isNaN($("#Invoice_Terms").val())) {
                NewInvoiceDate = NewInvoiceDate.addDays(parseInt($("#Invoice_Terms").val()));
            }
            $(".DueDate").val(NewInvoiceDate.getMonth() + 1 + "/" + NewInvoiceDate.getDate() + "/" + NewInvoiceDate.getFullYear());
        } else {
            console.log("text");
            if (DueDate != "" && DueDate != null) {
                $("#Invoice_DueDate").val(DueDate);

            }
            if (invDate != "" && invDate != null) {
                $("#Invoice_InvoiceDate").val(invDate);

            }
        }
    }
};
var CustomerSearchKeyDown = function (item, event) {
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
var CustomerSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Invoice/GetCustomerListByKey",
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
                    if (resultparse[i].Type == "Commercial") {
                        var name = resultparse[i].BusinessName;
                    }
                    else {
                        var name = resultparse[i].FirstName + ' ' + resultparse[i].LastName;
                    }

                    searchresultstring = searchresultstring + String.format(CustomerSuggestiontemplate,
                        resultparse[i].Address,/*0*/
                        resultparse[i].Address1,/*1*/
                        resultparse[i].Street, /*2*/
                        resultparse[i].Street1,/*3*/
                        resultparse[i].City,/*4*/
                        resultparse[i].City1,/*5*/
                        resultparse[i].State == "-1" ? "" : resultparse[i].State,/*6*/
                        resultparse[i].State1,/*7*/
                        resultparse[i].ZipCode,/*8*/
                        resultparse[i].ZipCode1,/*9*/
                        resultparse[i].BusinessName,/*10*/
                        resultparse[i].FirstName,/*11*/
                        resultparse[i].LastName,/*12*/
                        resultparse[i].EmailAddress,/*13*/
                        resultparse[i].CustomerId,/*14*/
                        resultparse[i].Type,/*15*/
                        name);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                InvoiceCustomerclickbind(item);
                if (resultparse.length > 5) {
                    $(".customer_name_insert_div .NewProjectSuggestion").height(200);
                    //$(".NewProjectSuggestion").css('position', 'relative');
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var DeleteInvoicebyId = function (InvDeleteId) {
    console.log("delete");
    var param = JSON.stringify({
        Id: InvDeleteId,
        //EmailAddress: EmailAddress,
       // "Invoice.InvoiceId": InvoiceId,
        InvoiceId: InvoiceId,

        //"Invoice.InvoiceEmailAddress": EmailAddress,
        //"Invoice.InvoiceCcEmailAddress": ccEmail,
        //"Invoice.BillingAddress": tinyMCE.get('Invoice_BillingAddress').getContent(),
        //"Invoice.Terms": $("#Invoice_Terms").val(),
        //"Invoice.ShippingAddress": tinyMCE.get('Invoice_ShippingAddress').getContent(),
        //"Invoice.ShippingAddress": tinyMCE.get('Invoice_ShippingAddress').getContent(),
        //"Invoice.ShippingVia": $("#Invoice_ShippingVia").val(),
        //"Invoice.ShippingDate": shippingDatePicker.getDate(),
        //"Invoice.ShippingCost": $("#Invoice_ShippingCost").val(),
        //"Invoice.TrackingNo": $("#Invoice_TrackingNo").val(),
       // "Invoice.CustomerId": $("#InvoiceCustomerId").val(),
        CustomerId: $("#InvoiceCustomerId").val(),
        //CusiD: CustomerId,
        //"Invoice.InvoiceDate": GetTimeFormat($("#Invoice_InvoiceDate").val()),
        //"Invoice.DueDate": GetTimeFormat($("#Invoice_DueDate").val()),
        //"Invoice.TotalAmount": FinalTotal.toFixed(2),
        //"Invoice.Amount": TotalAmount,
        //"Invoice.Message": $("#InvoiceMessage").val(),
        //"Invoice.Deposit": $("#Invoice_Deposit").val(),
        //"Invoice.DiscountAmount": DiscountDBAmount,
        //"Invoice.Discountpercent": DiscountDBPercent,
        //"Invoice.BalanceDue": BalanceDue.toFixed(2),
        //"Invoice.Tax": TaxAmount.toFixed(2),
        //"Invoice.TaxType": $("#taxType option:selected").text(),
        //"Invoice.Memo": memoval,
        //"Invoice.Description": $("#InvoiceDescription").val(),
        //"Invoice.DiscountType": $("#Invoice_DiscountType").val(),
        //"Invoice.JobNo": $("#JobNo").val(),
        //InvoiceDetailList: DetailList,
        //SendEmail: SendEmail,
        //CreatePdf: CreatePdf,
        //EmailDescription: EmailDescription,
        //EmailSubject: EmailSubject,
        //ccEmail: ccEmail
    });
    $.ajax({
        url: domainurl + "/Invoice/DeleteInvoice",
        data: {
            Id: InvDeleteId,
            InvoiceId: InvoiceId,
            CustomerId: CustomerId,

        },
        //data: param,

        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Invoice deleted successfully!");
                OpenInvoiceTab();
                CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

        }
    });
}
var CloneDeclinedInvoice = function (InvoiceId) {
    $.ajax({
        url: domainurl + "/Invoice/CloneDeclinedInvoice",
        data: { InvoiceId: InvoiceId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
                OpenInvoiceTab();
                CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

        }
    });
}
var CloneSelectCustomerInvoice = function (InvoiceId, CustomerId) {
    $.ajax({
        url: domainurl + "/Invoice/CloneCustomerInvoice",
        data: { InvoiceId: InvoiceId, CustomerId: CustomerId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
                OpenInvoiceTab();
                //CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

        }
    });
}
var CancelAndRecreateInvoice = function (InvoiceId) {
    $.ajax({
        url: domainurl + "/Invoice/CancelAndRecreateInvoice",
        data: { InvoiceId: InvoiceId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
                OpenInvoiceTab();
                CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

        }
    });
}
var InvoiceCustomerclickbind = function (item) {
    $('.customer_name_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.customer_name_insert_div .tt-menu').hide();

        var selectedEmail = $(clickitem).attr("data-emailAddress").trim();

        var BussiName = $(clickitem).attr("data-Bussiness").trim();
        var Customerfnum = $(clickitem).attr("data-firstName").trim();
        var Customerlnum = $(clickitem).attr("data-lastName").trim();
        var CustomerGuId = $(clickitem).attr("data-customerId").trim();
        var CustomerType = $(clickitem).attr("data-type").trim();
        console.log(CustomerType);
        if (CustomerType == "Commercial") {
            var displayname = BussiName;
        }
        else {
            var displayname = Customerfnum + " " + Customerlnum;
        }

        $("#CustomerList").val(displayname);
        $("#Invoice_InvoiceEmailAddress").val(selectedEmail);
        $("#InvoiceCustomerId").val(CustomerGuId);
        $.ajax({
            type: "POST",
            url: domainurl + "/Invoice/GetCustomerAddressByCustomerId",
            data: JSON.stringify({
                CustomerId: CustomerGuId
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result == true) {
                    tinyMCE.get('Invoice_BillingAddress').setContent(data.BillingAddressVal);
                    tinyMCE.get('Invoice_ShippingAddress').setContent(data.ShippingAddressVal);
                }
            }
        });
    });
    $('.customer_name_insert_div .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var OpenClosingConfirmationMessage = function () {
    if (IsChanged) {
        OpenConfirmationMessageNew("Confirmation", "Do you want to leave? Changes you made may not be saved.", function () {
            CloseTopToBottomModal();
        });
    } else {
        CloseTopToBottomModal();
    }
}

var CustomerSearchKeyDown = function (item, event) {
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

var CustomerSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Invoice/GetCustomerListByKey",
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
                    if (resultparse[i].Type == "Commercial") {
                        var name = resultparse[i].BusinessName;
                    }
                    else {
                        var name = resultparse[i].FirstName + ' ' + resultparse[i].LastName;
                    }

                    searchresultstring = searchresultstring + String.format(CustomerSuggestiontemplate,
                        resultparse[i].Address,/*0*/
                        resultparse[i].Address1,/*1*/
                        resultparse[i].Street, /*2*/
                        resultparse[i].Street1,/*3*/
                        resultparse[i].City,/*4*/
                        resultparse[i].City1,/*5*/
                        resultparse[i].State == "-1" ? "" : resultparse[i].State,/*6*/
                        resultparse[i].State1,/*7*/
                        resultparse[i].ZipCode,/*8*/
                        resultparse[i].ZipCode1,/*9*/
                        resultparse[i].BusinessName,/*10*/
                        resultparse[i].FirstName,/*11*/
                        resultparse[i].LastName,/*12*/
                        resultparse[i].EmailAddress,/*13*/
                        resultparse[i].CustomerId,/*14*/
                        resultparse[i].Type,/*15*/
                        name);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                InvoiceCustomerclickbind(item);
                if (resultparse.length > 5) {
                    $(".customer_name_insert_div .NewProjectSuggestion").height(200);
                    //$(".NewProjectSuggestion").css('position', 'relative');
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}


var SaveAndNew = function () {
    console.log("Test Test");
    if (CommonUiValidation()) {
        SaveInvoice(false, false, "others", null);
    }
    
    if ($(".HasItem").length != 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?customerid=" + customerId);
    }
}
var SaveAndClose = function () {
    if (CommonUiValidation()) {
        SaveInvoice(true, false, "others", null);
    }
    //if ($(".HasItem").length != 0) {
    //    CloseTopToBottomModal();
    //    OpenSuccessMessageNew("Success!", "Invoice Successfully Saved.", function () { OpenInvoiceTab() });
    //}
}
var SaveAndShare = function () {
    if (CommonUiValidation()) {
        SaveInvoice(false, true);
    }
}

var makeSendEmailUrl = function () {

    mailAdd = encodeURIComponent($("#Invoice_InvoiceEmailAddress").val());
    SendEmailUrl = domainurl + "/Invoice/SendEmailInvoice/?Id=" + Invoice_int_Id + "&EmailAddress=" + mailAdd;
    console.log("Email link created.:" + SendEmailUrl);

    $("#InvoiceInfoPrintAndSend").attr("href", SendEmailUrl);
}

var SaveAndSend = function () {

    makeSendEmailUrl();
    if (CommonUiValidation()) {
        SaveInvoice(false, true, "preview", null);
    }
    OpenInvoiceTab();
}
var InvoiceEqSuggestionclickbind = function (item) {
    $('.CustomerInvoiceTab .tt-suggestion').click(function () {
        console.log("Enter fired");
        var clickitem = this;
        $('.CustomerInvoiceTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));

        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

        var spnItemVendorCost = $(item).parent().parent().find('.spnProductEquipmentvendorcost');
        var spnProductVndorcostValue = $(this).attr('data-equipvendorcost');
        $(spnItemVendorCost).attr("data-equipvendorcost", spnProductVndorcostValue);
        $(spnItemVendorCost).text(TransCurrency + parseFloat(spnProductVndorcostValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemVendorCost).val(spnProductVndorcostValue);

        var spnItemDiscountCost = $(item).parent().parent().find('.spnProductItemDiscountAmount');
        $(spnItemDiscountCost).text(TransCurrency + parseFloat(0).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemDiscountCost).val(0);

        /*Item Rate Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductRate');
        $(spnItemRate).text(Currency + $(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductRate');
        $(txtItemRate).val($(this).attr('data-price'));
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
        /*Item Amount Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        $(spnItemRate).text(Currency + $(this).attr('data-price'));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val($(this).attr('data-price'));
 
        /*Taxable*/
        var IsTaxableItem = $(this).attr('data-taxable');
        console.log(item);
        var chkItemTaxable = $(item).parent().parent().find('.chkTaxable');
        console.log(IsTaxableItem);
        $(chkItemTaxable).prop('checked', IsTaxableItem);

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
                //console.log(IsTaxableVal);
                //var IsTaxableItem = IsTaxableVal;
                //var chkItemTaxable = $(item).parent().parent().find('.chkTaxable');
                //$(chkItemTaxable).prop('checked', IsTaxableItem);

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
var GetProductByBarCode = function (item, BRCode) {
    var url = domainurl + "/Invoice/GetEquipmentListByKey";
    var ExistEquipment = "";
    var ExistEquipmentInner = "";
    $(".HasItem").each(function () {
        ExistEquipmentInner += "'" + $(this).attr('data-id') + "',";
    });
    if (ExistEquipmentInner.length > 0) {
        ExistEquipmentInner = ExistEquipmentInner.slice(0, -1);
        ExistEquipment = "(" + ExistEquipmentInner + ")";
    }

    var Param = JSON.stringify({
        key: BRCode,
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".AgreementSummaryLoader").removeClass("hidden"),
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            var resultparse = JSON.parse(data.result);
            console.log(data.result);
            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                var EquipmentName = resultparse[0].EquipmentName.replaceAll('"', '\'\'');
                var EquipmentId = resultparse[0].EquipmentId;
                var RetailPrice = resultparse[0].RetailPrice;
                var Reorderpoint = resultparse[0].Reorderpoint;
                var QuantityAvailable = resultparse[0].QuantityAvailable;
                var EquipmentType = resultparse[0].EquipmentType;
                var EquipmentDescription = resultparse[0].EquipmentDescription;
                var ManufacturerName = resultparse[0].ManufacturerName;
                var IsTaxableVal = resultparse[0].IsTaxable;
                //for (var i = 0; i < resultparse.length; i++) {
                //    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                //        /*0*/resultparse[i].EquipmentId,
                //        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                //        /*2*/ resultparse[i].RetailPrice,
                //        /*3*/resultparse[i].Reorderpoint,
                //        /*4*/ resultparse[i].QuantityAvailable,
                //        /*5*/ resultparse[i].EquipmentType,
                //       /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                //        /*7*/resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/* ImageSource*/);
                //}
                var clickitem = this;
                $(item).parent().parent().addClass("HasItem");
                $(item).parent().parent().attr('data-id', EquipmentId);
                $(item).val(EquipmentName);
                $(item).attr('data-id', EquipmentId);
                var itemName = $(item).parent().find('span').text();
                $(itemName).text(EquipmentName);

                var spnItemRate = $(item).parent().parent().find('.spnProductRate');
                $(spnItemRate).text(Currency + RetailPrice);

                var txtItemRate = $(item).parent().parent().find('.txtProductRate');
                $(txtItemRate).val(RetailPrice);
                /*Item Description Set*/
                var spnDesc = $(item).parent().parent().find('.spnProductDesc');
                $(spnDesc).text(EquipmentDescription);

                var txtItemDesc = $(item).parent().parent().find('.txtProductDesc');
                $(txtItemDesc).val(EquipmentDescription);

                /*Item Quantity Set*/
                var spnItemQuantity = $(item).parent().parent().find('.spnProductQuantity');
                $(spnItemQuantity).text(1);
                var txtItemQuantity = $(item).parent().parent().find('.txtProductQuantity');
                $(txtItemQuantity).val(1);
                /*Item Amount Set*/
                var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
                $(spnItemRate).text(Currency + RetailPrice);
                var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
                $(txtItemRate).val(RetailPrice);
                console.log(IsTaxableVal);
                /*Taxable*/
                var IsTaxableItem = IsTaxableVal;
                var chkItemTaxable = $(item).parent().parent().find('.chkTaxable');
                $(chkItemTaxable).prop('checked', IsTaxableItem);

                $(item).parent().parent().removeClass("focusedItem");
                $(item).parent().parent().next().addClass("focusedItem");
                $(item).parent().parent().next().find('.ProductName').focus();
                if (TaxablePermit.toLowerCase() == "true") {
                    $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRowTaxable);
                }
                else {
                    $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
                }
                CalculateNewAmount();
                $(".NewProjectSuggestion").hide();
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);

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
        } else if ($(event.target).hasClass('txtProductRate')) {
            $($(trselected).next('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).next('tr')).find('input.txtProductAmount').focus();
        } else if ($(event.target).hasClass('chkTaxable')) {
            $($(trselected).next('tr')).find('input.chkTaxable').focus();
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
        } else if ($(event.target).hasClass('chkTaxable')) {
            $($(trselected).next('tr')).find('input.chkTaxable').focus();
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
var NewAmountCallCount = 0;
var CalculateNewAmount = function () {
    NewAmountCallCount++;
    if (NewAmountCallCount > 2) {
        console.log("calculate New Amount");
        IsChanged = true;
    }

    var amount = parseFloat('0');
    var nontaxamount = parseFloat('0');
    $(".txtProductAmount").each(function () {
        var _CalAmt = $(this).val().trim();
        _CalAmt = _CalAmt.replaceAll(',', '');

        var currAmount = parseFloat(_CalAmt);
        if (!isNaN(currAmount)) {
            amount += currAmount;
        }
    });
    //Non Tax amount calculate 
    if (TaxablePermit.toLowerCase() == "true") {
        $(".HasItem").each(function () {
            console.log("test");
            if ($(this).find('.chkTaxable').is(':checked') == false) {
                var _CalAmt = $(this).find('.txtProductAmount').val().trim();
                _CalAmt = _CalAmt.replaceAll(',', '');

                var currAmount = parseFloat(_CalAmt);
                if (!isNaN(currAmount)) {
                    nontaxamount += currAmount;
                }
            }

        });
    }
    amount = parseFloat(amount).toFixed(2);
    nontaxamount = parseFloat(nontaxamount).toFixed(2);
    amount1 = amount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    $(".amount").text(TransMakeCurrency + amount1);
    TotalAmount = amount;
    FinalTotal = amount;
    NonTaxValue = nontaxamount;
    BalanceDue = amount;
    if ($("#Invoice_DiscountType").val() == "percent") {
        var a = 0;
        var Fval = 0;
        var discountAmount = 0;
        if ($("#discountAmount").length > 0) {
            if ($("#discountAmount").val() == "") {
                discountAmount = 0;
            }
            else {
                discountAmount = $("#discountAmount").val();
            }
        }
        Fdiscountamount = TotalAmount - ((amount / 100) * discountAmount);
        if (discountAmount != "" && Fdiscountamount > 0) {
            var discountAmountPercent = parseFloat(discountAmount);
            DiscountDBPercent = discountAmountPercent;
            DiscountDBAmount = a;
            DiscountAmount = (amount / 100) * discountAmountPercent;
            FinalTotal = TotalAmount - DiscountAmount;
            BalanceDue = FinalTotal;
            $(".shippingAmountTxt").text(TransMakeCurrency + DiscountAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            if (DiscountAmount == 0) {
                $(".Discount-total").addClass('hidden');
            }
            else {
                $(".DiscountAmountTxt").text(TransMakeCurrency + Fdiscountamount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
        }
        else {
            $(".shippingAmountTxt").text(TransMakeCurrency + "0.00");
            $("#discountAmount").val("");
            $(".DiscountAmountTxt").text(TransMakeCurrency + "0.00");
        }
    }
    if ($("#Invoice_DiscountType").val() == "amount") {
        var a = 0;
        var discountAmount = 0;
        if ($("#discountAmount").length > 0) {
            if ($("#discountAmount").val() == "") {
                discountAmount = 0;
            }
            else {
                discountAmount = $("#discountAmount").val();
            }
        }
        if (discountAmount != "" && DiscountAmount < amount) {
            var discountAmountPercent = parseFloat(discountAmount);
            DiscountDBAmount = discountAmountPercent;
            DiscountDBPercent = a;
            DiscountAmount = discountAmountPercent;
            FinalTotal = TotalAmount - DiscountAmount;
            BalanceDue = FinalTotal;
            Fdiscountamount = amount - DiscountAmount;
            $(".shippingAmountTxt").text(TransMakeCurrency + DiscountAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            if (DiscountAmount == 0) {
                $(".Discount-total").addClass('hidden');
            }
            else {
                $(".DiscountAmountTxt").text(TransMakeCurrency + Fdiscountamount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
            /*$(".FinalTotalTxt").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            
            $(".balanceDueAmount").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));*/

        }
        else {
            $(".shippingAmountTxt").text(TransMakeCurrency + "0.00");
            $("#discountAmount").val("");
            $(".DiscountAmountTxt").text(TransMakeCurrency + "0.00");
        }
    }

    if ($("#taxType").val() != "") {
        CalculateTax();
    }
    /*
    if ($("#taxType").val() != "") {
        var TPVal = $("#taxType").val();
        var TPercent = parseFloat(TPVal);
        if ($("#discountAmount").val() != "") {
            TaxAmount = (Fdiscountamount / 100) * TPercent;
            $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }
        else {
            TaxAmount = (FinalTotal / 100) * TPercent;
            $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        }
        ////was commented before tax changes.
        ////FinalTotal = parseFloat(FinalTotal) + parseFloat(TaxAmount);
        ////$(".FinalTotalTxt").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        ////BalanceDue = FinalTotal;
        ////$(".balanceDueAmount").text("$" + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }*/
    if ($("#Invoice_ShippingCost").val() != "") {
        var shippingCostString = $("#Invoice_ShippingCost").val();
        ShippingAmount = parseFloat(shippingCostString);

    }
    var DA = 0;
    if ($("#Invoice_Deposit").val() != "") {
        DA = $("#Invoice_Deposit").val();
        if ($("#discountAmount").val() != "") {
            BalanceDue = Fdiscountamount - parseFloat(DA);
        }
        else {
            BalanceDue = FinalTotal - parseFloat(DA);
        }
        /* $(".balanceDueAmount").text("$" + BalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));*/
    }
    if ($("#discountAmount").val() != "") {
        BalanceDue = parseFloat(Fdiscountamount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount) - parseFloat(DA);
        FinalTotal = parseFloat(Fdiscountamount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount);
    }
    else {
        BalanceDue = parseFloat(TotalAmount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount) - parseFloat(DA);
        FinalTotal = parseFloat(TotalAmount) + parseFloat(TaxAmount) + parseFloat(ShippingAmount);
    } 
    
    $(".TotalAmount").text(TransMakeCurrency + (FinalTotal + ShippingAmount).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")); 
    $(".FinalTotalTxt").text(TransMakeCurrency + FinalTotal.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".balanceDueAmount").text(TransMakeCurrency + BalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    $(".amount-big").text(TransMakeCurrency + BalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
}

var CalculateTax = function () {
    var taxableamount = parseFloat('0');
    $(".txtProductAmount").each(function () {
        //if (!$(this).parent().parent().parent().hasClass("NonTaxable")) {
        var _CalAmt = $(this).val().trim();
        _CalAmt = _CalAmt.replaceAll(',', '');

        var currAmount = parseFloat(_CalAmt);
        if (!isNaN(currAmount)) {
            taxableamount += currAmount;
        }
        //}
    });

    if ($("#taxType").val() == "Custom") {
        TPVal = $(".tax_val").val();
    }
    else {
        TPVal = $("#taxType").val();
    }
    var TPercent = parseFloat(TPVal);
    if ($("#discountAmount").val() != "") {
        TaxAmount = ((taxableamount - DiscountAmount - NonTaxValue) / 100) * TPercent;
        $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }
    else {
        TaxAmount = ((taxableamount - NonTaxValue) / 100) * TPercent;
        $(".tax").text(TransMakeCurrency + TaxAmount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }
}

var InitRowIndex = function () {
    var i = 1;
    $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}
var OpenInvoiceSend = function () {
    /*console.log('Open invoice send');*/
    setTimeout(function () {
        $("#InvoiceInfoPrintAndSend").trigger('click');
    }, 500);
}
var SaveInvoice = function (SendEmail, CreatePdf, CameFrom, EmailDescription, invoiceid, EmailSubject, ccEmail, EmailAddress) {
    var memoval = "";
    console.log("Save invoice fired");
    if ($(".HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", function () { });
        return;
    }
    console.log("not returned");
    if ($("#InvoiceDescription").val().trim() == '') {
        var DescriptionText = "";
        $(".txtProductDesc").each(function () {
            if ($(this).val().trim() != '') {
                DescriptionText += $(this).val();
                DescriptionText += ", ";
            }
        });

        DescriptionText = DescriptionText.slice(0, -2);

        $("#InvoiceDescription").val(DescriptionText);
    }
    if (typeof (EmailAddress) == "undefined" || typeof (EmailAddress) == "") {
        EmailAddress = $("#Invoice_InvoiceEmailAddress").val();
    }

    if (typeof (SendEmail) == "undefined") {
        SendEmail = false;
    }
    if (typeof (CreatePdf) == "undefined") {
        CreatePdf = false;
    }
    if (typeof (CameFrom) == "undefined") {
        CameFrom = "";
    }
    if (typeof (EmailSubject) == "undefined") {
        EmailSubject = "";
    }
    var DetailList = []; 
    if (InvoiceTotalBanlanceDue != null && InvoiceTotalBanlanceDue != "undefined") {
        BalanceDue = BalanceDue - InvoiceTotalBanlanceDue;
    } 
    $(".HasItem").each(function () {
        DetailList.push({
            EquipmentId: $(this).attr('data-id'),
            EquipDetail: $(this).find('.txtProductDesc').val(),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find('.txtProductRate').val().trim().replaceAll(',', ''),
            TotalPrice: ($(this).find('.txtProductAmount').val().trim().replaceAll(',', '')),
            EquipName: $(this).find('.ProductName').val(),
            DiscountAmount: $(this).find('.txtProductItemDiscountAmount').val(),
            //Taxable: (!$(this).hasClass('NonTaxable')),
            Taxable: TaxablePermit.toLowerCase() == "true" ? $(this).find('.chkTaxable').is(':checked') : true,
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
            InvoiceId: InvoiceId
        });
    });
    if (memopermit == "True") {
        memoval = $("#Memo").val();
    }

    //if (parseFloat($(".tax_amount").text().replace('$', '')).toFixed(2) != TaxAmount.toFixed(2)) {
    //    OpenErrorMessageNew("Error!", "Tax calculation issue, close and re-open the Invoice " + $(".tax_amount").text() + " | " + TaxAmount + " | " + $("#taxType").val(), function () { });
    //    return;
    //}

    var url = domainurl + "/Invoice/AddInvoice";

    var param = JSON.stringify({
        EmailAddress: EmailAddress,
        "Invoice.InvoiceId": InvoiceId,
        "Invoice.InvoiceEmailAddress": EmailAddress,
        "Invoice.InvoiceCcEmailAddress": ccEmail,
        "Invoice.BillingAddress": tinyMCE.get('Invoice_BillingAddress').getContent(),
        "Invoice.Terms": $("#Invoice_Terms").val(),
        "Invoice.ShippingAddress": tinyMCE.get('Invoice_ShippingAddress').getContent(),
        "Invoice.ShippingAddress": tinyMCE.get('Invoice_ShippingAddress').getContent(),
        "Invoice.ShippingVia": $("#Invoice_ShippingVia").val(),
        "Invoice.ShippingDate": shippingDatePicker.getDate(),
        "Invoice.ShippingCost": $("#Invoice_ShippingCost").val(),
        "Invoice.TrackingNo": $("#Invoice_TrackingNo").val(),
        "Invoice.CustomerId": $("#InvoiceCustomerId").val(),
        "Invoice.InvoiceDate": GetTimeFormat($("#Invoice_InvoiceDate").val()),
        "Invoice.DueDate": GetTimeFormat($("#Invoice_DueDate").val()),
        "Invoice.TotalAmount": FinalTotal.toFixed(2),
        "Invoice.Amount": TotalAmount,
        "Invoice.Message": $("#InvoiceMessage").val(),
        "Invoice.Deposit": $("#Invoice_Deposit").val(),
        "Invoice.DiscountAmount": DiscountDBAmount,
        "Invoice.Discountpercent": DiscountDBPercent,
        "Invoice.BalanceDue": BalanceDue.toFixed(2),//75
        "Invoice.Tax": TaxAmount.toFixed(2),
        "Invoice.TaxType": $("#taxType option:selected").text(),
        "Invoice.Memo": memoval,
        "Invoice.Description": $("#InvoiceDescription").val(),
        "Invoice.DiscountType": $("#Invoice_DiscountType").val(),
        "Invoice.JobNo": $("#JobNo").val(),
        "Invoice.TaxPercentage": $(".tax_val").val(),
        "Invoice.InvoiceFor": $("#Invoice_InvoiceFor").val(),
        "Invoice.TicketId": $("#Invoice_TicketId").val(),
        "Invoice.PaymentType": $("#PaymentMethod").val(),
       // "Invoice.Id": parseInt(invoiceid),
        "Invoice.Id": Invoice_int_Id,
        InvoiceDetailList: DetailList,
        invoiceid: invoiceid,
        SendEmail: SendEmail,
        CreatePdf: CreatePdf,
        EmailDescription: EmailDescription,
        EmailSubject: EmailSubject,
        ccEmail: ccEmail
    });
    console.log($("#Invoice_Terms").val());
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
            if (CameFrom == "preview") {
                InvoiceNoteSave();
                OpenInvoiceSend();
            }
            else if (data.result && CameFrom != "others" && data.EmailSent == false) {
                InvoiceNoteSave();
                OpenSuccessMessageNew("Success!", "Invoice saved successfully!", function () { OpenInvoiceTab() });
                CloseTopToBottomModal();
            }
            else if (data.result && CameFrom != "others" && data.EmailSent == true) {
                InvoiceNoteSave();
                OpenSuccessMessageNew("Success!", data.message, function () {  OpenInvoiceTab() });
                CloseTopToBottomModal();
            } else if (data.result) {
                InvoiceNoteSave();
                OpenSuccessMessageNew("Success!", data.message, function () {  OpenInvoiceTab() });
                CloseTopToBottomModal();
            }
            else if (!data.result) {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
            $(".AddInvoiceLoader").addClass('hidden');
        }
    });

}
var LoadDiscount = function () {
    if (DiscountAmountDbValue != "0") {
        $("#discountAmount").val(DiscountAmountDbValue);
        $('#discountType').val('amount');
        DiscountDBPercent = 0;
    }
    if (DiscountPercentDbValue != "0") {
        $("#discountAmount").val(DiscountPercentDbValue);
        $('#discountType').val('percent');
        DiscountDBAmount = 0;
    }
}
var LoadTax = function () {
    console.log("TaxTypeDbValue" + TaxTypeDbValue);
    if (TaxTypeDbValue != null && TaxTypeDbValue != "") {
        console.log("TaxType" + TaxTypeDbValue)
        $("#taxType option").each(function () {
            if ($(this).text() == TaxTypeDbValue) {
                console.log("TaxType ok" + TaxTypeDbValue)
                $(this).prop('selected', 'selected');
            }
        });
    }
}

var Showshipping = function () {
    //$(".shipping").show();
    $(".Shipping").show();
}
var Hideshipping = function () {
    //$(".shipping").hide();
    $(".Shipping").hide();
}
var ShowDiposit = function () {
    $(".Diposit").show();
}
var HideDiposit = function () {
    $(".Diposit").hide();
}
var ShowDiscount = function () {
    $(".Discount").show();
    $(".Discount-total").removeClass('hidden');
}
var HideDiscount = function () {
    $(".Discount").hide();
}
var ShowShippingDiv = function () {
    $(".shipping-div").show();
    $(".shipping-amount-div").show();
}
var HideShippingDiv = function () {
    $(".shipping-div").hide();
    $(".shipping-amount-div").hide();
}
var ShowDiscountDiv = function () {
    $(".discount-amount-div").show();
    $(".Discount-total").removeClass('hidden');
}
var HideDiscountDiv = function () {
    $(".discount-amount-div").hide();
}
var ShowDepositDiv = function () {
    $(".deposit-amount-div").show();
}
var HideDepositDiv = function () {
    $(".deposit-amount-div").hide();
}

var InvoiceSettingsInitialLoad = function () {
    if (InvoiceShippingSetting == "True") {
        setTimeout(function () {
            $(".shipping-div").show();
            $(".shipping-amount-div").show();
            $(".Shipping").show();
        }, 10);
        //ShowShippingDiv();
        //Showshipping();
    }
    else {
        setTimeout(function () {
            $(".shipping-div").hide();
            $(".shipping-amount-div").hide();
            $(".Shipping").hide();
            $(".shippingAddress").val("");
        }, 10);
        //HideShippingDiv();
        //Hideshipping();
    }
    if (InvoiceDiscountSetting == "True") {
        setTimeout(function () {
            $(".discount-amount-div").show();
            $(".Discount").show();
        }, 10);
        //ShowDiscountDiv();
        //ShowDiscount();
    }
    else {
        setTimeout(function () {
            $(".discount-amount-div").hide();
            $(".Discount").hide();
        }, 10)
        //HideDiscountDiv();
        //HideDiscount();
    }
    if (InvoiceDepositSetting == "True") {
        setTimeout(function () {
            $(".deposit-amount-div").show();
            $(".Diposit").show();
        }, 10)
        //ShowDepositDiv();
        //ShowDiposit();
    }
    else {
        setTimeout(function () {
            $(".deposit-amount-div").hide();
            $(".Diposit").hide();
        }, 10)
        //HideDepositDiv();
        //HideDiposit();
    }
}
var SaveRefundCreditAmount = function (value, invid, note) {
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: "/Customer/SaveRefundCreditAmount",
        data: JSON.stringify({ value: value, customerid: CustomerLoadGuid, invid: invid, note: note }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                OpenSuccessMessageNew("Success", "Credit amount refund successfully", function () {
                    CloseTopToBottomModal();
                    OpenTransactionTab();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var SaveAddCreditAmount = function (value, invid, note) {
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: "/Customer/SaveAddCreditAmount",
        data: JSON.stringify({ value: value, customerid: CustomerLoadGuid, invid: invid, note: note}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                OpenSuccessMessageNew("Success", "Credit amount added successfully", function () {
                    CloseTopToBottomModal();
                    OpenTransactionTab();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var RefundCreditAmount = function (amount, invid) {
    if (amount.length > 0) {
        amount = amount.replace(',', '');
    }
   
    OpenConfirmationMessageNew("Confirmation", "Do you want to refund?", function () {
        OpenCreditAmountModal("Refund Amount", "<div class='form-group clearfix acm_field'><label class='label_full'>Total Amount: " + amount + "</label></div><div class='form-group clearfix acm_field'><label>Refund Amount: </label><input type=number class='form-control' id='refund_amount' placeholder='Refund Amount' value = '0' /></div><div class='form-group clearfix acm_field'><label>Refund Note: </label><textarea class='form-control' id='refund_note' placeholder='Refund Note' ></textarea></div>", function () {
            var refund = $("#refund_amount").val();
            var note = $("#refund_note").val();
            console.log("refund");
            if (parseFloat(refund) > 0 && parseFloat(refund) <= parseFloat(amount)) {
                SaveRefundCreditAmount(refund, invid, note);
            }
            else {
                OpenErrorMessageNew("Error", "Refund amount not equal to zero or greater than total credit amount");
            }
        })
    });
}

var AddCreditAmount = function (amount, invid) {
    OpenConfirmationMessageNew("Confirmation", "Do you want to add credit?", function () {
        OpenCreditAmountModal("Add Credit Amount", "<div class='form-group clearfix acm_field'><label class='label_full'>Maximum Credit Can Be Applied: " + amount + "</label></div><div class='form-group clearfix acm_field'><label>Credit Amount: </label><input type=number class='form-control' id='refund_amount' placeholder='Credit Amount' value = '0' /></div><div class='form-group clearfix acm_field'><label>Credit Note: </label><textarea class='form-control' id='refund_note' placeholder='ie: Invoice#'></textarea></div>", function () {
            var refund = $("#refund_amount").val();
            var note = $("#refund_note").val();

            amount = amount.replaceAll(",", "");
            if (parseFloat(refund) > 0 && parseFloat(refund) <= parseFloat(amount)) {
                SaveAddCreditAmount(refund, invid, note);
            }
            else {
                OpenErrorMessageNew("Error", "Credit amount not equal to zero or greater than total credit amount");
            }
        });
    });
}

$(document).ready(function () {

    AddInvoice.OnPageDocready();

    $("body").on('click', function (evt) {
        if (!$(evt.target).closest('#CustomerInvoiceTab tr').length) {
            $("#CustomerInvoiceTab tr").removeClass("focusedItem");
        }
    });


    $("#discountAmount").keyup(function () {
        if ($("#discountAmount").val() != "") {
            $(".Discount-total").removeClass('hidden');
        }
        else {
            $(".Discount-total").addClass('hidden');
        }
    })
    makeSendEmailUrl();
    InvoiceSettingsInitialLoad();

    $("#Invoice_InvoiceEmailAddress").keydown(function () {
        makeSendEmailUrl();
    });
    $("#Invoice_InvoiceEmailAddress").keyup(function () {
        makeSendEmailUrl();
    });
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
        field: $('#Invoice_InvoiceDate')[0],
        trigger: $('#InvoiceDateArea')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1
    });
    DueDatepicker = new Pikaday({
        field: $('#Invoice_DueDate')[0],

        trigger: $('#ExpireDateArea')[0],

        format: 'MM/DD/YYYY',
        firstDay: 1
    });
    shippingDatePicker = new Pikaday({
        field: $('.ShippingDatePicker')[0],
        trigger: $('#ShippingDateArea')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1
    });
    LoadDiscount();
    LoadTax();
    InitRowIndex();
    $(".DescStartCount").html($("#InvoiceDescription").val().length);
    $("#InvoiceDescription").keyup(function () {
        $(".DescStartCount").html($("#InvoiceDescription").val().length);
    });
    $(".StartCount").html($("#InvoiceMessage").val().length);
    $("#InvoiceMessage").keyup(function () {
        $(".StartCount").html($("#InvoiceMessage").val().length);
    });

    if (memopermit == "True") {
        $(".MemoStartCount").html($("#Memo").val().length);
        $("#Memo").keyup(function () {
            $(".MemoStartCount").html($("#Memo").val().length);
        });
    }

    $("#CustomerList").focusout(function () {
        setTimeout(function () {
            $(".customer_name_insert_div .tt-menu").hide();
        }, 200);
    });
    $("#CustomerInvoiceTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide();
        var ProductNameDom = $(this).parent().find('span.spnProductName');
        $(ProductNameDom).text($(this).val());
    });
    $(".CustomerInvoiceTab tbody").on('click', 'tr td .chkTaxable', function (e) {
        CalculateNewAmount();
    });
    $("#CustomerInvoiceTab tbody").on('blur', 'tr', function (item) {
        if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined'
            && typeof ($(item.target).parent().parent().parent().attr('data-id')) == 'undefined') {
            var trdom = $(item.target).parent().parent();
            $(trdom).find("input.ProductName").val('');
            $(trdom).find("span.spnProductName").text('');

            $(trdom).find("input.txtProductDesc").val('');
            $(trdom).find("span.spnProductDesc").text('');

            $(trdom).find("input.txtProductQuantity").val('');
            $(trdom).find("span.spnProductQuantity").text('');

            $(trdom).find("input.txtProductItemDiscountAmount").val('');
            $(trdom).find("span.spnProductItemDiscountAmount").text('');

            $(trdom).find("span.spnProductEquipmentvendorcost").text('');
            $(trdom).find("span.spnProductItemDiscountAmount").text('');

            $(trdom).find("input.txtProductRate").val('');
            $(trdom).find("span.spnProductRate").text('');

            $(trdom).find("input.txtProductAmount").val('');
            $(trdom).find("span.spnProductAmount").text('');

            CalculateNewAmount();
        }
    });
    /*focus table row*/
    $("#CustomerInvoiceTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        //console.log(e.target);
        if (!(InvoiceStatus == "Partial" || InvoiceStatus == "Paid")) {
            if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
                || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate") || $(e.target).hasClass("spnProductEquipmentvendorcost")
                || $(e.target).hasClass("spnProductAmount"))  {

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
        if (InvoiceStatus == "Partial" || InvoiceStatus == "Paid") {
            return;
        }
        if (TaxablePermit.toLowerCase() == "true") {
            $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRowTaxable);
        }
        else {
            $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
        }
        var i = 1;
        $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    /*Remove last row*/
    $(".CustomerInvoiceTab tbody").on('click', 'tr td i.fa', function (e) {
        if (InvoiceStatus == "Partial" || InvoiceStatus == "Paid") {
            return;
        }
        $(this).parent().parent().parent().remove();
        if ($(".CustomerInvoiceTab tbody tr").length < 2) {
            if (TaxablePermit.toLowerCase() == "true") {
                $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRowTaxable);
            }
            else {
                $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
            }
            //$("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
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
        var productQuantity = $(this).parent().parent().find('input.txtProductQuantity');
        $(ProductQuantityDom).text($(this).val());
        var ProductRateDom = $(this).parent().parent().find('input.txtProductRate');
        if ($(productQuantity).val() > 0) {
            if ($(ProductRateDom).val() != "" && parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) > 0) {
                var NewProductAmount = $(this).val() * parseFloat($(ProductRateDom).val().trim().replaceAll(',', ''));
                var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
                var spnProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
                $(txtProductAmountDom).val(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductAmountDom).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewAmount();
            }
        }
        else {
            console.log($(this).parent().find('span.spnProductQuantity').text("1"));
            $(productQuantity).val("1");
            $(ProductQuantityDom).val("1");

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
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductAmount", function () {

        console.log("Product Amount Change");
        var ProductAmount = $(this).parent().parent().find('input.txtProductAmount');

        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRateDom = $(this).parent().parent().parent().find('input.txtProductRate');
        var spnProductRateDom = $(this).parent().parent().parent().find('span.spnProductRate');
        if ($(ProductAmount).val() != "" && parseFloat($(ProductAmount).val().trim().replaceAll(',', '')) >= 0) {
            var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(ProductAmountDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
                $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductRateDom).text("$" + NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
            CalculateNewAmount();
        }
        else {
            var CalculateAmount = parseFloat($(ProductRateDom).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();
            $(ProductAmount).val(CalculateAmount);
            var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
            $(ProductAmountDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var NewProductRate = (parseFloat($(this).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val());
                $(ProductRateDom).val(NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                $(spnProductRateDom).text("$" + NewProductRate.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            }
            CalculateNewAmount();
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductRate", function () {
        /*
        *If product rate changes make change to amount.
        */
        var ProductQuantityDom = $(this).parent().parent().parent().find('input.txtProductQuantity');
        var ProductRate = $(this).parent().parent().find('input.txtProductRate');
        var txtProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
        if ($(ProductRate).val() != ""
            && !isNaN(parseFloat($(ProductRate).val().trim().replaceAll(',', '')))
            //&& parseFloat($(ProductRate).val().trim().replaceAll(',', '')) >= 0


        ) {
            var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
            $(ProductRateDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();


                var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text("$" + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewAmount();
            }
        }
        else {

            var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
            var CalculateRate = parseFloat($(txtProductAmountDom).val().trim().replaceAll(',', '')) / $(ProductQuantityDom).val();
            $(ProductRate).val(CalculateRate);
            var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
            $(ProductRateDom).text("$" + parseFloat($(this).val().trim().replaceAll(',', '')).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
            if ($(ProductQuantityDom).val() != "" && $(ProductQuantityDom).val() > 0) {
                var ProductAmount = parseFloat($(this).val().trim().replaceAll(',', '')) * $(ProductQuantityDom).val();


                var txtProductAmountDom = $(this).parent().parent().parent().find('input.txtProductAmount');
                $(txtProductAmountDom).val(ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                var spnProductAmountDom = $(this).parent().parent().parent().find('span.spnProductAmount');
                $(spnProductAmountDom).text("$" + ProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
                CalculateNewAmount();
            }
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductDesc", function () {
        IsChanged = true;
        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });
    $("#btnInvoiceStatusSave").click(function () {
        var url = domainurl + "/Invoice/InvoiceStatusSave/";
        var InvoiceStatus = $("#Invoice_Status").val();
        var Param = JSON.stringify({
            InvoiceId: Invoice_int_Id,
            InvoiceStatus: InvoiceStatus
        });
        $.ajax({
            url: url,
            data: Param,
            type: "Post",
            contentType: "application/json; charset=utf-8",
            dataType: "Json",
            success: function (data) {
                if (data.result == true) {
                    OpenSuccessMessageNew("Success", "", new function () {
                        CloseTopToBottomModal();
                        OpenInvoiceTab();
                    });
                }
                else {
                    OpenErrorMessageNew("Error", "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });
    $("#btnInvoiceTicketSave").click(function () {
        var url = domainurl + "/Invoice/InvoiceTicketSave/";
        var ticketId = $("#Invoice_TicketId").val();
        var Param = JSON.stringify({
            InvoiceId: Invoice_int_Id,
            TicketId: ticketId
        });
        $.ajax({
            url: url,
            data: Param,
            type: "Post",
            contentType: "application/json; charset=utf-8",
            dataType: "Json",
            success: function (data) {
                if (data.result == true) {
                    OpenSuccessMessageNew("Success", "");
                }
                else {
                    OpenErrorMessageNew("Error","")
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });
    $(".InvoiceSaveButton").click(function () {

        if ($(".HasItem").length == 0) {
            OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
        } else {
            if (CommonUiValidation()) {
                SaveInvoice(false, true, "others", null, Invoice_int_Id);
            }
            var Invoval = $("#Invoice_Status").val();
            var PaymentMethod = $("#PaymentMethod").val();
            if (Invoval == "Cancel") {
                $.ajax({
                    url: domainurl + "/Invoice/ConvertInvoiceStatus/",
                    data: { Invoval, PaymentMethod, Invoice_int_Id },
                    type: "Post",
                    dataType: "Json",
                    success: function (data) {
                        if (data == true) {
                            InvoiceNoteSave();
                            CloseTopToBottomModal();
                            OpenInvoiceTab();
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }
                })
            }
        }
    });
    $("#Invoice_DiscountType").change(function () {
        CalculateNewAmount();
    });
    $("#discountAmount").change(function () {
        CalculateNewAmount();
    });
    $('#Invoice_ShippingCost').focusout(function () {
        CalculateNewAmount();
    });
    $('#discountAmount').focusout(function () {
        CalculateNewAmount();
    });
    $('#Invoice_Deposit').focusout(function () {
        CalculateNewAmount();
    });
    $(".tax_val").change(function () {
        CalculateNewAmount();
    });
    $("#taxType").change(function () {
        if ($("#taxType").val() == "") {
            $(".tax_amount_custom").addClass('hidden');
            $(".tax ").text(TransMakeCurrency + "0.00");
            CalculateNewAmount();
        }
        else if ($("#taxType").val() == "Custom") {
            $(".tax_amount_custom").removeClass('hidden');
            $(".tax_val").val("0.00");
            CalculateNewAmount();
        }
        else {
            $(".tax_amount_custom").addClass('hidden');
            CalculateNewAmount();
        }
    });

    CalculateNewAmount();
    CalculateNewAmount();
    if (InvoiceStatus == "Partial") {
        var invoicedom = $(".invoice-informations");
        invoicedom.find("input").prop("disabled", true);
        invoicedom.find("select").prop("disabled", true);
        invoicedom.find("textarea").prop("disabled", true);  
        $(".balanceDueAmount").text(TransMakeCurrency + InvoiceBalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(".big-amount-top.amount").text(TransMakeCurrency + InvoiceBalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

    } else if (InvoiceStatus == "Paid") {
        var invoicedom = $(".invoice-informations");
        invoicedom.find("input").prop("disabled", true);
        invoicedom.find("select").prop("disabled", true);
        invoicedom.find("textarea").prop("disabled", true);
        $(".balanceDueAmount").text(TransMakeCurrency + InvoiceBalanceDue.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    }

    setTimeout(function () {
        $(".add-invoice-container input,.add-invoice-container textarea").change(function () {
            IsChanged = true;
        })
    }, 500);
});