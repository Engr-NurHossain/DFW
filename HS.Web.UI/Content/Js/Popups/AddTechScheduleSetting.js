var SaveTech = function () {
    if (CommonUiValidation()) {
        if (CommonUiValidation()) {
            $.ajax(
                {
                    type: "POST",
                    url: "TechScheduleSetting/AddTechScheduleSetting/",
                    data: {
                        Id: $("#Id").val(),
                        
                    },
                    success: function () {
                        //$('.inventory-popup').dialog('close');
                        $(".close").trigger("click");
                        LoadTechSetting(true);
                    }
                });

        }
    }
}
$(document).ready(function () {
    $("#SaveTech").click(function () {
        SaveTech();
    })
})