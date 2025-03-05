var EditCustomerDraft = function (CustomerId) {
    OpenTopToBottomModal(domainurl + "/CustomerPublic/AddCustomerDraft?id=" + CustomerId);
}
var EditCredential = function (CustomerId) {
    OpenRightToLeftModal(domainurl + "/Customer/EditCredential?CustomerId=" + CustomerId);
}


var idlist = [{ id: ".TokenPopUp", type: 'iframe', width: 500, height: 500 }];
jQuery.each(idlist, function (i, val) {
    magnificPopupObj(val);
});
var PasswordGenerate = function () {
    $.ajax({

        type: "POST",
        url: domainurl + "/Customer/GeneratePassword",
        data: '{length: ' + JSON.stringify("8") + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#password").val("");
            $("#password").val(response);
        }
    });
}
var CredentialSave = function () {
    parent.OpenTextModal("Give Verbal Password!", "Give your verbal password here.", function () {
        token = $("#CustomerOTP").val()
        checkTokenAndCredentialSave(token);

    });
}
var checkTokenAndCredentialSave = function (token) {
    var userlogin = {};
    userlogin.UserName = $("#username").val();
    userlogin.Password = $("#password").val();
    userlogin.Id = $("#Userlogin").val();
    userlogin.UserId = $("#CustomerId").val();
    userlogin.EmailAddress = $("#Email").val();
    if ($('#SendMailAddress').is(':checked') == true) {
        userlogin.SendMail = "true";
    }
    else {
        userlogin.SendMail = "false";
    }
    $.ajax({
        type: "POST",
        url: domainurl + "/Customer/CheckRoleAndSaveCredential",
        data: '{userlogin: ' + JSON.stringify(userlogin) + ',Token: '+JSON.stringify(token)+'}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.result == false) {

                OpenErrorMessageNew("Error!", response.message, function () {
                    $("#Right-To-Left-Modal-Body .close").click();
                });
            }
            
            else {
                OpenSuccessMessageNew("Success!", response.message, function () {
                    $("#Right-To-Left-Modal-Body .close").click();
                });
            }
    }, error: function (jqXHR, textStatus, errorThrown) {
            OpenErrorMessageNew("Error!", errorThrown);
        }
    });
}

var CheckTokenForCredential = function (token) {
    var userlogin = {};
    userlogin.UserName = $("#username").val();
    userlogin.Password = $("#password").val();
    userlogin.Id = $("#Userlogin").val();
    userlogin.UserId = $("#CustomerId").val();
    userlogin.EmailAddress = $("#Email").val();
    userlogin.Token = token;
    if ($('#SendMailAddress').is(':checked') == true) {
        userlogin.SendMail = "true";
    }
    else {
        userlogin.SendMail = "false";
    }

    $.ajax({
        type: "POST",
        url: domainurl + "/Customer/CheckTokenAndSaveCredential",
        data: '{userlogin: ' + JSON.stringify(userlogin) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response.result);
            if (response.result) {
                OpenSuccessMessageNew("Success!", response.message, function () {
                    $("#Right-To-Left-Modal-Body .close").click();
                });
            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
        }
    });

}

var OpenTicketTab = function () {
    $(".customer-options-tabs li").removeClass('active');
    $(".tab_Content_customer_items .tab-pane").removeClass('active');
    $(".TicketTab").removeClass('hidden');
    $(".TicketTab").addClass('active');
    $(".TicketTab_Load").addClass('active');
    $(".TicketTab_Load").html(TabsLoaderText);
    $(".TicketTab_Load").load(domainurl + "/CustomerPublic/TicketListPartial/?CustomerId=" + CustomerGuid);
}
var OpenCustomerDetailTab = function (e) {
    CustomerLoadId = $(e.target).attr('data-id');
     
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".CustomerDetailTab").removeClass('hidden');
    $(LoadCustomerDiv + ".CustomerDetailTab").addClass('active');
    $(LoadCustomerDiv + ".CustomerDetailTab_Load").addClass('active');
}
var OpenInvoiceTab = function () {
    $(".customer-options-tabs li").removeClass('active');
    $(".tab_Content_customer_items .tab-pane").removeClass('active');
    $(".InvoiceTab").removeClass('hidden');
    $(".InvoiceTab").addClass('active');
    $(".InvoiceTab_Load").addClass('active');
    $(".InvoiceTab_Load").html(TabsLoaderText);
    $(".InvoiceTab_Load").load(domainurl + "/Invoice/InvoicePartial/?CustomerId=" + CustomerGuid);
}

var OpenAnnouncementTab = function () {
    console.log("hlw");
    $(".customer-options-tabs li").removeClass('active');
    $(".tab_Content_customer_items .tab-pane").removeClass('active');
    $(".AnnouncementTab").removeClass('hidden');
    $(".AnnouncementTab").addClass('active');
    $(".AnnouncementTab_Load").addClass('active');
    $(".AnnouncementTab_Load").html(TabsLoaderText);
    $(".AnnouncementTab_Load").load(domainurl + "/CustomerPublic/ShowCustomerAnnouncement");
}

var OpenReferFriendTab = function () {
    console.log("hlw");
    $(".customer-options-tabs li").removeClass('active');
    $(".tab_Content_customer_items .tab-pane").removeClass('active');
    $(".ReferFriendTabTab").removeClass('hidden');
    $(".ReferFriendTabTab").addClass('active');
    $(".ReferFriendTab_Load").addClass('active');
    $(".ReferFriendTab_Load").html(TabsLoaderText);
    $(".ReferFriendTab_Load").load(domainurl + "/CustomerPublic/ShowReferedFriend");
}
$(document).ready(function () {
    //var LoadCustomerDiv = ".row custom_padding_for_row";
    //var ResponsibleGUID = $(LoadCustomerDiv + ".Responsible-Person-Histories").attr('idval');
  
    $(".Responsible-Person-Histories").load(domainurl + "/Customer/CustomerResponsiblePersonDetail?customerId=" + CustomerGuid);
    $(".Billing-Histories").load(domainurl + "/Customer/CustomerBillingHistories?customerId=" + CustomerGuid);


    $(".customer-file-content").load(domainurl + "/Customer/CustomerFileDetails?customerid=" + CustomerGuid);


    $(".history-JoinDate").load(domainurl + "/Customer/CustomerCreatedHistory?customerId=" + CustomerGuid);


    $(".lnvoice-Histories").load(domainurl + "/Customer/CustomerInvoiceHistories?customerId=" + CustomerGuid);


    $(".Estimate-Histories").load(domainurl + "/Customer/CreateEstimateHistoryPartial?CustomerId=" + CustomerGuid);


    $(".Payment-Histories").load(domainurl + "/Customer/InvoicePaymentHistoryPartial?CustomerId=" + CustomerGuid);

   
    $(".ServiceOrder-Histories").load(domainurl + "/Customer/CustomerServiceOrderHistories?customerId=" + CustomerGuid);


    $(".WorkOrder-Histories").load(domainurl + "/Customer/CustomerWorkOrderHistories?customerId=" + CustomerGuid);

    $(".System-Info-History").load(domainurl + "/Customer/CustomerSystemInfoDetails?customerId=" + CustomerGuid);


 
    $(".Notes-Box").load(domainurl + "/Customer/CustomerNoteBoxes?customerId=" + CustomerGuid);

    //$(".Orders-Box").load(domainurl + "/Customer/CustomerOrderBoxes?customerId=" + CustomerGuid);


    $(".Account-Activity-History").load(domainurl + "/Customer/CustomerAccountActivityDetails?customerId=" + CustomerGuid);



    $(".Responsible-Person-Histories").load(domainurl + "/Customer/CustomerResponsiblePersonDetail?customerId=" + CustomerGuid);

  
    $(".Api-Setting-Histories").load(domainurl + "/Customer/CustomerApiSettingDetail?customerId=" + CustomerGuid);



    $(".CustomerSendEmailHistory").load(domainurl + "/Customer/CustomerSendEmailHistory?CustomerId=" + CustomerGuid);
})
