function printFrame(id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}

$(document).ready(function () {
    var data1 = $("#User").val();
    var data2 = $("#Source").val();
    var data3 = $("#First").val();
    var data4 = $("#Last").val();
    console.log(data1);
    $("#iframePdf").attr('src', domainurl + "/Customer/CustomerListPDF/?data1=" + data1 + "&data2=" + data2 + "&data3=" + data3 + "&data4=" + data4);
})