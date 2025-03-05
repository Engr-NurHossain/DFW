var OpenFileTemplate = function (templateId) {
    OpenTopToBottomModal(domainurl + "/File/AddFileTemplate/?Id=" + templateId);
}
var AddFileTemplate = function () {
    OpenTopToBottomModal(domainurl + "/File/AddFileTemplate/");
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addFile");
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
});