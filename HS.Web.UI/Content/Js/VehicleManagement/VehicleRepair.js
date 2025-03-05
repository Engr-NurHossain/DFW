
var VehicleRepairSave = function () {
 
    var vehicleRepair = {};
    if ($('#InteriorClean').is(':checked')) {
        vehicleRepair.InteriorClean = true;
    }
    else {
        vehicleRepair.InteriorClean = false;
    }
    if ($('#ExteriorClean').is(':checked')) {
        vehicleRepair.ExteriorClean = true;
    }
    else {
        vehicleRepair.ExteriorClean = false;
    }

    if ($('#Vaccumed').is(':checked')) {
        vehicleRepair.Vaccumed = true;
    }
    else {
        vehicleRepair.Vaccumed = false;
    }

    if ($('#EquipmentOrganized').is(':checked')) {
        vehicleRepair.EquipmentOrganized = true;
    }
    else {
        vehicleRepair.EquipmentOrganized = false;
    }
    vehicleRepair.Id = $("#RepairId").val();
    vehicleRepair.UserId = $("#vehicle_userId").val();
    vehicleRepair.VehicleId = $("#VehiCleRepairId").val();
    vehicleRepair.StrDate = $("#RepairDate").val();
    vehicleRepair.Amount = $("#Amount").val();
    vehicleRepair.Note = $("#Note").val();
    vehicleRepair.Driver = $("#Driver").val();
    vehicleRepair.Mileage = $("#Mileage").val();
    vehicleRepair.Spent = $("#Spent").val();
    vehicleRepair.TireRotation = $("#TireRotation").val();
    vehicleRepair.Registration = $("#Registration").val();
    vehicleRepair.OildChange = $("#OildChange").val();
    vehicleRepair.TireDepth = $("#TireDepth").val();
  
   
    vehicleRepair.CreatedDate = $("#CreatedDate").val(); 
    $.ajax({

        type: "POST",
        url: domainurl + "/VehicleManagement/AddVehicleRepair",
        data: '{vehicleRepair: ' + JSON.stringify(vehicleRepair) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.result) {
                $("#vehicleId").val(response.Id);
                OpenSuccessMessageNew("Success!", response.message, function () {
                    RepairLogList($("#VehiCleRepairId").val());

                    $("#Right-To-Left-Modal-Body .close").click();
                });
            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
            //window.location.reload();
        }
    });
}
$(document).ready(function () {
    $('#btnSaveHrVehicleRepair').click(function () {
        if (CommonUiValidation()) {

            VehicleRepairSave();
        }
        else {
           
        }
    });
    var windowsHeight = window.innerHeight - 102;
    console.log(windowsHeight);
    $(".container_addfile").height(windowsHeight)
});