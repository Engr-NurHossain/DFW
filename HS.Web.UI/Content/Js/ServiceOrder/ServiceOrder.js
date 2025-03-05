var deleteService = $('.item-delete').attr('id');
var DataTablePageSize = 50;
var NewServiceLoad = function () {
    //$("#NewService").load("/ServiceOrder/AddServiceOrder?id=0&customerid=" + customerId);

}
//var ServiceCalendarLoad = function (date, month, year) {
//    //$("#NewService").html("");

//    OpenRightToLeftModal("/ServiceOrder/AddServiceOrder?id=0&customerid=" + customerId + "&Date=" + date + "&Month=" + month + "&Year=" + year)
//    //$("#NewService").load("/ServiceOrder/AddServiceOrder?id=0&customerid=" + customerId + "&Date=" + date + "&Month=" + month + "&Year=" + year);
        
//}
//var table = $('#tblinfo').DataTable({
//    "pageLength": DataTablePageSize,
//    "destroy": true
//});
var DeleteService = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/ServiceOrder/DeleteServiceAppointment",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {

            if (result) {
                console.log("ServiceOrder.js 2 inside Delete service.")
                $("#ServiceOrderTab").load(domainurl + "/ServiceOrder/ServiceOrderPartial/?customerid=" + CustomerGuid);
            }
            else {

            }
        },

        error: function () {
        }

    });
    //var customerId = deleteService;
    console.log("from customer delete" + customerId);
}

var table = $('#tblService').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    },
    "order": [[0, "desc"]]
});
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#BackServiceOrder").hide();
    $(".item-edit").click(function () {
        var idvalue = $(this).attr('data-id');
        OpenRightToLeftModal(domainurl + "/ServiceOrder/AddServiceOrder/?id=" + idvalue + "&customerId=" + CustomerLoadId);
        //$("#NewService").load("/ServiceOrder/AddServiceOrder/?id=" + idvalue + "&customerId=" + customerId);
    });
    $(".AddServiceOrder").click(function () {
        //$(".ServiceAppoinmentCalender").load("/Customer/CustomerAppoinments/?Id=" + customerId + "&Parent=ServiceAppoinmentCalender");
        OpenRightToLeftModal(domainurl + "/ServiceOrder/AddServiceOrder?id=0&customerid=" + CustomerLoadId)
        //$(".Service-table").hide();
        //ServiceCalendarLoad();
        //$(".Left-Modal-Open").click()
        //$("#BackServiceOrder").show();
        //$("#AddServiceOrder").hide();
    });
    $("#BackServiceOrder").click(function () {
        console.log("ServiceOrder.js");
        $("#ServiceOrderTab").load(domainurl + "/ServiceOrder/ServiceOrderPartial?customerid=" + CustomerLoadGuid);
    });
    $(".item-delete").click(function () {
        selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteService);
    });
    $(".toptobottomserviceorder").click(function () {
        var CustomerIdForProductInstallation = $(this).attr('idval-cusId');
        var AppointmentIdForProductInstallation = $(this).attr('idval');
        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + AppointmentIdForProductInstallation + "&CustomerId=" + CustomerIdForProductInstallation);
    });  
    $(".topToBottomServiceOrderSpan").click(function () {
        var CustomerIdForProductInstallation = $(this).attr('idval-cusId');
        var AppointmentIdForProductInstallation = $(this).attr('idval');
        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + AppointmentIdForProductInstallation + "&CustomerId=" + CustomerIdForProductInstallation);
    });
        
})
