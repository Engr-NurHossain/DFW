var SavePanelType = function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "Panel/AddPanelType/",
                data: {
                    Id: $("#Id").val(),
                    Name: $("#Name").val(),
                    Value: itemval
                },
                success: function () {
                    //$('.inventory-popup').dialog('close');

                }
            });
        $(".close").trigger("click");
        LoadPanelType(true);
    }
}
$(document).ready(function () {
    $('#Name').keypress(function (e) {
        if (e.which == 13) {
            SavePanelType();
        }
    });

    $("#SavePanel").click(function () {
        SavePanelType();
    });
    //$("#Name").keyup(function () {
    //    $('#Value').val($(this).val());
    //})
});