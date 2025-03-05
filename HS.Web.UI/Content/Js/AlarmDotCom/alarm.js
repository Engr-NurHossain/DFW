var SyncWithAlarm = function (syncid) {

    var url = domainurl + '/API/SyncAlarmCustomer';
    
    var param = JSON.stringify({
        CustomerId: syncid,
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
                OpenSuccessMessageNew("", data.message, function () {
                    OpenThirdPartyApiTab();
                })
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

$(document).ready(function () {
    if (AlarmCusId != "") {
        $(".AlarmComDetailTab_Load").load("/API/AlarmCustomerDetails/?CustomerId=" + AlarmCusId + "&&CustomerLoadGuid=" + CustomerLoadGuid);
    }
    $(".CreateCustomer").click(function () {
        OpenTopToBottomModal(String.format("/API/AddAlarmCustomer/?CustomerId={0}&Actions={1}", CustomerLoadGuid, CreateCustomer))
    });
    $(".CreateCommitment").click(function () {
        OpenTopToBottomModal(String.format("/API/AddAlarmCustomer/?CustomerId={0}&Actions={1}", CustomerLoadGuid, CreateCommiment))
    });
    $(".editsettingsbtn").click(function () {
        console.log("clicked");
        OpenRightToLeftModal(domainurl + "/API/EditGlobalSettingsAlarmSearchkeyandValue/")
    });
    $(".ActivateCommitment").click(function () {
        OpenTopToBottomModal(String.format("/API/AddAlarmCustomer/?CustomerId={0}&Actions={1}", CustomerLoadGuid, ActiveateCommitment))
    });
    $(".syncbtn").click(function () {
        console.log("dsfsdf");
        var syncid = $("#AlarmSyncId").val();
        SyncWithAlarm(syncid);
    })
    $(".TerminatedLog").click(function () {
        window.open('/API/TerminationHistoryLog?CustomerId=' + CustomerLoadGuid, '_blank');
    });
  
    
});
