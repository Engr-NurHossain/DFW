var DataTablePageSize = 50;
var UserData = "";
var HideAll = function () {
    $(".UserInputForm").addClass('hidden');//p2
    $(".CustomPermissionDiv").addClass('hidden');//p3
    $(".FinishDiv").addClass('hidden');//p4
    $(".choosePermissionGroup").addClass('hidden');//p1
}
var SaveUser = function () {
    var selectedID = [];
    var checkboxs = $('.checkbox-custom');
    for (var i = 0; i < checkboxs.length; i++) {
        if ($(checkboxs[i]).is(":checked")) {
            selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
        }
    }
    var url = domainurl + "/UserMgmt/AddUser/";
    var param = JSON.stringify({
        Id: $("#UserLogin_Id").val(),
        fName: $("#UserLogin_FirstName").val(),
        lName: $("#UserLogin_LastName").val(),
        title: $("#UserLogin_Title").val(),
        email: $("#UserLogin_UserName").val(),
        plist: selectedID,
        pGroup: $('input[name=PermissionGroup]:checked').val(),
        IsServiceCall: $("#IsServiceCall").is(':checked'),
        IsInstaller: $("#IsInstaller").is(':checked'),
        IsSalesPerson: $("#IsSalesPerson").is(':checked'),
        branchId: parseInt($("#branchId").val()),
        SendEmail: $("#SendEmailNotification").is(":checked"),
        RouteId: encodeURI($("#Route").val())
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
            if (data.result == true) {
                $(".LoadImgDiv").removeClass('hidden');
                setTimeout(function () {
                    parent.ClosePopupGiveSuccess();
                }, 2000);
            } else {
                parent.ClosePopupGiveError();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var IsUserExists = function () {
    var url = domainurl + "/UserMgmt/IsUserExists/";
    var param = JSON.stringify({
        email: $("#UserLogin_UserName").val(),
        Id: $("#UserLogin_Id").val()
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
            if (data == false) {
                $(".userexistsmsg").addClass('hidden');
            } else {
                $(".userexistsmsg").removeClass('hidden');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var GotToFinishPage = function () {
    var url = domainurl + "/UserMgmt/IsUserExists/";
    var param = JSON.stringify({
        Id: $("#UserLogin_Id").val(),
        email: $("#UserLogin_UserName").val(),
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
            if (data == false) {
                HideAll();
                $(".FinishDiv").removeClass('hidden');
                if ($("#SendEmailNotification").prop("checked") == true) {
                    $("#FinishMsgTxt").text(withEmailText);
                }
                else {
                    $("#FinishMsgTxt").text(withOutEmailText);
                }
                $(".userexistsmsg").addClass('hidden');
            } else {
                $(".userexistsmsg").removeClass('hidden');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var EmailValidationCheck = function () {
    if ($("#UserLogin_UserName").val() != $("#UserLogin_EmailAddress").val()) {
        $(".ConfirmEmail").removeClass('hidden');
        return false;
    } else {
        $(".ConfirmEmail").addClass('hidden');
        return true;
    }
}

/*var InitPermissions = function () {
    if ((typeof (UserpermissionList) != "undefined" && UserpermissionList.length > 0)) {
        UserpermissionList.forEach(function (item) {
            $('#checkbox-' + item).prop('checked', true);
        });
    }
}
var IsServiceCallClick = function (val) {
    ServiceCallPermissionList.forEach(function (item) {
        $('#checkbox-' + item).prop('checked', val);
    });
}
var IsInstallerClick = function (val) {
    InstallerPermissionList.forEach(function (item) {
        $('#checkbox-' + item).prop('checked', val);
    });
}
var IsSalesPersonClick = function (val) {
    SalesPersonPermissionList.forEach(function (item) {
        $('#checkbox-' + item).prop('checked', val);
    });
}
*/
var InitUsers = function () {
    if ($("#UserLogin_Id").val() > 0 && $("#UserLogin_UserName").val() != "") {
        $("#UserLogin_EmailAddress").val($("#UserLogin_UserName").val());
        $("#UserLogin_UserName").prop("disabled", true);
        $("#UserLogin_EmailAddress").prop("disabled", true);
    }
}
$(document).ready(function () {
    $(".add_user_content_scroll").height(window.innerHeight - 103);
    var table = $('#tblVideo').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });

    $(".CancelBtn").click(function () {
        parent.ClosePopup();
    });
    $("#NextBtn").click(function () {
        if (typeof ($('input[name=PermissionGroup]:checked').val()) == "undefined") {
            return;
        }
        var checkboxval = $('input[name=PermissionGroup]:checked').val();

        if (checkboxval == "Custom") {
            $(".CustomPermissionDiv").removeClass('hidden');
            $(".choosePermissionGroup").addClass('hidden');
        }
        else {
            $(".choosePermissionGroup").addClass('hidden');
            $(".UserInputForm").removeClass('hidden');
        }
        //if (checkboxval == "Regular" || checkboxval == "Admin" || checkboxval == "Reports") {
        //    $(".choosePermissionGroup").addClass('hidden');
        //    $(".UserInputForm").removeClass('hidden');
        //} else if (checkboxval == "Custom") {
        //    $(".CustomPermissionDiv").removeClass('hidden');
        //    $(".choosePermissionGroup").addClass('hidden');
        //}
        $("#UserCreateTitleDivId").html('Add User Details');
    });
    $("#UserLogin_EmailAddress").blur(function () {
        EmailValidationCheck();
    });
    $("#BackBtnP2").click(function () {
        $(".UserInputForm").addClass('hidden');//p2
        $(".CustomPermissionDiv").addClass('hidden');//p3
        $(".FinishDiv").addClass('hidden');//p4
        $(".choosePermissionGroup").removeClass('hidden');
        $("#UserCreateTitleDivId").html('Choose User Type');
    });
    $("#BackBtnP3").click(function () {
        $(".UserInputForm").addClass('hidden');//p2
        $(".CustomPermissionDiv").addClass('hidden');//p3
        $(".FinishDiv").addClass('hidden');//p4
        $(".choosePermissionGroup").removeClass('hidden');
        $("#UserCreateTitleDivId").html('Add User Details');
    });
    $("#NextBtnP2").click(function () {
        if (CommonUiValidation() && EmailValidationCheck()) {
            GotToFinishPage();
            $("#UserCreateTitleDivId").html('Finish Adding User');
        }
    });
    $("#NextBtnP3").click(function () {
        HideAll();
        $(".UserInputForm").removeClass('hidden');
    });
    $("#UserLogin_UserName").blur(function () {
        IsUserExists();
    });
    $("#FinishBtn").click(function () {
        $(".ChooseUserTypeHeader").hide();
        $(".FinishDiv").hide();
        SaveUser();
    });
    $("#BackBtnP4").click(function () {
        var checkboxval = $('input[name=PermissionGroup]:checked').val();
        if (checkboxval == "Regular" || checkboxval == "Admin" || checkboxval == "Reports") {
            HideAll();
            $(".UserInputForm").removeClass('hidden');
        } else if (checkboxval == "Custom") {
            HideAll();
            $(".CustomPermissionDiv").removeClass('hidden');
        }
        $(".UserInputForm").removeClass('hidden');//p2
        $(".CustomPermissionDiv").addClass('hidden');//p3
        $(".FinishDiv").addClass('hidden');//p4
        $(".choosePermissionGroup").addClass('hidden');
        $("#UserCreateTitleDivId").html('Add User Details');
    });
    /*$("#IsSalesPerson").change(function () {
        if ($(this).is(':checked')) {
            IsSalesPersonClick(true);
        } else {
            IsSalesPersonClick(false);
        }

    });
    $("#IsInstaller").change(function () {
        if ($(this).is(':checked')) {
            IsInstallerClick(true);
        } else {
            IsInstallerClick(false);
        }

    });
    $("#IsServiceCall").change(function () {
        if ($(this).is(':checked')) {
            IsServiceCallClick(true);
        } else {
            IsServiceCallClick(false);
        }

    });*/
    /*InitPermissions();*/
    InitUsers();
});
$(document).on('click', '.tree label', function (e) {
    $(this).next('ul').fadeToggle();
    e.stopPropagation();
});
$(document).on('change', '.tree input[type=checkbox]', function (e) {
    $(this).siblings('ul').find("input[type='checkbox']").prop('checked', this.checked);
    $(this).parentsUntil('.tree').children("input[type='checkbox']").prop('checked', this.checked);
    e.stopPropagation();
});