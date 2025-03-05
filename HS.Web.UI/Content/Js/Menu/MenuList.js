var pageno = '@ViewBag.PageNumber';
var OpenMenuById = function (menuId) {
    OpenTopToBottomModal(domainurl + "/MenuManagement/AddMenu/?Id=" + menuId);
}
var MenuListLoad = function (pageNo, order) {
    if (typeof (pageNo) != "undefined") {

        //var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        //if (typeof (IsLead) != "undefined" && IsLead) {
        //    LoadCustomerDiv = "";
        //}
        //var AssignedSearch = $(LoadCustomerDiv + ".Assigned_search").val();
        //var TicketTypeSearch = $(LoadCustomerDiv + ".TicketType_Search").val();
        //var TicketStatusSearch = $(LoadCustomerDiv + ".Ticket_Status_Search").val();
        //var MyTicketSearch = $(LoadCustomerDiv + ".TicketFor_search").val();

        var LoadUrl = domainurl + "/MenuManagement/LoadMenusPartial/?PageNo=" + pageNo
            + "&SearchText=" + encodeURI($("#searchtext").val())
        +
         "&order=" + order
        $(".Load_Menus").html(TabsLoaderText);
        $(".Load_Menus").load(LoadUrl);
        //var LoadUrl = domainurl + "/MenuManagement/LoadMenusPartial?PageNo=" + 1 + "&PageSize=50" + "&SearchText=" + encodeURI(searchtext)
        //    + "&order=" + order
        //$(".Load_Menus").html(TabsLoaderText);
        //$(".Load_Menus").load(LoadUrl);
    }
}
var OrderListLoad = function (pageNo, order) {
    if (typeof (pageNo) != "undefined") {

        //var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        //if (typeof (IsLead) != "undefined" && IsLead) {
        //    LoadCustomerDiv = "";
        //}
        //var AssignedSearch = $(LoadCustomerDiv + ".Assigned_search").val();
        //var TicketTypeSearch = $(LoadCustomerDiv + ".TicketType_Search").val();
        //var TicketStatusSearch = $(LoadCustomerDiv + ".Ticket_Status_Search").val();
        //var MyTicketSearch = $(LoadCustomerDiv + ".TicketFor_search").val();

        var LoadUrl = domainurl + "/Order/LoadOrdersPartial/?PageNo=" + pageNo
            + "&SearchText=" + encodeURI($("#searchtext").val())
            +
            "&order=" + order
        $(".Load_Orders").html(TabsLoaderText);
        $(".Load_Orders").load(LoadUrl);
        //var LoadUrl = domainurl + "/MenuManagement/LoadMenusPartial?PageNo=" + 1 + "&PageSize=50" + "&SearchText=" + encodeURI(searchtext)
        //    + "&order=" + order
        //$(".Load_Menus").html(TabsLoaderText);
        //$(".Load_Menus").load(LoadUrl);
    }
}
$(document).ready(function () {
    //var TicketLoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    //if (typeof (IsLead) != "undefined" && IsLead) {
    //    TicketLoadCustomerDiv = "";
    //}
    //$('[data-toggle="tooltip"]').tooltip();

    $(".btnAddNewMenu").click(function () {
        OpenTopToBottomModal(domainurl + "/MenuManagement/AddMenu");
    });
    $(".search_menu_btn").click(function () {
        MenuListLoad(1);
    });

    $('.menu_search_text').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            MenuListLoad(1);
        }
    });
    $(".icon_sort_timeclock").click(function () {
        var orderval = $(this).attr('data-val');
        MenuListLoad(pageno, orderval);
        OrderListLoad(pageno, orderval);
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
