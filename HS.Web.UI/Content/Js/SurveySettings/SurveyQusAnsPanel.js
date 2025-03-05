//var AddNewQuestion = function (item) {
//    console.log('call');
//    OpenRightToLeftModal("/Survey/AddQuestion?SurveyId=" + item)
//}
var UiValidationForQuestion = function () {
    var result = true;
    var inputlist = $("input");
    var textarealist = $("textarea");
    var selectList = $("select");
  
  
    for (var i = 0; i < textarealist.length; i++) {

        if (($(textarealist[i]).attr('datarequired') == 'true' || $(textarealist[i]).attr('daterequired') == 'true') && $(textarealist[i]).is(":visible")) {
            var check = Validation.ForTextbox($(textarealist[i]));
            if (!check)
                result = check;
        }
    }
    return result;
}
var SearchQuestions = function () {
    console.log("sdf");
    var searchText = $(".searchtext").val();
    $("#QuestionList").load(domainurl + "/Survey/ShowQusestions", { SurveyId: SurveyId, SearchText: searchText });
}
var SaveQuestion = function () {
    console.log("hlw");
    if (UiValidationForQuestion()) {
         SaveSurveyQuestion();
    }
    else {

    }
}
var SetQuestion = function (item) {
    console.log(item);
    OpenTopToBottomModal(domainurl + "/Survey/ShowQusAnsPanel?SurveyId=" + item)
   
}
var SaveSurveyQuestion = function () {
    var customQues = {};
    customQues.Question = $("#Question").val();
    customQues.QuestionType = $("#QuestionType").val();
    customQues.SurveyId = $("#SurveyId").val();
    customQues.Id = $("#Id").val();
    customQues.QuestionId = $("#QuestionId").val();
    $.ajax({
        type: "POST",
        url: domainurl + "/Survey/AddQuestion",
        data: '{customQues: ' + JSON.stringify(customQues) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.result) {

                OpenSuccessMessageNew("Success!", response.message, function () {
                    $("#Question").val('');
                    $("#QuestionType").val("-1");
                    $("#QuestionId").val("00000000-0000-0000-0000-000000000000");
                    $("#btn-SetQuestions").hide();
                    $(".close").click();
                    $("#QuestionList").load(domainurl + "/Survey/ShowQusestions?SurveyId=" + SurveyId);
                });
                $(".AnsEditModule").hide();
            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
            //window.location.reload();
        }
    });
}
var SelectedDataKey = "";

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide(); 
    $("#AddNewQues").load(domainurl + "/Survey/AddQuestion?SurveyId=" + SurveyId);
    $("#QuestionList").load(domainurl + "/Survey/ShowQusestions?SurveyId=" + SurveyId);
    $('.searchtext').keyup(function () {

        SearchQuestions();
    })

});
