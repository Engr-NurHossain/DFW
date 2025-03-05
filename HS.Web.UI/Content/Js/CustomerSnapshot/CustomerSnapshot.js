var DataTablePageSize = 50;
//var table = $('#tblSnapshot').DataTable({
//    "pageLength": DataTablePageSize,
//    "destroy": true
//});

//$(document).ready(function () {
//    $('#tblSnapshot tbody').on('click', 'tr', function () {
//        if ($(this).hasClass('selected')) {
//            $(this).removeClass('selected');
//        }
//        else {
//            table.$('tr.selected').removeClass('selected');
//            $(this).addClass('selected');
//        }
//    });
//});
$(document).ready(function () {
    $(".appoinmenttype-event").click(function () {
        var typeid = $(this).attr('id');
        if (typeid == "Sales") {
            console.log(typeid+" From Slaes");
            parent.OpenSalesTab(domainurl + "/Sales/SalesPartial/");
        }
        else if (typeid == "ServiceOrder") {
            console.log(typeid+"From ServiceOrder");
            parent.OpenServiceOrderTab(domainurl + "/ServiceOrder/ServiceOrderPartial/");
        }
        else if (typeid == "WorkOrder") {
            console.log(typeid + "From WorkOrder");
            parent.OpenWorkOrderTab(domainurl + "/WorkOrder/WorkOrderPartial/");
        }
        
    });
});