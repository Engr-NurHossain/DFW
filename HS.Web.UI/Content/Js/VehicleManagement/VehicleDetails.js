var AddVehicleRepair = function () {
    OpenRightToLeftModal(domainurl + "/VehicleManagement/AddVehicleRepair/?RepairId=");
}
//var EditVehicle = function (id) {
//    OpenRightToLeftModal('/VehicleManagement/AddNewVehicle?Id='+id);
//}

var SaveVehicle = function () {
    var vehicle = {};
    vehicle.Id = $("#vehicleId").val();
    vehicle.VehicleId = $("#vehicleGuidId").val();
    vehicle.VehicleNo = $("#VehicleNO").val();
    vehicle.UserId = $("#vehicle_userId").val();
    vehicle.VIN = $("#VIN").val();
    vehicle.LicenseNO = $("#LicenseNO").val();
    vehicle.Year = $("#Year").val();
    vehicle.Make = $("#Make").val();
    vehicle.Model = $("#Model").val();
    vehicle.MillageData = $("#MillageData").val();
    vehicle.ExpirationTag = $("#ExpirationTag").val();
    vehicle.TollTag = $("#TollTag").val();
    vehicle.QuickBookNo = $("#QuickBookNo").val();
    $.ajax({

        type: "POST",
        url: domainurl + "/VehicleManagement/SaveVehicle",
        data: '{vehicle: ' + JSON.stringify(vehicle) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.result) {
                $("#vehicleId").val(response.Id);
                OpenSuccessMessageNew("Success!", response.message, function () {
                    $("#Right-To-Left-Modal-Body .close").click();
                    LoadVehicleMgmtTab(true)
                })


            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
            //window.location.reload();
        }
    });
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $('#btnSaveHrVehicle').click(function () {
        if (CommonUiValidation()) { 
            SaveVehicle();
        }
        else {

        }
    });
    var windowsHeight = window.innerHeight -102;
    console.log(windowsHeight);
    $(".add_followup_reminder_container").height(windowsHeight)

});