$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#ShowEquipmentList").load("/API/AlarmEquipmentList?CustomerId=" + CustomerId);
})
