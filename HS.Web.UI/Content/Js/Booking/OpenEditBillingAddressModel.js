
var EditBillingAddressOnly = function (From) {

    console.log("Functin e dhuksi");
    var url = domainurl + "/Leads/EditBillingAddressOnly";
    console.log($("#CustomerAddressId").val());
    var CustomerAddressId = $("#CustomerAddressId").val();
   

    console.log(CustomerAddressId);
    var param = {
        "Id": $("#Id").val(),
        "FirstName": $("#FirstName").val(),
        "LastName": $("#LastName").val(),
        "BusinessName": $("#BusinessName").val(),
        "Street": $("#Street").val(),
        "City": $("#City").val(),
        "State": $("#State").val(),
        "ZipCode": $("#ZipCode").val(),
        "CustomerNo": $("#CustomerNo").val(),
        

    };
    var Fparam = JSON.stringify({
        'customer': param,
        'CustomerAddressId': CustomerAddressId
    });
    console.log(param)
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: Fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {

            console.log("Success e asi");
            if (data.result == true) {
                if (From == "Ticket") {
                    $(".customeraddress_style_ticket").html(data.AddressForTicket);
                    $(".close").click()
                }
                else {
                    if (Address == "BillingAddress") {
                        tinyMCE.get('Booking_BillingAddress').setContent(data.BillingAddressVal);
                    }
                    if (Address == "PickUpLocation") {
                        tinyMCE.get('Booking_PickUpLocation').setContent(data.BillingAddressVal);
                    }
                    if (Address == "DropOffLocation") {
                        tinyMCE.get('Booking_DropOffLocation').setContent(data.BillingAddressVal);
                    }
                    $(".close").click()
                    //tinyMCE.get('Booking_ShippingAddress').setContent(data.ShippingAddressVal);
                }
                   
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var loadDiffrentAddress = function (TicketId) {
    $(".DiffrentAddressMagnific").attr('href', '/Ticket/DiffrentAddressPopup?CustomerId=' + CustomerLoadGuid);
    $(".DiffrentAddressMagnific").click();
}
$(document).ready(function () {
    var DiffrentAddresstDom = 500;
    if (window.innerWidth < 500) {
        DiffrentAddresstDom = window.innerWidth;
    }
    $(".bodyForOpenBillingAddress").height(window.innerHeight - 123);
    $("#btnBillingAddressOnly").click(function () {
        EditBillingAddressOnly(From);
    })

    var idlist = [
        { id: ".DiffrentAddressMagnific", type: 'iframe', width: DiffrentAddresstDom, height: 420 },
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $("#DiffrentAddressPopup").click(function () {
        console.log("sdf")
        var LoadUrl = domainurl + "/Ticket/DiffrentAddressPopup?CustomerId=" + CustomerLoadGuid;
        $(".DiffrentAddressMagnific").attr("href", LoadUrl);
        $(".DiffrentAddressMagnific").click();
    });
})
$(window).resize(function () {
    $(".bodyForOpenBillingAddress").height(window.innerHeight - 123);
});

