function printFrame(id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var CreateBookingPdf = function () {
    var DetailList = [];
    parent.$("#CustomerBkTab .HasItem").each(function () {

        if ($(this).find('input.txtRugArea').val() > 0) { 
            DetailList.push({
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find('.txtProductRate').val(),
                TotalPrice: $(this).find('input.txtProductAmount').val(),
                RugType: $(this).find('.RugShape').val(),
                Discount: $(this).find('input.txtProductDiscount').val(),
                Length: $(this).find('.txtRugLength').val(),
                LengthInch: $(this).find('input.txtRugLengthInch').val(),
                Width: $(this).find('.txtRugWidth').val(),
                WidthInch: $(this).find('input.txtRugWidthInch').val(),
                Radius: $(this).find('.txtRugRadius').val(),
                RadiusInch: $(this).find('input.txtRugRadiusInch').val(),
                TotalSize: $(this).find('.txtRugArea').val(),
                Package: $(this).find('.txtProductPackage').val(),
                Included: $(this).find('.txtProductInclude').val(),
                AddedDate: '1-1-2017',
                BookingId: parent.BookingId
            });
        }
    });
    var ExtraItemDetailList = [];
    parent.$("#CustomerEstimateTab .HasItem").each(function () {
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
    var url = "/Booking/SaveBookingPdf";
    var param = JSON.stringify({
        EmailAddress: parent.$("#EmailAddress").val(),
        "Booking.BookingId": parent.BookingId,
        "Booking.BillingAddress": parent.tinyMCE.get('Booking_BillingAddress').getContent(),
        "Booking.PickUpLocation": parent.tinyMCE.get('Booking_PickUpLocation').getContent(),
        "Booking.DropOffLocation": parent.tinyMCE.get('Booking_DropOffLocation').getContent(),
        "Booking.PickUpDate": parent.$("#PickUpDate").val(),
        "Booking.DropOffDate": parent.$("#DropOffDate").val(),
        "Booking.CustomerId": parent.$("#BookingCustomerId").val(),
        "Booking.Amount": parent.TotalAmount,
        "Booking.TotalAmount": parent.TotalAmount,
        "Booking.DiscountAmount": parent.TotalDiscount,
        "Booking.Tax": parent.TotalTax,
        "Booking.TaxType": parent.$("#Booking_TaxType option:selected").text(),
        "Booking.BookingMessage": parent.$("textarea#BookingMessage").val(),
        BookingDetailsList: DetailList,
        BookingExtraItem: ExtraItemDetailList,
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
            $("#iframePdf").attr('src', "/" + data.filePath);
            $("#iframePdf").removeClass('hidden');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    if (typeof (PdfLocation) == 'undefined'
        || PdfLocation.trim() == '' || PdfLocation.trim() == '/') {
        CreateBookingPdf();
    }
});
