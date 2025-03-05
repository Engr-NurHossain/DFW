var GetBestPracticeData = function () {
    url = domainurl + "/API/GetBestPracticeDataByCusId/";
    $(".practiceData").show();
    var param = {
        CustomerId: CustomerIdInt
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
            console.log(data);
            if (data.result == "true") {
                console.log(data);
                if (data.EmailVerified == true) {
                    $(".email").html("Y");
                }
                else {
                    $(".email").html("N/A");
                }

                if (data.MobileContact == true) {
                    $(".contact").html("Y");
                }
                else {
                    $(".contact").html("N/A");
                }

                if (data.ArmingRemainder == true) {
                    $(".reminder").html("Y");
                }
                else {
                    $(".reminder").html("N/A");
                }

                if (data.ArmingRemainder == true) {
                    $(".email").html("Y");
                }
                else {
                    $(".email").html("N/A");
                }

                if (data.GeoDevice == true) {
                    $(".geodevice").html("Y");
                }
                else {
                    $(".geodevice").html("N/A");
                }

                if (data.AdvanceDevice == true) {
                    $(".advancedevice").html("Y");
                }
                else {
                    $(".advancedevice").html("N/A");
                }

                if (data.RuleOrSchedule == true) {
                    $(".rule").html("Y");
                }
                else {
                    $(".rule").html("N/A");
                }




            }

        }
    });

}

var RunSystemCheck = function () {
    var url = domainurl + '/API/RunSystemCheck';
    console.log(MonitoringPlatform);
    var param = JSON.stringify({
        CustomerId: CustomerIdInt

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
            if (data.result == "true") {
                OpenSuccessMessageNew("", data.message, function () {

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


var RemoveUnassociateCus = function (syncid) {
    var url = domainurl + '/API/RemoveUnassociateCus';
    var param = JSON.stringify({
        CustomerGuidId: CustomerLoadGuid,
        Platform: MonitoringPlatform
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
 
    $(".practiceData").hide();
    $(".bestPracticeApi").click(function () {
        GetBestPracticeData();
    })
    $(".Refresh").click(function () {
        OpenThirdPartyApiTab();
    })

    $(".syncbtn").click(function () {
        var syncid = $("#AlarmSyncId").val();
        SyncWithAlarm(syncid);
    })
    $(".CreateCustomer").click(function () {
        OpenTopToBottomModal(String.format("/API/AddAlarmCustomer/?CustomerId={0}&Actions={1}", CustomerLoadGuid, CreateCustomer))
    });
    $(".Unassociate").click(function () {
        OpenConfirmationMessageNew("", "Do you want to remove this customer?", function () {
            RemoveUnassociateCus();
        })
       
    })
    $(".ShowEquipments").click(function () {

        window.open('/API/AlarmEquipmentPartial?CustomerId=' + CustomerIdInt, '_blank');

    })
    $(".RunSystemCheck").click(function () {
        RunSystemCheck();
    })
    
    $(".DoSystemCheck").click(function () {
        window.open('/API/DoSystemRunTestByCusId?CustomerId=' + CustomerIdInt, '_blank');


    })
        
  
    $(".TerminateCustomer").click(function () {
        OpenTopToBottomModal("/API/CustomerTermination?CustomerId=" + CustomerIdInt + "&CustomerLoadGuid=" + CustomerLoadGuid);
    });
})
