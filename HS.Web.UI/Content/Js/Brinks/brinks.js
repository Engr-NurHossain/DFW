var SyncWithBrinks = function (syncid) {
    var url = domainurl + '/API/SyncBrinksCustomer';
    var param = JSON.stringify({
        BrinksRefId: syncid,
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
            console.log("dfsd");
            if (data.result == true) {
                OpenSuccessMessageNew("", "", function () { });
                $(".BrinksTab").click();
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


var PlaceAccountOnTestConfirm = function () {
    OpenConfirmationMessageNew("", "Do you want to place this account on test?", function () {
        PlaceAccountOnTest();
    });
}
var PlaceAccountOnTest = function () {

    var url = domainurl + '/API/PlaceAccountOnTest';
    var param = JSON.stringify({
        CustomerGuidId: CustomerLoadGuid,
        TestCategory: $("#TestCategory").val(),
        TestHour: $("#testHours").val(),
        TestSec: $("#testSec").val()

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
var CustomerLoadDivBrinks = ".third_party_api_tabs_" + CustomerLoadGuid;
$(document).ready(function () {
    if (BrinksRefId != "") {
        var CustomerLoadDivBrinks = ".third_party_api_tabs_" + CustomerLoadGuid;
        $(CustomerLoadDivBrinks + " #BrinksCustomerDetailsLoad").load("/API/BrinksCustomerDetails/?BrinksRefId=" + BrinksRefId + "&&CustomerId=" + CustomerLoadGuid);
    }
    $("#PlaceTest").click(function () {

        PlaceAccountOnTestConfirm();
    });
 
    $(".CreateTwoWayTest").click(function () {
        OpenRightToLeftModal("/Api/AddTwoWayTest");

    });
    $(".CreateCustomerBrinks").click(function () {
        OpenTopToBottomModal(String.format("/API/AddBrinksAccount/?CustomerId={0}", CustomerLoadGuid))
    });
    $(".editbrinkssettingsbtn").click(function () {
        console.log("clicked");
        OpenRightToLeftModal(domainurl + "/API/EditGlobalSettingsBrinksSearchkeyandValue/")
    });
    $(".syncbtnBrinks").click(function () {
        var syncid = $("#BrinksSyncId").val();
        SyncWithBrinks(syncid);
    })

});
