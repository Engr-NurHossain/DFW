//var equipmentId = null;
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
var inactiveProductService = function (equipmentId) {
    $.ajax({
        url: domainurl + "/Inventory/MakeEquipmentServiceInactive",
        data: { equipmentId: equipmentId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        parent.LoadEquipmentList();
    });
}
var activeProductService = function (equipmentId) {
    $.ajax({
        url: domainurl + "/Inventory/MakeEquipmentServiceActive",
        data: { equipmentId: equipmentId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        parent.LoadEquipmentList();
    });
}
var selectedDeleteId;
var DeleteProduct = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Inventory/DeleteProduct",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result == true) {
                EquipmentSearchKeyUp(1);
            }
            else {
                OpenErrorMessageNew("Error!", data.message, "");
            }
        },

        error: function () {
        }

    });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".ListViewLoader").hide();


    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Inventory/EquipmentsListPartial");
        }, 200);
    });
    setTimeout(function () {
        $(".ListContents").slideDown();
    }, 500);
    $(".stock-status-partial-view-div").load("Inventory/StockStatusPartialView/");
    $(".editEquipmentService").click(function () {
        var EditId = $(this).attr('idval');
        var TypeId = $(this).attr('data-id');
        if (TypeId == 1) {
            OpenRightToLeftLgModal(domainurl + "/Inventory/AddInventoryEquipment/?id=" + EditId);
        }
        else if (TypeId == 2) {
            OpenRightToLeftLgModal(domainurl + "/Inventory/AddNonInventory/?id=" + EditId);
        }
        else if (TypeId == 3) {
            OpenRightToLeftLgModal(domainurl + "/Inventory/AddService/?id=" + EditId);
        }
    });
    $(".product-edit-name").click(function () {
        var EditId = $(this).attr('idval');
        var TypeId = $(this).attr('data-id');
        if (TypeId == 1) {
            OpenRightToLeftLgModal(domainurl + "/Inventory/AddInventoryEquipment/?id=" + EditId);
        }
        else if (TypeId == 2) {
            OpenRightToLeftLgModal(domainurl + "/Inventory/AddNonInventory/?id=" + EditId);
        }
        else if (TypeId == 3) {
            OpenRightToLeftLgModal(domainurl + "/Inventory/AddService/?id=" + EditId);
        }
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
    //var equipmentId = $(".makeInactive").attr('data-id');
    $(".makeInactive").click(function () {
        var equipmentId = $(this).attr('data-id');
        inactiveProductService(equipmentId);
        //OpenSuccessMessage("Product/Service Deactive", "Are you sure to deactivate this product/service?", inactiveProductService);
    });
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
    $(".duplicateEquipment").click(function () {
        var EquipmentId = $(this).attr('idval');
        OpenRightToLeftLgModal("Inventory/DuplicateEquipment/?EquipmentId=" + EquipmentId);
    });
    $(".duplicateService").click(function () {
        var ServiceId = $(this).attr('idval');
        OpenRightToLeftLgModal("Inventory/DuplicateService/?ServiceId=" + ServiceId);
    });
    $(".duplicateNonInventory").click(function () {
        var ServiceId = $(this).attr('idval');
        OpenRightToLeftLgModal("Inventory/DuplicateNonInventory/?ServiceId=" + ServiceId);
    })
    $(".MakeInventoryProductActive").click(function () {
        var equipmentId = $(this).attr('data-id');
        activeProductService(equipmentId);
        //OpenSuccessMessage("Product/Service Active", "Are you sure to activate this product/service?", activeProductService(equipmentId));
    });
    $(".product-delete").click(function () {
        selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteProduct);
    })
});