var CreditScoreCheck = function () {
    url = domainurl + "/SmartLeads/GetCreditScore/";
    var param = {
        CustomerId: '@Model.CustomerId'
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
            if (data.result == "true") {
                if (data.Score != "" && data.Score != "undefined") {
                    OpenSuccessMessageNew("", "Credit score report generated successfully.", function () {
                        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartPackagePartial?id=" + LeadIdVal);

                    })
                }
                else {
                    OpenErrorMessageNew("", "Credit score not generated for some missing or invalid field.", function () {
                        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartPackagePartial?id=" + LeadIdVal);

                    })
                }

            }
            else {
                OpenErrorMessageNew("", "Internal Error!");
            }

        }
    });

}

var LoaderDom = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var OpenInstallationAgreement = function () {
    $("#InstallationAgreement")[0].click();
}
$(document).ready(function () {
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".LoadAgreementPopUp", type: 'iframe', width: Popupwidth, height: 650 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    var idlist1 = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".MapManufacturerMagnificPrevious", type: 'iframe', width: 920, height: 520 }
    ];
    jQuery.each(idlist1, function (i, val) {
        magnificPopupObj(val);
    });

    $('.bk-btn').click(function () {
        if ($(".bk-btn").attr("id-val") == 0) {
            LoadCustomerDetail(LeadIdVal);
        }
        else {
            LoadLeadsDetail(LeadIdVal);
        }
        //history.go(-1);
        //setTimeout(function () {
        //    window.location.reload();
        //}, 100);
        /*LoadLeadVerificationInfo(LeadIdVal, true);*/
        //LoadLeadsDetail(LeadIdVal);
    });
    console.log('loader');
    $("#TabsLoaderText").html(LoaderDom);
    if (setupClick == "Equipment") {
        $(".LoadEquipment").addClass('current');
        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartEquipmentPartial?LeadId=" + LeadIdVal);
        $(".LoadEquipment").addClass('activeli');
        $(".LoadPackage").removeClass('activeli');
    }
    else if (setupClick == "Service") {
        $(".LoadService").addClass('current');
        $(".LoadEquipment").addClass('current');
        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartServicePartial?LeadId=" + LeadIdVal);
        $(".LoadService").addClass('activeli');
        $(".LoadPackage").removeClass('activeli');
    }
    else if (setupClick == "Emergency") {
        $(".LoadService").addClass('current');
        $(".LoadEquipment").addClass('current');
        $(".LoadEmergency").addClass('activeli');
        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartAgreementDetails?id=" + LeadIdVal);
        $("#btnSavandNex span").text("Save & Sign");
        $("#btnSavandClose").removeClass('hidden');
        $("#btnPayNow").removeClass('hidden');
        $("#btnsendEcontract").removeClass('hidden');
        $("#btnsendIsPccontract").removeClass('hidden');
        $("#leadToCustomerConvert").removeClass('hidden');
        $(".LoadPackage").removeClass('activeli');
    }
    else if (setupClick == "Finalize") {
        $(".LoadService").addClass('current');
        $(".LoadEquipment").addClass('current');
        $(".LoadEmergency").addClass('current');
        $(".LoadFinalize").addClass('activeli');
        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartAgreementFinalize?id=" + LeadIdVal);
        $("#btnSavandNex span").text("Save & Sign");
        $("#btnSavandClose").removeClass('hidden');
        $(".ContractTypeDiv").removeClass('hidden');
        $("#btnPayNow").removeClass('hidden');
        $("#btnsendEcontract").removeClass('hidden');
        $("#btnsendIsPccontract").removeClass('hidden');
        $("#leadToCustomerConvert").removeClass('hidden');


        $(".LoadPackage").removeClass('activeli');
    }
    else {
        $("#LoadLeadDetail").load(domainurl + "/SmartLeads/SmartPackagePartial?id=" + LeadIdVal);
    }
    if (FifthSetup == "FifthSetup") {
        $(".LoadFinalize").addClass('current');
        $(".LoadEmergency").addClass('current');
        $(".LoadEquipment").addClass('current');
        $(".LoadService").addClass('current');
        $(".LoadPackage").addClass('current');
    }
    if (FourthSetup == "FourthSetup") {
        $(".LoadEmergency").addClass('current');
        $(".LoadEquipment").addClass('current');
        $(".LoadService").addClass('current');
        $(".LoadPackage").addClass('current');
    }
    if (ThirdSetup == "ThirdSetup") {
        $(".LoadEquipment").addClass('current');
        $(".LoadService").addClass('current');
        $(".LoadPackage").addClass('current');
    }
    if (SecondSetup == "SecondSetup") {
        $(".LoadService").addClass('current');
        $(".LoadPackage").addClass('current');
    }
    if (FirstSetup == "FirstSetup") {
        $(".LoadPackage").addClass('current');
    }
    $(".leadMapPopup").click(function () {
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + LeadGuid;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
    //$('[data-toggle="tooltip"]').tooltip();
});