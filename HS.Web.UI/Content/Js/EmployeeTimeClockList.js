var LastUpdatedTimeClock = function (item, Name, date) {
    console.log(Name);
    if (Name.length > 0) {
        var MessageUser = Name + " Updated on" + date;
        $("#tooltipmsgUser_" + item).html("");
        $("#tooltipmsgUser_" + item).html(MessageUser);
        $(".tooltipareaUser").addClass("payable_info_hover");
        // $(".payable_tooltip_div").css("right", "-100px");
        $(".payable_tooltip_div").css("background-color", "green");
    }
    else {
        $(".tooltipareaUser").removeClass("payable_info_hover");
    }
}
var DeleteEmployeeTimeClock = function (id) {
    OpenConfirmationMessageNew("Confirmation", "Are you sure, you want to delete this item?", function () {
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: "/TimeClockPto/DeleteEmployeeTimeClock",
            data: JSON.stringify({ id: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result) {
                    $(".employeetimeclock-load").load(domainurl + String.format("/TimeClockPto/EmployeeTimeClock/?UserId={0}", data.empid));
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    })
}
$(document).ready(function () {
    var idlist = [{ id: ".ClockMapPopUp", type: 'iframe', width: 500, height: 500 }];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".OpenMapPopup").click(function () {
        var latval = $(this).attr('data-lat');
        var lngval = $(this).attr('data-lng');
        var mapLoadUrl = domainurl+"/App/OpenPosition/?lat=" + latval + "&lng=" + lngval;
        $(".ClockMapPopUp").attr("href", mapLoadUrl);
        $(".ClockMapPopUp").click();
    });
    $(".icon_sort_timeclock").click(function () {
        var orderval = $(this).attr('data-val');
        FilterPayrollPaging(pageno, orderval);
    })
})