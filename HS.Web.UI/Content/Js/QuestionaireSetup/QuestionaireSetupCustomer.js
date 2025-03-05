
var SaveQuestionaire = function () {

    var ListAnswer = [];
    var url = domainurl + "/Customer/AddNewQuestionaire";
    //var questionId = $(this).attr('idval-qustionId');
    var q1ans = $('.test-ans');
    for (var qcount = 0; qcount < q1ans.length; qcount++) {
        var answer = $(q1ans[qcount]).is(':checked');
        var quesID = $(q1ans[qcount]).attr('idval-qustionId');
        ListAnswer.push({
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
                QaAnswersList: ListAnswer,
                LeadId: IDLEAD,
                TechDate: $("#TechnicianDate").val(),
                TechId: $("#TechnicianList").val()
            },
            success: function (data) {
                if (data.result == true && $("#TechnicianList").val() != "" && data.Techdateid != "") {
                    if (data.LeadQuestionaireId) {
                        var value = data.LeadQuestionaireId;
                        console.log(value);
                        OpenSuccessMessageNew("Success!", "Successfully Added QA1 Answer", LoadCustomerDetail(value, true));
                        CloseTopToBottomModal(true);
                        //$(".QuesBtn").hide();
                    }
                    $("#SaveBtnQues").hide();
                }
                else {
                    OpenErrorMessageNew("Error!", "Please Select Technician And Work order Appointment Date!");
                }
            }
        });
}

$(document).ready(function () {

    $("#SaveBtnQues").click(function () {
        SaveQuestionaire();
    });
});