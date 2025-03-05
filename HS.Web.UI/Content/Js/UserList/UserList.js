var DataTablePageSize = 50;
    var ClosePopup = function () {
        $.magnificPopup.close();
    }
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    if (location.href.toLowerCase().indexOf("/recruitment") > -1) {
        LoadRecruit(true);
    }
    //$("#OpenSuccess").click();
    LoadUserList()
}
var DeleteUser = function () {
    var selectedID = [];
    var checkboxs = $('.checkbox-custom');
    for (var i = 0; i < checkboxs.length; i++) {
        if ($(checkboxs[i]).is(":checked")) {
            selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
        }
    }
    for (var i = 0; i < selectedID.length; i++) {
        $.ajax({
            url: domainurl + "/Usermgmt/DeleteUser",
            data: { id: selectedID[i] },
            type: "Post",
            dataType: "Json"
        })
    }
    parent.LoadUserList();
}
var AssignForms = function (userId) {

    if (typeof (userId) == "undefined") {
        return
    }

    OpenRightToLeftModal(domainurl + "/UserMgmt/AssignForms/?UserId=" + userId);

}
var ResendInvitationMail = function (userid) {
    $.ajax({
        url: domainurl + "/Usermgmt/ResendVerificationEmail",
        data: { UserId: userid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, LoadUserMgmt);
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    })
}

var ActivateUser = function (userId) {
    var loadurl = domainurl + "/UserMgmt/ActivateUser/?userid=" + userId;
    var seturl = domainurl + "/UserMgmt/ActivateUser/?userid=" + userId;
    reload = true;
    LoadUrlContents(loadurl, seturl, reload);
}
var PrintUser = function () {
   

    var PdfUrl = "/UserMgmt/GetUserMgmtList?UserGroup=" + $("#UserGroupDropDown").val() + "&SearchText=" + $("#UserSearchText").val() + "&isCurrentEmployee=" + $("#CurrentEmployee").val();
    window.open(PdfUrl, '_blank');

}
var DeactivateUser = function (userId) {
    OpenConfirmationMessageNew("Confirm?", "Do you want to deactivate this user ?", function () {
        $.ajax({
            url: domainurl + "/Usermgmt/DeactivateUser",
            data: { UserId: userId },
            type: "Post",
            dataType: "Json",
            success: function (data) {
                if (data.result) {
                    OpenSuccessMessageNew("Success!", data.message, LoadUserMgmt);
                } else {
                    OpenErrorMessageNew("Error!", data.message);
                }
            }
        });
    });
}


var CurrentEmployeeSessionSet = function () {
    console.log("CurrentEmployeeSessionSet  working");
    var CurrentEmployeeSessionval=$("#CurrentEmployee").val();    
    $.ajax({
        url: domainurl + "/Usermgmt/CurrentEmployeeSessionSet",
        data: { SessionVal: CurrentEmployeeSessionval },
        type: "Post",
        dataType: "Json"
    })
    
    //parent.LoadUserList();
}
var UserListLoad = function (pageNo, order) {
    var usergruopval = $("#UserGroupDropDown").val();
    var searchtextval = $("#UserSearchText").val();
    var currentempval = $("#CurrentEmployee").val();
    if ($.session.get('UserGroupDropDownSessionVal') != "undefined" && $.session.get('UserGroupDropDownSessionVal') != null && $.session.get('UserGroupDropDownSessionVal') != "null") {
        usergruopval = $.session.get('UserGroupDropDownSessionVal');
    }
    if ($.session.get('UserSearchTextSessionVal') != "undefined" && $.session.get('UserSearchTextSessionVal') != null && $.session.get('UserSearchTextSessionVal') != "null") {
        searchtextval = $.session.get('UserSearchTextSessionVal');
    }
    if ($.session.get('CurrentEmployeeSessionVal') != "undefined" && $.session.get('CurrentEmployeeSessionVal') != null && $.session.get('CurrentEmployeeSessionVal') != "null") {
        currentempval = $.session.get('CurrentEmployeeSessionVal');
    }
    if (typeof (pageNo) != "undefined") {
        $(".loader-div").show(),
        $(".content-wrap.custom-head .ListContents").load(domainurl + "/UserMgmt/UserListPartial/?UserGroup="
            + usergruopval
            + "&searchText=" + encodeURI(searchtextval)
            + "&currentemp=" + currentempval
            + "&PageNo=" + pageNo
            + "&Order=" + order);
    }
}

$(document).ready(function () {

    $(".ListViewLoader").hide();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            UserListLoad(1);
        }, 200);
    });

   
    //var table = $('#tblUserList').DataTable({
    //    "pageLength": DataTablePageSize,
    //    "destroy": true,
    //    "language": {
    //        "emptyTable": "No data available"
    //    },
    //    "paging": false
    //});
    $('#UserSearchText').keypress(function (e) {
        if (e.which == 13) {
            $.session.set("UserGroupDropDownSessionVal", $("#UserGroupDropDown").val());
            $.session.set("UserSearchTextSessionVal", $("#UserSearchText").val());
            $.session.set("CurrentEmployeeSessionVal", $("#CurrentEmployee").val());
            UserListLoad(1);
        }
    });
    //$("#UserGroupDropDown").change(function () {
    //    UserListLoad(1);
    //});

    //$("#CurrentEmployee").change(function () {
    //    CurrentEmployeeSessionSet();
    //    UserListLoad(1);
    //});

    $("#ExcelImport").click(function () {
        window.location.href = "/Reports/NewReport/?ReportFor=User&UserGroup=" + $("#UserGroupDropDown").val() + "&FilterUser=" + $("#UserSearchText").val() + "&isSelected=" + $("#CurrentEmployee").val();
    });
    $("#btnSearchUser").click(function () {
        $.session.set("UserGroupDropDownSessionVal", $("#UserGroupDropDown").val());
        $.session.set("UserSearchTextSessionVal", $("#UserSearchText").val());
        $.session.set("CurrentEmployeeSessionVal", $("#CurrentEmployee").val());
        UserListLoad(1);
    });
    $('#tblUserList tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    //$("#srch-term").keyup(function () {
    //    $("#tblUserList_filter input").val($("#srch-term").val());
    //    $("#tblUserList_filter input").trigger('keyup');
    //});
    $("#AddNewManufacturer").click(function () {
        console.log("hlw");
        $(".addManufacturerMagnific").attr("href", domainurl + "/UserMgmt/AddUser/");
        $(".addManufacturerMagnific").click();
    });
    $(".EditOption").click(function () {
        var selectedID = [];
        var checkboxs = $('.checkbox-custom');
        for (var i = 0; i < checkboxs.length; i++) {
            if ($(checkboxs[i]).is(":checked")) {
                selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
            }
        }
        if (selectedID.length > 1) {
            OpenErrorMessageNew("Error!", "You can edit only one customer at a time, please select one customer.");
        }

        else if (selectedID.length <= 0) {
            OpenErrorMessageNew("Error!", "Please select at least one customer")
        }
        else {
            var id = selectedID[0];
            $(".addManufacturerMagnific").attr("href", domainurl + "/UserMgmt/AddUser/" + id);
            $(".addManufacturerMagnific").click();
            parent.LoadUserList();
        }
    });
         
    $(".DeleteOption").click(function () {
        var selectedID = [];
        var checkboxs = $('.checkbox-custom');
        for (var i = 0; i < checkboxs.length; i++) {
            if ($(checkboxs[i]).is(":checked")) {
                selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
            }
        }
        if (selectedID.length > 0) {
            OpenConfirmationMessageNew("Confirm?", "Are you sure to delete user?", DeleteUser);
        }

        else {
            OpenErrorMessageNew("Error!", "Please select at least one customer")
        }
    });

    setTimeout(function () {
        $(".ListContents").slideDown();
    }, 50);
    if (typeof currentemp  != 'undefined')
     $("#CurrentEmployee").val(currentemp);
});
