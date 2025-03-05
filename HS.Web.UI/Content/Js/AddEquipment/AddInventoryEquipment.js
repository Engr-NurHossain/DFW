var Asofdatepicker;
var saveNonInventory = function () {
    var AsOfDateVal = Asofdatepicker.getDate();
    var url = domainurl + "/Inventory/AddInventoryEquipment/";
    var param = JSON.stringify({
        id: $("#Id").val(),
        name: $("#Name").val(),
        sku: $("#Sku").val(),
        EquipmentTypeId: $("#EquipmentTypeId").val(),
        AsOfDate: AsOfDateVal,
        reorderpoint: $("#reorderpoint").val(),
        Cost: $("#Cost").val(),
        ManufacturerId: $("#ManufacturerId").val(),
        SupplierCost: $("#SupplierCost").val(),
        SupplierId: $("#SupplierId").val(),
        CreatedDate: $("#CreatedDate").val(),
        CompanyId: $("#CompanyId").val(),
        EquipmentId: $("#EquipmentId").val(),
        EquipmentClassId: $("#EquipmentClassId").val(),
        IsActive: $("#IsActive").val(),
        Retail: $("#Retail").val(),
        QtyOnHand: $("#QtyOnHand").val(),
        Comments: $("#Comments").val()
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
            //OpenRightToLeftLgModal();
            //parent.LoadEquipmentList();
            window.location.reload();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    $('.add_eqipm_inner').height(window.innerHeight - 103);
    Asofdatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#AsOfDate')[0] });
    $(".custom-span").click(function () {
        parent.$(".show-equipment-add-div").hide();
        parent.$(".container").show();
    });
    $("#saveProduct").click(function () {

        if (CommonUiValidation()) {
            if ($("#Name").val() != "" || $("#Cost").val() > 0 || $("#SupplierCost").val() > 0 || $("#QtyOnHand").val() > 0) {
                var SalesPrice = parseInt($("#Retail").val());
                var Cost = parseInt($("#SupplierCost").val());
                if (SalesPrice < Cost) {
                    OpenErrorMessageNew("Error!", "Cost cannot more than Retail Price")
                }
                else {
                    saveNonInventory();
                }
            }
            else {
                OpenErrorMessageNew("Error!", "Please Input Required Fields!");
                $(".required-field").css("border", "1px solid red");
                //$(".custom-span").show();
            }
        }

    });
});
$(window).resize(function () {
    $('.add_eqipm_inner').height(window.innerHeight - 103);
});