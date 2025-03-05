var ClosePopup = function () {
    $.magnificPopup.close();
}
var CustomerSystemNumberKeyUp = function (pageNumber) {
    if (typeof (pageNumber) == "undefined") {
        return;
    }
    $(".ListContents").load(domainurl + "/Customer/CustomerSystemNoListPartial/?PageNo=" + pageNumber + "&PageSize=" + 0);
}
var LoadcustomerList = function () {
    setTimeout(function () {
        $(".ListContents").hide();
        $(".ListViewLoader").show();
        //$(".ListContents").load("/Customer/CustomerSystemNoListPartial");
        CustomerSystemNumberKeyUp(1);
    }, 500);
}
var width = 650;
var height = 650;
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");

    if (window.innerWidth < 769)
    {
        width = window.innerWidth;
        height = window.innerHeight;
    }
   
    var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: width, height: height }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".ListViewLoader").show();
    setTimeout(function () {
        //$(".ListContents").load("/Customer/CustomerSystemNoListPartial");
        CustomerSystemNumberKeyUp(1);
        //$(".ListContents").slideDown();
    }, 500);
    $("#AddCustomerSystemNo").click(function () {
        $(".addManufacturerMagnific").click();
    });
    $("#AddCustomerSystemNoPrefix").click(function () {
        LoadCustomerSystemNoPrefix();
    });
});

