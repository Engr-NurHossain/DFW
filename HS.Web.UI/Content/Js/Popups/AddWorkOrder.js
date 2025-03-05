var counter = 0;
var SaveWorkorder = function () {
    if (CommonUiValidation()) {

        var ValidInputs = false;

        var today = new Date();
        var appDate = WorkDatepicker.getDate();
        //if (appDate > today) {
        //    ValidInputs = true;
        //}
        //else {
        //    OpenConfirmationMessage("Error", "Appointment date can not smaller or equeal than today. ");
        //}

        var url = domainurl + "/WorkOrder/AddWorkOrder";
        var param1 = {
            Id: $("#Id").val(),
            CustomerId: $("#CustomerIdVal").val(),
            AppointmentDate: $("#AppointmentDate").val(),
            AppointmentStartTime: $("#AppointmentStartTime").val(),
            AppointmentEndTime: $("#AppointmentEndTime").val(),
            WorkList: $("#WorkList").val(),
            CompanyId: $("#CompanyIdVal").val(),
            EmployeeId: $("#EmployeeId").val(),
            Notes: $("#Notes").val(),
            AppointmentId: $("#AppointmentId").val()
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
                        var cusId = $("#CustomerIdVal").val();
                        $("#WorkOrderTab").load(domainurl + "/WorkOrder/WorkOrderPartial?customerid=" + cusId);
                        var CustomerIdForProductInstallation = data.CustomerId;
                        var AppointmentIdForProductInstallation = data.AppointmentId;
                        OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder/?AppointmentId=" + AppointmentIdForProductInstallation + "&CustomerId=" + CustomerIdForProductInstallation);
                        CreateworkOrder(AppointmentIdForProductInstallation);
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
var CreateworkOrder = function (appId) {
    var AppointmentId = appId;
    $.ajax({
        url: domainurl + "/WorkOrder/WorkOrderCreateMail/",
        data: { AppointmentId },
        type: "Post",
        dataType: "Json"
    }).done(function () {
        setTimeout(function () {
            OpenSuccessMessageNew("Success!", "Work order create successfully.", "");
        }, 200);
        
    });
}

$(document).ready(function () {
    $("#SaveWorkOrder").click(function () {
        counter = counter + 1;
        if (counter == 1) {
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
                        SaveWorkorder();
                    }

                }

                if (m[1] != y[1]) {
                    if (a < b || a == b) {
                        OpenErrorMessageNew("Error!", "Start Time greater than End Time ", "");
                    }
                    else {
                        SaveWorkorder();
                    }
                }
            }
            else {
                OpenErrorMessageNew("Error!", "Appointment Date and Installer field couldn't be empty", "");
            }
        }
        //else {
        //    OpenConfirmationMessage("Warning!", "You have clicked more times, please wait until to load..........!", "");
        //}
    });
});