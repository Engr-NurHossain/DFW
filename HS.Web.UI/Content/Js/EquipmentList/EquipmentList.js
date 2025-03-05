var DataTablePageSize = 50;
var equipmentId = null;
var ClosePopup = function () {
    $.magnificPopup.close();
}
var LoadNewEquipment = function () {
    OpenRightToLeftLgModal("Inventory/AddEquipmentServiceBundleView");
}
var OpenAddEquipment = function () {
    EquipmentClassId = 1;
    $("#Right-To-Left-big-Modal-Body .close").click();
    setTimeout(function () {
        OpenTopToBottomModal(domainurl + "/Inventory/AddEquepment");
    }, 200);
}
var OpenAddService = function () {
    EquipmentClassId = 2;
    $("#Right-To-Left-big-Modal-Body .close").click();
    setTimeout(function () {
        OpenTopToBottomModal(domainurl + "/Inventory/AddService");
    }, 200);
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    });
    $(".ListViewLoader").hide();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Inventory/EquipmentsListPartial");
        }, 200);
    });
    var table = $('#tblVideo').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        //"scrollY": "290px",
        //"scrollCollapse": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $('#tblVideo tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    //$("#srch-term").keyup(function () {
    //    $("#tblVideo_filter input").val($("#srch-term").val());
    //    $("#tblVideo_filter input").trigger('keyup');
    //});
    //setTimeout(function () {
    //    $(".ListContents").slideDown();
    //}, 500);
    $("#AddNewEquipment").click(function () {
        OpenAddEquipment();
    });
    $("#AddNewService").click(function () {
        OpenAddService();
    });
    $(".deleteInventory").click(function () {
        var equipmentIdGuid = $(this).attr('idval');
        var equipmentIdInt = $(this).attr('data-id');
        $.ajax({
            url: domainurl + "/Inventory/DeleteInventory",
            data: { id: equipmentIdGuid, deleteId: equipmentIdInt },
            type: "Post",
            dataType: "Json"
        }).done(function () {
            parent.LoadEquipmentList();
        });
    });
    var equipmentId = $(".makeInactive").attr('data-id');
    $(".makeInactive").click(function () {
        var equipmentId = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure to deactivate this product/service?", inactiveProductService);
    });
    var inactiveProductService = function () {
        $.ajax({
            url: domainurl + "/Inventory/MakeEquipmentServiceInactive",
            data: { equipmentId: equipmentId },
            type: "Post",
            dataType: "Json"
        }).done(function () {
            parent.LoadEquipmentList();
        });
    }
    $(".adjustQuantity").click(function () {
        var equipmentId = $(this).attr('idval');
        var companyId = $(this).attr('data-id');
        OpenRightToLeftModal("Inventory/AdjustProductServiceQuantity/?equipmentId=" + equipmentId + "&companyId=" + companyId);
    });
    $(".adjustStartingValue").click(function () {
        var equipmentId = $(this).attr('idval');
        var companyId = $(this).attr('data-id');

        OpenRightToLeftModal("Inventory/AdjustStartingValue/?equipmentId=" + equipmentId + "&companyId=" + companyId);
    });
    //$(".stock-status-partial-view-div").load("Inventory/StockStatusPartialView/");
    $(".btn-reset-filter").click(function () {
        $("#ActiveStatus").val("-1");
        $("#EquipmentClass").val("-1");
        $("#EquipmentCategory").val("-1");
        $("#StockStatus").val("-1");
        function myFunction() {
            setTimeout(function () {
                $(".btn-apply-filter").click();
            }, 1500);
        }
    })
});