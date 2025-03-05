var SyncWithUCC = function (syncid) {
    var url = domainurl + '/API/SyncUCCCustomer';
    var param = JSON.stringify({
        UCCRefId: syncid,
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
                OpenSuccessMessageNew("", "", function () { });
                $(".UCCTab").click();
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
  
    if (UCCRefId != "") {
        var CustomerLoadDiv = ".third_party_api_tabs_" + CustomerLoadGuid;
        $(CustomerLoadDiv + " #UCCCustomerDetailsLoad").load("/API/UCCCustomerDetails/?UCCRefId=" + UCCRefId + "&CustomerId=" + CustomerLoadGuid);
    }
    $(".CreateCustomerUCC").click(function () {
        OpenTopToBottomModal(String.format("/API/AddUCCCustomer/?CustomerId={0}&Actions={1}", CustomerLoadGuid, CreateCustomer))
    });
    $(".edituccsettingsbtn").click(function () {
        console.log("clicked");
        OpenRightToLeftModal(domainurl + "/API/EditGlobalSettingsUCCSearchkeyandValue/")
    });
    $(".syncbtnUCC").click(function () {
        var syncid = $("#UCCSyncId").val();
        SyncWithUCC(syncid);
    })

});
