var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
var SearchText = "";


var my_date_format = function (input) {
    console.log(input + " r");
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();

    return (date);
};


var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
//var BillingListLoad = function (pageNo, order) {

//        var Search = encodeURI($(".RBSearchText").val());
//        if (typeof (pageNo) != "undefined" && pageNo > 0) {
//            var LoadUrl = domainurl + "/RecurringBilling/RecurringBillingListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + Search;
//            $(".loadeRBlist").html(TabsLoaderText);
//            $(".loadeRBlist").load(LoadUrl);
//    }
//}



$(document).ready(function () {

    $("#RBSearchbtn").click(function () {
        //BillingListLoad(1, null);
    });
    $(".btn-add-RecurringBilling").click(function () {
        OpenTopToBottomModal(domainurl + "/RecurringBilling/AddRecurringBilling?customerid=" + CustomerLoadId);
    });
});