var GetTimeFormat = function (date) {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return new Date(date + ' ' + time)
}
var Saveservice = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/ServiceOrder/AddServiceOrder";

        var param1 = {
            Id: $("#Id").val(),
            CustomerId: $("#CustomerIdVal").val(),
            AppointmentDate: $("#AppointmentDate").val(),
            AppointmentStartTime: $("#AppointmentStartTime").val(),
            AppointmentEndTime: $("#AppointmentEndTime").val(),
            CustomerList: $("#CustomerList").val(),
            CompanyId: $("#CompanyIdVal").val(),
            EmployeeId: $("#EmployeeId").val(),
            Notes: $("#Notes").val(),
        };
        var param2 = {
            ListHelperTech: $("#ListHelperTech").val()
        };
        var param = JSON.stringify({
            'ca': param1, 'cat': param2
        })

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result == true) {
                    $('.close').trigger('click');
                    setTimeout(function () {
                        OpenServiceOrderTab();
                        var CustomerIdForProductInstallation = data.CustomerId;
                        var AppointmentIdForProductInstallation = data.AppointmentId;
                        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + AppointmentIdForProductInstallation + "&CustomerId=" + CustomerIdForProductInstallation);
                        CreateServiceOrderMail(AppointmentIdForProductInstallation);
                    }, 600);
                }
                else {
                    OpenErrorMessageNew("Error!", data.message1, "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}


var CreateServiceOrderMail = function (AppId) {
    var AppointmentId = AppId;
    //var employeeId = $("#EmployeeId").val();
    //var customerName = $(".SaveService").attr("idval1");
    $.ajax({
        url: domainurl + "/ServiceOrder/ServiceOrderCreateMail/",
        data: { AppointmentId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        /*OpenConfirmationMessage("Success", "Service order create successfully.");*/
    });
}


$(document).ready(function () {

    $("#SaveService").click(function () {
        if ($("#AppointmentDate").val() != "" && $("#EmployeeId").val() != "" && $("#EmployeeId").val() != "-1") {
            var v = $("#AppointmentStartTime").val().split(":");
            var m = v[1].split("");
            var m = v[1].split(" ");
            var a = parseInt(v[0] + m[0]);

            var x = $("#AppointmentEndTime").val().split(":");
            var y = x[1].split("");
            var y = x[1].split(" ");
            var b = parseInt(x[0] + y[0]);
            if (m[1] == y[1]) {
                if (a > b || a == b) {
                    OpenErrorMessageNew("Error!", "Start Time greater than End Time ", "");
                }
                else {
                    Saveservice();
                }

            }

            if (m[1] != y[1]) {
                if (a < b || a == b) {
                    OpenErrorMessageNew("Error!", "Start Time greater than End Time ", "");
                }
                else {
                    Saveservice();
                }
            }
        }
        else {
            OpenErrorMessageNew("Error!", "Appointment Date and Installer field couldn't be empty", "");
        }
    })
});