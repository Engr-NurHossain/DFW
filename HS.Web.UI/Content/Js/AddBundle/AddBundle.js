
        $("#saveInventoryProduct").click(function () {
            console.log("e kemon bichar!")
        });

$(".custom-span").click(function () {
    parent.$(".add-bundle").hide();
    parent.$(".add-equipment-div").show();
});

$(document).ready(function () {
    $(".header-image").click(function () {
        parent.$(".add-bundle").hide();
        parent.$(".add-equipment-background").hide();
    });
});
