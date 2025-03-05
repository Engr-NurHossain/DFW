
/*
Id	Name
1	Inventory
2	NonInventory
3	Service
*/

$(document).ready(function () {

    $(".add-equipments").click(function () {
        EquipmentClassId = 1;
        $("#Right-To-Left-big-Modal-Body .close").click();
        setTimeout(function () {
            OpenTopToBottomModal(domainurl + "/Inventory/AddEquepment");
        }, 200);
        //$(".AddEquipmentContainer").addClass("hidden");
        //$(".EquipmentClass").html("<span>Inventory</span>");
        //$(".addequipment_div").removeClass("hidden");
        
    }); 
    /*$(".add-nonInventory").click(function () {
        EquipmentClassId = 3;
        $(".AddEquipmentContainer").addClass("hidden");
        $(".EquipmentClass").html("<span>Non-Inventory</span>");
        $(".addequipment_div").removeClass("hidden");
    });*/

    $(".add-services").click(function () {
        EquipmentClassId = 2;
        //$(".AddEquipmentContainer").addClass("hidden");
        //$(".EquipmentClass").html("<span>Service</span>");
        //$(".addequipment_div").removeClass("hidden");
        $("#Right-To-Left-big-Modal-Body .close").click();
        setTimeout(function () {
            OpenTopToBottomModal(domainurl + "/Inventory/AddService");
        }, 200);
    });
    $(".change_type").click(function () {
        $(".AddEquipmentContainer").removeClass("hidden");
        $(".addequipment_div").addClass("hidden");
    });
})