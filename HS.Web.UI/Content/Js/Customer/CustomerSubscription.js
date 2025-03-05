var IsSubscribedCustomer = false;

var SyncFromAuthorize = function (customerid) {
    var url = domainurl + "/Customer/SyncFromAuthorize";
    var passparam = JSON.stringify({
        CustomerId: customerid
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data.message);
            if (!data.result) {
                OpenErrorMessageNew("Error!", data.message);
            } else {
                OpenSuccessMessageNew("Success!", data.message);
            }
            CloseTopToBottomModal();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var UnSubscribetToAuthorize = function () {

    var url = domainurl + "/Customer/UnsubscribeFromAuthorize";
    var passparam = JSON.stringify({
        AuthorizeRefId: $("#AuthorizeRefId").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $("#unsubscribe_to_authorize").addClass('hidden');
            console.log(data.message);
            if (!data.result) {
                OpenErrorMessageNew("Error!", data.message);
            } else {
                OpenSuccessMessageNew("Success!", data.message);
            }

            CloseTopToBottomModal();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var UnSubscribetToForte = function () {

    var url = domainurl + "/Customer/UnsubscribeFromForte";
    var passparam = JSON.stringify({
        ForteRefId: $(".ForteRefIdView").val()
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $("#unsubscribe_to_authorize").addClass('hidden');
            var head = "Success!";
            if (!data.result) {
                head = "Error!";
            }
            parent.OpenSuccessMessageNew(head, data.message);
            CloseTopToBottomModal();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var SubscribeToForte = function () {
    var url = domainurl + "/Customer/SubscribeToForte/";
    var passparam = AuthorizeParamReady();
    if (passparam == null) {
        return;
    }
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data);
            //ARB Subscription details 
            if (data.PaymentMethod == "Credit Card" && data.PaymentInfoId > 0) {
                $("#debit_payment_info_id").val(data.PaymentInfoId);
            } else if (data.PaymentMethod == "ACH" && data.PaymentInfoId > 0) {
                $("#ach_payment_info_id").val(data.PaymentInfoId);
            }
            if (typeof (data.ForteId) != "undefined" && data.ForteId != "" && data.ForteId != null) {
                if (typeof (PermissionsCheckForteRefIdView) != "undefined" && PermissionsCheckForteRefIdView && PermissionsCheckForteRefIdView != "False") {
                    $(".ForteRefIdView").val(data.ForteId);
                    $("#ScheduleToken1").val(data.ForteId);
                    $("#CustomerToken").val(data.ForteCustomerId);
                    $("#PaymentToken").val(data.FortePaymentMethodId);
                }
                $("#unsubscribe_to_forte").removeClass('hidden');
                $("#subscribe_to_forte").addClass('hidden');
            }
            /*Message part*/
            if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
                parent.OpenSuccessMessageNew("Success!", data.AuthMessage);
            } else if (data.status != true) {
                parent.OpenErrorMessageNew("Error!", data.AuthMessage);
            }
            /*Message part ends*/
            //ARB Subscription details ends 
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var SubscribeToAuthorize = function () {
    var url = domainurl + "/Customer/SubscribeToAuthorize/";
    var passparam = AuthorizeParamReady();
    if (passparam == null) {
        return;
    }
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data);
            //ARB Subscription details 
            if (data.PaymentMethod == "Credit Card" && data.PaymentInfoId > 0) {
                $("#debit_payment_info_id").val(data.PaymentInfoId);
            } else if (data.PaymentMethod == "ACH" && data.PaymentInfoId > 0) {
                $("#ach_payment_info_id").val(data.PaymentInfoId);
            }
            if (typeof (data.AuthId) != "undefined" && data.AuthId != "" && data.AuthId != null) {
                $(".AuthorizeRefIdView").val(data.AuthId);
                $("#AuthorizeRefId").val(data.AuthId);
            }
            CustomerSubscriptionCheck();
            /*Message part*/
            if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
                parent.OpenSuccessMessageNew("Success!", data.AuthMessage);
            } else if (typeof (data.message) != "undefined") {
                parent.OpenErrorMessageNew("Error!", data.message);
            }
            /*Message part ends*/
            //ARB Subscription details ends 
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var UpdateAuthorizeSubscription = function () {
    var url = domainurl + "/Customer/UpdateAuthorizeSubscription/";
    var passparam = AuthorizeParamReady();
    if (passparam == null) {
        return;
    }
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data);
            //ARB Subscription details
            //$(".authsyncbtn").addClass("hidden");
            /*Message part*/
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }

            /*
            if (typeof (data.HasMessage) != "undefined" && data.HasMessage && data.AuthorizeMessage) {
                parent.OpenSuccessMessageNew("Error!", data.ForteMessage);
            } else if (typeof (data.message) != "undefined") {
                parent.OpenSuccessMessageNew("Error!", data.message);
            }*/

            /*Message part ends*/
            //ARB Subscription details ends 
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var UpdateForteSubscription = function () {
    var url = domainurl + "/Customer/UpdateForteSubscription/";
    var passparam = AuthorizeParamReady();
    if (passparam == null) {
        return;
    }
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: passparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data);
            //ARB Subscription details

            $(".authsyncbtn").addClass("hidden");
            /*Message part*/
            if (typeof (data.HasMessage) != "undefined" && data.HasMessage) {
                parent.OpenSuccessMessageNew("Success!", data.ForteMessage);
            } else if (typeof (data.ForteMessage) != "undefined") {
                parent.OpenSuccessMessageNew("Success!", data.ForteMessage);
            }
            else {
                OpenErrorMessageNew("Error!", data.ForteMessage, function () {

                });
            }
            /*Message part ends*/
            //ARB Subscription details ends 




        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var PaymentMethodChangeMethod = function () {
   // var ShowPaymentGetwayItems = true;

    if ($("#PaymentMethod").val() == "Credit Card") {
        $(".Customer_Debit_Div").removeClass('hidden');
        $(".Customer_ACH_Div").addClass('hidden');
        $(".PaymentGetwayData").removeClass('hidden');

    } else if ($("#PaymentMethod").val() == "ACH") {
        $(".Customer_ACH_Div").removeClass('hidden');
        $(".Customer_Debit_Div").addClass('hidden');
        $(".PaymentGetwayData").removeClass('hidden');
    }
    else {
        $(".Customer_ACH_Div").addClass('hidden');
        $(".Customer_Debit_Div").addClass('hidden');
        $(".PaymentGetwayData").addClass('hidden');
        //ShowPaymentGetwayItems = false;
    }

    /*Payment getway related items show hide
    if (ShowPaymentGetwayItems) {
        if (PaymentGetway == PaymentGetwayAuthorize) {
            $(".Authorize_Descriptin_Area").removeClass("hidden");
        } else if (PaymentGetway == PaymentGetwayForte) {
            $(".Forte_Descriptin_Area").removeClass("hidden");
        }
    }
    else {
        $(".Authorize_Descriptin_Area").addClass("hidden");
        $(".Forte_Descriptin_Area").addClass("hidden");
    }*/
}

var CustomerSubscriptionCheck = function ()
{
    if ($("#AuthorizeRefId").val() == "" && $("#ScheduleToken1").val() == "") {
        IsSubscribedCustomer = false;
        $(".subscribebtn").removeClass("hidden");
        $(".unsubscribebtn").addClass("hidden");
        
    } else {
        IsSubscribedCustomer = true;
        $(".subscribebtn").addClass("hidden");
        $(".unsubscribebtn").removeClass("hidden");
    }
}

$(document).ready(function () {
    PaymentMethodChangeMethod();
    CustomerSubscriptionCheck();
    $("#syncwithForte").click(function () {
        console.log("dfsd");
        if ($(".forteRefId").val() == '') {
            OpenErrorMessageNew("Error!", "Forte subscription id not found.");
        } else {
            if (PaymentMethodValidation()) {
                parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to sync current payment method data with Forte?", function () {
                    UpdateForteSubscription();
                });
            }
            else {
                ScrollToError();
            }
        }
    });
    $("#subscribe_to_authorize").click(function () {
        if (/*StartDateValidation()*/ true /*&& BillingStartDayValidation()*/) {
            if (PaymentMethodValidation()) {
                parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to subscribe this customer to authorize.net?", function () {
                    SubscribeToAuthorize();
                });
            } else {
                ScrollToError();
            }
        } else {
            ScrollToError();
            parent.OpenErrorMessageNew("Error!", "Start date has to be greater than or equal to today.");
        }

    });
    $("#subscribe_to_forte").click(function () {
        if (/*StartDateValidation()*/ true /*&& BillingStartDayValidation()*/) {
            if (PaymentMethodValidation()) {
                parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to subscribe this customer to forte?", function () {
                    SubscribeToForte();
                });
            } else {
                ScrollToError();
            }
        } else {
            ScrollToError();
            parent.OpenErrorMessageNew("Error!", "Start date has to be greater than or equal to today.");
        }

    });

    $("#PaymentMethod").change(function () {
        PaymentMethodChangeMethod();
    });
    $("#unsubscribe_to_authorize").click(function () {
        parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to unsubscribe this customer from authorize.net?", function () {
            UnSubscribetToAuthorize();
        });
    });
    $("#sync_from_authorize").click(function () {
        parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to sync billing info from authorize.net?", function () {
            SyncFromAuthorize(CustomerLoadGuid);
        });
    });

    $("#unsubscribe_to_forte").click(function () {
        parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to unsubscribe this customer from forte?", function () {
            UnSubscribetToForte();
        });
    });
    $("#syncwithauthorize").click(function () {
        if ($(".AuthorizeRefId").val() == '') {
            OpenErrorMessageNew("Error!", "Athorize.net subscription id not found.");
        } else {
            if (PaymentMethodValidation()) {
                parent.OpenConfirmationMessageNew("Confirm?", "Are you sure, you want to sync current payment method data with authorize.net?", function () {
                    UpdateAuthorizeSubscription();
                });
            }
            else {
                ScrollToError();
            }
        }
    });

    /*if ($("#PayGetway").val() == "Authorize.Net") {
        $(".Authorize_Descriptin_Area").show();
        $(".Forte_Descriptin_Area").hide();
    }
    else {
        $(".Authorize_Descriptin_Area").hide();
        $(".Forte_Descriptin_Area").show();
    }*/
    /*
    if (PaymentGetway == PaymentGetwayAuthorize) {
        $(".AuthorizeRefIdView").show();
    }
    else if (typeof (PermissionsCheckForteRefIdView) != "undefined" && PermissionsCheckForteRefIdView && PermissionsCheckForteRefIdView != "False") {
        $(".ForteRefIdView").show();

    }
    */
    /*
    setTimeout(function () {
       if ($("#PaymentMethod").val() == "ACH" || $("#PaymentMethod").val() == "Credit Card") {
           if ($("#AuthorizeRefIdView").val().trim() != "") {
               $("#unsubscribe_to_authorize").removeClass("hidden");

           } else {
               $("#subscribe_to_authorize").removeClass("hidden");

           }
           if (typeof (PermissionsCheckForteRefIdView) != "undefined" && PermissionsCheckForteRefIdView && PermissionsCheckForteRefIdView != "False") {
               if($(".ForteRefIdView").val().trim() != ""){
                   $("#unsubscribe_to_forte").removeClass("hidden");
               }
               else {
                   $("#subscribe_to_forte").removeClass("hidden");
               }

          
           } else {

               $("#subscribe_to_forte").removeClass("hidden");
           }
       }
    }, 200);*/

});