var DataTablePageSize = 50;
var idsupplier = 0;

var LoadSupplierList = function () {
    setTimeout(function () {
        $(".ListContents").hide();
        $(".ListViewLoader").show();
        LoadSupplier(true);
    }, 500);
}

var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}
var ExpenseVendorsImportFile = function () {
    OpenRightToLeftModal(domainurl +"/File/AddExpenseVendorImportFile/");
}
var DeleteVendor = function (VendorDeleteId) {
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", function () {
        $.ajax({
            url: domainurl + "/Supplier/DeleteSupplier",
            data: { id: VendorDeleteId },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result) {
                    $("#expense_vendor_list").load(domainurl + "/Supplier/SupplierPertial");
                }
            },

            error: function () {
            }
        });
    });
}
$(document).ready(function () {
    $("#AddNewSupplier").click(function () {
        OpenRightToLeftModal(domainurl + "/Supplier/AddSupplier/");
    });
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    });
    $(".ListViewLoader").hide();
    var valueid = $(".anc-click").attr('data-id');
    $(".item-edit-name").click(function () {
        var idsupplier = $(this).attr('data-id');
        LoadSupplierDetails(idsupplier);
    });
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Expense/ExpensePartial");
        }, 200);
    });
    var table = $('#tblSupplier').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        //"scrollY": "290px",
        //"scrollCollapse": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $('#tblSupplier tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $("#srch-term").keyup(function () {
        $("#tblSupplier_filter input").val($("#srch-term").val());
        $("#tblSupplier_filter input").trigger('keyup');
    });
    $("#AddNewSupplier").click(function () {
        $(".addManufacturerMagnific").attr('href', domainurl + '/Supplier/AddSupplier');
        $(".addManufacturerMagnific").click();
    });
    $(".EditSupplier").click(function () {
        var idsupplier = $(this).attr('data-id');
        //$(".addManufacturerMagnific").attr('href', '/Supplier/AddSupplier/' + idsupplier);
        //$(".addManufacturerMagnific").click();
        OpenRightToLeftModal(domainurl + "/Supplier/AddSupplier?id=" + idsupplier);
    });

    $("#btnDownloadInvReport").click(function () {
        location.href = domainurl + "/Supplier/SupplierListPartial/?GetReport=true";
    });
    //
    //var SupplierDetails = function (valueid) {

    //    var url = "/Supplier/SupplierDetails";
    //    //var valueid = $(".customer-content-list").attr('idval');
    //    var param = {
    //        id: valueid
    //    }
    //    $.ajax({
    //        type: "POST",
    //        ajaxStart: $(".loader-div").show(),
    //        url: url,
    //        data: JSON.stringify(param),
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        cache: false,
    //        success: function (data) {
    //            LoadSupplierDetails
    //        },
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            console.log(errorThrown);
    //        }
    //    })
    //}
    $(".delete-supplier").click(function () {
        idsupplier = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure to delete supplier?", DeleteSupplier);
    });

    setTimeout(function () {
        $(".ListContents").slideDown();
    }, 500);

});

var DeleteSupplier = function () {
    var delitem = idsupplier;
    $.ajax({
        url: domainurl + "/Supplier/DeleteSupplier",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                LoadSupplier(true);
            }
        },

        error: function () {
        }
    });
}

