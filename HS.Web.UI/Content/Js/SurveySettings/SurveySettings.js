var AddNewSurvey = function () {
    //$("#NewNotes").load("/Notes/AddNotes");
    
    OpenRightToLeftModal(domainurl + "/Survey/AddSurvey");
    //$("#NewNotes").load();
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#ShowSurveyList").load(domainurl + "/Survey/ShowSurveyList");
})