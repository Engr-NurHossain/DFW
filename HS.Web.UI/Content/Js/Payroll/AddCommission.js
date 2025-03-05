var SaveSalesCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveSalesCommission/";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            RMRCommission: $("#RMRCommission").val(),
            Adjustment: $("#Adjustment").val(),
            OriginalPoint: $("#OriginalPoint").val(),
            AdjustablePoint: $("#OriginalPoint").val(),
            TotalPoint: $("#OriginalPoint").val()
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var SaveTechCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveTechCommission/";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            BaseRMRCommission: $("#BaseRMRCommission").val(),
            //AddedRMRCommission: $("#AddedRMRCommission").val(),
            OriginalPoint: $("#OriginalPoint").val(),
            AdjustablePoint: $("#OriginalPoint").val(),
            TotalPoint: $("#OriginalPoint").val()
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var SaveAddAddMemberCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveAddMemberCommission";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            Commission: $("#Commission").val(),
            OriginalPoint: $("#OriginalPoint").val(),
            AdjustablePoint: $("#OriginalPoint").val(),
            TotalPoint: $("#OriginalPoint").val()
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var SaveFinRepCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveFinRepCommission";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            Commission: $("#Commission").val(),
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var SaveServiceCallCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveServiceCallCommission";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            Commission: $("#Commission").val()
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var SaveFollowUpCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveFollowUpCommission";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            Commission: $("#Commission").val()
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}
var SaveRescheduleCommission = function () {
    if (CommonUiValidation()) {
        var url = domainurl + "/Ticket/SaveRescheduleCommission";
        var param = JSON.stringify({
            CustomerId: $("#CustomerId").val(),
            TicketId: $("#TicketId").val(),
            UserId: $("#UserId").val(),
            Commission: $("#Commission").val()
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
                CloseRightToLeftModal();
                if (data == false) {
                    OpenErrorMessageNew("Error!", "");
                }
                else {
                    OpenSuccessMessageNew("Success!", "", function () {
                        ReloadTicket();
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        });
    }
}

$(document).ready(function () {
    $('.commission_inner_height').height(window.innerHeight - 100);
});
$(window).resize(function () {
    $('.commission_inner_height').height(window.innerHeight - 100);
});