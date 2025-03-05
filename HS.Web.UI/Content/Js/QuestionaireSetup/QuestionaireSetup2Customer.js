var SaveQuestionaire2 = function () {

    var ListAnswer2 = [];
    var url = domainurl + "/Customer/AddNewQuestionaire2";
    //var questionId = $(this).attr('idval-qustionId');
    var q1ans = $('.test-ans');
    for (var qcount = 0; qcount < q1ans.length; qcount++) {
        var answer = $(q1ans[qcount]).is(':checked');
        var quesID = $(q1ans[qcount]).attr('idval-qustionId');
        ListAnswer2.push({
            SelectedQuesid: parseInt(quesID),
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
                QaAnswersList: ListAnswer2,
                LeadId1: IDLEADVal
            },
            success: function (data) {
                if (data.LeadQuestionaireId) {
                    var value = data.LeadQuestionaireId;
                    console.log(value);
                    OpenSuccessMessageNew("Success!", "Successfully Added QA2 Answer", LoadCustomerDetail(value, true));
                    CloseTopToBottomModal(true);
                }
            }
        });
}

$(document).ready(function () {

    $("#SaveBtnQues2").click(function () {
        SaveQuestionaire2();
    });
});