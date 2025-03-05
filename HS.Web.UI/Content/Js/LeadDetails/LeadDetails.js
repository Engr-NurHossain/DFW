var reminderDate;
var customerId;
var CustomerName;
var CustomerMail;
var Id;
var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}

var MyRequestsCompleted = (function () {
    var numRequestToComplete,
        requestsCompleted,
        callBacks,
        singleCallBack;

    return function (options) {
        if (!options) options = {};

        numRequestToComplete = options.numRequest || 0;
        requestsCompleted = options.requestsCompleted || 0;
        callBacks = [];
        var fireCallbacks = function () {
            for (var i = 0; i < callBacks.length; i++) callBacks[i]();
        };
        if (options.singleCallback) callBacks.push(options.singleCallback);

        this.addCallbackToQueue = function (isComplete, callback) {
            if (isComplete) requestsCompleted++;
            if (callback) callBacks.push(callback);
            if (requestsCompleted == numRequestToComplete) fireCallbacks();
        };
        this.requestComplete = function (isComplete) {
            if (isComplete) requestsCompleted++;
            if (requestsCompleted == numRequestToComplete) fireCallbacks();
        };
        this.setCallback = function (callback) {
            callBacks.push(callBack);
        };
    };
})();
var LeadToCustomer = function (CustomerId, IntId, ProceedWithoutPaymentMethod) {
    var url = domainurl + "/SmartLeads/ConvertLeadToCustomer/";
    var param = JSON.stringify({
        CustomerIdStr: CustomerId,
        Id: IntId,
        ProceedWithoutPaymentMethod: ProceedWithoutPaymentMethod
    });
    $.ajax({
        url: url,
        data: param,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            console.log(data);
            $(".lead_details_container").removeClass("hidden");
            $(".lead_setup_partial_loader").addClass("hidden");
            if (data.result == false && typeof (data.noPayment) != "undefined") {
                OpenConfirmationMessageNew("", data.message, function () {
                    LeadToCustomer(CustomerId, IntId, true);
                });
            }
            else if (data.result == true) {
                //OpenSuccessMessageNew("Success", "Converted to customer successfully.", function () {
                RecurringBillsCreate(CustomerId, IntId);
                window.location.href = "/Customer/Customerdetail/?id=" + data.CID;
                //});

                //LoadCustomerDetail(data.CID, true);
            }
        }
    });
}
//var ShareCustomerInfo = function (CustomerId) {
//    var SendEmailUrl = "";
//    SendEmailUrl = domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId;
//    console.log("Email link created.:" + CustomerId);
//    OpenTopToBottomModal(domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId);

//    //$("#customerInfoShare").attr("href", SendEmailUrl);
//}
var AddCorrespondenceEmail = function (CusId) {
    OpenRightToLeftModal(domainurl + "/Leads/MailToSalesPerson/?id=" + CusId + "&Cid=0");
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addCorrespondence");
}

var RecurringBillsCreate = function (StrCustomerId, IntId) {
    var url = domainurl + "/RecurringBilling/CreateRMRByLeadToCustomerConvertion/";
    var param = JSON.stringify({
        CustomerId: StrCustomerId,
        Id: IntId
    });
    $.ajax({
        url: url,
        data: param,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            console.log(data);
        }
    });
}
var GetFinanceStatus = function () {

    var url = domainurl + "/SmartLeads/GetIsPcFinanceStatus";
    var param = JSON.stringify({
        CustomerId: CustomerLoadGuid,
        ApplicationId: IsPcAppId,

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
            console.log(data);
            if (data == false) {
                OpenErrorMessageNew("Error!", "Something Wrong");
            }
            else {
                OpenSuccessMessageNew("Success!", data.Message, function () {

                    window.location.href = "/Lead/Leadsdetail/?id=" + LeadId;
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {

    $(".emailTextTemplateList").hide();
    $(".slaesPersonContainer").hide();

    $(".leadfinance_status_button").click(function () {
        GetFinanceStatus();
    })

    $('#IsSalesPerson').click(function () {
        $(".slaesPersonContainer").toggle(this.checked);
    });

    reminderDate = new Pikaday({ format: 'MM/DD/YYYY', field: $('#CustomerNoteReminderDate')[0] });

    $(".back-to-leadList").click(function () {
        LoadLeads(true);
    });

    var FollowUpTavContentCustomerId = $(".FollowUpTabContent").attr('id-val')

    $(".FollowUpTabContent").load(domainurl + "/Leads/LoadLeadFollowUpTabPartial/?CustomerId=" + FollowUpTavContentCustomerId);

    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");
    //var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: 920, height: 520 }
    //];
    //jQuery.each(idlist, function (i, val) {
    //    magnificPopupObj(val);
    //});
    $('#editCustomerCustomerDetailsHeader').click(function () {
        var id = $(this).attr('idval');
        console.log(id);
        //$(".addManufacturerMagnific").attr("href", "/Leads/AddLeads?id=" + id);
        //$(".addManufacturerMagnific").click();
        LoadLeadVerificationInfo(id);
    });
    $('#editCustomerCustomerDetailsHeader1').click(function () {
        console.log("dhukss");
        var id = $(this).attr('idval');
        if (LeadV2 == '' || LeadV2 == null || LeadV2 == 'undefined') {
            LeadV2 = $(this).attr('version');
        }
        console.log(id);
        if (LeadV2 == "V2") {
            LoadLeadInfov2(id);

        }
        else {
            LoadLeadVerificationInfo(id);

        }

        //LoadFormGeneration(id, true);
        //LoadLeadVerificationInfo(id);
    });
    $('#editCustomerCustomerDetailsHeader2').click(function () {
        var id = $(this).attr('idval');
        console.log(id);
        LoadFormGeneration(id, true);
    });
    $(".leadToCustomerConvert").click(function () {
        var customerId = $(this).attr('idval');
        var CustomerName = $(this).attr('idval1');
        CustomerMail = $(this).attr('idval2');
        var Id = $(this).attr('data-id');
        var PassEmailCheck = false;
        if (CheckEmailWhenConvertingToCustomer.toLowerCase() == "true" && CustomerMail != "") {
            PassEmailCheck = true;
        } else if (CheckEmailWhenConvertingToCustomer.toLowerCase() != "true") {
            PassEmailCheck = true;
        }
        if (CustomerName != "" && PassEmailCheck) {
            OpenConfirmationMessageNew("Confirm?", ConvertLeadToCustomerMsg, function () {
                LeadToCustomer(customerId, Id, false);
                $(".lead_details_container").addClass("hidden");
                $(".lead_setup_partial_loader").html(TabsLoaderText);
            });
        }
        else {

            OpenErrorMessageNew("Error!", ConvertLeadToCustomerErrorMsg, "");
        }
    });
    $("#MailToSupplier").click(function () {

        if ($("#IsSalesPerson").is(":checked")) {

            var customerName = $("#MailToSupplier").attr('idval');
            var customerId = $("#MailToSupplier").attr('idval2');
            var supplierId = $('.sales-person-id').val();
            var mailBody = $('.txtAreaMailBody').val();
            var subject = $('#SubjectList').val();
            var SalesPersonListLeadEmailArray = $('#SalesPersonList').val();
            if (mailBody == "" || subject == "") {
                OpenErrorMessageNew("Error!", "Sorry! Please fill Mail Body and Mail Subject");
            }
            else {
                $.ajax({
                    url: domainurl + "/Leads/MailToSalesperson/",
                    data: { customerName, customerId, supplierId, mailBody, subject, SalesPersonListLeadEmailArray },
                    type: "Post",
                    dataType: "Json"
                }).done(function () {
                    OpenSuccessMessageNew("Success!", "Email sent successfully.");
                    parent.LoadLeads(true);
                });
            }

        }
        else if ($("#IsSalesPerson").is(":not(:checked)")) {

            var customerName = $("#MailToSupplier").attr('idval');
            var customerId = $("#MailToSupplier").attr('idval2');
            var mailBody = $('.txtAreaMailBody').val();
            var subject = $('#SubjectList').val();

            if (mailBody == "" || subject == "") {
                OpenErrorMessageNew("Error!", "Sorry! Please fill Mail Body and Mail Subject.");
            }
            else {
                $.ajax({
                    url: domainurl + "/Leads/MailToLeadByCurrentLogInuser/",
                    data: { customerId, customerName, mailBody, subject },
                    type: "Post",
                    dataType: "Json"
                }).done(function () {
                    OpenSuccessMessageNew("Success!", "Email sent successfully.");
                    parent.LoadLeads(true);
                });
            }
        }
    });
    //$(".btn-leadnote").click(function () {
    //    var Lnote = $("#Leadnote").val();
    //    console.log(Lnote);
    //    $.ajax({
    //        url: "/Leads/UpdateLeadNotes/",
    //        data: { Lnote, customerid },
    //        type: "Post",
    //        dataType: "Json"
    //    }).done(function () {
    //        if (Lnote != "") {
    //            OpenSuccessMessage ("Success!", "Successfully added to Customer Note", "");
    //            $("#Leadnote").val("");
    //        }
    //        else {
    //            OpenErrorMessageNew("Error!", "Needed to add some text", "");
    //        }
    //    });
    //})

    $(".back-to-leadList").click(function () {
        LoadLeads(true);
    });
    $("#IsTestAccount").change(function () {
        var Param = {
            CustomerId: CustomerLoadGuid,
            IsTestAccount: $("#IsTestAccount").prop('checked') ? true : false
        };
        $.ajax({
            type: "POST",
            url: domainurl + "/Customer/UpdateTestAccountChecked",
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    OpenSuccessMessageNew("Success!", "");
                    location.reload();
                }
                else {
                    OpenErrorMessageNew("Error!","");
                }
            }
        });
    });
    //$(".LoadLeadNotes").load("/Leads/LeadNotesPartial/?customerid=" + customerid);
    //$(".lead-history-join").load("/Leads/LeadJoiningHistoryPartial/?CustomerId=" + customerid);
    //$(".lead-history-notes").load("/Leads/LeadNotesHistoryPartial/?CustomerId=" + customerid);
    //$(".lead-history-followUps").load("/Leads/LeadFollowUpHistoryPartial/?CustomerId=" + customerid);
    //$(".lead-history-email").load("/Leads/LeadEmailHistoryPartial/?CustomerId=" + customerid);
});
