var pageno = '@ViewBag.PageNumber';
var OpenCategoryById = function (categoryId) {
    OpenTopToBottomModal(domainurl + "/MenuManagement/AddCategory/?Id=" + categoryId);
}
var CategoryListLoad = function (pageNo, order) {
    if (typeof (pageNo) != "undefined") {

        //var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        //if (typeof (IsLead) != "undefined" && IsLead) {
        //    LoadCustomerDiv = "";
        //}
        //var AssignedSearch = $(LoadCustomerDiv + ".Assigned_search").val();
        //var TicketTypeSearch = $(LoadCustomerDiv + ".TicketType_Search").val();
        //var TicketStatusSearch = $(LoadCustomerDiv + ".Ticket_Status_Search").val();
        //var MyTicketSearch = $(LoadCustomerDiv + ".TicketFor_search").val();

        var LoadUrl = domainurl + "/MenuManagement/LoadCategoriesPartial/?PageNo=" + pageNo
            + "&SearchText=" + encodeURI($(".category_search_text").val())
            + "&order=" + order
        $(".Load_Categories").html(TabsLoaderText);
        $(".Load_Categories").load(LoadUrl);
    }
}

$(document).ready(function () {
    //var TicketLoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    //if (typeof (IsLead) != "undefined" && IsLead) {
    //    TicketLoadCustomerDiv = "";
    //}
    //$('[data-toggle="tooltip"]').tooltip();

    $(".btnAddNewCategory").click(function () {
        OpenTopToBottomModal(domainurl + "/MenuManagement/AddCategory");
    });
    $(".search_category_btn").click(function () {
        CategoryListLoad(1);
    });

    $('.category_search_text').keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            CategoryListLoad(1);
        }
    });
    $(".icon_sort_timeclock").click(function () {
        var orderval = $(this).attr('data-val');
        CategoryListLoad(pageno, orderval);
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