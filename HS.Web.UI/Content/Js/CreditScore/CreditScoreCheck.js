var CreditSoftScoreCheckConfirm = function (CreditScoreBureau, ContactId) {
    OpenConfirmationMessageNew("", "Do you want to generate a credit report/not?", function () {
        CreditScoreCheck(true, CreditScoreBureau, ContactId);
    })
}
var CreditHardScoreCheckConfirm = function (CreditScoreBureau, ContactId) {
    OpenConfirmationMessageNew("", "Do you want to generate a credit report/not?", function () {
        CreditScoreCheck(false, CreditScoreBureau, ContactId);
    })
}

var SendCreditCheckRequest = function (HardOrSoft, BUREAU, ContactId) {
    console.log(HardOrSoft);
    if (BrinksCreditCheck == "true") {
        CheckBrinksCreditScore(HardOrSoft, BUREAU, ContactId);
    }
    else {
        if (HasCreditCheck == "true") {
            OpenConfirmationMessageNew("", "Credit Report for this customer has already been generated. Do you want to Generate again?", function () {
                CreditScoreCheck(HardOrSoft, BUREAU, ContactId);

            });

        }
        else {

            if (HardOrSoft == true) {
                CreditSoftScoreCheckConfirm(BUREAU, ContactId);
            }
            else {
                CreditHardScoreCheckConfirm(BUREAU, ContactId);
            }


        }
    }
}

var CreditScoreCheck = function (IsSoftCheck, CreditScoreBureau, ContactId) {
    url = domainurl + "/SmartLeads/GetCreditScore/";
    var param = {
        CustomerId: CustomerId,
        IsSoftCheck: IsSoftCheck,
        BUREAU: CreditScoreBureau,
        ContactId: ContactId
    };

    $.ajax({
        type: "POST",
        ajaxStart: $(".LoaderWorkingDiv").show(),
        url: url,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".LoaderWorkingDiv").hide();
            if (data.result == "true") {
                if (data.Score != "" && data.Score != "undefined") {
                    OpenSuccessMessageNew("", "Credit score report generated successfully.", function () {
                        //$("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartPackagePartial?id=" + LeadIdVal);

                    })
                }
                else {
                    OpenErrorMessageNew("", "Credit score not generated for some missing or invalid field.", function () {
                        //$("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartPackagePartial?id=" + LeadIdVal);

                    })
                }

            }
            else {
                $(".LoaderWorkingDiv").hide();
                OpenErrorMessageNew("", "Internal Error!");
            }

        }
    });

}

var CheckBrinksCreditScore = function (IsSoftCheck, CreditScoreBureau, ContactId) {

    url = domainurl + "/API/CheckBrinksCreditScore/";
    var param = {
        CustomerId: CustomerId,
        ContactId: ContactId
    };

    $.ajax({
        type: "POST",
        ajaxStart: $(".LoaderWorkingDiv").show(),
        url: url,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".LoaderWorkingDiv").hide();
            $(".sewsLoaderContact").hide();
            if (data.result == true) {
                if (data.matchcode == '1') {
                    OpenConfirmationMessageNew("", data.message, function () {
                        CreditScoreCheck(IsSoftCheck, CreditScoreBureau, ContactId);
                    })
                }
                else {
                    OpenConfirmationMessageNew("", "Do you want to generate a credit report/not?", function () {
                        CreditScoreCheck(IsSoftCheck, CreditScoreBureau, ContactId);
                    });

                }

            }
            else {
                $(".LoaderWorkingDiv").hide();
                $(".sewsLoaderContact").hide();


                OpenErrorMessageNew("", data.message);
            }
        }
    });
}

$(document).ready(function () {
    $("#EFXCheckCreditSoft").click(function () {

        if ($("#UseDiffAddress").is(':checked') == true) {
            console.log("dfsd");
            var LoadUrl = domainurl + "/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=" + LeadGuid + "&IsSoftCheck=true&Bureau=EFX&For=CreditCheck";
            $(".SecondaryContactMagnific").attr("href", LoadUrl);
            setTimeout(function () { $(".SecondaryContactMagnific").click(); }, 1000);

        }
        else {
            SendCreditCheckRequest(true, "EFX", 0);
        }

    });
    $("#EFXCheckCreditHard").click(function () {
        if ($("#UseDiffAddress").is(':checked') == true) {
            console.log("dfsd");
            var LoadUrl = domainurl + "/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=" + LeadGuid + "&IsSoftCheck=false&Bureau=EFX";
            $(".SecondaryContactMagnific").attr("href", LoadUrl);
            setTimeout(function () { $(".SecondaryContactMagnific").click(); }, 1000);

        }
        else {
            SendCreditCheckRequest(false, "EFX", 0);
        }
    });
    $("#TUCheckCreditSoft").click(function () {
        if ($("#UseDiffAddress").is(':checked') == true) {
            console.log("dfsd");
            var LoadUrl = domainurl + "/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=" + LeadGuid + "&IsSoftCheck=true&Bureau=TU";
            $(".SecondaryContactMagnific").attr("href", LoadUrl);
            setTimeout(function () { $(".SecondaryContactMagnific").click(); }, 1000);

        }
        else {
            SendCreditCheckRequest(true, "TU", 0);
        }


    });
    $("#TUCheckCreditHard").click(function () {
        if ($("#UseDiffAddress").is(':checked') == true) {
            console.log("dfsd");
            var LoadUrl = domainurl + "/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=" + LeadGuid + "&IsSoftCheck=false&Bureau=TU";
            $(".SecondaryContactMagnific").attr("href", LoadUrl);
            setTimeout(function () { $(".SecondaryContactMagnific").click(); }, 1000);

        }
        else {
            SendCreditCheckRequest(false, "Tu", 0);
        }


    });
})