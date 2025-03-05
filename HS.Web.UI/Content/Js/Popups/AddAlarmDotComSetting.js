var SaveAlarm = function () {
       
            $.ajax(
                {
                    type: "POST",
                    url: "Setup/AddAlarmDotComSetting/",
                    data: {
                        Id: $("#Id").val(),
                        Name: $("#Name").val(),
                        Value: $("#Value").val(),
                    },
                    success: function () {
                        //$('.inventory-popup').dialog('close');
                        $(".close").trigger("click");
                        LoadSettings(true);
                    }
                });

}
$(document).ready(function () {
    $("#SaveAlarm").click(function () {
        if (CommonUiValidation()) {
            SaveAlarm();
        }
    });
    //$('#Name').keyup(function () {
    //    $('#Value').val($(this).val());
    //});
})