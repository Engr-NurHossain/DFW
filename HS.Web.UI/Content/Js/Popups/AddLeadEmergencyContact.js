var SaveEmergencySetup = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Leads/AddEmergencyContact";
        var Param = JSON.stringify ({
            Id: $("#Id").val(),
            LeadCustomerID: $("#LeadCustomerID").val(),
            CrossSteet: $("#CrossSteet").val(),
            FirstName: $("#FirstName").val(),
            LastName: $("#LastName").val(),
            RelationShip: $("#RelationShip").val(),
            Phone: $("#Phone").val(),
            PhoneType: $("#PhoneType").val(),
            HasKey: $("#HasKey").val(),
            LeadId: $("#LeadId").val()
        });
        var PaymentParam = {
            Id: $("#Id").val(),
            RoutingNo: $("#RoutingNo").val(),
            AcountNo: $("#AcountNo").val(),
            BankAccountType: $("#BankAccountType").val(),
            AccountName: $("#AccountName").val(),
            CardType: $("#CardType").val(),
            CardNumber: $("#CardNumber").val(),
            CardExpireDate: $("#CardExpireDate").val(),
            CardSecurityCode: $("#CardSecurityCode").val()
        }
        
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: Param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                var LeadIdval = $("#LeadId").val();
                console.log("Lead Id Value: " + LeadId)
                setTimeout(function () {
                    LoadLeadsDetail(LeadIdval);
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
        console.log('btnSavandNex3')
        SaveEmergencySetup();
    });

});