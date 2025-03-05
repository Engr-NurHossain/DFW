var LoadcustomerList = function () {
        setTimeout(function () {
            $(".ListContents").hide();
            $(".ListViewLoader").show();
            $(".ListContents").load(domainurl + "/Supplier/SupplierListAndBillPartial");
        }, 500);
    }
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide();
    //$("#LoadManufacturers").addClass("active");
    //var newsupplierpopwinowwith = 920;
    //var newsupplierpopwinowheight = 655;
    //if (Device.MobileGadget()) {
    //    console.log("blabla");
    //    newsupplierpopwinowwith = window.innerWidth;
    //    newsupplierpopwinowheight = window.innerHeight;
    //}
    //var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: 920, height: 655, }
    //];
    //jQuery.each(idlist, function (i, val) {
    //    magnificPopupObj(val);
    //});

    //$(".ListViewLoader").show();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Supplier/SupplierListAndBillPartial");
            //$(".ListContents").slideDown();
        }, 200);
    });
    setTimeout(function () {
        $(".ListContents").load(domainurl + "/Supplier/SupplierListAndBillPartial");
        //$(".ListContents").slideDown();
    }, 500);
    
})
