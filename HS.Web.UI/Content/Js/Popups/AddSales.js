var Savesales = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Sales/AddSales";
        
        var param = JSON.stringify({
            Id: $("#Id").val(),
            CustomerId: $("#CustomerIdVal").val(),
            AppointmentDate: SalesDatepicker.getDate(),
            AppointmentStartTime: $("#AppointmentStartTime").val(),
            AppointmentEndTime: $("#AppointmentEndTime").val(),
            CustomerList: $("#CustomerList").val(),
            CompanyId: $("#CompanyIdVal").val(),
            EmployeeId: $("#EmployeeId").val(),
            Notes: $("#Notes").val(),
        });

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $('.close').trigger('click');
                setTimeout(function () {
                    var cusId = $("#CustomerIdVal").val();
                    console.log("Inside Customer reload  " + cusId)
                    $("#SalesTab").load(domainurl + "/Sales/SalesPartial/?customerid=" + cusId);
                }, 600);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })


    }
}
$(document).ready(function () {
    
    $("#SaveSales").click(function () {
        
        console.log("clicked")
        Savesales();
        
    })
});