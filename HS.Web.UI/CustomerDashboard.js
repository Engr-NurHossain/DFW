var EditCustomerDraft = function (CustomerId) {
    console.log("hi");
    OpenTopToBottomModal("/Customer/AddCustomerDraft?id=" + CustomerId);
}
var EditCredential = function (CustomerId) {
    OpenRightToLeftModal("/Customer/EditCredential?CustomerId=" + CustomerId);
}
var idlist = [{ id: ".TokenPopUp", type: 'iframe', width: 500, height: 500 }];
jQuery.each(idlist, function (i, val) {
    magnificPopupObj(val);
});
var PasswordGenerate = function () {
    $.ajax({

        type: "POST",
        url: "/Customer/GeneratePassword",
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
        url: "/Customer/CheckRoleAndSaveCredential",
        data: '{userlogin: ' + JSON.stringify(userlogin) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response.result);
            
                if (response.result == false && response.timeout == true) {

                    var token = "";
                    parent.OpenTextModal("Give OTP!", response.message, function () {
                        token = $("#CustomerOTP").val()
                        CheckTokenForCredential(token);
                    });


                }
                else if (response.timeout == false) {

                    parent.OpenErrorMessageNew("Error!", response.message);
                }
                else {
                    OpenSuccessMessageNew("Success!", response.message, function () {


                        $("#Right-To-Left-Modal-Body .close").click();
                    });
                }

            
          
    },
        error: function (jqXHR, textStatus, errorThrown) {
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
        url: "/Customer/CheckTokenAndSaveCredential",
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


$(document).ready(function () {


})
