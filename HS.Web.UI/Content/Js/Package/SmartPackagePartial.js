var LoadPackageEquipments = function (Pid, Lid) {
    console.log("dfsfdfsd");
    $(".package-additional-features").load(domainurl + "/SmartLeads/LoadSmartLeadPackageEquipments/?PackageId=" + Pid + "&LeadId=" + Lid);
}
var CreditSoftScoreCheckConfirm = function (CreditScoreBureau,ContactId) {
    OpenConfirmationMessageNew("", "Do you want to generate a credit report/not?", function () {
        CreditScoreCheck(true, CreditScoreBureau, ContactId);
    })
}
var CreditHardScoreCheckConfirm = function (CreditScoreBureau,ContactId) {
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

var CreditScoreCheck = function (IsSoftCheck, CreditScoreBureau,ContactId) {
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
                $(".LoaderWorkingDiv").hide();
                OpenErrorMessageNew("", "Internal Error!");
            }

        }
    });

}

var CheckBrinksCreditScore = function (IsSoftCheck, CreditScoreBureau,ContactId) {

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

var FillInstallType = function (SystemTypeId) {
    $.ajax({
        url: domainurl + '/SmartLeads/FillInstallType',
        type: "GET",
        dataType: "JSON",
        data: { SystemTypeId: SystemTypeId },
        success: function (data) {
            $("#SmartInstallTypeId").html("");
            $("#PackageId").html("");
            $("#ManufacturerId").html("");
            $("#PackageId").append(
                    $('<option></option>').val("-1").html("Please Select One"));
            $(".package-additional-features").html('');

            var InstallType = data.SmartInstallTypeList;
            $.each(InstallType, function (i, InstallType) {
                $("#SmartInstallTypeId").append(
                    $('<option></option>').val(InstallType.Id).html(InstallType.Name));
            });

            var ManufacturerType = data.ManufacturerList;
            $.each(ManufacturerType, function (i, ManufacturerType) {
                $("#ManufacturerId").append(
                    $('<option></option>').val(ManufacturerType.ManufacturerId).html(ManufacturerType.Name));
            });
        }
    });
}
var FillPackage = function (SystemTypeId, InstallTypeId,ManufacturerId) {
    $.ajax({
        url: domainurl + '/SmartLeads/FillPackage',
        type: "GET",
        dataType: "JSON",
        data: { SystemTypeId: SystemTypeId, InstallTypeId: InstallTypeId, ManufacturerId: ManufacturerId,LeadId:LeadId },
        success: function (data) {
            var LeadCS = data.LeadCS;
            if (LeadCS == 'undefined' || LeadCS == '' || LeadCS==null)
            {
                LeadCS = 0;
            }
            var PackageList = data.PackageList;

            $("#PackageId").html("");
            $(".package-additional-features").html('');
            var Template = '<option value="{0}" style="{2}"  >{1}</option>';
             
            $.each(PackageList, function (i, Package) {
                var Style = "";
                if (Package.MinCredit == '0' || Package.MinCredit == 'undefined')
                {
                    Style = "color:blue";
                }
                else if (parseInt(Package.MinCredit) > parseInt(LeadCS)) {
                    Style = "color:red";
                   
                }else{
                    Style = "color:green";
                }
                $("#PackageId").append(
                    String.format(Template, Package.PackageId, Package.PackageName, Style));
                    //$().val(Package.PackageId).html(Package.PackageName));
            });
        }
    });
}
var loadSecondaryContact = function () {
    $(".SecondaryContactMagnific").attr('href', '/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=' + LeadGuid);
    $(".SecondaryContactMagnific").click();
}
$(document).ready(function () {

    var idlist = [
        { id: ".SecondaryContactMagnific", type: 'iframe', width: 550, height: 420 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $("#btnSavandNex").removeClass("hidden");
    $("#PackageId").change(function () {
        if ($("#PackageId").val() != "-1") {
            $(this).removeClass('required');
        }
      
            LoadPackageEquipments($(this).val(), LeadId);
        
      
    });
    if (HasDiffrentCreditContact == 'True') {
        $("#UseDiffAddress").prop('checked', true)
        $("#SecondaryContactList").load("/SmartPackageSetup/SecondaryContactListForCreditCheck?CustomerId=" + LeadGuid+"&For=CreditCheck");
    }
    $("#UseDiffAddress").change(function () {
        if ($("#UseDiffAddress").is(':checked') == true) {
            $("#SecondaryContactList").load("/SmartPackageSetup/SecondaryContactListForCreditCheck?CustomerId=" + LeadGuid + "&For=CreditCheck");
        }
        else {
            $("#SecondaryContactList").html("");
        }
    });
  
   
    $("#SmartInstallTypeId").change(function () {
        if ($("#SmartInstallTypeId").val() != "-1") {
            $(this).removeClass('required');
        }
        FillPackage($("#SmartSystemTypeId").val(), $("#SmartInstallTypeId").val(), $("#ManufacturerId").val());
    });
    $("#ManufacturerId").change(function () {
        if ($("#SmartInstallTypeId").val() != "-1") {
            $("#SmartInstallTypeId").removeClass('required');
        }
        FillPackage($("#SmartSystemTypeId").val(), $("#SmartInstallTypeId").val(),$("#ManufacturerId").val());
    });
    
    $("#SmartSystemTypeId").change(function () {
        if ($("#SmartSystemTypeId").val() != "-1") {
            $(this).removeClass('required');
        }
        $("#ManufacturerId").val("-1");
        FillInstallType($("#SmartSystemTypeId").val());
    })


  
    if (PackageInitVal != "-1" && LeadId != null && LeadId > 0) {
        LoadPackageEquipments(PackageInitVal, LeadId);
    }
    console.log(PackageId);
    if (PackageId != "") {
        $("#PackageId").val(PackageId);
        $(".package-additional-features").load(domainurl + "/SmartLeads/LoadSmartLeadPackageEquipments/?PackageId=" + PackageId + "&LeadId=" + LeadId);
    }
    else {
        $("#PackageId").val("-1");
    }

    $("#EFXCheckCreditSoft").click(function () {

        if ($("#UseDiffAddress").is(':checked') == true) {
            console.log("dfsd");
            var LoadUrl = domainurl + "/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=" + LeadGuid+"&IsSoftCheck=true&Bureau=EFX&For=CreditCheck";
            $(".SecondaryContactMagnific").attr("href", LoadUrl);
            setTimeout(function () { $(".SecondaryContactMagnific").click();}, 1000);
            
        }
        else {
            SendCreditCheckRequest(true, "EFX",0);
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
            SendCreditCheckRequest(false, "EFX",0);
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
            SendCreditCheckRequest(true, "TU",0);
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
            SendCreditCheckRequest(false, "Tu",0);
        }
       

    });
});