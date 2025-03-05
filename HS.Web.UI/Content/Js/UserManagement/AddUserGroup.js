var SavePermissionGroup = function () {
    var TagString = "";
 
    $(".TypeCheckBox:checked").each(function () {

        TagString += $(this).attr("value") + ",";
    })
    TagString = TagString.slice(0, -1);
    $.ajax(
        {
            type: "POST",
            url: "UserMgmt/AddUserGroup/",
            data: {
                Id: $("#Id").val(), 
                Name: $("#Name").val(),
                Tag: TagString

            },
            success: function () {
                //$('.inventory-popup').dialog('close');
                $(".close").trigger("click");
                LoadUserGroup();
            }
        });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    //$('#actName').keypress(function (e) {
    //    if (e.which == 13) {
    //        SaveActivationfee();
    //    }
    //});
    var Tagarray = $("#UserTag").val().split(',');
    for (var i = 0; i < Tagarray.length; ++i) {
        // do something with `substr[i]`
     
        $("#Is_" + Tagarray[i]).prop('checked', true);

    }
    $("#SavePermissionGroup").click(function () {
        if (CommonUiValidation()) {
             
            SavePermissionGroup();
             
        }
    });
});