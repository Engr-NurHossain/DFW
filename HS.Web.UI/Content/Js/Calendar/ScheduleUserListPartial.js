$(document).ready(function () {
    $('.selectpicker_user').on('change', function () {
        if (isPermission.toLowerCase() == 'true') {
            var selected = $(this).val(),
                strUserList = String(selected);
            var IsValue = $("#IsAllTechnicianList").val();
            var typeval = $("#ListTicketType").val();
            if (IsValue == 'all') {
                $(".LoadSchedule").load(domainurl + "/Calendar/SchedulePartial?CurrentDate=" + selectrdDate + "&CurrentView=" + defview + "&UserVal=&typeval=" + typeval + "&status=" + IsValue);
            }
            else if (IsValue == 'none') {
                $(".LoadSchedule").load(domainurl + "/Calendar/SchedulePartial?CurrentDate=" + selectrdDate + "&CurrentView=" + defview + "&UserVal=&typeval=" + typeval + "&status=" + IsValue);
            }
            else {
                var splitFristResult = strUserList.split(',')[0];
                if (splitFristResult == 'all' || splitFristResult == 'none') {
                    var arr = selected.shift();
                }
                if (selected.length == empCount) {
                    $(".LoadSchedule").load(domainurl + "/Calendar/SchedulePartial?CurrentDate=" + selectrdDate + "&CurrentView=" + defview + "&UserVal=&typeval=" + typeval + "&status=all");
                    $("#IsAllTechnicianList").val("all");
                }
                else {
                    $(".LoadSchedule").load(domainurl + "/Calendar/SchedulePartial?CurrentDate=" + selectrdDate + "&CurrentView=" + defview + "&UserVal=" + selected + "&typeval=" + typeval + "&status=" + IsValue);
                }
            }
        }
    });
    $(".selectpicker_user").selectpicker('val', UserVal);
    $(".selectpicker_type").selectpicker('val', typeval);
});