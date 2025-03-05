
var ClearUnusedDom = function (ContactId) {
    var LoadCustomerDiv = "#contact_tab_" + ContactLoadId + " ";
    $(LoadCustomerDiv + ".tab-pane").each(function () {
        if (!$(this).hasClass('ContactDetailTab_Load')) {
            if (!$(this).hasClass('active')) {
                $(this).html(TabsLoaderText)
            }
        }

    });
}
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var OpenCorrespondenceTab = function () {
    console.log("ff");

    var LoadContactDiv = "#contact_tab_" + ContactLoadId + " ";
    $(LoadContactDiv + ".contact-options-tabs li").removeClass('active');
    $(LoadContactDiv + ".tab_Content_contact_items .tab-pane").removeClass('active');
    $(LoadContactDiv + ".CorrespondenceTab").removeClass('hidden');
    $(LoadContactDiv + ".CorrespondenceTab").addClass('active');
    $(LoadContactDiv + ".CorrespondenceTab_Load").addClass('active');
    $(LoadContactDiv + ".CorrespondenceTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadContactDiv + ".CorrespondenceTab_Load").load(domainurl + "/Contact/CorrespondenceList?ContactId=" + ContactLoadGuid);
    //UpdateCustomerTabCounter();
}
var OpenContactDetailTab = function () {
    console.log("ff");
    var LoadContactDiv = "#contact_tab_" + ContactLoadId + " ";
    $(LoadContactDiv + ".contact-options-tabs li").removeClass('active');
    $(LoadContactDiv + ".tab_Content_contact_items .tab-pane").removeClass('active');
    $(LoadContactDiv + ".ContactDetailTab").removeClass('hidden');
    $(LoadContactDiv + ".ContactDetailTab").addClass('active');
    $(LoadContactDiv + ".ContactDetailTab_Load").addClass('active');
    //$(LoadCustomerDiv + ".CustomerDetailTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    //$(LoadCustomerDiv + ".CustomerDetailTab_Load").load("/Customer/Customerdetail/?id=" + CustomerLoadId);
}