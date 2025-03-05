var DataTablePageSize = 50;
$(".datepicker").datepicker({
    //timepicker:false
});

var notTemplate = "";
notTemplate += "<div class='NoteTemplate'>"
                    + "<div class='InvoiceNote'>{2}"
                    + "</div>"
                    + "<div class='InvoiceNoteOptions clearfix'>"
                    + "<div class='NoteAddedByDiv'>"
                    + "<div>"
                    + "<span><b>Added By</b>:<span class='NoteAddedBy'>{0}</span></span>"
                    + "</div>"
                    + "<div>"
                    + "<span><b>Added Date</b>:<span class='NoteAddedDate'>{1}</span></span>"
                    + "</div>"
                    + "</div>"
                    + "</div>"
                + "</div>";


var makeInvoicePaid = function (InvoiceId) {
    /*Now Payment will by done by transaction*/
    /*var url = "/Invoice/MakeInvoicePaid";
    var param = JSON.stringify({
        id: InvoiceId
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
            if (data) {
                OpenSuccessMessage ("Success!", "Congratulations! Your invoice payment successfully done.");
                OpenInvoiceTab();
                CloseTopToBottomModal();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })*/

}

var EstimateConvertId = 0;
var ConvertEstimeToInvoiceById = function () {
    $.ajax({
        url: domainurl + "/Invoice/GetCustomerListByKey",
        data: { Id: EstimateConvertId },
        type: "Post",
        dataType: "Json",
        success: function () {
            CloseTopToBottomModal();
            OpenInvoiceTab();
            setTimeout(function () {
                OpenInvById(EstimateConvertId);
            },2000);
        }
    });
}

//var Showshipping = function () {
//    //$(".shipping").show();
//    $(".Shipping").show();
//}
//var Hideshipping = function () {
//    //$(".shipping").hide();
//    $(".Shipping").hide();
//}
//var ShowDiposit = function () {
//    $(".Diposit").show();
//}
//var HideDiposit = function () {
//    $(".Diposit").hide();
//}
//var ShowDiscount = function () {
//    $(".Discount").show();
//}
//var HideDiscount = function () {
//    $(".Discount").hide();
//}
//var ShowShippingDiv = function () {
//    $(".shipping-div").show();
//    $(".shipping-amount-div").show();
//}
var HideShippingDiv = function () {
    $(".shipping-div").hide();
    $(".shipping-amount-div").hide();
}
//var ShowDiscountDiv = function () {
//    $(".discount-amount-div").show();
//}
//var HideDiscountDiv = function () {
//    $(".discount-amount-div").hide();
//}
//var ShowDepositDiv = function () {
//    $(".deposit-amount-div").show();
//}
//var HideDepositDiv = function () {
//    $(".deposit-amount-div").hide();
//}

//var InvoiceSettingsInitialLoad = function () {
//    if (InvoiceShippingSetting == 'True') {
//        ShowShippingDiv();
//        Showshipping();
//    }
//    else {
//        HideShippingDiv();
//        Hideshipping();
//    }
//    if (InvoiceDiscountSetting == 'True') {
//        ShowDiscountDiv();
//        ShowDiscount();
//    }
//    else {
//        HideDiscountDiv();
//        HideDiscount();
//    }
//    if (InvoiceDepositSetting == 'True') {
//        ShowDepositDiv();
//        HideDiposit();
//    }
//    else {
//        HideDepositDiv();
//        HideDiposit();
//    }
//}

var InvoiceNoteSave = function () {
    if ($("#InvoiceNote").val() != null && $("#InvoiceNote").val() != '') {
    var url = domainurl + "/Invoice/AddInvoiceNote";
    var param = JSON.stringify({
        InvoiceId: Invoice_int_Id,
        Note: $("#InvoiceNote").val(),
        AddedBy: "0000000b-0004-0000-0000-000000050000",
        CompanyId: "0000000b-0004-0000-0000-000000050000",
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
            if (data.result) {
                var tempTemplate = String.format(notTemplate, data.AddedBy, data.AddedDate, data.Note);
                $(".InvoiceNotesList").append(tempTemplate);
                $("#InvoiceNote").val("")
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    //InvoiceSettingsInitialLoad();

    /*var WindowHeight = $(window).height();
    var divHeight = WindowHeight - 132;
    $(".invoice-informations").css("height", divHeight);*/

    setTimeout(function () {
        var WindowHeight = window.innerHeight;
        var divHeight = WindowHeight - ($(".add-invoice-container .div-header").height() + $(".invoice-footer").height() + 18);
        $(".invoie_contents_scroll").css("height", divHeight);
    }, 1000); 
    var table = $('#tblinfo').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $(".close-div").click(function () {
        parent.$(".add-invoice-div").hide();
    });

    /*
    //No Need, Payments will be done by Transaction
    $(".invoice-make-payment-div").click(function () {
        var invoideId = $(".invoice-make-poayment").attr('data-val');
        makeInvoicePaid(invoideId);
    });*/

    $(".settings-invoice").click(function () {
        OpenRightToLeftModal(domainurl + "/Invoice/InvoiceSettingsPartial");
    }); 
    $(".AddNewInvNotBtn").click(function () {
        if ($("#InvoiceNote").val().trim() == "") {
            return;
        }
        InvoiceNoteSave();
    });
})
