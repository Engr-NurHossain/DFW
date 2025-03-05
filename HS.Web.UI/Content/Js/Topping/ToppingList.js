var pageno = '@ViewBag.PageNumber';
var OpenToppingById = function (toppingId) {
    console.log("edit topping");
    OpenTopToBottomModal(domainurl + "/MenuManagement/AddTopping/?ToppingCategoryId=" + toppingId);
}
var ToppingListLoad = function (pageNo, order) {
    if (typeof (pageNo) != "undefined") {

        //var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        //if (typeof (IsLead) != "undefined" && IsLead) {
        //    LoadCustomerDiv = "";
        //}
        //var AssignedSearch = $(LoadCustomerDiv + ".Assigned_search").val();
        //var TicketTypeSearch = $(LoadCustomerDiv + ".TicketType_Search").val();
        //var TicketStatusSearch = $(LoadCustomerDiv + ".Ticket_Status_Search").val();
        //var MyTicketSearch = $(LoadCustomerDiv + ".TicketFor_search").val();

        var LoadUrl = domainurl + "/MenuManagement/LoadToppingsPartial/?PageNo=" + pageNo
            + "&SearchText=" + encodeURI($(".topping_search_text").val())
            + "&order=" + order
        $(".Load_Toppings").html(TabsLoaderText);
        $(".Load_Toppings").load(LoadUrl);
    }
}

$(document).ready(function () {
    //var TicketLoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    //if (typeof (IsLead) != "undefined" && IsLead) {
    //    TicketLoadCustomerDiv = "";
    //}
    //$('[data-toggle="tooltip"]').tooltip();

    $(".btnAddNewTopping").click(function () {
        OpenTopToBottomModal(domainurl + "/MenuManagement/AddTopping");
    });
    $(".search_topping_btn").click(function () {
        ToppingListLoad(1);
    });

    $('.topping_search_text').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            ToppingListLoad(1);
        }
    });
    $(".icon_sort_timeclock").click(function () {
        var orderval = $(this).attr('data-val');
        ToppingListLoad(pageno, orderval);
    })
    //$("#TicketReport").click(function () {
    //    ColumnName = "";
    //    var ids = "";
    //    var CustomerId = "";
    //    var flag = 0;
    //    console.log("fff");
    //    CustomerId = $(".CheckItems").attr('data-customerid');
    //    $('.ticket_export').each(function () {
    //        if ($(this).attr('data-info') != "" && $(this).attr('data-info') != undefined && $(this).attr('data-info') != null) {
    //            ColumnName += $(this).attr('data-info').trim() + "," + $(this).text().trim();
    //        }
    //    });
    //    window.open(domainurl + "/Reports/NewReport/?ColumnNames=" + ColumnName + "&ReportFor=Ticket&CustomerId=" + CustomerId, "_blank");
    //});
});