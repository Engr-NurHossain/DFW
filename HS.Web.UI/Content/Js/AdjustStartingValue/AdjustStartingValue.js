var adjustdate;

var SaveAdjustStartingValue = function () {
    var url = domainurl + "/Inventory/AdjustStartingValue/";
    var param = JSON.stringify({
        
        EuipmentIntId: $("#EuipmentIntId").val(),
        InventoryIntId: $("#InventoryIntId").val(),
        EquipmentId: $("#EquipmentId").val(),
        CompanyId: $("#CompanyId").val(),
        EquipmentCost: $("#EquipmentCost").val(),
        InventoryQuantity: $("#InventoryQuantity").val(),
        InventoryCost: $("#InventoryCost").val(),
        EquipmentAsOfDate: $("#EquipmentAsOfDate").val(),

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
            OpenRightToLeftModal();
            LoadEquipmentList();
        },
    });
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        /*parent.$(".modal-body").html('');*/
    })
    adjustdate = new Pikaday({ format: 'MM/DD/YYYY', field: $('#EquipmentAsOfDate')[0] });
   
    $("#adjustStartingValue").click(function () {
        SaveAdjustStartingValue();
    });
});
