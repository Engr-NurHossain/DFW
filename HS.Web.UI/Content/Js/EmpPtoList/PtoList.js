
var CheckPtoHours = function (item,PtoTime,RemainigTime) {
    var requestHour = PtoTime / 60;
    var Message = "";
 
    var remainingHour;
    if (RemainigTime != null)
    {
       remainingHour = RemainigTime - requestHour;
    }
    else {
        remainingHour = 0 - requestHour;
    }
      
    if (remainingHour < 0) {
        Message = "Warning: Hours requested exceeds available PTO hours.Current Pto hour:" + RemainigTime;
        $("#tooltipmsg_" + item).html("");
        $("#tooltipmsg_" + item).html(Message);
        $(".tooltiparea").addClass("payable_info_hover")
        $(".payable_tooltip_div").css("right", "0px");
        $(".payable_tooltip_div").css("left", "unset");
        $(".payable_tooltip_div").addClass("reject_custom")
        $(".payable_tooltip_div").css("background-color", "red");
    }

    else {
        $(".tooltiparea").removeClass("payable_info_hover")

    } 
}
var checkUserPtoHour = function (item, PtoTime, RemainigTime) {
    var requestHour = PtoTime / 60;

    var MessageUser = "";
    var remainingHour;
    if (RemainigTime != null && typeof (RemainigTime) != "undefined") {
        remainingHour = RemainigTime - requestHour;
    }
    else {
        remainingHour = 0 - requestHour;
        RemainigTime = "Not set";
    }
    if (remainingHour < 0) {
        MessageUser = "Current PTO hour: " + RemainigTime;
        $("#tooltipmsgUser_" + item).html("");
        $("#tooltipmsgUser_" + item).html(MessageUser);
        $(".tooltipareaUser").addClass("payable_info_hover");
        $(".payable_tooltip_div").css("right", "0px");
        $(".payable_tooltip_div").css("background-color", "red");
    }

    else {
        MessageUser = "Current PTO hour: " + RemainigTime;
        $("#tooltipmsgUser_" + item).html("");
        $("#tooltipmsgUser_" + item).html(MessageUser);
        $(".tooltipareaUser").addClass("payable_info_hover");
        $(".payable_tooltip_div").css("right", "0px");
        $(".payable_tooltip_div").css("background-color", "green");

    }
}

$(document).ready(function () {
  
   
});