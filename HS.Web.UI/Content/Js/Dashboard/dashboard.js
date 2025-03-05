
//var LoadCustomerView = function () {
//    $(".dashbord-customer-content-list").load("/Customer/CustomerViewList/?recent=false");
//}
var OpenDashboardReport = function () {
    OpenTopToBottomModal("/app/DashboardReport");
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    //$(".MainContentDiv").slideDown(4000);
    //LoadCustomerView();
});