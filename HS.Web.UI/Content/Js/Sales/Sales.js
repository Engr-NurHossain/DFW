var DataTablePageSize = 50;
var deleteSales = $('.item-delete').attr('id');
var editSales = $('.item-edit').attr('data-id');
var NewSalesLoad = function (date, month, year) {
    alert("st");
    $("#AddSales").click();
    alert("end");
    //$("#NewSales").load("/Sales/AddSales/");
    $("#NewSales").load(domainurl + "/Sales/AddSales?id=0&customerid=" + CustomerGuid);
}

var table = $('#tblSales').DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});
var DeleteSales = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Sales/DeleteSalesAppointment",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) { 
            if (result) {
                    
                $("#SalesTab").load(domainurl + "/Sales/SalesPartial/?customerid=" + CustomerGuid);
            } else {

            }
        }, 
        error: function () {
        }

    }); 
    console.log("from customer delete" + customerId);  
}
      
$(document).ready(function () {
        
    $(".item-edit").click(function () {
        var idvalue = $(this).attr('data-id');
        $("#NewSales").load(domainurl + "/Sales/AddSales/?id=" + idvalue + "&customerId=" + customerId);
    });
    
})