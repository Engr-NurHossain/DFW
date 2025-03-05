


var EditAnnounceMent = function (Id) {
    OpenRightToLeftModal(domainurl + '/Customer/AddAnnouncement/?Id=' + Id)
}
var DeleteAnnouncementConfirm = function(Id)
{
    OpenConfirmationMessageNew("","Do you want to delete this announcement?",function(){
      
        DeleteAnnouncement(Id);
    })
}
var DeleteAnnouncement = function (Id) {

    var url =domainurl +  "/Customer/DeleteAnnouncement";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify({
            Id: Id,

        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    $("#Announcement_List").load(domainurl + "/Customer/ShowAnnouncementList/");
                });

            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var LoadAnnouncementList = function (order) {
    if (typeof (order) == "undefined") {
        order = "";
    }
    $("#Announcement_List").html(LoaderDom);
    $("#Announcement_List").load(domainurl + "/Customer/ShowAnnouncementList/");
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    LoadAnnouncementList();
});