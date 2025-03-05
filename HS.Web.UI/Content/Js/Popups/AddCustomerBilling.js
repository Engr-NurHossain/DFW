var SaveBilling = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Billing/AddBilling";
        
        var param = {
            Id: $("#Id").val(),
            BillNo: $("#BillNo").val(),
            CompanyId: $("#CompanyId").val(),
            CustomerId: $("#CustomerIdVal").val(),
            Type: $("#Type").val(),
            Amount: $("#Amount").val(),
            PaymentMethod: $("#PaymentMethod").val(),
            PaymentStatus: $("#PaymentStatus").val(),
            PaymentDate: paymentdate.getDate(),
            PaymentDueDate: duedate.getDate(),
            BillCycle: $("#BillCycle").val(),
            Notes: $("#Note").val()
        };
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $('.close').trigger('click');
                setTimeout(function () {
                    var cusId = $("#CustomerIdVal").val();
                    $("#BillingTab").load(domainurl + "/Billing/BillingPartial/?customerid=" + cusId);
                }, 600);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })


    }
}
$(document).ready(function () {
    
    $("#BillSaveButton").click(function () {
        SaveBilling();
    })
    
});