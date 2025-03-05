var SaveLeadTechnician = function () {
    var url = domainurl + "/Leads/AddLeadRequestTechnician";
    var param = JSON.stringify({
        id: $("#id").val(),
        Installer: $("#Installer").val()
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
            if (data) {
                if (data.result == true) {
                    OpenRightToLeftModal();
                    OpenSuccessMessageNew("Success!", "Customer technician setup done successfully.");
                    //OpenTopToBottomModal("/Customer/QA1QuestionariePartial?id=" + LeadId + "&TechId=" + data.TechInstaller);
                }
                if (data.result == false) {
                    OpenRightToLeftModal();
                    OpenErrorMessageNew("Error!", "Error occured.");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    $("#btnSaveLeadTechnician").click(function () {
        if (CommonUiValidation()) {
            SaveLeadTechnician();
        }
    });
   /* $(".InstallerList-select2").select2({});*/
})