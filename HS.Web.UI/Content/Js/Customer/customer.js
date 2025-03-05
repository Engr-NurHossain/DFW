var firstdate = typeof ($(".min-date").val()) != "undefined" && $(".min-date").val() != "" && $(".min-date").val() != "01/01/0001" ? $(".min-date").val() : "";
var lastdate = typeof ($(".max-date").val()) != "undefined" && $(".max-date").val() != "" && $(".max-date").val() != "01/01/0001" ? $(".max-date").val() : "";
var LoadcustomerList = function () {
    setTimeout(function () {
        $(".ListContents").hide();
        $(".ListViewLoader").show();
        $(".ListContents").load(domainurl + "/Customer/CustomersListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
    }, 500);
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");
    var customerpopwinowwith = 920; 
    var customerpopwinowheight = 600;
    if (Device.All()) {
        customerpopwinowwith = window.innerWidth;
        customerpopwinowheight = window.innerHeight;
    }
    var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: customerpopwinowwith, height: customerpopwinowheight }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".ListViewLoader").show();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Customer/CustomersListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
            //$(".ListContents").slideDown();
        }, 200);
    });
    console.log('start date');
    $(".ListContents").load(domainurl + "/Customer/CustomersListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
 


});