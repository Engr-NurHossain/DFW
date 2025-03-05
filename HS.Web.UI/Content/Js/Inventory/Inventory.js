var UserFileUploadjqXHRData;
var LoadEquipmentList = function () {
    $(".ListContents").hide();
    $(".ListViewLoader").show();
    //$(".ListContents").load(domainurl + "/Inventory/EquipmentsListPartial");
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");
    $(".ListViewLoader").show();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Inventory/EquipmentsListPartial");
        }, 200);
    });
    //$(".ListContents").load("/Inventory/EquipmentsListPartial");

    $("#LoadServiceProduct").click(function () {
        LoadServiceProduct(false);
    });
    $("#LoadProductCategory").click(function () {
        window.location.href = domainurl + "/Customer/ProductCategoryPartial";
        //LoadProductCategory(false);
    });
    $("#LoadProductClass").click(function () {
        LoadProductClass(false);
    });
    $("#load_equipments_import").click(function () {
        OpenRightToLeftModal("/File/AddEquipmentsFile");
    });
    
})
