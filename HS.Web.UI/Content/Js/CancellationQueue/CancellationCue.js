String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var DataTablePageSize = 50;
var OpenReceivePayment = function (customerId) {
    OpenTopToBottomModal("/Transaction/ReceivePayment/?CustomerId=" + customerId);
}
var CancellationId = "";
var MakeRemoveFromQueueConfirm = function () {
    var url = "/Reports/MakeRemoveFromQueue";
    var RemoveFromQueueList = [];
    $(".CancellationCus").each(function (id) {
        CancellationId = $(this).attr('id');
        if ($(".IsCancelItem_" + CancellationId).is(':checked')) {
            RemoveFromQueueList.push(CancellationId);
        }
    });

    var param = JSON.stringify({
        RemoveFromQueueList: RemoveFromQueueList
    });

    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", function () {
                $(".CustomerCancellation_report").html(TabsLoaderText);

                $(".CustomerCancellation_report").load(domainurl + "/Reports/CancellationCuePartial?pageno=1&pagesize=50");
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var MakeCancelCustomerConfirm = function () {
    var url = "/Reports/MakeCancelCustomer";
    var CustomerIdList = [];
    var CustomerCancellationList = [];
    $(".CancellationCus").each(function (id) {
        CancellationId = $(this).attr('id');
        if ($(".IsCancelItem_" + CancellationId).is(':checked')) {
            CustomerCancellationList.push({
                "CustomerId": $(this).attr('customerid'),
                "IsInvoiceOff": $(".IsInvoiceOff_" + CancellationId).is(':checked'),
                "IsBillingOff": $(".IsBillingOff_" + CancellationId).is(':checked'),
                "IsAlarmOff": $(".IsAlarmOff_" + CancellationId).is(':checked')
            });
        }
        console.log(CustomerCancellationList);
    });
 
    var param = JSON.stringify({
        CancellationList: CustomerCancellationList
    });

    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            OpenSuccessMessageNew("Success!", "", function () {
                $(".CustomerCancellation_report").load(domainurl + "/Reports/CancellationCuePartial?pageno=1&pagesize=50");
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var MakeSelectAll = function () {
    if ($(".SelectAll").text() == "Select All")
    {
        $(".SelectAll").text("DeSelect All");
        $(".cancel-item").prop("checked", true);

    }
    else {
        $(".SelectAll").text("Select All");
        $(".cancel-item").prop("checked", false);
    }
}
var MakeRemoveFromQueue = function () {
    var CustomerIdList = [];
    $(".CancellationCus").each(function (id) {
        CancellationId = $(this).attr('id');
        if ($(".IsCancelItem_" + CancellationId).is(':checked')) {
            CustomerIdList.push($(this).attr('customerid'));
        }
    });
    if (CustomerIdList.length > 0) {
        OpenConfirmationMessageNew("", "Are you sure you want to remove selected customer(s) from cancellation queue?", function () {
            MakeRemoveFromQueueConfirm();
        });
    }
    else {
        OpenErrorMessageNew("Error!", "Please select a customer.", "");
    }
}
var MakeCancelCustomer = function () {
    var CustomerIdList = [];
    $(".CancellationCus").each(function (id) {
        CancellationId = $(this).attr('id');
        if ($(".IsCancelItem_" + CancellationId).is(':checked')) {
            CustomerIdList.push($(this).attr('customerid'));
        }
    });
    if (CustomerIdList.length > 0) {
        OpenConfirmationMessageNew("", "Are you sure you want to cancel selected customer(s)?", function () {
            MakeCancelCustomerConfirm();
        });
    }
    else {
        OpenErrorMessageNew("Error!", "Please select a customer.", "");
    }
}

var CustomerSearchKeyUpforCancelledcuswithpagination = function (pageno) {
    pagesize = parseInt(cn) +50;

    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    //$(".CancelledCustomer_report").html(TabsLoaderText);
    $(".CancelledCustomer_report").load(domainurl + "/Reports/CancelledCustomerPartial/?Start=" + StartDate + "&End=" + EndDate + "&pageno=" + pageno + "&pagesize=" + pagesize);
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $(".CustomerCancellation_report").html(TabsLoaderText);
    $(".CustomerCancellation_report").load(domainurl + "/Reports/CancellationCuePartial?pageno=1&pagesize=50&GetReport=false");
 
    $(".CustomerCancellationNav").click(function () {
        $(".CustomerCancellation_report").html(TabsLoaderText);
        $(".CustomerCancellation_report").load(domainurl + "/Reports/CancellationCuePartial?pageno=1&pagesize=50&GetReport=false");
    });

    $(".CancelledCustomerNav").click(function () {
        $(".CancelledCustomer_report").html(TabsLoaderText);
        $(".CancelledCustomer_report").load(domainurl + "/Reports/CancelledCustomerPartial?pageno=1&pagesize=50&GetReport=false");
    });
    $(".page-wrapper-contents").scroll(function (e) {
        var orderval = $(this).attr('data-val');

        e.preventDefault();

        if ($(e.target).scrollTop() + $(e.target).innerHeight() >= $(e.target)[0].scrollHeight) {
            $("#myTab li").each(function () {
                if ($(this).hasClass('CustomerCancellationNav active')) {
                    CustomerSearchKeyUp(1);
                }
                if ($(this).hasClass('CancelledCustomerNav active')) {
                    CustomerSearchKeyUpforCancelledcuswithpagination(1);
                }
          
            })
        }
    })
});