var DuplicateCustomer = function (type, value) {
    var Param = {
        "Type": type,
        "Value": value,
        "CustomerId": Id
    };
    var url = domainurl + "/Customer/DuplicateCustomer";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                if (parseInt(data.CustomerId) > 0) {
                    var anchor = "/Customer/Customerdetail/?id=" + data.CustomerId;
                    if (type == "BusinessName") {
                        $(".ExistHrefBusinessName").text(data.CustomerId);
                        $(".ExistHrefBusinessName").attr("href", anchor);

                        $("#BusinessNameExist").removeClass("hidden");
                    }
                    else if (type == "Phone") {
                        $(".ExistHrefPhone").text(data.CustomerId);
                        $(".ExistHrefPhone").attr("href", anchor);

                        $("#PhoneExist").removeClass("hidden");
                    }
                    else if (type == "Email") {
                        $(".ExistHrefEmail").text(data.CustomerId);
                        $(".ExistHrefEmail").attr("href", anchor);

                        $("#EmailExist").removeClass("hidden");
                    }
                }
            }
            else {
                if (type == "BusinessName") {
                    $("#BusinessNameExist").addClass("hidden");
                }
                else if (type == "Phone") {
                    $("#PhoneExist").addClass("hidden");
                }
                else if (type == "Email") {
                    $("#EmailExist").addClass("hidden");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}

function phone_validate(phno) {
    var regexPattern = new RegExp(/^[0-9-+]+$/);    // regular expression pattern
    return regexPattern.test(phno);
}
var CheckPrimaryPhone = function (CustomerId) {
    var phonenumber = $("#PrimaryPhone").val().replace(/[\s-()]/g, '').trim();
    var FormatedPhoneNo = $("#PrimaryPhone").val();
    ChackPhoneNumberValidity(phonenumber, "PrimaryPhone", CustomerId, FormatedPhoneNo);
}
var CheckCellPhone = function (CustomerId) {
    var phonenumber = $("#CellNo").val().replace(/[\s-()]/g, '').trim();
    var FormatedPhoneNo = $("#CellNo").val();
    ChackPhoneNumberValidity(phonenumber, "CellNo", CustomerId, FormatedPhoneNo);
}
var CheckSecondaryPhone = function (CustomerId) {
    var phonenumber = $("#SecondaryPhone").val().replace(/[\s-()]/g, '').trim();
    var FormatedPhoneNo = $("#SecondaryPhone").val();
    ChackPhoneNumberValidity(phonenumber, "secondaryPhone", CustomerId, FormatedPhoneNo);
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
                    if (phone_Type == "secondaryPhone") {
                        $("#isCellPhoneVerified").val("true");
                        $(".notVerifiedCellPhoneNumber").addClass("hidden");
                        $(".verifiedCellPhoneNumber").removeClass("hidden");
                    }
                    if (phone_Type == "CellNo") {
                        $("#isCellNoVerified").val("true");
                        $(".notVerifiedCellNumber").addClass("hidden");
                        $(".verifiedCellNumber").removeClass("hidden");
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
                    if (phone_Type == "secondaryPhone") {
                        $("#isCellPhoneVerified").val("false");
                        $(".notVerifiedCellPhoneNumber").removeClass("hidden");
                        $(".verifiedCellPhoneNumber").addClass("hidden");
                    }
                    if (phone_Type == "CellNo") {
                        $("#isCellNoVerified").val("false");
                        $(".notVerifiedCellNumber").removeClass("hidden");
                        $(".verifiedCellNumber").addClass("hidden");
                    }
                    OpenErrorMessageNew("", "Phone number not valid!");
                }
            }
        });
    }
    else {
        console.log("fdf");
        if (phone_Type == "PrimaryPhone") {
            $("#isSitePhoneVerified").val("false");
            $(".notVerifiedSitePhoneNumber").removeClass("hidden");
            $(".verifiedSitePhoneNumber").addClass("hidden");
        }
        if (phone_Type == "secondaryPhone") {
            $("#isCellPhoneVerified").val("false");
            $(".notVerifiedCellPhoneNumber").removeClass("hidden");
            $(".verifiedCellPhoneNumber").addClass("hidden");
        }
        if (phone_Type == "CellNo") {
            $("#isCellNoVerified").val("false");
            $(".notVerifiedCellNumber").removeClass("hidden");
            $(".verifiedCellNumber").addClass("hidden");
        }
        OpenErrorMessageNew("", "Phone number not valid!")
    }

}
/*this one should be fixed*/
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var Nonval = $("#InorActivebtn").attr('idval');
var LoadEmergencyContactList = function () {
    $("#EmergencyContactList").load(domainurl + "/Customer/CustomerEmergencyContactListPartial?Id=" + CustomerGuidId);
}
var LoadAddNewEmergencyContactDiv = function () {
    $("#AddNewEmgContactPartial").show();
    $("#AddNewEmgContactPartial").load(domainurl + "/Customer/AddCustomerEmergencyContact?EmgConId=" + CustomerGuidId + "&EmgId=0");
}
var EditEmergencyContactDiv = function (emgId) {
    $("#AddNewEmgContactPartial").show();
    $("#AddNewEmgContactPartial").load(domainurl + "/Customer/AddCustomerEmergencyContact?EmgConId=" + CustomerGuidId + "&EmgId=" + emgId);
}
var SetSecCuNo = function () {
    $("#SecondCustomerNo").val();
}
var SetFCuNo = function () {
    $("#CustomerNum").val();
}
var SetAddCuNo = function () {
    $("#AdditionalCustomerNo").val();
}
var FixAddCustomerWrapperHeight = function () {
    var CustomHeight = window.height - $('.cus-section').height() + 30;
    if (Device.MobileGadget()) {
        CustomHeight = window.height - $('.cus-section').height() + 70;
    }
    $(".add_customer_wrapper").height(CustomHeight);
}
var HideNewEmgContactPartial = function () {
    $("#AddNewEmgContactPartial").hide();
}
var FunctionActiveOrInactiveCustomer = function (cval) {
    $.ajax({
        url: domainurl + "/Customer/ConvertInactiveOrActiveCustomer/",
        data: { Nonval, cval },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.res == true) {
                CloseTopToBottomModal();
                parent.LoadCustomerDetail(data.Customerid, true);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}
var DeleteCustomerById = function (customerid) {
    console.log("delete");
    $.ajax({
        url: domainurl + "/Customer/DeleteCustomer",
        data: { Id: customerid, CusId: CustomerGuidId, CusIntId: Id },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Deleted successfully!", function () {
                    console.log("deleted callback executed");
                    CloseTopToBottomModal();
                    window.location.href = domainurl + "/Customer";
                });
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
};


var LoadScheduleCalendar = function () {
    var Param = {
        "Id": $("#Ticket_Id").val(),
        "TicketId": $("#Ticket_TicketId").val(),
        "Subject": $("#Ticket_Subject").val(),
        "Status": $("#Ticket_Status").val(),
        "CustomerId": $("#CustomerList").val(),
        "TicketType": $("#Ticket_TicketType").val(),
        "CompletionDate": GetTimeFormat($("#Ticket_CompletionDate").val()),
        "AppointmentStartTime": $("#AppointmentStartTime").val(),
        "AppointmentEndTime": $("#AppointmentEndTime").val(),
    };
    var url = domainurl + "/Ticket/SaveTicketSession";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                var pathname = window.location.pathname.toLowerCase();
                if (pathname == '/scheduleinfo') {
                    window.location.href = "/ScheduleInfo?date=" + $("#Ticket_CompletionDate").val() + "&viewtype=" + "Daily" + "&TicketId=" + $("#Ticket_Id").val() + "&CustomerId=" + GuidCustomer;
                }
                else {
                    window.location.href = "/calendar?ticketdate=" + $("#Ticket_CompletionDate").val() + "&ticketid=" + $("#Ticket_Id").val() + "&customerid=" + GuidCustomer;
                }
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
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
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
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
                    console.log(data.score);
                    if (data.Score != "" && data.Score != "undefined") {
                        OpenSuccessMessageNew("", "Credit score report generated successfully.")
                    }
                    else {
                        OpenErrorMessageNew("", "Credit score not generated for some missing or invalid field.")
                    }

                }
                else {
                    OpenErrorMessageNew("", "Internal Error!");
                }

            }
        });
   
}

$(document).ready(function () {

    var idlist = [
        { id: ".SecondaryContactMagnific", type: 'iframe', width: 550, height: 420 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#BusinessName").blur(function () {
        DuplicateCustomer("BusinessName", $(this).val());
    });
    $("#PrimaryPhone").blur(function () {
        DuplicateCustomer("Phone", $(this).val());
    });
    $("#EmailAddress").blur(function () {
        DuplicateCustomer("Email", $(this).val());
    });

    $("#btnSchedule").click(function () {
        LoadScheduleCalendar();
    })

    //if (SalesLocation != '') {
    //    $('#SalesLocation').val(SalesLocation);
    //}
    //else {
    //    $('#SalesLocation').val('-1');
    //}


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

    $("#CreditRecordpdf").click(function () {
        console.log("dsf");
        var pdfUrl = "/File/DownloadCreditCheckPdf?id=" + ReportPdfId;
        console.log(pdfUrl);
        window.open(pdfUrl);
    });

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
            var LoadUrl = domainurl + "/SmartPackageSetup/GetAllOtherCustomerContact?CustomerId=" + CustomerGuidId + "&IsSoftCheck=true&Bureau=EFX&For=CreditCheck";
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
    if (HasCreditCheck == "false") {
        $("#CreditScore").hide();

    }

    $("#checkEmail").click(function () {
        url = domainurl + "/Customer/VerifyEmail/";
        var param = {
            id: OpenedCustomerIntId,
            CustomerId: CustomerGuidId,
            EmailAddress: $("#EmailAddress").val(),
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
                console.log("data" + data);
                $(".loader-div").hide();
                if (data.result == false) {

                    $("#verifiedemail").addClass("hidden");
                    $("#notverified").removeClass("hidden");

                    OpenErrorMessageNew("Error!", data.message, function () {
                    });
                }
                else {
                    $("#verifiedemail").removeClass("hidden");
                    $("#notverified").addClass("hidden");

                    OpenSuccessMessageNew("Success!", data.message, function () {
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    });
    //if (EmpSalesLocation != '-1' && EmpSalesLocation != '' && SalesLocation == '') {
    //    if (EmpSalesLocation == 'Co_outside-SGN' || EmpSalesLocation == 'Co_outside') {
    //        $('#SalesLocation').val("Company");
    //    }
    //    else {
    //        $('#SalesLocation').val("Inside");
    //    }
    //}

    $("#ACH_BankAccountType").val(BankAccountTypeVal);
    $("#ACH_ECheckType").val(ECheckTypeVal);
    var CompletionDatepicker = new Pikaday({
        field: $('#Ticket_CompletionDate')[0],
        format: 'MM/DD/YYYY'
    });
    $('.selectpicker').selectpicker();
    InitializeSuburbDropdown($('.dropdown_customar'));
    InitializeContactDropdown($('.dropdown_contact'));
    InitializeSuburbDropdownChild($('.dropdown_customar_child'));


    $("#btnQa1").click(function () {
        OpenTopToBottomModal(domainurl + "/Customer/QA1QuestionariePartial?id=" + OpenedCustomerIntId);
    })
    $("#btnQa2").click(function () {
        OpenTopToBottomModal(domainurl + "/Customer/QA2QuestionariePartial?id=" + OpenedCustomerIntId);
    })
    $("#btnTech").click(function () {
        OpenTopToBottomModal(domainurl + "/Customer/LeadTechCallPartial?id=" + OpenedCustomerIntId);
    })
    var cancelBodyTemplate = "Reason:" + "<textarea>" + "</textarea>";

    //$("#InorActivebtn").click(function () {
    //    if (IsCustomerActive == "True") {
    //        OpenCancelCustomer(cancelHeaderTemplate, cancelBodyTemplate, function () {
    //            cval = $("#CustomerCancel").val();
    //            if (cval != "") {
    //                FunctionActiveOrInactiveCustomer(cval)
    //            }
    //            else {
    //                OpenErrorMessageNew("Error!", "Please write reason to rolled over customer", "");
    //            }
    //        })
    //    }
    //    else {
    //        OpenConfirmationMessageNew("Confirm?", "Are you want to activate customer?", function () {
    //            FunctionActiveOrInactiveCustomer("true");
    //        });
    //    }
    //})

    $("#InorActivebtn").click(function () {
        if (IsCustomerActive == "True") {
            OpenTopToBottomModal("/Customer/CustomerCancellationConfirm?CustomerId=" + CustomerGuidId);
        }
        else {
            OpenConfirmationMessageNew("Confirm?", "Are you want to activate customer?", function () {
                FunctionActiveOrInactiveCustomer("true");
            });
        }
    });
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".LoadCancellationPopUp", type: 'iframe', width: Popupwidth, height: 650 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#ResendCancellationFile").click(function () {
        $("#CancellationAgreement").click();
    });
    FixAddCustomerWrapperHeight();
    if (FundCompany != null && FundCompany != '') {
        $("#FundingCompany").val(FundCompany);
    }
    if (CustomerFund != '') {
        var Fundval = CustomerFund == "True" ? '1' : '0';
        $("#CustomerFunded").val(Fundval);
    }
    if (CustomerMaintenance != '') {
        var Mainval = CustomerMaintenance == "True" ? '1' : '0';
        $("#Maintenance").val(Mainval);
    }
    //if (CustomerBilltax != '') {
    //    var Taxval = CustomerBilltax == "True" ? '1' : '0';
    //    $("#BillTax").val(Taxval);
    //}
    if (CreditScoreVal != '') {
        $("#CreditScore").val(CreditScoreVal);
    }
    if (MonthlyMonitoringFeeVal != '') {
        $("#MonthlyMonitoringFee").val(MonthlyMonitoringFeeVal);
    }
    $(".customer-system-info").load(domainurl + "/Customer/CustomerSystemInfoPartial?id=" + cusSysId + "&CusID=" + cussysCusid + "&ComID=" + cussysComid);

    $(".customer-system-info-alarm").load(domainurl + "/API/CustomerSystemInfoList?CustomerId=" + CustomerGuidId);

    $(document).click(function () {
        setTimeout(function () {
            $(".customerNoSuggestion").hide();
            $(".customerNoSuggestion2").hide();
            $(".customerNoSuggestion3").hide();
        }, 200);

    })
    if (preContactMethod != null && preContactMethod != '') {
        $("#PreferredContactMethod").val(preContactMethod)
    }
    if (PaymentMethod != null && PaymentMethod != '') {
        $("#PaymentMethod").val(PaymentMethod);
    }
    if (BillingCycle != null && BillingCycle != '') {
        $("#BillCycle").val(BillingCycle);
    }
    if (valuetab != "") {
        $('.previousAddress').find('a').trigger("click");
    }

    LoadEmergencyContactList();
    $("#btnCustomerAddNewEmgContact").click(function () {
        LoadAddNewEmergencyContactDiv();
    });
    $("#DeleteThisCustomer").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete?", function () {
            DeleteCustomerById(CustomerGuidId)
        });
    });
    setTimeout(function () {
        if (IsGotoBill == '1') {
            $("#CustoerEditNav").find('ul').find('li#GoToBill').addClass('active').find('a').get(0).click();
        }
    }, 200);
    setTimeout(function () {
        if (IsSystemInfo == '1') {
            $("#CustoerEditNav").find('ul').find('li#GoToSystem').addClass('active').find('a').get(0).click();
        }
    }, 200);
    setTimeout(function () {
        if (AlarmRefId != "") {
            $("#CustoerEditNav").find('ul').find('li#GoToAlarmSystem').addClass('active').find('a').get(0).click();
        }
       

    }, 200);
    setTimeout(function () {
        if (IsAccountActivity == '1') {
            $("#CustoerEditNav").find('ul').find('li#GoAccountActivity').addClass('active').find('a').get(0).click();
        }
    }, 200);
    setTimeout(function () {
        if (IsEmergency == '1') {
            $("#CustoerEditNav").find('ul').find('li#GoEmergency').addClass('active').find('a').get(0).click();
        }
    }, 200);
    setTimeout(function () {
        if (IsEmergency != '1' && IsAccountActivity != '1' && IsSystemInfo != '1' && IsGotoBill != '1') {
            $("#CustoerEditNav").find('ul').find('li#GotoContactInformation').addClass('active').find('a').get(0).click();
        }
    }, 200);

    $('#CustoerEditNav a').click(function (item) {
        $('#CustoerEditNav li').removeClass('active');
        $($(item.target).parent()).addClass('active');
    });
    $(".add_customer_wrapper_custom").on("scroll", function () {
        var windscroll = $(".add_customer_wrapper_custom").scrollTop();
        $('#CustoerEditNav a').each(function (i) {

            console.log($($(this).attr("href")));
            if ($($(this).attr("href")).position().top <= 85) {
                $('#CustoerEditNav li').removeClass('active');
                $('#CustoerEditNav li').eq(i).addClass('active');
            }
        });
    });
    $("body").on('click', function (evt) {
        console.log('Add Customer body click');
        var clickClass = $(evt.target || evt.srcElement).attr("class");
        if (clickClass != "add_customer_top_menu_drop" && clickClass != "fa fa-bars" && clickClass != "add_customer_top_menu_btn") {
            $(".add_customer_top_menu_drop").animate({
                left: "-100%"
            }, 300, function () {
                $(".add_customer_top_menu_drop").hide();
            });
        }
    });
    $(".add_customer_top_menu_btn").click(function () {
        $(".add_customer_top_menu_drop").animate({
            left: "0"
        }, 300, function () {
            $(".add_customer_top_menu_drop").show();
        });
    });

    if (typeof (leadsourceval) != "undefined" && leadsourceval != null && leadsourceval != "" && leadsourceval != "-1") {
        console.log(leadsourceval);
        $("#LeadSource").val(leadsourceval);
    }

    if (typeof (CustomerStatus) != "undefined" && CustomerStatus != null && CustomerStatus != "" && CustomerStatus != "-1") {
        console.log(CustomerStatus);
        $("#CustomerStatus").val(CustomerStatus);
    }

    if (typeof (InstalledStatusval) != "undefined" && InstalledStatusval != null && InstalledStatusval != "" && InstalledStatusval != "-1") {
        console.log(InstalledStatusval);
        $("#InstalledStatus").val(InstalledStatusval);
    }
    $("#SalesLocation").val(SalesLocation);
    $("#SoldBy").change(function () {
        console.log("test");
        $("#SalesLocation").val($("#SoldBy").find(':selected').attr('salesloc'));
        $("#SalesLocation").select2();
    });
});