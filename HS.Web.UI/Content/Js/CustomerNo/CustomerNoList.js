var DataTablePageSize = 50;
$(document).ready(function () {
    $(".ListViewLoader").hide();
    $('.toggle-demo').bootstrapToggle();
    var tableCustomerList = $('#tblCustomerNoList').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        //"scrollY": "480px",
        //"scrollCollapse": true,,
        "language": {
            "emptyTable": "No data available"
        }
    });
    //$('#tblCustomerNoList tbody').on('click', 'tr', function () {

    //    if ($(this).hasClass('selected')) {
    //        $(this).removeClass('selected');
    //    }
    //    else {
    //        table.$('tr.selected').removeClass('selected');
    //        $(this).addClass('selected');
    //    }
    //});
});
