var OpenCancellationAgreement = function () {
    //OpenTopToBottomModal(domainurl + "/File/LoadFiletemplateForFileManagement/?customerId=" + CustomerLoadId + "&IsCancellationQueue=" + true);
    $("#CancellationAgreement").click();
}
//var CancellationView = function () {
//    OpenTopToBottomModal(domainurl + "/File/LoadFiletemplateForFileManagement/?customerId=" + CustomerLoadId + "&IsCancellationQueue=" + true);
//}
var InsertCustomerCancellationData = function () {
    var url = domainurl + "/Customer/InsertCustomerCancellationData";
    var param = JSON.stringify({
        CustomerId: CustomerId,
        CancellationDate: $("#CancellationDate").val(),
        RemainingBalance: $("#remainingbalance").val(),
        ExpirationDays: $("#ExpirationDays").val(),
        Note: $("#CacellationReason").val(),
        CacellationReasonList: $("#CacellationReasonList").val(),
        EmployeeReason: $("#CacellationReasons").val()
    })
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
                if (data.IsCancellAgreement == true) {
                    OpenConfirmationMessageNew("Confirm", "Are you sure you want to send cancellation document?", function () {
                        OpenCancellationAgreement();
                    }, function () {
                        OpenSuccessMessageNew("Success!", "Successfully added this customer to cancellation queue", CloseTopToBottomModal());
                    })
                }
                else {
                    OpenSuccessMessageNew("Success!", "Successfully added this customer to cancellation queue", CloseTopToBottomModal());
                }

            }
            else {
                OpenErrorMessageNew('Error!', data.message, "");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    $('.selectpicker').selectpicker();
    $(".ccc_inner").height(window.innerHeight - $(".ccc_header").height() - $(".ccc_footer").height() - 41);
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".LoadCancellationPopUp", type: 'iframe', width: Popupwidth, height: 650 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    var idlist1 = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".MapManufacturerMagnificPrevious", type: 'iframe', width: 920, height: 520 }
    ];
    jQuery.each(idlist1, function (i, val) {
        magnificPopupObj(val);
    });

    CancellationDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',

        field: $('.cancel-datepicker')[0],
        trigger: $('#CancellationDate_custom')[0],
        firstDay: 1,
        minDate: new Date()
    });

    $("#SaveCancel").click(function () {
        if (CommonUiValidation()) {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to add this customer to cancellation queue?", function () {
                InsertCustomerCancellationData();
            });
        }

    })
    $("#CacellationReasons").change(function () {
        console.log("dropdownchange");
        console.log($("#CacellationReasons").val());
        var a = $("#CacellationReasons").val();
        if (a == "-1") {
            $("#CacellationReasonsmsg").show();
        }
        else {
            $("#CacellationReasonsmsg").hide();

        }
    })
    $("#CancellationDate").change(function () {

        if ($("#CancellationDate").val != "") {
            $("#CancellationDatemsg").hide();
            $(".cancel-datepicker").removeClass('required');
        }
        else {
            $("#CancellationDatemsg").show();

        }
    })
})
$(window).resize(function () {
    $(".ccc_inner").height(window.innerHeight - $(".ccc_header").height() - $(".ccc_footer").height() - 41);
})
