var SaveProductServiceQuantity = function () {
    var url = domainurl + "/Inventory/AdjustProductServiceQuantity/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        InventoryId: $("#InventoryId").val(),
        EquipmentId: $("#EquipmentId").val(),
        CompanyId: $("#CompanyId").val(),
        SupplierCost: $("#SupplierCost").val(),
        Cost: $("#Cost").val(),
        Retail: $("#Retail").val(),
        CreatedDate: $("#CreatedDate").val(),
        CreatedBy: $("#CreatedBy").val(),
        Quantity: $("#Quantity").val()
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
    $(".img-container").click(function () {
        parent.$(".adjust-quantity").hide();
        parent.$(".adjust-quantity-background").hide();
    });

    $(".new-qty-input").focusout(function () {
        var newQuantity = parseInt($(".new-qty-input").val());
        var oldQuantity = parseInt($(".old-quantity").text());
        var changeQuantity = newQuantity - oldQuantity;
        $(".chnage-quantity").text(changeQuantity);
    });

    $(".save-btn-div").click(function () {
        SaveProductServiceQuantity();
    });

});