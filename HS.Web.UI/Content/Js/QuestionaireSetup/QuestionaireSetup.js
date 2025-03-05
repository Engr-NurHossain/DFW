
var SaveQuestionaire = function () {

    var ListAnswer = [];
    var url = domainurl + "/Leads/AddNewQuestionaire";
            //var questionId = $(this).attr('idval-qustionId');
            var q1ans =  $('.test-ans');
            for(var qcount = 0; qcount < q1ans.length; qcount++) {
                var answer = $(q1ans[qcount]).is(':checked');
                var quesID = $(q1ans[qcount]).attr('idval-qustionId');
                ListAnswer.push({
                    SelectedQuesid: parseInt( quesID),
                    SelectedAnswer: answer.toString()
                });
                console.log("count = " + qcount);
            }
        $.ajax(
            {
                type: "POST",
                url: url,
                data: {
                    LeadCustomerId: $("#cusID").val(),
                    QaAnswersList: ListAnswer,
                    LeadId: IDLEAD
                },
                success: function (data) {
                    if (data.LeadQuestionaireId) {
                        var value = data.LeadQuestionaireId;
                        console.log(value);
                        OpenSuccessMessageNew("Success!", "Successfully Added QA1 Answer", LoadLeadsDetail(value, true));
                        CloseTopToBottomModal(true);
                    }
                }
            });
}

$(document).ready(function () {

    $("#SaveBtnQues").click(function () {
        SaveQuestionaire();
    });
});