var SaveProductCategory = function () {
    if (CommonUiValidation()) { 
        
        var url = domainurl + "/Customer/AddProductCategory/";
        var ParentId = $("#ParentId").val();
        if (!$('#cbx').is(':checked')) {
            ParentId=null;
        }
        var param = JSON.stringify({
            Id: $("#Id").val(),
            Name: $("#Name").val(),
            ParentId: ParentId
        });

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
         
        $(".close").trigger("click");
        LoadProductCategory(true);
    }
}
$(document).ready(function () { 
    $('#Name').keypress(function (e) {
        if (e.which == 13) {
            SaveProductCategory();
        }
        
    });

    $("#SaveProductCategory").click(function () { 
        SaveProductCategory();
        
    });
     
    $("#ParentId").hide();
    
    $('#cbx').change(function () {
        if ($('#cbx').is(':checked'))
            $("#ParentId").fadeIn();
        else
            $('#ParentId').fadeOut();
    });
    
        
});
