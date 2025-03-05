var LoadpackageOptionalList = function () {
    var packageId = $("#packageid").val();
    OpenSuccessMessageNew("Success!", "Package service saved successfully.", function () {
        if (typeof packageId != 'undefined') {
            $(".company-package-services-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageServicesListPartial");
            var InvoiceLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
            $(".TopToBottomModal .ContentsDiv").html(InvoiceLoaderText);
            setTimeout(function () {
                $(".TopToBottomModal .ContentsDiv").load(domainurl + "/SmartPackageSetup/packagesettingslist/" + packageId);
            }, 700);
        }
        else {
            $(".company-package-services-list-div").load(domainurl + "/SmartPackageSetup/CompanyPackageServicesListPartial");
        }
    });
}
var OthersKeyDown = function () {

}
var savePackageOptional = function () {
    var url = domainurl + "/SmartPackageSetup/AddCompanyPackageServices/";

    var ServiceEquipments = [];
    $(".ServiceEqTab .HasItem").each(function () {
        ServiceEquipments.push({
            EquipmentId: $(this).attr('data-id'), 
            Quantity: $(this).find('.txtProductQuantity').val(),
            EquipmentPrice: $(this).find('.txtProductAmount').val(),
            EquipmentName: $(this).find('.ProductName').val(),
            SmartPackageEquipmentServiceId: '00000000-0000-0000-0000-000000000000' 
        });
    });

    var param = JSON.stringify({
        id: $("#Id").val(),
        PackageId: $("#PackageId").val(),
        EquipmentId: $("#EquipmentId").val(),
        Price: $("#Price").val(),
        OriginalPrice: $("#OriginalPrice").val(),
        EptNo: $("#EptNo").val(),
        ServiceEquipments: ServiceEquipments
    });
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data == false) {
                OpenErrorMessageNew("Error!", "Selected Equipment already taken");
            }
            else {
                LoadpackageOptionalList();
                //OpenRightToLeftLgModal();
                OpenRightToLeftModal();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var LoadEquipmentAndService = function (e) {
    var url = domainurl + "/Leads/LoadEquipmentAndService/" + e;
    var equipment = $("#EquipmentId");
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            equipment.empty().append('<option selected="selected" value="-1">Please Select</option>');
            $.each(data, function () {
                equipment.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var PropertyUserSuggestiontemplate =
                '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                   + "<p class='tt-sug-text'>"
                   + "<em class='tt-sug-type'>{5}</em>{1}" + "<br />"
                       + "<em class='tt-eq-price'>${2}</em>"
                       + "<br />"
                   + "</p> "
                + "</div>";
var NewEquipmentRow = "<tr>"
                        + "<td valign='top' class='rowindex'></td>"
                        + "<td valign='top'><input type='text'class='ProductName' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event)' />"
                            + "<div class='tt-menu'>"
                                + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                            + "</div>"
                            + "<span class='spnProductName'></span>"
                        + "</td>"
                        + "<td valign='top'>"
                            + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
                            + "<span class='spnProductQuantity'></span>"
                        + "</td>"
                        + "<td valign='top' class='tableActions'>"
                            + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                        + "</td>"
                    + "</tr>";

var PropertyUserSuggestiontemplate =
               '<div class="tt-suggestion tt-selectable" data-select="{1}" data-price="{2}" data-type="{5}" data-id="{0}" data-description="{6}">'
                  + "<p class='tt-sug-text'>"
                  + "<em class='tt-sug-type'>{5}</em>{1}" + "<br /> <em class='tt_sug_manufac'>{7}</em>"
                      + "<em class='tt-eq-price'>${2}</em>"
                      + "<br />"
                  + "</p> "
               + "</div>";
var NewEquipmentRow = "<tr>"
                    + "<td valign='top' class='rowindex'></td>"
                    + "<td valign='top'><input type='text'class='ProductName' onkeydown='SearchKeyDown(this,event)' onkeyup='SearchKeyUp(this,event)' />"
                        + "<div class='tt-menu'>"
                            + "<div class='tt-dataset tt-dataset-autocomplete'> </div> "
                        + "</div>"
                        + "<span class='spnProductName'></span>"
                    + "</td>"
                    + "<td valign='top' class='hidden'>"
                        + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductQuantity' />"
                        + "<span class='spnProductQuantity'></span>"
                    + "</td>"
                    + "<td valign='top'>"
                      +"<div class='pAmount'>"
                       +"<div class='input-group-prepend'>"
                        +"<div class='input-group-text'>$</div>"
                        +"</div>"
                        + "<input type='text' onkeydown='OthersKeyDown(this,event)' class='txtProductAmount' />"
                        +"</div> "
                        + "<span class='spnProductAmount'></span>"
                    + "</td>"
                    + "<td valign='top' class='tableActions'>"
                        + "<i class='fa fa-trash-o' aria-hidden='true'></i>"
                    + "</td>"
                + "</tr>";

var InvoiceEqSuggestionclickbind = function (item) {
    $('.ServiceEqTab .tt-suggestion').click(function () {
        var clickitem = this;
        $('.ServiceEqTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-select'));
        $(item).attr('data-id', $(clickitem).attr('data-id'));
        var itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());

        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        var spnItemQuantity = $(item).parent().parent().find('.spnProductQuantity');
        $(spnItemQuantity).text(1);
        var txtItemQuantity = $(item).parent().parent().find('.txtProductQuantity');
        $(txtItemQuantity).val(1);

        /*Item Amount Set*/
        var spnItemRate = $(item).parent().parent().find('.spnProductAmount');
        $(spnItemRate).text($(this).attr('data-price')); 
        //var NewProductAmount = parseFloat($(this).attr('data-price').trim().replaceAll(',', ''));
        //$(spnItemRate).text(NewProductAmount.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));

        var txtItemRate = $(item).parent().parent().find('.txtProductAmount');
        $(txtItemRate).val($(this).attr('data-price'));

    });
    $('.ServiceEqTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13 || event.keyCode == 9)
        return false;
    var ExistEquipment = "";
    var ExistEquipmentInner = "";
    $(".HasItem").each(function () {
        ExistEquipmentInner += "'" + $(this).attr('data-id') + "',";
    });
    if (ExistEquipmentInner.length > 0) {
        ExistEquipmentInner = ExistEquipmentInner.slice(0, -1);
        ExistEquipment = "(" + ExistEquipmentInner + ")";
    }
    $.ajax({
        url: domainurl + "/Invoice/GetEquipmentListByKey",
        data: {
            key: $(item).val(),
            ExistEquipment: ExistEquipment
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        /*0*/resultparse[i].EquipmentId,
                        /*1*/ resultparse[i].EquipmentName.replaceAll('"', '\'\''),
                        /*2*/ resultparse[i].RetailPrice,
                        /*3*/resultparse[i].Reorderpoint,
                        /*4*/ resultparse[i].QuantityAvailable,
                        /*5*/ resultparse[i].EquipmentType.replaceAll('"', '\'\''),
                        /*6*/resultparse[i].EquipmentDescription.replaceAll('"', '\'\'')
                        , resultparse[i].ManufacturerName.replaceAll('"', '\'\'')/* ImageSource*/);
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
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {/*Enter*/
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {/*Down*/
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugstionDom).is(':visible')) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
            event.preventDefault();
        }
        else {

            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).next('tr')).addClass('focusedItem');
            if ($(event.target).hasClass('ProductName')) {
                $($(trselected).next('tr')).find('input.ProductName').focus();
            }
        }
    }

    if (event.keyCode == 38) {/*UP*/
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0 && $(ttSugstionDom).is(':visible')) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }
        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).prev('tr')).addClass('focusedItem');
            $($(trselected).prev('tr')).find('input.ProductName').focus();
        }
    }
}

$(document).ready(function () {
    InitializeSuburbDropdown($('.dropdown_equipment'), $("#PackageId").val(), "2");
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#savePackageServices").click(function () {
        if (CommonUiValidation()) {
            savePackageOptional();
        }
    });

    /*focus table row*/
    $(".ServiceEqTab tbody").on('click', 'tr', function (e) {
        console.log(e.target);
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
            || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
            || $(e.target).hasClass("spnProductAmount")) {
            $(".ServiceEqTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        }
        else if ($(e.target).hasClass("pAmount"))
        {
            $(".ServiceEqTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
        else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $(".ServiceEqTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
    });
    /*Add new row*/
    $(".ServiceEqTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        $(".ServiceEqTab tbody tr:last").after(NewEquipmentRow);
        var i = 1;
        $(".ServiceEqTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    /*Remove last row*/
    $(".ServiceEqTab tbody").on('click', 'tr td i.fa', function (e) {
        $(this).parent().parent().remove();
        if ($(".ServiceEqTab tbody tr").length < 2) {
            $(".ServiceEqTab tbody tr:last").after(NewEquipmentRow);
        }
        var i = 1;
        $(".ServiceEqTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".ServiceEqTab tbody").on('change', "tr td .txtProductQuantity", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductQuantity');
        $(ProductQuantityDom).text($(this).val()); 

    });
    $(".ServiceEqTab tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductRateDom = $(this).parent().parent().find('span.spnProductAmount');
        $(ProductRateDom).text($(this).val());

    });

    /* $(".PackageId_select2").select2({})
     $(".EquipmentId_select2").select2({})*/
})