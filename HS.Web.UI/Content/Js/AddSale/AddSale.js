
var SalesDatepicker;
$(document).ready(function () {
    SalesDatepicker = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#AppointmentDate')[0]
    });
    /*$(".select_search").select2({});*/
});
