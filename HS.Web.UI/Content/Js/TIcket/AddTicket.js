var InvoiceDatepicker;
var DueDatepicker;
var ServiceOrderDate;
var Amount;
var SubTotal;
var TaxTotal;
var DetailList = [];
var CompletionDatepicker = "";
var CompletedDatepicker = "";
var counter = 0;
var AddedEquipmentList = [""];
var RemovedEquipmentList = [""];
var IsStatusChange = false;
var IsInstalChange = false;
var isbilling = false;
var geocoder;
var currentformatedAddress;

var latitude = '';
var longitude = '';
if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(function (position) {
        latitude = position.coords.latitude,
            longitude = position.coords.longitude
    });
}

$(".OpenEditPickUpLocationModel").click(function () {
    var BillingAddress = "PickUpLocation";
    OpenRightToLeftModal(domainurl + "/Booking/OpenEditBillingAddressModel/?Address=PickUpLocation" + "&CustomerId=" + GuidCustomer + "&BookingId=" + BookingId + "&from=Ticket");
});
$(".OpenEditDropOffLocationModel").click(function () {
    console.log("is working");
    var BillingAddress = "DropOffLocation";
    OpenRightToLeftModal(domainurl + "/Booking/OpenEditBillingAddressModel/?Address=" + encodeURI(BillingAddress) + "&CustomerId=" + GuidCustomer + "&BookingId=" + BookingId + "&from=Ticket");
});


$(".OpenEditBillingAddressModel").click(function () {
    var BillingAddress = "BillingAddress";
    OpenRightToLeftModal(domainurl + "/Booking/OpenEditBillingAddressModel/?Address=" + encodeURI(BillingAddress) + "&CustomerId=" + GuidCustomer + "&BookingId=" + BookingId + "&from=Ticket");
});
function successFunction(position) {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;
    codeLatLng(lat, lng)
}

function errorFunction() {
    alert("Geocoder failed");
}

function initialize() {
    $('.tt-menu').hide();
    geocoder = new google.maps.Geocoder();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successFunction, errorFunction);
    }
}
function codeLatLng(lat, lng) {

    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            console.log(results)
            if (results[1]) {
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                        //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate
                        if (results[0].address_components[i].types[b] == "locality") {
                            //this is the object you are looking for
                            currentformatedAddress = results[0].formatted_address;
                            window.open("https://www.google.com/maps?saddr=" + currentformatedAddress.replace(",", "").replace(" ", "+") + "&daddr=" + DestinationCusAddress.replace(",", "").replace(" ", "+"), "_blank");
                            //console.log(results[0]);
                            break;
                        }
                    }
                }
                //city data
                //console.log(city.short_name + " " + city.long_name)
                //alert(city.short_name + " " + city.long_name)
                //$("#city_search_text_index").val(city.short_name);

            } else {
                alert("No results found");
            }
        } else {
            alert("Geocoder failed due to: " + status);
        }
    });
}
var GetDirection = function () {
    initialize();
};

var EditSalesRmrCommission = function (id) {
    $("#labelSalesRmrCommission_"+id).addClass("hidden");
    $("#btnSalesRmrCommission_"+id).addClass("hidden");
    $("#divSalesRmrCommission_"+id).removeClass("hidden");
};
var UpdateSalesRmrCommission = function (id) {
    var url = domainurl + "/Ticket/UpdateSalesRmrCommission";
    var CommissionValue = $("#txtSalesRmrComission_" + id).val();
    var Param = JSON.stringify({
        Id: id,
        CommissionValue: CommissionValue
    });
    $.ajax({
        type: "POST",
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.res == true) {
                OpenSuccessMessageNew("", "", function () {
                    ReloadTicket();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);

        }
    });
};
var ExitSalesRmrCommission = function (id) {
    $("#labelSalesRmrCommission_" + id).removeClass("hidden");
    $("#btnSalesRmrCommission_" + id).removeClass("hidden");
    $("#divSalesRmrCommission_" + id).addClass("hidden");
};

var EditTechRmrCommission = function (id) {
    $("#labelTechRmrCommission_" + id).addClass("hidden");
    $("#btnTechRmrCommission_" + id).addClass("hidden");
    $("#divTechRmrCommission_" + id).removeClass("hidden");
};
var UpdateTechRmrCommission = function (id) {
    var url = domainurl + "/Ticket/UpdateTechRmrCommission";
    var CommissionValue = $("#txtTechRmrComission_" + id).val();
    var Param = JSON.stringify({
        Id: id,
        CommissionValue: CommissionValue
    });
    $.ajax({
        type: "POST",
        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.res == true) {
                OpenSuccessMessageNew("", "", function () {
                    ReloadTicket();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);

        }
    });
};
var ExitTechRmrCommission = function (id) {
    $("#labelTechRmrCommission_" + id).removeClass("hidden");
    $("#btnTechRmrCommission_" + id).removeClass("hidden");
    $("#divTechRmrCommission_" + id).addClass("hidden");
};

var SendTicketEmail = function () {
    var url = domainurl + "/Ticket/SendTicketEmail";
    var Param = JSON.stringify({
        Id: TicketIntId,
        TicketId: TicketId
    });
    $.ajax({
        type: "POST",

        url: url,
        data: Param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                OpenSuccessMessageNew("", "Ticket Pdf sent successfully.", function () { });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);

        }
    });
}
var PropertyUserSuggestiontemplate =
    '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}" data-quantityonhand="{7}" data-suppliercost="{8}" data-retail="{9}" data-sku="{11}" data-point="{12}" data-equipvendorcost="{13}" >'
    //+ "<img src='{7}' class='EquipmentImage'>"
    + "<p class='tt-sug-text'>"
    + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /><em class='tt_sug_manufac'>{10} - {11}</em>"
    + "<em class='tt-eq-price'>${2}</em>"
    + "<br />"
    + "</p> "
    + "</div>";
var ProductRateTD = "";
var RetailPriceTD = "";
var EquipvendorcostTD = "";
var ActionsTD = "";
var NewEquipmentRow1 = "<tr>"
    + "<td valign='top' class='rowindex'></td>";

if (ShowTicketEquipmentReleaseButton == "True") {
    NewEquipmentRow1 += "<td valign='top' style='display: none;'><input type='checkbox' class='chkQuantityValid'/></td>";
}

NewEquipmentRow1 += "<td valign='top'><input type='text'class='ProductName' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event)' />"
    + "<div class='tt-menu'>"
    + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
    + "</div>"
    + "<span class='spnProductName'></span>"
    + "</td>";

NewEquipmentRow1 += "<td valign='top'>"
    + "<p class='spnProductDesc spnSku'></p>"
    + "</td>";
if (ShowDescriptionPermit == "True") {
    NewEquipmentRow1 += "<td valign='top'>"
        + "<input type='text' class='txtProductDesc' />"
        + "<span class='spnProductDesc'></span>"
        + "</td>";
}
if (ShowPointPermit == "True") {
    NewEquipmentRow1 += "<td valign='top'>"
        + "<p class='spnProductPoint'></p>"
        + "</td>";
}
NewEquipmentRow1 += "<td valign='top'>"
    + "<p class='spnProductQuantityOnHand'></p>"
    + "</td>";

if (ShowInstallPermit == "True") {
    NewEquipmentRow1 += "<td valign='top'>"
        + "<p class='spnProductQuantityLeft'></p>"
        + "</td>";
}
NewEquipmentRow1 += "<td valign='top'>"
    + "<input type='number' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
    + "<span class='spnProductQuantity'></span>"
    + "</td>";
var NewServiceRow1 = "<tr class='IsService'>"
    + "<td valign='top' class='rowindex'></td>"
    + "<td valign='top'><input type='text'class='ProductName' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event,\"Service\")' />"
    + "<div class='tt-menu'>"
    + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
    + "</div>"
    + "<span class='spnProductName'></span>"
    + "</td>";
if (ShowDescriptionPermit == "True") {
    NewServiceRow1 += "<td valign='top'>"
        + "<input type='text' class='txtProductDesc' />"
        + "<span class='spnProductDesc'></span>"
        + "</td>";
}

NewServiceRow1 += "<td valign='top'>"
    + "<input type='number' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
    + "<span class='spnProductQuantity'></span>"
    + "</td>";
if (ShowTicketEquipmentCost == "True") {
    NewEquipmentRow1 += "<td valign='top'>"
        + "<p class='spnProductEquipmentvendorcost'></p>"
        + "</td>";
}

if (ShowCost == "True") {
    ProductRateTD = "<td valign='top'>"
        + "<input type='number' onkeydown='OthersKeyDown(this, event)' class='txtProductUnitPrice' />"
        + "<span class='spnProductRate'></span>"
        + "</td>";
} else {
    ProductRateTD = "<td valign='top' class='hidden'>"
        + "<input type='number' onkeydown='OthersKeyDown(this, event)' class='txtProductUnitPrice' />"
        + "<span class='spnProductRate'></span>"
        + "</td>";
}
if (ShowRetailPrice == "True") {
    RetailPriceTD = "<td valign='top'>"
        + "<input type='number' onkeydown='OthersKeyDown(this, event)' class='txtProductAmount' />"
        + "<span class='spnProductAmount'></span>"
        + "</td>";
} else {
    RetailPriceTD = "<td valign='top' class='hidden'>"
        + "<input type='number' onkeydown='OthersKeyDown(this, event)' class='txtProductAmount' />"
        + "<span class='spnProductAmount'></span>"
        + "</td>";
}
//EquipvendorcostTD = "<td valign='top' class='hidden'>"

//    + "<span class='spnProductEquipmentvendorcost'></span>"
//    + "</td>";
ActionsTD = "<td valign='top' class='tableActions'>"
    + "<i class='fa fa-trash-o trash-hidden' aria-hidden='true'></i>"
    + "</td>"
    + "</tr>";
var NewEquipmentRow = NewEquipmentRow1 + ProductRateTD + RetailPriceTD + ActionsTD;
var NewServiceRow = NewServiceRow1 + ProductRateTD + RetailPriceTD + ActionsTD;



String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var SmartSetup = function () {
    window.location.href = "/smartLeadSetup/?id=" + TicketCustomerIntId;
}
var AddSalesCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddSalesCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var AddTechCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddTechCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var AddAddMemberCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddAddMemberCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var AddFinRepCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddFinRepCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var AddServiceCallCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddServiceCallCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var AddFollowUpCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddFollowUpCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var AddRescheduleCommission = function () {
    OpenRightToLeftModal(domainurl + "/Ticket/AddRescheduleCommission/?TicketId=" + TicketId + "&CustomerId=" + GuidCustomer);
}
var updateSalesCommission = function (CustomerId, UserId) {
    var url = domainurl + "/Ticket/UpdateSalesCommission/";
    var param = JSON.stringify({
        CustomerId: CustomerId,
        UserId: UserId,
        SalesCommission: $("#SalesCommission").val()
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
                OpenSuccessMessageNew("Success!", "Sales Commission has been updated successfully !", function () {
                    ReloadTicket();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var updateTechCommission = function (CustomerId, UserId) {
    var url = domainurl + "/Ticket/UpdateTechCommission/";
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data);
            if (data == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", "Tech Commission has been updated successfully !", function () {
                    ReloadTicket();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var SendAgreementPopup = function (from) {
    OpenTopToBottomModal("/Ticket/SendAgreementPopup?CustomerId=" + GuidCustomer + "&TicketId=" + TicketIntId + "&from=" + from);
}

var RecreateAgreement = function (id, ticketid) {
    //SaveServiceOrderEquipmentDetailsFromTicket(EquipmentTypes.Equipment);
    //SaveServiceOrderEquipmentDetailsFromTicket(EquipmentTypes.Service);
    //var Param = {
    //    "TicketId": TicketId,
    //    "CustomerId": GuidCustomer
    //};
    //var url = domainurl + "/Ticket/RecreateAgreement";
    //$.ajax({
    //    type: "POST",
    //    url: url,
    //    data: JSON.stringify(Param),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    cache: false,
    //    success: function (data) {
    //        if (data) {
    //            $("#InstallationAgreement").click();
    //        }
    //        else {
    //            OpenErrorMessageNew("Error!", data.message);
    //        }
    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        $(".ticket-loader-div").hide();
    //        console.log(errorThrown);
    //    }
    //});
    var loadUrl = domainurl + "/SmartLeads/GetSmartLeadsForPopUp?LeadId=" + TicketCustomerIntId + "&grant=false" + "&templateid=0" + "&firstpage=false&ticketid=" + ticketid + "&recreate=true";
    $(".FirstPageAgreementDocument").attr('href', loadUrl);
    $(".FirstPageAgreementDocument").click();
}
var CloneTicketConfirm = function () {
    OpenConfirmationMessageNew("", "Do you want to clone this ticket?", function () {
        CloneTicket();
    })
}
var RescheduleTicketConfirmation = function () {
    OpenConfirmationMessageNew("Confirmation", "Do you want to follow up this ticket?", function () {
        RescheduleTicketConfirm();
    });
}
var CloneTicket = function () {
    var Param = {
        "ticketId": TicketIntId
    };
    var url = domainurl + "/Ticket/CloneTicket";
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result && data.ticketId > 0) {
                OpenSuccessMessageNew("Success!", "", function () {
                    OpenTicketTab();
                    OpenTicketById(data.ticketId);
                });
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".ticket-loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var RescheduleTicketConfirm = function () {
    var DetailList = [];
    $("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").each(function () {
        var InstallQty = 0;
        if (parseInt($(this).find('.txtProductQuantity').val()) < parseInt($(this).find('.txtProductQuantityLeft').val())) {
            
            InstallQty = parseInt($(this).find('.txtProductQuantityLeft').val());
        }
        else {
            InstallQty = parseInt($(this).find('.txtProductQuantity').val()) - parseInt($(this).find('.txtProductQuantityLeft').val());
        }
        if ($(this).find('.chkQuantityValid').prop('checked')) {
            DetailList.push({
                Id: $(this).attr('data-int-id'),
                EquipmentId: $(this).attr('data-id'),
                CreatedByUid: $(this).attr('user-id'),
                EquipName: $(this).find('.ProductName').val(),
                EquipDetail: $(this).find('.txtProductDesc').val(),
                Quantity: InstallQty,
                UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                TotalPrice: $(this).find('.txtProductAmount').val(),
                EquipmentName: $(this).find('.ProductName').val(),
                TechnicianId: $("#AssignedTo").val(),
                CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                QuantityLeftEquipment: 0,
                IsService: $(this).hasClass('IsService'),
                InventoryTechQty: $(this).find('.spnProductQuantityLeft').text(),
                IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                IsBilling: $(this).find('.spnIsBilling').text(),
                IsCopied: $(this).find('.spnIsCopied').text(),
                IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
            });
        }
        else {
            DetailList.push({
                Id: $(this).attr('data-int-id'),
                EquipmentId: $(this).attr('data-id'),
                CreatedByUid: $(this).attr('user-id'),
                EquipName: $(this).find('.ProductName').val(),
                EquipDetail: $(this).find('.txtProductDesc').val(),
                Quantity: InstallQty,
                UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                TotalPrice: $(this).find('.txtProductAmount').val(),
                EquipmentName: $(this).find('.ProductName').val(),
                TechnicianId: $("#AssignedTo").val(),
                CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                QuantityLeftEquipment: 0,
                IsService: $(this).hasClass('IsService'),
                IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                IsBilling: $(this).find('.spnIsBilling').text(),
                IsCopied: $(this).find('.spnIsCopied').text(),
                IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
            });
        }
    });
    var Param = {
        "ticketId": TicketIntId,
        "EquipmentDetailList": DetailList
    };
    var url = domainurl + "/Ticket/RescheduleTicket";
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "", function () {
                    OpenTicketTab();
                    OpenTicketById(data.ticketId);

                });
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".ticket-loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var ErrorCallBack = function () {
    SaveClockInOut(ClockInOutType, pos);
}
var ClockInOut = function (Type) {
    console.log("ClockInOut");
    if (Type == 'start') {
        if ($(".timer.watch").text() == "") {
            $('.timer').countimer('start');
        } else {
            $('.timer').countimer('resume');
        }
        ClockInOutType = Type;
    }
    else {
        $('.timer').countimer('stop');
        ClockInOutType = 'end';
    }
    if (location.protocol == "https:") {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                SaveClockInOut(ClockInOutType, pos);
            }, ErrorCallBack);
        } else {
            SaveClockInOut(ClockInOutType, pos);
        }
    }
    else {
        SaveClockInOut(ClockInOutType, pos);
    }

}
var SaveClockInOut = function (Type, POS) {
    var Param = {
        UserId: GuidCustomer,
        Time: '1-1-2018',
        Lat: POS.lat,
        Lng: POS.lng,
        Type: Type,
        TicketId: TicketId
    };
    var url = domainurl + "/Ticket/JobTimeStartStop";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                if (Type == "start") {
                    $(".jobstart").hide();
                    $(".jobend").show();

                } else {
                    $(".jobend").hide();
                    $(".jobstart").show();
                }
            }
            else {

                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var TimerDocReady = function () {
    if (isClockIn == "True") {
        $('.timer').countimer({
            autoStart: false,
            initHours: Hour,
            initMinutes: Minute,
            initSeconds: Second
        });
    }
    else {
        $('.timer').countimer({
            autoStart: true,
            initHours: Hour,
            initMinutes: Minute,
            initSeconds: Second
        });
    }
    if (isClockIn == "True") {
        $(".jobend").hide();
        $(".jobstart").show();
    }
    else {
        $(".jobstart").hide();
        $(".jobend").show();
    }
}

var OpenTextAndAmmountModal = function (HeaderMessage, BodyMessage, ToDoFunc) {

    $("#ModalOpenText .message_header_title").text("Enter your amount and description");
    $("#ModalOpenTextAndAmmount p").text(BodyMessage);
    if (typeof (ToDoFunc) == "function") {
        console.log("func : " + ToDoFunc);
        DefaultSuccessfunc = ToDoFunc;
        $(".close").unbind();

    } else {
        DefaultSuccessfunc = function () { };
    }
    $("#OpenTextAndAmmountModal").click();
}


var CloseTopToBottomTicket = function () {
    console.log("checkclose");
    if (location.href.toLocaleLowerCase().indexOf('/scheduleinfo?date=') > -1) {
        var NewLocation = domainurl + '/Customer/Customerdetail/?id=' + TicketCustomerIntId + '#TicketTab';
        location.href = NewLocation;
    }
    var pathname = window.location.pathname.toLowerCase();
    if (pathname == '/calendar') {
        window.location.href = "/calendar?ticketdate=" + $("#Ticket_CompletionDate").val();
    }
    $(".modal-backdrop").remove();
    CloseTopToBottomModal();
}
var GetTimeFormat = function (date) {
    var time = "00:00:00";
    return (date + ' ' + time);
}
var ReloadTicket = function () {
    console.log("reload ticket");
    if (typeof (OpenTicketTab) != 'undefined') {
        OpenTicketTab();
    }
    if (typeof (OpenLeadEstimateTab) != 'undefined') {
        OpenLeadEstimateTab();
    }
    $(".TopToBottomModal .top_to_bottom_modal_container .ContentsDiv").html(TabsLoaderText);
    $(".TopToBottomModal .top_to_bottom_modal_container .ContentsDiv").load("/Ticket/AddTicket/?Id=" + TicketIntId);
}
var AddTicketConfirmation = function (IsDispatch, RecreateInvoice) {
    console.log("confirmation");

    var CusTicketType = $("#Ticket_TicketType").val();
    if (typeof (CusTicketType) == "undefined") {
        CusTicketType = TicketType;
    }
    var TicketStatus = $("#Ticket_Status").val();
    var IsOpen = $("#StatusToogle").prop("checked");
    //if ((CusTicketType == "Installation" || CusTicketType == "Service" || CusTicketType == "Inspection") && TicketStatus == "Completed") {
    //    if (EquipmentQuantityValidationAll()) {
    //        OpenConfirmationMessageNew("", " Do you want to create invoice ?", function () {
    //            OpenTextAndAmmountModal("", "", function () {
    //                var InvDes = $("#InvDes").val();
    //                var InvAmmount = $("#InvAmmount").val();
    //                AddTicketWithInv(InvDes, InvAmmount)
    //            })

    //        }, function () {
    //            AddTicket();
    //        });
    //    }
    //    else {
    //        OpenErrorMessageNew("Error!", "Not enough equipment quantity on hand");
    //    }
    //}
    //else {
    //    AddTicket();
    //}
    if (CommonUiValidation()) {
        AddTicket(IsDispatch, RecreateInvoice);
    }
}

var CreateInvoiceTicketAddItem = function () {
    OpenConfirmationMessageNew("Confirmation", "Do you want to create invoice?", function () {
        SaveInvoiceTicketAddItem();
    });
}
var ajaxLoading = false;
var AddTicket = function (IsDispatch, RecreateInvoice, SendNotification) {
    AddTicketReply();
    console.log(IsDispatch);
    if (typeof (IsDispatch) == "undefined") {
        IsDispatch = false;
    }
    if (typeof (RecreateInvoice) == "undefined") {
        RecreateInvoice = false;
    }
    var DetailList = [];
    var IsClosed = false;
    var TicketBookingId = "";
    if (typeof (BookingId) != "undefined") {
        TicketBookingId = BookingId
    }
    var TicketBookingDetailsList = [];
    if (typeof (GetTicketBookingDetailList) != "undefined") {
        TicketBookingDetailsList = GetTicketBookingDetailList();
    }
    var TicketBookingExtraItemList = [];
    if (typeof (GetTicketBookingExtraItemList) != "undefined") {
        TicketBookingExtraItemList = GetTicketBookingExtraItemList();
    }
    if (typeof (SendNotification) == "undefined") {
        SendNotification = false;
    }
    if ($("#StatusToogle").prop("checked") != undefined) {
        IsClosed = !$("#StatusToogle").prop("checked");
    }
    $("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").each(function () {
        if ($(this).attr('data-release') == 'True') {
            DetailList.push({
                Id: $(this).attr('data-int-id'),
                EquipmentId: $(this).attr('data-id'),
                CreatedByUid: $(this).attr('user-id'),
                EquipName: $(this).find('.ProductName').val(),
                EquipDetail: $(this).find('.txtProductDesc').val(),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                TotalPrice: $(this).find('.txtProductAmount').val(),
                EquipmentName: $(this).find('.ProductName').val(),
                TechnicianId: $("#AssignedTo").val(),
                CheckedEqp: true,
                IsService: $(this).hasClass('IsService'),
                IsEquipmentRelease: true,
                QuantityLeftEquipment: $(this).find('.txtProductQuantityLeft').val(),
                IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                IsBilling: $(this).find('.spnIsBilling').text(),
                IsCopied: $(this).find('.spnIsCopied').text(),
                IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
            });
        }
        else {
            if ($(this).find('.chkQuantityValid').prop('checked')) {
                DetailList.push({
                    Id: $(this).attr('data-int-id'),
                    EquipmentId: $(this).attr('data-id'),
                    CreatedByUid: $(this).attr('user-id'),
                    EquipName: $(this).find('.ProductName').val(),
                    EquipDetail: $(this).find('.txtProductDesc').val(),
                    Quantity: $(this).find('.txtProductQuantity').val(),
                    UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                    TotalPrice: $(this).find('.txtProductAmount').val(),
                    EquipmentName: $(this).find('.ProductName').val(),
                    TechnicianId: $("#AssignedTo").val(),
                    CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                    QuantityLeftEquipment: $(this).find('.txtProductQuantityLeft').val(),
                    IsService: $(this).hasClass('IsService'),
                    IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                    IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                    ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                    ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                    IsBilling: $(this).find('.spnIsBilling').text(),
                    IsCopied: $(this).find('.spnIsCopied').text(),
                    IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
                });
            }
            else {
                DetailList.push({
                    Id: $(this).attr('data-int-id'),
                    EquipmentId: $(this).attr('data-id'),
                    CreatedByUid: $(this).attr('user-id'),
                    EquipName: $(this).find('.ProductName').val(),
                    EquipDetail: $(this).find('.txtProductDesc').val(),
                    Quantity: $(this).find('.txtProductQuantity').val(),
                    UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                    TotalPrice: $(this).find('.txtProductAmount').val(),
                    EquipmentName: $(this).find('.ProductName').val(),
                    TechnicianId: $("#AssignedTo").val(),
                    CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                    QuantityLeftEquipment: $(this).find('.txtProductQuantityLeft').val(),
                    IsService: $(this).hasClass('IsService'),
                    IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                    IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                    ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                    ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                    IsBilling: $(this).find('.spnIsBilling').text(),
                    IsCopied: $(this).find('.spnIsCopied').text(),
                    IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
                });
            }
        }
    });
    
    //var ErrorCallBack = function () {
    //    return false;
    //}
    

    var Param = {
        "Ticket.Id": $("#Ticket_Id").val(),
        "Ticket.TicketId": $("#Ticket_TicketId").val(),
        "Ticket.Subject": $("#Ticket_Subject").val(),
        "Ticket.Message": tinyMCE.get('BodyContent').getContent(),
        "Ticket.Status": $("#Ticket_Status").val(),
        "Ticket.CustomerId": GuidCustomer,
        "Ticket.TicketType": $("#Ticket_TicketType").val(),
        "Ticket.CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "Ticket.CompletedDate": GetTimeFormat($("#Ticket_CompletedDate").val()),
        "Ticket.AppointmentStartTime": $("#AppointmentStartTime").val(),
        "Ticket.AppointmentEndTime": $("#AppointmentEndTime").val(),
        "Ticket.RackNo": $("#Ticket_RackNo").val(),
        "Ticket.Locations": $("#Ticket_Locations").val(),
        "Ticket.MiscName": $("#MiscName").val(),
        "Ticket.MiscValue": $("#MiscValue").val(),
        "Ticket.IsAgreementTicket": $("#IsAgreementTicket").prop("checked"),
        "Ticket.IsClosed": IsClosed,
        IsDispatch: IsDispatch,
        NotifyCustomer: $("#NotifyCustomer").is(":checked"),
        Assigned: $("#AssignedTo").val(),
        UserList: $("#AdditionalMembers").val(),
        NotifyingUserList: $("#NotifyingMembers").val(),
        ReasonList: $("#ReasonMembers").val(),
        "CustomerAppointment.CustomerAppointmentEquipmentList": DetailList,
        /*AddedEquipmentList: AddedEquipmentList,
        RemovedEquipmentList: RemovedEquipmentList,*/
        IsStatusChange: IsStatusChange,
        BookingId: TicketBookingId,
        TicketBookingDetails: TicketBookingDetailsList,
        TicketBookingExtraItems: TicketBookingExtraItemList,
        RecreateInvoice: RecreateInvoice,
        Lat: latitude,
        Lng: longitude,
    };
    var url = domainurl + "/Ticket/AddTicket";
    if (!ajaxLoading) {
        ajaxLoading = true;
        $.ajax({
            type: "POST",
            ajaxStart: $(".ticket-loader-div").show(),
            url: url,
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                ajaxLoading = false;
                console.log("hlw");
                if (data.result) {
                    if (!SendNotification) {
                        OpenSuccessMessageNew("Success!", data.message);
                    }
                    else {
                        var ConditionWidth2 = 600;
                        if (window.innerWidth < 600) {
                            ConditionWidth2 = window.innerWidth;
                        }
                        var ConditionHeight2 = 239;
                        if (window.innerHeight < 239) {
                            ConditionHeight2 = window.innerHeight;
                        }
                        var idlistnotification = [{ id: ".SendNotification", type: 'iframe', width: ConditionWidth2, height: ConditionHeight2 },
                        ];
                        jQuery.each(idlistnotification, function (i, val) {
                            magnificPopupObj(val);
                        });
                        $(".SendNotification").click();
                    }
                    $("#ModalSuccessMessage .success_close").text("Close");
                    TicketIntId = data.TicketId;
                    ReloadTicket();
                }
                else {
                    $(".ticket-loader-div").hide();
                    OpenErrorMessageNew("Error!", data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                ajaxLoading = false;
                console.log(errorThrown);
            }
        });
    }
}
var CheckScheduleConflict = function () {
    var DetailList = [];
    var Param = {
        "Ticket.Id": $("#Ticket_Id").val(),
        "Ticket.TicketId": $("#Ticket_TicketId").val(),
        "Ticket.Status": $("#Ticket_Status").val(),
        "Ticket.CustomerId": GuidCustomer,
        "Ticket.TicketType": $("#Ticket_TicketType").val(),
        "Ticket.CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "Ticket.AppointmentStartTime": $("#AppointmentStartTime").val(),
        "Ticket.AppointmentEndTime": $("#AppointmentEndTime").val(),
        "Ticket.IsClosed": IsClosed,
        Assigned: $("#AssignedTo").val(),
        UserList: $("#AdditionalMembers").val(),
    };
    var url = domainurl + "/Ticket/CheckScheduleConflict";
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".ticket-loader-div").hide();
            if (!data.result) {
                OpenErrorMessageNew("", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".ticket-loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var OpenInvById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?Id=" + invId);
        UpdateCustomerTabCounter();
    }
}
var ReceivePaymentByInv = function (invId) {
    if (typeof (CustomerLoadId) == "undefined") {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?InvoiceId=" + invId);
    } else {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + CustomerLoadId + "&InvoiceId=" + invId);
    }

}
var AddTicketWithInv = function (InvDes, InvAmmount) {
    /*save Ticket and create an invoice for the job */
    var DetailList = [];
    var IsClosed = false;
    if ($("#StatusToogle").prop("checked") != undefined) {
        IsClosed = !$("#StatusToogle").prop("checked");
    }
    $("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").each(function () {
        if ($(this).find('.chkQuantityValid').prop('checked')) {
            DetailList.push({
                Id: $(this).attr('data-int-id'),
                EquipmentId: $(this).attr('data-id'),
                EquipName: $(this).find('.ProductName').val(),
                CreatedByUid: $(this).attr('user-id'),
                EquipDetail: $(this).find('.txtProductDesc').val(),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                TotalPrice: $(this).find('.txtProductAmount').val(),
                EquipmentName: $(this).find('.ProductName').val(),
                TechnicianId: $("#AssignedTo").val(),
                CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                QuantityLeftEquipment: $(this).find('.txtProductQuantityLeft').val(),
                IsService: $(this).hasClass('IsService'),
                InventoryTechQty: $(this).find('.spnProductQuantityLeft').text(),
                IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                IsBilling: $(this).find('.spnIsBilling').text(),
                IsCopied: $(this).find('.spnIsCopied').text(),
                IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
            });
        }
        else {
            DetailList.push({
                Id: $(this).attr('data-int-id'),
                EquipmentId: $(this).attr('data-id'),
                EquipName: $(this).find('.ProductName').val(),
                CreatedByUid: $(this).attr('user-id'),
                EquipDetail: $(this).find('.txtProductDesc').val(),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                TotalPrice: $(this).find('.txtProductAmount').val(),
                EquipmentName: $(this).find('.ProductName').val(),
                TechnicianId: $("#AssignedTo").val(),
                CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                QuantityLeftEquipment: $(this).find('.txtProductQuantityLeft').val(),
                IsService: $(this).hasClass('IsService'),
                IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                IsBilling: $(this).find('.spnIsBilling').text(),
                IsCopied: $(this).find('.spnIsCopied').text(),
                IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
            });
        }
    });
    var Param = {
        "Ticket.Id": $("#Ticket_Id").val(),
        "Ticket.TicketId": $("#Ticket_TicketId").val(),
        "Ticket.Subject": $("#Ticket_Subject").val(),
        "Ticket.Message": tinyMCE.get('BodyContent').getContent(),
        "Ticket.Status": $("#Ticket_Status").val(),
        "Ticket.CustomerId": GuidCustomer,
        "Ticket.TicketType": $("#Ticket_TicketType").val(),
        "Ticket.CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "Ticket.CompletedDate": GetTimeFormat($("#Ticket_CompletedDate").val()),
        "Ticket.AppointmentStartTime": $("#AppointmentStartTime").val(),
        "Ticket.AppointmentEndTime": $("#AppointmentEndTime").val(),
        "Ticket.IsClosed": IsClosed,
        Assigned: $("#AssignedTo").val(),
        UserList: $("#AdditionalMembers").val(),
        NotifyingUserList: $("#NotifyingMembers").val(),
        ReasonList: $("#ReasonMembers").val(),

        amount: InvAmmount,
        desc: InvDes,
        "CustomerAppointment.CustomerAppointmentEquipmentList": DetailList,
        AddedEquipmentList: AddedEquipmentList,
        RemovedEquipmentList: RemovedEquipmentList,
        Lat: latitude,
        Lng: longitude,
    };
    var url = domainurl + "/Ticket/AddTicket";
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log("hlw");

            if (data.result) {

                console.log("hlww");

                OpenSuccessMessageNew("Success!", data.message, function () {
                    TicketIntId = data.TicketId;
                    if (data.InvId > 0) {

                        OpenInvById(data.InvId);

                    }
                    else {
                        ReloadTicket();
                    }

                });
            }
            else {
                $(".ticket-loader-div").hide();
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var SaveInvoiceTicketAddItem = function () {
    //Save Ticket with Equipment and service Items
    var DetailList = [];

    $("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").each(function () {
        console.log($(this).find('.spnIsInvoiceCreate').text());
        if ($(this).find('.spnIsInvoiceCreate').text() == "False") {
            DetailList.push({
                Id: $(this).attr('data-int-id'),
                EquipmentId: $(this).attr('data-id'),
                EquipName: $(this).find('.ProductName').val(),
                CreatedByUid: $(this).attr('user-id'),
                EquipDetail: $(this).find('.txtProductDesc').val(),
                Quantity: $(this).find('.txtProductQuantity').val(),
                UnitPrice: $(this).find(".txtProductUnitPrice").val(),
                TotalPrice: $(this).find('.txtProductAmount').val(),
                EquipmentName: $(this).find('.ProductName').val(),
                TechnicianId: $("#AssignedTo").val(),
                CheckedEqp: $(this).find('.chkQuantityValid').prop('checked'),
                QuantityLeftEquipment: $(this).find('.txtProductQuantityLeft').val(),
                IsService: $(this).hasClass('IsService'),
                InventoryTechQty: $(this).find('.spnProductQuantityLeft').text(),
                IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
                IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
                ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
                ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
                IsBilling: $(this).find('.spnIsBilling').text(),
                IsCopied: $(this).find('.spnIsCopied').text(),
                IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
            });
        }
    });
    var Param = {
        "Ticket.Id": $("#Ticket_Id").val(),
        "Ticket.TicketId": $("#Ticket_TicketId").val(),
        "Ticket.Subject": $("#Ticket_Subject").val(),
        "Ticket.Message": tinyMCE.get('BodyContent').getContent(),
        "Ticket.Status": $("#Ticket_Status").val(),
        "Ticket.CustomerId": GuidCustomer,
        "Ticket.TicketType": $("#Ticket_TicketType").val(),
        "Ticket.CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "Ticket.CompletedDate": GetTimeFormat($("#Ticket_CompletedDate").val()),
        "Ticket.AppointmentStartTime": $("#AppointmentStartTime").val(),
        "Ticket.AppointmentEndTime": $("#AppointmentEndTime").val(),
        "Ticket.IsClosed": IsClosed,
        Assigned: $("#AssignedTo").val(),
        UserList: $("#AdditionalMembers").val(),
        NotifyingUserList: $("#NotifyingMembers").val(),
        ReasonList: $("#ReasonMembers").val(),

        "CustomerAppointment.CustomerAppointmentEquipmentList": DetailList,
        AddedEquipmentList: AddedEquipmentList,
        RemovedEquipmentList: RemovedEquipmentList,
    };

    var url = domainurl + "/Ticket/CreateInvoiceTicketAddItem";
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    TicketIntId = data.TicketId;
                    if (data.Invoiceid > 0) {
                        OpenInvById(data.Invoiceid);
                    }
                    else {
                        ReloadTicket();
                    }

                });
            }
            else {
                $(".ticket-loader-div").hide();
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var CustomerCreditForTicketInvoice = function (id) {
    var url = domainurl + "/Ticket/CustomerCreditForTicketInvoice";
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: JSON.stringify({ appointeqpid: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                ReloadTicket();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var AddTicketReply = function () {

    console.log("asdasfasasfas");
    if (typeof ($("#privetmsg").val()) != "undefined") {
        var Param = {
            "TicketId": $("#Ticket_TicketId").val(),
            "isPrivet": $("#privetmsg").is(":checked"),
            "Message": tinyMCE.get('BodyContent').getContent(),
            "showoverview": $("#overview_show").prop('checked'),
            "AppointmentStartTime": $("#AppointmentStartTime").val(),
            "AppointmentEndTime": $("#AppointmentEndTime").val(),
        };
    }
    else {
        var Param = {
            "TicketId": $("#Ticket_TicketId").val(),
            "isPrivet": false,
            "Message": tinyMCE.get('BodyContent').getContent(),
            "showoverview": $("#overview_show").prop('checked'),
            "AppointmentStartTime": $("#AppointmentStartTime").val(),
            "AppointmentEndTime": $("#AppointmentEndTime").val(),
        };
    }

    var url = domainurl + "/Ticket/AddTicketReply";
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            //if (data.result) {
            //    OpenSuccessMessageNew("Success!", data.message, function () {

            //        //CloseTopToBottomModal();
            //        ReloadTicket();
            //    });
            //}
            //else {
            //    OpenErrorMessageNew("Error!", data.message);
            //}
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".ticket-loader-div").hide();
            console.log(errorThrown);
        }
    });

}

var OpenTicketInvoice = function (invoiceid) {
    if (typeof (invoiceid) != "undefined") {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?InvoiceId=" + invoiceid);
    }
}
var OpenTicketEstimate = function (EstimateId) {
    if (typeof (EstimateId) != "undefined") {
        OpenTopToBottomModal(domainurl + "/Estimate/AddEstimate/?InvoiceId=" + EstimateId);
    }
}
var AddAttachment = function (invoiceid) {
    var Param = {
        "TicketId": $("#Ticket_TicketId").val(),
        "InvoiceId": invoiceid,
    };
    var url = domainurl + "/Ticket/AttachTicketInvoie";
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    //OpenTicketTab();
                    //CloseTopToBottomModal();
                    ReloadTicket();
                });
            }
            else {
                $(".ticket-loader-div").hide();
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".ticket-loader-div").hide();
            console.log(errorThrown);
        }
    });

}

var InitUploader = function () {
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/File/UploadTicketFile/?TicketId=' + TicketId,
        dataType: 'json',
        formData: { FileDescription: $("#FileDescription").val() },
        add: function (e, data) {
            if ($("#FileDescription").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#FileDescription").val(filename);
            }
            $("#SelectedFileName").text(data.files[0].name);
            UserFileUploadjqXHRData = data;
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
            UserFileUploadjqXHRData = null;
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);
            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                //$("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);
                OpenSuccessMessageNew("Success!", data.result.message, function () {
                    //OpenTicketTab();
                    //CloseTopToBottomModal();
                    ReloadTicket();
                });
            } else {
                OpenErrorMessageNew("Error!", data.result.message);
            }

        },
        fail: function (event, data) {
            if (data.files[0].error) {
                console.log(data.files[0].error);
            }
            UserFileUploadjqXHRData = null;
        }
    });
}

var EquipmentQuantityValidationAll = function () {
    return true;
    console.log("EquipmentQuantityValidationAll");
    //var result = false;
    //if (!$("#CustomerInvoiceTab .HasItem").is(":visible")) {
    //    result = true;
    //}
    //$("#CustomerInvoiceTab .HasItem").each(function () {
    //    if ($(this).attr('data-release') == "True") {
    //        return true;
    //    }
        
    //    var Quantity = parseInt('0');
    //    var QuantityOnHand = parseInt('0');
    //    if ($(this).find('.txtProductQuantity').val() != null && $(this).find('.txtProductQuantity').val() != undefined) {
    //        Quantity = parseInt($(this).find('.txtProductQuantity').val());
    //    }
    //    if ($(this).find('.spnProductQuantityOnHand').text() != null && $(this).find('.spnProductQuantityOnHand').text() != undefined) {
    //        QuantityOnHand = parseInt($(this).find('.spnProductQuantityOnHand').text());
    //    }
    //    if (Quantity > QuantityOnHand) {
    //        result = false;
    //        //return result;
    //    }
    //    else {
    //        result = true;
    //    }
    //});
    //return result;
}
var onHandValidation = function () {
    var result = false;
    if (!$("#CustomerInvoiceTab .HasItem").is(":visible")) {
        result = true;
    }

    $("#CustomerInvoiceTab .HasItem").each(function () {
             
        var QuantityOnHand = parseInt('0');
        var data_new_added = $(this).find('.spnProductQuantityOnHand').attr('data-new-added');
        var data_is_equipexist = $(this).find('.spnProductQuantityOnHand').attr('data-is-equipexist');
           
            if ($(this).find('.spnProductQuantityOnHand').text() != null && $(this).find('.spnProductQuantityOnHand').text() != undefined) {
                QuantityOnHand = parseInt($(this).find('.spnProductQuantityOnHand').text());
        }
        if (data_is_equipexist == '0') {
            if (QuantityOnHand < 0) {
                result = false;
                return result;
            }
            else {
                result = true;
            }
        }
        else { result = true; } 
    });
    return result;
}
var EquipmentQuantityValidation = function () {
    return true;
    console.log("EquipmentQuantityValidation");
    //var result = false;
    //if (!$("#CustomerInvoiceTab .HasItem").is(":visible")) {
    //    result = true;
    //}
    //var ReleasedItem = 0;
    //$("#CustomerInvoiceTab .HasItem").each(function () {
    //    if ($(this).attr('data-release') == "True") {
    //        ReleasedItem++;
    //        result = true;
    //    }
    //    if ($(this).find('.chkQuantityValid').prop("checked")) {
    //        var Quantity = parseInt('0');
    //        var QuantityOnHand = parseInt('0');
    //        if ($(this).find('.txtProductQuantity').val() != null && $(this).find('.txtProductQuantity').val() != undefined) {
    //            Quantity = parseInt($(this).find('.txtProductQuantity').val());
    //        }
    //        if ($(this).find('.spnProductQuantityOnHand').text() != null && $(this).find('.spnProductQuantityOnHand').text() != undefined) {
    //            QuantityOnHand = parseInt($(this).find('.spnProductQuantityOnHand').text());
    //        }
    //        if (Quantity > QuantityOnHand) {
    //            result = false;
    //            return result;
    //        }
    //        else {
    //            result = true;
    //        }
    //    }
    //    else {
    //        result = true;
    //    }
    //});
    //return result;
}
var QuantityValidation = function () {
    
    console.log("QuantityValidation");
    var ticketStatus = $('#Ticket_Status').val();
    var result = false;
    if (!$("#CustomerInvoiceTab .HasItem").is(":visible")) {
        result = true;
    }
    var ReleasedItem = 0;
    $("#CustomerInvoiceTab .HasItem").each(function () {
        console.log('===Validation===');
        console.log($(this));
        var drelease = $(this).attr('data-release');
        var dnew = $(this).find('.spnProductQuantityOnHand').attr('data-new-added');
        var dexists = $(this).find('.spnProductQuantityOnHand').attr('data-is-equipexist');
        var donhand = $(this).find('.spnProductQuantityOnHand').attr('data-onHand');
        var dqtyleft = parseInt($(this).find('.spnProductQuantityLeft').attr('data-qtyLeft'));
        var psku = $(this).find('.spnProductDesc').html();
        console.log('Release:', drelease, ' New ', dnew, ' Exists ', dexists, ' OnHand ', donhand, ' Qty Left', dqtyleft, ' SKU ', psku);
        if ($(this).attr('data-release') == "True") {
            ReleasedItem++;
            result = true;
        }
        var data_new_added = $(this).find('.spnProductQuantityOnHand').attr('data-new-added');
        var data_is_equipexist = $(this).find('.spnProductQuantityOnHand').attr('data-is-equipexist');
        if (data_is_equipexist == '0') {
            var Quantity = parseInt('0');
            var QuantityOnHand = parseInt('0');
            if ($(this).find('.txtProductQuantityLeft').val() != null && $(this).find('.txtProductQuantityLeft').val() != undefined) {
                Quantity = parseInt($(this).find('.txtProductQuantityLeft').val());
                console.log('Quantity', Quantity);
            }
            if ($(this).find('.spnProductQuantityOnHand').text() != null && $(this).find('.spnProductQuantityOnHand').text() != undefined) {
                QuantityOnHand = parseInt($(this).find('.spnProductQuantityOnHand').text());
                console.log('On Hand', QuantityOnHand);
            }
            if (QuantityOnHand < (Quantity-dqtyleft) && (data_new_added == "1" || typeof $(this).attr('data-release') === "undefined")) {
                console.log('Others');
                result = false;
                return result;
            }
            else {
                result = true;
            }
        }
        else if (data_is_equipexist == '1') {

            if ($(this).find('.txtProductQuantityLeft').val() != null && $(this).find('.txtProductQuantityLeft').val() != undefined) {
                Quantity = parseInt($(this).find('.txtProductQuantityLeft').val());
                console.log('Quantity', Quantity);
            }
            if ($(this).find('.spnProductQuantityOnHand').text() != null && $(this).find('.spnProductQuantityOnHand').text() != undefined) {
                QuantityOnHand = parseInt($(this).find('.spnProductQuantityOnHand').text());
                console.log('On Hand', QuantityOnHand);
            }
            result = true;

        }



    });
    return result;
}

var SaveServiceOrderEquipmentDetailsFromTicket = function (EquipmentType) {
    var url = domainurl + "/Ticket/UpdateCustomerAppoinment";
    var DetailList = [];
    var param;
    var ParentTable = "#CustomerInvoiceTab ";
    if (EquipmentType == EquipmentTypes.Service) {
        ParentTable = "#CustomerServiceTable ";
    }
    $(ParentTable + ".HasItem").each(function () {
        DetailList.push({
            Id: $(this).attr('data-int-id'),
            EquipmentId: $(this).attr('data-id'),
            CreatedByUid: $(this).attr('user-id'),
            EquipName: $(this).find('.ProductName').val(),
            EquipDetail: $(this).find('.txtProductDesc').val(),
            Quantity: $(this).find('.txtProductQuantity').val(),
            UnitPrice: $(this).find(".txtProductUnitPrice").val(),
            TotalPrice: $(this).find('.txtProductAmount').val(),
            EquipmentName: $(this).find('.ProductName').val(),
            QuantityLeftEquipment: parseInt($(this).find('.spnProductQuantityLeft').text()),
            IsService: $(this).hasClass('IsService'),
            IsEquipmentRelease: $(this).find('.spnIsEquipmentRelease').text(),
            IsInvoiceCreate: $(this).find('.spnIsInvoiceCreate').text(),
            ReferenceInvoiceId: $(this).find('.spnReferenceInvoiceId').text(),
            ReferenceInvDetailId: $(this).find('.spnReferenceInvoiceDetailId').text(),
            IsBilling: $(this).find('.spnIsBilling').text(),
            IsCopied: $(this).find('.spnIsCopied').text(),
            IsBillingProcess: $(this).find('.spnIsBillingProcess').text(),
        });
    });
    param = JSON.stringify({
        "CustomerAppointment.CustomerAppointmentEquipmentList": DetailList,
        "CustomerAppointment.AppointmentId": TicketId,
        "CustomerAppointment.TaxPercent": $("#CustomerAppointment_TaxPercent").val(),
        "CustomerAppointment.TaxType": $("#CustomerAppointment_TaxPercent option:selected").text(),
        "CustomerAppointment.TaxTotal": TaxTotal,
        "CustomerAppointment.TotalAmount": SubTotal,
        "CustomerAppointment.TotalAmountTax": Amount,
        "CustomerAppointment.IsComplete": IsComplete,
        AddedEquipmentList: AddedEquipmentList,
        RemovedEquipmentList: RemovedEquipmentList,
        "Ticket.TicketId": $("#Ticket_TicketId").val(),
        Assigned: $("#AssignedTo").val(),
        EquipmentType: EquipmentType
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {

            if (data.result == true) {
                OpenSuccessMessageNew("Success!", data.message);
                setTimeout(function () {
                    ReloadTicket();
                }, 1000);
            } else {
                $(".ticket-loader-div").hide();
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".ticket-loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var HideAllAttachmentsDiv = function () {
    $(".attach_invoice").addClass("hidden");
    $(".attach_estimate").addClass("hidden");
    $(".addfile_tr").addClass("hidden");
    $(".Add_equipment").addClass("hidden");
    $(".Add_service").addClass("hidden");
}

var AddTime = function () {
    if ($("#AppointmentStartTime").val() != "-1"
        /*&& $("#AppointmentEndTime").val() == "-1"*/) {
        var hour = (parseInt($("#AppointmentStartTime").val().split(":")[0]) + HoursToAdd) % 24;
        if (hour < 10) {
            hour = '0' + hour;
        }
        hour = hour + ":" + $("#AppointmentStartTime").val().split(":")[1];
        $("#AppointmentEndTime").val(hour);
    }
}
var TimeCheckValidation = function () {
    $("#AppointmentStartTime").removeClass("required");
    $("#AppointmentEndTime").removeClass("required");

    if ($("#AppointmentEndTime").val() == "-1"
        && $("#AppointmentStartTime").val() == "-1") {
        $("#AppointmentStartTime").removeClass("required");
        $("#AppointmentEndTime").removeClass("required");
        return true;
    }
    else if ($("#AppointmentEndTime").val() == "-1"
        && $("#AppointmentStartTime").val() != "-1") {
        $("#AppointmentEndTime").addClass("required");
        return false;
    }
    else if ($("#AppointmentEndTime").val() != "-1"
        && $("#AppointmentStartTime").val() == "-1") {
        $("#AppointmentStartTime").addClass("required");
        return false;
    }
    else if (new Date("05/30/2018" + " " + $("#AppointmentEndTime").val()) > new Date("05/30/2018" + " " + $("#AppointmentStartTime").val())) {
        $("#AppointmentStartTime").removeClass("required");
        $("#AppointmentEndTime").removeClass("required");
        return true;
    }

    $("#AppointmentEndTime").addClass("required");
    $("#AppointmentStartTime").addClass("required");
    return false;

}
var StartTimeCheckValidation = function () {
    $("#AppointmentStartTime").removeClass("required");
    var starttime = $("#AppointmentStartTime").val();
    if (starttime != "-1" && starttime != "" && starttime != "null" && starttime != "undefined") {
        $("#AppointmentStartTime").removeClass("required");
        $("#AppointmentEndTime").removeClass("required");
        return true;
    }
    else {
        $("#AppointmentStartTime").addClass("required");
        return false;
    }
}
var EndTimeCheckValidation = function () {
    $("#AppointmentEndTime").removeClass("required");
    var endtime = $("#AppointmentEndTime").val();
    if (endtime != "-1" && endtime != "" && endtime != "null" && endtime != "undefined") {
        $("#AppointmentEndTime").removeClass("required");
        return true;
    }
    else {
        $("#AppointmentEndTime").addClass("required");
        return false;
    }
}

var AssignedToValidation = function () {

    $("#AssignedTo").removeClass("required");
    if ($("#AssignedTo").val() == "00000000-0000-0000-0000-000000000000") {
        $("#AssignedTo").addClass("required");
        return false;
    }
    else if ($("#AssignedTo").val() != "00000000-0000-0000-0000-000000000000") {
        $("#AssignedTo").removeClass("required");
        return true;
    }
    $("#AssignedTo").addClass("required");
    return false;

}




var LoadDropdownValues = function () {
    //var count = localStorage.clickcount;
    if ($("#AssignedTo").children('option').length < 3 && TicketIntId == 0) {
        //localStorage.clickcount = Number(localStorage.clickcount) + 1;
        var url = domainurl + "/Ticket/LoadAddTicketDropdownListValue";
        $.ajax({
            type: "GET",
            ajaxStart: $(".loader-div").show(),
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result == true) {
                    $('#Equipment_SoldBy').html('');
                    $('#Service_SoldBy').html('');
                    for (var i = 0; i < data.EmpList.length; i++) {
                        var opt = new Option(data.EmpList[i].Text, data.EmpList[i].Value);
                        $('#Equipment_SoldBy').append(opt);
                        $('#Service_SoldBy').append(opt);
                    }
                    for (var j = 0; j < data.TechnicianUserList.length; j++) {
                        $('#AssignedTo').append($('<option>', { value: data.TechnicianUserList[j].UserId, text: data.TechnicianUserList[j].FirstName + ' ' + data.TechnicianUserList[j].LastName, email: data.TechnicianUserList[j].Email, phone: data.TechnicianUserList[j].Phone, }));
                        //$('#AssignedTo').append('<option value="' + TechnicianUserList[j].UserId + '" >' + TechnicianUserList[j].FirstName + ' ' + TechnicianUserList[j].LastName +'</option>');
                    }
                    var removeItem = $("#AssignedTo").val();
                    $(".load_additional_members").load("/Ticket/LoadTicketAdditionalMembers?ticketid=0&assigned=" + removeItem);

                    $('#Survey').html('');
                    for (var k = 0; k < data.SurveyList.length; k++) {
                        var opt = new Option(data.SurveyList[k].Text, data.SurveyList[k].Value);
                        $('#Survey').append(opt);
                    }
                    $('#NotifyingMembers').html('');
                    for (var l = 0; l < data.NotifyingList.length; l++) {
                        var opt = new Option(data.NotifyingList[l].Text, data.NotifyingList[l].Value);
                        $('#NotifyingMembers').append(opt);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}




var initWindow = function () {
    if (screen.width < 414) {
        $(".add_ticket_main").height((window.innerHeight - $(".header-title").height() - 55));
    }
    else {
        $(".add_ticket_main").height((window.innerHeight - 107));
    }

    $('.selectpicker').selectpicker();

    if (TicketStatus == "Init") {
        $("#Ticket_Status").val("Created");
    }

    $(".CustomerInvoiceTab tbody").sortable({
        update: function () {
            var i = 1;
            $(".CustomerInvoiceTab tbody tr td:first-child").each(function () {
                $(this).text(i);
                i += 1;
            });
        }
    }).disableSelection();
    //var disallowedDates = ['2020-09-01', '2020-09-25', '2020-09-26', '2020-09-05', '2020-09-13', '2020-09-29', '2020-09-11'];
    //console.log("calendar");
    CompletionDatepicker = new Pikaday({ 
        field: $('.CompletionDate')[0],
        format: 'MM/DD/YYYY',
        onSelect: function (dateText) { 
            EmployeeHolidayCheck();
        }
        
       //minDate: moment().toDate(),
    });
    CompletedDatepicker = new Pikaday({
        field: $('#Ticket_CompletedDate')[0],
        format: 'MM/DD/YYYY',
        onSelect: function (dateText) { 
            EmployeeHolidayCheck();
        }
    });
    var Popupwidth = 920;
    var Popupheight = 600;
    if (Device.All()) {
        Popupwidth = window.innerWidth;
        Popupheight = window.innerHeight;
    }
    var idlist = [{ id: "#TicketPrint", type: 'iframe', width: Popupwidth, height: Popupheight }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    if (ScheduleTicketStatus != '') {
        $("#Ticket_Status").val(ScheduleTicketStatus);
    }
}

var LoadScheduleCalendar = function () {
    //GuidCustomer = $('#CustomerList').val();
    console.log("GuidCustomer");
    var Param = {
        "Id": $("#Ticket_Id").val(),
        "TicketId": $("#Ticket_TicketId").val(),
        "Subject": $("#Ticket_Subject").val(),
        "Message": tinyMCE.get('BodyContent').getContent(),
        "Status": $("#Ticket_Status").val(),
        "CustomerId": GuidCustomer,
        "TicketType": $("#Ticket_TicketType").val(),
        "CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "AppointmentStartTime": $("#AppointmentStartTime").val(),
        "AppointmentEndTime": $("#AppointmentEndTime").val(),
    };
    var url = domainurl + "/Ticket/SaveTicketSession";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                var pathname = window.location.pathname.toLowerCase();
                if (pathname == '/scheduleinfo') {
                    window.location.href = "/ScheduleInfo?date=" + $("#Ticket_CompletionDate").val() + "&viewtype=" + "Daily" + "&TicketId=" + $("#Ticket_Id").val() + "&CustomerId=" + GuidCustomer;
                }
                else {
                    window.location.href = "/calendar?ticketdate=" + $("#Ticket_CompletionDate").val() + "&ticketid=" + $("#Ticket_Id").val() + "&customerid=" + GuidCustomer;
                }
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var findMaxValue = function (element) {
    var maxValue = undefined;
    $('option', element).each(function () {
        var val = $(this).attr('value');
        val = parseInt(val, 10);
        if (maxValue === undefined || maxValue < val) {
            maxValue = val;
        }
    });
    return maxValue;
}
var loadticketsignature = function (id) {
    $(".MapManufacturerMagnific_signature").attr('href', '/Ticket/LoadTicketSignature?id=' + id);
    $(".MapManufacturerMagnific_signature").click();
}
var loadrescheduleticket = function (TicketId) {
    $(".ReScheduleTicketMagnific").attr('href', '/Ticket/RescheduleTicketPopup?TicketId=' + TicketId);
    $(".ReScheduleTicketMagnific").click();
}

var loadMemberAppointment = function (TicketId) {


    $(".MemberAppointmentMagnific").attr('href', "/Ticket/MemberAppointmentPupup?TicketId=" + TicketId + "&UserList=" + $("#AdditionalMembers").val() + "&AppointmentDate=" + $("#Ticket_CompletionDate").val() + "&CustomerId=" + GuidCustomer);
    $(".MemberAppointmentMagnific").click();
}
var loadTicketNotifySetting = function () {
    $(".TicketNotificationMagnific").attr('href', '/Ticket/TicketNotifySetting');
    $(".TicketNotificationMagnific").click();
}
var loadCreateAddendum = function (id) {
    if (WorkToBePerformedPermit.toLocaleLowerCase() == 'true') {
        $(".LoadWorkToBePerformedAddendumPopUp").attr('href', '/Ticket/GetWorkToBePerformedAddendumPopUp?CustomerId=' + GuidCustomer + "&TicketId=" + TicketId);
        $(".LoadWorkToBePerformedAddendumPopUp").click();
    }
    else {
        $(".LoadCustomerAddendumPopUp").attr('href', '/Ticket/GetCustomerAddendumPopUp?CustomerId=' + GuidCustomer + "&TicketId=" + TicketId);
        $(".LoadCustomerAddendumPopUp").click();
    }

}
var CreateCustomerAddendum = function () {
    console.log("ff");
    $("#InstallCustomerAddendum").click();
}

var OpenLogTicket = function () {

    $(".LoadTicketLogItem").html(TabsLoaderText);
    $(".LoadTicketLogItem").load("/Ticket/LoadLogTicketReplyPartial?ticketid=" + TicketId);
}
var OpenNoteTicket = function () {

    $(".LoadTicketNoteItem").html(TabsLoaderText);
    $(".LoadTicketNoteItem").load("/Ticket/LoadNoteTicketReplyPartial?ticketid=" + TicketId);
}
var SearchReplyTicketLog = function () {
    var searchText = $(".search_item_val").val();
    $(".LoadTicketLogItem").html(TabsLoaderText);
    $(".LoadTicketLogItem").load("/Ticket/LoadLogTicketReplyPartial?ticketid=" + TicketId + "&search=" + encodeURI(searchText));
}
var SearchReplyTicketNote = function () {
    var searchText = $(".search_item_val_note").val();
    $(".LoadTicketNoteItem").html(TabsLoaderText);
    $(".LoadTicketNoteItem").load("/Ticket/LoadNoteTicketReplyPartial?ticketid=" + TicketId + "&search=" + encodeURI(searchText));
}
var InvoiceEqSuggestionclickbind = function (item, result) {
    if (result) {
        console.log("Programmatically passed result:", result);
        
        $('.CustomerInvoiceTab .tt-menu').hide();
        $(item).val(result.EquipmentName);
        $(item).attr('data-id', result.EquipmentId);

        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());
        
        var AddedString = "";
        if (CurrentUserFullName != "") {
            AddedString += "Added By: " + CurrentUserFullName;
        }
        if (result.SKU != "") {
            AddedString += ", SKU: " + result.SKU;
        }
        if (AddedString != "") {
            $(itemName).attr('title', AddedString);
        }

        AddedEquipmentList = AddedEquipmentList.concat($(item).val());
        $(item).parent().parent().attr('data-id', result.EquipmentId);
        $(item).parent().parent().addClass('HasItem');

        $(item).parent().parent().find('.spnProductDesc').text(result.Description);
        $(item).parent().parent().find('.txtProductDesc').val(result.Description);
        
        var spnItemRate = $(item).parent().parent().find('.spnProductPoint');

    
        var formattedPoint = parseFloat(result.Point || 0).toFixed(2);

      
        $(spnItemRate)
            .text(formattedPoint)
            .attr('data-point', formattedPoint);


        $(item).parent().parent().find('.spnProductQuantityOnHand')
            .attr('data-new-added', '1')
            .attr('data-is-equipexist', '0')
            .text(Math.max(0, parseInt(result.QuantityOnHand || 0)));

        $(item).parent().parent().find('.spnProductQuantity').text(1);
        $(item).parent().parent().find('.txtProductQuantity').val(1);
        
        $(item).parent().parent().find('.spnSku').text(result.SKU);

      
        $(item).parent().parent().find('.spnProductQuantityLeft').text(0);
        $(item).parent().parent().find('.txtProductQuantityLeft').val(0);
        
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        if (spnItemRate.length > 0) {
            var spnProductAmountValue = result.RetailPrice || 0;
            $(spnItemRate).text(
                TransCurrency +
                parseFloat(spnProductAmountValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
            );
            $(item).parent().parent().find('.txtProductAmount').val(spnProductAmountValue);
        } else {
            console.error("spnProductAmount element not found.");
        }
        var spnItemUnitPrice = $(item).parent().parent().find('.spnProductRate');
        if (spnItemUnitPrice.length > 0) {
            var unitPriceValue = result.RetailPrice || 0;
            $(spnItemUnitPrice).text(
                TransCurrency +
                parseFloat(unitPriceValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
            );
            $(item).parent().parent().find('.txtProductUnitPrice').val(unitPriceValue);
        } else {
            console.error("spnProductRate element not found.");
        }

       
        var spnItemVendorCost = $(item).parent().parent().find('.spnProductEquipmentvendorcost');
        if (spnItemVendorCost.length > 0) {
            $(spnItemVendorCost).text(
                TransCurrency +
                parseFloat(result.Equipmentvendorcost || 0).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
            );
            $(spnItemVendorCost).val(result.Equipmentvendorcost);
        } else {
            console.error("spnProductEquipmentvendorcost element not found.");
        }

 
        CalculateNewAmount();
    }


    
    $('.CustomerInvoiceTab .tt-suggestion').click(function () {
        console.log("clickbind1");

        var clickitem = this;
        $('.CustomerInvoiceTab .tt-menu').hide();

        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());
        var AddedString = "";
        if (CurrentUserFullName != "") {
            AddedString += "Added By: " + CurrentUserFullName;
        }
        if ($(clickitem).attr('data-sku') != "") {
            AddedString += ", SKU: " + $(clickitem).attr('data-sku');
        }
        if (AddedString != "") {
            $(itemName).attr('title', AddedString);
        }

        AddedEquipmentList = AddedEquipmentList.concat($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');

        var spnItemRate = $(item).parent().parent().find('.spnProductDesc');
        $(spnItemRate).text($(this).attr('data-description'));
        var txtItemRate = $(item).parent().parent().find('.txtProductDesc');
        $(txtItemRate).val($(this).attr('data-description'));

        var spnItemRate = $(item).parent().parent().find('.spnProductPoint');
        $(spnItemRate).text($(this).attr('data-point'));
        $(spnItemRate).attr("data-point", $(this).attr('data-point'));

        var spnItemRate = $(item).parent().parent().find('.spnProductQuantityOnHand');
        $(spnItemRate).attr('data-new-added', '1');
        $(spnItemRate).attr('data-is-equipexist', '0');
        if (parseInt($(this).attr('data-quantityonhand')) < 0) {
            $(spnItemRate).text(0);
        } else {
            $(spnItemRate).text($(this).attr('data-quantityonhand'));
        }

        var spnItemRate = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemRate).text(1);
        var txtItemRate = $(item).parent().parent().find('.txtProductQuantity');
        $(txtItemRate).val(1);

        var spnItemSKU = $(item).parent().parent().find('.spnSku');
        $(spnItemSKU).text($(this).attr('data-sku'));

        var spnQtyLeft = $(item).parent().parent().find('.spnProductQuantityLeft');
        $(spnQtyLeft).text(0);
        var txtQtyLeft = $(item).parent().parent().find('.txtProductQuantityLeft');
        $(txtQtyLeft).val(0);

        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        var spnProductAmountValue = $(this).attr('data-retail');

        var spnItemVendorCost = $(item).parent().parent().find('.spnProductEquipmentvendorcost');
        var spnProductVndorcostValue = $(this).attr('data-equipvendorcost');
        $(spnItemVendorCost).text(TransCurrency + parseFloat(spnProductVndorcostValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        $(spnItemVendorCost).val(spnProductVndorcostValue);

        $(spnItemRate).text(TransCurrency + parseFloat(spnProductAmountValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val(spnProductAmountValue);

        var spnItemUnitPrice = $(item).parent().parent().find('.spnProductRate');
        if (spnItemUnitPrice.length > 0) {
            var unitPriceValue = $(this).attr('data-retail') || 0; 
            $(spnItemUnitPrice).text(
                TransCurrency +
                parseFloat(unitPriceValue).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
            );
            $(item).parent().parent().find('.txtProductUnitPrice').val(unitPriceValue);
        } else {
            console.error("spnProductRate element not found.");
        }
        CalculateNewAmount();
    });
    $('.CustomerInvoiceTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var ExistEquipment = "";
var ExistEquipmentInner = "";

//var isTimerSet = false;
//var lclTimer;

var lastSearchKey = "";
var typingTimeout; 

var SearchKeyUp = function (item, event, EquipmentType) {
    var RequestUrl = "/Invoice/GetEquipmentListByKeyTechnicianId";
    if (EquipmentType == EquipmentTypes.Service) {
        RequestUrl = "/Invoice/GetOnlyServiceListByKey";
    }

    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13) {
        return false;
    }

    var currentSearchKey = $(item).val();
    lastSearchKey = currentSearchKey;
    console.log("Current search key updated:", currentSearchKey);

    clearTimeout(typingTimeout);

    
    typingTimeout = setTimeout(function () {
        $("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").each(function () {
            console.log($(this).attr('data-release'));
            if ($(this).attr('data-release') == 'True') {
                ExistEquipmentInner += "'" + $(this).attr('data-id') + "',";
            }
        });

        if (ExistEquipmentInner.length > 0) {
            ExistEquipmentInner = ExistEquipmentInner.slice(0, -1);
            ExistEquipment = "(" + ExistEquipmentInner + ")";
        }

        var Param = {
            "key": currentSearchKey,
            "technicianId": $("#AssignedTo").val(),
            ExistEquipment: ExistEquipment
        };

        console.log("Search key with Barcode:", Param.key);

        $.ajax({
            type: "POST",
            url: domainurl + RequestUrl,
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var resultparse = JSON.parse(data.result);

                if (resultparse.length > 0) {
                    var searchresultstring = "<div class='NewProjectSuggestion'>";
                    for (var i = 0; i < resultparse.length; i++) {
                        searchresultstring += String.format(PropertyUserSuggestiontemplate,
                            resultparse[i].EquipmentId,
                            resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                            resultparse[i].RetailPrice,
                            resultparse[i].Reorderpoint,
                            resultparse[i].QuantityAvailable,
                            resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                            resultparse[i].EquipmentDescription.replaceAll('"', '\'\''),
                            resultparse[i].QuantityOnHand,
                            resultparse[i].SupplierCost,
                            resultparse[i].RetailPrice,
                            resultparse[i].ManufacturerName.replaceAll('"', '\'\''),
                            resultparse[i].SKU,
                            resultparse[i].Point,
                            resultparse[i].Equipmentvendorcost,
                            resultparse[i].BarCode
                        );
                    }
                    searchresultstring += "</div>";

                    var ttdom = $($(item).parent()).find('.tt-menu');
                    var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                    $(ttdomComplete).html(searchresultstring);
                    $(ttdom).show();

                    var matchedResult = resultparse.find(r => r.Barcode === lastSearchKey);
                    console.log("Last search key for matching:", lastSearchKey);

                    if (matchedResult && resultparse.length === 1) {
                        console.log("Exact match found for key with Barcode:", matchedResult.Barcode);
                        InvoiceEqSuggestionclickbind(item, matchedResult);
                    } else {
                        console.log("No exact match found for key with Barcode.");
                        InvoiceEqSuggestionclickbind(item);
                    }

                    if (resultparse.length > 4) {
                        $(".NewProjectSuggestion").height(352);
                        $(".NewProjectSuggestion").css('position', 'relative');
                        $(".NewProjectSuggestion").perfectScrollbar();
                    }
                } else {
                    $('.tt-menu').hide();
                }
            }
        });
    }, 300);
};





var SearchKeyDown = function (item, event) {
    console.log('down');
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

var SaveSurveyForTicket = function (ticketId) {
    if ($("#Survey").val() != "-1") {
        $("#Survey").attr("style", "border-color: #ccc");
        $(".TicketSurveyMagnific").attr("href", "/Ticket/GetPopUpTicketSurvey?cusid=" + GuidCustomer + "&ticketid=" + ticketId);
        $(".TicketSurveyMagnific").click();
    }
    else {
        $("#Survey").attr("style", "border-color: red");
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
        } else if ($(event.target).hasClass('txtProductUnitPrice')) {
            $($(trselected).next('tr')).find('input.txtProductUnitPrice').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).next('tr')).find('input.txtProductAmount').focus();
        }
        //else if ($(event.target).hasClass('txtProductEquipmentvendorcost')) {
        //    $($(trselected).next('tr')).find('input.txtProductEquipmentvendorcost').focus();
        //}
        else if ($(event.target).hasClass('txtProductQuantityLeft')) {
            $($(trselected).next('tr')).find('input.txtProductQuantityLeft').focus();
        }
    } else if (event.keyCode == 38) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).prev('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtProductDesc')) {
            $($(trselected).prev('tr')).find('input.txtProductDesc').focus();
        } else if ($(event.target).hasClass('txtProductQuantity')) {
            $($(trselected).prev('tr')).find('input.txtProductQuantity').focus();
        } else if ($(event.target).hasClass('txtProductUnitPrice')) {
            $($(trselected).prev('tr')).find('input.txtProductUnitPrice').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).prev('tr')).find('input.txtProductAmount').focus();
        }
        //else if ($(event.target).hasClass('txtProductEquipmentvendorcost')) {
        //    $($(trselected).prev('tr')).find('input.txtProductEquipmentvendorcost').focus();
        //}
        else if ($(event.target).hasClass('txtProductQuantityLeft')) {
            $($(trselected).next('tr')).find('input.txtProductQuantityLeft').focus();
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
    console.log("amount");
    var amount = parseFloat('0');
    var subTotal = parseFloat('0');
    var taxTotal = parseFloat('0');

    $(".txtProductAmount").each(function (e, f) {
        //console.log(e);
        //var currAmountStr = parseFloat($(this).val().trim());
        //currAmountStr = currAmountStr.replaceAll(',', '');

        var _CalAmt = $(this).val().trim();
        _CalAmt = _CalAmt.replaceAll(',', '');

        var currAmount = parseFloat(_CalAmt);
        if (!isNaN(currAmount)) {
            amount += currAmount;
            SubTotal = amount;
        }
    });
    amount = parseFloat(amount).toFixed(2);
    amount1 = amount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    $(".amount").text(TransCurrency + amount1);
    $(".total").text(TransCurrency + amount1);
    var tp = $("#CustomerAppointment_TaxPercent").val();
    if (tp != "" && tp != "0" && tp != null) {
        $(".tax-percent").text(tp + "%");
        var ta = (amount) * (tp / 100);
        TaxTotal = ta.toFixed(2);
        $(".tax-amount").text(TransCurrency + ta.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        amount = parseFloat(amount) + parseFloat(ta);
        amount2 = amount.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
        $(".total").text(TransCurrency + amount2);
    }
    if (tp == "0") {
        $(".tax-percent").text(0 + "%");
        $(".tax-amount").text(TransCurrency + "0.00");
        TaxTotal = 0.0;
    }
    TotalAmount = amount;
    Amount = amount;
}
var InitRowIndex = function () {
    var i = 1;
    //$("#CustomerInvoiceTab tbody tr td:first-child").each(function () {
    //    $(this).prepend('<span>' + i + ' <i class="fa fa-arrow-circle-o-down dropbtn_equipment" aria-hidden="true" onclick="OpenBadInventoryDropDown(' + (i - 1) + ')"></i></span>');
    //    i += 1;
    //});
    i = 1;
    //$("#CustomerServiceTable tbody tr td:first-child").each(function () {
    //    $(this).prepend('<span>' + i + ' <i class="fa fa-arrow-circle-o-down dropbtn_service" aria-hidden="true" onclick="OpenBadInventoryServiceDropDown(' + (i - 1) + ')"></i></span>');
    //    i += 1;
    //});
}
var InitRowIndexBadEquipment = function () {

    var i = 1;
    $("#CustomerInvoiceTab tbody tr td:first-child").each(function () {
        i += 1;
    });
    //if ($("#CustomerInvoiceTab tbody tr:last td:first-child").find('span').length == 0) {
    //    $("#CustomerInvoiceTab tbody tr:last td:first-child").prepend('<span>' + (i - 1) + ' <i class="fa fa-arrow-circle-o-down dropbtn_equipment" aria-hidden="true" onclick="OpenBadInventoryDropDown(' + (i - 2) + ')"></i></span><div class="dropdown_content_equipment_' + (i - 2) + ' hidden bad_inventory_equipment"><i class="fa fa-user-circle" aria-hidden="true"></i><div class="UserSuggestion tt-menu"><div class="tt-menu-header">Sold By</div> <div class="tt-dataset tt-dataset-autocomplete"> </div> </div></div>');
    //}
}
var InitRowIndexBadService = function () {

    var i = 1;
    $("#CustomerServiceTable tbody tr td:first-child").each(function () {
        i += 1;
    });
    //if ($("#CustomerServiceTable tbody tr:last td:first-child").find('span').length == 0) {
    //    $("#CustomerServiceTable tbody tr:last td:first-child").prepend('<span>' + (i - 1) + ' <i class="fa fa-arrow-circle-o-down dropbtn_service" aria-hidden="true" onclick="OpenBadInventoryServiceDropDown(' + (i - 2) + ')"></i></span><div class="dropdown_content_service_' + (i - 2) + ' hidden bad_inventory_equipment"><i class="fa fa-user-circle" aria-hidden="true"></i><div class="UserSuggestion tt-menu"><div class="tt-menu-header">Sold By</div> <div class="tt-dataset tt-dataset-autocomplete"> </div> </div></div>');
    //}
}
var DeleteFile = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Ticket/DeleteTicketReply",
        data: {
            id: delitem
        },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                ReloadTicket();
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },

        error: function () {
        }
    });
}
var CreateDOTicket = function () {
    if ($("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "No items choosen.");
        return 0;
    }
    var url = domainurl + "/PurchaseOrder/AddDemandOrderTicket";
    var DetailList = [];
    $("#CustomerInvoiceTab .HasItem,#CustomerServiceTable .HasItem").each(function (e, item) {
        var Qty = parseInt(0);
        var QtyOnHand = parseInt(0);
        if ($(item).find('.txtProductQuantity').val() != undefined || $(item).find('.txtProductQuantity').val() != null) {
            Qty = parseInt($(item).find('.txtProductQuantity').val());
        }
        if ($(item).find('.spnProductQuantityOnHand').text() != undefined || $(item).find('.spnProductQuantityOnHand').text() != null) {
            QtyOnHand = parseInt($(item).find('.spnProductQuantityOnHand').text());
        }
        if (Qty > QtyOnHand) {
            DetailList.push({
                EquipmentId: $(item).attr('data-id'),
                EquipName: $(item).find('.ProductName').val(),
                EquipDetail: $(item).find('.txtProductDesc').val(),
                Quantity: Qty - QtyOnHand,
                RecieveQty: $(item).find('.txtProductQuantityReceived').val(),
                CreatedDate: '1-1-2017',
            });
        }
    });
    var param = JSON.stringify({
        TechnicianId: $("#AssignedTo").val(),
        PurchaseOrderDetail: DetailList,
        TicketId: TicketIntId
    });
    if (DetailList != null) {
        $.ajax({
            type: "POST",
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $(".AddInvoiceLoader").addClass('hidden');
                if (data.result) {
                    OpenSuccessMessageNew("Success!", "DO successfully created.", function () {
                    });
                } else {
                    OpenErrorMessageNew("Error!", data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    }
}
var DownloadTicketUploadFile = function (id) {
    alert();
}
var OpenBadInventoryDropDown = function (val) {
    if ($(".dropdown_content_equipment_" + val).hasClass('hidden')) {
        $(".dropdown_content_equipment_" + val).removeClass('hidden');
    }
    else {
        $(".dropdown_content_equipment_" + val).addClass('hidden');
    }
    for (var l = 0; l < 100; l++) {
        if (l != val) {
            $(".dropdown_content_equipment_" + l).addClass('hidden');
        }
    }
}
var OpenBadInventoryServiceDropDown = function (val) {
    if ($(".dropdown_content_service_" + val).hasClass('hidden')) {
        $(".dropdown_content_service_" + val).removeClass('hidden');
    }
    else {
        $(".dropdown_content_service_" + val).addClass('hidden');
    }
    for (var l = 0; l < 100; l++) {
        if (l != val) {
            $(".dropdown_content_service_" + l).addClass('hidden');
        }
    }
}
var TicketDatePicker = function () {
    var url = "/Ticket/SaveCompletedDateTicket";
    var param = JSON.stringify({
        ticketid: TicketIntId
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                $("#Ticket_CompletedDate").val(data.compDate);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}
var istikpaid = false;
var SaveCompletedDateTicket = function () {
    if (typeof (TicketType) != "undefined" && TicketType != null && TicketType != "" && (TicketType == "Drop Off" || TicketType == "Pick Up") && isticketpay == "False" && $("#Ticket_Status").val() == "Completed") {
        OpenSuccessMessageNew("Payment", "<div class = 'container-fluid'><div class = 'form-group clearfix'><div style='float:left;font-weight:600;'>Paid</div><div style='float:left;width:100%;'><select class = 'form-control' id='is_paid' onchange=changeticketPaymentpaid()><option value='0'>No</option><option value='1'>Yes</option></select></div></div><div class = 'form-group clearfix payment_method hidden'><div style='float:left;font-weight:600;'>Payment Method</div><div style='float:left;width:100%;'><select class='form-control' id='ticket_pay_method'><option value='ach'>ACH</option><option value='Credit Card'>Credit Card</option><option value='cash'>Cash</option></select></div></div><div class ='form-group clearfix confirm_no hidden'><div style='float:left;font-weight:600;'>Confirmation No</div><div style='float:left;width:100%;'><input type='number' class='form-control' id=pay_confirm_no /></div></div></div>", function () {
            if (istikpaid) {
                var url = "/Ticket/SaveTicketPayment";
                var param = JSON.stringify({
                    TicketId: TicketId,
                    CustomerId: GuidCustomer,
                    PaymentMethod: $("#ticket_pay_method").val(),
                    ConfirmationNo: $("#pay_confirm_no").val(),
                    IsPaid: istikpaid
                });
                $.ajax({
                    type: "POST",
                    url: url,
                    data: param,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (data) {
                        if (data.result) {
                            OpenSuccessMessageNew("Success", "Ticket payment saved successfully");
                            $("#ModalSuccessMessage .success_close").text("Close");
                        }
                    }
                });
            }
        });
    }
    else {
        //TicketDatePicker();
    }
    //TicketDatePicker();
}
var OpenBadInventoryAndUserAssignPopup = function (appid, caeIntId, cusid, countid, onhandqty) {
    var loadUrl = domainurl + "/Ticket/LoadBadInventoryAndUserAssignPopup?appid=" + appid + "&caeIntId=" + caeIntId + "&cusid=" + cusid + "&count=" + countid + "&qty=" + onhandqty;
    $(".BadInventoryAndUserAssignMagnific").attr('href', loadUrl);
    $(".BadInventoryAndUserAssignMagnific").click();
}
var UndoThisEquipmentConfirmation = function (AppointmentId, caeIntId) {
    var url = domainurl + "/Ticket/UndoThisEquipment/";
    var param = JSON.stringify({
        Id: caeIntId,
        AppointmentId: AppointmentId
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
            if (data.result == true) {
                OpenSuccessMessageNew("Success!", "", ReloadTicket());
            }
            else {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var UndoThisEquipment = function (AppointmentId, caeIntId) {
    OpenConfirmationMessageNew("", "Are you sure you want to undo this equipment?", function () {
        UndoThisEquipmentConfirmation(AppointmentId, caeIntId);
    });
}
var changeticketPaymentpaid = function () {
    if ($("#is_paid").val() == "0") {
        $(".payment_method").addClass('hidden');
        $(".confirm_no").addClass('hidden');
        $("#ModalSuccessMessage .success_close").text("Close");
        istikpaid = false;
    }
    else {
        $(".payment_method").removeClass('hidden');
        $(".confirm_no").removeClass('hidden');
        $("#ModalSuccessMessage .success_close").text("Submit");
        istikpaid = true;
    }
}
var CreateCodeDocument = function (id) {
    var loadUrl = domainurl + "/Ticket/GetCodeSafetyPopup?id=" + id;
    $(".CodeSafetyDocumentMagnific").attr('href', loadUrl);
    $(".CodeSafetyDocumentMagnific").click();
}
var loadFirstPage = function (id, ticketid) {
    var loadUrl = domainurl + "/SmartLeads/GetSmartLeadsForPopUp?LeadId=" + TicketCustomerIntId + "&grant=false" + "&templateid=0" + "&firstpage=true&ticketid=" + ticketid;
    $(".FirstPageAgreementDocument").attr('href', loadUrl);
    $(".FirstPageAgreementDocument").click();
}
var loadCommercialAgreement = function (id, ticketid) {
    var loadUrl = domainurl + "/SmartLeads/GetSmartLeadsForPopUp?LeadId=" + TicketCustomerIntId + "&grant=false" + "&templateid=0" + "&ticketid=" + ticketid + "&commercial=true";
    $(".CommercialAgreementDocument").attr('href', loadUrl);
    $(".CommercialAgreementDocument").click();
}

var updatecontractsigned = function (Id) {
    var url = domainurl + "/Ticket/UpdateContractSigned/";
    var param = JSON.stringify({
        Id: Id,
        TicketId: TicketIntId
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

                OpenTicketById(data.ticketId);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var DeleteCommissionConfirmation = function (type, commissionId) {
    var url = domainurl + "/Ticket/DeleteCommission";
    var param = JSON.stringify({
        Type: type,
        CommissionId: commissionId
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
                    ReloadTicket();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var DeleteCommission = function (type, commissionId) {
    OpenConfirmationMessageNew("", "Are you sure you want to delete?", function () {
        DeleteCommissionConfirmation(type, commissionId);
    });
}
var CheckValue = [];

var CheckValue = {
    SendToCustomer: $("#SendToCustomer").prop('checked') == true ? true : false,
    SendToTech: $("#SendToTech").prop('checked') == true ? true : false,
    SendToAdditionalMembers: $("#SendToAdditionalMembers").prop('checked') == true ? true : false,
    SendToNotifyingMembers: $("#SendToNotifyingMembers").prop('checked') == true ? true : false,
}
var ResetCheckvalue = function () {
    CheckValue.SendToCustomer = false;
    CheckValue.SendToTech = false;
    CheckValue.SendToAdditionalMembers = false;
    CheckValue.SendToNotifyingMembers = false;
}
$(document).ready(function () {

    var href = window.location.href;
    window.history.pushState({ href: href }, '', href);
    

    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var PopupwidthCustom = 320;
    //if (window.innerWidth < 920) {
    //    PopupwidthCustom = window.innerWidth;
    //}
    var ConditionWidth = 600;
    if (window.innerWidth < 600) {
        ConditionWidth = window.innerWidth;
    }
    var ConditionHeight = 600;
    if (window.innerHeight < 600) {
        ConditionHeight = window.innerHeight;
    }

    var ReScheduleTicketDom = 500;
    if (window.innerWidth < 500) {
        ReScheduleTicketDom = window.innerWidth;
    }

    var idlist = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".MapManufacturerMagnificPrevious", type: 'iframe', width: 920, height: 520 },
    { id: ".LoadAgreementPopUp", type: 'iframe', width: Popupwidth, height: 650 },
    { id: ".LoadCustomerAddendumPopUp ", type: 'iframe', width: Popupwidth, height: 650 },
    { id: ".LoadWorkToBePerformedAddendumPopUp ", type: 'iframe', width: Popupwidth, height: ConditionHeight },
    { id: ".MapManufacturerMagnific_signature", type: 'iframe', width: 600, height: 500 },
    { id: ".ReScheduleTicketMagnific", type: 'iframe', width: ReScheduleTicketDom, height: 420 },
    { id: ".MemberAppointmentMagnific", type: 'iframe', width: 550, height: 420 },
    { id: ".TicketNotificationMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".BadInventoryAndUserAssignMagnific", type: 'iframe', width: ConditionWidth, height: 300 },
    { id: ".CodeSafetyDocumentMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".EquipmentConditionMagnific", type: 'iframe', width: ConditionWidth, height: ConditionHeight },
    { id: ".RUgTicketAgreementMagnific", type: 'iframe', width: 600, height: 500 },
    { id: ".TicketSurveyMagnific", type: 'iframe', width: PopupwidthCustom, height: 210 },
    { id: ".FirstPageAgreementDocument", type: 'iframe', width: Popupwidth, height: 650 }, 
    { id: ".CommercialAgreementDocument", type: 'iframe', width: Popupwidth, height: 650 },
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    TimerDocReady();
    InitRowIndex();

    if ($("#Ticket_Status").val() == "Completed" || $("#Ticket_Status").val() == "Closed") {
        $(".jobstart").hide();
        $(".jobend").hide();
        $('.timer').countimer('stop');
        ClockInOutType = 'end';
        $("#DivCompletedDate").removeClass("hidden");
    }

    $(".TicBalanceDue").click(function () {
        OpenTopToBottomModal("/Transaction/ReceivePayment/?CustomerId=" + TicketCustomerIntId);
    });
    $("#DispatchBtn").click(function () {
        //$("#Ticket_Status").val("InProgress");

        OpenConfirmationMessageNew("", "Are you sure yo want to dispatch?", function () {
            $("#Ticket_Status").val("InProgress");
            AddTicket(true);
        });
    });
    $("#AgreementBtn").click(function () {
        var tickettype = $(this).attr('data-val');
        var id = $(this).attr('data-id');
        $(".RUgTicketAgreementMagnific").attr("href", "/Ticket/LoadTicketAgreement/?ticketType=" + tickettype + "&id=" + id);
        $(".RUgTicketAgreementMagnific").click();
    });
    $("#RescheduleTicketPopup").click(function () {

        var LoadUrl = domainurl + "/Ticket/RescheduleTicketPupup?TicketId=" + TicketId;
        $(".ReScheduleTicketMagnific").attr("href", LoadUrl);
        $(".ReScheduleTicketMagnific").click();
    });
    $("#SendNotification").click(function () {
        console.log("Call");
        AddTicket(false, false, true);
    });
    $("#MemberAppointmentPopup").click(function () {

        var LoadUrl = domainurl + "/Ticket/MemberAppointmentPupup?TicketId=" + TicketId + "&UserList=" + $("#AdditionalMembers").val() + "&AppointmentDate=" + $("#Ticket_CompletionDate").val() + "&CustomerId=" + GuidCustomer;
        console.log(LoadUrl);
        $(".MemberAppointmentMagnific").attr("href", LoadUrl);
        $(".MemberAppointmentMagnific").click();
    });

    $("#StatusToogle").change(function () {
        console.log("StatusToogle")
        if ($("#StatusToogle").prop("checked")) {
            if (isClockIn == "True") {
                $(".jobend").hide();
                $(".jobstart").show();
            }
            else {
                $(".jobstart").hide();
                $(".jobend").show();
            }
            ClockInOut('start')
        }
        else {
            console.log("close");
            $(".jobstart").hide();
            $(".jobend").hide();
            $('.timer').countimer('stop');
            ClockInOutType = 'end';
            $("#Ticket_Status").val("Completed");
        }

    })

    $('.txtProductQuantityLeft').change(function () {
        console.log($(this).val());
        if ($(this).val() < 0) $(this).val(0);
    });

    $('.txtProductQuantity').change(function () {
        console.log($(this).val());
        if ($(this).val() < 0) $(this).val(0);
    });

    $('.txtProductQuantityLeft').keyup(function () {
        var myVal = parseInt($(this).val());
        if (isNaN(myVal)) {
            $(this).val(0);
        }
        else {
            if ($(this).val() < 0) $(this).val(0);
        }
        console.log('Parsed ', myVal);
    });

    $("#Ticket_Status").change(function () {
        if ($("#Ticket_Status").val() == "Completed" || $("#Ticket_Status").val() == "Closed") {
            $(".jobstart").hide();
            $(".jobend").hide();
            $("#DivCompletedDate").removeClass("hidden");
        }
        else {
            if (isClockIn == "True") {
                $(".jobend").hide();
                $(".jobstart").show();
            }
            else {
                $(".jobstart").hide();
                $(".jobend").show();
            }
            $("#DivCompletedDate").addClass("hidden");
        }
        IsStatusChange = true;
    })
    $("#StatusToogle").bootstrapToggle({
        on: 'Open',
        off: 'Closed'
    });
    $("#StatusToogle").unbind("onchange");
    //$("#StatusToogle").on("change", function () {
    //    if(!$("#StatusToogle").prop("checked"))
    //    {
    //        //var MaxStatusValue=findMaxValue($("#Ticket_Status"));
    //        $("#Ticket_Status").val("Closed");
    //    }
    //});
    if (TicketFieldsPermission == "True") {
        $(".additionalmembers").hide();
        $(".notifyingmembers").hide();
        $("#AssignedTo").prop("disabled", true);
        $("#AppointmentEndTime").prop("disabled", true);
        $("#AppointmentStartTime").prop("disabled", true);
    }

    if (window.innerWidth < 421) {
        $(".add_ticket_product_table").width($(".header-title").width() - 20);
    }
    //if (IsClosedPermission == 'True') {
    //    $("#CreateTicket").show();
    //}
    //else {
    //    if (IsClosed == "True") {
    //        $("#CreateTicket").hide();
    //    }
    //    else {
    //        $("#CreateTicket").show();
    //    }
    //}

    //$("#Ticket_CompletionDate,#AppointmentStartTime,#AppointmentEndTime,#AssignedTo").change(function(){
    //    CheckScheduleConflict();
    //});
    $('#CustomerList').change(function () {
        GuidCustomer = $('#CustomerList').val();

        $('.custom-title-span-dynamic').html("");
        $('.custom-title-span-dynamic').html($("#CustomerList :selected").text().replace('Customer', ''));

    });

    InitializeSuburbDropdown($('.dropdown_customar'));
    initWindow();
    $("#AppointmentStartTime").change(function () {
        AddTime();
    });
    $("#update-sales-commission").click(function () {
        $("#edit-sale-commission").removeClass("hidden");
        $("#salesCommissionlabel").addClass("hidden");
        $("#update-sales-commission").addClass("hidden");
    });

    $("#exit-sales").click(function () {
        $("#edit-sale-commission").addClass("hidden");
        $("#salesCommissionlabel").removeClass("hidden");
        $("#update-sales-commission").removeClass("hidden");
    });
    $("#update-tech-commission").click(function () {
        $("#edit-tec-commission").removeClass("hidden");
        $("#techCommissionlabel").addClass("hidden");
        $("#update-tech-commission").addClass("hidden");
    });

    $("#exit-tech").click(function () {
        $("#edit-tec-commission").addClass("hidden");
        $("#techCommissionlabel").removeClass("hidden");
        $("#update-tech-commission").removeClass("hidden");
    })
    function validateProductQuantityForSpecificSection($section) {
        var productQuantityLeft = parseInt($section.find(".spnProductQuantityLeft").text());
        var productQuantityOnHand = parseInt($section.find(".spnProductQuantityOnHand").text());
        
        if (productQuantityOnHand === 0 && productQuantityLeft >= 1) {
            OpenErrorMessageNew("Error!", "Not enough equipment quantity on hand");
            return false;
        }
        return true;
    }


    
    $("#CreateTicket").click(function () {
        localStorage.clickcount = 0;
        var idval = $(this).attr('data-id');
        var TicketOpen = $("#StatusToogle").prop("checked");
        var $section = $(".validate-row");
        if (!validateProductQuantityForSpecificSection($section)) {
              return false;
        }
        if ($("#Ticket_Status").val() == "Completed") {
            if (!EquipmentQuantityValidation()) {
                OpenErrorMessageNew("Error!", "Not enough equipment quantity on hand");
            }
            else if (!QuantityValidation()) {
                OpenErrorMessageNew("Error!", "Not enough equipment quantity on hand");
            }
            else {
                if (signature == "True" && ticketsignature == "" && CommonUiValidation()) {
                    OpenConfirmationMessageNew("Confirmation", "Do you want to add Signature?", function () {
                        loadticketsignature(idval);
                    }, function () {
                        setTimeout(function () {
                            AddTicketConfirmation(false, true);
                        }, 1000);
                    });
                }
                else {
                    if (CommonUiValidation()) {
                        AddTicketConfirmation(false, true);
                    }
                }
                    ClockInOut('stop');
                }
        }
            else {
                if (($("#street").val() != "" || $("#city").val() != "" && $("#state").val() != "") && $("#zipcode").val() != "") {
                    console.log("for validation");
                    var check


                    if (CommonUiValidation() && TimeCheckValidation() && AssignedToValidation() && $("#AssignedTo").val() != null && EquipmentQuantityValidation() && onHandValidation() && QuantityValidation()) {
                        AddTicketConfirmation(false, true);
                    }
                    else if (!AssignedToValidation()) {
                        OpenErrorMessageNew("Error!", "You have not selected any assigned user")
                    }
                    else if (!EquipmentQuantityValidation()) {
                        OpenErrorMessageNew("Error!", "Not enough equipment quantity on hand");
                    }
                    else if (!onHandValidation()) {
                        OpenErrorMessageNew("Error!", "Add items On Hand before installation");
                    }
                    else if (!QuantityValidation()) {
                        OpenErrorMessageNew("Error!", "Not enough equipment quantity on hand");
                    }
                }
                else {
                    if (typeof ($("#CustomerList").val()) != "undefined" && $("#CustomerList").val() != null && $("#CustomerList").val() != "") {
                        AddTicketConfirmation(false, true);
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Can not create a " + strTicket + " without an address.");
                    }
                }
            }
    });
    $("#MakeReply").click(function () {
        console.log("asi");
        if (tinyMCE.get('BodyContent').getContent() != "") {
            AddTicketReply();
        }
        else {
            $(".mce-panel").css('border-color', "red");
        }
    });
    $(".AttachInvoiceEstimatebtn").click(function () {
        if (AttachVal == "Invoice") {
            if ($("#InvoiceList").val() == "-1") {
                OpenErrorMessageNew("Error!", "Please choose an estimate first");
            } else {
                AddAttachment($("#InvoiceList").val());
            }

        } else if (AttachVal == "Estimate") {
            if ($("#EstimateList").val() == "-1") {
                OpenErrorMessageNew("Error!", "Please choose an invoice first");
            } else {
                AddAttachment($("#EstimateList").val());
            }
        }
    });
    $("#UploadCustomerFileBtn").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });

    InitUploader();
    $("#AddFileBtn").click(function () {
        InitUploader();
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        } else {
            OpenErrorMessageNew("Error!", "Please choose file to upload.");
        }
        return false;
    });
    $("#UpdateEqList").click(function () {
        SaveServiceOrderEquipmentDetailsFromTicket(EquipmentTypes.Equipment);
    });
    $("#UpdateServiceList").click(function () {
        SaveServiceOrderEquipmentDetailsFromTicket(EquipmentTypes.Service);
    });
    $("#AttachEstimate").click(function () {
        HideAllAttachmentsDiv();
        AttachVal = "Estimate";
        $(".attach_estimate").removeClass("hidden");
    });
    $("#AttachInvoice").click(function () {
        HideAllAttachmentsDiv();
        AttachVal = "Invoice";
        $(".attach_invoice").removeClass("hidden");
        $(".attach_invoice").removeClass("hidden");
    });
    $("#AttachFile").click(function () {
        HideAllAttachmentsDiv();
        $(".addfile_tr").removeClass("hidden");
    });
    $("#SaveMisc").click(function () {
        var url = domainurl + "/Ticket/SaveMisc";
        var param = JSON.stringify({
            TicketId: TicketId,
            MiscName: $("#MiscName").val(),
            MiscValue: $("#MiscValue").val()
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
                if (data.result == true) {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
                else {
                    OpenErrorMessageNew("Error!", "Something Wrong");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    $("#AttachProducts").click(function () {
        HideAllAttachmentsDiv();
        $(".Add_equipment").removeClass("hidden");
    });
    $("#AttachServices").click(function () {
        HideAllAttachmentsDiv();
        $(".Add_service").removeClass("hidden");
    });

    /*$("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });*/
    $("#PrintTicket").click(function () {
        $("#TicketPrint").click();
    });
    //$(".add_ticktet_btn_footer").width($(".header-title").width());
    if (LogActive.toLowerCase() == "true") {
        console.log("log active");
        OpenLogTicket();
    }
    else {
        console.log("note active");
        OpenNoteTicket();
    }
    $(".search_item_val").keyup(function (event) {
        event.preventDefault();
        if (event.keyCode == 13) {
            SearchReplyTicketLog();
        }
    });
    $(".search_item_val_note").keyup(function (event) {
        event.preventDefault();
        if (event.keyCode == 13) {
            SearchReplyTicketNote();
        }
    });



    $(document).on('click', '.Ticket-delete-reply', function () {
      
        selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", DeleteFile);
        //LoadProductCategory(true);
    });
    $("#CreateDOTicket").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to create DO?", CreateDOTicket);
    });
    /*Equiepment menu hide on body click*/
    $("#CustomerInvoiceTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide()
    });
    /*Row Click*/
    $("#CustomerInvoiceTab tbody").on('click', 'tr', function (e) {
        console.log("hit");
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        //console.log(e.target);

        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount") || $(e.target).hasClass("spnProductEquipmentvendorcost")
            || $(e.target).hasClass("spnProductQuantityLeft")) {

            $("#CustomerInvoiceTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerInvoiceTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
        InitRowIndexBadEquipment();
    });
    $("#CustomerServiceTable tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        //console.log(e.target);

        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount")) {

            $("#CustomerServiceTable tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerServiceTable tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
        InitRowIndexBadService();
    });
    /*Row Click Ends*/
    /*Add New Row Start (done)*/
    $("#CustomerInvoiceTab tbody").on('click', 'tr:last', function (e) {
        if (AfterCompleteTicketItemAdd == 'true' || TicketStatus != "Completed") {
            if ($(e.target).hasClass('fa')) {
                return;
            }
            $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
        }
    });
    $("#CustomerServiceTable tbody").on('click', 'tr:last', function (e) {
        if (AfterCompleteTicketItemAdd == 'true' || TicketStatus != "Completed") {
            if ($(e.target).hasClass('fa')) {
                return;
            }
            $("#CustomerServiceTable tbody tr:last").after(NewServiceRow);
        }
    });
    /*Add New Row Ends*/
    /*If Equipment/Service is not selected clean up values*/
    $("#CustomerInvoiceTab tbody, #CustomerServiceTable tbody").on('blur', 'tr', function (item) {
        if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined') {
            var trdom = $(item.target).parent().parent();
            $(trdom).find("input.ProductName").val('');
            $(trdom).find("span.spnProductName").text('');

            $(trdom).find("input.txtProductDesc").val('');
            $(trdom).find("span.spnProductDesc").text('');
            $(trdom).find("span.spnSku").text('');

            $(trdom).find("span.spnProductPoint").text('');
            $(trdom).find("span.spnProductQuantityOnHand").text('');

            $(trdom).find("input.txtProductQuantity").val('');
            $(trdom).find("span.spnProductQuantity").text('');

            $(trdom).find("input.txtProductUnitPrice").val('');
            $(trdom).find("span.spnProductRate").text('');

            $(trdom).find("input.txtProductAmount").val('');
            $(trdom).find("span.spnProductAmount").text('');

            $(trdom).find("span.spnProductEquipmentvendorcost").text('');

            //$(trdom).find("input.txtProductEquipmentvendorcost").val('');
            //$(trdom).find("span.spnProductEquipmentvendorcost").text('');

            $(trdom).find("input.txtProductQuantityLeft").val('');
            $(trdom).find("span.spnProductQuantityLeft").text('');
            CalculateNewAmount();
        }
    });
    /*If Equipment/Service is not selected clean up values ends*/
    /*Remove Product from equipment/Service List*/
    $(".CustomerInvoiceTab tbody").on('click', 'tr td i.fa.fa-trash-o', function (e) {
        var ThisObject = $(this);
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", function () {
            var ThisRow = ThisObject.parent().parent();
            var ProductName = $(ThisRow).find("input.ProductName").val();
            if (ProductName.trim() != "") {
                RemovedEquipmentList = RemovedEquipmentList.concat(ProductName);
            }
            ThisObject.parent().parent().remove();
            if ($("#CustomerInvoiceTab tbody tr").length < 2) {
                $("#CustomerInvoiceTab tbody tr:last").after(NewEquipmentRow);
            }
            if ($("#CustomerServiceTable tbody tr").length < 2) {
                $("#CustomerServiceTable tbody tr:last").after(NewServiceRow);
            }
            //var i = 1;
            //InitRowIndex();
            CalculateNewAmount();
            if (typeof (ThisObject.attr('data-id')) != "undefined" && ThisObject.attr('data-id') != null && ThisObject.attr('data-id') != "") {
                CustomerCreditForTicketInvoice(ThisObject.attr('data-id'));
            }

        });
    });
    $("#btnSchedule").click(function () {
        LoadScheduleCalendar();
    });
    

    $(".top_to_bottom_modal_container").click(function (e) {
        //e.preventDefault();
        //console.log(e);
        if (!$(e.target).hasClass('fa-user-circle')) {
            $('.UserSuggestion.tt-menu').hide()
        }
    });
    $("#Service_SoldBy").change(function () {
        if ($(this).val() != "-1") {
            $("#CustomerServiceTable tbody tr i.fa-user-circle").attr('user-id', $(this).val());
            $("#CustomerServiceTable tbody tr").attr('user-id', $(this).val());
        }
    });
    $("#Equipment_SoldBy").change(function () {
        if ($(this).val() != "-1") {
            $("#CustomerInvoiceTab tbody tr i.fa-user-circle").attr('user-id', $(this).val());
            $("#CustomerInvoiceTab tbody tr").attr('user-id', $(this).val());
        }
    });
    /*Remove Product from equipment/Service List Ends*/
    $(".CustomerInvoiceTab tbody").on('click', 'tr td i.fa.fa-user-circle', function (e) {
        var ttMenuDom = $(this).parent().find(".tt-menu");
        var IsVisible = $(ttMenuDom).is(":visible");
        $('.tt-menu').hide();
        if (!IsVisible) {
            $(ttMenuDom).show();
        }

        var datasetDom = $(this).parent().find(".tt-dataset");
        var userId = $(this).attr('user-id');
        console.log(userId);
        if (userId == '' || typeof (userId) == 'undefined') {
            userId = CurrentUserUserId;
        }

        if ($(datasetDom).html().trim() == "" || true) {
            var SetupHTML = "";
            var UserSuggTemplate = "<div data-id='{1}' class='UserSuggestionDiv {2}'>{0}</div>";
            $(UserList).each(function () {
                var RowItem = String.format(UserSuggTemplate, this.Name, this.UserId, (this.UserId == userId ? 'active' : ''));

                SetupHTML += RowItem;

            });
            $(datasetDom).html(SetupHTML);

        }
        //if( $(ttMenuDom).is(":visible")){
        //    $(ttMenuDom).hide();
        //}else{
        //    $(ttMenuDom).show();
        //}

    });
    $(".CustomerInvoiceTab tbody").on('click', '.UserSuggestionDiv', function (e) {
        var ActiveDom = $(this).parent().find('.active');
        $(ActiveDom).removeClass('active');
        $(this).addClass('active');
        var UserId = $(this).attr('data-id');
        $(this).parent().parent().parent().parent().parent().attr('user-id', UserId);
        $('.tt-menu').hide();
    });

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductDesc", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantity", function () {
        console.log("quantity");
        var ProductPointDom = $(this).parent().parent().find('p.spnProductPoint');
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        var inputProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
        var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        var inputProductRateDom = $(this).parent().parent().find('input.txtProductUnitPrice');

        var Point = $(ProductPointDom).attr("data-point");
        var Quantity = $(this).val();
        var UnitPrice = $(inputProductRateDom).val();
        var TotalAmount = (parseFloat(Quantity) * parseFloat(UnitPrice)).toFixed(2);
        var TotalPoint = (parseFloat(Quantity) * parseFloat(Point)).toFixed(2);

        $(ProductQuantityDom).text($(this).val());
        $(ProductAmountDom).text(Currency + TotalAmount);
        $(inputProductAmountDom).val(TotalAmount);
        $(ProductPointDom).text(TotalPoint);
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductUnitPrice", function () {
        //console.log("UnitPrice");
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        var inputProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');
        var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        var inputProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');
        var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        var inputProductRateDom = $(this).parent().parent().find('input.txtProductUnitPrice');
        var Quantity = $(inputProductQuantityDom).val();
        var UnitPrice = $(inputProductRateDom).val();
        var TotalAmount = (parseFloat(Quantity) * parseFloat(UnitPrice)).toFixed(2);
        $(ProductQuantityDom).text($(this).val());
        $(ProductRateDom).text(Currency + $(inputProductRateDom).val());
        $(ProductAmountDom).text(Currency + TotalAmount);
        $(inputProductAmountDom).val(TotalAmount);
    });

    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductAmount", function () {

        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        var inputProductQuantityDom = $(this).parent().parent().find('input.txtProductQuantity');

        var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        var inputProductAmountDom = $(this).parent().parent().find('input.txtProductAmount');

        var ProductRateDom = $(this).parent().parent().find('span.spnProductRate');
        var inputProductRateDom = $(this).parent().parent().find('input.txtProductUnitPrice');

        var Quantity = $(inputProductQuantityDom).val();
        var TotalAmount = parseFloat($(this).val());
        var UnitPrice = (parseFloat(TotalAmount) / parseFloat(Quantity)).toFixed(2);


        $(inputProductRateDom).val(UnitPrice);
        $(ProductRateDom).text(Currency + UnitPrice);

        $(ProductAmountDom).text(Currency + TotalAmount);
        $(inputProductAmountDom).val(TotalAmount);

    });


    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductAmountDom = $(this).parent().find('span.spnProductAmount');
        var Amount = $(this).val();
        $(ProductAmountDom).text(TransCurrency + Amount);
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtProductQuantityLeft", function () {

        var ProductAmountDom = $(this).parent().find('span.spnProductQuantityLeft');
        var Amount = $(this).val();
        $(ProductAmountDom).text(Amount);
    });
    var removeItem = $("#AssignedTo").val();

    $(".load_additional_members").load("/Ticket/LoadTicketAdditionalMembers?ticketid=" + TicketIntId + "&assigned=" + removeItem);
    
   console.log("LoadingFind");
    var elements = document.getElementsByClassName("add_ticket_main");
    elements[0].addEventListener('click', LoadDropdownValues, false);
    //$(".ticketCloseDiv").click(function () {
    //    var count = localStorage.clickcount = 0;
    //});
    $(".AssignedPersonContactInfo .AssignedPhone").text($("#AssignedTo").find(":selected").attr('phone') == '' ? '-' : $("#AssignedTo").find(":selected").attr('phone'));
    $(".AssignedPersonContactInfo .AssignedEmail").text($("#AssignedTo").find(":selected").attr('email') == '' ? '-' : $("#AssignedTo").find(":selected").attr('email'));
    $("#AssignedTo").change(function () {
        EmployeeHolidayCheck();
        removeItem = $("#AssignedTo").val();
        $(".load_additional_members").load("/Ticket/LoadTicketAdditionalMembers?ticketid=" + TicketIntId + "&assigned=" + removeItem);

        $(".AssignedPersonContactInfo .AssignedPhone").text($("#AssignedTo").find(":selected").attr('phone') == '' ? '-' : $("#AssignedTo").find(":selected").attr('phone'));
        $(".AssignedPersonContactInfo .AssignedEmail").text($("#AssignedTo").find(":selected").attr('email') == '' ? '-' : $("#AssignedTo").find(":selected").attr('email'));

    })
     
    $(".LoadReferenceTicketList").load("/Ticket/LoadReferenceTicketList?id=" + TicketIntId);
    $("#btn_del_ticket").click(function () {
        console.log("deleteticket");
        OpenConfirmationMessageNew("Confirmation", "Are you sure, you want to delete this item?", function () {
            var idval = $("#btn_del_ticket").attr('data-id');
            $.ajax({
                type: "POST",
                url: "/Ticket/DeleteCustomerTicket",
                data: JSON.stringify({ id: idval, CustomerId: GuidCustomer, TicketId: TicketIntId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                beforeSend: function () {
                    $(".loader-div").show();
                },
                success: function (data) {
                    $(".loader-div").hide(); // Hide the loader on success
                    OpenSuccessMessageNew("Success", "Ticket Deleted Successfully", function () {
                        console.log("Ticket deletion callback executed");
                        CloseTopToBottomTicket();
                        OpenTicketTab();
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $(".loader-div").hide();
                    console.log(errorThrown);
                }
            });
        });
    });
    $(".addressMapPopup").click(function () {
        CustomerGuidID = $(this).attr('data-id');
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + CustomerGuidID;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
    $("#PrintTicketForPhone").click(function () {
        var DownloadUrl = domainurl + "/Ticket/PrintTicket/?Id=" + TicketIntId;
        parent.window.open(DownloadUrl, '_blank');
        parent.$.magnificPopup.close();
    });
});
$(window).resize(function () {
    //$(".top_to_bottom_modal_container").height(window.height);
    if (screen.width < 414) {
        $(".add_ticket_main").height((window.innerHeight - $(".header-title").height() - 55));
    }
    else {
        $(".add_ticket_main").height((window.innerHeight - 107));
    }

});

var TimeZoneValue = function (dt) {
    return (-dt.getTimezoneOffset() < 0 ? '-' : '+') + (Math.abs(dt.getTimezoneOffset() / 60) < 10 ? '0' : '') + (Math.abs(dt.getTimezoneOffset() / 60)) + '00';
}

var EmployeeHolidayCheck = function () {
    $("#spanText").addClass("hidden");
    $(".add_ticktet_btn_footer #CreateTicket").show();
    var SelectedEmployee = $("#AssignedTo").val();
    var SelectedDate = $("#Ticket_CompletionDate").val();
    if (SelectedDate != null && SelectedEmployee != null) {
        var url = "/Calendar/EmployeeHolidayChecker";
        var param = JSON.stringify({
            employeeId: SelectedEmployee,
            date: SelectedDate,
        });
        $.ajax({
            type: "POST",
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) { 
                if (data.calendarlist != "") {
                    var arr = data.calendarlist.split(',');  
                    if (CompletionDatepicker != "") {
                        CompletionDatepicker.destroy();
                    }
                    //CompletedDatepicker = "";
                    CompletionDatepicker = new Pikaday({
                        field: $('.CompletionDate')[0],
                        format: 'MM/DD/YYYY',
                        onSelect: function (dateText) { 
                            EmployeeHolidayCheck();
                        },
                        //minDate: moment().toDate(),        
                        disableDayFn: function (date) {
                            if (arr.length > 0) { 
                                var zone = String(TimeZoneValue(date));
                                var firstChar = zone.charAt(0);
                                if (firstChar == '-') {
                                    date.setDate(date.getDate() - 1);
                                }
                                date = date.addDays(1);
                                date = date.toISOString().split('T')[0];
                                for (let i = 0; i < arr.length; i++) {
                                    if (date == arr[i]) {
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    });
                }
                if (data.operationsdisableDate != "") {
                    var array = data.operationsdisableDate.split(','); 
                    if (array.length > 0) { 
                            $("#spanText").text("(Have a day off this date.)");
                            $("#spanText").removeClass("hidden"); 
                     } 

                    if (CompletionDatepicker != "") {
                        CompletionDatepicker.destroy();
                    }
                    //CompletedDatepicker = "";
                    CompletionDatepicker = new Pikaday({
                        field: $('.CompletionDate')[0],
                        format: 'MM/DD/YYYY',
                        onSelect: function (dateText) { 
                            EmployeeHolidayCheck();
                        },
                        //minDate: moment().toDate(),        
                        disableDayFn: function (date) {
                            if (array.length > 0) {
                                var zone = String(TimeZoneValue(date));
                                var firstChar = zone.charAt(0);
                                if (firstChar == '-') {
                                    date.setDate(date.getDate() - 1);
                                }
                                date = date.addDays(1);
                                date = date.toISOString().split('T')[0];
                                for (let i = 0; i < array.length; i++) {
                                    if (date == array[i]) {
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    });
                }
                else {
                    if (CompletionDatepicker != "") {
                        CompletionDatepicker.destroy();
                    }
                    CompletionDatepicker = new Pikaday({
                        field: $('.CompletionDate')[0],
                        format: 'MM/DD/YYYY',
                        onSelect: function (dateText) { 
                            EmployeeHolidayCheck();
                        }
                    });
                }
                if (data.result) {
                    if (data.employeeinfo.Type == "MultipleDay") {
                        OpenLeaveMessageNew(data.employeeinfo.Name + " has " + data.employeeinfo.PaidStatus + " leave!", "Multiple day off from " + data.employeeinfo.StartDate + " to " + data.employeeinfo.EndDate + " ! Please allocate others to do this task.", "Do you still want to schedule " + data.employeeinfo.Name.toLowerCase() + " for this task?");
                    }
                    else if (data.employeeinfo.Type == "HalfDay") {
                        OpenLeaveMessageNew(data.employeeinfo.Name + " has " + data.employeeinfo.PaidStatus + " leave!", "Half day off on " + data.employeeinfo.StartDate + " ! Please allocate others to do this task.", "Do you still want to schedule " + data.employeeinfo.Name.toLowerCase() + " for this task?");
                    }
                    else if (data.employeeinfo.Type == "CustomTime") {
                        OpenLeaveMessageNew(data.employeeinfo.Name + " has " + data.employeeinfo.PaidStatus + " leave!", "Leave time from " + data.employeeinfo.StartTime + " to " + data.employeeinfo.EndTime + " ! Please allocate others to do this task.", "Do you still want to schedule " + data.employeeinfo.Name.toLowerCase() + " for this task?");
                    }
                    else {
                        OpenLeaveMessageNew(data.employeeinfo.Name + " has " + data.employeeinfo.PaidStatus + " leave!", "Full day off on " + data.employeeinfo.StartDate + " ! Please allocate others to do this task.", "Do you still want to schedule " + data.employeeinfo.Name.toLowerCase() + " for this task?");
                    }

                    //OpenSuccessMessageNew("Success", "Ticket payment saved successfully");
                    //$("#ModalSuccessMessage .success_close").text("Close");
                }
            }
        });
    }
}