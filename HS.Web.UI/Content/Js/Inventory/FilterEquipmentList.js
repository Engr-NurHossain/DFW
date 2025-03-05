var table = $("#tbleq_filter").DataTable({
    "ordering": false,
    searching: false, paging: false, info: false
})
var ChangeEquipStatus = function (dd, id, isActive) {
    if ($(dd).prop("checked")) {
        isActive = true;
    }
    $.ajax({
        url: domainurl + "/Inventory/ChangeEquipmentStatus",
        data: {
            id: id,
            isActive: isActive
        },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result == true) {


            }
        },

        error: function () {
        }
    });
}
$(document).ready(function () {
    var customerreportpopwinowwith = 600;
    var customerreportpopwinowheight = 510;
    var customerprintpopwinowwith = 920;
    var customerprintpopwinowheight = 600;
    $(".StatusToogle").bootstrapToggle({
        on: 'Active',
        off: 'Inactive'
    });
    $(".StatusToogle").unbind("onchange");
    if (Device.MobileGadget()) {
        customerreportpopwinowwith = window.innerWidth;
        customerreportpopwinowheight = window.innerHeight;
        customerprintpopwinowwith = window.innerWidth;
        customerprintpopwinowheight = window.innerHeight;
    }
    var idlist = [{ id: ".ExportEquipmentReport", type: 'iframe', width: customerreportpopwinowwith, height: customerreportpopwinowheight }];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#EquipmentReport").click(function () {
        var ActiveStatus = 1;
        if (typeof ($(".ActiveStatus").val()) != "undefined") {
            var ActiveStatus = $(".ActiveStatus").val();
        }
        var ActiveInactiveStatus = encodeURI($("#ActiveStatus").val());
        var EquipmentClass = 1;
        if (typeof ($(".EquipmentClass").val()) != "undefined") {
            EquipmentClass = $(".EquipmentClass").val();
        }
        var EquipmentCategory = -1;
        if (typeof ($(".EquipmentCategory").val()) != "undefined") {
            EquipmentCategory = $(".EquipmentCategory").val();
        }
        var searchtext = encodeURIComponent($(".eqpInvetory").val());
        location.href = domainurl + "/Reports/FilterEquipmentsListDownload/?ActiveStatus=" + ActiveStatus + "&ActiveInactiveStatus=" + ActiveInactiveStatus + "&EquipmentClass=" + EquipmentClass + "&EquipmentCategory=" + EquipmentCategory +/* "&StockStatus=" + StockStatus +*/ "&SearchText=" + searchtext;

    });
    $(".icon_sort_eq").click(function () {
        var orderval = $(this).attr('data-val');
        console.log(orderval)
        $("#SortingVal").val(orderval);
        InventorySearchKeyUp(pageno, orderval);
    });
});