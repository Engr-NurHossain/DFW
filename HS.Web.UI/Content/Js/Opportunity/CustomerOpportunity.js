var EditOpportunity = function (id) {
    OpenTopToBottomModal(domainurl + "/Opportunity/AddOpportunity?id=" + id + "&CustomerId=" + CustomerLoadGuid + "&opportunityTab=" + opportunityTab);
}

var NavigatePageListing = function (pagenumber, order) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    var searchText = $(LoadCustomerDiv + ".opportunity_serach_item").val();
    $(LoadCustomerDiv + ".ListContents").load(domainurl + "/Opportunity/LoadCustomerOpportunityList", { PageNumber: pagenumber, SearchText: searchText, Order: order, CustomerId: CustomerLoadGuid });
}

var OpenNewOpportunity = function () {
    OpenTopToBottomModal(domainurl + "/Opportunity/AddOpportunity?id=0&CustomerId=" + customerid + "&opportunityTab=" + opportunityTab);
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addOpportunity");
}
$(document).ready(function () {
    NavigatePageListing(1, null);
});