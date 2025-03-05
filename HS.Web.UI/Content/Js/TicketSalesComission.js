$(document).ready(function () {
    $(".adjustment_sales_comission").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentSalesComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".sales_total_comission_prev_" + id).addClass('hidden');
                    $(".sales_total_comission_" + id).text("$" + data.totalcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    });
    $(".adjustmentpoint_sales_comission").blur(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $(this).val("0");
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentPointSalesComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".sales_total_point_prev_" + id).addClass('hidden');
                    $(".sales_total_point_" + id).text(data.totalpoint);
                    $(".mainadjustablepoint_sales_" + id).text(data.mainadjustablepoint);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    });
    $(".adjustment_tech_comission").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentTechComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".tech_total_comission_prev_" + id).addClass('hidden');
                    $(".tech_total_comission_" + id).text("$" + data.totaltechcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    })
    $(".adjustmentpoint_tech_comission").blur(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $(this).val("0");
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentPointTechComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".tech_total_point_prev_" + id).addClass('hidden');
                    $(".tech_total_point_" + id).text(data.totalpoint);
                    $(".mainadjustablepoint_tech_" + id).text(data.mainadjustablepoint);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    });
    $(".adjustment_additional_comission").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentadditionalComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".additional_total_comission_prev_" + id).addClass('hidden');
                    $(".additional_total_comission_" + id).text("$" + data.totaladdcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    })
    $(".adjustmentpoint_additional_comission").blur(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $(this).val("0");
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentPointAdditionalComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".additional_total_point_prev_" + id).addClass('hidden');
                    $(".additional_total_point_" + id).text(data.totalpoint);
                    $(".mainadjustablepoint_additional_" + id).text(data.mainadjustablepoint);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    });
    $(".adjustment_fin_rep").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentFinRepCommission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".fin_rep_comission_prev_" + id).addClass('hidden');
                    $(".finrep_total_comission_" + id).text("$" + data.totaladdcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    })
    $(".adjustment_additional_comission_followup").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentFollowupComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".additional_total_comission_prev_followup_" + id).addClass('hidden');
                    $(".additional_total_comission_followup_" + id).text("$" + data.totaladdcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    })

    $(".adjustment_additional_comission_reschedule").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentReScheduleComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".additional_total_comission_prev_reschedule_" + id).addClass('hidden');
                    $(".additional_total_comission_reschedule_" + id).text("$" + data.totaladdcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    })

    $(".adjustment_additional_comission_service").change(function () {
        var idval = $(this).attr('data-id');
        var value = $(this).val();
        var id = $(this).attr('data-val');
        $.ajax({
            type: "POST",
            url: "/Ticket/AdjustmentServiceCallComission",
            data: JSON.stringify({ comissionid: idval, val: value }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.res) {
                    $(".additional_total_comission_prev_service_" + id).addClass('hidden');
                    $(".additional_total_comission_service_" + id).text("$" + data.totaladdcomis);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    })
})