var DataTablePageSize = 50;
var ConvertEstimeToInvoiceById = function (EstimateConvertId) {
    $.ajax({
        url: domainurl + "/Invoice/ConvertEstimateToInvoice",
        data: { Id: EstimateConvertId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            //OpenEstimateTab();
            CloseTopToBottomModal();
            if (typeof (OpenInvoiceTab) != "undefined") {
                OpenInvoiceTab();
            }
            OpenSuccessMessageNew("", data.message);
            //OpenInvById(EstimateConvertId);
        }
    });
}
var OpenInvoiceNotesById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        if (typeof (CustomerLoadId) == "undefined") {
            CustomerLoadId = 0;
        }
        OpenRightToLeftModal(domainurl + "/Invoice/ShowInvoiceNotes/?InvoiceId=" + invId);
    }
}
var DeleteEstimateById = function (EstDeleteId) {
    $.ajax({
        url: domainurl + "/Invoice/DeleteInvoice",
        data: { Id: EstDeleteId },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Estimate deleted successfully.");
                OpenEstimateTab();
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}
var OpenEstById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal("/Estimate/AddEstimate/?Id=" + invId);
    }
}
var OpenEstimatorById = function (EstimatorId) {
    if (typeof (EstimatorId) != "undefined" && EstimatorId > 0) {
        OpenTopToBottomModal("/Estimator/AddEstimator/?Id=" + EstimatorId);
    }
}
var OpenInvById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?Id=" + invId);
    }
    else if (typeof (invId) != "undefined" && invId.indexOf('INV') > -1) {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?InvoiceId=" + invId);
    }
}
var ConvertEstimateToInvoice = function (id) {
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to Convert this Estimate to Invoice?", function () {
        ConvertEstimeToInvoiceById(id);
    });
}
$(document).ready(function () {


    if (typeof (CustomerLoadId) != "undefined") {
        /*
        This block will run only in customer detail page.
        */
        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        var EstimateTable = $(LoadCustomerDiv + '.tblEstimate').DataTable({
            "pageLength": DataTablePageSize,
            "destroy": true,
            "language": {
                "emptyTable": "No data available"
            },
            "order": [[2, "desc"]]
        });
    } else {
        var EstimateTable = $('.tblEstimate').DataTable({
            "pageLength": DataTablePageSize,
            "destroy": true,
            "language": {
                "emptyTable": "No data available"
            },
            "order": [[2, "desc"]]
        });
    }

    //EstimateTable.order([1, 'desc']).draw();
    $(".btn-add-estimator").click(function () {
        OpenTopToBottomModal("/Estimator/AddEstimator?customerid=" + CustomerLoadId);
    });
    $(".item-delete").click(function () {
        EstDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete?", function () {
            DeleteEstimateById(EstDeleteId)
        });
    });
    //$(".Convert-EstimeteToInvoice").click(function () {
    //    EstimateConvertId = $(this).attr("data-id");

    //})
})
