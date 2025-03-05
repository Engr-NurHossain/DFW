

var FilterLeadTracking = function (leadId, check ) {
    //var searchtext = $(".PortfolioSearchText").val();
    if (check != 1) {
        var StartDate = $(".StartDate").val();
        var EndDate = $(".EndDate").val();
    }
    else {
        var StartDate = null;
        var EndDate = null;
    }

    $("#loadTrackingList").load("/Leads/LoadLeadTrackingPartial?leadId=" + leadId + "&StartDate=" + StartDate + "&EndDate=" + EndDate);
}






$(document).ready(function () {
    var LeadId = $("#leadId").attr('value');
    
    FilterLeadTracking(LeadId,1);

    

    StartDateDatepicker = new Pikaday({
        field: $('.StartDate')[0],
        format: 'MM/DD/YYYY',
        minDate: new Date(1990, 12, 31),
        maxDate: moment().toDate(),
    });
    EndDateDatepicker = new Pikaday({
        field: $('.EndDate')[0],
        format: 'MM/DD/YYYY',
        minDate: new Date(1990, 12, 31),
        maxDate: moment().toDate(),
    });
    $("#btnsearchtext").click(function () {
        var StartDate = $(".StartDate").val();
        var EndDate = $(".EndDate").val();

        if (StartDate > EndDate) {
            $(".StartDate").addClass("required");
            $(".EndDate").addClass("required");
        }
        else {
            $(".StartDate").removeClass("required");
            $(".EndDate").removeClass("required");
            FilterLeadTracking(LeadId, null);
        }
        
    })
    $(".TrackingSearchText").keypress(function (e) {
        if (e.which == 13) {
            $("#btnsearchtext").click();
        }
    });




});

