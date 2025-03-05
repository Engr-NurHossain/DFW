
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
var LoadUserList = function () {
    var usergruopval = "";
    var searchtextval = "";
    var currentempval = "";
    //if ($.session.get('UserGroupDropDownSessionVal') != "undefined" && $.session.get('UserGroupDropDownSessionVal') != null && $.session.get('UserGroupDropDownSessionVal') != "null") {
    //    usergruopval = $.session.get('UserGroupDropDownSessionVal');
    //}
    //if ($.session.get('UserSearchTextSessionVal') != "undefined" && $.session.get('UserSearchTextSessionVal') != null && $.session.get('UserSearchTextSessionVal') != "null") {
    //    searchtextval = $.session.get('UserSearchTextSessionVal');
    //}
    //if ($.session.get('CurrentEmployeeSessionVal') != "undefined" && $.session.get('CurrentEmployeeSessionVal') != null && $.session.get('CurrentEmployeeSessionVal') != "null") {
    //    currentempval = $.session.get('CurrentEmployeeSessionVal');
    //}
    $(".ListContents").html("");
    $(".ListViewLoader").show();
    $(".ListContents").load(domainurl + "/UserMgmt/UserListPartial/?UserGroup=" + usergruopval + "&searchText=" + encodeURI(searchtextval) + "&currentemp=" + currentempval +"&PageNo=" + 1);
}

$(document).ready(function () {  
    $(".LoaderWorkingDiv").hide(); 
    var addmanufacpopwinowwith = 600;
    var addmanufacpopwinowheight = 575;
    if (Device.MobileGadget()) {
        addmanufacpopwinowwith = window.innerWidth;
        addmanufacpopwinowheight = window.innerHeight;
    }
    var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: addmanufacpopwinowwith, height: addmanufacpopwinowheight }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".ListViewLoader").show();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            var usergruopval = "";
            var searchtextval = "";
            var currentempval = "";
            //if ($.session.get('UserGroupDropDownSessionVal') != "undefined" && $.session.get('UserGroupDropDownSessionVal') != null && $.session.get('UserGroupDropDownSessionVal') != "null") {
            //    usergruopval = $.session.get('UserGroupDropDownSessionVal');
            //}
            //if ($.session.get('UserSearchTextSessionVal') != "undefined" && $.session.get('UserSearchTextSessionVal') != null && $.session.get('UserSearchTextSessionVal') != "null") {
            //    searchtextval = $.session.get('UserSearchTextSessionVal');
            //}
            //if ($.session.get('CurrentEmployeeSessionVal') != "undefined" && $.session.get('CurrentEmployeeSessionVal') != null && $.session.get('CurrentEmployeeSessionVal') != "null") {
            //    currentempval = $.session.get('CurrentEmployeeSessionVal');
            //}
            $(".ListContents").load(domainurl + "/UserMgmt/UserListPartial/?UserGroup=" + usergruopval + "&searchText=" + encodeURI(searchtextval) + "&currentemp=" + currentempval + "&PageNo=" + 1);
        }, 200);
    });
    setTimeout(function () {
        var usergruopval = "";
        var searchtextval = "";
        var currentempval = "";
        //if ($.session.get('UserGroupDropDownSessionVal') != "undefined" && $.session.get('UserGroupDropDownSessionVal') != null && $.session.get('UserGroupDropDownSessionVal') != "null") {
        //    usergruopval = $.session.get('UserGroupDropDownSessionVal');
        //}
        //if ($.session.get('UserSearchTextSessionVal') != "undefined" && $.session.get('UserSearchTextSessionVal') != null && $.session.get('UserSearchTextSessionVal') != "null") {
        //    searchtextval = $.session.get('UserSearchTextSessionVal');
        //}
        //if ($.session.get('CurrentEmployeeSessionVal') != "undefined" && $.session.get('CurrentEmployeeSessionVal') != null && $.session.get('CurrentEmployeeSessionVal') != "null") {
        //    currentempval = $.session.get('CurrentEmployeeSessionVal');
        //}
        $(".ListContents").load(domainurl + "/UserMgmt/UserListPartial/?UserGroup=" + usergruopval + "&searchText=" + encodeURI(searchtextval) + "&currentemp=" + currentempval + "&PageNo=" + 1);
    }, 500);
})
