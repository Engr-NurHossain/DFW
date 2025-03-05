var SavePaymentSetup = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Leads/AddPaymentInfo";
        
        var Param = JSON.stringify({
            Id: $("#Id").val(),
            RoutingNo: $("#RoutingNo").val(),
            AcountNo: $("#AcountNo").val(),
            BankAccountType: $("#BankAccountType").val(),
            AccountName: $("#AccountName").val(),
            CardType: $("#CardType").val(),
            CardNumber: $("#CardNumber").val(),
            CardExpireDate: $("#CardExpireDate").val(),
            CardSecurityCode: $("#CardSecurityCode").val()
        });
        
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: Param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                setTimeout(function () {
                    //LoadLeadsDetail(LeadIdval);
                }, 600);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })


    }
}
$(document).ready(function () {
    $("#btnSavandNex").click(function () {
        console.log('btnSavandNex1')
        SavePaymentSetup();
    });
});