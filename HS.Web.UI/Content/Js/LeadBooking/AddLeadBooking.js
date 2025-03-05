var BalanceDue = 0;
var SendEmailUrl = "";
var mailAdd = "";  
var PI = "3.1416";

var BookingSubTotal = 0;
var ExtraItemSubTotal = 0;
//var TotalAmount = 0; //BookingSubTotal + ExtraItemSubTotal//
var TotalTax = 0;
var Discount = 0;
var TotalDiscount = 0;
var FinalAmount = 0;

var SendEmailUrl = "";
var mailAdd = "";
var ShipAddress = $("#Booking_BillingAddress").val();
var PickUpDatepicker;
var DropOffDatepicker;
var PickUpDate;
var customerid;
var detailId;
var Popupwidth = 920;
Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}


var CurrentCustomerId = $("#BookingCustomerId").val();
var OpenACHAddView = function () {
    OpenRightToLeftModal(domainurl + "/SmartLeads/ACHAddViewPaymentMethod?customerid=" + CurrentCustomerId);
}
var OpenCCAddView = function () {
    OpenRightToLeftModal(domainurl + "/SmartLeads/CCAddViewPaymentMethod?customerid=" + CurrentCustomerId);//+ "&IsFromBooking=" + true
}


//var CalculateDiscount = function () {
//    TotalDiscount = 0;
//    $("#CustomerEstimateTab tr.HasItem .txtProductDiscount, #CustomerBkTab tr.HasItem .txtProductDiscount").each(function () {
//        var CurrentRowAmount = parseFloat($(this).val());
//        if (!isNaN(CurrentRowAmount)) {
//            TotalDiscount += CurrentRowAmount;
//        }
//    });
//}
var GetTimeFormat = function (date) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return new Date(date + ' ' + time)
}
 

var BookingCustomerclickbind = function (item) {
    $('.customer_name_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        setTimeout(function () {
            $('.customer_name_insert_div .tt-menu').hide();
        },100);
        

        var selectedEmail = $(clickitem).attr("data-emailAddress").trim();

        var BussiName = $(clickitem).attr("data-Bussiness").trim();
        var Customerfnum = $(clickitem).attr("data-firstName").trim();
        var Customerlnum = $(clickitem).attr("data-lastName").trim();
        var CustomerGuId = $(clickitem).attr("data-customerId").trim();

        var displayname = BussiName == '' ? Customerfnum + " " + Customerlnum : BussiName;
        $("#CustomerList").val(displayname);
        console.log("ClickBind" + selectedEmail);
        $("#EmailAddress").val(selectedEmail);
        $("#BookingCustomerId").val(CustomerGuId);
        $(".shippingAddress").val("");
        $.ajax({
            type: "POST",
            url: "/Booking/GetCustomerAddressByCustomerId",
            data: JSON.stringify({
                CustomerId: CustomerGuId
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result == true) {
                    tinyMCE.get('Booking_BillingAddress').setContent(data.BillingAddressVal);
                    tinyMCE.get('Booking_ShippingAddress').setContent(data.ShippingAddressVal);
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

var CustomerSearchKeyDown = function (item, event) {
    console.log("CustomerSearchKeyDown" + event.keyCode);
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
        url: "/Booking/GetLeadListByKey",
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
                    var name = resultparse[i].BusinessName == '' ? resultparse[i].FirstName + ' ' + resultparse[i].LastName : resultparse[i].BusinessName;

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

                BookingCustomerclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(200);
                    $(".NewProjectSuggestion").css('position', 'relative');
                }
            }
            if (resultparse.length == 0)
                setTimeout(function () {
                    $('.tt-menu').hide();
                },100);
        }
    });
}

//Save And Close Method 
var SaveAndClose = function () {
    SaveBooking(true, false, "others");
    CloseTopToBottomModal(); 
    OpenSuccessMessageNew("Booking Saved", "Booking Successfully Saved", function () { parent.OpenLeadBookTab() });
}
//Generate Send Email Url Method 
var makeSendEmailUrl = function () {

    mailAdd = encodeURIComponent($("#EmailAddress").val());
    SendEmailUrl = "/Booking/SendEmailBooking/?Id=" + UrlModelBookingId + "&EmailAddress=" + mailAdd;
    $("#BookingPrintAndSend").attr("href", SendEmailUrl);
}
//Save And Send Method 
var SaveAndSend = function () {
    console.log("sdfsdf");
    //is exist
    makeSendEmailUrl();
    SaveBooking(false, true, "preview");
    //OpenLeadBookTab();
}

//var RugShapeSuggestionclickbind = function (item) {
//    $('.CustomerBkTab .rug-type-suggestion .tt-suggestion').click(function () {
//        console.log('rug-type-suggestion')
//        var clickitem = this;
//        $('.CustomerBkTab .tt-menu').hide();
//        $(item).val($(clickitem).attr('data-displaytext'));
//        $(item).attr('data-id', $(clickitem).attr('data-id'));

//        var itemRugShape = $(item).parent().parent().find('.spnRugShape');
//        $(itemRugShape).text($(item).val());
        
//        if ($(clickitem).attr('data-displaytext') == 'Rectangle' || $(clickitem).attr('data-displaytext') == 'Rectangle/Square'
//            || $(clickitem).attr('data-displaytext') == 'Oval') {
//            $(item).parent().parent().find('.txtRugLength').removeClass('hidden');
//            $(item).parent().parent().find('.spnRugLength').removeClass('hidden');
//            $(item).parent().parent().find('.txtRugLengthInch').removeClass('hidden');
//            $(item).parent().parent().find('.spnRugLengthInch').removeClass('hidden');

//            $(item).parent().parent().find('.txtRugRadius').addClass('hidden');
//            $(item).parent().parent().find('.spnRugRadius').addClass('hidden');
//            $(item).parent().parent().find('.txtRugWidth').removeClass('hidden');
//            $(item).parent().parent().find('.spnRugWidth').removeClass('hidden');
//            $(item).parent().parent().find('.txtRugRadiusInch').addClass('hidden');
//            $(item).parent().parent().find('.spnRugRadiusInch').addClass('hidden');
//            $(item).parent().parent().find('.txtRugWidthInch').removeClass('hidden');
//            $(item).parent().parent().find('.spnRugWidthInch').removeClass('hidden');

//            $(item).parent().parent().find('.bkQuarterX').removeClass('hidden');
//        }
//        else {
//            $(item).parent().parent().find('.txtRugRadius').removeClass('hidden');
//            $(item).parent().parent().find('.spnRugRadius').removeClass('hidden');
//            $(item).parent().parent().find('.txtRugRadiusInch').removeClass('hidden');
//            $(item).parent().parent().find('.spnRugRadiusInch').removeClass('hidden');

//            $(item).parent().parent().find('.txtRugLength').addClass('hidden');
//            $(item).parent().parent().find('.spnRugLength').addClass('hidden');
//            $(item).parent().parent().find('.txtRugWidth').addClass('hidden');
//            $(item).parent().parent().find('.spnRugWidth').addClass('hidden');
//            $(item).parent().parent().find('.txtRugLengthInch').addClass('hidden');
//            $(item).parent().parent().find('.spnRugLengthInch').addClass('hidden');
//            $(item).parent().parent().find('.txtRugWidthInch').addClass('hidden');
//            $(item).parent().parent().find('.spnRugWidthInch').addClass('hidden');

//            $(item).parent().parent().find('.bkQuarterX').addClass('hidden');
//        }

//        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
//        $(item).parent().parent().addClass('HasItem');
//    });

//    $('.CustomerBkTab .rug-type-suggestion .tt-suggestion').hover(function () {
//        var clickitem = this;
//        $('.tt-suggestion').removeClass("active");
//        $(clickitem).addClass('active');
//    });
//}

function round2Fixed(value) {
    value = +value;
    if (isNaN(value))
        return NaN;
    // Shift
    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + 2) : 2)));
    // Shift back
    value = value.toString().split('e');
    return (+(value[0] + 'e' + (value[1] ? (+value[1] - 2) : -2))).toFixed(2);
}




var ExpirationDateValidation = function () {
    var result = true;
    var now = new Date();
    var Today = new Date(now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDate());
    var ExpirationDate = new Date($("#Invoice_DueDate").val());
    if (Today != "Invalid Date" && ExpirationDate != "Invalid Date") {
        if (ExpirationDate < Today) {
            $("#Invoice_DueDate").addClass("required");
            $("#DueDateGtToday").removeClass("hidden");
            result = false;
        } else {
            $("#Invoice_DueDate").removeClass("required");
            $("#DueDateGtToday").addClass("hidden");
            result = true;
        }
    }
    return result;
}

var SaveBooking = function (SendEmail, CreatePdf, CameFrom, EmailDescription, EmailSubject, CCMail) {
    if (typeof (SendEmail) == "undefined") {
        SendEmail = false;
    }
    if (typeof (CreatePdf) == "undefined") {
        CreatePdf = true;
    }
    if (typeof (CameFrom) == "undefined") {
        CameFrom = "";
    }
    if (typeof (EmailDescription) == "undefined") {
        EmailDescription = "";
    }
    if (typeof (EmailSubject) == "undefined") {
        EmailSubject = "";
    }
    //if ($("#CustomerBkTab .HasItem").length == 0) {
    //    OpenErrorMessageNew("Error!", "You have to select at least one item to proceed", function () { });
    //    return;
    //}

    var DetailList = [];
    var RugZeroCount = 0;
    $("#CustomerBkTab .HasItem").each(function () {
        if ($(this).find('input.txtRugArea').val() > 0) {
            DetailList.push({
                Quantity: $(this).find('input.txtProductQuantity').val(),
                UnitPrice: $(this).find('input.txtProductRate').val(),
                TotalPrice: $(this).find('input.txtProductAmount').val(),
                RugType: $(this).find('input.RugShape').val(),
                Length: $(this).find('input.txtRugLength').val(),
                LengthInch: $(this).find('input.txtRugLengthInch').val(),
                Width: $(this).find('input.txtRugWidth').val(),
                WidthInch: $(this).find('input.txtRugWidthInch').val(),
                Radius: $(this).find('input.txtRugRadius').val(),
                RadiusInch: $(this).find('input.txtRugRadiusInch').val(),
                Discount: $(this).find('input.txtProductDiscount').val(),
                TotalSize: $(this).find('input.txtRugArea').val(),
                TotalSizeInch: $(this).find('input.txtRugAreaInch').val(),
                Package: $(this).find('input.txtProductPackage').val(),
                Included: $(this).find('input.txtProductInclude').val(),
                AddedDate: '1-1-2017',
                BookingId: BookingId
            });
        } else {
            RugZeroCount++;
        }
    });

    if (RugZeroCount > 0) {
        OpenErrorMessageNew("", "Please input rug area correctly to proceed.");
        return;
    }

    var ExtraItemDetailList = [];
    $("#CustomerEstimateTab .HasItem").each(function () {
        ExtraItemDetailList.push({
            EquipmentId: $(this).attr('data-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', '')),
            TotalPrice: $(this).find('.txtProductQuantity').val() * parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', '')),
            EquipDetail: $(this).find('.txtProductDesc').val(),
            EquipName: $(this).find('.ProductName').val(),
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
        });
    });
    var url = "/Booking/AddLeadBooking";
    var param = JSON.stringify({
        EmailAddress: $("#EmailAddress").val(),
        //EmailDescription: $(".txtEmailbody").val(),
        "Booking.BookingId": BookingId,
        "Booking.BillingAddress": parent.tinyMCE.get('Booking_BillingAddress').getContent(),
        "Booking.PickUpLocation": parent.tinyMCE.get('Booking_PickUpLocation').getContent(),
        "Booking.DropOffLocation": parent.tinyMCE.get('Booking_DropOffLocation').getContent(),
        "Booking.CustomerId": $("#BookingCustomerId").val(),
        "Booking.EmailAddress": $("#EmailAddress").val(),
        "Booking.Amount": TotalAmount,
        "Booking.TotalAmount": TotalAmount,
        "Booking.DiscountAmount": TotalDiscount,
        "Booking.Tax": TotalTax,
        "Booking.TaxType": $("#Booking_TaxType option:selected").text(),
        "Booking.BookingMessage": $("textarea#BookingMessage").val(),
        "Booking.PickUpDate": $("#PickUpDate").val(),
        "Booking.DropOffDate": $("#DropOffDate").val(),
        //"Booking.DiscountType": $("#Invoice_DiscountType").val(),
        BookingExtraItem: ExtraItemDetailList,
        BookingDetailsList: DetailList,
        SendEmail: SendEmail,
        CreatePdf: CreatePdf,
        EmailDescription: EmailDescription,
        EmailSubject: EmailSubject,
        CCMail: CCMail
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
            if (typeof (LeadDetailTabCount) != "undefined") {
                LeadDetailTabCount();
            }
            if (typeof (OpenBookingTab) != "undefined") {
                OpenBookingTab();
            }
            if (CameFrom == "preview") {
                if ($("#EmailAddress").val() == "") {
                    OpenErrorMessageNew("Error!", "Email address field couldn't be empty", "");
                }
                else {
                    $("#BookingPrintAndSend").click();
                    //$("#BookingResend").click();
                }
            }
            else {
                if (data.result && data.EmailSent == true && CameFrom != "others") {
                    OpenSuccessMessageNew("Success", data.message, function () { parent.OpenLeadBookTab() });
                    CloseTopToBottomModal();
                }
                else if (data.result && CameFrom != "others") {
                    OpenSuccessMessageNew("Success!", "Booking saved successfully!", function () { parent.OpenLeadBookTab() });
                    CloseTopToBottomModal();
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })

}

var LoadTax = function () { 
    if (TaxTypeDbValue != null && TaxTypeDbValue != "") { 
        $("#Booking_TaxType option").each(function () {
            if ($(this).text() == TaxTypeDbValue) {
                $(this).prop('selected', 'selected');
            }
        });
    }
}
 

//Delete Booking Method
var DeleteBookingById = function (BookingDeleteId) {
    $.ajax({
        url: "/Booking/DeleteBooking",
        data: {
            Id: BookingDeleteId
        },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Booking deleted successfully!");
                OpenLeadBookTab();
                CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
            if (data.result) {
                OpenConfirmtionMessage("Success!", "Booking deleted successfully!");
            }
        }
    });
}

//Resend Booking Email
var ResendBookingEmail = function (BookingID) {
    

    $.ajax({
        url: "/Booking/ResendBookingEmail",
        data: {
            BookingId: BookingID,
            EmailAddress: $("#EmailAddress").val()
        },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                console.log(data);
                OpenSuccessMessageNew("Success!", data.message);                
                CloseTopToBottomModal();
                OpenLeadBookTab();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}

//Booking Clone Method
var CloneBooking = function (bookingId) {
    $.ajax({
        url: "/Booking/CloneBooking",
        data: {
            BookingId: bookingId
        },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
                OpenLeadBookTab();
                CloseTopToBottomModal();

            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}

//Approve Booking
var ApproveBooking = function (bookingId) {
    $.ajax({
        url: "/Booking/ApproveBooking",
        data: {
            BookingId: bookingId
        },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    location.href = '/Customer/Customerdetail/?id=' + data.CustomerId + '#TicketTab';
                    CloseTopToBottomModal();
                });                
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}

$(document).ready(function () {

    if (window.innerWidth < 921)
    {
        Popupwidth = window.innerWidth;
    }


    $(".booking_contents_scroll").height(window.innerHeight - 90);

    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    });
    PickUpDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#PickUpDate')[0], trigger: $('#PickUpDate_custom')[0], firstDay: 1 });
    DropOffDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#DropOffDate')[0], trigger: $('#DropOffDate_custom')[0], firstDay: 1 });

    $(".booking_message_div .StartCount").html($("#BookingMessage").val().length);
    $("#BookingMessage").keyup(function () {
        $(".booking_message_div .StartCount").html($("#BookingMessage").val().length);
    });
    $(".BookingPrintOrPreview").click(function () {
        //if ($(".HasItem").length == 0) {
        //    OpenErrorMessageNew("Error!", "You have to select at least one item to proceed", "");
        //}
        //else {
            $("#BookingPrint").click();
        //}
    });
    $(".BookingPrintAndSend").click(function () {
        ////if ($(".HasItem").length == 0) {
        ////    OpenErrorMessageNew("Error!", "You have to select at least one item to proceed", "");
        ////}

        ////else {
        //    $("#BookingPrintAndSend").click();
        //}
    });
    //Approve Booking Button Action
    $(".btnCloneBooking").click(function () {
        OpenConfirmationMessageNew("Confirmation Required!", "Are you sure, you want to make a duplicate copy of this booking.?", function () {
           

            CloneBooking(Booking_int_Id);
        });
    });
    //Clone Booking Button Action
    $(".ApproveBookingBtn").click(function () {
        OpenConfirmationMessageNew("Confirmation Required!", "Are you sure, you want to approve this booking?", function () {
            ApproveBooking(Booking_int_Id);
        });
    });
    //Decline Button Action
    $(".btnDecline").click(function () {
       // var url = "/Booking/DeclineLeadBookingStatus";
        var idval = $(this).attr('data-id');
        var param = JSON.stringify({
            id: idval,
        });

        $.ajax({
            url: "/Booking/DeclineLeadBookingStatus",
            data: {
                Id: idval
            },
            type: "Post",
            dataType: "Json",
            success: function (data) {
                if (data.result) {
                    OpenSuccessMessageNew("Success!", "Booking Cancelled successfully!");
                    //OpenLeadBookTab();
                    CloseTopToBottomModal();

                } else {
                    OpenErrorMessageNew("Error!", data.message);
                }
            }
        });
    });


    $(".OpenEditBillingAddressModel").click(function () {
        var BillingAddress = "BillingAddress";
        OpenRightToLeftModal(domainurl + "/Booking/OpenEditBillingAddressModel/?id=" + BookingIntId + "&Address=" + encodeURI(BillingAddress) + "&CustomerId=" + CustomerIdForEditBooking + "&BookingId=" + BookingId +"&from=Booking");
    });
    $(".OpenEditPickUpLocationModel").click(function () {
        var BillingAddress = "PickUpLocation";
        OpenRightToLeftModal(domainurl + "/Booking/OpenEditBillingAddressModel/?id=" + BookingIntId + "&Address=" + encodeURI(BillingAddress) + "&CustomerId=" + CustomerIdForEditBooking + "&BookingId=" + BookingId + "&from=Booking");
    });
    $(".OpenEditDropOffLocationModel").click(function () {
        console.log("is working");
        var BillingAddress = "DropOffLocation";
        OpenRightToLeftModal(domainurl + "/Booking/OpenEditBillingAddressModel/?id=" + BookingIntId + "&Address=" + encodeURI(BillingAddress) + "&CustomerId=" + CustomerIdForEditBooking + "&BookingId=" + BookingId + "&from=Booking");
    });
    //Booking Delete Button
    $(".BookingDeleteButton").click(function () {
        OpenConfirmationMessageNew("Delete Confirmation!", "Are you sure you want to delete this booking?", function () {
            DeleteBookingById(Booking_int_Id);
        });
    });

    //Resend Email
    $(".btnResend").click(function () {
        OpenConfirmationMessageNew("Resend Confirmation!", "Are you sure you want to resend this booking?", function () {
            console.log("ascasdc");

            ResendBookingEmail(Booking_int_Id);
        });
    });


    $("#discountAmount").keyup(function () {
        if ($("#discountAmount").val() != "") {
            $(".Discount-total").removeClass('hidden');
        }
        else {
            $(".Discount-total").addClass('hidden');
        }
    });

    $("#EmailAddress").keydown(function () {
        makeSendEmailUrl();
    });
    $("#EmailAddress").keyup(function () {
        makeSendEmailUrl();
    });


    var Popupheight = 600;
    if (Device.All()) {
        Popupwidth = window.innerWidth;
        Popupheight = window.innerHeight;
    }
    var idlist = [{ id: ".BkPreview", type: 'iframe', width: Popupwidth, height: Popupheight }];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".CustomerBkTab tbody").sortable({
        update: function () {
            var i = 1;
            $(".CustomerBkTab tbody tr td:first-child").each(function () {
                $(this).text(i);
                i += 1;
            });
        }
    }).disableSelection();

    InitRowIndex();
    CalculateDiscount();

    $("#CustomerList").focusout(function () {
        setTimeout(function () {
            $(".customer_name_insert_div .tt-menu").hide();
        }, 200);
    });
     
    
    //Booking Save Button Check Coditions 
    $(".BookingSaveButton").click(function () {

        if (($.trim($('.txtProductAmount').val())) != 'NaN' && ($.trim($('.txtProductAmount').val())) != '0.00') {
            SaveBooking();
        } else {
            OpenErrorMessageNew("Error!", "Please enter the Size (Length and Width OR Radius)", "");
        }
        //if ($("#CustomerBkTab .HasItem").length == 0) {
        //    OpenErrorMessageNew("Error!", "You have to select at least one item to proceed", "");
        //}
        //if ($.trim($('.RugShape').val()) == '') {
        //    OpenErrorMessageNew("Error!", "You have to select at least one rug shape", "");
        //}
        //else if ($.trim($('.txtProductPackage').val()) == '') {
        //    OpenErrorMessageNew("Error!", "You have to select at least one package", "");
        //}
        //else if ($("#CustomerBkTab .HasItem").length != 0 && ($.trim($('.RugShape').val())) != '' && ($.trim($('.txtProductPackage').val())) != '') {
          
        //}
    });

    $('#Invoice_ShippingCost').focusout(function () {
        CalculateNewBookingAmount();
    }); 
    
    $("#Booking_TaxType").change(function () { 
        CalculateNewBookingAmount(); 
    });

    CalculateNewBookingAmount();
    $(".RugShape").click(function () {
        SearchKeyUp(this, event);
    });
    $(".txtProductPackage").click(function () {
        PackageSearchKeyUp(this, event);
    });
});

$(window).resize(function () {
    if (window.innerWidth < 921) {
        Popupwidth = window.innerWidth;
    }
    $(".top_to_bottom_modal_container").height(window.height);

    $(".booking_contents_scroll").height(window.height - 55);

});