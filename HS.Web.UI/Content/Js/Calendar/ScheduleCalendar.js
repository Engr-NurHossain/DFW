var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var dateheadercalendar;
var EditDateHeader = function () {
    var dateheader = "<div class='' style='float:left;'><input class='form-control' id='sche_date_header_calendar' /></div><div style='float:left'><button class='btn' id='btn_close_date_header' onclick='CloseDateHeader()' title='Cancel'><i class='fa fa-close'></i></button></div><div class='' style='float:left;'><button class='btn' id='btn_sche_date_header' onclick='SaveDateHeader()' title='Set Date'><i class='fa fa-save'></i></button></div>";
    $(".sche_event_header h2").html(dateheader);
    $("#sche_date_header_calendar").val(startdate);
    dateheadercalendar = new Pikaday({ format: 'MM/DD/YYYY', field: $('#sche_date_header_calendar')[0], firstDay: 1 });
}
var CloseDateHeader = function () {
    $(".sche_event_header h2").html(DateTitle);
    $(".sche_event_header h2").append("<div style='float:right;margin-left:10px;cursor:pointer;' class='edit_date_header_schedule'><i class = 'fa fa-edit' onclick='EditDateHeader()'></i></div>");
}
$(document).ready(function () {
    $(".selectpicker_user").selectpicker('val', UserVal);
    $(".selectpicker_type").selectpicker('val', typeval);
    if (pageno != null && pageno != 0) {
        pageno = parseInt(pageno) + 1;
    }
    if (pageno1 != null && pageno1 != 0) {
        pageno1 = parseInt(pageno1) + 1;
    }

    $(".sche_event_header h2 div").remove();
    $(".sche_event_header h2").html(DateTitle);

    $(".sche_event_header h2").append("<div style='float:right;margin-left:10px;cursor:pointer;' class='edit_date_header_schedule'><i class = 'fa fa-edit' onclick='EditDateHeader()'></i></div>");
})