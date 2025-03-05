var date = new Date();
var Currentdate = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
var Currenttime = date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
var tt = "AM";
var h = date.getHours();
if (h >= 12) {
    h = h - 12;
    tt = "PM";
}
if (h == 0) {
    h = 12;
}
Currenttime = Currenttime + " " + tt;
var ctz = date.getTimezoneOffset();;
function getTimezoneName() {
    var timeZoneHiddenField;

    if (720 == ctz) { timeZoneHiddenField = 'Dateline Standard Time'; }
    else if (660 == ctz) { timeZoneHiddenField = 'Samoa Standard Time'; }
    else if (660 == ctz) { timeZoneHiddenField = 'Hawaiian Standard Time'; }
    else if (480 == ctz) { timeZoneHiddenField = 'Alaskan Standard Time'; }
    else if (420 == ctz) { timeZoneHiddenField = 'Pacific Standard Time'; }
    else if (420 == ctz) { timeZoneHiddenField = 'US Mountain Standard Time'; }
    else if (360 == ctz) { timeZoneHiddenField = 'Central America Standard Time'; }
    else if (300 == ctz) { timeZoneHiddenField = 'Central Standard Time'; }
    else if (300 == ctz) { timeZoneHiddenField = 'SA Pacific Standard Time'; }
    else if (240 == ctz) { timeZoneHiddenField = 'Eastern Standard Time'; }
    else if (270 == ctz) { timeZoneHiddenField = 'Venezuela Standard Time'; }
    else if (240 == ctz) { timeZoneHiddenField = 'SA Western Standard Time'; }
    else if (240 == ctz) { timeZoneHiddenField = 'Central Brazilian Standard Time'; }
    else if (180 == ctz) { timeZoneHiddenField = 'Atlantic Standard Time'; }
    else if (180 == ctz) { timeZoneHiddenField = 'Montevideo Standard Time'; }
    else if (180 == ctz) { timeZoneHiddenField = 'E. South America Standard Time'; }
    else if (150 == ctz) { timeZoneHiddenField = 'Mid-Atlantic Standard Time'; }
    else if (120 == ctz) { timeZoneHiddenField = 'SA Eastern Standard Time'; }
    else if (60 == ctz) { timeZoneHiddenField = 'Cape Verde Standard Time'; }
    else if (0 == ctz) { timeZoneHiddenField = 'Azores Daylight Time'; }
    else if (0 == ctz) { timeZoneHiddenField = 'Morocco Standard Time'; }
    else if (-60 == ctz) { timeZoneHiddenField = 'GMT Standard Time'; }
    else if (-60 == ctz) { timeZoneHiddenField = 'Namibia Standard Time'; }
    else if (-120 == ctz) { timeZoneHiddenField = 'Central European Standard Time'; }
    else if (-120 == ctz) { timeZoneHiddenField = 'South Africa Standard Time'; }
    else if (-180 == ctz) { timeZoneHiddenField = 'GTB Standard Time'; }
    else if (-180 == ctz) { timeZoneHiddenField = 'E. Africa Standard Time'; }
    else if (-240 == ctz) { timeZoneHiddenField = 'Russian Standard Time'; }
    else if (-270 == ctz) { timeZoneHiddenField = 'Afghanistan Standard Time'; }
    else if (-300 == ctz) { timeZoneHiddenField = 'Pakistan Standard Time'; }
    else if (-330 == ctz) { timeZoneHiddenField = 'India Standard Time'; }
    else if (-345 == ctz) { timeZoneHiddenField = 'Nepal Standard Time'; }
    else if (-360 == ctz) { timeZoneHiddenField = 'Central Asia Standard Time'; }
    else if (-390 == ctz) { timeZoneHiddenField = 'Myanmar Standard Time'; }
    else if (-420 == ctz) { timeZoneHiddenField = 'SE Asia Standard Time'; }
    else if (-480 == ctz) { timeZoneHiddenField = 'North Asia East Standard Time'; }
    else if (-540 == ctz) { timeZoneHiddenField = 'Tokyo Standard Time'; }
    else if (-570 == ctz) { timeZoneHiddenField = 'Cen. Australia Standard Time'; }
    else if (-600 == ctz) { timeZoneHiddenField = 'E. Australia Standard Time'; }
    else if (-630 == ctz) { timeZoneHiddenField = 'Tasmania Standard Time'; }
    else if (-660 == ctz) { timeZoneHiddenField = 'West Pacific Standard Time'; }
    else if (-690 == ctz) { timeZoneHiddenField = 'Central Pacific Standard Time'; }
    else if (-720 == ctz) { timeZoneHiddenField = 'New Zealand Standard Time'; }
    return timeZoneHiddenField;
}
var DoUserLogin = function () {
    console.log("doUserLogin");
    var url = domainurl + "/Login/LoginAjax/";
        var param = JSON.stringify({
            UserName: $("#UserName").val(),
            Password: $("#Password").val(),
            Remember: $('#Remember').prop('checked'),
            Currentdate: Currentdate,
            Currenttime: Currenttime,
            Currentzone: ctz,
            currenttimezone: getTimezoneName()
        });
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $(".loader-div").hide();
                if (data.data != "none") {
                    var urltemp = window.location.href.split("ReturnUrl=");
                    var redirecturl = "";
                    if (urltemp.length > 1) {
                        redirecturl = decodeURIComponent(urltemp[1]);
                    }
                    if (window.location.pathname == "/" || window.location.pathname.toLowerCase().indexOf("/login") > -1) {
                        if (typeof (redirecturl) != "undefined" && redirecturl != null && redirecturl != "") {
                            window.location.href = domainurl + redirecturl;
                        }
                        else {
                            window.location.href = domainurl + "/DashBoard";
                        }
                        
                    }
                    else if (typeof (CloseFullScreenLoginModal) != "undefined" && typeof (SessionIsActive) != "undefined") {
                        if (CurrentUserUserId == data.UserId) {
                            SessionIsActive = true;
                            CloseFullScreenLoginModal();
                        } else {
                            if (typeof (redirecturl) != "undefined" && redirecturl != null && redirecturl != "") {
                                window.location.href = domainurl + redirecturl;
                            }
                            else {
                                window.location.href = domainurl + "/DashBoard";
                            }
                            
                        }
                         
                    } else {
                        if (typeof (redirecturl) != "undefined" && redirecturl != null && redirecturl != "") {
                            window.location.href = domainurl + redirecturl;
                        }
                        else {
                            window.location.href = domainurl + "/DashBoard";
                        }
                    }
                } else {
                    if (typeof (OpenErrorMessageNew) != "undefined") {
                        OpenErrorMessageNew("", "Login failed.");
                    } else {
                        $("#LoginFailed").click();
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
                if (typeof (OpenErrorMessageNew) != "undefined") {
                    OpenErrorMessageNew("","Can't connect to the server. Please try again with an active internet connection.");
                }
            }
        });
    }
$(document).ready(function () {
    
    
    $("#BtnLogin").click(function () {
        if (CommonUiValidation()) {
            if ($('#Remember').prop('checked') == true)
            {
                var Cookie = $("#UserName").val() + "||" + $("#Password").val();
                var encryptedAES = CryptoJS.AES.encrypt(Cookie, "My Secret Passphrase"); 
                $.cookie("_UserInfo_", encryptedAES.toString(), { expires: 7, path: '/', domain: location.hostname });
            }
            else if ($('#Remember').prop('checked') == false)
            {
                console.log("test");
                var Cookie = "" + "||" + "";
                var encryptedAES = CryptoJS.AES.encrypt(Cookie, "My Secret Passphrase");
                $.cookie("_UserInfo_", encryptedAES.toString(), { expires: 7, path: '/', domain: location.hostname });
            }
            setTimeout(function () {
                DoUserLogin();
            },50);
        }
    });
    $('#UserName,#Password').keypress(function (e) {
        if (e.which == 13) {
            DoUserLogin();
        }
    });

    //$("input").attr("autocomplete", "off")

    //$('#Password').attr("autocomplete", "off");
    //setTimeout('$("#Password").val("");', 2000);

    //$("input#Password").attr("autocomplete", "off");

    //$(function () {
    //    var passElem = $("input#Password");
    //    passElem.focus(function () {
    //        passElem.prop("type", "password");
    //    });
    //});
});

$(function () {
    $("form input").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('button[type=submit] .default').click();
            return false;
        } else {
            return true;
        }
    });
});
