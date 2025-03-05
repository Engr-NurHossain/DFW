var SyncWithNMC = function (syncid) {
    var url = domainurl + '/API/SyncNMCCustomer';
    var param = JSON.stringify({
        NMCRefId: syncid,
        CustomerId: CustomerLoadGuid,
        CustomerGuidId: CustomerLoadGuid
    })
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                OpenSuccessMessageNew("", data.message, function () { });
                $(".NMCConnectTab").click();
            }
            else {
                OpenErrorMessageNew("", data.message, function () {

                })

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var CustomerLoadDiv = ".third_party_api_tabs_" + CustomerLoadGuid;
$(document).ready(function () {

    if (NMCRefId != "") {
        var CustomerLoadDiv = ".third_party_api_tabs_" + CustomerLoadGuid;
        $(CustomerLoadDiv + " #NMCCustomerDetailsLoad").load("/API/NMCCustomerDetails/?NMCRefId=" + NMCRefId + "&CustomerId=" + CustomerLoadGuid);
    }
    $(".CreateCustomerNMC").click(function () {
        OpenTopToBottomModal(String.format("/API/AddNmcCustomer/?CustomerId={0}", CustomerLoadGuid));
    });
    $(".editNMCsettingsbtn").click(function () {
        console.log("clicked");
        OpenRightToLeftModal(domainurl + "/API/EditGlobalSettingsNMCSearchkeyandValue/")
    });
    $(".syncbtnNMC").click(function () {
        var syncid = $("#NMCSyncId").val();
        SyncWithNMC(syncid);
    })

});
