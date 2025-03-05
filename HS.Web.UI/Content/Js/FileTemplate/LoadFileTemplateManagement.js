
var fileTemplateChange = function () {
    console.log("test");
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    if ($(LoadCustomerDiv + ".TemplateId").val() > 0) {
        console.log($("#customerId").val());
        OpenTopToBottomModal(domainurl + "/File/LoadFiletemplateForFileManagement/?Id=" + $(LoadCustomerDiv + ".TemplateId").val() + "&customerId=" + CustomerLoadId);
    }
}
var fileTemplateChangeforlead = function () {
    console.log("test");
    console.log(CustomerLoadId);
    console.log($(".TemplateId").val());
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    console.log(LoadCustomerDiv);

    if ($(".TemplateId").val() > 0) {
        console.log($("#customerId").val());
        OpenTopToBottomModal(domainurl + "/File/LoadFiletemplateForFileManagement/?Id=" + $(".TemplateId").val() + "&customerId=" + CustomerLoadId);
    }
}
