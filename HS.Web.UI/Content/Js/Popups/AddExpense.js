var datePayment;
var SaveExpense = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Funding/AddExpense";

        var param = JSON.stringify({
            Id: $("#Id").val(),
            CustomerId: $("#CustomerIdVal").val(),
            CompanyId: $("#CompanyIdVal").val(),
            Amount: $("#Amount").val(),
            PaymentStatus: $("#PaymentStatus").val(),
            PaymentMethod: $("#PaymentMethod").val(),
            PaymentDate: datePayment.getDate(),
            Notes: $("#Notes").val(),
            UpdatedBy: $("#UpdatedBy").val
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
                    $("#FundingTab").load(domainurl + "/Funding/FundingPartial?customerid=" + cusId);
                }, 600);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })


    }
}
$(document).ready(function () {
    datePayment = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#PaymentDate')[0]
    });
    $("#SaveExpense").click(function () {

        console.log("clicked")
        SaveExpense();
    });

});