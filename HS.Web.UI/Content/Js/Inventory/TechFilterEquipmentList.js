$(document).ready(function () {
    var customerreportpopwinowwith = 600;
    var customerreportpopwinowheight = 510;
    var customerprintpopwinowwith = 920;
    var customerprintpopwinowheight = 600;

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
        var selectedID = [];
        var checkboxs = $('.Export_excel_equipment');
        for (var i = 0; i < checkboxs.length; i++) {
            selectedID.push(parseInt($(checkboxs[i]).attr('data-id')));
        }
        var ColumnName = "";
        $('.eq').each(function () {
            if ($(this).attr('data-info') != "" && $(this).attr('data-info') != undefined && $(this).attr('data-info') != null) {
                ColumnName += $(this).attr('data-info').trim() + "," + $(this).text().trim() + "-";
            }
        });
        $(".ExportEquipmentReport").attr('href', domainurl + "/Reports/ExportConfirm/?ColumnName=" + ColumnName + "&Ids=" + selectedID + "&ReportFor=Equipment");
        $(".ExportEquipmentReport").click();
    });
    $(".icon_sort_eq").click(function () {
        var orderval = $(this).attr('data-val');
        InventorySearchKeyUp1(pageno, orderval);
    });
    $("#ChkSelectAllEquipment").change(function () {
        if ($(this).prop("checked") == true) {
            $(".ChkSelectEquipment").prop("checked", true);
        }
        else {
            $(".ChkSelectEquipment").prop("checked", false);
        }
    });
    $(".ChkSelectEquipment").change(function () {
        if ($(this).prop("checked") == false) {
            $("#ChkSelectAllEquipment").prop("checked", false);
        }
    });
});