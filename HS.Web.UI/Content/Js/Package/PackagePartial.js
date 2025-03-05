
var LoadPackageEquipments = function (Pid, Lid) {
    $(".package-additional-features").load(domainurl + "/Leads/LoadLeadPackageEquipments/?PackageId=" + Pid + "&LeadId=" + Lid);
}
$(document).ready(function () {
    $("#PackageType").change(function () {
        if ($("#PackageType").val() != "-1") {
            $(this).removeClass('required');
        }
        var PackageId = parseInt($(this).val());
        LoadPackageEquipments(PackageId, LeadId);
    });
    $("#InstallType").change(function () {
        if ($("#InstallType").val() != "-1") {
            $(this).removeClass('required');
        }
    })
    $("#PackageSystemType").change(function () {
        if ($("#PackageSystemType").val() != "-1") {
            $(this).removeClass('required');
        }
    })
});