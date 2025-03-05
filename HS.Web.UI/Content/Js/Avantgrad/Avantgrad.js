var SyncWithAvantgrad = function (syncid) {
    var url = domainurl + '/API/SyncAvantgradCustomer';
    var param = JSON.stringify({
        AvantgradRefId: syncid,
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
            console.log("Avantgrad");
            if (data.result == true) {
                OpenSuccessMessageNew("", data.message, function () { });
                $(".AvantgradTab").click();
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

    if (AvantgradRefId != "") {
        var CustomerLoadDiv = ".third_party_api_tabs_" + CustomerLoadGuid;
        $(CustomerLoadDiv + " #AvantgradCustomerDetailsLoad").load("/API/AvantgradCustomerDetails/?AvantgradRefId=" + AvantgradRefId + "&CustomerId=" + CustomerLoadGuid);
    }
    $(".CreateCustomerAvantgrad").click(function () {
        OpenTopToBottomModal(String.format("/API/AddAvantgradCustomer/?CustomerId={0}", CustomerLoadGuid));
    });
    $(".editAvantgradsettingsbtn").click(function () {
        console.log("clicked");
        OpenRightToLeftModal(domainurl + "/API/EditGlobalSettingsAvantgradSearchkeyandValue/")
    });
    $(".syncbtnAvantgrad").click(function () {
        var syncid = $("#AvantgradSyncId").val();
        console.log(syncid);
        SyncWithAvantgrad(syncid);
    })

});
