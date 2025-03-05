var saveInstallation = function (CId) {
    var url = domainurl + "/WorkOrder/CompleteTaskAfterInstallation/";
    var param = JSON.stringify({
        customerId: CId,
        zone1: $("#Zone1").val(),
        zone2: $("#Zone2").val(),
        zone3: $("#Zone3").val(),
        zone4: $("#Zone4").val(),
        zone5: $("#Zone5").val(),
        zone6: $("#Zone6").val(),
        zone7: $("#Zone7").val(),
        zone8: $("#Zone8").val(),
        zone9: $("#Zone9").val(),
        installationdate: WorkDatepicker.getDate(),
        QA1: $("#QA1").val(),
        QA1picker: QA1picker.getDate(),
        QA2: $("#QA2").val(),
        QA2picker: QA2picker.getDate(),
        WorkOrderEmployeeId: $("#WorkOrderEmployeeId").val()
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data == true) {

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {

    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".btn-setup-sys-info").click(function () {
        var CustomerId = $(this).attr('idval');
        saveInstallation(CustomerId);
        CloseTopToBottomModal();
    });
    $("#Cancel").click(function () {

    });
});






