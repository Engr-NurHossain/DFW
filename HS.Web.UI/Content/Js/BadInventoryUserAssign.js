var ChangeCreatedByUser = function (id, changeid, installedBy, chkExist, chkIsNonCommissionable) {
    $.ajax({
        type: "POST",
        ajaxStart: $(".ticket-loader-div").show(),
        url: domainurl + "/Ticket/ChangeCreatedByUser",
        data: JSON.stringify({
            id: id,
            changeid: changeid,
            InstalledBy: installedBy,
            chkExist: chkExist,
            chkIsNonCommissionable: chkIsNonCommissionable
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                parent.ReloadTicket();
                parent.ClosePopup();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var CreatePOBadInventory = function (id) {
    var equipmentid = parent.$(".badinventory_move_" + id).attr('data-id');
    var customerid = parent.$(".badinventory_move_" + id).attr('data-cusid');
    var quantity = parent.$(".badinventory_move_" + id).attr('data-qty');
    var techid = parent.$("#AssignedTo").val();
    var TicketId = parent.$(".badinventory_move_" + id).attr('data-ticketid');
    var doquantity = parent.$(".badinventory_move_" + id).attr('data-doqty');
    var dohand = parent.$(".badinventory_move_" + id).attr('data-dohand');
    var name = parent.$(".badinventory_move_" + id).attr('data-name');
    var detail = parent.$(".badinventory_move_" + id).attr('data-dodetail');
    var tikintid = parent.$(".badinventory_move_" + id).attr('data-tikintid');
    var url = "/PurchaseOrder/AddDemandOrderTicket";
    var DetailList = [];
    var Qty = parseInt(0);
    var QtyOnHand = parseInt(0);
    if (doquantity != undefined || doquantity != null) {
        Qty = parseInt(doquantity);
    }
    if (dohand != undefined || dohand != null) {
        QtyOnHand = parseInt(dohand);
    }
    DetailList.push({
        EquipmentId: equipmentid,
        EquipName: name,
        EquipDetail: detail,
        Quantity: 1,
        RecieveQty: 0,
        CreatedDate: '1-1-2017',
    });
    var param = JSON.stringify({
        TechnicianId: parent.$("#AssignedTo").val(),
        PurchaseOrderDetail: DetailList,
        TicketId: tikintid
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
                parent.$(".AddInvoiceLoader").addClass('hidden');
                if (data.result) {
                    parent.OpenSuccessMessageNew("Success!", "DO successfully created.", function () {
                        CreateBadInventory(count);
                    });
                } else {
                    parent.OpenErrorMessageNew("Error!", data.message);
                    parent.ClosePopup();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                parent.OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
                parent.ClosePopup();
            }
        });
    }
}
var CreateBadInventory = function(id) { 
    var equipmentid = parent.$(".badinventory_move_" + id).attr('data-id');
    var customerid = parent.$(".badinventory_move_" + id).attr('data-cusid');
    var quantity = parent.$(".badinventory_move_" + id).attr('data-qty');
    var techid = parent.$("#AssignedTo").val();
    var TicketId = parent.$(".badinventory_move_" + id).attr('data-ticketid');
    var doquantity = parent.$(".badinventory_move_" + id).attr('data-doqty');
    var dohand = parent.$(".badinventory_move_" + id).attr('data-dohand');
    var name = parent.$(".badinventory_move_" + id).attr('data-name');
    var detail = parent.$(".badinventory_move_" + id).attr('data-dodetail');
    var tikintid = parent.$(".badinventory_move_" + id).attr('data-tikintid');
    var DetailList = [];
    var Qty = parseInt(0);
    var QtyOnHand = parseInt(0);
    var param = { EquipmentId: equipmentid, CustomerId: customerid, TechnicianId: techid, Quantity: 1, Description: $("#badinvdes").val(), Reason: $("#badinvreason").val() };
    var param1 = { TicketId: TicketId };
    var fparam = JSON.stringify({ 'eqreturn': param, 'Ticket': param1 })
    $.ajax({
        type: "POST",
        url: "/Ticket/EquipmentMoveBadInventory",
        data: fparam,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                //AddTicketConfirmation();
                parent.OpenTicketById(TicketIntId);
                parent.ClosePopup();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    $("#btn_reorder_eqp").click(function () {
        parent.OpenConfirmationMessageNew("Confirmation", "Do you want to tag it as a bad equipment?", function () {
            $(".userassignpopup").addClass('hidden');
            $(".badinvpopup").removeClass('hidden');
            $(".badinvpopup .btnPObadinv").removeClass('hidden');
        }, function () {
            $(".userassignpopup").addClass('hidden');
            $(".badinvpopup").removeClass('hidden');
            $(".badinvpopup .btnbadinv").removeClass('hidden');
        })
    })
    $("#btn_submit_data").click(function () {
        var changeid = $("#CustomerAppointmentEquipment_CreatedByUid").val();
        var installedBy = $("#InstalledBy").val();
        var chkExist = $(".chk_equipment_exist").prop('checked');
        var chkIsNonCommissionable = $(".chk_isnoncommissionable").prop('checked');
        ChangeCreatedByUser(appeqpid, changeid, installedBy, chkExist, chkIsNonCommissionable);
    })
    $("#btn_submit_data_PObad").click(function () {
        var qtyhand = parent.$(".badinventory_move_" + count).attr('data-dohand');
        if (CommonUiValidation()) {
            CreatePOBadInventory(count);
        }
    })
    $("#btn_submit_data_bad").click(function () {
        if (CommonUiValidation()) {
            CreateBadInventory(count);
        }
    })
})