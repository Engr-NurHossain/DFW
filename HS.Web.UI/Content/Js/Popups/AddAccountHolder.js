
var SaveAccHolder = function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "AccountHolder/AddAccountHolder/",
                data: {
                    Id: $("#Id").val(),
                    Name: $("#accName").val(),
                    InHouse: $("#InHouse").prop("checked")
                },
                success: function () {
                    //$('.inventory-popup').dialog('close');

                }
            });
        $(".close").trigger("click");
        LoadAccountHolder(true);
    }
}
$(document).ready(function () {
    $('#accName').keypress(function (e) {
        if (e.which == 13) {
            SaveAccHolder();
        }
    });

    $("#SaveAccHolder").click(function () {
        SaveAccHolder();
    });
    //var value = $("#InHouse").prop("checked");
   
    //if (value == true) {
    //    $("#InHouse").prop('checked', true);
    //}
    //else {
    //    $("#InHouse").prop('checked', false);
    //}


});