$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $(".CalendarSettings").load(domainurl + "/Calendar/CalendarSettingsPartial/");
});
var OpenPtoTab = function () {
    $(".LoadDayOff").load(domainurl + "/Calendar/PTOPartial/");
}