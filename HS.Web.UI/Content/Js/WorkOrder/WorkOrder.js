var DataTablePageSize = 50;
var NewWorkOrderLoad = function (date, month, year) {
    alert("st");
    $("#AddSales").click();
    alert("end");
    //$("#NewSales").load("/Sales/AddSales/");
    $("#NewWorkOrder").load(domainurl + "/WorkOrder/AddWorkOrder?id=0&customerid=" + CustomerGuid);
}
var table = $('#tblWorkorder').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});

$(document).ready(function () {
    $('#tblWorkorder tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $(".toptobottomModalworkorder").click(function () {
        var CustomerIdForProductInstallation = $(this).attr('id');
        var AppointmentIdForProductInstallation = $(this).attr('idval');
        OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder/?AppointmentId=" + AppointmentIdForProductInstallation + "&CustomerId=" + CustomerIdForProductInstallation);
    })
    
});