var DobDatepicker;
var SalesDatepicker;
var InstallDatepicker;
var CutInDatepicker;

$(document).ready(function () {
    if (CreditScoreVal != '') {
        $("#CreditScore").val(CreditScoreVal);
    }
    if (MonthlyMonitoringFeeVal != '') {
        $("#MonthlyMonitoringFee").val(MonthlyMonitoringFeeVal);
    }
    if (NoteVal != '') {
        $("#Note").val(NoteVal);
    }
    DobDatepicker = new Pikaday({ format: 'MM/DD/YYYY', yearRange: [1920, 2010], field: $('#DateofBirth')[0] });
    SalesDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#SalesDate')[0], trigger: $('#SalesDate_custom')[0], firstDay: 1 });
    InstallDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#InstallDate')[0] });
    CutInDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#CutInDate')[0] });
})
