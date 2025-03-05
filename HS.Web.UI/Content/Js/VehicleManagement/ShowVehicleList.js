var RepairLogList = function (item) {
    $("#repairTable").show();
    OpenTopToBottomModal(domainurl + "/VehicleManagement/VehicleRepairList?VehicleId=" + item)
}
var MilageLogList = function (item) {
    $("#repairTable").show();
    OpenTopToBottomModal(domainurl + "/VehicleManagement/VehicleMilageList?VehicleId=" + item)
}

var AddVehicleRepairLog = function (vehicleId) {
    OpenTopToBottomModal(domainurl + '/VehicleManagement/AddRepairLog/?VehicleId=' + vehicleId)
}
var AddMilageLog = function (vehicleId) {
    OpenTopToBottomModal(domainurl + '/VehicleManagement/AddMileageData/?VehicleId=' + vehicleId)
}
var DeleteVehicle = function (id) {
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this Vehicle?", function () {
        DeleteVehicleById(id)
    });
}
var DeleteVehicleById = function (id) {
    $.ajax({
        url: domainurl + "/VehicleManagement/DeleteVehicle",
        data: { Id: id },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Vehicle deleted successfully.");
                LoadVehicleMgmtTab(true);
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}
var LoadVehicleListByOrder = function (order) {
    if (typeof (order) == "undefined") {
        order = "";
    }
    $("#vehicle_List").html(LoaderDom);
    $("#vehicle_List").load(domainurl + "/vehiclemanagement/ShowVehilceListByOrder/?OrderBy=" + order);
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide(); 
    LoadVehicleListByOrder();
});