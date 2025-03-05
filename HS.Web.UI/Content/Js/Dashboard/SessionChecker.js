var SessionIsActive = true;
var SessionCheckerTimeOut = (1000 * 60);
var SessionChecker = function () { 
    setTimeout(function () {
        SessionChecker();
    }, SessionCheckerTimeOut);
    if (SessionIsActive) {
        $.ajax({
            type: "POST",
            url: domainurl + "/App/SessionChecker",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result == false) {
                    SessionIsActive = false;
                    /*OpenErrorMessageNew("Error!", data.message, function () {
                        location.href = domainurl + "/login";
                    });*/
                    OpenFullScreenLoginModal()
                } else if (data.result == true) {
                    SessionIsActive = true;
                    $(".notification_counter").text(data.notificationCount);
                    if (data.notificationCount > 0) {
                        $(".notification_counter").removeClass("hidden");
                    } else {
                        $(".notification_counter").addClass("hidden");
                    }

                    if (data.IsClockedIn) {
                        $(".ClockInOutIconLi").removeClass("clock_out_color");
                        $(".ClockInOutIconLi").addClass("clock_in_color");
                    } else {
                        $(".ClockInOutIconLi").removeClass("clock_in_color");
                        $(".ClockInOutIconLi").addClass("clock_out_color");
                    }


                    if (data.AnnouncementCount > 0) {
                        if (typeof $.cookie("_AnnouncementOldCount") != 'undefined' && $.cookie("_AnnouncementOldCount") != null) {
                            var AnnounceOldCount = $.cookie("_AnnouncementOldCount");
                            if (parseInt(data.AnnouncementCount) > parseInt(AnnounceOldCount)) {
                                ShowAnnouncement();
                            }
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                SessionIsActive = false;
                /*OpenErrorMessageNew("Error!", "Your session has been timed out. Please login.", function () {
                    location.href = domainurl + "/login";
                });*/
                OpenFullScreenLoginModal();
            }
        });
    }
}
SessionChecker();