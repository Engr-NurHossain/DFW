var PropertyLeadtemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{4}" data-id="{0}" data-description="{3}">'
                   + "<p class='tt-sug-text'>"
                       + "<em class='tt-sug-type'>{4}</em>{1} {5}" + "<br />"
                       + "<em class='tt-eq-price'>{2}</em>"
                       + "<br />"
                   + "</p> "
                + "</div>";
var LeadEquipmentRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td valign='top' class='tdVehicleNo'><input type='text'class='VehicleNo' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event)' />"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnVehicleNo'></span>"
                        + "</td>"
                        //+ "<td valign='top' class='hidden'>"
                        //    + "<input type='text' class='txtDriver' onkeydown='SearchKeyDown(this,event)' onkeyup='DriverSearchKeyUp(this,event)' />"
                        //    + "<div class='tt-menu'>"
                        //        + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                        //    + "</div>"
                        //    + "<span class='spnDriver'></span>"
                        //+ "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtRepairDate' />"
                            + "<span class='spnRepairDate'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtSpent' />"
                            + "<span class='spnSpent'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtTireRotation' />"
                            + "<span class='spnTireRotation'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' class='txtOilChange' />"
                            + "<span class='spnOilChange'></span>"
                        + "</td>"
                         + "<td valign='top'>"
                            + "<input type='text' class='txtNote' />"
                            + "<span class='spnNote'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";

var InvoiceEqSuggestionclickbind = function (item) {
    $('.LeadEquipmentTab .tt-suggestion').click(function () {
        var clickitem = this;
        $('.LeadEquipmentTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
    });
    $('.LeadEquipmentTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var DriverSuggestionclickbind = function (item) {
    $('.LeadEquipmentTab .tt-suggestion').click(function () {
        var clickitem = this;
        $('.LeadEquipmentTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('driver-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('driver-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasDriver');
    });
    $('.LeadEquipmentTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}

var DriverSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/VehicleManagement/GetDriversListByKey",
        data: {
            key: $(item).val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyLeadtemplate,
                        /*0*/resultparse[i].UserId,
                        /*1*/ resultparse[i].EMPName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].PermissionGroupName,
                        /*3*/resultparse[i].UserName.replaceAll('"', '\'\''),
                        /*4*/ resultparse[i].UserName.replaceAll('"', '\'\''),
                        /*5*/ ''
                        );
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();
                DriverSuggestionclickbind(item);

                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    $(".NewProjectSuggestion").perfectScrollbar()
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}

var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/VehicleManagement/GetVehicleListByKey",
        data: {
            key: $(item).val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyLeadtemplate,
                        /*0*/ resultparse[i].VehicleId,
                        /*1*/ resultparse[i].VehicleNo,
                        /*2*/ resultparse[i].Year,
                        /*3*/ resultparse[i].Make,
                        /*4*/ resultparse[i].Model,
                        /*5*/ resultparse[i].DriverName != '' ? "[" + resultparse[i].DriverName + "]" : ''
                        );
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                InvoiceEqSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    $(".NewProjectSuggestion").perfectScrollbar()
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDown = function (item, event) {

    if (event.keyCode == 13) {
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
        if ($('.tt-suggestion').length > 0) {
            if ($('.tt-suggestion.active').length == 0) {
                $($('.tt-suggestion').get(0)).addClass('active');
                $(item).val($($('.tt-suggestion').get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $('.tt-suggestion');
                var activesuggestion = $('.tt-suggestion.active');
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $('.tt-suggestion').removeClass('active');
                    var possibleactive = $('.tt-suggestion').get(indexactive + 1);
                    $($('.tt-suggestion').get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
        }
        event.preventDefault();
    }
    if (event.keyCode == 38) {
        if ($('.tt-suggestion').length > 0 && $('.tt-suggestion.active').length > 0) {
            var suggestionlist = $('.tt-suggestion');
            var activesuggestion = $('.tt-suggestion.active');
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $('.tt-suggestion').removeClass('active');
                var possibleactive = $('.tt-suggestion').get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
        }
        event.preventDefault();
    }
}

var InitRowIndex = function () {
    var i = 1;
    $(".LeadEquipmentTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
    $(".LeadEquipmentTab tbody tr").each(function () {
        if (!$(this).hasClass("dpInit")) {
            new Pikaday({
                field: $(this).find(".txtRepairDate")[0],
                format: 'MM/DD/YYYY'
            });
            new Pikaday({
                field: $(this).find(".txtTireRotation")[0],
                format: 'MM/DD/YYYY'
            });
            new Pikaday({
                field: $(this).find(".txtOilChange")[0],
                format: 'MM/DD/YYYY'
            });
            $(this).addClass("dpInit");
        }
    });
}

var SaveRepairLog = function () {
    var DetailList = [];
    $("#mileagedatatab .HasItem").each(function () {
        if ($(this).find('.txtRepairDate').val() != '' && $(this).find('.txtSpent').val() != '') {
            DetailList.push({
                VehicleId: (VehicleId != '' ? VehicleId : $(this).attr('data-id')),
                Driver: '00000000-0000-0000-0000-000000000000',//$(this).attr('driver-id'),//$(this).attr('txtDriver'),
                StrRepairDate: $(this).find('.txtRepairDate').val(),
                Spent: $(this).find('.txtSpent').val(),
                StrTireRotation: $(this).find('.txtTireRotation').val(),
                StrOilChange: $(this).find('.txtOilChange').val(),
                Note: $(this).find('.txtNote').val()
            });
        }
    });
    if (DetailList.length == 0) {
        OpenErrorMessageNew("", "'Repair Date' and 'Spent' amount is required.");
        return false;
    } else {
        var url = domainurl + "/VehicleManagement/AddRepairLog";
        $.ajax({
            type: "POST",
            ajaxStart: $(".MilageLoader").show(),
            url: url,
            data: JSON.stringify(DetailList),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $(".MilageLoader").hide();
                if (data.result) {
                    OpenSuccessMessageNew("", data.message);
                    CloseTopToBottomModal();
                } else {
                    OpenErrorMessageNew("", data.message)
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //console.log(errorThrown);
                $(".MilageLoader").hide();
            }
        });
    }
}

$(document).ready(function () {
    InitRowIndex();
    $("#mileagedatatab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#mileagedatatab tr").removeClass("focusedItem");
        $(this).addClass("focusedItem");
    });

    $("#mileagedatatab tbody").on('change', "tr td input", function (e) {
        if ($(e.target).hasClass('txtNote')) {
            var ProductQuantityDom = $(e.target).parent().find('span.spnNote');
            $(ProductQuantityDom).text($(e.target).val());
        } else if ($(e.target).hasClass('txtDriver')) {
            var ProductQuantityDom = $(e.target).parent().find('span.spnDriver');
            $(ProductQuantityDom).text($(e.target).val());
        } else if ($(e.target).hasClass('txtRepairDate')) {
            var ProductQuantityDom = $(e.target).parent().find('span.spnRepairDate');
            $(ProductQuantityDom).text($(e.target).val());
        } else if ($(e.target).hasClass('txtSpent')) {
            var ProductQuantityDom = $(e.target).parent().find('span.spnSpent');
            $(ProductQuantityDom).text($(e.target).val());
        } else if ($(e.target).hasClass('txtTireRotation')) {
            var ProductQuantityDom = $(e.target).parent().find('span.spnTireRotation');
            $(ProductQuantityDom).text($(e.target).val());
        } else if ($(e.target).hasClass('txtOilChange')) {
            var ProductQuantityDom = $(e.target).parent().find('span.spnOilChange');
            $(ProductQuantityDom).text($(e.target).val());
        }
    });


    $("#mileagedatatab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $("#mileagedatatab tbody tr:last").after(LeadEquipmentRow);
        if (VehicleId != '') {
            $("#mileagedatatab td.tdVehicleNo").addClass("hidden");
            $("#mileagedatatab tr").attr('data-id', VehicleId);
            $("#mileagedatatab tr").addClass('HasItem');
        }
        //if (DriverId != '') {
        //    $("#mileagedatatab tr").attr('driver-id', DriverId);
        //    $("#mileagedatatab tr").addClass('HasDriver');
        //    $("#mileagedatatab tr:last .txtDriver").val(DriverName);
        //    $("#mileagedatatab tr:last .spnDriver").text(DriverName);
        //}
        InitRowIndex();
    });
    $(".LeadEquipmentTab tbody").on('click', 'tr td i.fa', function (e) {
        $(this).parent().parent().remove();
        if ($(".LeadEquipmentTab tbody tr").length < 2) {
            $("#mileagedatatab tbody tr:last").after(LeadEquipmentRow);
            if (VehicleId != '') {
                $("#mileagedatatab td.tdVehicleNo").addClass("hidden");
                $("#mileagedatatab tr").attr('data-id', VehicleId);
                $("#mileagedatatab tr").addClass('HasItem');
            }
            //if (DriverId != '') {
            //    $("#mileagedatatab tr").attr('driver-id', DriverId);
            //    $("#mileagedatatab tr").addClass('HasDriver');
            //    $("#mileagedatatab tr:last .txtDriver").val(DriverName);
            //    $("#mileagedatatab tr:last .spnDriver").text(DriverName);
            //}
        }
        InitRowIndex();
    });
    $(".LeadEquipmentTab tbody").on('change', "tr td .txtMileageData", function () {
        var MilageDataDom = $(this).parent().find('span.spnMileageData');
        $(MilageDataDom).text($(this).val());
    });
    $("#btnSaveMilageLog").click(function () {
        if ($("#mileagedatatab .HasItem").length == 0) {
            OpenErrorMessageNew("", "You need to add at least one data to continue.");
        } else {
            SaveRepairLog();
        }
    });
});