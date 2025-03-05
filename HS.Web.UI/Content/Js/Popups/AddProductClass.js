var SaveProductClass = function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "Inventory/AddProductClass/",
                data: {
                    Id: $("#Id").val(),
                    Name: $("#Name").val(), 
                },
                success: function () {
                    //$('.inventory-popup').dialog('close');

                }
            });
        $(".close").trigger("click");
        LoadProductClass(true);
    }
}
$(document).ready(function () {
    $('#Name').keypress(function (e) {
        if (e.which == 13) {
            SaveProductClass();
        }
    });

    $("#SaveProductClass").click(function () {
        SaveProductClass();
    });

});
