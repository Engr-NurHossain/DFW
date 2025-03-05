var CustomerLeadImportFile = function () {
    OpenRightToLeftModal(domainurl + "/File/AddCustomerLeadImportFile/");
}
var AssignToUser = function () {
    OpenRightToLeftModal("/Leads/AddLeadAssignedToUser");
}


var LoadLeadList = function () {
    setTimeout(function () {
        $(".ListContents").hide();
        $(".ListViewLoader").show();
        $(".ListContents").load(domainurl + "/Leads/LeadsListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
    }, 5);
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");
    //var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: 920, height: 520 }
    //];
    //jQuery.each(idlist, function (i, val) {
    //    magnificPopupObj(val);
    //});
    $(".ListViewLoader").show();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Leads/LeadsListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
            //$(".ListContents").slideDown();
        }, 5);
    });
    setTimeout(function () {
        $(".ListContents").load(domainurl + "/Leads/LeadsListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
        //$(".ListContents").slideDown();
    }, 5);

    
})
