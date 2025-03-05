var SaveMMR = function () {
    $.ajax(
        {
            type: "POST",
            url: "Setup/AddMmrSetup/",
            data: {
                Id: $("#Id").val(),
                Name: $("#Name").val(),
                Value: $("#Value").val()
            },
            success: function () {
                //$('.inventory-popup').dialog('close');
                $(".close").trigger("click");
                OpenSuccessMessageNew("Success", "MMR added successfully");
                $(".LoadMMR").load(domainurl + "/Setup/MMRS/");
            }
        });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    //$('#Name').keypress(function (e) {
    //    if (e.which == 13) {
    //        var value = $("#Name").val().replace('$','');
    //        if (!$.isNumeric(value)) {
    //            OpenConfirmationMessage("Warning", "Please Input Valid Number", "");
    //        }
    //        else{
    //            SaveMMR();
    //        }
    //    }
    //});

    $("#SaveMMR").click(function () {

        if (CommonUiValidation()) {
            var value = $("#Value").val();
            if (!$.isNumeric(value)) {
                OpenErrorMessageNew("Error!", "Please Input Valid Number", "");
                OpenRightToLeftModal(close);
            }
            else {
                SaveMMR();
            }
        }
        //else {
        //    SaveMMR();
        //}

        //else {

        //}
    });

    //$('#Name').keyup(function () {
    //    $('#Value').val($(this).val());
    //});
});
