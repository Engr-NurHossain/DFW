var AddRescheduleTicket = function () {
    var url = "/Ticket/AddRescheduleTicket/";
    var AdditionalMemberList = [];
    $(".HasMember").each(function () {
        var isReschedulePay = false;
        if ($(this).find(".MemberReschedulePay").prop("checked")) {
            isReschedulePay = true;
        }
        AdditionalMemberList.push({
            UserId: $(this).attr('data-id'),
            IsReschedulePay: isReschedulePay,

        });
    });
    var param = JSON.stringify({
        Reason: $("#Reasontext").val(),
        TicketId: TicketId,
        CompletionDate: CompletionDate,
        CustomerId: CustomerId,
        AdditionalMemberList: AdditionalMemberList
        //IsPay: $('#IsPay').prop("checked")
    });

    console.log(param); 

    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                setTimeout(function () {
                    parent.OpenSuccessMessageNew("Success", "", function () {
                        //parent.OpenTicketTab();
                        parent.OpenTicketById(data.newTicketId);
                        parent.ClosePopup();
                    })
                }, 600);
            }
            else {
                parent.OpenErrorMessageNew("Error", "No additional members selected for added commission");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

$(document).ready(function () {
    $(".resch_ticket_container").height(window.innerHeight - 132);
    $(".different_address_pop_container").height(window.innerHeight - 142);
    $(".resch_mem_ticket_container").height(window.innerHeight - 141);
    $("#SaveReschedule").click(function ()
    {
        AddRescheduleTicket();
    })

});
$(window).resize(function () {
    $(".resch_ticket_container").height(window.innerHeight - 132);
    $(".different_address_pop_container").height(window.innerHeight - 142);
    $(".resch_mem_ticket_container").height(window.innerHeight - 141);
})
