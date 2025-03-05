var valEquipOptional = [];
var ConfirmPackageChange = function () {
    var LeadPackageDetailList = [];
    var EquipmentListForWorkOrder = [];
    var ServiceList = [];
    var EquipmentPartialProductsList = [];
    var SmartPackageEquipmentServiceEquipmentList = [];

    if (CommonUiValidation()) {
        $('.Package-Include-Equipments').each(function () {
            var tmpIncludeEquipmentId = $(this).attr('id-val');
            var IncludeEquipmentId = $(this).attr('id-PackageEqpId');
            var PacId = $(this).attr('idval-PackageId');
            var IncludeEquipmentNumber = $(this).attr('id-EquipNum');
            EquipmentListForWorkOrder.push({
                SelectedEquipmentId: tmpIncludeEquipmentId,
                SelectedEquipmentPrice: '0.0',
                SelectedEquipmentIsFree: true,
                IsIncluded: true,
                IsOptionalEqp: false,
                IsDevice: false,
                NumOfEquipments: IncludeEquipmentNumber
            });
        });
        $('.Package-Include-Service').each(function () {
            var EquipmentId = $(this).attr('id-val');
            var PacId = $(this).attr('idval-PackageId');
            var SmartPrice = $(this).attr('service-price');
            ServiceList.push({
                EquipmentId: EquipmentId,
                PackageId: PacId,
                MonthlyRate: SmartPrice,
                Total: SmartPrice
            });
        });
        $('.device-equipments').each(function () {
            if ($(this).prop('checked') == true) {
                if (deviceEquipmentList.length <= CurrentPackageLimit) {
                    var deviceEquipmentId = $(this).attr('idval');
                    var deviceEquipmentPrice = $(this).attr('id-eqpPrice');
                    var deviceEquipmentIsFree = $(this).attr('id-eqpIsFree');
                    var IncludeEquipmentId = $(this).attr('id-PackageEqpId');
                    var PacId = $(this).attr('idval-PackageId');
                    var DeviceEptNo = $(this).attr('id-EptNo');
                    EquipmentListForWorkOrder.push({
                        SelectedEquipmentId: deviceEquipmentId,
                        SelectedEquipmentPrice: deviceEquipmentPrice,
                        SelectedEquipmentIsFree: deviceEquipmentIsFree,
                        IsIncluded: false,
                        IsOptionalEqp: false,
                        IsDevice: true,
                        NumOfEquipments: DeviceEptNo
                    });
                }
            }
        });
        $(".text-numoptional").each(function () {
            valEquipOptional.push($(this).val());
        });
        var count = 0;
        $('.optional-equipments').each(function () {
            if ($(this).prop('checked') == true) {
                var optionalEquipmentId = $(this).attr('idval');
                var optionalEquipmentPrice = $(this).attr('id-eqpPrice');
                var optionalEquipmentIsFree = $(this).attr('id-eqpIsFree');
                var IncludeEquipmentId = $(this).attr('id-PackageEqpId');
                var PacId = $(this).attr('idval-PackageId');
                EquipmentListForWorkOrder.push({
                    SelectedEquipmentId: optionalEquipmentId,
                    SelectedEquipmentPrice: optionalEquipmentPrice,
                    SelectedEquipmentIsFree: optionalEquipmentIsFree,
                    IsIncluded: false,
                    IsOptionalEqp: true,
                    IsDevice: false,
                    NumOfEquipments: valEquipOptional[count]
                });
            }
            count++;
        });

        $(".service-equipments:checked").each(function () {
            var serviceEqpId = $(this).attr('idint');
            var EquipmentId = $(this).attr('idval');
            var Quantity = $(".ServiceEqpQuantity_" + serviceEqpId).val();
            var Price = $(".EqpPrice_" + serviceEqpId).val();
            var SmartPackageEquipmentServiceId = $(this).attr('SmartPackageEquipmentServiceId');
            SmartPackageEquipmentServiceEquipmentList.push({
                SmartPackageEquipmentServiceId: SmartPackageEquipmentServiceId,
                EquipmentId: EquipmentId,
                Quantity: Quantity,
                EquipmentPrice: Price
            });

        });

        var url = domainurl + "/Ticket/ConfirmPackageChange";
        var PackageParam = [];
        PackageParam = {
            PackageId: $("#PackageCustomermodel_PackageId").val(),
            LeadId: TicketCustomerIntId,
            SmartPackageEquipmentServiceEquipmentList: SmartPackageEquipmentServiceEquipmentList,
            EquipmentList: EquipmentListForWorkOrder,
            ServiceList: ServiceList,
            TicketId: TicketId
        };
        var setupParam = JSON.stringify({
            'ModelAddleadPackage': PackageParam
        });
        console.log(JSON.parse(setupParam));
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: setupParam,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                OpenSuccessMessageNew("Success!","", function () {
                    ReloadTicket();
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}
$(document).ready(function () {
    $("#PackageCustomermodel_PackageId").change(function () {
        var Pid = $(this).val();
        if (Pid != "-1")
        {
            $(".package-additional-features").load(domainurl + "/SmartLeads/LoadSmartLeadPackageEquipments/?PackageId=" + Pid + "&LeadId=" + TicketCustomerIntId);
            $("#ConfirmPackageChange").removeClass("hidden");
        }
        else {
            $(".package-additional-features").html("");
            $("#ConfirmPackageChange").addClass("hidden");
        }
    });
    $("#ConfirmPackageChange").click(function () {
        ConfirmPackageChange();
    });
});