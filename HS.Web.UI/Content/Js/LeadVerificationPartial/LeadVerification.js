
var EstCloseDatePicker;
var ProjectWalkDatePicker;
function FormateFax(Value) {
    var CleanValue = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            CleanValue = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#Fax").css({ "border": "1px solid #babec5" });
        }
        else if (Value.length > 10) {
            CleanValue = Value;
            $("#Fax").css({ "border": "1px solid red" });
        }
        else {
            $("#Fax").css({ "border": "1px solid red" });
            CleanValue = Value;
        }
    }
    return CleanValue;
}
var DeleteLead = function () {
    var url = domainurl + "/Leads/DeleteLead";
    $.ajax({
        url: url,
        data: { id: selectedID },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data == true) {
                OpenSuccessMessageNew("Success!", "Lead Deleted Successfully.", function () {
                    var pagenumber = 0;
                    LoadLeads(true);
                    CloseTopToBottomModal();
                });
            }
        }
    });
}
var initDocReady = function () {
    EstCloseDatePicker = new Pikaday({
        field: $('#EstCloseDate')[0],
        trigger: $('#EstCloseDate_custom')[0],
        format: 'MM/DD/YYYY',
    });
    ProjectWalkDatePicker = new Pikaday({
        field: $('#ProjectWalkDate')[0],
        trigger: $('#ProjectWalkDate_custom')[0],
        format: 'MM/DD/YYYY',
    });
}
function phone_validate(phno) {
    var regexPattern = new RegExp(/^[0-9-+]+$/);    // regular expression pattern
    return regexPattern.test(phno);
}

var ChackPhoneNumberValidity = function (phone_number, phone_Type, CustomerId, FormatedPhoneNo) {
    console.log(phone_number);

    if (phone_validate(phone_number)) {
        url = domainurl + "/Customer/VerifyPhone/";
        var param = {

            PhoneNumber: phone_number,
            CustomerId: CustomerId,
            PhoneType: phone_Type,
            FormatedPhoneNo: FormatedPhoneNo
        };
        // verify phone number via AJAX call
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (json) {
                console.log(json);
                // Access and use your preferred validation result objects

                if (json.result == "true") {
                    if (phone_Type == "PrimaryPhone") {
                        $("#isSitePhoneVerified").val("true");
                        $(".notVerifiedSitePhoneNumber").addClass("hidden");
                        $(".verifiedSitePhoneNumber").removeClass("hidden");
                    }
                    else if (phone_Type == "secondaryPhone") {
                        $("#isSecondaryPhoneVerified").val("true");
                        $(".notVerifiedSecondaryPhoneNumber").addClass("hidden");
                        $(".verifiedSecondaryPhoneNumber").removeClass("hidden");
                    }
                    else if (phone_Type == "CellNo") {
                        $("#isCellPhoneVerified").val("true");
                        $(".notVerifiedCellPhoneNumber").addClass("hidden");
                        $(".verifiedCellPhoneNumber").removeClass("hidden");
                    }
                    var VerifyInfo = "";
                    if (json.country_code != "") {
                        VerifyInfo += "<b>Country</b>: " + json.country_code + "<br/>";
                    }
                    if (json.carrier != "") {
                        VerifyInfo += "<b>Carrier<b/>: " + json.carrier + "<br/>";
                    }
                    if (json.location != "") {
                        VerifyInfo += "<b>Location<b/>: " + json.location + "<br/>";
                    }
                    if (json.line_type != "") {
                        VerifyInfo += "<b>Line type<b/>: " + json.line_type + "<br/>";
                    }
                    OpenSuccessMessageNew("Validate", "Phone number is valid.<br/>" + VerifyInfo);
                }
                else {
                    if (phone_Type == "PrimaryPhone") {
                        $("#isSitePhoneVerified").val("false");
                        $(".notVerifiedSitePhoneNumber").removeClass("hidden");
                        $(".verifiedSitePhoneNumber").addClass("hidden");
                    }
                    else if (phone_Type == "secondaryPhone") {
                        $("#isSecondaryPhoneVerified").val("true");
                        $(".notVerifiedSecondaryPhoneNumber").removeClass("hidden");
                        $(".verifiedSecondaryPhoneNumber").addClass("hidden");
                    }
                    else if (phone_Type == "CellNo") {
                        $("#isCellPhoneVerified").val("false");
                        $(".notVerifiedCellPhoneNumber").removeClass("hidden");
                        $(".verifiedCellPhoneNumber").addClass("hidden");
                    }

                    OpenErrorMessageNew("", "Phone number not valid!");
                }
            }
        });
    }
    else {
        if (phone_Type == "PrimaryPhone") {
            $("#isSitePhoneVerified").val("false");
            $(".notVerifiedSitePhoneNumber").removeClass("hidden");
            $(".verifiedSitePhoneNumber").addClass("hidden");
        }
        else if (phone_Type == "secondaryPhone") {
            $("#isSecondaryPhoneVerified").val("true");
            $(".notVerifiedSecondaryPhoneNumber").removeClass("hidden");
            $(".verifiedSecondaryPhoneNumber").addClass("hidden");
        }
        else if (phone_Type == "CellNo") {
            $("#isCellPhoneVerified").val("false");
            $(".notVerifiedCellPhoneNumber").removeClass("hidden");
            $(".verifiedCellPhoneNumber").addClass("hidden");
        }
        OpenErrorMessageNew("", "Phone number not valid!")
    }

}

var CheckPrimaryPhone = function (CustomerId) {
    var phonenumber = $("#PrimaryPhone").val().replace(/[\s-()]/g, '').trim();
    var FormatedPhoneNo = $("#PrimaryPhone").val();
    ChackPhoneNumberValidity(phonenumber, "PrimaryPhone", CustomerId, FormatedPhoneNo);
}
var CheckCellPhone = function (CustomerId) {
    var phonenumber = $(".CellPhone").val().replace(/[\s-()]/g, '').trim();
    var FormatedPhoneNo = $(".CellPhone").val()
    ChackPhoneNumberValidity(phonenumber, "CellNo", CustomerId, FormatedPhoneNo);
}

var CheckSecondaryPhone = function (CustomerId) {
    var phonenumber = $("#SecondaryPhone").val().replace(/[\s-()]/g, '').trim();
    var FormatedPhoneNo = $("#SecondaryPhone").val();
    ChackPhoneNumberValidity(phonenumber, "secondaryPhone", CustomerId, FormatedPhoneNo);
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
var CheckBrinksCreditScore = function (IsSoftCheck, CreditScoreBureau, ContactId) {

    url = domainurl + "/API/CheckBrinksCreditScore/";
    var param = {
        CustomerId: CustomerGuidId,
        ContactId: ContactId
    };

    $.ajax({
        type: "POST",
        ajaxStart: $(".LoaderWorkingDiv,.sewsLoaderContact").show(),
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
var CreditScoreCheck = function (IsSoftCheck, CreditScoreBureau, ContactId) {
    url = domainurl + "/JsonRequest/GetCreditScore/";
    var param = {
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        SSN: $("#SSN").val(),
        ADDRESS: $("#Street").val(),
        CITY: $("#City").val(),
        STATE: $("#State").val(),
        ZIP: $("#ZipCode").val(),
        DOB: $("#DateofBirth").val(),
        CustomerId: CustomerGuidId,
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
                $("#CreditScore").show();
                $(".score").html("");
                $(".hitStatus").html("");

                $("#CreditRecordpdf").html("");

                $(".score").html(data.Score);

                $(".hitStatus").html(data.HitStatus);



                responseresult = data.response;
                $("#CreditRecordpdf").html(data.ReportName);
                ReportPdfLink = data.ReportLink;
                if (data.Score != "" && data.Score != "undefined") {
                    OpenSuccessMessageNew("", "Credit score report generated successfully.", function () {
                        //window.location.href = '/LeadsVerificationDetail?id=' + cusId;
                        location.reload();
                    })
                }
                else {
                    OpenErrorMessageNew("", "Credit score not generated for some missing or invalid field.")
                }


            }
            else {
                $(".LoaderWorkingDiv").hide();
                OpenErrorMessageNew("", "Internal Error!", function () {
                    location.reload();
                });
            }

        }
    });

}
var CreditSoftScoreCheckConfirm = function (CreditScoreBureau, ContactId) {
    console.log("sdf");
    OpenConfirmationMessageNew("", "Do you want to generate a credit report/not?", function () {
        CreditScoreCheck(true, CreditScoreBureau, ContactId);
    })
}
var CreditHardScoreCheckConfirm = function (CreditScoreBureau, ContactId) {
    OpenConfirmationMessageNew("", "Do you want to generate a credit report/not?", function () {
        CreditScoreCheck(false, CreditScoreBureau, ContactId);
    })
}
var responseresult = "";

$(document).ready(function () {
    var idlist = [
        { id: ".SecondaryContactMagnific", type: 'iframe', width: 550, height: 420 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    initDocReady();
    var PrimaryPhone = $("#PrimaryPhone").val();
    if (PrimaryPhone != undefined && PrimaryPhone.length == 10) {
        $("#PrimaryPhone").val(PrimaryPhone.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var SecondaryPhone = $("#SecondaryPhone").val();
    if (SecondaryPhone != undefined && SecondaryPhone.length == 10) {
        $("#SecondaryPhone").val(SecondaryPhone.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var CellNo = $("#CellNo").val();
    if (CellNo != undefined && CellNo.length == 10) {
        $("#CellNo").val(CellNo.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var AgreementPhoneNo = $("#AgreementPhoneNo").val();
    if (AgreementPhoneNo != undefined && AgreementPhoneNo.length == 10) {
        $("#AgreementPhoneNo").val(AgreementPhoneNo.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    document.title = "Edit " + CustomerName + "'s Profile";
    $("#Fax").keyup(function () {
        var Fax = $(this).val();
        if (Fax != undefined && Fax != null && Fax != "") {
            var cleanFax = FormateFax(Fax);
            $(this).val(cleanFax);
        }
    });
    console.log(LeadsCarrier);
    if (LeadsCarrier != '') {
        $("#Carrier").val(LeadsCarrier)

    }
    $("#Soldby").change(function () {
        $("#SalesLocation").val($("#Soldby").find(':selected').attr('salesloc'));
        $("#SalesLocation").select2();
    });
    $("#CustomerExtended_AppoinmentSetBy").change(function () {
        var appoinmentsetselectval = $("#CustomerExtended_AppoinmentSetBy").find(':selected').val();
        var currentselectsoldby = $("#Soldby").find(':selected').val();
        var modelselectsoldby = $("#Soldby").find(':selected').attr('selectsoldby');
        var url = domainurl + "/Leads/UpdateSalesPersonListForAddLead/";
        var param = {
            modelSelectSalesPerson: $("#Soldby").find(':selected').attr('selectsoldby'),
            removeSalesPerson: $("#CustomerExtended_AppoinmentSetBy").find(':selected').val()
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
                if (data) {
                    $("#Soldby option").remove();
                    var resultparse = JSON.parse(data.result);
                    console.log(resultparse.length);
                    if (resultparse.length > 0) {
                        var searchresultstring = "<option value='00000000-0000-0000-0000-00000000000'>Please Select</option>";
                        for (var i = 0; i < resultparse.length; i++) {
                            if (appoinmentsetselectval != currentselectsoldby && currentselectsoldby == resultparse[i].UserId) {
                                searchresultstring += "<option value='" + resultparse[i].UserId + "' selected='" + true + "' selectsoldby='" + modelselectsoldby + "' salesloc='" + resultparse[i].SalesCommissionStructure + "' >" + resultparse[i].FirstName + " " + resultparse[i].LastName + "</option>";
                            }
                            else {
                                searchresultstring += "<option value='" + resultparse[i].UserId + "' selectsoldby='" + modelselectsoldby + "' salesloc='" + resultparse[i].SalesCommissionStructure + "' >" + resultparse[i].FirstName + " " + resultparse[i].LastName + "</option>";
                            }
                        }
                        $("#Soldby").html(searchresultstring);
                    }
                }
                else {
                    OpenErrorMessageNew("Error!", "Invalid user selected");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    if (typeof (accountType) != "undefined" && accountType != null && accountType != "") {
        $(".CustomerAccountTypeselectpicker").selectpicker('val', accountType);
    }
    else if (window.location.pathname != "/SmartLeadSetup/") {
        $(".CustomerAccountTypeselectpicker").selectpicker();
    }
    $("#Soldby").select2();
    $("#SoldBy2").select2();
    $("#SoldBy3").select2();
    $("#AccessGivenTo").select2();
    $("#CustomerExtended_FinanceRep").select2();
    $("#CustomerExtended_AppoinmentSetBy").select2();
    $("#SalesLocation").select2();
    $("#Type").select2();
    $("#CSProvider").select2();
    $("#LeadSource").select2();
    if (leadsource != "-1") {
        $("#LeadSource").val(leadsource).trigger("change");
    }
    $("#PhoneType").select2();
    $("#PreferredContactMethod").select2();
    $("#BestTimeToCall").select2();
    $("#CreditScoreValue").select2();
    $("#BranchId").select2();
    $("#BusinessAccountType").select2();
    $("#TaxExemption").select2();
    $("#LeadStatus").select2();
    $("#LeadSourceType").select2();
    $("#CustomerExtended_FinanceCompany").select2();
    $("#CustomerExtended_IsFinanced").select2();
    $("#CustomerExtended_Term").select2();
    //$("#LeadSource").change(function () {
    //    if ($(this).attr('datarequired') == 'true' && $(this).val() == "-1") {
    //        $($(this).next()).addClass('required');
    //    } else {
    //        $($(this).next()).removeClass('required');
    //    }
    //});
    if (cusId > 0) {
        $("#AddNotes").show();
    }
    else {
        $("#AddNotes").hide();
    }

    $("#AddNotes").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/AddLeadNotes?CustomerId=" + CustomerGuidId + "&From=leadform")
    });
    if (businessnamerequired.toLowerCase() == "true") {
        if ($("#Type").val() == "Commercial") {
            console.log("pointer");
            $('#BusinessName').attr('datarequired', 'true');
            $(".star").removeClass("hidden");
        }
        else {
        }

        $("#Type").change(function () {
            console.log("pointer");
            if ($("#Type").val() == "Commercial") {
                $('#BusinessName').attr('datarequired', 'true');
                $(".star").removeClass("hidden");


            }
            else {
                $(".star").addClass("hidden");
                $('#BusinessName').attr('datarequired', 'false');



            }
        })
    }

    $("#DownloadCreditRecord").click(function () {
        var pdfUrl = "/Leads/ViewCreditScore?CustomerId=" + CustomerGuidId;
        console.log(pdfUrl);
        window.open(pdfUrl, '_blank');
    });

    $("#CreditRecordpdf").click(function () {
        console.log("dsf");
        var pdfUrl = "/File/DownloadCreditCheckPdf?id=" + ReportPdfId;
        console.log(pdfUrl);
        window.open(pdfUrl);
    });
    if (HasCreditCheck == "false") {
        $("#CreditScore").hide();

    }

    $("#UseDiffAddress").change(function () {
        if ($("#UseDiffAddress").is(':checked') == true) {
            $("#SecondaryContactList").load("/SmartPackageSetup/SecondaryContactListForCreditCheck?CustomerId=" + CustomerGuidId + "&For=CreditCheck");
        }
        else {
            $("#SecondaryContactList").html("");
        }
    });


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
    //$("#ReferringCustomer").change(function () {
    //    if ($("#ReferringCustomer").val() != "00000000-0000-0000-0000-000000000000") {
    //        $("#LeadSource").val('ReferredbyCustomer');
    //    }

    //});
    $("#checkEmail").click(function () {
        url = domainurl + "/Customer/VerifyEmail/";
        var param = {
            id: $("#id").val(),
            CustomerId: CustomerGuidId,
            EmailAddress: $(".EmailAddress").val(),
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

                $(".loader-div").hide();
                if (data.result == false) {
                    $("#verifiedemail").addClass("hidden");
                    $("#notverifiedemail").removeClass("hidden");

                    OpenErrorMessageNew("Error!", data.message, function () { });
                }
                else {
                    isVerified = true
                    $("#isVerified").val(isVerified);

                    $("#verifiedemail").removeClass("hidden");
                    $("#notverifiedemail").hide();
                    OpenSuccessMessageNew("Success!", data.message, function () { });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });





    })
    $(".leadMapPopup").click(function () {
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + CustomerGuidId;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
    var idlist1 = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".MapManufacturerMagnificPrevious", type: 'iframe', width: 920, height: 520 }
    ];
    jQuery.each(idlist1, function (i, val) {
        magnificPopupObj(val);
    });

    $('#FirstName').on('keypress', function (event) {

        if ($(this).val().length == 1) {
            var $this = $(this),
                val = $this.val(),
                regex = /\b[a-z]/g;

            val = val.toLowerCase().replace(regex, function (letter) {
                return letter.toUpperCase();
            });
            $(this).val(val);
        }

    });
    $('#LastName').on('keypress', function (event) {

        if ($(this).val().length == 1) {
            var $this = $(this),
                val = $this.val(),
                regex = /\b[a-z]/g;

            val = val.toLowerCase().replace(regex, function (letter) {
                return letter.toUpperCase();
            });
            $(this).val(val);
        }

    });


    //if (SalesLocation != "") {
    //    $("#SalesLocation").val(SalesLocation);
    //}
    //else {
    //    $("#SalesLocation").val("-1");
    //}
    //if (EmpSalesLocation != '-1' && EmpSalesLocation != '' && SalesLocation == '') {
    //    if (EmpSalesLocation == 'Co_outside-SGN' || EmpSalesLocation == 'Co_outside') {
    //        $('#SalesLocation').val("Company");
    //    }
    //    else {
    //        $('#SalesLocation').val("Inside");
    //    }
    //}
    if (preContactMethod != null && preContactMethod != '') {
        $("#PreferredContactMethod").val(preContactMethod)
    }

    $("#isVerified").val(isVerified);
    $("#ExistingPanel").val(ExistingPanel);
    if (streetType != '') {
        $("#StreetType").val(streetType);
    }

    if ($('#InstallType :selected').val() == "TakeOver") {
        $("#ExistingPanel").show();
    }
    else {
        $("#ExistingPanel").hide();
    }
    InitializeSuburbDropdown($('.dropdown_customar'));
    InitializeContactDropdown($('.dropdown_contact'));
    //var Tabindex = $('#ReferringCustomer').attr(tabindex);
    //$('#select2-ReferringCustomer-container').prop('tabindex', Tabindex);

    if ($('#InstallType :selected').val() == "TakeOver") {
        $("#ExistingPanel").show();
    }
    $("#InstallType").change(function () {
        console.log("sdf");
        if ($('#InstallType :selected').val() == "TakeOver") {
            $("#ExistingPanel").show();
        }
        else {
            $("#ExistingPanel").hide();
        }
    })

    //$('#Carrier').val(LeadsCarrier);
    //$('#PhoneType').val(LeadsPhoneType)
    if (leadsource != null && leadsource != "") {
        $("#LeadSource").val(leadsource);
    }
    $("#btnDeleteLead").click(function () {
        selectedID = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteLead);
    })
    $("#InstallType").filter(function () {
        return $("#InstallType").val() != "null";
    });
    //$('[data-toggle="tooltip"]').tooltip();
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    $("#verifybtn").click(function () {
        LoadVerifyLeads(IDvalue);
    });
    $("#cussetupbtn").click(function () {
        var value = parseInt(IDvalue);
        LoadLeadSetup(value);
    });
    $("#documentbtn").click(function () {
        var id = parseInt(IDvalue);
        LoadDocumentCenter(id);
    });
    $(".addressmap").load(domainurl + "/Leads/LeadVerifyAddressMap/?LeadId=" + IDvalue);
    $("#btnRequestTechLead").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/LeadRequestTechnician/?LeadId=" + IDvalue);
    });

    $("#btnbooklead").click(function () {
        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?CustomerId=" + CustomerGuidId);
    });
});

