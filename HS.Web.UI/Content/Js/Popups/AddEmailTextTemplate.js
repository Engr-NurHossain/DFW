var SaveTextTemplate = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Leads/LeadEmailTextTemplatePartial";
        
        var param = {
            Id: $("#idval").val(),
            TextContent: $("#TxtContent").val()
        };
        
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: JSON.stringify( param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $('.close').trigger('click');
                setTimeout(function () {
                    LoadLeadList(true);
                }, 600);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    }
}
$(document).ready(function () {
    
    $("#SaveTextEmail").click(function () {
        SaveTextTemplate();
    })
    
});