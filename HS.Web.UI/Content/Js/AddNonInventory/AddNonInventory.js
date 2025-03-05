var saveProduct = function () {
    var url = domainurl + "/Inventory/AddNonInventory/";
    var param = JSON.stringify({
        id: $("#id").val(),
        name: $("#Name").val(),
        sku: $("#SKU").val(),
        Comments: $("#Comments").val(),
        EquipmentTypeId: $("#EquipmentTypeId").val(),
        Cost: $("#Cost").val(),
        ManufacturerId: $("#ManufacturerId").val(),
        SupplierCost: $("#SupplierCost").val(),
        SupplierId: $("#SupplierId").val(),
        CreatedDate: $("#CreatedDate").val(),
        CompanyId: $("#CompanyId").val(),
        EquipmentId: $("#EquipmentId").val(),
        EquipmentClassId: $("#EquipmentClassId").val(),
        IsActive: $("#IsActive").val(),
        Retail: $("#Retail").val()
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
    $(".custom-span").click(function () {
        parent.$(".show-non-inventory-div").hide();
        parent.$(".container").show();
    });

    $("#saveProduct").click(function () {

        if (CommonUiValidation()) {
            if ($("#name").val() != "" || $("#Cost").val() > 0 || $("#SupplierCost").val() > 0 || $("#QtyOnHand").val() > 0) {
                var SalesPrice = parseInt($("#Retail").val());
                var Cost = parseInt($("#SupplierCost").val());
                if (SalesPrice < Cost) {
                    OpenErrorMessageNew("Error!", "Cost cannot more than Retail Price")
                }
                else {
                    saveProduct();
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