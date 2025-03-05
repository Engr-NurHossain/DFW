var InvDeleteId = 0;
var InvoiceList = [];
var DeleteInvoicebyId = function () {
    $.ajax({
        url: domainurl + "/Invoice/DeleteInvoice",
        data: { Id: InvDeleteId },
        type: "Post",
        dataType: "Json",
        success: function () {
            OpenInvoiceTab();
        }
    });
}

var OpenInvById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?Id=" + invId);
    }
    else if (typeof (invId) != "undefined" && invId.indexOf('INV') > -1) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?InvoiceId=" + invId);
    }
}

var OpenInvCustomerById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoiceCustomer/?Id=" + invId);
    }
}
var OpenInvByIdAndCustomerId = function (invId, CusId) {
    if (typeof (invId) != "undefined" && invId.toLowerCase().indexOf("inv") > -1) {
        if (typeof (customerId) == "undefined") {
            customerId = 0;
        }
        /*console.log("/Invoice/AddInvoice/?CustomerGuid=" + CusId + "&Id=" + invId.split('-')[1]);*/
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?CustomerGuid=" + CusId + "&InvoiceId=" + invId);

    }
}
var ReceivePaymentByInv = function (invId) {
    if (typeof (CustomerLoadId) == "undefined") {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?InvoiceId=" + invId);
    } else {
        OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + CustomerLoadId + "&InvoiceId=" + invId);
    }

}
var ReceivePaymentByInvAndCus = function (invId, cusId) {
    OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerGuid=" + cusId + "&InvoiceId=" + invId);
}
var OpenCustomerDetailById = function (cusId) {
    if (typeof (cusId) == "undefined") {
        return;
    }
    window.open(domainurl + "/Customer/Customerdetail/?id=" + cusId);
}
var LoadARBData = function () {
    var SearchBy = "";
    var BilligCycle = $("#BillingCycle").val();
    var BillingDay = $("#BillingDayDrp").val();
    var TaxApplied = $("#TaxApplied").val();
    var SearchText = $(".all_invoices_search_text").val();
    var PaymentFilter = $("#PaymentFilter").val();
    var pageNo = 1;
    $(".invoices-tabs .tab-pane.active").html(TabsLoaderText);
    $(".invoices-tabs .tab-pane.active").load(domainurl + "/Sales/AllInvoices/?PageNo=" + pageNo
        + "&SearchText=" + SearchText
        + "&SearchBy=" + SearchBy
        + "&InvoiceFor=" + encodeURI(InvoiceFor)
        + "&BillingCycle=" + BilligCycle
        + "&BillingDay=" + BillingDay
        + "&IsTax=" + TaxApplied
        + "&PaymentFilter=" + PaymentFilter
        + "&SortBy=" + InvoiceSortBy
        + "&SortOrder=" + InvoiceSortOrder
        );
}
var LoadACHARBData = function (pageno, orderval) {
    console.log(pageno+" "+orderval);
    //var SearchBy = "";
    var BilligCycle = $("#BillingCycle").val();
    //var BillingDay = $("#BillingDayDrp").val();
    //var TaxApplied = $("#TaxApplied").val();
    var SearchText = $(".all_invoices_search_text").val();
    var StartDate = $(".InvoiceFilterStartDate").val();
    var EndDate = $(".InvoiceFilterEndDate").val();

    //var PaymentFilter = $("#PaymentFilter").val();
    var InvoiceSortOrder = orderval;
    var pageNo = 1;
    $(".invoices-tabs .tab-pane.active").html(TabsLoaderText);

 

    $(".invoices-tabs .tab-pane.active").load(domainurl + "/Sales/SalesARBInvoices/?PageNo=" + pageNo
        + "&SearchText=" + encodeURI(SearchText)
        //+ "&SearchBy=" + SearchBy
        + "&InvoiceFor=" + encodeURI(InvoiceFor)
        + "&BillingCycle=" + BilligCycle
        //+ "&BillingDay=" + BillingDay
        //+ "&IsTax=" + TaxApplied
        //+ "&PaymentFilter=" + PaymentFilter
        + "&SortBy=" + InvoiceSortBy
        + "&SortOrder=" + InvoiceSortOrder
        + "&InvoiceStartDate=" + StartDate
        + "&InvoiceEndDate=" + EndDate
        );
}


var LoadARBDataMonthly = function () {
    var SearchBy = "";
    var BilligCycle = "Monthly";
    var BillingDay = $("#BillingDayDrp").val();
    var TaxApplied = $("#TaxApplied").val();
    var SearchText = $(".all_invoices_search_text").val();
    var PaymentFilter = $("#PaymentFilter").val();
    var pageNo = 1;
    $(".invoices-tabs .tab-pane.active").html(TabsLoaderText);
    $(".invoices-tabs .tab-pane.active").load(domainurl + "/Sales/AllInvoices/?PageNo=" + pageNo
        + "&SearchText=" + SearchText
        + "&SearchBy=" + SearchBy
        + "&InvoiceFor=" + encodeURI(InvoiceFor)
        + "&BillingCycle=" + BilligCycle
        + "&BillingDay=" + BillingDay
        + "&IsTax=" + TaxApplied
        + "&PaymentFilter=" + PaymentFilter
        + "&SortBy=" + InvoiceSortBy
        + "&SortOrder=" + InvoiceSortOrder
        );
}
var LoadARBDataByStatus = function (status) {
    var SearchBy = "";
    var BilligCycle = $("#BillingCycle").val();
    var BillingDay = $("#BillingDayDrp").val();
    var TaxApplied = $("#TaxApplied").val();
    var SearchText = $(".all_invoices_search_text").val();
    var PaymentFilter = $("#PaymentFilter").val();
    var pageNo = 1;
    $(".invoices-tabs .tab-pane.active").html(TabsLoaderText);
    $(".invoices-tabs .tab-pane.active").load(domainurl + "/Sales/AllInvoices/?PageNo=" + pageNo
        + "&SearchText=" + SearchText
        + "&SearchBy=" + SearchBy
        + "&InvoiceFor=" + encodeURI(InvoiceFor)
        + "&BillingCycle=" + BilligCycle
        + "&BillingDay=" + BillingDay
        + "&IsTax=" + TaxApplied
        + "&PaymentFilter=" + PaymentFilter
        + "&SortBy=" + InvoiceSortBy
        + "&SortOrder=" + InvoiceSortOrder
        + "&Status=" + status
        );
}
$(document).ready(function () {


    //$('#BillingCycle').val('Monthly');

    $("#all_invoices_search_button").click(function () {
        LoadARBData();
    });
    $("#allinvoices_search_button").click(function () {
        LoadInvoiceData();
    }); 
    $("#all_ach_cc_invoices_search_button").click(function () {
        LoadACHARBData();
    });
  
    $("#TotalCustomerMMR").click(function () {
        LoadARBData();
    });
    $("#MonthlyCustomer").click(function () {
        LoadARBDataMonthly();
    });
    $("#OpenInvoice").click(function () {
        LoadARBDataByStatus("Open");
    });
    $("#ReturnedCustomer").click(function () {
        LoadARBDataByStatus("Declined");
    });
    $("#CreditCardExipiring").click(function () {
        LoadARBDataCreditCardExipiring();
    });
    //$("#AddInvoice").click(function () {
    //    OpenTopToBottomModal("/Invoice/AddInvoice/?CustomerId=" + customerId);
    //});
    $(".item-delete").click(function () {
        InvDeleteId = $(this).attr("data-id");

        OpenConfirmationMessageNew("Confirm?", "Are your sure you want to delete?", DeleteInvoicebyId);
    });
    $("#PrintInvoiceList").click(function () {
        if ($("#OpenInvoiceVal").text() == "0") {
            OpenErrorMessageNew("Error!", "You have no invoice to print.");
        }
        else {
            InvoiceList = [];
            $(".checkboxes:checked").each(function () {
                InvoiceList.push($(this).attr('idval'));
            });
            if ($(".checkboxes:checked").length == 0) {
                var SearchBy = "";
                var BilligCycle = $("#BillingCycle").val();
                var BillingDay = $("#BillingDayDrp").val();
                var TaxApplied = $("#TaxApplied").val();
                var SearchText = $(".all_invoices_search_text").val();
                var PaymentFilter = $("#PaymentFilter").val();
                var pageNo = 1;
                var success = window.open(domainurl + "/Invoice/DownloadInvoicePdfList/?PageNo=" + pageNo
               + "&SearchText=" + SearchText
               + "&SearchBy=" + SearchBy
               + "&InvoiceFor=" + encodeURI(InvoiceFor)
               + "&BillingCycle=" + BilligCycle
               + "&BillingDay=" + BillingDay
               + "&IsTax=" + TaxApplied
               + "&PaymentFilter=" + PaymentFilter
               + "&SortBy=" + InvoiceSortBy
               + "&SortOrder=" + InvoiceSortOrder
               + "&OpenStatus=open"
               );
                if (!success) {
                    OpenErrorMessageNew("Error!", "Your browser cancelling popups.");
                }
            } else {
                var param = JSON.stringify({
                    InvoiceIdList: InvoiceList
                });
                var url = domainurl + "/Invoice/SaveInvoicePdfList";
                $.ajax({
                    type: "POST",
                    /*ajaxStart: $(".loader-div").show(),*/
                    url: url,
                    data: param,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (data) {
                        if (data.result) {
                            var success = window.open(data.filePath, '_blank');
                            if (!success) {
                                OpenErrorMessageNew("Error!", "Your browser cancelling popups.");
                            }
                        } else {
                            OpenErrorMessageNew("Error!", data.message);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }
                });
            }
        }
    });

    $("#BillingDayDrp").val($("#BillingDay").val());
    $("#TaxApplied").val($("#IsTax").val());
    $("#PaymentFilter").val($("#PaymentFilterVal").val());

    setTimeout(function () {
        /*Give a Print All Option ONLY for Invoice Tab*/

        if (typeof (InvoiceFor) != 'undefined' && InvoiceFor == "SystemGenerated") {
            $("#PrintInvoiceList").show();
        }
    }, 100);
});
