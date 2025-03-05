var AddCorrespondenceEmail = function (CusId) {
    OpenRightToLeftModal(domainurl + "/Leads/MailToSalesPerson/?id=" + CusId + "&Cid=0");
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addCorrespondence");
}
var AddCorrespondenceSms = function (CusId) {
    OpenRightToLeftModal(domainurl + "/Leads/SMSToSalesPerson/?id=" + CusId + "&Cid=0");
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addCorrespondence");
}
$(document).ready(function () {
    var CorrespondenceTable = $('#tblCorrespondence').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        },
        "order": [[1, "desc"]]
    });
    CorrespondenceTable.order([1, 'desc']).draw();
    $(".LoadMailOrSms").click(function () {
        var typeval = $(this).attr('data-id');
        if (typeval == "Email") {
            var idval = $(this).attr('idval');
            OpenRightToLeftModal(domainurl + "/Leads/MailToSalesPerson/?id=" + Leadid + "&Cid=" + idval);
        }
        else {
            var idval = $(this).attr('idval');
            OpenRightToLeftModal(domainurl + "/Leads/SMSToSalesPerson/?id=" + Leadid + "&Cid=" + idval);
        }
    })
    $("#tblCorrespondence_wrapper").find('.row').css("margin-left", "0");
    $("#tblCorrespondence_wrapper").find('.row').css("margin-right", "0");
    $("#tblCorrespondence_wrapper").find('.col-sm-12').css("padding-left", "0");
    $("#tblCorrespondence_wrapper").find('.col-sm-12').css("padding-right", "0");
    $("#tblCorrespondence_wrapper").find('.col-sm-7').css("padding-right", "0");
})