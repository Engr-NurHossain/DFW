var saveService = function () {
    var url = domainurl + "/Inventory/AddService/";
    var param = JSON.stringify({
        id: $("#id").val(),
        name: $("#Name").val(),
        sku: $("#sku").val(),
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

    $(".sales-info-div").hide();
    //var SupplierCost = $("#SupplierCost").val(),
    //if(){$('.checkbox-sales').attr('checked', true);}


    $('.checkbox-sales').click(function () {
        if ($(this).prop("checked") == true) {
            $(".sales-info-div").show();
        }
        else if ($(this).prop("checked") == false) {
            $(".sales-info-div").hide();
        }
    });

    $(".custom-span").click(function () {
        parent.$(".show-service-add-div").hide();
        parent.$(".container").show();
    });

    $("#saveService").click(function () {

        if (CommonUiValidation()) {
            if ($("#name").val() != "" || $("#Cost").val() > 0) {
                var SalesPrice = parseInt($("#Retail").val());
                var Cost = parseInt($("#SupplierCost").val());
                if (SalesPrice < Cost) {
                    OpenErrorMessageNew("Error!", "Cost cannot more than Retail Price")
                }
                else {
                    saveService();
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