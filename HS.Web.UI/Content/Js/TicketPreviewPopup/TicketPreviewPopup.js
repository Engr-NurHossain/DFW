
var printFrame = function (id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}
//var LoadHtmlForMobile = function () {
//    //var Url = domainurl + "/Public/CustomerInvoiceHtml/?Code=" + Token;
//    var Url = domainurl + "/Ticket/PrintTicket/?Id=" + ViewBagId + "&TicketId=" + ViewBagTicketId;
//    $("#AggrementDivMobile").load(Url);
//}
//$(document).ready(function () {
//    console.log("ticketPrint");
//    LoadHtmlForMobile();
//});