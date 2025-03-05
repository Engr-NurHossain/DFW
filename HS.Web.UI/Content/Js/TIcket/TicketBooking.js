var InitialBalanceDueAmount = 0;

var RecreateConfirmation = function () {
    OpenConfirmationMessageNew("", "Are you sure you want to recreate invoice?", function () {
        SaveTicketBookingItems(true);
    });
}

var OpenRugCondtionPopup = function (TkBookID) {
    var loadUrl = domainurl + "/Ticket/RugConditionPopup/?DataId=" + TkBookID;
    $(".EquipmentConditionMagnific").attr('href', loadUrl);
    $(".EquipmentConditionMagnific").click();
}

var GetTicketBookingDetailList = function () {
    var DetailList = [];
    $("#CustomerBkTab .HasItem").each(function () {
        DetailList.push({
            Id: $(this).attr('data-id'),
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
    });
    return DetailList;
}
var GetTicketBookingExtraItemList = function () {
    var ExtraItemDetailList = [];
    $(".BookingExtraItems .HasItem").each(function () {
        ExtraItemDetailList.push({
            Id: $(this).attr('data-id'),
            EquipmentId: $(this).attr('data-eq-id'),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', '')),
            TotalPrice: $(this).find('.txtProductQuantity').val() * parseFloat($(this).find('.txtProductRate').val().trim().replaceAll(',', '')),
            EquipDetail: $(this).find('.txtProductDesc').val(),
            EquipName: $(this).find('.ProductName').val(),
            InventoryId: '00000000-0000-0000-0000-000000000000',
            CreatedDate: '1-1-2017',
        });
    });

    return ExtraItemDetailList;
}

var DeleteBookingItems = function (id, ticketid) {
    var url = domainurl + "/Ticket/DeleteBookingItems";
    var param = JSON.stringify({
        id: id,
        ticketid: ticketid
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
            if (data == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", "", function () {
                    OpenTicketById(ticketid);
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var DeleteBookingExtraItems = function (id, ticketid) {
    var url = domainurl + "/Ticket/DeleteBookingExtraItems";
    var param = JSON.stringify({
        id: id,
        ticketid: ticketid
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
            if (data == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", "", function () {
                    OpenTicketById(ticketid);
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var SaveTicketBookingItems = function (RecreateInvoice) {
    var url = "/Ticket/SaveTicketBookingItems";

    var DetailList = GetTicketBookingDetailList();
    var ExtraItemDetailList = GetTicketBookingExtraItemList();
    
    var param = JSON.stringify({
        BookingId: BookingId,
        RecreateInvoice: RecreateInvoice,
        TicketBookingDetails: DetailList,
        TicketBookingExtraItems: ExtraItemDetailList
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
            ReloadTicket()
            if (data.result) {
                OpenSuccessMessageNew("", data.message);
            } else {
                OpenErrorMessageNew("", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var InitialDueAmount = 0;
var InitialTicketAmount = 0;
var BalanceDiffCalcCallCount = 0;

var BalanceDiffCalc = function () {
    if (BalanceDiffCalcCallCount > 0) { //for prevent firing when there is no change
        var AmountChange = parseFloat(TotalAmount.toFixed(2)) - parseFloat(InitialTicketAmount.toFixed(2));
        var NewTotalAmount = parseFloat(InitialDueAmount.toFixed(2)) + parseFloat(AmountChange.toFixed(2));
        if (InitialTicketAmount == 0) {
            AmountChange = 0;
        }
        $(".AmountChanged").text(AmountChange.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        $(".TotalBalanceDueAmount").text(NewTotalAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
    }
    BalanceDiffCalcCallCount++;
}

$(document).ready(function () {
    CalculateNewBookingAmount(); 

    InitialDueAmount = $(".TotalBalanceDueAmount").text();
    
    if (typeof (InitialDueAmount) == "undefined") {
        InitialDueAmount = 0;
    } else {
        InitialDueAmount = parseFloat(InitialDueAmount.replaceAll(",", ""));
    }
    InitialTicketAmount = TotalAmount;

    $(".BookingTotalAmountChanged").removeClass("hidden");

});